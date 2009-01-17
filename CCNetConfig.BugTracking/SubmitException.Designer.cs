namespace CCNetConfig.BugTracking {
  partial class SubmitException {
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager ( typeof ( SubmitException ) );
			this.submitBug = new System.Windows.Forms.Button ();
			this.noSubmit = new System.Windows.Forms.Button ();
			this.errorMessage = new System.Windows.Forms.Label ();
			this.showDetails = new System.Windows.Forms.Button ();
			this.textdetails = new System.Windows.Forms.TextBox ();
			this.includeConfig = new System.Windows.Forms.CheckBox ();
			this.SuspendLayout ();
			// 
			// submitBug
			// 
			this.submitBug.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.submitBug.Location = new System.Drawing.Point ( 283, 116 );
			this.submitBug.Name = "submitBug";
			this.submitBug.Size = new System.Drawing.Size ( 170, 27 );
			this.submitBug.TabIndex = 0;
			this.submitBug.Text = "&Submit Bug && Continue";
			this.submitBug.UseVisualStyleBackColor = true;
			this.submitBug.Click += new System.EventHandler ( this.submitBug_Click );
			// 
			// noSubmit
			// 
			this.noSubmit.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.noSubmit.Location = new System.Drawing.Point ( 176, 116 );
			this.noSubmit.Name = "noSubmit";
			this.noSubmit.Size = new System.Drawing.Size ( 101, 27 );
			this.noSubmit.TabIndex = 1;
			this.noSubmit.Text = "&Continue";
			this.noSubmit.UseVisualStyleBackColor = true;
			this.noSubmit.Click += new System.EventHandler ( this.noSubmit_Click );
			// 
			// errorMessage
			// 
			this.errorMessage.Location = new System.Drawing.Point ( 12, 9 );
			this.errorMessage.Name = "errorMessage";
			this.errorMessage.Size = new System.Drawing.Size ( 441, 75 );
			this.errorMessage.TabIndex = 2;
			// 
			// showDetails
			// 
			this.showDetails.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.showDetails.Location = new System.Drawing.Point ( 12, 116 );
			this.showDetails.Name = "showDetails";
			this.showDetails.Size = new System.Drawing.Size ( 79, 27 );
			this.showDetails.TabIndex = 3;
			this.showDetails.Text = "Show &Details";
			this.showDetails.UseVisualStyleBackColor = true;
			this.showDetails.Click += new System.EventHandler ( this.showDetails_Click );
			// 
			// textdetails
			// 
			this.textdetails.Location = new System.Drawing.Point ( 12, 103 );
			this.textdetails.Multiline = true;
			this.textdetails.Name = "textdetails";
			this.textdetails.ReadOnly = true;
			this.textdetails.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.textdetails.Size = new System.Drawing.Size ( 440, 139 );
			this.textdetails.TabIndex = 4;
			this.textdetails.Visible = false;
			// 
			// includeConfig
			// 
			this.includeConfig.Anchor = ( (System.Windows.Forms.AnchorStyles)( ( System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right ) ) );
			this.includeConfig.AutoSize = true;
			this.includeConfig.Checked = true;
			this.includeConfig.CheckState = System.Windows.Forms.CheckState.Checked;
			this.includeConfig.Location = new System.Drawing.Point ( 283, 94 );
			this.includeConfig.Name = "includeConfig";
			this.includeConfig.Size = new System.Drawing.Size ( 179, 17 );
			this.includeConfig.TabIndex = 5;
			this.includeConfig.Text = "Include Configuration if available";
			this.includeConfig.UseVisualStyleBackColor = true;
			// 
			// SubmitException
			// 
			this.AcceptButton = this.submitBug;
			this.AutoScaleDimensions = new System.Drawing.SizeF ( 6F, 13F );
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size ( 465, 151 );
			this.Controls.Add ( this.includeConfig );
			this.Controls.Add ( this.showDetails );
			this.Controls.Add ( this.errorMessage );
			this.Controls.Add ( this.noSubmit );
			this.Controls.Add ( this.submitBug );
			this.Controls.Add ( this.textdetails );
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Icon = ( (System.Drawing.Icon)( resources.GetObject ( "$this.Icon" ) ) );
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SubmitException";
			this.ShowInTaskbar = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Submit Bug";
			this.ResumeLayout ( false );
			this.PerformLayout ();

    }

    #endregion

    private System.Windows.Forms.Button submitBug;
    private System.Windows.Forms.Button noSubmit;
    private System.Windows.Forms.Label errorMessage;
    private System.Windows.Forms.Button showDetails;
    private System.Windows.Forms.TextBox textdetails;
		private System.Windows.Forms.CheckBox includeConfig;
  }
}