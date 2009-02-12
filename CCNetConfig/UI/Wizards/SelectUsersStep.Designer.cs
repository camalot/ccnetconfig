namespace CCNetConfig.UI.Wizards
{
    partial class SelectUsersStep
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectUsersStep));
            this.stepCaption = new System.Windows.Forms.Label();
            this.userList = new System.Windows.Forms.ListView();
            this.userNameColumn = new System.Windows.Forms.ColumnHeader("(none)");
            this.displayNameColumn = new System.Windows.Forms.ColumnHeader();
            this.userPermissionList = new System.Windows.Forms.ImageList(this.components);
            this.SuspendLayout();
            // 
            // stepCaption
            // 
            this.stepCaption.AutoSize = true;
            this.stepCaption.Location = new System.Drawing.Point(3, 0);
            this.stepCaption.Name = "stepCaption";
            this.stepCaption.Size = new System.Drawing.Size(119, 13);
            this.stepCaption.TabIndex = 0;
            this.stepCaption.Text = "Please select the users:";
            // 
            // userList
            // 
            this.userList.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.userList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.userNameColumn,
            this.displayNameColumn});
            this.userList.FullRowSelect = true;
            this.userList.Location = new System.Drawing.Point(6, 16);
            this.userList.Name = "userList";
            this.userList.Size = new System.Drawing.Size(507, 272);
            this.userList.SmallImageList = this.userPermissionList;
            this.userList.TabIndex = 1;
            this.userList.UseCompatibleStateImageBehavior = false;
            this.userList.View = System.Windows.Forms.View.Details;
            this.userList.MouseClick += new System.Windows.Forms.MouseEventHandler(this.userList_MouseClick);
            // 
            // userNameColumn
            // 
            this.userNameColumn.Text = "User Name";
            this.userNameColumn.Width = 250;
            // 
            // displayNameColumn
            // 
            this.displayNameColumn.Text = "Display Name";
            this.displayNameColumn.Width = 216;
            // 
            // userPermissionList
            // 
            this.userPermissionList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("userPermissionList.ImageStream")));
            this.userPermissionList.TransparentColor = System.Drawing.Color.Transparent;
            this.userPermissionList.Images.SetKeyName(0, "Deny");
            this.userPermissionList.Images.SetKeyName(1, "Allow");
            this.userPermissionList.Images.SetKeyName(2, "Inherit");
            // 
            // SelectUsersStep
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.userList);
            this.Controls.Add(this.stepCaption);
            this.Name = "SelectUsersStep";
            this.Size = new System.Drawing.Size(516, 291);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label stepCaption;
        private System.Windows.Forms.ListView userList;
        private System.Windows.Forms.ImageList userPermissionList;
        private System.Windows.Forms.ColumnHeader userNameColumn;
        private System.Windows.Forms.ColumnHeader displayNameColumn;
    }
}
