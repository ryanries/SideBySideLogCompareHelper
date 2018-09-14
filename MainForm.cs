// SideBySideLogCompareHelper
// Joseph Ryan Ries, Sep 14 2018
// ryanries09@gmail.com
//
// This app is designed to assist with the analysis of two (or more) separate log files side by side
// The goal is to essentially mirror keyboard input to each instance of the desired application, so that
// when for example the user hits the down arrow key, the log file scrolls down by one line in each 
// separate log file simultaneously. This works for things like Notepad, Process Monitor, TextAnalysisTool.NET,
// and pretty much any other desktop app. You can also keep more than two instances of an app in sync. For
// example, if you have three copies of Notepad open, pressing the 'a' key will result in the letter a being
// typed in all three instances of Notepad.
//
// The main limitation of this approach is that you cannot type too quickly or things will fall out of sync.
// But it works pretty well if you just want to keep two or more log files on the same line to make them easier
// to compare.

using System;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace LogComparisonHelper
{
    public partial class MainForm : Form
    {
        private const int INPUT_KEYBOARD = 1;

        private const int KEYEVENTF_KEYUP = 2;

        private const int WH_KEYBOARD_LL = 13;

        private const int WM_KEYDOWN = 0x0100;

        private static IntPtr gLLKeyboardHookID = IntPtr.Zero;

        private static LowLevelKeyboardProc gLLKeyboardProc = HookCallback;        

        private static List<Process> gProcessesToSync = new List<Process>();

        private const int SYNTHETIC_KEYPRESS = 1776;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            PopulateProcesses();
        }

        private async void PopulateProcesses()
        {
            ProcessListComboBox.Enabled = false;

            RefreshButton.Enabled = false;

            StartButton.Enabled = false;

            StopButton.Enabled = false;

            ProcessListComboBox.Items.Clear();

            // To be elligible, the process must be:
            // - In the same session as you
            // - Have more than one instance of the process running
            // - Have active windows on the same desktop as you
            // - Does not work on Immersive (UWP) apps            

            await Task.Run(() =>
            {
                Process[] processList = Process.GetProcesses();

                Array.Sort(processList, (s1, s2) => String.Compare(s1.ProcessName, s2.ProcessName));

                int CurrentSessionId = WTSGetActiveConsoleSessionId();

                foreach (Process process in processList)
                {
                    int instanceCount = 0;

                    foreach (Process p in processList)
                        if (p.ProcessName.Equals(process.ProcessName) && (process.SessionId == CurrentSessionId) && (p.SessionId == CurrentSessionId))
                            instanceCount++;

                    if (instanceCount > 1)
                    {
                        if (process.MainWindowHandle != IntPtr.Zero)
                            if (!ProcessListComboBox.Items.Contains(process.ProcessName))
                                Invoke((MethodInvoker)(() => ProcessListComboBox.Items.Add(process.ProcessName)));
                    }
                }              
            });

            if (ProcessListComboBox.Items.Count > 0)
                ProcessListComboBox.SelectedIndex = 0;

            ProcessListComboBox.Enabled = true;

            RefreshButton.Enabled = true;

            StartButton.Enabled = true;            
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            Process[] allInstances = Process.GetProcessesByName(ProcessListComboBox.Items[ProcessListComboBox.SelectedIndex].ToString());

            int CurrentSessionId = WTSGetActiveConsoleSessionId();

            foreach (Process process in allInstances)
            {
                int instanceCount = 0;

                foreach (Process p in allInstances)
                    if (p.ProcessName.Equals(process.ProcessName) && (process.SessionId == CurrentSessionId) && (p.SessionId == CurrentSessionId))
                        instanceCount++;

                if (instanceCount > 1)                
                    if (process.MainWindowHandle != IntPtr.Zero)
                        gProcessesToSync.Add(process);                            
            }

            if (gProcessesToSync.Count < 2)
            {
                MessageBox.Show($"Could not find 2 or more instances of " + ProcessListComboBox.Items[ProcessListComboBox.SelectedIndex].ToString() + " in this session with active windows!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                gProcessesToSync.Clear();

                return;
            }


            gLLKeyboardHookID = SetHook(gLLKeyboardProc);

            if (gLLKeyboardHookID == IntPtr.Zero)
            {
                MessageBox.Show("Failed to install keyboard hook! Win32 Error: " + Marshal.GetLastWin32Error(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                gProcessesToSync.Clear();

                return;
            }

            RefreshButton.Enabled = false;

            ProcessListComboBox.Enabled = false;

            StartButton.Enabled = false;

            StopButton.Enabled = true;            

            if (MinimizeCheckbox.Checked)
                WindowState = FormWindowState.Minimized;
        }

        private void StopButton_Click(object sender, EventArgs e)
        {
            RefreshButton.Enabled = true;

            ProcessListComboBox.Enabled = true;

            StartButton.Enabled = true;

            StopButton.Enabled = false;

            gProcessesToSync.Clear();
           
            UnhookWindowsHookEx(gLLKeyboardHookID);
        }

        private void RefreshButton_Click(object sender, EventArgs e)
        {
            PopulateProcesses();
        }
        
        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0 && wParam == (IntPtr)WM_KEYDOWN)
            {
                bool WrongWindow = true;

                IntPtr ForegroundWindow = GetForegroundWindow();

                foreach (Process process in gProcessesToSync)                
                    if (process.MainWindowHandle == ForegroundWindow)
                        WrongWindow = false;
                
                if (WrongWindow)
                    return CallNextHookEx(gLLKeyboardHookID, nCode, wParam, lParam);

                KBDLLHOOKSTRUCT KBInput = (KBDLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(KBDLLHOOKSTRUCT));

                if (KBInput.dwExtraInfo != (IntPtr)SYNTHETIC_KEYPRESS)
                {
                    foreach (Process process in gProcessesToSync)
                    {
                        if (ForegroundWindow != process.MainWindowHandle)
                        {
                            SetForegroundWindow(process.MainWindowHandle);

                            SendKeyDown((ushort)KBInput.vkCode);

                            SendKeyUp((ushort)KBInput.vkCode);

                            Thread.Sleep(50);
                        }
                    }

                    SetForegroundWindow(ForegroundWindow);
                }
            }

            return CallNextHookEx(gLLKeyboardHookID, nCode, wParam, lParam);
        }

        // I know that I am supposed to use the Windows API functions Get/SetMessageExtraInfo to
        // get and set dwExtraInfo instead of modifying it directly, but I don't care.

        private static void SendKeyDown(ushort key)
        {
            INPUT input = new INPUT
            {
                Type = INPUT_KEYBOARD
            };

            input.Data.Keyboard = new KEYBDINPUT()
            {
                wVk         = key,
                wScan       = 0,
                dwFlags     = 0,
                time        = 0,
                dwExtraInfo = (IntPtr)SYNTHETIC_KEYPRESS
            };

            INPUT[] inputs = new INPUT[] { input };

            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        private static void SendKeyUp(ushort key)
        {
            INPUT input = new INPUT
            {
                Type = INPUT_KEYBOARD
            };

            input.Data.Keyboard = new KEYBDINPUT()
            {
                wVk         = key,
                wScan       = 0,
                dwFlags     = KEYEVENTF_KEYUP,
                time        = 0,
                dwExtraInfo = (IntPtr)SYNTHETIC_KEYPRESS
            };

            INPUT[] inputs = new INPUT[] { input };

            SendInput(1, inputs, Marshal.SizeOf(typeof(INPUT)));
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (Process curProcess = Process.GetCurrentProcess())
            using (ProcessModule curModule = curProcess.MainModule)
                return SetWindowsHookEx(WH_KEYBOARD_LL, proc, GetModuleHandle(curModule.ModuleName), 0);
        }

        [DllImport("Kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.U4)]
        public static extern int WTSGetActiveConsoleSessionId();

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        public static extern int SetForegroundWindow(IntPtr hwnd);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

        [StructLayout(LayoutKind.Sequential)]
        internal struct HARDWAREINPUT
        {
            public uint Msg;
            public ushort ParamL;
            public ushort ParamH;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;            
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct KBDLLHOOKSTRUCT
        {
            public uint vkCode;
            public uint scanCode;
            public uint flags;
            public uint time;
            public IntPtr dwExtraInfo;
        }        

        [StructLayout(LayoutKind.Sequential)]
        internal struct MOUSEINPUT
        {
            public int X;
            public int Y;
            public uint MouseData;
            public uint Flags;
            public uint Time;
            public IntPtr ExtraInfo;
        }

        [StructLayout(LayoutKind.Explicit)]
        internal struct MOUSEKEYBDHARDWAREINPUT
        {
            [FieldOffset(0)]
            public HARDWAREINPUT Hardware;
            [FieldOffset(0)]
            public KEYBDINPUT Keyboard;
            [FieldOffset(0)]
            public MOUSEINPUT Mouse;
        }

        [StructLayout(LayoutKind.Sequential)]
        internal struct INPUT
        {
            public uint Type;
            public MOUSEKEYBDHARDWAREINPUT Data;
        }

        private void helpButton_Click(object sender, EventArgs e)
        {
            Process.Start("https://github.com/ryanries/SideBySideLogCompareHelper");
        }
    }
}
