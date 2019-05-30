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
using GameUserStatistics;

namespace QuizzUI
{
    /// <summary>
    /// Interaction logic for ListBoxQuizzItm.xaml
    /// </summary>
    public partial class ListBoxQuizzItm : UserControl
    {
        GameProblem problem;
        int idx = 0;

        public int Index
        {
            get { return idx; }
            set { idx = value; }
        }

        public GameProblem Problem
        {
            get { return problem; }
            set { problem = value; }
        }

        public ListBoxQuizzItm()
        {
            InitializeComponent();
        }

        public ListBoxQuizzItm(GameProblem Problem, int Index)
        {
            InitializeComponent();
            this.problem = Problem;
            this.Loaded += new RoutedEventHandler(ListBoxQuizzItm_Loaded);
        }

        void ListBoxQuizzItm_Loaded(object sender, RoutedEventArgs e)
        {
            if (problem != null)
            {
                lblEnumerate.Content = idx;
                tbxQuizzName.Text = problem.Question;
            }
        }

        private void cbxEnable_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
