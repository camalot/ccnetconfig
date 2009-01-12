namespace CCNetConfig.UI
{
    partial class ValidationForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ValidationForm));
            this.validationResults = new System.Windows.Forms.ListView();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.statusMessage = new System.Windows.Forms.ToolStripStatusLabel();
            this.currentProgress = new System.Windows.Forms.ToolStripProgressBar();
            this.columnHeader1 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader2 = new System.Windows.Forms.ColumnHeader();
            this.columnHeader3 = new System.Windows.Forms.ColumnHeader();
            this.validationLoader = new System.ComponentModel.BackgroundWorker();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // validationResults
            // 
            this.validationResults.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
            this.validationResults.Dock = System.Windows.Forms.DockStyle.Fill;
            this.validationResults.FullRowSelect = true;
            this.validationResults.Location = new System.Drawing.Point(0, 0);
            this.validationResults.MultiSelect = false;
            this.validationResults.Name = "validationResults";
            this.validationResults.Size = new System.Drawing.Size(349, 276);
            this.validationResults.TabIndex = 0;
            this.validationResults.UseCompatibleStateImageBehavior = false;
            this.validationResults.View = System.Windows.Forms.View.Details;
            this.validationResults.DoubleClick += new System.EventHandler(this.validationResults_DoubleClick);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMessage,
            this.currentProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 276);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(349, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // statusMessage
            // 
            this.statusMessage.Name = "statusMessage";
            this.statusMessage.Size = new System.Drawing.Size(232, 17);
            this.statusMessage.Spring = true;
            this.statusMessage.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // currentProgress
            // 
            this.currentProgress.Name = "currentProgress";
            this.currentProgress.Size = new System.Drawing.Size(100, 16);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Item";
            this.columnHeader1.Width = 90;
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Type";
            this.columnHeader2.Width = 90;
            // 
            // columnHeader3
            // 
            this.columnHeader3.Text = "Issue";
            this.columnHeader3.Width = 150;
            // 
            // validationLoader
            // 
            this.validationLoader.WorkerReportsProgress = true;
            this.validationLoader.WorkerSupportsCancellation = true;
            this.validationLoader.DoWork += new System.ComponentModel.DoWorkEventHandler(this.validationLoader_DoWork);
            this.validationLoader.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.validationLoader_RunWorkerCompleted);
            this.validationLoader.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.validationLoader_ProgressChanged);
            // 
            // ValidationForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(349, 298);
            this.Controls.Add(this.validationResults);
            this.Controls.Add(this.statusStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ValidationForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Validation Results";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ValidationForm_FormClosing);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListView validationResults;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ColumnHeader columnHeader2;
        private System.Windows.Forms.ColumnHeader columnHeader3;
        private System.Windows.Forms.ToolStripStatusLabel statusMessage;
        private System.Windows.Forms.ToolStripProgressBar currentProgress;
        private System.ComponentModel.BackgroundWorker validationLoader;
    }
}