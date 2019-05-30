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

namespace QuizzMasterStudio.Question
{
    /// <summary>
    /// Interaction logic for AnswerEditor.xaml
    /// </summary>
    public partial class AnswerEditor : Window
    {
        public AnswerEditor()
        {
            InitializeComponent();
        }

        public AnswerEditor(string Answer)
        {
            InitializeComponent();
            this.tbxAnswer.Text = Answer;
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void btnOkay_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void cbxAnswerSettings_Checked(object sender, RoutedEventArgs e)
        {
            grdAnswerSettings.IsEnabled = true;
        }

        private void cbxAnswerSettings_Unchecked(object sender, RoutedEventArgs e)
        {
            grdAnswerSettings.IsEnabled = false;
        }
    }
}
