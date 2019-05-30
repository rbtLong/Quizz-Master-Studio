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
using System.IO;
using GameEngine.GameQuizzDataTypes;
using System.Windows.Media.Animation;
using System.Threading;

namespace QuizzUI.Quizz
{
    /// <summary>
    /// Interaction logic for FreeResponse.xaml
    /// </summary>
    public partial class FreeResponse : UserControl
    {
        public event OnSubmitEventHandler OnSubmit;
        public delegate void OnSubmitEventHandler(object sender, string Answer);

        private GameProblem prb = null;
        private SynchronizationContext synch;
        private int seed = 1;

        public GameProblem Problem
        {
            get { return prb; }
            set { prb = value; }
        }

        public FreeResponse()
        {
            InitializeComponent();
            synch = SynchronizationContext.Current;
        }

        public void LoadQuestion(GameProblem Problem)
        {
            tbxAnswer.Clear();
            tbxAnswer.Focus();
            if (prb != null)
            {
                byte[] buff = new byte[Problem.Question.Length];
                for (int i = 0; i < buff.Length; ++i)
                    buff[i] = Convert.ToByte(Problem.Question[i]);

                if (Problem != null && Problem.Question != null && Problem.Question.Length > 0)
                {
                    TextRange trData = new TextRange(rtbQuestion.Document.ContentStart,
                        rtbQuestion.Document.ContentEnd);
                    trData.Load(new MemoryStream(buff, true), DataFormats.Rtf);
                }
            }
        }

        private void tbxAnswer_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return || e.Key == Key.Enter)
                if(OnSubmit != null)
                    OnSubmit(this, tbxAnswer.Text);
        }

        public void TriggerCorrectAnswerEffect()
        {
            lblText.Visibility = System.Windows.Visibility.Visible;
            lblText.Foreground = new SolidColorBrush(Color.FromRgb(0, 255, 0));
            lblText.Content = GenerateRandomCorrectAnswerPhrase();
            Storyboard fx = (Resources["ScrollUpFadeOut"] as Storyboard);
            fx.Stop();
            fx.Completed += (o,s) =>
                {
                    fx.Stop();
                    synch.Post((ob) =>
                        {
                            lblText.Visibility = System.Windows.Visibility.Collapsed;
                        }, null);
                };
            fx.Begin();
        }

        public void TriggerWrongAnswerEffect()
        {
            lblText.Visibility = System.Windows.Visibility.Visible;
            lblText.Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            lblText.Content = GenerateRandomWrongAnswerPhrase();
            Storyboard fx = (Resources["ScrollUpFadeOut"] as Storyboard);
            fx.Stop();
            fx.Completed += (o, s) =>
            {
                fx.Stop();
                synch.Post((ob) =>
                {
                    lblText.Visibility = System.Windows.Visibility.Collapsed;
                }, null);
            };
            fx.Begin();
        }

        private string GenerateRandomCorrectAnswerPhrase()
        {
            DateTime dt = DateTime.Now;
            int seed1 = (((dt.Millisecond / 2) * 3) % 100) + seed,
                seed2 = ((dt.Millisecond % 100) + dt.Second) + seed++;
            Random rand1 = new Random(seed1),
                   rand2 = new Random(seed2);

            return string.Format("{0}{1}",
                EffectPhrases.CorrectPhrases[rand1.Next(0, EffectPhrases.CorrectPhrases.Length)],
                EffectPhrases.PositiveExpressionMarks[rand2.Next(0, EffectPhrases.PositiveExpressionMarks.Length)]);
        }

        private string GenerateRandomWrongAnswerPhrase()
        {
            DateTime dt = DateTime.Now;
            int seed1 = (((dt.Millisecond / 5) * 2) % 100) + seed,
                seed2 = ((dt.Millisecond % 100) + dt.Second) + seed++;
            Random rand1 = new Random(seed1),
                   rand2 = new Random(seed2);

            return string.Format("{0}{1}",
                EffectPhrases.WrongPhrases[rand1.Next(0, EffectPhrases.WrongPhrases.Length)],
                EffectPhrases.NegativeExpressionMarks[rand2.Next(0, EffectPhrases.NegativeExpressionMarks.Length)]);
        }

    }
}
