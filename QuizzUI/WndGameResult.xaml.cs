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
using Quiz = global::Quizz;
using GameEngine.GameQuizzDataTypes;
using GameUserStatistics.ScoreBoard;
using GameUserStatistics;

namespace QuizzUI
{
    /// <summary>
    /// Interaction logic for WndGameResult.xaml
    /// </summary>
    public partial class WndGameResult : Window
    {
        GameQuizzSet qSet;
        GameResultCollection gmResult;

        public GameQuizzSet QSet
        {
            get { return qSet; }
            set 
            { 
                qSet = value;
                UpdateSubTitle();

                GmResult = new GameResultCollection(qSet.Id,
                    new System.IO.DirectoryInfo(App.User.DefaultGameResultLocation));
            }
        }

        public GameResultCollection GmResult
        {
            get { return gmResult; }
            set
            { 
                gmResult = value; 
                for(int i=gmResult.Results.Count; i != 0; --i)
                    scoreBoard.LbxScoreBoard.Items.Add(
                        new ScoreBoardItem(gmResult.Results[i-1]) 
                        { 
                            HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch,
                        }); 

            }
        }


        public WndGameResult()
        {
            InitializeComponent();
        }

        private void UpdateSubTitle()
        {
            scoreBoard.QuizName = string.Format("[{0}]", qSet.Name);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void scoreBoard_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if(e.LeftButton == MouseButtonState.Pressed)
                this.DragMove();
        }
    }
}
