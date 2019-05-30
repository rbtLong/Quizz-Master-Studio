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
using Quiz = global::Quizz;
using WPF.MDI;
using System.Threading;
using System.Windows;
using GameEngine.GameQuizzDataTypes;
using UserQuizzPreferences;

namespace QuizzMasterStudio.Quizz
{
    public class ProblemSession
    {
        bool isForced = false;
        bool isNew = false;
        SynchronizationContext synch;
        GameQuizzSet qSet;
        GameProblem prob;
        MdiChild wnd;
        QuizzUI.MainWindow quizUI = null;
        Question.FreeResponseQuestion wndPrb;
        bool ignoreSessionCloseEvent = false;

        public event SaveMadeEventHandler SaveMade;
        public delegate void SaveMadeEventHandler(ProblemSession sender);

        public event SessionClosedEventHandler SessionClosed;
        public delegate void SessionClosedEventHandler(ProblemSession sender);

        #region Public Property
        public bool SessionChanged
        {
            get { return wndPrb.IsChanged; }
        }

        public MdiChild Wnd
        {
            get { return wnd; }
            set { wnd = value; }
        }

        public Quiz.QuizzSet.QuizzSet QSet
        {
            get { return qSet; }
        }

        public GameProblem Prob
        {
            get { return prob; }
        }

        public bool IsNew
        {
            get { return isNew; }
        }
        #endregion

        public ProblemSession(GameQuizzSet QuizzSet, GameProblem Problem, 
            MdiChild Window, Question.FreeResponseQuestion WndProb)
        {
            synch = SynchronizationContext.Current;
            qSet = QuizzSet;
            prob = Problem;
            wnd = Window;
            wndPrb = WndProb;

            isNew = (qSet.Problems.IndexOf(prob) == -1);

            wndPrb.ChangesMade += new Question.FreeResponseQuestion.ChangesMadeEventHandler(wndPrb_ChangesMade);
            wndPrb.CancelClicked += new Question.FreeResponseQuestion.CancelClickedEventHandler(wndPrb_CancelClicked);
            wnd.Closing += new System.Windows.RoutedEventHandler(wnd_Closing);
            wndPrb.AppliedClicked += new Question.FreeResponseQuestion.ApplyClickedEventHandler(wndPrb_AppliedClicked);
            wndPrb.tbxProbName.TextChanged += new System.Windows.Controls.TextChangedEventHandler(tbxProbName_TextChanged);
            wndPrb.btnLaunchQuizz.Click += new RoutedEventHandler(btnLaunchQuizz_Click);
        }

        void btnLaunchQuizz_Click(object sender, RoutedEventArgs e)
        {
            if (quizUI == null)
            {
                quizUI = new QuizzUI.MainWindow(App.User, qSet);
                quizUI = null;
            }
            else
                quizUI.Focus();
        }

        void tbxProbName_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            wnd.Title = string.Format("Question: {0}.) {1} [{2}]",
                                qSet.Problems.IndexOf(prob)+1,
                                prob.Name,
                                qSet);
        }

        void wnd_Closing(object sender, System.Windows.RoutedEventArgs e)
        {
            if (!isForced)
            {
                if (wndPrb.IsChanged)
                    switch (MessageBox.Show("Would you like to save the changes before exiting?",
                        String.Format("Save Changes? [{0}]", wnd.Title),
                        MessageBoxButton.YesNoCancel))
                    {
                        case MessageBoxResult.Yes:
                            if (isNew)
                            {
                                qSet.Problems.Add(prob);
                                isNew = false;
                            }

                            SaveChanges();
                            break;

                        case MessageBoxResult.Cancel:
                            e.Handled = true;
                            return;
                    }

                if (SessionClosed != null && !ignoreSessionCloseEvent)
                    SessionClosed(this);
            }
        }

        void wndPrb_CancelClicked(object sender)
        {
            wnd.CloseGracefully();
        }

        void wndPrb_ChangesMade(object sender)
        {
            synch.Post((o) =>
            {
                int cPos = wnd.Title.LastIndexOf('*');
                bool asteriskExists = cPos == wnd.Title.Length - 1;

                if (wndPrb.IsChanged && !asteriskExists)
                    if (wnd.Title.Trim().Length > 0 && !asteriskExists)
                        wnd.Title += '*';
                else
                    if (asteriskExists)
                        wnd.Title = wnd.Title.Remove(cPos, 1);
            }, null);
        }

        public void SaveChanges()
        {
            if (wndPrb.IsChanged)
                wndPrb.ApplyChanges();

            if (isNew)
            {
                qSet.Problems.Add(prob);
                isNew = false;
            }

            qSet.SaveFile();

            if (SaveMade != null)
                SaveMade(this);
        }

        public void SaveAsChanges(string Path)
        {
            if (isNew)
            {
                qSet.Problems.Add(prob);
                isNew = false;
            }

            qSet.SaveAsFile(Path);
        }

        void wndPrb_AppliedClicked(object sender, GameProblem Problem)
        {
            SaveChanges();
        }

        public void CloseGracefully(bool IgnoreClosingEvent)
        {
            ignoreSessionCloseEvent = IgnoreClosingEvent;
            wnd.CloseGracefully();
        }

        public void ForceClose()
        {
            ignoreSessionCloseEvent = true;
            isForced = true;
            wnd.Close();
        }
    }
}
