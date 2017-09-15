using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.Runtime.InteropServices;

namespace ConsoleApplication4
{
    class Program
    {
        static void Main(string[] args)
        {
            SpecialKeyController keyController = new SpecialKeyController();
            keyController.SetNumLock(false);  // set the numlock key to off
           
        }

        public class SpecialKeyController
        {
            [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
            private static extern short GetKeyState(int keyCode); // import GetKeyState function from the User32 library, returns 0 if numlock is not pressed and not toggled

            [DllImport("user32.dll", EntryPoint = "keybd_event")]
            private static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo); // import function from library which can simulate different states of a key press

            // Windows constants
            private const byte VK_NUMLOCK = 0x90; 
            private const uint KEYEVENTF_EXTENDEDKEY = 1; 
            private const int KEYEVENTF_KEYUP = 0x2; 
            private const int KEYEVENTF_KEYDOWN = 0x0;

            public bool GetNumLock()
            {
                return (((ushort)GetKeyState(VK_NUMLOCK)) & 0xffff) != 0;  // returns true if the numlock key is toggled, false if not
                                                                     // checks if any of the bits in the return value of GetKeyState are not 0
                                                                     // if all bits are 0, then return false
            }

            public void SetNumLock(bool bState) 
            { 
                if (GetNumLock() != bState) // only simulate a key stroke if the num lock key is off
                { 
                    keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYDOWN, 0); // simulate a press of the num lock key
                    keybd_event(VK_NUMLOCK, 0x45, KEYEVENTF_EXTENDEDKEY | KEYEVENTF_KEYUP, 0); // simulate a release of the num lock key
                } 
            }
        } 
    }
}