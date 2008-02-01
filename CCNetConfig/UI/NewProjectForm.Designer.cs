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
  partial class NewProjectForm {
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( NewProjectForm ) );
      this.OkButton = new System.Windows.Forms.Button ();
      this.cancelButton = new System.Windows.Forms.Button ();
      this.label1 = new System.Windows.Forms.Label ();
      this.projectNameTextBox = new System.Windows.Forms.TextBox ();
      this.errorProvider = new System.Windows.Forms.ErrorProvider ( this.components );
      ( (System.ComponentModel.ISupportInitialize)( this.errorProvider ) ).BeginInit ();
      this.SuspendLayout ();
      // 
      // OkButton
      // 
      this.OkButton.Location = new System.Drawing.Point ( 241, 32 );
      this.OkButton.Name = "OkButton";
      this.OkButton.Size = new System.Drawing.Size ( 75, 28 );
      this.OkButton.TabIndex = 0;
      this.OkButton.Text = "&OK";
      this.OkButton.UseVisualStyleBackColor = true;
      this.OkButton.Click += new System.EventHandler ( this.OkButton_Click );
      // 
      // cancelButton
      // 
      this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.cancelButton.Location = new System.Drawing.Point ( 322, 32 );
      this.cancelButton.Name = "cancelButton";
      this.cancelButton.Size = new System.Drawing.Size ( 75, 28 );
      this.cancelButton.TabIndex = 1;
      this.cancelButton.Text = "&Cancel";
      this.cancelButton.UseVisualStyleBackColor = true;
      this.cancelButton.Click += new System.EventHandler ( this.cancelButton_Click );
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point ( 12, 9 );
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size ( 74, 13 );
      this.label1.TabIndex = 2;
      this.label1.Text = "Project Name:";
      // 
      // projectNameTextBox
      // 
      this.projectNameTextBox.Location = new System.Drawing.Point ( 96, 6 );
      this.projectNameTextBox.Name = "projectNameTextBox";
      this.projectNameTextBox.Size = new System.Drawing.Size ( 283, 20 );
      this.projectNameTextBox.TabIndex = 3;
      // 
      // errorProvider
      // 
      this.errorProvider.ContainerControl = this;
      this.errorProvider.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "errorProvider.Icon" ) ) );
      // 
      // NewProjectForm
      // 
      this.AcceptButton = this.OkButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.cancelButton;
      this.ClientSize = new System.Drawing.Size ( 409, 66 );
      this.Controls.Add ( this.projectNameTextBox );
      this.Controls.Add ( this.label1 );
      this.Controls.Add ( this.cancelButton );
      this.Controls.Add ( this.OkButton );
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "NewProjectForm";
      this.Text = "New Project";
      this.Load += new System.EventHandler ( this.NewProjectForm_Load );
      ( (System.ComponentModel.ISupportInitialize)( this.errorProvider ) ).EndInit ();
      this.ResumeLayout ( false );
      this.PerformLayout ();

    }

    #endregion

    private System.Windows.Forms.Button OkButton;
    private System.Windows.Forms.Button cancelButton;
    private System.Windows.Forms.Label label1;
    private System.Windows.Forms.TextBox projectNameTextBox;
    private System.Windows.Forms.ErrorProvider errorProvider;
  }
}