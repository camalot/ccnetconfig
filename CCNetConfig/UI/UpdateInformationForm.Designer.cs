namespace CCNetConfig.UI {
  partial class UpdateInformationForm {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( UpdateInformationForm ) );
      this.pictureBox1 = new System.Windows.Forms.PictureBox ();
      this.changeComments = new System.Windows.Forms.TextBox ();
      this.label2 = new System.Windows.Forms.Label ();
      this.infoLabel = new System.Windows.Forms.Label ();
      this.okButton = new System.Windows.Forms.Button ();
      this.cancelButton = new System.Windows.Forms.Button ();
      ( (System.ComponentModel.ISupportInitialize)( this.pictureBox1 ) ).BeginInit ();
      this.SuspendLayout ();
      // 
      // pictureBox1
      // 
      this.pictureBox1.Image = global::CCNetConfig.Properties.Resources.updatesIcon;
      this.pictureBox1.Location = new System.Drawing.Point ( 4, 9 );
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size ( 48, 48 );
      this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.pictureBox1.TabIndex = 0;
      this.pictureBox1.TabStop = false;
      // 
      // changeComments
      // 
      this.changeComments.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
      this.changeComments.Location = new System.Drawing.Point ( 70, 69 );
      this.changeComments.Multiline = true;
      this.changeComments.Name = "changeComments";
      this.changeComments.ReadOnly = true;
      this.changeComments.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.changeComments.Size = new System.Drawing.Size ( 354, 55 );
      this.changeComments.TabIndex = 3;
      this.changeComments.TabStop = false;
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point ( 1, 69 );
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size ( 63, 13 );
      this.label2.TabIndex = 4;
      this.label2.Text = "Description:";
      // 
      // infoLabel
      // 
      this.infoLabel.Location = new System.Drawing.Point ( 67, 9 );
      this.infoLabel.Name = "infoLabel";
      this.infoLabel.Size = new System.Drawing.Size ( 357, 48 );
      this.infoLabel.TabIndex = 5;
      this.infoLabel.Text = "Updates are available.";
      // 
      // okButton
      // 
      this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.okButton.Location = new System.Drawing.Point ( 329, 137 );
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size ( 95, 32 );
      this.okButton.TabIndex = 7;
      this.okButton.Text = "&Update Now";
      this.okButton.UseVisualStyleBackColor = true;
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point ( 217, 137 );
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size ( 95, 32 );
      this.cancelButton.TabIndex = 8;
      this.cancelButton.Text = "&Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // UpdateInformationForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size ( 436, 181 );
      this.Controls.Add ( this.cancelButton );
      this.Controls.Add ( this.okButton );
      this.Controls.Add ( this.infoLabel );
      this.Controls.Add ( this.label2 );
      this.Controls.Add ( this.changeComments );
      this.Controls.Add ( this.pictureBox1 );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
      this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "UpdateInformationForm";
      this.ShowInTaskbar = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Updates Available";
      ( (System.ComponentModel.ISupportInitialize)( this.pictureBox1 ) ).EndInit ();
      this.ResumeLayout ( false );
      this.PerformLayout ();

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.TextBox changeComments;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label infoLabel;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
  }
}