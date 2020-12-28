using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace 鼠标键盘连点器
{

    /*
     * 鼠标钩子
     * 代码出处：
     * https://blog.csdn.net/hu421160052/article/details/99680147
     */

    class MouseHook
    {
        private Point point;
        private Point Point
        {
            get { return point; }
            set
            {
                if (point != value)
                {
                    point = value;
                    if (MouseMoveEvent != null)
                    {
                        var e = new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0);
                        MouseMoveEvent(this, e);
                    }
                }
            }
        }
        private int hHook;
        private static int hMouseHook = 0;
        private const int WM_MOUSEMOVE = 0x200;
        private const int WM_LBUTTONDOWN = 0x201;
        private const int WM_RBUTTONDOWN = 0x204;
        private const int WM_MBUTTONDOWN = 0x207;
        private const int WM_LBUTTONUP = 0x202;
        private const int WM_RBUTTONUP = 0x205;
        private const int WM_MBUTTONUP = 0x208;
        private const int WM_LBUTTONDBLCLK = 0x203;
        private const int WM_RBUTTONDBLCLK = 0x206;
        private const int WM_MBUTTONDBLCLK = 0x209;
        private const int WM_MOUSEWHEEL = 0x020A;


        public const int WH_MOUSE_LL = 14;
        public Win32Api.HookProc hProc;
        public MouseHook()
        {
            this.Point = new Point();
        }
        public int SetHook()
        {
            hProc = new Win32Api.HookProc(MouseHookProc);
            hHook = Win32Api.SetWindowsHookEx(WH_MOUSE_LL, hProc, IntPtr.Zero, 0);
            return hHook;
        }
        public void UnHook()
        {
            Win32Api.UnhookWindowsHookEx(hHook);
        }
        private int MouseHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            //MessageBox.Show(wParam.ToString("X"));
            Win32Api.MouseHookStruct MyMouseHookStruct = (Win32Api.MouseHookStruct)Marshal.PtrToStructure(lParam, typeof(Win32Api.MouseHookStruct));
            int temp = 2 << 16;
            int zDelta =//(int) wParam % temp;
                //((int)((long)wParam >> 16));
                //(int)wParam;
                 NativeMethods.GET_WHEEL_DELTA_WPARAM(wParam);

            //MouseLLHookStruct mouseHookStruct = (MouseLLHookStruct)Marshal.PtrToStructure(lParam, typeof(MouseLLHookStruct));
            //short mouseDelta = (short)((mouseHookStruct.mouseData >> 16) & 0xffff);

            if (nCode < 0)
            {
                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
            else
            {
                MouseButtons button = MouseButtons.None;
                int clickCount = 0;
                switch ((Int32)wParam)
                {
                    case WM_LBUTTONDOWN:
                        //鼠标左键按下
                        button = MouseButtons.Left;
                        clickCount = 1;
                        MouseDownEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                    case WM_RBUTTONDOWN:
                        //鼠标右键按下
                        button = MouseButtons.Right;
                        clickCount = 1;
                        MouseDownEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                    case WM_MBUTTONDOWN:
                        //鼠标中键按下
                        button = MouseButtons.Middle;
                        clickCount = 1;
                        MouseDownEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                    case WM_LBUTTONUP:
                        //鼠标左键松开
                        button = MouseButtons.Left;
                        clickCount = 1;
                        MouseUpEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                    case WM_RBUTTONUP:
                        //鼠标右键松开
                        button = MouseButtons.Right;
                        clickCount = 1;
                        MouseUpEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                    case WM_MBUTTONUP:
                        //鼠标中间松开
                        button = MouseButtons.Middle;
                        clickCount = 1;
                        MouseUpEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));
                        break;
                    case WM_MOUSEWHEEL:
                        //鼠标滑动滚轮
                        /*button = MouseButtons.Middle;
                        clickCount = 1;
                        MouseUpEvent(this, new MouseEventArgs(button, clickCount, point.X, point.Y, 0));*/
                        //MessageBox.Show("test");
                        /*var e1 = new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, zDelta);
                        MouseWheelEvent(this, e1);*/
                        
                        break;
                    case WM_MOUSEMOVE:
                        //鼠标移动事件
                        var e2 = new MouseEventArgs(MouseButtons.None, 0, point.X, point.Y, 0);
                        MouseMoveEvent(this, e2);
                        break;
                }

                this.Point = new Point(MyMouseHookStruct.pt.x, MyMouseHookStruct.pt.y);
                return Win32Api.CallNextHookEx(hHook, nCode, wParam, lParam);
            }
        }

        internal static class NativeMethods
        {
            internal static ushort HIWORD(IntPtr dwValue)
            {
                return (ushort)((((long)dwValue) >> 0x10) & 0xffff);
            }
            internal static ushort HIWORD(uint dwValue)
            {
                return (ushort)(dwValue >> 0x10);
            }
            internal static int GET_WHEEL_DELTA_WPARAM(IntPtr wParam)
            {
                return (short)HIWORD(wParam);
            }
            internal static int GET_WHEEL_DELTA_WPARAM(uint wParam)
            {
                return (short)HIWORD(wParam);
            }
        }

        public delegate void MouseMoveHandler(object sender, MouseEventArgs e);
        public event MouseMoveHandler MouseMoveEvent;

        public delegate void MouseClickHandler(object sender, MouseEventArgs e);
        public event MouseClickHandler MouseClickEvent;

        public delegate void MouseDownHandler(object sender, MouseEventArgs e);
        public event MouseDownHandler MouseDownEvent;

        public delegate void MouseUpHandler(object sender, MouseEventArgs e);
        public event MouseUpHandler MouseUpEvent;

        public delegate void MouseWheelHandler(object sender, MouseEventArgs e);
        public event MouseWheelHandler MouseWheelEvent ;

        /*public delegate void MouseMoveOHandler(object sender, MouseEventArgs e);
        public event MouseMoveHandler MouseMoveEvent;*/

        //WM_MOUSEMOVE

        //this.MouseWheel += new MouseEventHandler(Form1_MouseWheel);
    }
}
