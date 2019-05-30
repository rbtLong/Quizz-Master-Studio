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

namespace UserProfileUI
{
    /// <summary>
    /// Interaction logic for UserListItem.xaml
    /// </summary>
    public partial class UserListItem : UserControl
    {
        private Profile person;

        public delegate void DoubleClickedEventHandler(object sender, MouseButtonEventArgs e);
        public event DoubleClickedEventHandler DoubleClicked;

        #region Public Properties
        public string UserName
        {
            get { return person.Name; }
        }
        public Uri IconPath
        {
            get { return person.IconPath; }
            set { person.IconPath = value; }
        }
        public DateTime LastLoggedIn
        {
            get { return person.LastAccessed; }
        }
        public Profile User
        {
            get { return person; }
        }
        #endregion

        public UserListItem()
        {
            InitializeComponent();
        }

        public UserListItem(Profile Person)
        {
            InitializeComponent();
            this.person = Person;
            this.tbxName.Text = person.Name;
            this.tbxLastLoggedIn.Text = person.LastAccessed == new DateTime()
                ? "Never Accessed"
                : person.LastAccessed.ToString();

            try
            {
                BitmapImage bImg = new BitmapImage(Person.IconPath);
                imgIcon.Source = bImg;
            }
            catch
            {
                person.IconPath = new Uri("UserIcons/User.png", UriKind.RelativeOrAbsolute);
                BitmapImage bmp = new BitmapImage();
                bmp.BeginInit();
                bmp.UriSource = person.IconPath;
                bmp.EndInit();
                imgIcon.Source = bmp;
                person.Save();
            }
        }

        private void Grid_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        private void UserControl_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (DoubleClicked != null)
                DoubleClicked(sender, e);
        }
    }
}
