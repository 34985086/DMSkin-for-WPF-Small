﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace DMSkin.WPF.Small
{
    public class Win32
    {


        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowLong(IntPtr hWnd, int nIndex);
        [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr", SetLastError = true)]
        private static extern IntPtr IntSetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);

        [DllImport("user32.dll", EntryPoint = "SetWindowLong", SetLastError = true)]
        private static extern Int32 IntSetWindowLong(IntPtr hWnd, int nIndex, Int32 dwNewLong);

        [DllImport("kernel32.dll", EntryPoint = "SetLastError")]
        public static extern void SetLastError(int dwErrorCode);

        private static int IntPtrToInt32(IntPtr intPtr)
        {
            return unchecked((int)intPtr.ToInt64());
        }

        public static IntPtr SetWindowLong(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
        {
            int error = 0;
            IntPtr result = IntPtr.Zero;
            // Win32 SetWindowLong doesn't clear error on success  
            SetLastError(0);

            if (IntPtr.Size == 4)
            {
                // use SetWindowLong  
                Int32 tempResult = IntSetWindowLong(hWnd, nIndex, IntPtrToInt32(dwNewLong));
                error = Marshal.GetLastWin32Error();
                result = new IntPtr(tempResult);
            }
            else
            {
                // use SetWindowLongPtr  
                result = IntSetWindowLongPtr(hWnd, nIndex, dwNewLong);
                error = Marshal.GetLastWin32Error();
            }

            if ((result == IntPtr.Zero) && (error != 0))
            {
                throw new System.ComponentModel.Win32Exception(error);
            }

            return result;
        }


        ////四个坐标
        //Win32.POINTAPI[] poin = new Win32.POINTAPI[4];
        ////是否正在绘制边角
        //bool ReWindowState = false;
        ////重设主窗口裁剪区域
        //public void ReWindow()
        //{
        //    if (ReWindowState)//已经在绘制过程
        //    {
        //        return;
        //    }
        //    ReWindowState = true;
        //    Task.Factory.StartNew(() =>
        //    {
        //        //150毫秒延迟,并且150毫秒内不会重复触发多次
        //        Thread.Sleep(150);
        //        //让窗体不被裁剪
        //        poin[3].x = (int)ActualWidth;
        //        poin[1].y = (int)ActualHeight;
        //        poin[2].x = (int)ActualWidth;
        //        poin[2].y = (int)ActualHeight;
        //        IntPtr hRgn = Win32.CreatePolygonRgn(ref poin[0], 4, 0);
        //        Win32.SetWindowRgn(Handle, hRgn, true);
        //        ReWindowState = false;
        //        //Debug.WriteLine("触发时间:" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
        //    });
        //}


        //public struct POINTAPI
        //{
        //    internal int x;
        //    internal int y;
        //}
        //[DllImport("gdi32.dll")]
        //public static extern IntPtr CreatePolygonRgn(ref POINTAPI lpPoint, int nCount, int nPolyFillMode);

        //[DllImport("User32.dll", CharSet = CharSet.Auto)]
        //public static extern bool SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw);
        public const int WM_TRUE = 0x1;
        public const int WM_FALSE = 0x0;
        public const int WM_NCCALCSIZE = 0x0083;
        public const int WM_NCPAINT = 0x0085;
        public const int WM_NCACTIVATE = 0x0086;

        public const int WM_NCUAHDRAWCAPTION = 0xAE;
        public const int WM_NCUAHDRAWFRAME = 0xAF;

        //不在ALT+TAB中
        public const int WS_EX_TOOLWINDOW = 0x00000080;

        // Sent to a window when the size or position of the window is about to change
        //发送到一个窗口时，窗口的大小和位置变化有关
        public const int WM_GETMINMAXINFO = 0x0024;
        /// <summary>
        /// 系统操作
        /// </summary>
        public const int WM_SYSCOMMAND = 0x112;
        /// <summary>
        /// 最小化
        /// </summary>
        public const int SC_MINIMIZE = 0xF020;
        /// <summary>
        /// 恢复
        /// </summary>
        public const int SC_RESTORE = 0xF120;

        // Retrieves a handle to the display monitor that is nearest to the window  
        //检索到靠近窗口的显示监视器的处理
        public const int MONITOR_DEFAULTTONEAREST = 2;

        // Retrieves a handle to the display monitor  
        [DllImport("user32.dll")]
        public static extern IntPtr MonitorFromWindow(IntPtr hwnd, int dwFlags);

        // RECT structure, Rectangle used by MONITORINFOEX  
        [StructLayout(LayoutKind.Sequential)]
        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        // MONITORINFOEX structure, Monitor information used by GetMonitorInfo function  
        [StructLayout(LayoutKind.Sequential)]
        public class MONITORINFOEX
        {
            public int cbSize;
            public RECT rcMonitor; // The display monitor rectangle  
            public RECT rcWork; // The working area rectangle  
            public int dwFlags;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 0x20)]
            public char[] szDevice;
        }

        // Point structure, Point information used by MINMAXINFO structure  
        [StructLayout(LayoutKind.Sequential)]
        public struct POINT
        {
            public int x;
            public int y;

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }
        }

        // MINMAXINFO structure, Window's maximum size and position information  
        [StructLayout(LayoutKind.Sequential)]
        public struct MINMAXINFO
        {
            public POINT ptReserved;
            public POINT ptMaxSize; // The maximized size of the window  
            public POINT ptMaxPosition; // The position of the maximized window  
            public POINT ptMinTrackSize;
            public POINT ptMaxTrackSize;
        }

        // Get the working area of the specified monitor  
        [DllImport("user32.dll")]
        public static extern bool GetMonitorInfo(HandleRef hmonitor, [In, Out] MONITORINFOEX monitorInfo);

        // Sent to a window in order to determine what part of the window corresponds to a particular screen coordinate  
        public const int WM_NCHITTEST = 0x0084;

        /// <summary>  
        /// Indicates the position of the cursor hot spot.  
        /// </summary>  
        public enum HitTest : int
        {
            /// <summary>  
            /// On the screen background or on a dividing line between windows (same as HTNOWHERE, except that the DefWindowProc function produces a system beep to indicate an error).  
            /// </summary>  
            HTERROR = -2,

            /// <summary>  
            /// In a window currently covered by another window in the same thread (the message will be sent to underlying windows in the same thread until one of them returns a code that is not HTTRANSPARENT).  
            /// </summary>  
            HTTRANSPARENT = -1,

            /// <summary>  
            /// On the screen background or on a dividing line between windows.  
            /// </summary>  
            HTNOWHERE = 0,

            /// <summary>  
            /// In a client area.  
            /// </summary>  
            HTCLIENT = 1,

            /// <summary>  
            /// In a title bar.  
            /// </summary>  
            HTCAPTION = 2,

            /// <summary>  
            /// In a window menu or in a Close button in a child window.  
            /// </summary>  
            HTSYSMENU = 3,

            /// <summary>  
            /// In a size box (same as HTSIZE).  
            /// </summary>  
            HTGROWBOX = 4,

            /// <summary>  
            /// In a size box (same as HTGROWBOX).  
            /// </summary>  
            HTSIZE = 4,

            /// <summary>  
            /// In a menu.  
            /// </summary>  
            HTMENU = 5,

            /// <summary>  
            /// In a horizontal scroll bar.  
            /// </summary>  
            HTHSCROLL = 6,

            /// <summary>  
            /// In the vertical scroll bar.  
            /// </summary>  
            HTVSCROLL = 7,

            /// <summary>  
            /// In a Minimize button.  
            /// </summary>  
            HTMINBUTTON = 8,

            /// <summary>  
            /// In a Minimize button.  
            /// </summary>  
            HTREDUCE = 8,

            /// <summary>  
            /// In a Maximize button.  
            /// </summary>  
            HTMAXBUTTON = 9,

            /// <summary>  
            /// In a Maximize button.  
            /// </summary>  
            HTZOOM = 9,

            /// <summary>  
            /// In the left border of a resizable window (the user can click the mouse to resize the window horizontally).  
            /// </summary>  
            HTLEFT = 10,

            /// <summary>  
            /// In the right border of a resizable window (the user can click the mouse to resize the window horizontally).  
            /// </summary>  
            HTRIGHT = 11,

            /// <summary>  
            /// In the upper-horizontal border of a window.  
            /// </summary>  
            HTTOP = 12,

            /// <summary>  
            /// In the upper-left corner of a window border.  
            /// </summary>  
            HTTOPLEFT = 13,

            /// <summary>  
            /// In the upper-right corner of a window border.  
            /// </summary>  
            HTTOPRIGHT = 14,

            /// <summary>  
            /// In the lower-horizontal border of a resizable window (the user can click the mouse to resize the window vertically).  
            /// </summary>  
            HTBOTTOM = 15,

            /// <summary>  
            /// In the lower-left corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).  
            /// </summary>  
            HTBOTTOMLEFT = 16,

            /// <summary>  
            /// In the lower-right corner of a border of a resizable window (the user can click the mouse to resize the window diagonally).  
            /// </summary>  
            HTBOTTOMRIGHT = 17,

            /// <summary>  
            /// In the border of a window that does not have a sizing border.  
            /// </summary>  
            HTBORDER = 18,

            /// <summary>  
            /// In a Close button.  
            /// </summary>  
            HTCLOSE = 20,

            /// <summary>  
            /// In a Help button.  
            /// </summary>  
            HTHELP = 21,
        };

        // Posted when the user presses the left mouse button while the cursor is within the nonclient area of a window  
        public const int WM_NCLBUTTONDOWN = 0x00A1;

        // Sends the specified message to a window or windows  
        [DllImport("user32.dll", EntryPoint = "SendMessage")]
        public static extern int SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
    }
}
