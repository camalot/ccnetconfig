/*
 * Copyright (c) 2006, Ryan Conrad. All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the following conditions are met:
 * - Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * 
 * - Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the following disclaimer in the 
 *    documentation and/or other materials provided with the distribution.
 * 
 * - Neither the name of the Camalot Designs nor the names of its contributors may be used to endorse or promote products derived from this software 
 *    without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE 
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT OWNER OR CONTRIBUTORS BE 
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE 
 * GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT 
 * LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH 
 * DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;

namespace CCNetConfig.Components {
  /// <summary>
  /// Defines the shape of hook procedures that can be called by the OpenFileDialog
  /// </summary>
  internal delegate IntPtr WndHookProc ( IntPtr hWnd, UInt16 msg, Int32 wParam, Int32 lParam );

  /// <summary>
  /// Values that can be placed in the OPENFILENAME structure, we don't use all of them
  /// </summary>
  internal class OpenFileNameFlags {
    public const Int32 ReadOnly = 0x00000001;
    public const Int32 OverWritePrompt = 0x00000002;
    public const Int32 HideReadOnly = 0x00000004;
    public const Int32 NoChangeDir = 0x00000008;
    public const Int32 ShowHelp = 0x00000010;
    public const Int32 EnableHook = 0x00000020;
    public const Int32 EnableTemplate = 0x00000040;
    public const Int32 EnableTemplateHandle = 0x00000080;
    public const Int32 NoValidate = 0x00000100;
    public const Int32 AllowMultiSelect = 0x00000200;
    public const Int32 ExtensionDifferent = 0x00000400;
    public const Int32 PathMustExist = 0x00000800;
    public const Int32 FileMustExist = 0x00001000;
    public const Int32 CreatePrompt = 0x00002000;
    public const Int32 ShareAware = 0x00004000;
    public const Int32 NoReadOnlyReturn = 0x00008000;
    public const Int32 NoTestFileCreate = 0x00010000;
    public const Int32 NoNetworkButton = 0x00020000;
    public const Int32 NoLongNames = 0x00040000;
    public const Int32 Explorer = 0x00080000;
    public const Int32 NoDereferenceLinks = 0x00100000;
    public const Int32 LongNames = 0x00200000;
    public const Int32 EnableIncludeNotify = 0x00400000;
    public const Int32 EnableSizing = 0x00800000;
    public const Int32 DontAddToRecent = 0x02000000;
    public const Int32 ForceShowHidden = 0x10000000;
  };

  /// <summary>
  /// Values that can be placed in the FlagsEx field of the OPENFILENAME structure
  /// </summary>
  internal class OpenFileNameFlagsEx {
    public const Int32 NoPlacesBar = 0x00000001;
  };

  /// <summary>
  /// Id's for the controls located in the file dialog.
  /// </summary>
  internal class FileDialogControls {
    public const UInt32 LookInLabel = 0x00000443;
    public const UInt32 LookInComboBox = 0x0000471;
    public const UInt32 NavigationToolbar = 0x0000440;
    public const UInt32 PlacesBar = 0x000004A0;
    public const UInt32 UnknownListbox = 0x00000460;
    public const UInt32 ContentExplorer = 0x00000461;
    public const UInt32 FileNameLabel = 0x00000442;
    public const UInt32 FileNameComoboBoxEx = 0x0000047C;
    public const UInt32 FileOfTypeLabel = 0x00000441;
    public const UInt32 FileOfTypeComboBox = 0x00000470;
    public const UInt32 OpenAsReadOnlyCheckBox = 0x00000410;
    public const UInt32 OpenSaveButton = 0x00000001;
    public const UInt32 CancelButton = 0x00000002;
    public const UInt32 HelpButton = 0x0000040E;
    public const UInt32 Grip = 0xFFFFFFFF;
  }

  /// <summary>
  /// A small subset of the window messages that can be sent to the OpenFileDialog
  /// These are just the ones that this implementation is interested in
  /// </summary>
  internal class WindowMessage {
    public const int InitDialog = 0x0110;
    public const int Size = 0x0005;
    public const int Notify = 0x004E;
    public const int Destroy = 0x2;
    public const int SetFont = 0x0030;
    public const int GetFont = 0x0031;
    public const int Paint = 0x000F;
    public const int Null = 0x0;
    public const int Create = 0x1;
    public const int Activate = 0x6;
    public const int SetFocus = 0x7;
    public const int KillFocus = 0x8;
    public const int Close = 0x10;
    public const int Quit = 0x12;
    public const int EraseBackground = 0x14;
    public const int SettingChange = 0x1A;
    public const int SetCursor = 0x20;
    public const int MouseActivate = 0x21;
    public const int WindowPositionChanging = 0x46;
    public const int SetIcon = 0x80;
    public const int NCCalcSize = 0x83;
    public const int NCHitTest = 0x84;
    public const int NCPaint = 0x0085;
    public const int NCActivate = 0x86;
    public const int KeyDown = 0x0100;
    public const int KeyUp = 0x0101;
    public const int Char = 0x0102;
    public const int SysKeyDown = 0x0104;
    public const int SysKeyUp = 0x0105;
    public const int SysChar = 0x0106;
    public const int SysCommand = 0x112;
    public const int Timer = 0x0113;
    public const int MouseMove = 0x0200;
    public const int LeftButtonDown = 0x0201;
    public const int LeftButtonUp = 0x0202;
    public const int LeftButtonDblClick = 0x0203;
    public const int RightButtonDown = 0x0204;
    public const int RightButtonUp = 0x0205;
    public const int RightButtonDblClick = 0x0206;
    public const int MiddleButtonDown = 0x0207;
    public const int MiddleButtonUp = 0x0208;
    public const int MiddleButtonDblClick = 0x0209;
    public const int MouseWheel = 0x020A;
    public const int XButtonDown = 0x020B;
    public const int XButtonUp = 0x020C;
    public const int XButtonDblClick = 0x020D;
    public const int Sizing = 0x0214;
    public const int Moving = 0x0216;
    public const int MouseLeave = 0x02A3;
  };

  /// <summary>
  /// The possible notification messages that can be generated by the OpenFileDialog
  /// We only look for CDN_SELCHANGE
  /// </summary>
  internal class CommonDlgNotification {
    private const UInt16 First = unchecked ( ( UInt16 ) ( ( UInt16 ) 0 - ( UInt16 ) 601 ) );

    public const UInt16 InitDone = ( First - 0x0000 );
    public const UInt16 SelChange = ( First - 0x0001 );
    public const UInt16 FolderChange = ( First - 0x0002 );
    public const UInt16 ShareViolation = ( First - 0x0003 );
    public const UInt16 Help = ( First - 0x0004 );
    public const UInt16 FileOk = ( First - 0x0005 );
    public const UInt16 TypeChange = ( First - 0x0006 );
    public const UInt16 IncludeItem = ( First - 0x0007 );
  }

  /// <summary>
  /// Messages that can be send to the common dialogs
  /// We only use CDM_GETFILEPATH
  /// </summary>
  internal class CommonDlgMessage {
    private const UInt16 User = 0x0400;
    private const UInt16 First = User + 100;

    public const UInt16 GetFilePath = First + 0x0001;
  };

  /// <summary>
  /// See the documentation for OPENFILENAME
  /// </summary>
  internal struct OpenFileName {
    public Int32 lStructSize;
    public IntPtr hwndOwner;
    public IntPtr hInstance;
    [MarshalAs ( UnmanagedType.LPTStr )]
    public string lpstrFilter;
    [MarshalAs ( UnmanagedType.LPTStr )]
    public string lpstrCustomFilter;
    public Int32 nMaxCustFilter;
    public Int32 nFilterIndex;
    //[MarshalAs ( UnmanagedType.LPTStr )]
    public IntPtr lpstrFile;
    public Int32 nMaxFile;
    [MarshalAs ( UnmanagedType.LPTStr )]
    public string lpstrFileTitle;
    public Int32 nMaxFileTitle;
    [MarshalAs ( UnmanagedType.LPTStr )]
    public string lpstrInitialDir;
    [MarshalAs ( UnmanagedType.LPTStr )]
    public string lpstrTitle;
    public Int32 Flags;
    public Int16 nFileOffset;
    public Int16 nFileExtension;
    [MarshalAs ( UnmanagedType.LPTStr )]
    public string lpstrDefExt;
    public Int32 lCustData;
    public WndHookProc lpfnHook;
    public IntPtr lpTemplateName;
    public IntPtr pvReserved;
    public Int32 dwReserved;
    public Int32 FlagsEx;
  };

  /// <summary>
  /// Part of the notification messages sent by the common dialogs
  /// </summary>
  [StructLayout ( LayoutKind.Explicit )]
  internal struct NMHDR {
    [FieldOffset ( 0 )]
    public IntPtr hWndFrom;
    [FieldOffset ( 4 )]
    public UInt16 idFrom;
    [FieldOffset ( 8 )]
    public UInt16 code;
  };

  /// <summary>
  /// Part of the notification messages sent by the common dialogs
  /// </summary>
  [StructLayout ( LayoutKind.Explicit )]
  internal struct OfNotify {
    [FieldOffset ( 0 )]
    public NMHDR hdr;
    [FieldOffset ( 12 )]
    public IntPtr ipOfn;
    [FieldOffset ( 16 )]
    public IntPtr ipFile;
  };

  /// <summary>
  /// Win32 window style constants
  /// We use them to set up our child window
  /// </summary>
  internal class DlgStyle {
    public const Int32 DsSetFont = 0x00000040;
    public const Int32 Ds3dLook = 0x00000004;
    public const Int32 DsControl = 0x00000400;
    public const Int32 WsChild = 0x40000000;
    public const Int32 WsClipSiblings = 0x04000000;
    public const Int32 WsVisible = 0x10000000;
    public const Int32 WsGroup = 0x00020000;
    public const Int32 SsNotify = 0x00000100;
  };

  /// <summary>
  /// Win32 "extended" window style constants
  /// </summary>
  internal class ExStyle {
    public const Int32 WsExNoParentNotify = 0x00000004;
    public const Int32 WsExControlParent = 0x00010000;
  };

  /// <summary>
  /// An in-memory Win32 dialog template
  /// Note: this has a very specific structure with a single static "label" control
  /// See documentation for DLGTEMPLATE and DLGITEMTEMPLATE
  /// </summary>
  [StructLayout ( LayoutKind.Sequential )]
  internal class DlgTemplate {
    // The dialog template - see documentation for DLGTEMPLATE
    public Int32 style = DlgStyle.Ds3dLook | DlgStyle.DsControl | DlgStyle.WsChild | DlgStyle.WsClipSiblings | DlgStyle.SsNotify;
    public Int32 extendedStyle = ExStyle.WsExControlParent;
    public Int16 numItems = 1;
    public Int16 x = 0;
    public Int16 y = 0;
    public Int16 cx = 0;
    public Int16 cy = 0;
    public Int16 reservedMenu = 0;
    public Int16 reservedClass = 0;
    public Int16 reservedTitle = 0;

    // Single dlg item, must be dword-aligned - see documentation for DLGITEMTEMPLATE
    public Int32 itemStyle = DlgStyle.WsChild;
    public Int32 itemExtendedStyle = ExStyle.WsExNoParentNotify;
    public Int16 itemX = 0;
    public Int16 itemY = 0;
    public Int16 itemCx = 0;
    public Int16 itemCy = 0;
    public Int16 itemId = 0;
    public UInt16 itemClassHdr = 0xffff;	// we supply a constant to indicate the class of this control
    public Int16 itemClass = 0x0082;	// static label control
    public Int16 itemText = 0x0000;	// no text for this control
    public Int16 itemData = 0x0000;	// no creation data for this control
  };

  /// <summary>
  /// The rectangle structure used in Win32 API calls
  /// </summary>
  [StructLayout ( LayoutKind.Sequential )]
  internal struct RECT {
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;
  };

  /// <summary>
  /// The point structure used in Win32 API calls
  /// </summary>
  [StructLayout ( LayoutKind.Sequential )]
  internal struct POINT {
    public int X;
    public int Y;
  };

  /// <summary>
  /// Contains all of the p/invoke declarations for the Win32 APIs used in this sample
  /// </summary>
  public class NativeMethods {

    /// <summary>
    /// Gets the DLG item.
    /// </summary>
    /// <param name="hWndDlg">The h WND DLG.</param>
    /// <param name="Id">The id.</param>
    /// <returns></returns>
    [DllImport ( "User32.dll", CharSet = CharSet.Unicode )]
    internal static extern IntPtr GetDlgItem ( IntPtr hWndDlg, UInt32 Id );

    /// <summary>
    /// Gets the parent.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <returns></returns>
    [DllImport ( "User32.dll", CharSet = CharSet.Unicode )]
    internal static extern IntPtr GetParent ( IntPtr hWnd );

    /// <summary>
    /// Sets the parent.
    /// </summary>
    /// <param name="hWndChild">The h WND child.</param>
    /// <param name="hWndNewParent">The h WND new parent.</param>
    /// <returns></returns>
    [DllImport ( "User32.dll", CharSet = CharSet.Unicode )]
    internal static extern IntPtr SetParent ( IntPtr hWndChild, IntPtr hWndNewParent );

    /// <summary>
    /// Sends the message.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <param name="msg">The MSG.</param>
    /// <param name="wParam">The w param.</param>
    /// <param name="buffer">The buffer.</param>
    /// <returns></returns>
    [DllImport ( "User32.dll", CharSet = CharSet.Unicode )]
    internal static extern UInt32 SendMessage ( IntPtr hWnd, UInt32 msg, UInt32 wParam, StringBuilder buffer );

    /// <summary>
    /// Sends the message.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <param name="Msg">The MSG.</param>
    /// <param name="wParam">The w param.</param>
    /// <param name="lParam">The l param.</param>
    /// <returns></returns>
    [DllImport ( "user32.dll" )]
    internal static extern UInt32 SendMessage ( IntPtr hWnd, UInt32 Msg, UInt32 wParam, UInt32 lParam );

    /// <summary>
    /// Gets the window rect.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <param name="rc">The rc.</param>
    /// <returns></returns>
    [DllImport ( "user32.dll", CharSet = CharSet.Unicode )]
    internal static extern int GetWindowRect ( IntPtr hWnd, ref RECT rc );

    /// <summary>
    /// Gets the client rect.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <param name="rc">The rc.</param>
    /// <returns></returns>
    [DllImport ( "user32.dll", CharSet = CharSet.Unicode )]
    internal static extern int GetClientRect ( IntPtr hWnd, ref RECT rc );

    /// <summary>
    /// Screens to client.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <param name="pt">The pt.</param>
    /// <returns></returns>
    [DllImport ( "user32.dll", CharSet = CharSet.Unicode )]
    internal static extern bool ScreenToClient ( IntPtr hWnd, ref POINT pt );

    /// <summary>
    /// Moves the window.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <param name="X">The X.</param>
    /// <param name="Y">The Y.</param>
    /// <param name="Width">The width.</param>
    /// <param name="Height">The height.</param>
    /// <param name="repaint">if set to <c>true</c> [repaint].</param>
    /// <returns></returns>
    [DllImport ( "user32.dll", CharSet = CharSet.Unicode )]
    internal static extern bool MoveWindow ( IntPtr hWnd, Int32 X, Int32 Y, Int32 Width, Int32 Height, bool repaint );

    /// <summary>
    /// Gets the name of the open file.
    /// </summary>
    /// <param name="ofn">The ofn.</param>
    /// <returns></returns>
    [DllImport ( "ComDlg32.dll", CharSet = CharSet.Unicode )]
    internal static extern bool GetOpenFileName ( ref OpenFileName ofn );

    /// <summary>
    /// Gets the name of the save file.
    /// </summary>
    /// <param name="lpofn">The lpofn.</param>
    /// <returns></returns>
    [DllImport ( "Comdlg32.dll", CharSet = CharSet.Auto, SetLastError = true )]
    internal static extern bool GetSaveFileName ( ref OpenFileName lpofn );

    /// <summary>
    /// Comms the DLG extended error.
    /// </summary>
    /// <returns></returns>
    [DllImport ( "ComDlg32.dll", CharSet = CharSet.Unicode )]
    internal static extern Int32 CommDlgExtendedError ( );

    /// <summary>
    /// Creates the window ex.
    /// </summary>
    /// <param name="dwExStyle">The dw ex style.</param>
    /// <param name="lpClassName">Name of the lp class.</param>
    /// <param name="lpWindowName">Name of the lp window.</param>
    /// <param name="dwStyle">The dw style.</param>
    /// <param name="x">The x.</param>
    /// <param name="y">The y.</param>
    /// <param name="nWidth">Width of the n.</param>
    /// <param name="nHeight">Height of the n.</param>
    /// <param name="hWndParent">The h WND parent.</param>
    /// <param name="hMenu">The h menu.</param>
    /// <param name="hInstance">The h instance.</param>
    /// <param name="lpParam">The lp param.</param>
    /// <returns></returns>
    [DllImport ( "user32.dll", CharSet = CharSet.Auto )]
    internal static extern IntPtr CreateWindowEx ( Int32 dwExStyle, string lpClassName, string lpWindowName, UInt32 dwStyle, Int32 x, Int32 y,
        Int32 nWidth, Int32 nHeight, IntPtr hWndParent, Int32 hMenu, Int32 hInstance, Int32 lpParam );

    /// <summary>
    /// Destroys the window.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <returns></returns>
    [DllImport ( "user32.dll" )]
    internal static extern bool DestroyWindow ( IntPtr hWnd );

    /// <summary>
    /// Sets the window text.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <param name="lpString">The lp string.</param>
    /// <returns></returns>
    [DllImport ( "user32.dll", CharSet = CharSet.Auto )]
    internal static extern bool SetWindowText ( IntPtr hWnd, string lpString );

    /// <summary>
    /// Sets the window pos.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <param name="hWndInsertAfter">The h WND insert after.</param>
    /// <param name="X">The X.</param>
    /// <param name="Y">The Y.</param>
    /// <param name="cx">The cx.</param>
    /// <param name="cy">The cy.</param>
    /// <param name="uFlags">The u flags.</param>
    /// <returns></returns>
    [DllImport ( "user32.dll" )]
    internal static extern bool SetWindowPos ( IntPtr hWnd, IntPtr hWndInsertAfter, Int32 X, Int32 Y, Int32 cx, Int32 cy, UInt32 uFlags );

    /// <summary>
    /// Gets the window text.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <param name="lpString">The lp string.</param>
    /// <param name="nMaxCount">The n max count.</param>
    /// <returns></returns>
    [DllImport ( "user32.dll", SetLastError = true, CharSet = CharSet.Auto )]
    internal static extern int GetWindowText ( IntPtr hWnd, [Out] StringBuilder lpString, int nMaxCount );

    /// <summary>
    /// Gets the length of the window text.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <returns></returns>
    [DllImport ( "user32.dll", SetLastError = true, CharSet = CharSet.Auto )]
    internal static extern int GetWindowTextLength ( IntPtr hWnd );

    /// <summary>
    /// A character buffer
    /// </summary>
    public abstract class CharBuffer {
      /// <summary>
      /// Initializes a new instance of the <see cref="CharBuffer"/> class.
      /// </summary>
      protected CharBuffer ( ) {
      }

      /// <summary>
      /// Allocs the co task mem.
      /// </summary>
      /// <returns></returns>
      public abstract IntPtr AllocCoTaskMem ( );
      /// <summary>
      /// Creates the buffer.
      /// </summary>
      /// <param name="size">The size.</param>
      /// <returns></returns>
      public static NativeMethods.CharBuffer CreateBuffer ( int size ) {
        if ( Marshal.SystemDefaultCharSize == 1 )
          return new NativeMethods.AnsiCharBuffer ( size );
        else
          return new NativeMethods.UnicodeCharBuffer ( size );
      }

      /// <summary>
      /// Gets the string.
      /// </summary>
      /// <returns></returns>
      public abstract string GetString ( );
      /// <summary>
      /// Puts the co task mem.
      /// </summary>
      /// <param name="ptr">The PTR.</param>
      public abstract void PutCoTaskMem ( IntPtr ptr );
      /// <summary>
      /// Puts the string.
      /// </summary>
      /// <param name="s">The s.</param>
      public abstract void PutString ( string s );
    }

    /// <summary>
    /// An Ansi character buffer
    /// </summary>
    public class AnsiCharBuffer : NativeMethods.CharBuffer {
      /// <summary>
      /// Initializes a new instance of the <see cref="AnsiCharBuffer"/> class.
      /// </summary>
      /// <param name="size">The size.</param>
      public AnsiCharBuffer ( int size ) {
        this.buffer = new byte[ size ];
      }

      /// <summary>
      /// Allocs the co task mem.
      /// </summary>
      /// <returns></returns>
      public override IntPtr AllocCoTaskMem ( ) {
        IntPtr ptr1 = Marshal.AllocCoTaskMem ( this.buffer.Length );
        Marshal.Copy ( this.buffer, 0, ptr1, this.buffer.Length );
        return ptr1;
      }

      /// <summary>
      /// Gets the string.
      /// </summary>
      /// <returns></returns>
      public override string GetString ( ) {
        int num1 = this.offset;
        while ( ( num1 < this.buffer.Length ) && ( this.buffer[ num1 ] != 0 ) ) {
          num1++;
        }
        string text1 = Encoding.Default.GetString ( this.buffer, this.offset, num1 - this.offset );
        if ( num1 < this.buffer.Length ) {
          num1++;
        }
        this.offset = num1;
        return text1;
      }

      /// <summary>
      /// Puts the co task mem.
      /// </summary>
      /// <param name="ptr">The PTR.</param>
      public override void PutCoTaskMem ( IntPtr ptr ) {
        Marshal.Copy ( ptr, this.buffer, 0,this.buffer.Length );
        this.offset = 0;
      }

      /// <summary>
      /// Puts the string.
      /// </summary>
      /// <param name="s">The s.</param>
      public override void PutString ( string s ) {
        byte[ ] buffer1 = Encoding.Default.GetBytes ( s );
        int num1 = Math.Min ( buffer1.Length, this.buffer.Length - this.offset );
        Array.Copy ( buffer1, 0, this.buffer, this.offset, num1 );
        this.offset += num1;
        if ( this.offset < this.buffer.Length ) {
          this.buffer[ this.offset++ ] = 0;
        }
      }


      internal byte[ ] buffer;
      internal int offset;
    }

    /// <summary>
    /// A Unicode character buffer
    /// </summary>
    public class UnicodeCharBuffer : NativeMethods.CharBuffer {
      /// <summary>
      /// Initializes a new instance of the <see cref="UnicodeCharBuffer"/> class.
      /// </summary>
      /// <param name="size">The size.</param>
      public UnicodeCharBuffer ( int size ) {
        this.buffer = new char[ size ];
      }

      /// <summary>
      /// Allocs the co task mem.
      /// </summary>
      /// <returns></returns>
      public override IntPtr AllocCoTaskMem ( ) {
        IntPtr ptr1 = Marshal.AllocCoTaskMem ( this.buffer.Length * 2 );
        Marshal.Copy ( this.buffer, 0, ptr1, this.buffer.Length );
        return ptr1;
      }

      /// <summary>
      /// Gets the string.
      /// </summary>
      /// <returns></returns>
      public override string GetString ( ) {
        int num1 = this.offset;
        while ( ( num1 < this.buffer.Length ) && ( this.buffer[ num1 ] != '\0' ) ) {
          num1++;
        }
        string text1 = new string ( this.buffer, this.offset, num1 - this.offset );
        if ( num1 < this.buffer.Length ) {
          num1++;
        }
        this.offset = num1;
        return text1;
      }

      /// <summary>
      /// Puts the co task mem.
      /// </summary>
      /// <param name="ptr">The PTR.</param>
      public override void PutCoTaskMem ( IntPtr ptr ) {
        Marshal.Copy ( ptr, this.buffer, 0, this.buffer.Length );
        this.offset = 0;
      }

      /// <summary>
      /// Puts the string.
      /// </summary>
      /// <param name="s">The s.</param>
      public override void PutString ( string s ) {
        int num1 = Math.Min ( s.Length, this.buffer.Length - this.offset );
        s.CopyTo ( 0, this.buffer, this.offset, num1 );
        this.offset += num1;
        if ( this.offset < this.buffer.Length ) {
          this.buffer[ this.offset++ ] = '\0';
        }
      }

      internal char[ ] buffer;
      internal int offset;
    }


    #region Constants
    /// <summary>
    /// Add a string
    /// </summary>
    public const int CB_ADDSTRING = 0x143;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_DELETESTRING = 0x144;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_ERR = -1;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_FINDSTRING = 0x14c;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_FINDSTRINGEXACT = 0x158;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_GETCURSEL = 0x147;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_GETDROPPEDSTATE = 0x157;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_GETEDITSEL = 320;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_GETITEMDATA = 0x150;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_GETITEMHEIGHT = 340;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_INSERTSTRING = 330;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_LIMITTEXT = 0x141;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_RESETCONTENT = 0x14b;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_SETCURSEL = 0x14e;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_SETDROPPEDWIDTH = 0x160;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_SETEDITSEL = 0x142;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_SETITEMHEIGHT = 0x153;
    /// <summary>
    /// 
    /// </summary>
    public const int CB_SHOWDROPDOWN = 0x14f;
    /// <summary>
    /// 
    /// </summary>
    public const int CBEN_ENDEDITA = -805;
    /// <summary>
    /// 
    /// </summary>
    public const int CBEN_ENDEDITW = -806;
    /// <summary>
    /// 
    /// </summary>
    public const int CBN_DBLCLK = 2;
    /// <summary>
    /// 
    /// </summary>
    public const int CBN_DROPDOWN = 7;
    /// <summary>
    /// 
    /// </summary>
    public const int CBN_EDITCHANGE = 5;
    /// <summary>
    /// 
    /// </summary>
    public const int CBN_SELCHANGE = 1;
    /// <summary>
    /// 
    /// </summary>
    public const int CBN_SELENDOK = 9;
    /// <summary>
    /// 
    /// </summary>
    public const int CBS_AUTOHSCROLL = 0x40;
    /// <summary>
    /// 
    /// </summary>
    public const int CBS_DROPDOWN = 2;
    /// <summary>
    /// 
    /// </summary>
    public const int CBS_DROPDOWNLIST = 3;
    /// <summary>
    /// 
    /// </summary>
    public const int CBS_HASSTRINGS = 0x200;
    /// <summary>
    /// 
    /// </summary>
    public const int CBS_NOINTEGRALHEIGHT = 0x400;
    /// <summary>
    /// 
    /// </summary>
    public const int CBS_OWNERDRAWFIXED = 0x10;
    /// <summary>
    /// 
    /// </summary>
    public const int CBS_OWNERDRAWVARIABLE = 0x20;
    /// <summary>
    /// 
    /// </summary>
    public const int CBS_SIMPLE = 1;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_ERASE = 4;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_HIDE = 0;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_INVALIDATE = 2;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_MAX = 10;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_MAXIMIZE = 3;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_MINIMIZE = 6;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_NORMAL = 1;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_RESTORE = 9;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_SCROLLCHILDREN = 1;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_SHOW = 5;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_SHOWMAXIMIZED = 3;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_SHOWMINIMIZED = 2;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_SHOWMINNOACTIVE = 7;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_SHOWNA = 8;
    /// <summary>
    /// 
    /// </summary>
    public const int SW_SHOWNOACTIVATE = 4;
    /// <summary>
    /// 
    /// </summary>
    public const int SWP_DRAWFRAME = 0x20;
    /// <summary>
    /// 
    /// </summary>
    public const int SWP_HIDEWINDOW = 0x80;
    /// <summary>
    /// 
    /// </summary>
    public const int SWP_NOACTIVATE = 0x10;
    /// <summary>
    /// 
    /// </summary>
    public const int SWP_NOMOVE = 2;
    /// <summary>
    /// 
    /// </summary>
    public const int SWP_NOSIZE = 1;
    /// <summary>
    /// 
    /// </summary>
    public const int SWP_NOZORDER = 4;
    /// <summary>
    /// 
    /// </summary>
    public const int SWP_SHOWWINDOW = 0x40;
    /// <summary>
    /// 
    /// </summary>
    public const string WC_DATETIMEPICK = "SysDateTimePick32";
    /// <summary>
    /// 
    /// </summary>
    public const string WC_LISTVIEW = "SysListView32";
    /// <summary>
    /// 
    /// </summary>
    public const string WC_MONTHCAL = "SysMonthCal32";
    /// <summary>
    /// 
    /// </summary>
    public const string WC_PROGRESS = "msctls_progress32";
    /// <summary>
    /// 
    /// </summary>
    public const string WC_STATUSBAR = "msctls_statusbar32";
    /// <summary>
    /// 
    /// </summary>
    public const string WC_TABCONTROL = "SysTabControl32";
    /// <summary>
    /// 
    /// </summary>
    public const string WC_TOOLBAR = "ToolbarWindow32";
    /// <summary>
    /// 
    /// </summary>
    public const string WC_TRACKBAR = "msctls_trackbar32";
    /// <summary>
    /// 
    /// </summary>
    public const string WC_TREEVIEW = "SysTreeView32";
    /// <summary>
    /// 
    /// </summary>
    public const int WS_BORDER = 0x800000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_CAPTION = 0xc00000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_CHILD = 0x40000000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_CLIPCHILDREN = 0x2000000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_CLIPSIBLINGS = 0x4000000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_DISABLED = 0x8000000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_DLGFRAME = 0x400000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_APPWINDOW = 0x40000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_CLIENTEDGE = 0x200;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_CONTEXTHELP = 0x400;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_CONTROLPARENT = 0x10000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_DLGMODALFRAME = 1;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_LAYERED = 0x80000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_LEFT = 0;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_LEFTSCROLLBAR = 0x4000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_MDICHILD = 0x40;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_RIGHT = 0x1000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_RTLREADING = 0x2000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_STATICEDGE = 0x20000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_TOOLWINDOW = 0x80;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_EX_TOPMOST = 8;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_HSCROLL = 0x100000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_MAXIMIZE = 0x1000000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_MAXIMIZEBOX = 0x10000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_MINIMIZE = 0x20000000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_MINIMIZEBOX = 0x20000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_OVERLAPPED = 0;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_POPUP = -2147483648;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_SYSMENU = 0x80000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_TABSTOP = 0x10000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_THICKFRAME = 0x40000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_VISIBLE = 0x10000000;
    /// <summary>
    /// 
    /// </summary>
    public const int WS_VSCROLL = 0x200000;
    /// <summary>
    /// 
    /// </summary>
    public const int WSF_VISIBLE = 1;
    #endregion
  }
}
