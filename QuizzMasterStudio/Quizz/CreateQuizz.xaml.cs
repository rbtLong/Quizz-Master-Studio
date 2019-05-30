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
using Quiz = global::Quizz;
using System.IO;
using GameEngine.GameQuizzDataTypes;

namespace QuizzMasterStudio.Quizz
{
    /// <summary>
    /// Interaction logic for CreateQuizz.xaml
    /// </summary>
    public partial class CreateQuizz : Window
    {

        #region Public Properties
        public GameQuizzSet Quizz
        {
            get { return gmSetting.QuizzSet; }
        }

        public Profile User
        {
            get { return gmSetting.User; }
            set { gmSetting.User = value; }
        }
        #endregion

        public CreateQuizz()
        {
            InitializeComponent();
            gmSetting.IsNew = true;
            this.Loaded += new RoutedEventHandler(CreateQuizz_Loaded);

        }

        void CreateQuizz_Loaded(object sender, RoutedEventArgs e)
        {
            lblTitle.Content = "New Quizz";
            this.Title = "[New Quizz]";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            if(gmSetting.ValidateFoms(true))
                this.DialogResult = true;
        }
    }
}
