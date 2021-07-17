using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading;

namespace DDJDS_SFR
{
    class Program
    {
        static byte[] settingsSignature;
        static byte[] scoreSignature;
        static byte[] unkSignature;
        static byte[] saveFileBytes;
        static string saveFilePath;
        static bool minAnim = true;
        public static bool useAnimations = true;

        static bool validateFile()
        {
            BinaryReader SF = new BinaryReader(new MemoryStream(saveFileBytes));
            SF.Read(saveFileBytes, 0, 0x180);
            string recognisableText = new string(SF.ReadChars(15));
            if (recognisableText != "DOODLEJUMPSAVES") { return false; } else { return true; }
        }

        static Section getSection(int SectionIndex)
        {
            BinaryReader SF = new BinaryReader(new MemoryStream(saveFileBytes));
            SF.BaseStream.Position = SectionIndex * 64;
            try
            {
                byte[] sectionData = SF.ReadBytes(64);
                if (Encoding.Default.GetString(sectionData).Replace("\0", null) == "") { throw new Exception(); } // Section has no data
                return new Section(sectionData, SectionIndex);
            }
            catch
            {
                return null;
            }
        }

        static void getSignatures()
        {
            BinaryReader SF = new BinaryReader(new MemoryStream(getSection(7).rawData));
            scoreSignature = SF.ReadBytes(4);
            SF.ReadBytes(4); // Repeated in file (idk why)
            settingsSignature = SF.ReadBytes(4);
            SF.ReadBytes(4); // Repeated in file (idk why)
            unkSignature = SF.ReadBytes(4);
            SF.ReadBytes(4); // Repeated in file (idk why)
        }

        static void printSaveFileType()
        {
            BinaryReader SF = new BinaryReader(new MemoryStream(saveFileBytes));
            SF.BaseStream.Position = SF.BaseStream.Length - 16;
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WriteToConsoleWithCursorOffset("Save File Type:", 62, 21, true);
            Console.ForegroundColor = ConsoleColor.Yellow;
            if (Encoding.Default.GetString(SF.ReadBytes(16)) == "|-DESMUME SAVE-|") { WriteToConsoleWithCursorOffset(".DSV - DeSmuME", 64, 22, true); return; }
            else
            {
                switch (Path.GetExtension(saveFilePath).ToLower())
                {
                    case ".sav":
                        WriteToConsoleWithCursorOffset(".SAV - Default", 64, 22, true);
                        break;
                    case ".dsv":
                        WriteToConsoleWithCursorOffset(".DSV - DeSmuME", 64, 22, true);
                        break;
                    default:
                        WriteToConsoleWithCursorOffset(Path.GetExtension(saveFilePath).ToUpper() + " - Unknown", 64, 22, true);
                        break;
                }
            }
        }

        static void Main(string[] args)
        {
            bool readDone = false;
            Console.Title = "Doodle Jump DS Save File Reader";
            Console.WriteLine();
            Console.WriteLine(" Doodle Jump DS Save File Reader - v1.0.1 by misonothx");
            Console.WriteLine(" |- https://github.com/miso-xyz/DDJDS-SFR");
            Console.WriteLine();
            if (args.Count() == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" No arguments found!");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(" |- Drag & Drop your save file on the application to read it");
                goto end;
            }
            if (args.Contains("--noAnim")) { useAnimations = false; }
            if (args.Contains("--allAnim")) { minAnim = false; }
            saveFilePath = args[0];
            saveFileBytes = File.ReadAllBytes(args[0]);
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine(" Checking if file is a valid Doodle Jump DS Save File...");
            if (!validateFile())
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(" Invalid File!");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine(" |- Coudn't find recognisable string 'DOODLEJUMPSAVES'");
                goto end;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(" Save File is valid!");
                Console.ResetColor();
            }
            Console.WriteLine();
            bool hasMaxScore = false;
            int column = 0;
            int topPos = Console.CursorTop;
            for (int x = 0; x < 6; x++)
            {
                Section section = getSection(x);
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                WriteToConsoleWithCursorOffset(" Section N°" + (x + 1) + ":", 20 * column, topPos, true);
                for (int x_ = 0; x_ < 5; x_++)
                {
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    WriteToConsoleWithCursorOffset("   Score N°" + (x_ + 1) + ": ", 20 * column, false);
                    int points = section.Scores[x_].Points;
                    string pointString = "";
                    bool isMaxScore = false;
                    switch (points.ToString().Length)
                    {
                        case 7:
                            pointString = points.ToString()[0] + "."  + points.ToString()[1] + points.ToString()[2] + points.ToString()[3] + "M";
                            break;
                        case 8:
                            pointString = points.ToString()[0] + points.ToString()[1] + "."  + points.ToString()[2] + points.ToString()[3] + "M";
                            break;
                        case 9:
                            pointString = points.ToString()[0] + points.ToString()[1] + points.ToString()[2] + "." + points.ToString()[3] + "M";
                            break;
                        case 10:
                            if (points == int.MaxValue) { pointString = "Max"; isMaxScore = true; }
                            else {pointString = points.ToString()[0] + "." + points.ToString()[1] + points.ToString()[2] + points.ToString()[3] + "B";}
                            break;
                        default:
                            pointString = points.ToString();
                            break;
                    }
                    if (isMaxScore) { hasMaxScore = true; Console.ForegroundColor = ConsoleColor.Magenta; } else { if (points > 100000) { Console.ForegroundColor = ConsoleColor.Green; } else { Console.ForegroundColor = ConsoleColor.Yellow; } }
                    WriteToConsoleWithCursorOffset(pointString, (20 * column) + ("   Score N°" + (x_ + 1) + ": ").Length, true);
                    if (x_ == 4)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        WriteToConsoleWithCursorOffset("   Markers: ", 20 * column, false);
                        if (section.scoreMarkers) { Console.ForegroundColor = ConsoleColor.Green; } else { Console.ForegroundColor = ConsoleColor.Red; }
                        WriteToConsoleWithCursorOffset(section.scoreMarkers.ToString(), (20 * column) + ("   Markers: ").Length, true);
                        Console.ForegroundColor = ConsoleColor.DarkGray;
                        WriteToConsoleWithCursorOffset("   Sound: ", 20 * column, false);
                        if (section.sound) { Console.ForegroundColor = ConsoleColor.Green; } else { Console.ForegroundColor = ConsoleColor.Red; }
                        WriteToConsoleWithCursorOffset(section.sound.ToString(), (20 * column) + ("   Sound: ").Length, true);
                    }
                }
                if (column == 2) { column = 0; topPos += 9; } else { column++; }
            }
            getSignatures();
            Console.CursorTop -= 12;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            WriteToConsoleWithCursorOffset("Signatures:", 62, true);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WriteToConsoleWithCursorOffset("  Score:", 62, true);
            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteToConsoleWithCursorOffset("    " + BitConverter.ToString(scoreSignature), 62, true);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WriteToConsoleWithCursorOffset("  Settings:", 62, true);
            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteToConsoleWithCursorOffset("    " + BitConverter.ToString(settingsSignature), 62, true);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WriteToConsoleWithCursorOffset("  Unknown:", 62, true);
            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteToConsoleWithCursorOffset("    " + BitConverter.ToString(unkSignature), 62, true);
            randomDrawing();
            readDone = true;
            printSaveFileType();
            if (hasMaxScore)
            {
                Thread maxReachedAnimation = new Thread(new ThreadStart(maxScoreReachedText));
                maxReachedAnimation.IsBackground = true;
                maxReachedAnimation.Start();
                Thread maxReachedDetailAnimation = new Thread(new ThreadStart(maxScoreDetailText));
                maxReachedDetailAnimation.IsBackground = true;
                maxReachedDetailAnimation.Start();
            }
        end:
            Console.WriteLine();
            Console.ResetColor();
            if (readDone) { Thread pauseAnimation = new Thread(new ThreadStart(initBlinkPauseText)); pauseAnimation.IsBackground = true; pauseAnimation.Start(); }
            else { Console.Write(" Press any key to exit..."); }
            Console.ReadKey();
        }

        // too much threads causes UI issues
        static void initBlinkPauseText() { CLIAnimations.blinkingText("Press any key to exit...", ConsoleColor.Black, ConsoleColor.Gray, 500, 25, 5, useAnimations); }
        static void maxScoreReachedText() { CLIAnimations.blinkingText("Max Reached!", ConsoleColor.DarkMagenta, ConsoleColor.Magenta, 250, 63, 8, !minAnim); }
        static void maxScoreDetailText() { CLIAnimations.blinkingText("  (2.147B)  ", ConsoleColor.Magenta, ConsoleColor.DarkMagenta, 250, 63, 9, !minAnim); }

        static void WriteToConsoleWithCursorOffset(string data, int leftOffset, bool splitLine)
        {
            Console.CursorLeft = leftOffset;
            if (splitLine) { Console.WriteLine(data); } else { Console.Write(data); }
        }

        static void WriteToConsoleWithCursorOffset(string data, bool splitLine, int topOffset)
        {
            Console.CursorTop = topOffset;
            if (splitLine) { Console.WriteLine(data); } else { Console.Write(data); }
        }

        static void WriteToConsoleWithCursorOffset(string data, int leftOffset, int topOffset, bool splitLine)
        {
            Console.CursorLeft = leftOffset;
            Console.CursorTop = topOffset;
            if (splitLine) { Console.WriteLine(data); } else { Console.Write(data); }
        }

        static void randomDrawing()
        {
            Random rng = new Random();
            switch (rng.Next(0,4))
            {
                case 0:
                    WriteToConsoleWithCursorOffset("  ▄█▀▀█▄", 64, 1, true);
                    WriteToConsoleWithCursorOffset(" ███▀▀███", 64, 2, true);
                    WriteToConsoleWithCursorOffset(" ███  ███", 64, 3, true);
                    WriteToConsoleWithCursorOffset("  ▀████▀", 64, 4, true);
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Green;
                    WriteToConsoleWithCursorOffset(" ▄▄▄▄▄▄ ", 65, 1, true);
                    Console.CursorTop = 2;
                    Console.CursorLeft = 65;
                    Console.Write("██");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.Write("^");
                    Console.Write("▄▄");
                    Console.ForegroundColor = ConsoleColor.Black;
                    Console.Write("^");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("██");
                    WriteToConsoleWithCursorOffset("████████", 65, 3, true);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    WriteToConsoleWithCursorOffset("════════", 65, 4, true);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    WriteToConsoleWithCursorOffset("┘┘┘┘┘┘┘┘", 65, 5, true);
                    break;
            }
        }
    }
}