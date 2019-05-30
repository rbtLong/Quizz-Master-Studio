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
using GameEngine.GameQuizzDataTypes;

namespace QuizzMasterStudio.Quizz
{
    /// <summary>
    /// Interaction logic for WndEditQuizz.xaml
    /// </summary>
    public partial class WndEditQuizz : Window
    {
        EditQuizz gmSettings;

        public EditQuizz GmSettings
        {
            get { return gmSettings; }
            set { gmSettings = value; }
        }

        public GameQuizzSet Quizz
        {
            get { return gmSettings.QuizzSet; }
        }

        public WndEditQuizz()
        {
            InitializeComponent();
            gmSettings = new EditQuizz(null, null, false);
            setupLayout();
        }

        public WndEditQuizz(Profile Usr, GameQuizzSet Quizz)
        {
            InitializeComponent();
            GmSettings = new EditQuizz(Quizz, Usr, false);
            this.grdSettings.Children.Add(gmSettings);
            setupLayout();
        }

        private void setupLayout()
        {
            lblTitle.Content = "Edit Quizz";
            this.Title = "[Edit Quizz]";
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnOk_Click(object sender, RoutedEventArgs e)
        {
            if(GmSettings.ValidateFoms(true))
                this.DialogResult = true;
        }
    }
}
