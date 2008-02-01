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
using System.Windows.Forms;
using System.IO;
using System.Xml.Serialization;
using System.Drawing;
using Microsoft.Win32;
using System.Runtime.InteropServices;
using System.Windows.Forms.VisualStyles;
using System.Reflection;
using System.ComponentModel;

namespace CCNetConfig.Controls {
  /// <summary>
  /// Extends the <see cref="System.Windows.Forms.PropertyGrid">PropertyGrid</see>
  /// </summary>
  public class PropertyGridEx : PropertyGrid {
    // Internal PropertyGrid Controls
    private object _propertyGridView;
    private object _hotCommands;
    private object _helpComment;

    private ToolStrip _toolstrip = null;
    private Label _helpCommentTitle = null;
    private Label _helpCommentDescription = null;
    private FieldInfo _propertyGridEntries = null;
    // Properties variables
    private bool _autoSizeProperties = true;
    private bool _drawFlatToolbar = true;

    /// <summary>
    /// Initializes a new instance of the <see cref="PropertyGridEx"/> class.
    /// </summary>
    public PropertyGridEx () {
      // Add any initialization after the InitializeComponent() call.
      SetStyle ( ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true );

      _propertyGridView = base.GetType ().BaseType.InvokeMember ( "gridView", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, this, null );
      _hotCommands = base.GetType ().BaseType.InvokeMember ( "hotcommands", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, this, null );
      _toolstrip = (ToolStrip)base.GetType ().BaseType.InvokeMember ( "toolStrip", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, this, null );
      _helpComment = base.GetType ().BaseType.InvokeMember ( "doccomment", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, this, null );
      if ( _helpComment != null ) {
        _helpCommentTitle = (Label)_helpComment.GetType ().InvokeMember ( "m_labelTitle", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, _helpComment, null );
        _helpCommentDescription = (Label)_helpComment.GetType ().InvokeMember ( "m_labelDesc", BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, _helpComment, null );
      }

      if ( _propertyGridView != null ) {
        _propertyGridEntries = _propertyGridView.GetType ().GetField ( "allGridEntries", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly );
      }

      // Apply Toolstrip style
      if ( _toolstrip != null ) {
        ApplyToolStripRenderMode ( _drawFlatToolbar );
      }
    }

    /// <summary>
    /// Moves the splitter to.
    /// </summary>
    /// <param name="x">The x.</param>
    public void MoveSplitterTo ( int x ) {
      _propertyGridView.GetType ().InvokeMember ( "MoveSplitterTo", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.Instance, null, _propertyGridView, new object[] { x } );
    }

    /// <summary>
    /// </summary>
    public override void Refresh () {
      base.Refresh ();
      if ( _autoSizeProperties ) {
        AutoSizeSplitter ( 32 );
      }
    }

    /// <summary>
    /// Applies the tool strip render mode.
    /// </summary>
    /// <param name="drawFlatToolbar">if set to <c>true</c> [draw flat toolbar].</param>
    protected void ApplyToolStripRenderMode ( bool drawFlatToolbar ) {
      if ( drawFlatToolbar ) {
        _toolstrip.Renderer = new ToolStripSystemRenderer ();
      } else {
        ToolStripProfessionalRenderer renderer = new ToolStripProfessionalRenderEx ();
        renderer.RoundedEdges = false;
        _toolstrip.Renderer = renderer;
      }
    }
    /// <summary>
    /// Raises the <see cref="E:System.Windows.Forms.Control.Resize"></see> event.
    /// </summary>
    /// <param name="e"></param>
    protected override void OnResize ( System.EventArgs e ) {
      base.OnResize ( e );
      if ( _autoSizeProperties ) {
        AutoSizeSplitter ( 32 );
      }
    }

    /// <summary>
    /// Autosizes splitter.
    /// </summary>
    /// <param name="RightMargin">The right margin.</param>
    protected void AutoSizeSplitter ( int RightMargin ) {

      GridItemCollection itemCollection = (System.Windows.Forms.GridItemCollection)_propertyGridEntries.GetValue ( _propertyGridView );
      if ( itemCollection == null ) {
        return;
      }
      System.Drawing.Graphics g = System.Drawing.Graphics.FromHwnd ( this.Handle );
      int CurWidth = 0;
      int MaxWidth = 0;

      foreach ( GridItem item in itemCollection ) {
        if ( item.GridItemType == GridItemType.Property ) {
          CurWidth = (int)g.MeasureString ( item.Label, this.Font ).Width + RightMargin;
          if ( CurWidth > MaxWidth ) {
            MaxWidth = CurWidth;
          }
        }
      }

      MoveSplitterTo ( MaxWidth );
    }

    #region Properties

    /// <summary>
    /// Gets or sets a value indicating whether [auto size properties].
    /// </summary>
    /// <value><c>true</c> if [auto size properties]; otherwise, <c>false</c>.</value>
    [Category ( "Behavior" ), DefaultValue ( false ), DescriptionAttribute ( "Move automatically the splitter to better fit all the properties shown." )]
    public bool AutoSizeProperties {
      get {
        return _autoSizeProperties;
      }
      set {
        _autoSizeProperties = value;
        if ( value )
          AutoSizeSplitter ( 32 );
      }
    }


    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="T:System.Windows.Forms.PropertyGrid"></see> control paints its toolbar with flat buttons.
    /// </summary>
    /// <value></value>
    /// <returns>true if the <see cref="T:System.Windows.Forms.PropertyGrid"></see> paints its toolbar with flat buttons; otherwise false. The default is false.</returns>
    [Category ( "Appearance" ), DefaultValue ( false ), DescriptionAttribute ( "Draw a flat toolbar" )]
    public new bool DrawFlatToolbar {
      get {
        return _drawFlatToolbar;
      }
      set {
        _drawFlatToolbar = value;
        ApplyToolStripRenderMode ( _drawFlatToolbar );
      }
    }

    /// <summary>
    /// Gets the tool strip.
    /// </summary>
    /// <value>The tool strip.</value>
    [Category ( "Appearance" ), DisplayName ( "Toolstrip" ), DesignerSerializationVisibility ( DesignerSerializationVisibility.Content ), DescriptionAttribute ( "Toolbar object" ), Browsable ( true )]
    public ToolStrip ToolStrip {
      get {
        return _toolstrip;
      }
    }

    /// <summary>
    /// Gets the help comment.
    /// </summary>
    /// <value>The help comment.</value>
    [Category ( "Appearance" ), DisplayName ( "Help" ), DescriptionAttribute ( "HelpComment object. Represent the comments area of the PropertyGrid." ), DesignerSerializationVisibility ( DesignerSerializationVisibility.Hidden ), Browsable ( false )]
    public Control HelpComment {
      get {
        return (System.Windows.Forms.Control)_helpComment;
      }
    }

    /// <summary>
    /// Gets the help comment title.
    /// </summary>
    /// <value>The help comment title.</value>
    [Category ( "Appearance" ), DisplayName ( "HelpTitle" ), DesignerSerializationVisibility ( DesignerSerializationVisibility.Content ), DescriptionAttribute ( "Help Title Label." ), Browsable ( true )]
    public Label HelpCommentTitle {
      get {
        return _helpCommentTitle;
      }
    }

    /// <summary>
    /// Gets the help comment description.
    /// </summary>
    /// <value>The help comment description.</value>
    [Category ( "Appearance" ), DisplayName ( "HelpDescription" ), DesignerSerializationVisibility ( DesignerSerializationVisibility.Content ), DescriptionAttribute ( "Help Description Label." ), Browsable ( true )]
    public Label HelpCommentDescription {
      get {
        return _helpCommentDescription;
      }
    }

    /// <summary>
    /// Gets or sets the help comment image.
    /// </summary>
    /// <value>The help comment image.</value>
    [Category ( "Appearance" ), DisplayName ( "HelpImageBackground" ), DescriptionAttribute ( "Help Image Background." )]
    public Image HelpCommentImage {
      get {
        return ( (Control)_helpComment ).BackgroundImage;
      }
      set {
        ( (Control)_helpComment ).BackgroundImage = value;
      }
    }

    #endregion
    /// <summary>
    /// Extends the ToolStripProfessionalRender
    /// </summary>
    public sealed class ToolStripProfessionalRenderEx : ToolStripProfessionalRenderer {
      /// <summary>
      /// Initializes a new instance of the <see cref="ToolStripProfessionalRenderEx"/> class.
      /// </summary>
      public ToolStripProfessionalRenderEx () : base ( new TanColorTable () ) {
        this.RoundedEdges = true;
      }

      /// <summary>
      /// Initializes a new instance of the <see cref="ToolStripProfessionalRenderEx"/> class.
      /// </summary>
      /// <param name="colorTable">The color table.</param>
      public ToolStripProfessionalRenderEx ( ProfessionalColorTable colorTable ) : base ( colorTable ) {
        this.RoundedEdges = true;
      }
    }

    /// <summary>
    /// Extends the ProfessionalColorTable
    /// </summary>
    public sealed class TanColorTable  : ProfessionalColorTable {
      /// <summary>
      /// Initializes a new instance of the <see cref="TanColorTable"/> class.
      /// </summary>
      public TanColorTable () {

      }

      /// <summary>
      /// Froms the color of the known.
      /// </summary>
      /// <param name="color">The color.</param>
      /// <returns></returns>
      internal Color FromKnownColor ( TanColorTable.KnownColors color ) {
        return (Color)this.ColorTable[color];
      }

      /// <summary>
      /// Inits the tan luna colors.
      /// </summary>
      /// <param name="rgbTable">The RGB table.</param>
      internal static void InitTanLunaColors ( ref Dictionary<TanColorTable.KnownColors, Color> rgbTable ) {
        rgbTable[TanColorTable.KnownColors.GripDark] = Color.FromArgb ( 0xc1, 190, 0xb3 );
        rgbTable[TanColorTable.KnownColors.SeparatorDark] = Color.FromArgb ( 0xc5, 0xc2, 0xb8 );
        rgbTable[TanColorTable.KnownColors.MenuItemSelected] = Color.FromArgb ( 0xc1, 210, 0xee );
        rgbTable[TanColorTable.KnownColors.ButtonPressedBorder] = Color.FromArgb ( 0x31, 0x6a, 0xc5 );
        rgbTable[TanColorTable.KnownColors.CheckBackground] = Color.FromArgb ( 0xe1, 230, 0xe8 );
        rgbTable[TanColorTable.KnownColors.MenuItemBorder] = Color.FromArgb ( 0x31, 0x6a, 0xc5 );
        rgbTable[TanColorTable.KnownColors.CheckBackgroundMouseOver] = Color.FromArgb ( 0x31, 0x6a, 0xc5 );
        rgbTable[TanColorTable.KnownColors.MenuItemBorderMouseOver] = Color.FromArgb ( 0x4b, 0x4b, 0x6f );
        rgbTable[TanColorTable.KnownColors.ToolStripDropDownBackground] = Color.FromArgb ( 0xfc, 0xfc, 0xf9 );
        rgbTable[TanColorTable.KnownColors.MenuBorder] = Color.FromArgb ( 0x8a, 0x86, 0x7a );
        rgbTable[TanColorTable.KnownColors.SeparatorLight] = Color.FromArgb ( 0xff, 0xff, 0xff );
        rgbTable[TanColorTable.KnownColors.ToolStripBorder] = Color.FromArgb ( 0xa3, 0xa3, 0x7c );
        rgbTable[TanColorTable.KnownColors.MenuStripGradientBegin] = Color.FromArgb ( 0xe5, 0xe5, 0xd7 );
        rgbTable[TanColorTable.KnownColors.MenuStripGradientEnd] = Color.FromArgb ( 0xf4, 0xf2, 0xe8 );
        rgbTable[TanColorTable.KnownColors.ImageMarginGradientBegin] = Color.FromArgb ( 0xfe, 0xfe, 0xfb );
        rgbTable[TanColorTable.KnownColors.ImageMarginGradientMiddle] = Color.FromArgb ( 0xec, 0xe7, 0xe0 );
        rgbTable[TanColorTable.KnownColors.ImageMarginGradientEnd] = Color.FromArgb ( 0xbd, 0xbd, 0xa3 );
        rgbTable[TanColorTable.KnownColors.OverflowButtonGradientBegin] = Color.FromArgb ( 0xf3, 0xf2, 240 );
        rgbTable[TanColorTable.KnownColors.OverflowButtonGradientMiddle] = Color.FromArgb ( 0xe2, 0xe1, 0xdb );
        rgbTable[TanColorTable.KnownColors.OverflowButtonGradientEnd] = Color.FromArgb ( 0x92, 0x92, 0x76 );
        rgbTable[TanColorTable.KnownColors.MenuItemPressedGradientBegin] = Color.FromArgb ( 0xfc, 0xfc, 0xf9 );
        rgbTable[TanColorTable.KnownColors.MenuItemPressedGradientEnd] = Color.FromArgb ( 0xf6, 0xf4, 0xec );
        rgbTable[TanColorTable.KnownColors.ImageMarginRevealedGradientBegin] = Color.FromArgb ( 0xf7, 0xf6, 0xef );
        rgbTable[TanColorTable.KnownColors.ImageMarginRevealedGradientMiddle] = Color.FromArgb ( 0xf2, 240, 0xe4 );
        rgbTable[TanColorTable.KnownColors.ImageMarginRevealedGradientEnd] = Color.FromArgb ( 230, 0xe3, 210 );
        rgbTable[TanColorTable.KnownColors.ButtonCheckedGradientBegin] = Color.FromArgb ( 0xe1, 230, 0xe8 );
        rgbTable[TanColorTable.KnownColors.ButtonCheckedGradientMiddle] = Color.FromArgb ( 0xe1, 230, 0xe8 );
        rgbTable[TanColorTable.KnownColors.ButtonCheckedGradientEnd] = Color.FromArgb ( 0xe1, 230, 0xe8 );
        rgbTable[TanColorTable.KnownColors.ButtonSelectedGradientBegin] = Color.FromArgb ( 0xc1, 210, 0xee );
        rgbTable[TanColorTable.KnownColors.ButtonSelectedGradientMiddle] = Color.FromArgb ( 0xc1, 210, 0xee );
        rgbTable[TanColorTable.KnownColors.ButtonSelectedGradientEnd] = Color.FromArgb ( 0xc1, 210, 0xee );
        rgbTable[TanColorTable.KnownColors.ButtonPressedGradientBegin] = Color.FromArgb ( 0x98, 0xb5, 0xe2 );
        rgbTable[TanColorTable.KnownColors.ButtonPressedGradientMiddle] = Color.FromArgb ( 0x98, 0xb5, 0xe2 );
        rgbTable[TanColorTable.KnownColors.ButtonPressedGradientEnd] = Color.FromArgb ( 0x98, 0xb5, 0xe2 );
        rgbTable[TanColorTable.KnownColors.GripLight] = Color.FromArgb ( 0xff, 0xff, 0xff );
      }

      /// <summary>
      /// Gets the starting color of the gradient used when the button is checked.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used when the button is checked.</returns>
      public override Color ButtonCheckedGradientBegin {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonCheckedGradientBegin );
          }
          return base.ButtonCheckedGradientBegin;
        }
      }

      /// <summary>
      /// Gets the end color of the gradient used when the button is checked.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used when the button is checked.</returns>
      public override Color ButtonCheckedGradientEnd {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonCheckedGradientEnd );
          }
          return base.ButtonCheckedGradientEnd;
        }
      }

      /// <summary>
      /// Gets the middle color of the gradient used when the button is checked.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used when the button is checked.</returns>
      public override Color ButtonCheckedGradientMiddle {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonCheckedGradientMiddle );
          }
          return base.ButtonCheckedGradientMiddle;
        }
      }


      /// <summary>
      /// Gets the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientBegin"></see>, <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientMiddle"></see>, and <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientEnd"></see> colors.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientBegin"></see>, <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientMiddle"></see>, and <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonPressedGradientEnd"></see> colors.</returns>
      public override Color ButtonPressedBorder {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonPressedBorder );
          }
          return base.ButtonPressedBorder;
        }
      }

      /// <summary>
      /// Gets the starting color of the gradient used when the button is pressed.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used when the button is pressed.</returns>
      public override Color ButtonPressedGradientBegin {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonPressedGradientBegin );
          }
          return base.ButtonPressedGradientBegin;
        }
      }

      /// <summary>
      /// Gets the end color of the gradient used when the button is pressed.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used when the button is pressed.</returns>
      public override Color ButtonPressedGradientEnd {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonPressedGradientEnd );
          }
          return base.ButtonPressedGradientEnd;
        }
      }


      /// <summary>
      /// Gets the middle color of the gradient used when the button is pressed.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used when the button is pressed.</returns>
      public override Color ButtonPressedGradientMiddle {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonPressedGradientMiddle );
          }
          return base.ButtonPressedGradientMiddle;
        }
      }

      /// <summary>
      /// Gets the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientBegin"></see>, <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientMiddle"></see>, and <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientEnd"></see> colors.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use with the <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientBegin"></see>, <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientMiddle"></see>, and <see cref="P:System.Windows.Forms.ProfessionalColorTable.ButtonSelectedGradientEnd"></see> colors.</returns>
      public override Color ButtonSelectedBorder {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonPressedBorder );
          }
          return base.ButtonSelectedBorder;
        }
      }


      /// <summary>
      /// Gets the starting color of the gradient used when the button is selected.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used when the button is selected.</returns>
      public override Color ButtonSelectedGradientBegin {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonSelectedGradientBegin );
          }
          return base.ButtonSelectedGradientBegin;
        }
      }
      /// <summary>
      /// Gets the end color of the gradient used when the button is selected.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used when the button is selected.</returns>
      public override Color ButtonSelectedGradientEnd {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonSelectedGradientEnd );
          }
          return base.ButtonSelectedGradientEnd;
        }
      }
      /// <summary>
      /// Gets the middle color of the gradient used when the button is selected.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used when the button is selected.</returns>
      public override Color ButtonSelectedGradientMiddle {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonSelectedGradientMiddle );
          }
          return base.ButtonSelectedGradientMiddle;
        }
      }

      /// <summary>
      /// Gets the solid color to use when the button is checked and gradients are being used.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid color to use when the button is checked and gradients are being used.</returns>
      public override Color CheckBackground {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.CheckBackground );
          }
          return base.CheckBackground;
        }
      }

      /// <summary>
      /// Gets the solid color to use when the button is checked and selected and gradients are being used.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid color to use when the button is checked and selected and gradients are being used.</returns>
      public override Color CheckPressedBackground {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.CheckBackgroundMouseOver );
          }
          return base.CheckPressedBackground;
        }
      }

      /// <summary>
      /// Gets the solid color to use when the button is checked and selected and gradients are being used.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid color to use when the button is checked and selected and gradients are being used.</returns>
      public override Color CheckSelectedBackground {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.CheckBackgroundMouseOver );
          }
          return base.CheckSelectedBackground;
        }
      }


      /// <summary>
      /// Gets the color scheme.
      /// </summary>
      /// <value>The color scheme.</value>
      internal static string ColorScheme {
        get {
          return TanColorTable.DisplayInformation.ColorScheme;
        }
      }

      /// <summary>
      /// Gets the color table.
      /// </summary>
      /// <value>The color table.</value>
      private Dictionary<KnownColors, Color> ColorTable {
        get {
          if ( this.tanRGB == null ) {
            this.tanRGB = new Dictionary<KnownColors, Color> ( (int)KnownColors.LastKnownColor );
            TanColorTable.InitTanLunaColors ( ref this.tanRGB );
          }
          return this.tanRGB;
        }
      }

      /// <summary>
      /// Gets the color to use for shadow effects on the grip (move handle).
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the color to use for shadow effects on the grip (move handle).</returns>
      public override Color GripDark {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.GripDark );
          }
          return base.GripDark;
        }
      }

      /// <summary>
      /// Gets the color to use for highlight effects on the grip (move handle).
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the color to use for highlight effects on the grip (move handle).</returns>
      public override Color GripLight {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.GripLight );
          }
          return base.GripLight;
        }
      }
      /// <summary>
      /// Gets the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see>.</returns>
      public override Color ImageMarginGradientBegin {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ImageMarginGradientBegin );
          }
          return base.ImageMarginGradientBegin;
        }
      }

      /// <summary>
      /// Gets the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see>.</returns>
      public override Color ImageMarginGradientEnd {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ImageMarginGradientEnd );
          }
          return base.ImageMarginGradientEnd;
        }
      }

      /// <summary>
      /// Gets the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see>.</returns>
      public override Color ImageMarginGradientMiddle {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ImageMarginGradientMiddle );
          }
          return base.ImageMarginGradientMiddle;
        }
      }

      /// <summary>
      /// Gets the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see> when an item is revealed.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see> when an item is revealed.</returns>
      public override Color ImageMarginRevealedGradientBegin {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ImageMarginRevealedGradientBegin );
          }
          return base.ImageMarginRevealedGradientBegin;
        }
      }

      /// <summary>
      /// Gets the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see> when an item is revealed.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see> when an item is revealed.</returns>
      public override Color ImageMarginRevealedGradientEnd {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ImageMarginRevealedGradientEnd );
          }
          return base.ImageMarginRevealedGradientEnd;
        }
      }

      /// <summary>
      /// Gets the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see> when an item is revealed.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used in the image margin of a <see cref="T:System.Windows.Forms.ToolStripDropDownMenu"></see> when an item is revealed.</returns>
      public override Color ImageMarginRevealedGradientMiddle {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ImageMarginRevealedGradientMiddle );
          }
          return base.ImageMarginRevealedGradientMiddle;
        }
      }

      /// <summary>
      /// Gets the color that is the border color to use on a <see cref="T:System.Windows.Forms.MenuStrip"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use on a <see cref="T:System.Windows.Forms.MenuStrip"></see>.</returns>
      public override Color MenuBorder {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.MenuBorder );
          }
          return base.MenuItemBorder;
        }
      }


      /// <summary>
      /// Gets the border color to use with a <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use with a <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see>.</returns>
      public override Color MenuItemBorder {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.MenuItemBorder );
          }
          return base.MenuItemBorder;
        }
      }

      /// <summary>
      /// Gets the starting color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is pressed.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is pressed.</returns>
      public override Color MenuItemPressedGradientBegin {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.MenuItemPressedGradientBegin );
          }
          return base.MenuItemPressedGradientBegin;
        }
      }

      /// <summary>
      /// Gets the end color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is pressed.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is pressed.</returns>
      public override Color MenuItemPressedGradientEnd {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.MenuItemPressedGradientEnd );
          }
          return base.MenuItemPressedGradientEnd;
        }
      }

      /// <summary>
      /// Gets the middle color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is pressed.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used when a top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is pressed.</returns>
      public override Color MenuItemPressedGradientMiddle {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ImageMarginRevealedGradientMiddle );
          }
          return base.MenuItemPressedGradientMiddle;
        }
      }

      /// <summary>
      /// Gets the solid color to use when a <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> other than the top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is selected.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid color to use when a <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> other than the top-level <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is selected.</returns>
      public override Color MenuItemSelected {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.MenuItemSelected );
          }
          return base.MenuItemSelected;
        }
      }

      /// <summary>
      /// Gets the starting color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is selected.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is selected.</returns>
      public override Color MenuItemSelectedGradientBegin {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonSelectedGradientBegin );
          }
          return base.MenuItemSelectedGradientBegin;
        }
      }

      /// <summary>
      /// Gets the end color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is selected.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used when the <see cref="T:System.Windows.Forms.ToolStripMenuItem"></see> is selected.</returns>
      public override Color MenuItemSelectedGradientEnd {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ButtonSelectedGradientEnd );
          }
          return base.MenuItemSelectedGradientEnd;
        }
      }


      /// <summary>
      /// Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip"></see>.</returns>
      public override Color MenuStripGradientBegin {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.MenuStripGradientBegin );
          }
          return base.MenuStripGradientBegin;
        }
      }

      /// <summary>
      /// Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.MenuStrip"></see>.</returns>
      public override Color MenuStripGradientEnd {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.MenuStripGradientEnd );
          }
          return base.MenuStripGradientEnd;
        }
      }

      /// <summary>
      /// Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton"></see>.</returns>
      public override Color OverflowButtonGradientBegin {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.OverflowButtonGradientBegin );
          }
          return base.OverflowButtonGradientBegin;
        }
      }

      /// <summary>
      /// Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton"></see>.</returns>
      public override Color OverflowButtonGradientEnd {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.OverflowButtonGradientEnd );
          }
          return base.OverflowButtonGradientEnd;
        }
      }

      /// <summary>
      /// Gets the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripOverflowButton"></see>.</returns>
      public override Color OverflowButtonGradientMiddle {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.OverflowButtonGradientMiddle );
          }
          return base.OverflowButtonGradientMiddle;
        }
      }


      /// <summary>
      /// Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer"></see>.</returns>
      public override Color RaftingContainerGradientBegin {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.MenuStripGradientBegin );
          }
          return base.RaftingContainerGradientBegin;
        }
      }

      /// <summary>
      /// Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStripContainer"></see>.</returns>
      public override Color RaftingContainerGradientEnd {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.MenuStripGradientEnd );
          }
          return base.RaftingContainerGradientEnd;
        }
      }


      /// <summary>
      /// Gets the color to use to for shadow effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the color to use to for shadow effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator"></see>.</returns>
      public override Color SeparatorDark {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.SeparatorDark );
          }
          return base.SeparatorDark;
        }
      }

      /// <summary>
      /// Gets the color to use to for highlight effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the color to use to for highlight effects on the <see cref="T:System.Windows.Forms.ToolStripSeparator"></see>.</returns>
      public override Color SeparatorLight {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.SeparatorLight );
          }
          return base.SeparatorLight;
        }
      }

      /// <summary>
      /// Gets the border color to use on the bottom edge of the <see cref="T:System.Windows.Forms.ToolStrip"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the border color to use on the bottom edge of the <see cref="T:System.Windows.Forms.ToolStrip"></see>.</returns>
      public override Color ToolStripBorder {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ToolStripBorder );
          }
          return base.ToolStripBorder;
        }
      }


      /// <summary>
      /// Gets the solid background color of the <see cref="T:System.Windows.Forms.ToolStripDropDown"></see>.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the solid background color of the <see cref="T:System.Windows.Forms.ToolStripDropDown"></see>.</returns>
      public override Color ToolStripDropDownBackground {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ToolStripDropDownBackground );
          }
          return base.ToolStripDropDownBackground;
        }
      }

      /// <summary>
      /// Gets the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip"></see> background.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the starting color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip"></see> background.</returns>
      public override Color ToolStripGradientBegin {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ImageMarginGradientBegin );
          }
          return base.ToolStripGradientBegin;
        }
      }

      /// <summary>
      /// Gets the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip"></see> background.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the end color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip"></see> background.</returns>
      public override Color ToolStripGradientEnd {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ImageMarginGradientEnd );
          }
          return base.ToolStripGradientEnd;
        }
      }

      /// <summary>
      /// Gets the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip"></see> background.
      /// </summary>
      /// <value></value>
      /// <returns>A <see cref="T:System.Drawing.Color"></see> that is the middle color of the gradient used in the <see cref="T:System.Windows.Forms.ToolStrip"></see> background.</returns>
      public override Color ToolStripGradientMiddle {
        get {
          if ( !this.UseBaseColorTable ) {
            return this.FromKnownColor ( TanColorTable.KnownColors.ImageMarginGradientMiddle );
          }
          return base.ToolStripGradientMiddle;
        }
      }

      /// <summary>
      /// Gets a value indicating whether [use base color table].
      /// </summary>
      /// <value><c>true</c> if [use base color table]; otherwise, <c>false</c>.</value>
      private bool UseBaseColorTable {
        get {
          bool flag1 = !TanColorTable.DisplayInformation.IsLunaTheme || ( ( TanColorTable.ColorScheme != "HomeStead" ) && ( TanColorTable.ColorScheme != "NormalColor" ) );
          if ( flag1 && ( this.tanRGB != null ) ) {
            this.tanRGB.Clear ();
            this.tanRGB = null;
          }
          return flag1;
        }
      }



      // Fields
      private const string blueColorScheme = "NormalColor";
      private const string oliveColorScheme = "HomeStead";
      private const string silverColorScheme = "Metallic";
      private Dictionary<TanColorTable.KnownColors, Color> tanRGB;

      // Nested Types
      private static class DisplayInformation {
        // Methods
        /// <summary>
        /// Initializes the <see cref="DisplayInformation"/> class.
        /// </summary>
        static DisplayInformation () {
          SystemEvents.UserPreferenceChanged += new UserPreferenceChangedEventHandler ( TanColorTable.DisplayInformation.OnUserPreferenceChanged );
          TanColorTable.DisplayInformation.SetScheme ();
        }

        /// <summary>
        /// Called when [user preference changed].
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Microsoft.Win32.UserPreferenceChangedEventArgs"/> instance containing the event data.</param>
        private static void OnUserPreferenceChanged ( object sender, UserPreferenceChangedEventArgs e ) {
          TanColorTable.DisplayInformation.SetScheme ();
        }

        /// <summary>
        /// Sets the scheme.
        /// </summary>
        private static void SetScheme () {
          TanColorTable.DisplayInformation.isLunaTheme = false;
          if ( VisualStyleRenderer.IsSupported ) {
            DisplayInformation.colorScheme = VisualStyleInformation.ColorScheme;

            if ( !VisualStyleInformation.IsEnabledByUser ) {
              return;
            }
            StringBuilder builder1 = new StringBuilder ( 0x200 );
            GetCurrentThemeName ( builder1, builder1.Capacity, null, 0, null, 0 );
            string text1 = builder1.ToString ();
            TanColorTable.DisplayInformation.isLunaTheme = string.Equals ( "luna.msstyles", Path.GetFileName ( text1 ), StringComparison.InvariantCultureIgnoreCase );
          } else {
            TanColorTable.DisplayInformation.isLunaTheme = true;
            TanColorTable.DisplayInformation.colorScheme = null;
          }
        }


        // Properties

        /// <summary>
        /// Gets the color scheme.
        /// </summary>
        /// <value>The color scheme.</value>
        public static string ColorScheme {
          get {
            return colorScheme;
          }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is luna theme.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is luna theme; otherwise, <c>false</c>.
        /// </value>
        internal static bool IsLunaTheme {
          get {
            return isLunaTheme;
          }
        }

        // Fields
        [ThreadStatic]
        private static string colorScheme;
        [ThreadStatic]
        private static bool isLunaTheme;
        private const string lunaFileName = "luna.msstyles";

        /// <summary>
        /// Gets the name of the current theme.
        /// </summary>
        /// <param name="pszThemeFileName">Name of the PSZ theme file.</param>
        /// <param name="dwMaxNameChars">The dw max name chars.</param>
        /// <param name="pszColorBuff">The PSZ color buff.</param>
        /// <param name="dwMaxColorChars">The dw max color chars.</param>
        /// <param name="pszSizeBuff">The PSZ size buff.</param>
        /// <param name="cchMaxSizeChars">The CCH max size chars.</param>
        /// <returns></returns>
        [DllImport ( "uxtheme.dll", CharSet = CharSet.Auto )]
        public static extern int GetCurrentThemeName ( StringBuilder pszThemeFileName, int dwMaxNameChars, StringBuilder pszColorBuff, int dwMaxColorChars, StringBuilder pszSizeBuff, int cchMaxSizeChars );

      }

      internal enum KnownColors {
        ButtonPressedBorder,
        MenuItemBorder,
        MenuItemBorderMouseOver,
        MenuItemSelected,
        CheckBackground,
        CheckBackgroundMouseOver,
        GripDark,
        GripLight,
        MenuStripGradientBegin,
        MenuStripGradientEnd,
        ImageMarginRevealedGradientBegin,
        ImageMarginRevealedGradientEnd,
        ImageMarginRevealedGradientMiddle,
        MenuItemPressedGradientBegin,
        MenuItemPressedGradientEnd,
        ButtonPressedGradientBegin,
        ButtonPressedGradientEnd,
        ButtonPressedGradientMiddle,
        ButtonSelectedGradientBegin,
        ButtonSelectedGradientEnd,
        ButtonSelectedGradientMiddle,
        OverflowButtonGradientBegin,
        OverflowButtonGradientEnd,
        OverflowButtonGradientMiddle,
        ButtonCheckedGradientBegin,
        ButtonCheckedGradientEnd,
        ButtonCheckedGradientMiddle,
        ImageMarginGradientBegin,
        ImageMarginGradientEnd,
        ImageMarginGradientMiddle,
        MenuBorder,
        ToolStripDropDownBackground,
        ToolStripBorder,
        SeparatorDark,
        SeparatorLight,
        LastKnownColor = SeparatorLight,

      }

    }
  }
}
