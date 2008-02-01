namespace CCNetConfig.Core.Components {
  partial class MultilineStringEditorForm {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( MultilineStringEditorForm ) );
      this.rtbData = new System.Windows.Forms.RichTextBox ();
      this.okButton = new System.Windows.Forms.Button ();
      this.panel1 = new System.Windows.Forms.Panel ();
      this.cancelButton = new System.Windows.Forms.Button ();
      this.panel1.SuspendLayout ();
      this.SuspendLayout ();
      // 
      // rtbData
      // 
      this.rtbData.AcceptsTab = true;
      this.rtbData.DetectUrls = false;
      this.rtbData.Dock = System.Windows.Forms.DockStyle.Fill;
      this.rtbData.Location = new System.Drawing.Point ( 0, 0 );
      this.rtbData.Name = "rtbData";
      this.rtbData.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.ForcedVertical;
      this.rtbData.ShowSelectionMargin = true;
      this.rtbData.Size = new System.Drawing.Size ( 524, 245 );
      this.rtbData.TabIndex = 0;
      this.rtbData.Text = "";
      // 
      // okButton
      // 
      this.okButton.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.okButton.Location = new System.Drawing.Point ( 332, 3 );
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size ( 86, 29 );
      this.okButton.TabIndex = 1;
      this.okButton.Text = "&OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler ( this.okButton_Click );
      // 
      // panel1
      // 
      this.panel1.Controls.Add ( this.cancelButton );
      this.panel1.Controls.Add ( this.okButton );
      this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.panel1.Location = new System.Drawing.Point ( 0, 245 );
      this.panel1.Name = "panel1";
      this.panel1.Size = new System.Drawing.Size ( 524, 37 );
      this.panel1.TabIndex = 2;
      // 
      // cancelButton
      // 
      this.cancelButton.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point ( 433, 3 );
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size ( 86, 29 );
      this.cancelButton.TabIndex = 2;
      this.cancelButton.Text = "&Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new System.EventHandler ( this.cancelButton_Click );
      // 
      // MultilineStringEditorForm
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size ( 524, 282 );
      this.Controls.Add ( this.rtbData );
      this.Controls.Add ( this.panel1 );
      this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
      this.Name = "MultilineStringEditorForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "String Editor";
      this.panel1.ResumeLayout ( false );
      this.ResumeLayout ( false );

    }

    #endregion

    private System.Windows.Forms.RichTextBox rtbData;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Panel panel1;
    private System.Windows.Forms.Button cancelButton;
  }
}