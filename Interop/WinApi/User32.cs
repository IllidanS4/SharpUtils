﻿/* Date: 5.3.2017, Time: 16:38 */
using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

namespace IllidanS4.SharpUtils.Interop.WinApi
{
	partial class Win32Control
	{
		private static class User32
		{
			public const string Lib = "user32.dll";
			const CharSet DefaultCharSet = CharSet.Auto;
			
			public const uint WM_SETTEXT = 0x000C;
			public const uint WM_GETTEXT = 0x000D;
			public const uint WM_GETTEXTLENGTH = 0x000E;
			public const uint WM_SETFONT = 0x0030;
			public const uint WM_GETFONT = 0x0031;
			
			public const int GWL_EXSTYLE = -20;
			public const int GWL_HINSTANCE = -6;
			public const int GWL_HWNDPARENT = -8;
			public const int GWL_ID = -12;
			public const int GWL_STYLE = -16;
			public const int GWL_USERDATA = -21;
			public const int GWL_WNDPROC = -4;
			
			public const uint SWP_NOACTIVATE = 0x0010;
			public const uint SWP_NOMOVE = 0x002;
			public const uint SWP_NOSIZE = 0x0001;
			public const uint SWP_NOZORDER = 0x0004;
			public const uint SWP_SHOWWINDOW = 0x0040;
			
			public const uint GA_PARENT = 1;
			public const uint GA_ROOT = 2;
			public const uint GA_ROOTOWNER = 3;
			
			public const int GCW_ATOM = -32;
			
			public const int BS_CHECKBOX = 0x00000002;
			public const int BS_RADIOBUTTON = 0x00000004;
			
			public const int WS_DISABLED = 0x08000000;
			public const int WS_VISIBLE = 0x10000000;
			
			public const int SW_HIDE = 0;
			public const int SW_SHOW = 5;
			
			public const uint BM_GETCHECK = 0x00F0;
			public const uint BM_SETCHECK = 0x00F1;
			
			public const int SBS_VERT = 1;
			
			public const uint RDW_INVALIDATE = 0x0001;
			public const uint RDW_ERASE = 0x0004;
			public const uint RDW_ALLCHILDREN = 0x0080;
			
			#region Messages
			public delegate IntPtr WndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
			
			public static void PostMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
			{
				bool ret = InternalPostMessage(hWnd, msg, wParam, lParam);
				if(!ret)
				{
					throw new Win32Exception();
				}
			}
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet, EntryPoint="PostMessage")]
			static extern bool InternalPostMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
			#endregion
			
			#region Class
			[StructLayout(LayoutKind.Sequential)]
			public struct WNDCLASSEX
			{
				public uint cbSize;
				public uint style;
				[MarshalAs(UnmanagedType.FunctionPtr)]
				public WndProc lpfnWndProc;
				public int cbClsExtra;
				public int cbWndExtra;
				public IntPtr hInstance;
				public IntPtr hIcon;
				public IntPtr hCursor;
				public IntPtr hbrBackground;
				public string lpszMenuName;
				public string lpszClassName;
				public IntPtr hIconSm;
			}
			
			public static string GetClassName(IntPtr hWnd, bool type)
			{
				var buffer = new StringBuilder(256);
				uint result = type ? RealGetWindowClass(hWnd, buffer, (uint)buffer.Capacity) : InternalGetClassName(hWnd, buffer, buffer.Capacity);
				if(result == 0) throw new Win32Exception();
				return buffer.ToString();
			}
			
			public static WNDCLASSEX GetClassInfoEx(IntPtr hInstance, string lpClassName)
			{
				WNDCLASSEX cls;
				if(!InternalGetClassInfoEx(hInstance, lpClassName, out cls)) throw new Win32Exception();
				return cls;
			}
			
			public static WNDCLASSEX GetClassInfoEx(IntPtr hInstance, ushort lpClassName)
			{
				WNDCLASSEX cls;
				if(!InternalGetClassInfoEx(hInstance, (int)lpClassName, out cls)) throw new Win32Exception();
				return cls;
			}
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern ushort GetClassWord(IntPtr hWnd, int nIndex);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet, EntryPoint="GetClassName")]
			static extern uint InternalGetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			static extern uint RealGetWindowClass(IntPtr hwnd, StringBuilder pszType, uint cchType);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet, EntryPoint="GetClassInfoEx")]
			static extern bool InternalGetClassInfoEx(IntPtr hInstance, string lpClassName, out WNDCLASSEX lpWndClass);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet, EntryPoint="GetClassInfoEx")]
			static extern bool InternalGetClassInfoEx(IntPtr hInstance, int lpClassName, out WNDCLASSEX lpWndClass);
			#endregion
			
			#region Data
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern uint GetWindowThreadProcessId(IntPtr hWnd, out int lpdwProcessId);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern int GetWindowLong(IntPtr hWnd, int nIndex);
			
			public static IntPtr GetWindowInstance(IntPtr hWnd)
			{
				return (IntPtr)GetWindowLong(hWnd, GWL_HINSTANCE);
			}
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
			
			public static IntPtr GetWindowLongPtr(IntPtr hWnd, int nIndex)
			{
				if(IntPtr.Size == sizeof(int)) return (IntPtr)GetWindowLong(hWnd, nIndex);
				return GetWindowLongPtr64(hWnd, nIndex);
			}
			
			public static IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong)
			{
				if(IntPtr.Size == sizeof(int)) return (IntPtr)SetWindowLong(hWnd, nIndex, (int)dwNewLong);
				return SetWindowLongPtr64(hWnd, nIndex, dwNewLong);
			}
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet, EntryPoint="GetWindowLongPtr")]
			static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet, EntryPoint="SetWindowLongPtr")]
			static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
			#endregion
			
			#region Inheritance
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern IntPtr GetDesktopWindow();
			
			/*[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern IntPtr GetParent(IntPtr hWnd);*/
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern IntPtr GetAncestor(IntPtr hwnd, uint gaFlags);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
			
			public delegate bool WNDENUMPROC(IntPtr hwnd, IntPtr lParam);

			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool EnumChildWindows(IntPtr hwndParent, WNDENUMPROC lpEnumFunc, IntPtr lParam);
			
			#endregion
			
			#region Layout
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet, EntryPoint="GetWindowRect")]
			static extern bool InternalGetWindowRect(IntPtr hWnd, out RECT lpRect);
			
			public static RECT GetWindowRect(IntPtr hWnd)
			{
				RECT rect;
				if(!InternalGetWindowRect(hWnd, out rect)) throw new Win32Exception();
				return rect;
			}
			
			[StructLayout(LayoutKind.Sequential)]
			public struct RECT
			{
			     public int left;
			     public int top;
			     public int right;
			     public int bottom;
			}
			
			[StructLayout(LayoutKind.Sequential)]
			public struct POINT
			{
				public int x;
				public int y;
			}
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet, EntryPoint="GetWindowPlacement")]
			static extern bool InternalGetWindowPlacement(IntPtr hWnd, ref WINDOWPLACEMENT lpwndpl);
			
			public static WINDOWPLACEMENT GetWindowPlacement(IntPtr hWnd)
			{
				var placement = new WINDOWPLACEMENT();
				placement.length = Marshal.SizeOf(placement);
				if(!InternalGetWindowPlacement(hWnd, ref placement)) throw new Win32Exception();
				return placement;
			}
			
			[StructLayout(LayoutKind.Sequential)]
		    public struct WINDOWPLACEMENT
		    {
		        public int length;
		        public int flags;
		        public int showCmd;
		        public POINT ptMinPosition;
		        public POINT ptMaxPosition;
		        public RECT rcNormalPosition;
		    }
			
		    [DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool SetWindowPos(IntPtr hWnd, IntPtr hWndInsertAfter, int X, int Y, int cx, int cy, uint uFlags);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern int GetWindowRgn(IntPtr hWnd, IntPtr hRgn);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern int SetWindowRgn(IntPtr hWnd, IntPtr hRgn, bool redraw);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool ClientToScreen(IntPtr hWnd, ref POINT lpPoint);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool ScreenToClient(IntPtr hWnd, ref POINT lpPoint);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern int MapWindowPoints(IntPtr hWndFrom, IntPtr hWndTo, [In, Out] POINT[] lpPoints, int cPoints);
			#endregion
			
			#region Graphics
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool DeleteObject(IntPtr hObject);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern IntPtr GetWindowDC(IntPtr hWnd);
			
			public static int GetWindowStyle(IntPtr hWnd)
			{
				return (int)GetWindowLong(hWnd, GWL_STYLE);
			}
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool UpdateWindow(IntPtr hWnd);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool RedrawWindow(IntPtr hWnd, ref RECT lprcUpdate, IntPtr hrgnUpdate, uint flags);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool RedrawWindow(IntPtr hWnd, IntPtr lprcUpdate, IntPtr hrgnUpdate, uint flags);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool InvalidateRect(IntPtr hWnd, ref RECT lpRect, bool bErase);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool InvalidateRect(IntPtr hWnd, IntPtr lpRect, bool bErase);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool InvalidateRgn(IntPtr hWnd, IntPtr hRgn, bool bErase);
			#endregion
			
			#region Behaviour
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern IntPtr SetActiveWindow(IntPtr hWnd);
			
			[DllImport(Lib, SetLastError=true, CharSet=DefaultCharSet)]
			public static extern IntPtr SetFocus(IntPtr hWnd);
			#endregion
		}
	}
}
