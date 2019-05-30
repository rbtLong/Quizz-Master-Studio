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
using GameEngine.GameQuizzDataTypes;
using System.Threading;
using UserProfile;
using System.IO;

namespace QuizzMasterStudio.Quizz
{
    /// <summary>
    /// Interaction logic for EditQuizz.xaml
    /// </summary>
    public partial class EditQuizz : UserControl
    {

        bool isNew = false;
        bool isChanged = false;
        SynchronizationContext synch;
        Profile user;
        GameQuizzSet qSet;
        DateTime createdOn = DateTime.Now;
        
        public CheckBox cbxDoNotFinish = new CheckBox();

        #region Public Properties
        public bool IsNew
        {
            get { return isNew; }
            set 
            { 
                isNew = value;
                tbxFileName.IsReadOnly = tbxQuizzName.IsReadOnly = !isNew;
            }
        }

        public GameQuizzSet QuizzSet
        {
            get { return qSet; }
            set 
            { 
                qSet = value;
                gmSetting.Settings = qSet.Settings;
            }
        }

        public bool IsChanged
        {
            get { return isChanged; }
            set { isChanged = value; }
        }

        public string IdToken
        {
            get { return qSet.Id; }
        }

        public string CreatedBy
        {
            get { return user.Name; }
        }

        public string Path
        {
            get { return GameQuizzSet.DefaultLocalPath; }
        }

        public DateTime CreatedOn
        {
            get { return createdOn; }
        }

        public Profile User
        {
            get { return user; }
            set
            {
                if (value != null)
                {
                    user = value;

                    synch.Post((o) =>
                    {
                        tbxCreatedBy.Text = user.Name;
                    }, null);
                }
            }
        }
        #endregion

        public EditQuizz()
        {
            InitializeComponent();
            user = null;
            qSet = new GameQuizzSet("Undefined");
            synch = SynchronizationContext.Current;
            this.Loaded += new RoutedEventHandler(EditorLoaded);
            this.Loaded += new RoutedEventHandler(EditQuizz_Loaded);
        }

        public EditQuizz(GameQuizzSet Quizz, Profile User, bool IsNew)
        {
            InitializeComponent();
            user = User;
            qSet = Quizz;
            this.IsNew = IsNew;
            synch = SynchronizationContext.Current;
            this.Loaded += new RoutedEventHandler(setLoadedProfile);
            this.Loaded += new RoutedEventHandler(EditQuizz_Loaded);
        }

        void EditQuizz_Loaded(object sender, RoutedEventArgs e)
        {
            tbxCreatedBy.Text = this.user == null ? "-- Unknown --" : this.user.Name;
            tbxPath.Text = this.Path;
            cbxDoNotFinish.Name = "cbxDoNotFinish";
            cbxDoNotFinish.Content = "Do Not Finish (Loop Indefinitely)";
            cbxDoNotFinish.Click += new RoutedEventHandler(cbxDoNotFinish_Click);
            gmSetting.AddNewProperty(cbxDoNotFinish);
        }

        void cbxDoNotFinish_Click(object sender, RoutedEventArgs e)
        {
            qSet.Settings.DoesNotFinish = cbxDoNotFinish.IsChecked.Value;
        }

        void setLoadedProfile(object sender, RoutedEventArgs e)
        {
            tbxQuizzName.Text = qSet.Name;
            tbxFileName.Text = qSet.FileId.FullName;
            tbxCreatedBy.Text = user.Name;
            tbxCreatedOn.Text = qSet.FileId.CreationTime.ToString();
            gmSetting.Settings = qSet.Settings;
            tbxIdToken.Text = qSet.Id;
            tbxQuizzName.TextChanged += new TextChangedEventHandler(tbxQuizzName_TextChanged);
            tbxCreatedOn.Text = createdOn.ToString();
        }

        void EditorLoaded(object sender, RoutedEventArgs e)
        {
            gmSetting.Settings = qSet.Settings;
            tbxIdToken.Text = qSet.Id;
            tbxQuizzName.TextChanged += new TextChangedEventHandler(tbxQuizzName_TextChanged);
            tbxCreatedOn.Text = createdOn.ToString();
        }

        void tbxQuizzName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbxQuizzName.Text.Length > 0)
            {
                qSet.Name = tbxQuizzName.Text;
                tbxFileName.Text = tbxQuizzName.Text + ".qzm";
            }
            else
            {
                qSet.Name = "Undefined";
                tbxFileName.Text = "";
            }
        }

        public bool ValidateFoms(bool ShowMessage)
        {
            string msg = "The following errors must be corrected before continuing:\n",
                   err = msg;

            if (tbxQuizzName.Text.Length < 1)
                err += "\t- Quizz must have a name";

            if(isNew)
                if (qSet.FileId.Exists)
                    err += "\t- File name already exists";

            if (msg != err)
            {
                if (ShowMessage)
                    MessageBox.Show(err, "Creation Error");
                return false;
            }
            return true;
        }

        private void tbxFileName_TextChanged(object sender, TextChangedEventArgs e)
        {
            qSet.FileId = new FileInfo(System.IO.Path.Combine(Path, tbxFileName.Text));
        }
    }
}
