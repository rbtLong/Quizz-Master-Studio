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
using System.Threading;
using System.Windows.Media.Animation;
using GameUserStatistics;
using ScoreSystem;
using System.IO;

namespace QuizzUI
{
    /// <summary>
    /// Interaction logic for QuizzContainer.xaml
    /// </summary>
    public partial class QuizzContainer : Window
    {
        private SynchronizationContext synch;
        private GameEngine.Core game;
        private GameQuizzSet qSet = null;

        public GameQuizzSet QSet
        {
            get { return qSet; }
            set { qSet = value; }
        }

        public QuizzContainer()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(QuizzContainer_Loaded);
            synch = SynchronizationContext.Current;
        }

        void QuizzContainer_Loaded(object sender, RoutedEventArgs e)
        {
            if (qSet != null)
                game = new GameEngine.Core(qSet);

            game.OnGameStateChanged += new GameEngine.Core.GameStateChangedEventHandler(game_OnGameStateChanged);
            game.OnGameClockTick += new GameEngine.Core.GameClockTickEventHandler(game_OnGameClockTick);
            game.OnNewQuestion += new GameEngine.Core.NewQuestionEventHandler(game_OnNewQuestion);
            game.OnScoreChanged += new GameEngine.Core.ScoreChangedEventHandler(game_OnScoreChanged);
            game.OnScoreIncreased += new GameEngine.Core.ScoreChangedEventHandler(game_OnScoreIncreased);
            game.OnScoreDecreased += new GameEngine.Core.ScoreChangedEventHandler(game_OnScoreDecreased);
            game.OnWrongAnswer += new GameEngine.Core.OnAnswerEventHandler(game_OnWrongAnswer);
            game.OnRightAnswer += new GameEngine.Core.OnAnswerEventHandler(game_OnRightAnswer);
            game.OnGameCompleted += new GameEngine.Core.GameCompletedEventHandler(game_OnGameCompleted);

            ctrlQuizz.OnSubmit += new Quizz.FreeResponse.OnSubmitEventHandler(ctrlQuizz_OnSubmit);
            ctrlQuizz.btnPause.Click += new RoutedEventHandler(btnPause_Click);

            this.Title = string.Format("Running: {0}", qSet.Name);
        }

        void game_OnGameStateChanged(object o, GameEngine.GameState NewState)
        {
            
        }

        void game_OnGameCompleted(object o, EventArgs args)
        {
            ctrlQuizz.IsEnabled = false;
            MessageBox.Show("Done!");
            Thread thdResult = new Thread(new ThreadStart(() =>
                {
                    List<ProblemScore> scrs = new List<ProblemScore>();
                    foreach (GameProblem itm in qSet.Problems)
                        scrs.Add(new ProblemScore(itm.Id, itm.Score));
                    GameResultCollection grc = new GameResultCollection(qSet.Name, qSet.Id,
                        new GameResult(game.GameStarted, game.GameEnded, game.TimeLapsed, scrs));
                    grc.SaveToFile(new DirectoryInfo(App.User.DefaultGameResultLocation));

                    new WndGameResult()
                    {
                        QSet = qSet,
                    }.ShowDialog();

                }));
            thdResult.SetApartmentState(ApartmentState.STA);
            thdResult.Start();
        }

        void game_OnScoreDecreased(object o, ScoreSystem.Score Amount, double TotalScore)
        {
            fxLblScore.Visibility = System.Windows.Visibility.Visible;
            fxLblScore.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            fxLblScore.Content = Amount.CurrentScore.ToString("00.00");
            Storyboard fx = (Resources["ScoreScrollUp"] as Storyboard);
            fx.Stop();
            fx.Completed += (ob, s) =>
            {
                fx.Stop();
                synch.Post((obj) =>
                {
                    fxLblScore.Visibility = System.Windows.Visibility.Collapsed;
                }, null);
            };
            fx.Begin();
        }

        void game_OnScoreIncreased(object o, ScoreSystem.Score Amount, double TotalScore)
        {
            fxLblScore.Visibility = System.Windows.Visibility.Visible;
            fxLblScore.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            fxLblScore.Visibility = System.Windows.Visibility.Visible;
            fxLblScore.Content = ("+"+Amount.CurrentScore.ToString("00.00"));
            Storyboard fx = (Resources["ScoreScrollUp"] as Storyboard);
            fx.Stop();
            fx.Completed += (ob, s) =>
            {
                fx.Stop();
                synch.Post((obj) =>
                {
                    fxLblScore.Visibility = System.Windows.Visibility.Collapsed;
                }, null);
            };
            fx.Begin();
        }

        void game_OnRightAnswer(object o, GameProblem Question)
        {
            synch.Post((ob) =>
                ctrlQuizz.TriggerCorrectAnswerEffect(), null);
        }

        void game_OnWrongAnswer(object o, GameProblem Question)
        {
            synch.Post((ob) =>
                ctrlQuizz.TriggerWrongAnswerEffect(), null);
        }

        void game_OnScoreChanged(object o, ScoreSystem.Score Amount, double TotalScore)
        {
            synch.Post((ob) =>
                lblScore.Content = string.Format("Score: {0:0.00}", TotalScore),
                null);
        }

        void ctrlQuizz_OnSubmit(object sender, string Answer)
        {
            game.Grade(Answer);
        }

        void game_OnNewQuestion(object o, GameProblem Question, int ProblemIdx, DateTime TimeGenerated)
        {
            synch.Post((ob) =>
                {
                    ctrlQuizz.LoadQuestion(Question);
                }, null);
        }

        void game_OnGameClockTick(object o, TimeSpan ElapsedGameTime)
        {
            synch.Post((ob) =>
                {
                    lblTime.Content = string.Format("Time Elapsed: {1:00}'{00:ss}\"",
                        ElapsedGameTime, Convert.ToInt32(ElapsedGameTime.TotalMinutes));
                }, null);
        }

        void btnPause_Click(object sender, RoutedEventArgs e)
        {
            grdGame.IsEnabled = false;
            game.StopGame();
            premessage.Visibility = System.Windows.Visibility.Visible;
            this.Focus();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter && !grdGame.IsEnabled)
            {
                grdGame.IsEnabled = true;
                premessage.Visibility = System.Windows.Visibility.Collapsed;
                game.PlayGame();
                this.ctrlQuizz.tbxAnswer.Focus();
            }
        }

        private void lblShowAnswer_Click(object sender, RoutedEventArgs e)
        {
            string ans = string.Empty;
            foreach (string itm in game.CurrentQuestion.Answer)
                ans += (itm + "\n");
            MessageBox.Show(ans);
        }

        private void miExit_Click(object sender, RoutedEventArgs e)
        {
            Environment.Exit(0);
        }

    }
}
