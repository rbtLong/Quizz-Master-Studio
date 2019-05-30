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

namespace GameUserStatistics.ScoreBoard
{
    /// <summary>
    /// Interaction logic for ScoreBoardItem.xaml
    /// </summary>
    public partial class ScoreBoardItem : UserControl
    {
        GameResult gmResult;

        public GameResult GmResult
        {
            get { return gmResult; }
            set 
            { 
                gmResult = value;
                UpdateLabels();
            }
        }

        public ScoreBoardItem()
        {
            InitializeComponent();
        }

        public ScoreBoardItem(GameResult Result)
        {
            InitializeComponent();
            this.gmResult = Result;
            UpdateLabels();
        }

        private void UpdateLabels()
        {
            lblDate.Content = gmResult.Ended.ToString("MMM/dd");
            lblScore.Content = gmResult.TotalScore.ToString("0.00");
            lblElapsedTime.Content = string.Format("{0:mm}'{0:ss}\".{0:ff}", 
                gmResult.TimeElapsed);
        }
    }
}
