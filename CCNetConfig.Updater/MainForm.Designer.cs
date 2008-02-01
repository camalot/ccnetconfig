namespace CCNetConfig.Updater {
  /// <summary>
  /// The main form for the updater
  /// </summary>
  partial class MainForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose ( bool disposing ) {
      if ( disposing && ( components != null ) ) {
        components.Dispose ();
      }
      base.Dispose ( disposing );
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent () {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( MainForm ) );
      this.progressBar1 = new System.Windows.Forms.ProgressBar ();
      this.lblStatus = new System.Windows.Forms.Label ();
      this.cancelButton = new System.Windows.Forms.Button ();
      this.SuspendLayout ();
      // 
      // progressBar1
      // 
      this.progressBar1.Location = new System.Drawing.Point ( 12, 25 );
      this.progressBar1.Name = "progressBar1";
      this.progressBar1.Size = new System.Drawing.Size ( 519, 37 );
      this.progressBar1.TabIndex = 0;
      // 
      // lblStatus
      // 
      this.lblStatus.AutoSize = true;
      this.lblStatus.Location = new System.Drawing.Point ( 12, 6 );
      this.lblStatus.Name = "lblStatus";
      this.lblStatus.Size = new System.Drawing.Size ( 124, 13 );
      this.lblStatus.TabIndex = 1;
      this.lblStatus.Text = "Initializing, Please Wait...";
      // 
      // CancelButton
      // 
      this.cancelButton.Location = new System.Drawing.Point ( 402, 68 );
      this.cancelButton.Name = "CancelButton";
      this.cancelButton.Size = new System.Drawing.Size ( 129, 34 );
      this.cancelButton.TabIndex = 2;
      this.cancelButton.Text = "&Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // MainForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size ( 543, 110 );
      this.Controls.Add ( this.cancelButton );
      this.Controls.Add ( this.lblStatus );
      this.Controls.Add ( this.progressBar1 );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
      this.MaximizeBox = false;
      this.Name = "MainForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.ResumeLayout ( false );
      this.PerformLayout ();

    }

    #endregion

    private System.Windows.Forms.ProgressBar progressBar1;
    private System.Windows.Forms.Label lblStatus;
    private System.Windows.Forms.Button cancelButton;
  }
}