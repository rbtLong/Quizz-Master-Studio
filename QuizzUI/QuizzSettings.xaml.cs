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
using GameEngine.GameQuizzDataTypes;

namespace QuizzUI
{
    /// <summary>
    /// Interaction logic for QuizzSettings.xaml
    /// </summary>
    public partial class QuizzSettings : Window
    {
        GameQuizzSet quizz = null;

        public GameQuizzSet Quizz
        {
            get { return quizz; }
            set { quizz = value; }
        }

        public QuizzSettings()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(QuizzSettings_Loaded);
        }

        void QuizzSettings_Loaded(object sender, RoutedEventArgs e)
        {
            if (quizz != null)
            {
                int i = 0;
                foreach (GameProblem itm in quizz.Problems)
                    plbQuizzList.lbxProblems.Items.Add(
                        new ListBoxQuizzItm(itm,i++));
            }
        }

    }
}
