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
using Quizz.QuizzSet;
using Quizz.StringFreeReponse;
using Quizz;
using System.Xml.Linq;

namespace GameEngine.GameQuizzDataTypes
{
    public class GameProblem : StringFreeResponseProblem
    {
        bool isEnabled = true;
        List<ScoreSystem.Score> score = new List<ScoreSystem.Score>();
        List<GameAnswer> answers;
        private QuizzSettings.QuestionSettings settings;

        #region Public Properties
        public bool IsEnabled
        {
            get { return isEnabled; }
            set { isEnabled = value; }
        }

        public List<ScoreSystem.Score> Score
        {
            get { return score; }
            set { score = value; }
        }

        public QuizzSettings.QuestionSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        public static bool operator ==(GameProblem First,
            GameProblem Second)
        {
            if(!object.ReferenceEquals(First,Second))
                return false;
            else if (!(First is GameProblem)
                && !(Second is GameProblem))
                return false;

            return First.Equals(Second);
        }

        public static bool operator !=(GameProblem First,
            GameProblem Second)
        {
            return !(First == Second);
        }
        #endregion

        public GameProblem()
            : base()
        {
            settings = new QuizzSettings.QuestionSettings();
            loadComponents();
        }

        public GameProblem(string Question, string[] Answer)
            : base(Question, Answer)
        {
            settings = new QuizzSettings.QuestionSettings();
            loadComponents();
        }

        public GameProblem(string Question, string[] Answer, ProblemType Kind)
            : base(Question, Answer, Kind)
        {
            settings = new QuizzSettings.QuestionSettings();
            loadComponents();
        }

        public GameProblem(string Question, string[] Answer, QuizzSettings.QuestionSettings Settings)
            : base(Question, Answer)
        {
            this.settings = Settings;
            loadComponents();
        }

        public GameProblem(string Question, string[] Answer, ProblemType Kind,
            QuizzSettings.QuestionSettings Settings)
            : base(Question, Answer, Kind)
        {
            this.settings = Settings;
            loadComponents();
        }

        public GameProblem(XElement elmProblem)
            : base(elmProblem)
        {
            this.settings = new QuizzSettings.QuestionSettings(elmProblem);
            loadComponents();
        }

        public GameProblem(StringFreeResponseProblem Prob, XElement Settings)
            : base(Prob)
        {
            this.settings = new QuizzSettings.QuestionSettings(Settings);
            loadComponents();
        }

        private void loadComponents()
        {
            answers = new List<GameAnswer>();
            OnAnswersLoaded += new AnswersLoadedEventHandler(GameProblem_OnAnswersLoaded);
            OnSaveBindings += new OnXMLBindingEventHandler(GameProblem_OnSaveBindings);
        }

        void GameProblem_OnSaveBindings(object sender, List<XElement> Entries)
        {
            Entries.Add(settings.ToXElement());
        }


        void GameProblem_OnAnswersLoaded(object sender, XElement Answer)
        {
            answers.Add(new GameAnswer(Answer));
        }

        public override bool Equals(object obj)
        {
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }


    }
}
