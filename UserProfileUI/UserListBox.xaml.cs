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
using System.Windows.Navigation;
using System.Windows.Shapes;
using UserProfile;
using Microsoft.Win32;
using System.Xml.Linq;
using System.IO;

namespace UserProfileUI
{
    /// <summary>
    /// Interaction logic for UserListBox.xaml
    /// </summary>
    public partial class UserListBox : UserControl
    {
        private List<Profile> users;

        public event UserDeletedEventHandler UserDeleted;
        public delegate void UserDeletedEventHandler(object sender, string Person);

        public delegate void DoubleClickedEventHandler(object sender, MouseButtonEventArgs e);
        public event DoubleClickedEventHandler DoubleClicked;

        public event UserImportedEventHandler UserImported;
        public delegate void UserImportedEventHandler(object sender);

        #region Public Members
        public Profile CurrentlySelectedUser
        {
            get { return (lbxUsers.Items[lbxUsers.SelectedIndex] as UserListItem).User; }
        }

        public List<Profile> Users
        {
            get { return users; }
            set 
            { 
                users = value;
                updatePersonList();
            }
        }
        #endregion

        public UserListBox()
        {
            InitializeComponent();
            users = new List<Profile>();
            lbxUsers.SelectionChanged += new SelectionChangedEventHandler(lbxUsers_SelectionChanged);
            lbxUsers.MouseDoubleClick += new MouseButtonEventHandler(lbxUsers_MouseDoubleClick);
        }

        void lbxUsers_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DoubleClicked != null)
                DoubleClicked(sender, e);
        }

        void lbxUsers_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            miConfigure.IsEnabled = miDelete.IsEnabled = lbxUsers.SelectedIndex > -1;
        }

        public UserListBox(IEnumerable<Profile> Profiles)
        {
            InitializeComponent();
            this.users = Profiles.ToList<Profile>();
        }

        public void AddUser(Profile Person)
        {
            users.Add(Person);
            updatePersonList();
        }

        private void updatePersonList()
        {
            lbxUsers.Items.Clear();
            foreach (Profile itm in users)
                lbxUsers.Items.Add(new UserListItem(itm));
        }

        public bool IsDupelicateName(string Name)
        {
            return (users.Find((itm) => itm.Name == Name) != null);
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
            switch (MessageBox.Show("Are you sure you want , Permanently delete this profile?",
                "Permenantly Delete Profile?", MessageBoxButton.YesNo))
            { 
                case MessageBoxResult.Yes:
                    string name = CurrentlySelectedUser.Name;
                    Profile.DeleteProfile(CurrentlySelectedUser);

                    if (UserDeleted != null) 
                        UserDeleted(this, name);

                    MessageBox.Show(name + " has been successfully deleted.");
                    break;
            }
        }

        public void ClearAllUsers()
        {
            lbxUsers.Items.Clear();
            users.Clear();
        }

        private void miImport_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog oFi = new OpenFileDialog();
            oFi.Filter = "Quizz Master User Profile | *.upl";
            oFi.Multiselect = false;
            oFi.Title = "Select Profile";
            oFi.InitialDirectory = Profile.DefaultFolder;

            bool? result = oFi.ShowDialog();
            if (result.HasValue && result.Value)
            {
                try
                {
                    Profile person = new Profile(XDocument.Load(oFi.FileName));
                    string[] Files = Directory.GetFiles(new FileInfo(oFi.FileName).DirectoryName, 
                        "*.*", SearchOption.AllDirectories);

                    foreach (string itm in Files)
                    {
                        string dest = itm.Replace(new FileInfo(oFi.FileName).Directory.FullName,
                            person.DefaultUserFolder.FullName);
                        FileInfo fi = new FileInfo(dest);
                        if (!fi.Directory.Exists)
                            Directory.CreateDirectory(fi.Directory.FullName);
                        File.Copy(itm, dest, true);
                    }

                    if (UserImported != null)
                        UserImported(this);
                }
                catch(IOException ex)
                {

                    MessageBox.Show("Could not load that profile.\n " + ex);
                }
            }
        }

    }

}
