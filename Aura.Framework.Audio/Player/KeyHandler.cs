using Aura.Media.EventArguments;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Aura.Media
{
    /// <summary>
    /// A class that manages a global low level media-keys hook.
    /// </summary>
    public class KeyHandler : IDisposable
    {
        #region Constant, Structure and Delegate Definitions

        /// <summary>
        /// Defines the callback type for the hook.
        /// </summary>
        public delegate int KeyboardHookProc(int code, int wParam, ref KeyboardHookStruct lParam);

        /// <summary>
        /// Defines a series of <see cref="int"/> values to set the <see cref="KeyboardHookProc"/>.
        /// </summary>
        public struct KeyboardHookStruct
        {
            public int vkCode;
            public int scanCode;
            public int flags;
            public int time;
            public int dwExtraInfo;
        }

        private const int WH_KEYBOARD_LL = 13;
        private const int WM_KEYDOWN = 0x100;
        private const int WM_KEYUP = 0x101;
        private const int WM_SYSKEYDOWN = 0x104;
        private const int WM_SYSKEYUP = 0x105;

        #endregion Constant, Structure and Delegate Definitions

        #region Instance Variables

        /// <summary>
        /// The collections of keys to watch for
        /// </summary>
        private List<Keys> _Keys = new List<Keys>();

        /// <summary>
        /// The collections of media-keys to watch for
        /// </summary>
        public List<string> MediaKeys
        {
            get
            {
                List<string> final = new List<string>();

                foreach (var item in _Keys)
                {
                    final.Add(item.ToString());
                }
                return final;
            }
            set
            {
                foreach (string item in MediaKeys)
                {
                    Keys key;
                    Enum.TryParse(item, out key);

                    _Keys.Add(key);
                }
            }
        }

        /// <summary>
        /// Handle to the hook, need this to unhook and call the next hook
        /// </summary>
        public IntPtr HandleHook { get; private set; } = IntPtr.Zero;

        #endregion Instance Variables

        #region Events

        /// <summary>
        /// Occurs when one of the hooked keys is pressed (using System.Windows.Forms).
        /// </summary>
        public event KeyEventHandler KeyDown;

        /// <summary>
        /// Occurs when one of the hooked keys is released (using System.Windows.Forms).
        /// </summary>
        public event KeyEventHandler KeyUp;

        /// <summary>
        /// Occurs when one of the hooked keys is pressed (using System.Windows.Forms).
        /// </summary>
        public event KeyEventHandler KeyPressed;

        /// <summary>
        /// Occurs when one of the hooked keys is pressed (without System.Windows.Forms).
        /// </summary>
        public event EventHandler<MediaKeyEventArgs> MediaKeyPressed;

        #endregion Events

        #region Constructors and Destructors

        /// <summary>
        /// Initializes a new instance of the <see cref="KeyHandler"/> class and installs the keyboard hook.
        /// </summary>
        public KeyHandler()
        {
            MediaKeys = new List<string>();

            IntPtr hInstance = LoadLibrary("User32");

            HandleHook = SetWindowsHookEx(WH_KEYBOARD_LL, HookProcess, hInstance, 0);

            _Keys.Add(Keys.MediaStop);
            _Keys.Add(Keys.MediaPlayPause);
            _Keys.Add(Keys.MediaNextTrack);
            _Keys.Add(Keys.MediaPreviousTrack);
        }

        /// <summary>
        /// Releases unmanaged resources and performs other cleanup operations before the <see cref="KeyHandler"/> is reclaimed by garbage collection and uninstalls the keyboard hook.
        /// </summary>
        public void Dispose()
        {
            Terminate();
        }

        #endregion Constructors and Destructors

        #region Public Methods

        /// <summary>
        /// Terminates the global hook.
        /// </summary>
        public void Terminate()
        {
            UnhookWindowsHookEx(HandleHook);
        }

        /// <summary>
        /// Initializes the callback for the keyboard hook.
        /// </summary>
        public int HookProcess(int code, int wParam, ref KeyboardHookStruct lParam)
        {
            if (code >= 0)
            {
                Keys key = (Keys)lParam.vkCode;
                if (_Keys.Contains(key))
                {
                    if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                    {
                        switch (key)
                        {
                            case Keys.MediaNextTrack:
                                MediaKeyPressed(this, new MediaKeyEventArgs(Aura.Framework.Audio.Player.Enumerators.MediaKeys.Forward));
                                return 1;

                            case Keys.MediaPlayPause:
                                MediaKeyPressed(this, new MediaKeyEventArgs(Aura.Framework.Audio.Player.Enumerators.MediaKeys.PausePlay));
                                return 1;

                            case Keys.MediaPreviousTrack:
                                MediaKeyPressed(this, new MediaKeyEventArgs(Aura.Framework.Audio.Player.Enumerators.MediaKeys.Backward));
                                return 1;

                            case Keys.MediaStop:
                                MediaKeyPressed(this, new MediaKeyEventArgs(Aura.Framework.Audio.Player.Enumerators.MediaKeys.Stop));
                                return 1;
                        }
                    }

                    KeyEventArgs kea = new KeyEventArgs(key);
                    if ((wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                    {
                        KeyDown(this, kea);
                        KeyPressed(this, kea);
                    }
                    else if ((wParam == WM_KEYUP || wParam == WM_SYSKEYUP) && (KeyUp != null))
                    {
                        KeyUp(this, kea);
                    }
                    if (kea.Handled)
                        return 1;
                }
            }
            return CallNextHookEx(HandleHook, code, wParam, ref lParam);
        }

        #endregion Public Methods

        #region DLL imports

        /// <summary>
        /// Sets the windows hook, do the desired event, one of hInstance or threadId must be non-null.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern IntPtr SetWindowsHookEx(int idHook, KeyboardHookProc callback, IntPtr hInstance, uint threadId);

        /// <summary>
        /// Unhooks the windows hook.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        /// <summary>
        /// Calls the next hook.
        /// </summary>
        [DllImport("user32.dll")]
        private static extern int CallNextHookEx(IntPtr idHook, int nCode, int wParam, ref KeyboardHookStruct lParam);

        /// <summary>
        /// Loads the library.
        /// </summary>
        [DllImport("kernel32.dll")]
        private static extern IntPtr LoadLibrary(string lpFileName);

        #endregion DLL imports
    }
}