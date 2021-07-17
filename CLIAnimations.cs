using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace DDJDS_SFR
{
    class CLIAnimations
    {
        public static void blinkingText(string data, ConsoleColor offColor, ConsoleColor onColor, int msDelay, int leftPos, int topPos, bool animated)
        {
            bool visible = true;
            Console.CursorTop = topPos;
            Console.CursorLeft = leftPos;
            if (!Program.useAnimations || !animated)
            {
                Console.ForegroundColor = onColor;
                Console.Write(data);
                return;
            }
            while (true)
            {
                if (visible) { Console.ForegroundColor = onColor; } else { Console.ForegroundColor = offColor; }
                Console.Write(data);
                Thread.Sleep(msDelay);
                Console.CursorTop = topPos;
                Console.CursorLeft = leftPos;
                visible = !visible;
            }
        }
    }
}
