using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using CCNetConfig.Core;
using CCNetConfig.CCNet.Security;

namespace CCNetConfig.UI.Wizards
{
    /// <summary>
    /// A wizard step to allow the user to select one of more users.
    /// </summary>
    public partial class SelectUsersStep : UserControl
    {
        #region Constructors
        /// <summary>
        /// Initialises a new <see cref="SelectUsersStep"/>.
        /// </summary>
        /// <param name="configuration"></param>
        public SelectUsersStep(CruiseControl configuration)
        {
            InitializeComponent();

            var security = configuration.Security as InternalServerSecurity;
            if (security != null)
            {
                foreach (var user in security.Users)
                {
                    var newUser = new ListViewItem(new string[] {
                        user.UserName,
                        user.DisplayName
                    }, "Inherit");
                    userList.Items.Add(newUser);
                }
            }
        }
        #endregion

        #region Public properties
        #region AllowedUsers
        /// <summary>
        /// The allowed users.
        /// </summary>
        public string[] AllowedUsers
        {
            get
            {
                var users = new List<string>();
                foreach (ListViewItem user in userList.Items)
                {
                    if (user.ImageKey == "Allow") users.Add(user.Text);
                }
                return users.ToArray();
            }
        }
        #endregion

        #region DeniedUsers
        /// <summary>
        /// The denied users.
        /// </summary>
        public string[] DeniedUsers
        {
            get
            {
                var users = new List<string>();
                foreach (ListViewItem user in userList.Items)
                {
                    if (user.ImageKey == "Deny") users.Add(user.Text);
                }
                return users.ToArray();
            }
        }
        #endregion

        #region Caption
        /// <summary>
        /// The display caption.
        /// </summary>
        public string Caption
        {
            get { return stepCaption.Text; }
            set { stepCaption.Text = value; }
        }
        #endregion

        #region userList_MouseClick()
        /// <summary>
        /// Handle a mouse click on the user list.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void userList_MouseClick(object sender, MouseEventArgs e)
        {
            var item = userList.GetItemAt(e.X, e.Y);
            if (item != null)
            {
                switch (item.ImageKey)
                {
                    case "Allow":
                        item.ImageKey = "Deny";
                        break;
                    case "Deny":
                        item.ImageKey = "Inherit";
                        break;
                    case "Inherit":
                        item.ImageKey = "Allow";
                        break;
                }
            }
        }
        #endregion
        #endregion
    }
}
