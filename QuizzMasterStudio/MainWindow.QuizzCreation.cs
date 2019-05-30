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
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Controls;
using WPF.MDI;
using System.Windows.Media.Imaging;
using Quiz = global::Quizz;
using QuizzMasterStudio.Quizz;
using System.IO;
using GameEngine.GameQuizzDataTypes;

namespace QuizzMasterStudio
{
    partial class MainWindow
    {
        List<GameQuizzSet> qSet;
        List<ProblemSession> problemSess = new List<ProblemSession>();

        private void initializeQuizzCreation()
        {
            ctrlQuizz.MouseDoubleClick += new System.Windows.Input.MouseButtonEventHandler(ctrlQuizz_MouseDoubleClick);
            ctrlQuizz.QuizzSelectionChanged += new QuizzSelector.QuizzSelectionChangedEventHandler(ctrlQuizz_QuizzSelectionChanged);
            ctrlQuizz.btnAdd.Click += new RoutedEventHandler(btnAdd_Click);
            ctrlQuizz.btnDelete.Click += new RoutedEventHandler(btnDelete_Click);
            ctrlQuizz.btnEdit.Click += new RoutedEventHandler(btnEdit_Click);

            ctrlQuestion.QuestionSelectionChanged += new QuestionSelector.QuestionSelectionChangedEventHandler(ctrlQuestion_QuestionSelectionChanged);
            ctrlQuestion.miDeleteQuestion.Click += new RoutedEventHandler(miDeleteQuestion_Click);
            ctrlQuestion.miAddFreeResponseQuestion.Click += new RoutedEventHandler(miAddFreeResponseQuestion_Click);

            ctrlQuestion.expContainer.IsExpanded = (ctrlQuizz.lbxQuizzSelect.SelectedIndex > -1);
            reloadQuizzList();
        }

        void ctrlQuizz_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            editQuizzWindow();
        }

        void miAddFreeResponseQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlQuizz.lbxQuizzSelect.SelectedIndex > -1)
            {
                Question.FreeResponseQuestion qst = new Question.FreeResponseQuestion(new GameProblem(), true);

                qst.SaveAsClicked += new Question.FreeResponseQuestion.ApplyClickedEventHandler(qst_SaveAsClicked);
                qst.AppliedClicked += new Question.FreeResponseQuestion.ApplyClickedEventHandler(qst_AppliedClicked);

                WPF.MDI.MdiChild mChild = new WPF.MDI.MdiChild()
                {
                    WindowId = null,
                    Title = string.Format("[New Question] [{0}]", 
                        ctrlQuizz.lbxQuizzSelect.Items[ctrlQuizz.lbxQuizzSelect.SelectedIndex]),
                    Content = qst,
                    Background = new SolidColorBrush(Colors.White),
                    Width = 500,
                    Height = 400,
                    Icon = new BitmapImage(new Uri("/Images/document.png", UriKind.Relative)),
                };
                
                GameQuizzSet qui =
                    ctrlQuizz.lbxQuizzSelect.Items[ctrlQuizz.lbxQuizzSelect.SelectedIndex]
                    as GameQuizzSet;

                ProblemSession sess = new ProblemSession(qui, qst.GameProblem, mChild, qst);
                sess.SaveMade += new ProblemSession.SaveMadeEventHandler(sess_SaveMade);
                sess.SessionClosed += new ProblemSession.SessionClosedEventHandler(sess_SessionClosed);
                problemSess.Add(sess);
                mdiContainer.Children.Add(mChild);
            }
        }

        void miDeleteQuestion_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlQuestion.lbxQuestionSelect.SelectedIndex > -1)
            {
                GameQuizzSet qui = qSet[ctrlQuizz.lbxQuizzSelect.SelectedIndex];
                switch(MessageBox.Show(String.Format("Are you sure you want to delete the question: {0}",
                    qui.Name), "Delete Question?", MessageBoxButton.YesNo))
                {
                    case MessageBoxResult.Yes:
                        qui.Remove(ctrlQuestion.lbxQuestionSelect.SelectedIndex);
                        qui.SaveFile();
                        reloadQuizzList();
                        reloadQuestionList();
                        break;
                }
            }
        }

        private void reloadQuizzList()
        {
            int idx = ctrlQuizz.lbxQuizzSelect.SelectedIndex;
            string fullPath = GameQuizzSet.DefaultLocalPath;
            if (!Directory.Exists(fullPath))
                Directory.CreateDirectory(fullPath);
            else
                ctrlQuizz.lbxQuizzSelect.ItemsSource = qSet = GameQuizzSet.GetQuizzSet(fullPath).ToList();

            if (idx <= qSet.Count - 1)
                ctrlQuizz.lbxQuizzSelect.SelectedIndex = idx;
        }

        private void reloadQuestionList()
        {
            int sIdx = ctrlQuestion.lbxQuestionSelect.SelectedIndex;
            ctrlQuestion.lbxQuestionSelect.ItemsSource = (ctrlQuizz.lbxQuizzSelect.Items[ctrlQuizz.lbxQuizzSelect.SelectedIndex]
                as GameQuizzSet).Problems;
            ctrlQuestion.lbxQuestionSelect.SelectedIndex = sIdx;
        }

        void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (ctrlQuizz.lbxQuizzSelect.SelectedIndex > -1)
            {
                GameQuizzSet qui = ctrlQuizz.lbxQuizzSelect.Items[ctrlQuizz.lbxQuizzSelect.SelectedIndex]
                    as GameQuizzSet;

                ProblemSession[] sess = problemSess.FindAll(
                    (o) =>
                    {
                        return o.QSet == qui;
                    }).ToArray();

                foreach (ProblemSession itm in sess)
                {
                    itm.ForceClose();
                    problemSess.Remove(itm);
                }

                switch (MessageBox.Show(string.Format("Are you sure you want to delete the quizz: {0}?", qui.Name),
                    "Delete Confirm", MessageBoxButton.YesNo))
                {
                    case MessageBoxResult.Yes:
                        File.Delete(GameQuizzSet.FullFilePath(qui.Name));
                        reloadQuizzList();
                        break;

                    case MessageBoxResult.No:
                        break;
                }
            }
        }

        void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CreateQuizzDialog();
        }

        private void CreateQuizzDialog()
        {
            CreateQuizz cQuizz = new CreateQuizz() 
            {
                User = App.User,
            };

            bool? dial = cQuizz.ShowDialog();
            if (dial.HasValue && dial.Value)
            {
                cQuizz.Quizz.SaveFile();
                reloadQuizzList();
            }
        }

        void ctrlQuizz_QuizzSelectionChanged(object sender, SelectionChangedEventArgs e, ListBoxItem itm)
        {
            editSelectedQuizz();
        }

        void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            editQuizzWindow();
        }

        private void editSelectedQuizz()
        {
            ctrlQuestion.expContainer.IsExpanded = (ctrlQuizz.lbxQuizzSelect.SelectedIndex > -1);
            if (ctrlQuizz.lbxQuizzSelect.SelectedIndex > -1)
            {
                GameQuizzSet gm = ctrlQuizz.lbxQuizzSelect.Items[ctrlQuizz.lbxQuizzSelect.SelectedIndex]
                    as GameQuizzSet;
                ctrlQuestion.lbxQuestionSelect.ItemsSource = gm.Problems;
            }
        }

        private void editQuizzWindow()
        {
            if (ctrlQuizz.lbxQuizzSelect.SelectedIndex > -1)
            {
                Quizz.WndEditQuizz qEdit = new WndEditQuizz(App.User,
                    ctrlQuizz.lbxQuizzSelect.Items[ctrlQuizz.lbxQuizzSelect.SelectedIndex]
                    as GameQuizzSet);
                bool? result = qEdit.ShowDialog();
                if (result.HasValue && result.Value)
                {
                    qEdit.Quizz.SaveFile();
                    reloadQuizzList();
                }
            }
        }

        void ctrlQuestion_QuestionSelectionChanged(object sender, SelectionChangedEventArgs e, ListBoxItem itm)
        {
            if (ctrlQuestion.lbxQuestionSelect.SelectedIndex > -1 && ctrlQuizz.lbxQuizzSelect.SelectedIndex > -1)
            {
                synch.Post((o) =>
                {
                    Quizz.ProblemSession sess =
                        (problemSess.Find((ob) =>
                        {
                            return (ob.Prob.Id == (ctrlQuestion.lbxQuestionSelect.Items[ctrlQuestion.lbxQuestionSelect.SelectedIndex]
                                as GameProblem).Id);
                        }));

                    if (sess != null)
                        sess.Wnd.Focus();
                    else
                    {

                        Question.FreeResponseQuestion qst = new Question.FreeResponseQuestion(
                            ctrlQuestion.lbxQuestionSelect.Items[ctrlQuestion.lbxQuestionSelect.SelectedIndex]
                            as GameProblem, false);

                        qst.SaveAsClicked += new Question.FreeResponseQuestion.ApplyClickedEventHandler(qst_SaveAsClicked);
                        qst.AppliedClicked += new Question.FreeResponseQuestion.ApplyClickedEventHandler(qst_AppliedClicked);

                        WPF.MDI.MdiChild mChild = new WPF.MDI.MdiChild()
                        {
                            WindowId = ctrlQuestion.lbxQuestionSelect.Items[ctrlQuestion.lbxQuestionSelect.SelectedIndex],
                            Title = string.Format("Question: {0} [{1}]",
                                ctrlQuestion.lbxQuestionSelect.SelectedIndex + 1, ctrlQuizz.lbxQuizzSelect.Items[ctrlQuizz.lbxQuizzSelect.SelectedIndex]),
                            Content = qst,
                            Background = new SolidColorBrush(Colors.White),
                            Width = 500,
                            Height = 400,
                            Icon = new BitmapImage(new Uri("/Images/document.png", UriKind.Relative)),
                        };

                        sess = new ProblemSession(ctrlQuizz.lbxQuizzSelect.Items[ctrlQuizz.lbxQuizzSelect.SelectedIndex] as GameQuizzSet,
                            ctrlQuestion.lbxQuestionSelect.Items[ctrlQuestion.lbxQuestionSelect.SelectedIndex] as GameProblem, 
                            mChild, qst);
                        sess.SaveMade += new ProblemSession.SaveMadeEventHandler(sess_SaveMade);
                        sess.SessionClosed += new ProblemSession.SessionClosedEventHandler(sess_SessionClosed);
                        problemSess.Add(sess);
                        mdiContainer.Children.Add(mChild);
                    }
                }, null);
            }
        }

        void sess_SessionClosed(ProblemSession sender)
        {
            problemSess.Remove(sender);
        }

        void sess_SaveMade(ProblemSession sender)
        {
            reloadQuizzList();
        }

        void qst_SaveClicked(object sender, GameProblem Problem)
        {
            reloadQuestionList();
        }

        void qst_AppliedClicked(object sender, GameProblem Problem)
        {
            reloadQuestionList();
        }

        void qst_SaveAsClicked(object sender, GameProblem Problem)
        {
            reloadQuestionList();
        }

        private bool closeMdiSessions()
        {
            foreach (ProblemSession itm in problemSess)
                itm.CloseGracefully(true);

            return (mdiContainer.Children.Count > 0);
        }
    }
}
