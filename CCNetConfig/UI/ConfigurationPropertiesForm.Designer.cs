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
namespace CCNetConfig.UI {
  partial class ConfigurationPropertiesForm {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( ConfigurationPropertiesForm ) );
      this.label1 = new System.Windows.Forms.Label ();
      this.fileNameLabel = new System.Windows.Forms.Label ();
      this.label3 = new System.Windows.Forms.Label ();
      this.label4 = new System.Windows.Forms.Label ();
      this.createdDateLabel = new System.Windows.Forms.Label ();
      this.lastModifiedDate = new System.Windows.Forms.Label ();
      this.label2 = new System.Windows.Forms.Label ();
      this.numberOfProjectsLabel = new System.Windows.Forms.Label ();
      this.logoPictureBox = new System.Windows.Forms.PictureBox ();
      this.versionComboBox = new System.Windows.Forms.ComboBox ();
      this.okButton = new System.Windows.Forms.Button ();
      this.cancelButton = new System.Windows.Forms.Button ();
      this.label5 = new System.Windows.Forms.Label ();
      ( (System.ComponentModel.ISupportInitialize)( this.logoPictureBox ) ).BeginInit ();
      this.SuspendLayout ();
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point ( 12, 9 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size ( 57, 13 );
      this.label1.TabIndex = 0;
      this.label1.Text = "File Name:";
      // 
      // fileNameLabel
      // 
      this.fileNameLabel.AutoSize = true;
      this.fileNameLabel.Location = new System.Drawing.Point ( 113, 9 );
      this.fileNameLabel.Name = "fileNameLabel";
      this.fileNameLabel.Size = new System.Drawing.Size ( 66, 13 );
      this.fileNameLabel.TabIndex = 1;
      this.fileNameLabel.Text = "ccnet.config";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point ( 12, 78 );
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size ( 73, 13 );
      this.label3.TabIndex = 2;
      this.label3.Text = "Last Modified:";
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point ( 12, 40 );
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size ( 73, 13 );
      this.label4.TabIndex = 3;
      this.label4.Text = "Date Created:";
      // 
      // createdDateLabel
      // 
      this.createdDateLabel.AutoSize = true;
      this.createdDateLabel.Location = new System.Drawing.Point ( 113, 40 );
      this.createdDateLabel.Name = "createdDateLabel";
      this.createdDateLabel.Size = new System.Drawing.Size ( 96, 13 );
      this.createdDateLabel.TabIndex = 4;
      this.createdDateLabel.Text = "2/1/2007 7:04 PM";
      // 
      // lastModifiedDate
      // 
      this.lastModifiedDate.AutoSize = true;
      this.lastModifiedDate.Location = new System.Drawing.Point ( 113, 78 );
      this.lastModifiedDate.Name = "lastModifiedDate";
      this.lastModifiedDate.Size = new System.Drawing.Size ( 96, 13 );
      this.lastModifiedDate.TabIndex = 5;
      this.lastModifiedDate.Text = "2/1/2007 7:04 PM";
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point ( 12, 115 );
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size ( 70, 13 );
      this.label2.TabIndex = 6;
      this.label2.Text = "# of Projects:";
      // 
      // numberOfProjectsLabel
      // 
      this.numberOfProjectsLabel.AutoSize = true;
      this.numberOfProjectsLabel.Location = new System.Drawing.Point ( 113, 115 );
      this.numberOfProjectsLabel.Name = "numberOfProjectsLabel";
      this.numberOfProjectsLabel.Size = new System.Drawing.Size ( 13, 13 );
      this.numberOfProjectsLabel.TabIndex = 7;
      this.numberOfProjectsLabel.Text = "0";
      // 
      // logoPictureBox
      // 
      this.logoPictureBox.Image = global::CCNetConfig.Properties.Resources.logo96;
      this.logoPictureBox.Location = new System.Drawing.Point ( 286, 9 );
      this.logoPictureBox.Name = "logoPictureBox";
      this.logoPictureBox.Size = new System.Drawing.Size ( 96, 96 );
      this.logoPictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
      this.logoPictureBox.TabIndex = 8;
      this.logoPictureBox.TabStop = false;
      // 
      // versionComboBox
      // 
      this.versionComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.versionComboBox.FormattingEnabled = true;
      this.versionComboBox.Location = new System.Drawing.Point ( 116, 149 );
      this.versionComboBox.Name = "versionComboBox";
      this.versionComboBox.Size = new System.Drawing.Size ( 164, 21 );
      this.versionComboBox.TabIndex = 9;
      // 
      // okButton
      // 
      this.okButton.Location = new System.Drawing.Point ( 286, 197 );
      this.okButton.Name = "okButton";
      this.okButton.Size = new System.Drawing.Size ( 96, 30 );
      this.okButton.TabIndex = 10;
      this.okButton.Text = "&OK";
      this.okButton.UseVisualStyleBackColor = true;
      this.okButton.Click += new System.EventHandler ( this.okButton_Click );
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point ( 184, 197 );
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size ( 96, 30 );
      this.cancelButton.TabIndex = 11;
      this.cancelButton.Text = "&Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      // 
      // label5
      // 
      this.label5.AutoSize = true;
      this.label5.Location = new System.Drawing.Point ( 12, 152 );
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size ( 45, 13 );
      this.label5.TabIndex = 12;
      this.label5.Text = "Version:";
      // 
      // ConfigurationPropertiesForm
      // 
      this.AcceptButton = this.okButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size ( 394, 239 );
      this.Controls.Add ( this.label5 );
      this.Controls.Add ( this.cancelButton );
      this.Controls.Add ( this.okButton );
      this.Controls.Add ( this.versionComboBox );
      this.Controls.Add ( this.logoPictureBox );
      this.Controls.Add ( this.numberOfProjectsLabel );
      this.Controls.Add ( this.label2 );
      this.Controls.Add ( this.lastModifiedDate );
      this.Controls.Add ( this.createdDateLabel );
      this.Controls.Add ( this.label4 );
      this.Controls.Add ( this.label3 );
      this.Controls.Add ( this.fileNameLabel );
      this.Controls.Add ( this.label1 );
      this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
      this.Name = "ConfigurationPropertiesForm";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "Configuration Properties";
      ( (System.ComponentModel.ISupportInitialize)( this.logoPictureBox ) ).EndInit ();
      this.ResumeLayout ( false );
      this.PerformLayout ();

    }

    #endregion

    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label fileNameLabel;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.Label label4;
    private System.Windows.Forms.Label createdDateLabel;
    private System.Windows.Forms.Label lastModifiedDate;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.Label numberOfProjectsLabel;
    private System.Windows.Forms.PictureBox logoPictureBox;
    private System.Windows.Forms.ComboBox versionComboBox;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Label label5;
  }
}