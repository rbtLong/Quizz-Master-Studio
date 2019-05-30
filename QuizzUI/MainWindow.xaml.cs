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
using System.IO;
using GameEngine.GameQuizzDataTypes;
using UserProfile;
using UserProfileUI;
using System.Threading;
using Quizz.QuizzSet;

namespace QuizzUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SynchronizationContext synch;
        List<GameQuizzSet> quizzes = new List<GameQuizzSet>();
        bool isShortCutted = false;

        public MainWindow(Profile User, GameQuizzSet Quiz)
        {
            InitializeComponent();
            App.User = User;
            isShortCutted = true;
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            synch = SynchronizationContext.Current;
            StartQuiz(Quiz);
        }

        public MainWindow(Profile User)
        {
            InitializeComponent();
            App.User = User;
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            synch = SynchronizationContext.Current;
        }

        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainWindow_Loaded);
            synch = SynchronizationContext.Current;
        }

        void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            checkLogin();
            quizzes = GameQuizzSet.GetQuizzSet().ToList<GameQuizzSet>();
            if (quizzes.Count > 0)
            {
                lbxQuizzes.Items.Clear();
                lbxQuizzes.ItemsSource = quizzes;
            }
        }

        void checkLogin()
        {
            if (App.User == null)
            {
                this.Hide();
                UserProfileManager usrSelector = new UserProfileManager() 
                { 
                    WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen,
                };
                bool? result = usrSelector.ShowDialog();
                if (result.HasValue && result.Value)
                    this.Show();
                else
                    Environment.Exit(0);
            }
        }

        private void btnSelect_Click(object sender, RoutedEventArgs e)
        {
            if(lbxQuizzes.SelectedIndex > -1)
            {
                StartQuiz(lbxQuizzes.Items[lbxQuizzes.SelectedIndex] as GameQuizzSet);
            }
        }

        private void StartQuiz(GameQuizzSet quiz)
        {
            this.Hide();
            QuizzContainer quizz = new QuizzContainer()
            {
                QSet = quiz,
            };

            if (isShortCutted)
            {
                quizz.Show();
                Close();
            }
            else
            {
                quizz.ShowDialog();
                this.Show();
            }
        }
    }
}
