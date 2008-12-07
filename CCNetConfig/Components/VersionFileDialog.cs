/*
 * Copyright (c) 2006 - 2008, Ryan Conrad. All rights reserved.
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
using System.Drawing;
using System.Collections;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CCNetConfig.Core.Exceptions;

namespace CCNetConfig.Components {
  /// <summary>
  /// An Open/Save file dialog that requires the selection of a version.
  /// </summary>
  public sealed class VersionFileDialog : MarshalByRefObject, IDisposable {
    #region Public Events and Enums
    /// <summary>
    /// The type of dialog to display.
    /// </summary>
    public enum VersionFileDialogType {
      /// <summary>
      /// A Save File Dialog
      /// </summary>
      SaveFileDialog = 0,
      /// <summary>
      /// An Open File Dialog
      /// </summary>
      OpenFileDialog = 1,
    }

    /// <summary>
    /// Clients can implement handlers of this type to catch "selection changed" events
    /// </summary>
    public delegate void SelectionChangedHandler ( object sender, string path, out int newVersionIndex );

    /// <summary>
    /// This event is fired whenever the user selects an item in the dialog
    /// </summary>
    public event SelectionChangedHandler SelectionChanged;


    #endregion
    #region Private Members
    private OpenFileName openFileName;
    private NativeMethods.CharBuffer charBuffer = null;
    private List<Version> _versions = null;
    private VersionFileDialogType _fileDialogType = VersionFileDialogType.OpenFileDialog;
    private IntPtr _labelHandle = IntPtr.Zero;
    private IntPtr _comboHandle = IntPtr.Zero;
    private IntPtr _parentHWnd = IntPtr.Zero;
    private IWin32Window _ownerWindow = null;
    private string _filter = string.Empty;
    private string _defaultExt = string.Empty;
    private string _intialDirectory = string.Empty;
    private string[] _fileNames = null;
    private int _filterIndex = -1;
    private Screen _activeScreen;
    private int _selectedVersionIndex = -1;
    private bool _addExtension = false;
    private bool _checkFileExists = false;
    private bool _checkPathExists = false;
    private bool _enableSizing = true;
    private bool _dereferenceLinks = true;
    private bool _restoreDirectory = false;
    private bool _showHelp = false;
    private bool _supportMultiDottedExtensions = true;
    private bool _validateNames = true;
    private bool _allowMultiselect = false;
    private bool _createPrompt = false;
    private bool _overwritePrompt = true;
    private bool _noPlacesBar = false;
    private bool _versionRequired = true;
    private string _title = string.Empty;
    private string _okButtonText = string.Empty;
    private string _helpButtonText = "&Help";
    private string _cancelButtonText = "&Cancel";
    private string _fileNameLabelText = "File &name:";
    private string _fileTypeLabelText = "Files of &type:";
    private string _versionLabelText = "&Version:";
    private string _versionRequiredMessage = "Please Select a Version before Opening/Saving the file";

    private ButtonHook _openSaveButtonHook = null;
    #endregion
    #region Constructor
    /// <summary>
    /// Initializes a new instance of the <see cref="FileDialog"/> class.
    /// </summary>
    public VersionFileDialog () {
      _versions = new List<Version> ();
      _versions.Add ( new Version ( "1.0" ) );
      _versions.Add ( new Version ( "1.1" ) );
      _versions.Add ( new Version ( "1.1.1" ) );
      _versions.Add ( new Version ( "1.2" ) );
      _versions.Add ( new Version ( "1.2.1" ) );
      _versions.Add ( new Version ( "1.3" ) );
      _versions.Add(new Version("1.3.1"));
      _versions.Add(new Version("1.4"));
      _versions.Add(new Version("1.4.1"));
    }

    /// <summary>
    /// Releases unmanaged resources and performs other cleanup operations before the
    /// <see cref="VersionFileDialog"/> is reclaimed by garbage collection.
    /// </summary>
    ~VersionFileDialog () {
      this.Dispose ();
    }
    #endregion
    #region Public Properties
    /// <summary>
    /// Gets or sets the default extension.
    /// </summary>
    /// <value>The default extension.</value>
    public string DefaultExt { get { return _defaultExt; } set { _defaultExt = value; } }
    /// <summary>
    /// Gets or sets the filter.
    /// </summary>
    /// <value>The filter.</value>
    public string Filter {
      get { return _filter; }
      set {
        if ( string.Compare ( value, this._filter, true ) != 0 ) {
          if ( !string.IsNullOrEmpty ( value ) ) {
            string[] textArray1 = value.Split ( new char[] { '|' } );
            if ( ( textArray1 == null ) || ( ( textArray1.Length % 2 ) != 0 ) ) {
              throw new ArgumentException ( "Invalid Filter" );
            }
          } else {
            value = null;
          }
          this._filter = value;
        }

      }
    }
    /// <summary>
    /// Gets or sets a value indicating whether to add extension.
    /// </summary>
    /// <value><c>true</c> if [add extension]; otherwise, <c>false</c>.</value>
    public bool AddExtension { get { return this._addExtension; } set { this._addExtension = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether the dialog box displays a warning if the user specifies a file name that does not exist.
    /// </summary>
    /// <value><c>true</c> check if file exists; otherwise, <c>false</c>.</value>
    public bool CheckFileExists { get { return this._checkFileExists; } set { this._checkFileExists = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether path exists.
    /// </summary>
    /// <value><c>true</c> if [check path exists]; otherwise, <c>false</c>.</value>
    public bool CheckPathExists { get { return this._checkPathExists; } set { this._checkPathExists = value; } }
    /// <summary>
    /// Gets or sets the name of the file.
    /// </summary>
    /// <value>The name of the file.</value>
    public string FileName {
      get {
        if ( ( this._fileNames != null ) && ( this._fileNames[0].Length > 0 ) ) {
          return this._fileNames[0];
        }
        return string.Empty;
      }
      set {
        if ( value == null ) {
          this._fileNames = null;
        } else {
          this._fileNames = new string[] { value };
        }
      }
    }
    /// <summary>
    /// Gets or sets the index of the filter.
    /// </summary>
    /// <value>The index of the filter.</value>
    public int FilterIndex { get { return this._filterIndex; } set { this._filterIndex = value; } }
    /// <summary>
    /// Gets or sets the ok button text.
    /// </summary>
    /// <value>The ok button text.</value>
    public string OkButtonText {
      get { return this._okButtonText; }
      set { this._okButtonText = value; }
    }
    /// <summary>
    /// Gets or sets the cancel button text.
    /// </summary>
    /// <value>The cancel button text.</value>
    public string CancelButtonText {
      get { return this._cancelButtonText; }
      set { this._cancelButtonText = value; }
    }
    /// <summary>
    /// Gets or sets the help button text.
    /// </summary>
    /// <value>The button text.</value>
    public string HelpButtonText { get { return this._helpButtonText; } set { this._helpButtonText = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether [enable resizing].
    /// </summary>
    /// <value><c>true</c> if [enable resizing]; otherwise, <c>false</c>.</value>
    public bool EnableResizing { get { return this._enableSizing; } set { this._enableSizing = value; } }
    /// <summary>
    /// Gets or sets the version.
    /// </summary>
    /// <value>The version.</value>
    public Version SelectedVersion {
      get {
        if ( this._selectedVersionIndex >= 0 && this._selectedVersionIndex < this._versions.Count )
          return this._versions[this._selectedVersionIndex];
        else
          return null;
      }
      set { this._selectedVersionIndex = this._versions.IndexOf ( value ); }
    }
    /// <summary>
    /// Gets or sets the version list.
    /// </summary>
    /// <value>The version list.</value>
    public List<Version> VersionList { get { return this._versions; } set { this._versions = value; } }
    /// <summary>
    /// Gets or sets the file names.
    /// </summary>
    /// <value>The file names.</value>
    public string[] FileNames { get { return this._fileNames; } set { this._fileNames = value; } }
    /// <summary>
    /// Gets or sets the initial directory.
    /// </summary>
    /// <value>The initial directory.</value>
    public string InitialDirectory { get { return this._intialDirectory; } set { this._intialDirectory = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether [restore directory].
    /// </summary>
    /// <value><c>true</c> if [restore directory]; otherwise, <c>false</c>.</value>
    public bool RestoreDirectory { get { return this._restoreDirectory; } set { this._restoreDirectory = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether [show help].
    /// </summary>
    /// <value><c>true</c> if [show help]; otherwise, <c>false</c>.</value>
    public bool ShowHelp { get { return this._showHelp; } set { this._showHelp = value; } }
    /// <summary>
    /// Gets or sets whether the dialog box supports displaying and saving files that have multiple file name extensions.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if [support multi dotted extensions]; otherwise, <c>false</c>.
    /// </value>
    public bool SupportMultiDottedExtensions { get { return this._supportMultiDottedExtensions; } set { this._supportMultiDottedExtensions = value; } }
    /// <summary>
    /// Gets or sets the file dialog box title.
    /// </summary>
    /// <value>The title.</value>
    public string Title { get { return this._title; } set { this._title = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether the dialog box accepts only valid Win32 file names.
    /// </summary>
    /// <value><c>true</c> if [validate names]; otherwise, <c>false</c>.</value>
    public bool ValidateNames { get { return this._validateNames; } set { this._validateNames = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether [dereference links].
    /// </summary>
    /// <value><c>true</c> if [dereference links]; otherwise, <c>false</c>.</value>
    public bool DereferenceLinks { get { return this._dereferenceLinks; } set { this._dereferenceLinks = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether the Save As dialog box displays a warning if the user specifies a file name that already exists.
    /// </summary>
    /// <value><c>true</c> if [overwrite prompt]; otherwise, <c>false</c>.</value>
    public bool OverwritePrompt { get { return this._overwritePrompt && this.DialogType == VersionFileDialogType.SaveFileDialog; } set { this.OverwritePrompt = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether the dialog box prompts the user for permission to create a file if the user specifies a file that does not exist.
    /// </summary>
    /// <value><c>true</c> if [create prompt]; otherwise, <c>false</c>.</value>
    public bool CreatePrompt { get { return this._createPrompt && this.DialogType == VersionFileDialogType.SaveFileDialog; } set { this._createPrompt = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether the dialog box allows multiple files to be selected.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if [allow multiple file selection]; otherwise, <c>false</c>.
    /// </value>
    public bool AllowMultipleFileSelection {
      get { return this._allowMultiselect && this.DialogType == VersionFileDialogType.OpenFileDialog; }
      set { this._allowMultiselect = value; }
    }
    /// <summary>
    /// Gets or sets the file name label text.
    /// </summary>
    /// <value>The file name label text.</value>
    public string FileNameLabelText { get { return this._fileNameLabelText; } set { this._fileNameLabelText = value; } }
    /// <summary>
    /// Gets or sets the file type label text.
    /// </summary>
    /// <value>The file type label text.</value>
    public string FileTypeLabelText { get { return this._fileTypeLabelText; } set { this._fileTypeLabelText = value; } }
    /// <summary>
    /// Gets or sets the version label text.
    /// </summary>
    /// <value>The version label text.</value>
    public string VersionLabelText { get { return this._versionLabelText; } set { this._versionLabelText = value; } }
    /// <summary>
    /// Gets or sets the type of the dialog.
    /// </summary>
    /// <value>The type of the dialog.</value>
    public VersionFileDialogType DialogType { get { return this._fileDialogType; } set { this._fileDialogType = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether to show the places bar].
    /// </summary>
    /// <value><c>true</c> no places bar; otherwise, <c>false</c>.</value>
    public bool NoPlacesBar { get { return this._noPlacesBar; } set { this._noPlacesBar = value; } }
    /// <summary>
    /// Gets or sets a value indicating whether [version is required].
    /// </summary>
    /// <value><c>true</c> if [version is required]; otherwise, <c>false</c>.</value>
    public bool VersionIsRequired { get { return this._versionRequired; } set { this._versionRequired = value; } }
    /// <summary>
    /// Gets or sets the version required message.
    /// </summary>
    /// <value>The version required message.</value>
    public string VersionRequiredMessage { get { return this._versionRequiredMessage; } set { this._versionRequiredMessage = value; } }
    /// <summary>
    /// Gets or sets the version selected index.
    /// </summary>
    /// <value>The index of the version.</value>
    public int VersionIndex { get { return this._selectedVersionIndex; } set { this._selectedVersionIndex = value; } }
    #endregion
    #region Public Methods
    /// <summary>
    /// Runs a common dialog box with a default owner.
    /// </summary>
    /// <returns></returns>
    public DialogResult ShowDialog () {
      return this.ShowDialog ( null );
    }

    /// <summary>
    /// Runs a common dialog box with the specified owner.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <returns></returns>
    public DialogResult ShowDialog ( IWin32Window owner ) {
      return this.RunDialog ( owner );
    }
    #endregion
    #region Private Methods
    /// <summary>
    /// Gets the window client rect.
    /// </summary>
    /// <param name="hWnd">The hWND.</param>
    /// <param name="parentHWnd">The parent HWND.</param>
    /// <returns></returns>
    private RECT GetWindowClientRect ( IntPtr hWnd, IntPtr parentHWnd ) {
      if ( hWnd == IntPtr.Zero )
        return new RECT ();

      RECT rect = new RECT ();
      NativeMethods.GetWindowRect ( hWnd, ref rect );
      POINT pt1 = new POINT ();
      pt1.X = rect.Left;
      pt1.Y = rect.Bottom;
      NativeMethods.ScreenToClient ( parentHWnd, ref pt1 );

      POINT pt2 = new POINT ();
      pt2.X = rect.Right;
      pt2.Y = rect.Top;

      NativeMethods.ScreenToClient ( parentHWnd, ref pt2 );
      rect = new RECT ();
      rect.Bottom = pt1.Y;
      rect.Left = pt1.X;
      rect.Top = pt2.Y;
      rect.Right = pt2.X;
      return rect;
    }

    /// <summary>
    /// Sets the window item text.
    /// </summary>
    /// <param name="hWnd">The h WND.</param>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    private bool SetWindowItemText ( IntPtr hWnd, string text ) {
      return NativeMethods.SetWindowText ( hWnd, text );
    }

    /// <summary>
    /// Sets the dialog item caption.
    /// </summary>
    /// <param name="control">The control.</param>
    /// <param name="text">The text.</param>
    /// <returns></returns>
    private bool SetDialogItemCaption ( UInt32 control, string text ) {
      IntPtr ctrl = NativeMethods.GetDlgItem ( this._parentHWnd, control );
      if ( ctrl != IntPtr.Zero )
        return SetWindowItemText ( ctrl, text );
      else
        return false;
    }


    /// <summary>
    /// Gets the multiselect files.
    /// </summary>
    /// <param name="charBuffer">The char buffer.</param>
    /// <returns></returns>
    private string[] GetMultiselectFiles ( NativeMethods.CharBuffer charBuffer ) {
      string text1 = charBuffer.GetString ();
      string text2 = charBuffer.GetString ();
      if ( text2.Length == 0 ) {
        return new string[] { text1 };
      }
      if ( text1[text1.Length - 1] != '\\' ) {
        text1 = text1 + @"\";
      }
      ArrayList list1 = new ArrayList ();
      while ( true ) {
        if ( ( text2[0] != '\\' ) && ( ( ( text2.Length <= 3 ) || ( text2[1] != ':' ) ) || ( text2[2] != '\\' ) ) ) {
          text2 = text1 + text2;
        }
        list1.Add ( text2 );
        text2 = charBuffer.GetString ();
        if ( text2.Length <= 0 ) {
          string[] textArray1 = new string[list1.Count];
          list1.CopyTo ( textArray1, 0 );
          return textArray1;
        }
      }
    }

    /// <summary>
    /// Runs the dialog.
    /// </summary>
    /// <param name="owner">The owner.</param>
    /// <returns></returns>
    private DialogResult RunDialog ( IWin32Window owner ) {
      _ownerWindow = owner;
      InitializeDialog ();
      //show the dialog
      bool result = this.DialogType == VersionFileDialogType.OpenFileDialog ? NativeMethods.GetOpenFileName ( ref openFileName ) : NativeMethods.GetSaveFileName ( ref openFileName );

      if ( !result ) {
        int ret = NativeMethods.CommDlgExtendedError ();
        if ( ret != 0 )
          throw new ApplicationException ( "Couldn't show file open dialog - " + ret.ToString () );
        return DialogResult.Cancel;
      }

      if ( _selectedVersionIndex < 0 && this.VersionIsRequired )
        throw new VersionNotSelectedException ();

      this.charBuffer = null;
      if ( openFileName.lpstrFile != IntPtr.Zero ) {
        Marshal.FreeCoTaskMem ( openFileName.lpstrFile );
      }

      return DialogResult.OK;
    }

    /// <summary>
    /// Initializes the dialog.
    /// </summary>
    private void InitializeDialog () {
      openFileName = new OpenFileName ();
      this.charBuffer = NativeMethods.CharBuffer.CreateBuffer ( 0x2000 );

      openFileName.lStructSize = Marshal.SizeOf ( openFileName );
      openFileName.lpstrFilter = Filter.Replace ( '|', '\0' ) + '\0';

      openFileName.lpstrFile = this.charBuffer.AllocCoTaskMem ();
      openFileName.nMaxFile = 0x2000;
      if ( _fileNames != null && _fileNames.Length > 0 && !string.IsNullOrEmpty ( _fileNames[0] ) )
        openFileName.lpstrFileTitle = System.IO.Path.GetFileName ( _fileNames[0] ) + new string ( '\0', 512 );
      else
        openFileName.lpstrFileTitle = new string ( '\0', 512 );
      openFileName.nMaxFileTitle = openFileName.lpstrFileTitle.Length;

      if ( this.DialogType == VersionFileDialogType.OpenFileDialog && string.IsNullOrEmpty ( this._title ) )
        openFileName.lpstrTitle = "Open File";
      else if ( this.DialogType == VersionFileDialogType.SaveFileDialog && string.IsNullOrEmpty ( this._title ) )
        openFileName.lpstrTitle = "Save File As...";
      else
        openFileName.lpstrTitle = this._title;

      if ( this.DialogType == VersionFileDialogType.OpenFileDialog && string.IsNullOrEmpty ( this.OkButtonText ) )
        this.OkButtonText = "&Open";
      else if ( this.DialogType == VersionFileDialogType.SaveFileDialog && string.IsNullOrEmpty ( this.OkButtonText ) )
        this.OkButtonText = "&Save";

      if ( string.IsNullOrEmpty ( this.CancelButtonText ) )
        this.CancelButtonText = "&Cancel";

      openFileName.lpstrDefExt = _defaultExt;

      //position the dialog above the active window
      openFileName.hwndOwner = _ownerWindow != null ? _ownerWindow.Handle : IntPtr.Zero;
      //we need to find out the active screen so the dialog box is
      //centred on the correct display
      _activeScreen = Screen.FromControl ( Form.ActiveForm );

      SetDialogFlags ();
      //this is where the hook is set. Note that we can use a C# delegate in place of a C function pointer
      openFileName.lpfnHook = new WndHookProc ( HookProc );

      //if we're running on Windows 98/ME then the struct is smaller
      if ( System.Environment.OSVersion.Platform != PlatformID.Win32NT ) {
        openFileName.lStructSize -= 12;
      }
    }

    /// <summary>
    /// Sets the dialog flags.
    /// </summary>
    private void SetDialogFlags () {
      //set up some sensible flags
      openFileName.Flags = OpenFileNameFlags.Explorer | OpenFileNameFlags.LongNames |
        OpenFileNameFlags.EnableHook | OpenFileNameFlags.HideReadOnly |
        OpenFileNameFlags.EnableIncludeNotify;

      if ( this.CheckFileExists )
        openFileName.Flags |= OpenFileNameFlags.FileMustExist;

      if ( this.CheckPathExists )
        openFileName.Flags |= OpenFileNameFlags.PathMustExist;

      if ( this.EnableResizing )
        openFileName.Flags |= OpenFileNameFlags.EnableSizing;

      if ( this.AddExtension )
        openFileName.Flags |= OpenFileNameFlags.ExtensionDifferent;

      if ( !this.DereferenceLinks )
        openFileName.Flags |= OpenFileNameFlags.NoDereferenceLinks;

      if ( this.DialogType == VersionFileDialogType.SaveFileDialog )
        openFileName.Flags |= OpenFileNameFlags.OverWritePrompt;

      if ( this.ShowHelp )
        openFileName.Flags |= OpenFileNameFlags.ShowHelp;

      if ( this.OverwritePrompt )
        openFileName.Flags |= OpenFileNameFlags.OverWritePrompt;

      if ( this.RestoreDirectory )
        openFileName.Flags |= OpenFileNameFlags.NoChangeDir;

      if ( !this.ValidateNames )
        openFileName.Flags |= OpenFileNameFlags.NoValidate;

      if ( this.CreatePrompt )
        openFileName.Flags |= OpenFileNameFlags.CreatePrompt;

      if ( this.AllowMultipleFileSelection )
        openFileName.Flags |= OpenFileNameFlags.AllowMultiSelect;

      if ( this.NoPlacesBar )
        openFileName.FlagsEx = OpenFileNameFlagsEx.NoPlacesBar;

    }

    /// <summary>
    /// Hooks the proc.
    /// </summary>
    /// <param name="hdlg">The HDLG.</param>
    /// <param name="msg">The MSG.</param>
    /// <param name="wParam">The w param.</param>
    /// <param name="lParam">The l param.</param>
    /// <returns></returns>
    private IntPtr HookProc ( IntPtr hdlg, UInt16 msg, Int32 wParam, Int32 lParam ) {
      _parentHWnd = NativeMethods.GetParent ( hdlg );
      switch ( msg ) {
        case WindowMessage.InitDialog:
          SetDialogItemCaption ( FileDialogControls.CancelButton, this.CancelButtonText );
          SetDialogItemCaption ( FileDialogControls.HelpButton, this.HelpButtonText );
          SetDialogItemCaption ( FileDialogControls.OpenSaveButton, this.OkButtonText );
          SetDialogItemCaption ( FileDialogControls.FileNameLabel, this.FileNameLabelText );
          SetDialogItemCaption ( FileDialogControls.FileOfTypeLabel, this.FileTypeLabelText );
          CenterDialogWindow ();
          CreateVersionComboBox ();
          HookOpenSaveButton ();
          break;
        case WindowMessage.Destroy:
          this.Dispose ();
          break;
        case WindowMessage.Notify:
          return HandleNotifyMessage ( lParam );
        case WindowMessage.Size:
          break;
      }
      return IntPtr.Zero;
    }

    /// <summary>
    /// Hooks the open/save button.
    /// </summary>
    private void HookOpenSaveButton () {
      IntPtr openSaveButton = NativeMethods.GetDlgItem ( _parentHWnd, FileDialogControls.OpenSaveButton );
      if ( openSaveButton != IntPtr.Zero ) {
        _openSaveButtonHook = new ButtonHook ( openSaveButton );
        _openSaveButtonHook.Click += delegate ( object sender, CancelEventArgs e ) {
          _selectedVersionIndex = (int)NativeMethods.SendMessage ( _comboHandle, NativeMethods.CB_GETCURSEL, 0, 0 );
          if ( this._selectedVersionIndex <= -1 && this.VersionIsRequired ) {
            MessageBox.Show ( this.VersionRequiredMessage, "Select Version", MessageBoxButtons.OK, MessageBoxIcon.Stop );
            e.Cancel = true;
          } else {
            e.Cancel = false;
          }
        };
      }
    }

    /// <summary>
    /// Handles the notify message.
    /// </summary>
    /// <param name="lParam">The l param.</param>
    private IntPtr HandleNotifyMessage ( Int64 lParam ) {
      //we need to intercept the CDN_FILEOK message
      //which is sent when the user selects a filename
      IntPtr ipNotify = new IntPtr ( lParam );
      OfNotify ofNot = (OfNotify)Marshal.PtrToStructure ( ipNotify, typeof ( OfNotify ) );
      int code = ofNot.hdr.code;
      Console.WriteLine(ofNot.hdr.code.ToString());
      switch ( code ) {
        case CommonDlgNotification.FileOk:
          _selectedVersionIndex = (int)NativeMethods.SendMessage ( _comboHandle, NativeMethods.CB_GETCURSEL, 0, 0 );
          this.charBuffer.PutCoTaskMem ( openFileName.lpstrFile );
          if ( !this.AllowMultipleFileSelection ) {
            this._fileNames = new string[] { this.charBuffer.GetString () };
          } else {
            this._fileNames = this.GetMultiselectFiles ( this.charBuffer );
          }
          break;
        case CommonDlgNotification.FolderChange:
          break;
        case CommonDlgNotification.Help:
          break;
        case CommonDlgNotification.IncludeItem:
          break;
        case CommonDlgNotification.InitDone:
          break;
        case CommonDlgNotification.SelChange:
          // gets the selected file name
          StringBuilder pathBuffer = new StringBuilder ( 260 );
          UInt32 ret = NativeMethods.SendMessage ( _parentHWnd, CommonDlgMessage.GetFilePath, 260, pathBuffer );
          string path = pathBuffer.ToString ();

          // raise the selection changed event.
          int nvi = this.VersionIndex;
          if (SelectionChanged != null)
            this.SelectionChanged ( this, path, out nvi );
          // set the version index from the value set in the handler
          NativeMethods.SendMessage ( _comboHandle, NativeMethods.CB_SETCURSEL, (uint)nvi, (uint)0 );
          break;
        case CommonDlgNotification.ShareViolation:
          break;
        case CommonDlgNotification.TypeChange:
          break;
      }
      return IntPtr.Zero;
    }

    /// <summary>
    /// Creates the version combo box.
    /// </summary>
    private void CreateVersionComboBox () {
      //we need to find the label to position our new label under
      IntPtr fileTypeWindow = NativeMethods.GetDlgItem ( _parentHWnd, FileDialogControls.FileOfTypeLabel );
      RECT aboveRect = GetWindowClientRect ( fileTypeWindow, _parentHWnd );
      //create the label
      IntPtr labelHandle = NativeMethods.CreateWindowEx ( 0, "STATIC", "versionLabel",
        NativeMethods.WS_VISIBLE | NativeMethods.WS_CHILD | NativeMethods.WS_TABSTOP,
        aboveRect.Left, aboveRect.Bottom + 12, 200, 32, _parentHWnd, 0, 0, 0 );
      NativeMethods.SetWindowText ( labelHandle, this.VersionLabelText );

      UInt32 fontHandle = NativeMethods.SendMessage ( fileTypeWindow, WindowMessage.GetFont, 0, 0 );
      NativeMethods.SendMessage ( labelHandle, WindowMessage.SetFont, fontHandle, (Int32)0 );

      //we now need to find the combo-box to position the new combo-box under
      IntPtr fileComboWindow = NativeMethods.GetDlgItem ( _parentHWnd, FileDialogControls.FileOfTypeComboBox );
      aboveRect = GetWindowClientRect ( fileComboWindow, _parentHWnd );
      //we create the new combobox
      IntPtr comboHandle = NativeMethods.CreateWindowEx ( 0, "ComboBox", "versionCombobox",
        ( NativeMethods.WS_VISIBLE | NativeMethods.WS_CHILD | NativeMethods.CBS_HASSTRINGS |
        NativeMethods.CBS_DROPDOWNLIST | NativeMethods.WS_TABSTOP ),
        aboveRect.Left, aboveRect.Bottom + 8,aboveRect.Right - aboveRect.Left,
        150, _parentHWnd, 0, 0, 0 );
      NativeMethods.SendMessage ( comboHandle, WindowMessage.SetFont, fontHandle, 0 );

      //and add the versions we want to offer
      foreach ( Version ver in this._versions )
        NativeMethods.SendMessage ( comboHandle, NativeMethods.CB_ADDSTRING, 0, new StringBuilder ( ver.ToString () ) );

      NativeMethods.SendMessage ( comboHandle, NativeMethods.CB_SETCURSEL, (uint)this.VersionIndex, 0 );

      //remember the handles of the controls we have created so we can destroy them after
      _labelHandle = labelHandle;
      _comboHandle = comboHandle;

      SetFilterSelectedIndex ();
    }

    /// <summary>
    /// Centers the dialog window.
    /// </summary>
    private void CenterDialogWindow () {
      //we need to centre the dialog
      Rectangle sr = _activeScreen != null ? _activeScreen.Bounds : Screen.PrimaryScreen.Bounds;
      RECT cr = new RECT ();
      NativeMethods.GetWindowRect ( _parentHWnd, ref cr );
      int x = ( sr.Right + sr.Left - ( cr.Right - cr.Left ) ) / 2;
      int y = ( sr.Bottom + sr.Top - ( cr.Bottom - cr.Top ) ) / 2;
      NativeMethods.SetWindowPos ( _parentHWnd, IntPtr.Zero, x, y, cr.Right - cr.Left, cr.Bottom - cr.Top + 32, NativeMethods.SWP_NOZORDER );
    }

    /// <summary>
    /// Sets the index of the filter selected.
    /// </summary>
    private void SetFilterSelectedIndex () {
      IntPtr filterCombo = NativeMethods.GetDlgItem ( _parentHWnd, FileDialogControls.FileOfTypeComboBox );
      if ( filterCombo != IntPtr.Zero )
        NativeMethods.SendMessage ( filterCombo, NativeMethods.CB_SETCURSEL, (uint)this.FilterIndex, 0 );
    }

    #endregion
    #region IDisposable Members
    /// <summary>
    /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
    /// </summary>
    public void Dispose () {
      if ( this._openSaveButtonHook != null && this._openSaveButtonHook.Handle != IntPtr.Zero )
        _openSaveButtonHook.Dispose ();

      //destroy the handles we have created
      if ( _comboHandle != IntPtr.Zero )
        NativeMethods.DestroyWindow ( _comboHandle );
      if ( _labelHandle != IntPtr.Zero )
        NativeMethods.DestroyWindow ( _labelHandle );
    }
    #endregion

    #region ButtonHook
    /// <summary>
    /// Hooks to the click event of a button
    /// </summary>
    internal class ButtonHook : NativeWindow, IDisposable {
      /// <summary>
      /// The Click event of the button
      /// </summary>
      public event CancelEventHandler Click;
      /// <summary>
      /// Initializes a new instance of the <see cref="ButtonHook"/> class.
      /// </summary>
      /// <param name="hWnd">The h WND.</param>
      public ButtonHook ( IntPtr hWnd ) {
        this.AssignHandle ( hWnd );
      }
      /// <summary>
      /// Invokes the default window procedure associated with this window.
      /// </summary>
      /// <param name="m">A <see cref="T:System.Windows.Forms.Message"></see> that is associated with the current Windows message.</param>
      protected override void WndProc ( ref Message m ) {
        switch ( m.Msg ) {
          case WindowMessage.LeftButtonDown:
          case WindowMessage.LeftButtonDblClick:
            CancelEventArgs evt = new CancelEventArgs ();
            OnClick ( evt );
            if ( evt.Cancel )
              return;
            break;
        }
        base.WndProc ( ref m );
      }

      /// <summary>
      /// Raises the <see cref="E:Click"/> event.
      /// </summary>
      /// <param name="e">The <see cref="System.ComponentModel.CancelEventArgs"/> instance containing the event data.</param>
      protected virtual void OnClick ( CancelEventArgs e ) {
        if ( this.Click != null )
          this.Click ( this, e );
      }
      #region IDisposable Members

      /// <summary>
      /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
      /// </summary>
      public void Dispose () {
        this.ReleaseHandle ();
      }

      #endregion
    }
    #endregion
  }
}
