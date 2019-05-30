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
using ScoreSystem;
using System.Xml.Linq;
using LinqXMLTools;
using System.IO;

namespace GameUserStatistics
{
    public class GameResult
    {
        List<ProblemScore> scores = new List<ProblemScore>();
        DateTime started;
        DateTime ended;
        TimeSpan timeElapsed;

        #region Public Members
        public List<ProblemScore> Scores
        {
            get { return scores; }
            set { scores = value; }
        }

        public double TotalScore
        {
            get 
            {
                double total = 0;
                foreach (ProblemScore itm in scores)
                    foreach(Score score in itm.Scores)
                        total += score.CurrentScore;
                return total;
            }
        }

        public int IncorrectAttempts
        {
            get 
            {
                int wrong = 0;
                foreach (ProblemScore itm in scores)
                    foreach (Score score in itm.Scores)
                        if (!score.IsCorrect)
                            wrong++;
                return wrong;
            }
        }

        public int CorrectAttempts
        {
            get
            {
                int correct = 0;
                foreach (ProblemScore itm in scores)
                    foreach (Score score in itm.Scores)
                        if (score.IsCorrect)
                            correct++;
                return correct;
            }
        }

        public int TotalAttempts
        {
            get
            {
                return scores.Count;
            }
        }

        public DateTime Started
        {
            get { return started; }
        }

        public DateTime Ended
        {
            get
            {
                return ended;
            }
        }

        public TimeSpan TimeElapsed
        {
            get { return timeElapsed; }
        }
        #endregion

        public GameResult(DateTime Started, DateTime Ended, 
            TimeSpan ElapsedTime, IEnumerable<ProblemScore> Scores)
        {
            scores = Scores.ToList<ProblemScore>();
            this.started = Started;
            this.ended = Ended;
            timeElapsed = ElapsedTime;
        }

        public GameResult(XElement XResult)
        {
            XElement xRes;
            if (XResult.Name == "GameResult")
                xRes = XResult;
            else if (XResult.Element("GameResult") != null)
                xRes = XResult.Element("GameResult");
            else
                throw new Exception("Result XElement is invalid.");

            XMLParser.ParseAttribute(xRes, "Started", ref started);
            XMLParser.ParseAttribute(xRes, "Ended", ref ended);
            XMLParser.ParseAttribute(xRes, "TimeElapsed", ref timeElapsed);
            XElement[] xProb = (from c in xRes.Elements("ProblemScore")
                                select c).ToArray<XElement>();

            foreach (XElement itm in xProb)
                scores.Add(new ProblemScore(itm));
        }

        public XElement ToXElement()
        {
            XElement[] probs = (from c in scores
                                select c.ToXElement()).ToArray<XElement>();

            return new XElement("GameResult",
                new XAttribute("TotalScore", TotalScore),
                new XAttribute("IncorrectAttempts", CorrectAttempts),
                new XAttribute("CorrectAttempts", CorrectAttempts),
                new XAttribute("TotalAttempts", TotalAttempts),
                new XAttribute("Started", started.ToString()),
                new XAttribute("Ended", ended.ToString()),
                new XAttribute("TimeElapsed", timeElapsed.ToString()),
                probs);
        }

    }
}
