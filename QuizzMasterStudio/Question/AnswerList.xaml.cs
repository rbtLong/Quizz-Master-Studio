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

namespace QuizzMasterStudio.Question
{
    /// <summary>
    /// Interaction logic for AnswerList.xaml
    /// </summary>
    public partial class AnswerList : UserControl
    {
        public event AnswerListDeletedEventHandler AnswerDeleted;
        public delegate void AnswerListDeletedEventHandler(object sender, int Index, string Answer);

        public event AnswerListAddedEventHandler AnswerAdded;
        public delegate void AnswerListAddedEventHandler(object sender, string Answer);

        public event AnswerListEditedEventHandler AnswerEdited;
        public delegate void AnswerListEditedEventHandler(object sender, int Position, string OldAnswer, string NewAnswer);

        public AnswerList()
        {
            InitializeComponent();
        }

        private void btnAddAnswer_Click(object sender, RoutedEventArgs e)
        {
            if (lbxAnswers.Items.IndexOf(tbxAnswer.Text) != -1)
            {
                MessageBox.Show("That answer entry already exists.");
                return;
            }
            if (tbxAnswer.Text.Trim().Length == 0)
            {
                MessageBox.Show("Enter a value for your answer.");
                return;
            }

            string nAns = String.Copy(tbxAnswer.Text);
            tbxAnswer.Clear();

            if (AnswerAdded != null)
                AnswerAdded(this, nAns);
        }

        private void miEdit_Click(object sender, RoutedEventArgs e)
        {
            if (lbxAnswers.SelectedIndex > -1)
            {
                string oldAnswer = lbxAnswers.Items[lbxAnswers.SelectedIndex].ToString(),
                       newAnswer = string.Empty;

                AnswerEditor aEditor = new AnswerEditor();
                aEditor.tbxAnswer.Text = lbxAnswers.Items[lbxAnswers.SelectedIndex].ToString();
                bool? isOK = aEditor.ShowDialog();
                if (isOK.HasValue && isOK.Value)
                {
                    newAnswer = aEditor.tbxAnswer.Text;
                    if (AnswerEdited != null)
                        AnswerEdited(this, lbxAnswers.SelectedIndex, oldAnswer, newAnswer);
                }
            }
        }

        private void miDelete_Click(object sender, RoutedEventArgs e)
        {
            if (lbxAnswers.SelectedIndex > -1)
            {
                if (AnswerDeleted != null)
                    AnswerDeleted(this, lbxAnswers.SelectedIndex, lbxAnswers.Items[lbxAnswers.SelectedIndex] as string);
            }
        }

        private void miPopOut_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
