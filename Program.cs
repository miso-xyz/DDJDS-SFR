using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace DDJDS_SFR
{
    class Program
    {
        static byte[] settingsSignature;
        static byte[] scoreSignature;
        static byte[] unkSignature;
        static byte[] saveFileBytes;

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

        static void Main(string[] args)
        {
            Console.Title = "Doodle Jump DS Save File Reader";
            Console.WriteLine();
            Console.WriteLine(" Doodle Jump DS Save File Reader - v1.0 by misonothx");
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
            int column = 0;
            int topPos = Console.CursorTop;
            for (int x = 0; x < 6; x++)
            {
                Section section = getSection(x);
                Console.CursorTop = topPos;
                Console.CursorLeft = 20 * column;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine(" Section N°" + (x+1) + ":");
                for (int x_ = 0; x_ < 5; x_++)
                {
                    Console.CursorLeft = 20 * column;
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.Write("   Score N°" + (x_+1) + ": ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(section.Scores[x_].Points);
                    if (x_ == 5) { Console.WriteLine(); }
                }
                Console.CursorLeft = 30 * column;
                if (column == 2) { column = 0; topPos+=7; } else { column++; }
            }
            getSignatures();
            Console.CursorTop -= 13;
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            WriteToConsoleWithCursorOffset("Signatures:",61);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WriteToConsoleWithCursorOffset("  Score:", 61);
            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteToConsoleWithCursorOffset("    " + BitConverter.ToString(scoreSignature), 61);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WriteToConsoleWithCursorOffset("  Settings:", 61);
            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteToConsoleWithCursorOffset("    " + BitConverter.ToString(settingsSignature), 61);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            WriteToConsoleWithCursorOffset("  Unknown:", 61);
            Console.ForegroundColor = ConsoleColor.Cyan;
            WriteToConsoleWithCursorOffset("    " + BitConverter.ToString(unkSignature), 61);
            randomDrawing();
            Console.CursorTop = 20;
        end:
            Console.WriteLine();
            Console.ResetColor();
            Console.Write(" Press any key to exit...");
            Console.ReadKey();
        }

        static void WriteToConsoleWithCursorOffset(string data, int leftOffset)
        {
            Console.CursorLeft = leftOffset;
            Console.WriteLine(data);
        }

        static void WriteToConsoleWithCursorOffset(string data, int leftOffset, int topOffset)
        {
            Console.CursorLeft = leftOffset;
            Console.CursorTop = topOffset;
            Console.WriteLine(data);
        }

        static void randomDrawing()
        {
            Random rng = new Random();
            switch (rng.Next(4,4))
            {
                case 0:
                    WriteToConsoleWithCursorOffset("  ▄█▀▀█▄", 63, 1);
                    WriteToConsoleWithCursorOffset(" ███▀▀███", 63, 2);
                    WriteToConsoleWithCursorOffset(" ███  ███", 63, 3);
                    WriteToConsoleWithCursorOffset("  ▀████▀", 63, 4);
                    break;
                case 4:
                    Console.ForegroundColor = ConsoleColor.Green;
                     WriteToConsoleWithCursorOffset(" ▄▄▄▄▄▄ ", 63,1);
                    Console.CursorTop = 2;
                    Console.CursorLeft = 63;
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
                    WriteToConsoleWithCursorOffset("████████", 63,3);
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    WriteToConsoleWithCursorOffset("════════", 63, 4);
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    WriteToConsoleWithCursorOffset("┘┘┘┘┘┘┘┘", 63,5);
                    break;
            }
        }
    }
}