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
using System.Threading;
using UserProfile;
using WPF.MDI;
using QuizzMasterStudio.Quizz;
using Quiz = global::Quizz;
using System.IO;
using UserProfileUI;

namespace QuizzMasterStudio
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        bool? result;
        SynchronizationContext synch;

        public MainWindow()
        {
            InitializeComponent();
            synch = SynchronizationContext.Current;
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Hide();
            SplashScreen splash = new SplashScreen();
            bool? result = splash.ShowDialog();
            if (result.HasValue && result.Value)
                this.Show();

            signInDialog();
            initializeQuizzCreation();

            this.Closing += new System.ComponentModel.CancelEventHandler(MainWindow_Closing);
        }


        void MainWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = closeMdiSessions();
        }

        private void signInDialog()
        {
            this.Hide();
            UserProfileManager usrProf = new UserProfileManager();
            usrProf.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
            result = usrProf.ShowDialog();
            if (result.HasValue && result.Value)
                App.User = usrProf.CurrentlySelected;

            if (result.HasValue && !result.Value)
                Environment.Exit(0);
            else
                signIn();
        }

        private void signIn()
        {
            this.Title = string.Format("QuizzMaster Studio [User: {0}]", App.User.Name);
            this.Show();
            this.WindowState = System.Windows.WindowState.Maximized;
            this.Focus();
        }

        private void signOut()
        {
            App.User = null;
            ctrlQuestion.lbxQuestionSelect.SelectedIndex = -1;
            ctrlQuizz.lbxQuizzSelect.SelectedIndex = -1;
            ctrlQuestion.expContainer.IsExpanded = false;
            signInDialog();
        }

        private void miSignOut_Click(object sender, RoutedEventArgs e)
        {
            if(!closeMdiSessions())
                signOut();
        }


        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

        private void miCreateQuizz_Click(object sender, RoutedEventArgs e)
        {
            CreateQuizzDialog();
        }
    }
}
