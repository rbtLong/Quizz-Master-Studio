#region License
/*   QuizzMasterStudio Copyright © 2012 Robert Long
 *  
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <http://www.gnu.org/licenses/>.
 *   The full license is also included in the root folder.
 */
#endregion

#region Contact

/* Robert Long
 * Email: rbtLong@live.com
 */

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using UserProfile;
using System.Threading;

namespace UserProfileUI
{
    /// <summary>
    /// Interaction logic for UserProfileManager.xaml
    /// </summary>
    public partial class UserProfileManager : Window
    {
        SynchronizationContext synch;
        string iconPath = "UserIcons/User.png";

        public Profile CurrentlySelected
        {
            get { return lbxUsers.CurrentlySelectedUser; }
        }

        public UserProfileManager()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(UserProfileManager_Loaded);
            synch = SynchronizationContext.Current;
        }

        void UserProfileManager_Loaded(object sender, RoutedEventArgs e)
        {
            lbxUsers.DoubleClicked += new UserListBox.DoubleClickedEventHandler(lbxUsers_DoubleClicked);
            lbxUsers.UserDeleted += new UserListBox.UserDeletedEventHandler(lbxUsers_UserDeleted);
            lbxUsers.UserImported += new UserListBox.UserImportedEventHandler(lbxUsers_UserImported);
            loadUsers();
        }

        void lbxUsers_UserImported(object sender)
        {
            synch.Post((o) => loadUsers(), null);
        }

        void lbxUsers_DoubleClicked(object sender, MouseButtonEventArgs e)
        {
            this.DialogResult = true;
        }

        void lbxUsers_UserDeleted(object sender, string Person)
        {
            synch.Post((o) => loadUsers(), null);
        }

        private void loadUsers()
        {
            lbxUsers.ClearAllUsers();
            lbxUsers.lbxUsers.SelectionChanged += new SelectionChangedEventHandler(LbxUsers_SelectionChanged);
            foreach (Profile itm in Profile.GetProfilesFromFolder())
                lbxUsers.AddUser(itm);
        }

        void LbxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            btnOkay.IsEnabled = (lbxUsers.lbxUsers.SelectedIndex > -1);
            synch.Post((o) =>
                {
                    if (lbxUsers.lbxUsers.SelectedIndex > -1)
                    {
                        tbxDescription.Text = string.Format(
                            "Name: {0}\n"
                            + "Last Accessed: {1}\n"
                            + "Profile Created: {2}\n"
                            + "Login Count: {3}\n",
                            lbxUsers.CurrentlySelectedUser.Name,
                            lbxUsers.CurrentlySelectedUser.LastAccessed == new DateTime()
                                ? "Never Accessed"
                                : lbxUsers.CurrentlySelectedUser.LastAccessed.ToString(),
                            lbxUsers.CurrentlySelectedUser.DateCreated,
                            lbxUsers.CurrentlySelectedUser.LogInCount);
                            
                    }
                }, null);
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            lbxUsers.CurrentlySelectedUser.Save();
            this.DialogResult = true;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnAddUser_Click(object sender, RoutedEventArgs e)
        {
            if (validateForm(true))
            {
                Profile user = new Profile(tbxName.Text, new Uri(iconPath, UriKind.RelativeOrAbsolute));
                user.SaveComplete += (s,p) => loadUsers();
                user.Save();
            }
        }

        private bool validateForm(bool ShowDialog)
        {
            string msg = "The following errors must be corrected before continuing:\n",
                   err = msg;

            if (tbxName.Text.Trim().Length < 1)
                err += "\t- You must enter a Name\n";
            //if (tbxUsrIcon.Text.Trim().Length < 1)
            //    err += "\t- You must select a Picture\n";
            if (lbxUsers.IsDupelicateName(tbxName.Text))
                err += "\t- The Name you've picked already exists\n";

            if (msg != err)
            {
                MessageBox.Show(err);
                return false;
            }

            return true;
        }

        private void tbxUsrIcon_MouseDown(object sender, MouseButtonEventArgs e)
        {
            ctmuUserIcon.PlacementTarget = this;
            ctmuUserIcon.StaysOpen = true;
            ctmuUserIcon.IsOpen = true; 
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            imgUsrIcon.BeginInit();
            imgUsrIcon.Source = ((sender as MenuItem).Icon as Image).Source;
            imgUsrIcon.EndInit();
            iconPath = "UserIcons/" + (sender as MenuItem).Header;
        }

    }
}
