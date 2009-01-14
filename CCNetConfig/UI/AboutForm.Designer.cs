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
  partial class AboutForm {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose( bool disposing ) {
      if ( disposing && ( components != null ) ) {
        components.Dispose ();
      }
      base.Dispose (disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent() {
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( AboutForm ) );
			this.pictureBox1 = new System.Windows.Forms.PictureBox ();
			this.homeLink = new System.Windows.Forms.LinkLabel ();
			this.nameLabel = new System.Windows.Forms.Label ();
			this.versionLabel = new System.Windows.Forms.Label ();
			this.codePlexLink = new System.Windows.Forms.LinkLabel ();
			this.lstContributors = new System.Windows.Forms.ListBox ();
			this.label1 = new System.Windows.Forms.Label ();
			this.label2 = new System.Windows.Forms.Label ();
			this.lstModules = new System.Windows.Forms.ListBox ();
			this.okButton = new System.Windows.Forms.Button ();
			this.label3 = new System.Windows.Forms.Label ();
			this.detailsTextBox = new System.Windows.Forms.TextBox ();
			this.donate = new System.Windows.Forms.PictureBox ();
			( (System.ComponentModel.ISupportInitialize)( this.pictureBox1 ) ).BeginInit ();
			( (System.ComponentModel.ISupportInitialize)( this.donate ) ).BeginInit ();
			this.SuspendLayout ();
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::CCNetConfig.Properties.Resources.logo96;
			this.pictureBox1.Location = new System.Drawing.Point ( 12, 12 );
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size ( 104, 99 );
			this.pictureBox1.TabIndex = 0;
			this.pictureBox1.TabStop = false;
			// 
			// homeLink
			// 
			this.homeLink.AutoSize = true;
			this.homeLink.Location = new System.Drawing.Point ( 9, 330 );
			this.homeLink.Name = "homeLink";
			this.homeLink.Size = new System.Drawing.Size ( 164, 13 );
			this.homeLink.TabIndex = 1;
			this.homeLink.TabStop = true;
			this.homeLink.Text = "Copyright © Ryan Conrad 2006 - ";
			this.homeLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler ( this.homeLink_LinkClicked );
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Font = new System.Drawing.Font ( "Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
			this.nameLabel.Location = new System.Drawing.Point ( 122, 9 );
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size ( 130, 24 );
			this.nameLabel.TabIndex = 2;
			this.nameLabel.Text = "CCNetConfig";
			// 
			// versionLabel
			// 
			this.versionLabel.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.versionLabel.Font = new System.Drawing.Font ( "Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ( (byte)( 0 ) ) );
			this.versionLabel.Location = new System.Drawing.Point ( 258, 9 );
			this.versionLabel.Name = "versionLabel";
			this.versionLabel.Size = new System.Drawing.Size ( 184, 24 );
			this.versionLabel.TabIndex = 3;
			this.versionLabel.Text = "00.00.0000.0000";
			this.versionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			// 
			// codePlexLink
			// 
			this.codePlexLink.AutoSize = true;
			this.codePlexLink.Location = new System.Drawing.Point ( 354, 114 );
			this.codePlexLink.Name = "codePlexLink";
			this.codePlexLink.Size = new System.Drawing.Size ( 88, 13 );
			this.codePlexLink.TabIndex = 5;
			this.codePlexLink.TabStop = true;
			this.codePlexLink.Text = "CodePlex Project";
			this.codePlexLink.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler ( this.codePlexLink_LinkClicked );
			// 
			// lstContributors
			// 
			this.lstContributors.FormattingEnabled = true;
			this.lstContributors.Location = new System.Drawing.Point ( 126, 55 );
			this.lstContributors.Name = "lstContributors";
			this.lstContributors.Size = new System.Drawing.Size ( 316, 56 );
			this.lstContributors.TabIndex = 6;
			this.lstContributors.SelectedIndexChanged += new System.EventHandler ( this.lstContributors_SelectedIndexChanged );
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point ( 123, 38 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size ( 66, 13 );
			this.label1.TabIndex = 7;
			this.label1.Text = "Contributors:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point ( 9, 117 );
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size ( 111, 13 );
			this.label2.TabIndex = 9;
			this.label2.Text = "Installed Components:";
			// 
			// lstModules
			// 
			this.lstModules.FormattingEnabled = true;
			this.lstModules.Location = new System.Drawing.Point ( 12, 134 );
			this.lstModules.Name = "lstModules";
			this.lstModules.Size = new System.Drawing.Size ( 430, 69 );
			this.lstModules.TabIndex = 8;
			this.lstModules.SelectedIndexChanged += new System.EventHandler ( this.lstModules_SelectedIndexChanged );
			// 
			// okButton
			// 
			this.okButton.Location = new System.Drawing.Point ( 349, 312 );
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size ( 93, 28 );
			this.okButton.TabIndex = 10;
			this.okButton.Text = "&Close";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler ( this.okButton_Click );
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point ( 9, 220 );
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size ( 42, 13 );
			this.label3.TabIndex = 11;
			this.label3.Text = "Details:";
			// 
			// detailsTextBox
			// 
			this.detailsTextBox.BackColor = System.Drawing.SystemColors.Window;
			this.detailsTextBox.Location = new System.Drawing.Point ( 12, 236 );
			this.detailsTextBox.Multiline = true;
			this.detailsTextBox.Name = "detailsTextBox";
			this.detailsTextBox.ReadOnly = true;
			this.detailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.detailsTextBox.Size = new System.Drawing.Size ( 430, 52 );
			this.detailsTextBox.TabIndex = 12;
			// 
			// donate
			// 
			this.donate.Cursor = System.Windows.Forms.Cursors.Hand;
			this.donate.Image = global::CCNetConfig.Properties.Resources.donate;
			this.donate.Location = new System.Drawing.Point ( 12, 294 );
			this.donate.Name = "donate";
			this.donate.Size = new System.Drawing.Size ( 92, 26 );
			this.donate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
			this.donate.TabIndex = 13;
			this.donate.TabStop = false;
			this.donate.Click += new System.EventHandler ( this.donate_Click );
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size ( 451, 352 );
			this.Controls.Add ( this.donate );
			this.Controls.Add ( this.detailsTextBox );
			this.Controls.Add ( this.label3 );
			this.Controls.Add ( this.okButton );
			this.Controls.Add ( this.label2 );
			this.Controls.Add ( this.lstModules );
			this.Controls.Add ( this.label1 );
			this.Controls.Add ( this.lstContributors );
			this.Controls.Add ( this.codePlexLink );
			this.Controls.Add ( this.versionLabel );
			this.Controls.Add ( this.nameLabel );
			this.Controls.Add ( this.homeLink );
			this.Controls.Add ( this.pictureBox1 );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "AboutForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "About CCNetConfig";
			( (System.ComponentModel.ISupportInitialize)( this.pictureBox1 ) ).EndInit ();
			( (System.ComponentModel.ISupportInitialize)( this.donate ) ).EndInit ();
			this.ResumeLayout ( false );
			this.PerformLayout ();

    }

    #endregion

    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.LinkLabel homeLink;
    private System.Windows.Forms.Label nameLabel;
    private System.Windows.Forms.Label versionLabel;
    private System.Windows.Forms.LinkLabel codePlexLink;
    private System.Windows.Forms.ListBox lstContributors;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.Label label2;
    private System.Windows.Forms.ListBox lstModules;
    private System.Windows.Forms.Button okButton;
    private System.Windows.Forms.Label label3;
    private System.Windows.Forms.TextBox detailsTextBox;
		private System.Windows.Forms.PictureBox donate;

  }
}