using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace CCNetConfig.UI {
  /// <summary>
  /// 
  /// </summary>
  public partial class SplashForm : Form {
    /// <summary>
    /// Initializes a new instance of the <see cref="SplashForm"/> class.
    /// </summary>
    public SplashForm ( ) {
      this.Cursor = Cursors.WaitCursor;
      InitializeComponent ( );
    }

    /// <summary>
    /// </summary>
    /// <param name="e">A <see cref="T:System.Windows.Forms.PaintEventArgs"/> that contains the event data.</param>
    protected override void OnPaint ( PaintEventArgs e ) {
      ControlPaint.DrawBorder3D ( e.Graphics, this.ClientRectangle, Border3DStyle.Flat );
      base.OnPaint ( e );
    }
  }
}
