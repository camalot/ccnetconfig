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
namespace CCNetConfig.BugTracking {
  partial class SubmitBugDialog {
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
			this.components = new System.ComponentModel.Container ();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( SubmitBugDialog ) );
			this.cancelButton = new System.Windows.Forms.Button ();
			this.submitButton = new System.Windows.Forms.Button ();
			this.errorInfo = new System.Windows.Forms.ErrorProvider ( this.components );
			this.lblDescription = new System.Windows.Forms.Label ();
			this.descriptionTextBox = new System.Windows.Forms.TextBox ();
			this.lblDetail = new System.Windows.Forms.Label ();
			this.detailsTextBox = new System.Windows.Forms.TextBox ();
			this.lblAttach = new System.Windows.Forms.Label ();
			this.attachedFileTextBox = new System.Windows.Forms.TextBox ();
			this.attachButton = new System.Windows.Forms.Button ();
			this.submissionStatusProgressBar = new System.Windows.Forms.ProgressBar ();
			this.label1 = new System.Windows.Forms.Label ();
			this.textemail = new System.Windows.Forms.TextBox ();
			( (System.ComponentModel.ISupportInitialize)( this.errorInfo ) ).BeginInit ();
			this.SuspendLayout ();
			// 
			// cancelButton
			// 
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point ( 313, 287 );
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size ( 117, 37 );
			this.cancelButton.TabIndex = 5;
			this.cancelButton.Text = "&Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// submitButton
			// 
			this.submitButton.Location = new System.Drawing.Point ( 436, 287 );
			this.submitButton.Name = "submitButton";
			this.submitButton.Size = new System.Drawing.Size ( 117, 37 );
			this.submitButton.TabIndex = 4;
			this.submitButton.Text = "&Submit";
			this.submitButton.UseVisualStyleBackColor = true;
			this.submitButton.Click += new System.EventHandler ( this.submitButton_Click );
			// 
			// errorInfo
			// 
			this.errorInfo.ContainerControl = this;
			this.errorInfo.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "errorInfo.Icon" ) ) );
			// 
			// lblDescription
			// 
			this.lblDescription.AutoSize = true;
			this.lblDescription.Location = new System.Drawing.Point ( 8, 52 );
			this.lblDescription.Name = "lblDescription";
			this.lblDescription.Size = new System.Drawing.Size ( 97, 13 );
			this.lblDescription.TabIndex = 2;
			this.lblDescription.Text = "Description of Bug:";
			// 
			// descriptionTextBox
			// 
			this.descriptionTextBox.Location = new System.Drawing.Point ( 11, 68 );
			this.descriptionTextBox.Multiline = true;
			this.descriptionTextBox.Name = "descriptionTextBox";
			this.descriptionTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.descriptionTextBox.Size = new System.Drawing.Size ( 542, 38 );
			this.descriptionTextBox.TabIndex = 1;
			// 
			// lblDetail
			// 
			this.lblDetail.AutoSize = true;
			this.lblDetail.Location = new System.Drawing.Point ( 9, 109 );
			this.lblDetail.Name = "lblDetail";
			this.lblDetail.Size = new System.Drawing.Size ( 42, 13 );
			this.lblDetail.TabIndex = 4;
			this.lblDetail.Text = "Details:";
			// 
			// detailsTextBox
			// 
			this.detailsTextBox.Location = new System.Drawing.Point ( 11, 125 );
			this.detailsTextBox.Multiline = true;
			this.detailsTextBox.Name = "detailsTextBox";
			this.detailsTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.detailsTextBox.Size = new System.Drawing.Size ( 542, 94 );
			this.detailsTextBox.TabIndex = 2;
			// 
			// lblAttach
			// 
			this.lblAttach.AutoSize = true;
			this.lblAttach.Location = new System.Drawing.Point ( 8, 229 );
			this.lblAttach.Name = "lblAttach";
			this.lblAttach.Size = new System.Drawing.Size ( 107, 13 );
			this.lblAttach.TabIndex = 6;
			this.lblAttach.Text = "Config File (Optional):";
			// 
			// attachedFileTextBox
			// 
			this.attachedFileTextBox.Location = new System.Drawing.Point ( 121, 225 );
			this.attachedFileTextBox.Name = "attachedFileTextBox";
			this.attachedFileTextBox.ReadOnly = true;
			this.attachedFileTextBox.Size = new System.Drawing.Size ( 397, 20 );
			this.attachedFileTextBox.TabIndex = 7;
			// 
			// attachButton
			// 
			this.attachButton.Location = new System.Drawing.Point ( 525, 225 );
			this.attachButton.Name = "attachButton";
			this.attachButton.Size = new System.Drawing.Size ( 28, 21 );
			this.attachButton.TabIndex = 3;
			this.attachButton.Text = "...";
			this.attachButton.UseVisualStyleBackColor = true;
			this.attachButton.Click += new System.EventHandler ( this.attachButton_Click );
			// 
			// submissionStatusProgressBar
			// 
			this.submissionStatusProgressBar.Enabled = false;
			this.submissionStatusProgressBar.Location = new System.Drawing.Point ( 12, 252 );
			this.submissionStatusProgressBar.MarqueeAnimationSpeed = 50;
			this.submissionStatusProgressBar.Maximum = 30;
			this.submissionStatusProgressBar.Name = "submissionStatusProgressBar";
			this.submissionStatusProgressBar.Size = new System.Drawing.Size ( 541, 29 );
			this.submissionStatusProgressBar.TabIndex = 9;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point ( 12, 9 );
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size ( 123, 13 );
			this.label1.TabIndex = 10;
			this.label1.Text = "Contact Email (Optional):";
			// 
			// textemail
			// 
			this.textemail.Location = new System.Drawing.Point ( 11, 25 );
			this.textemail.Name = "textemail";
			this.textemail.Size = new System.Drawing.Size ( 542, 20 );
			this.textemail.TabIndex = 0;
			// 
			// SubmitBugDialog
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size ( 570, 334 );
			this.Controls.Add ( this.textemail );
			this.Controls.Add ( this.label1 );
			this.Controls.Add ( this.attachButton );
			this.Controls.Add ( this.attachedFileTextBox );
			this.Controls.Add ( this.lblAttach );
			this.Controls.Add ( this.detailsTextBox );
			this.Controls.Add ( this.lblDetail );
			this.Controls.Add ( this.descriptionTextBox );
			this.Controls.Add ( this.lblDescription );
			this.Controls.Add ( this.submitButton );
			this.Controls.Add ( this.cancelButton );
			this.Controls.Add ( this.submissionStatusProgressBar );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.HelpButton = true;
			this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SubmitBugDialog";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Submit A Bug";
			( (System.ComponentModel.ISupportInitialize)( this.errorInfo ) ).EndInit ();
			this.ResumeLayout ( false );
			this.PerformLayout ();

    }

    #endregion

    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Button submitButton;
    private System.Windows.Forms.ErrorProvider errorInfo;
    private System.Windows.Forms.Label lblAttach;
    private System.Windows.Forms.TextBox detailsTextBox;
    private System.Windows.Forms.Label lblDetail;
    private System.Windows.Forms.TextBox descriptionTextBox;
    private System.Windows.Forms.Label lblDescription;
    private System.Windows.Forms.ProgressBar submissionStatusProgressBar;
    private System.Windows.Forms.Button attachButton;
    private System.Windows.Forms.TextBox attachedFileTextBox;
    private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox textemail;
  }
}