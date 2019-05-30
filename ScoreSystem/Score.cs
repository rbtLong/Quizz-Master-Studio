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
using System.Xml.Linq;
using LinqXMLTools;

namespace ScoreSystem
{
    public class Score
    {
        bool isCorrect;
        double currentScore;
        int conseuctiveStreak;

        DateTime timeStamp = DateTime.Now;
        TimeSpan responseTime = new TimeSpan(0);

        #region Public Fields
        public bool IsCorrect
        {
            get { return isCorrect; }
            set { isCorrect = value; }
        }

        public TimeSpan ResponseTime
        {
            get { return responseTime; }
            set { responseTime = value; }
        }

        public DateTime TimeStamp
        {
            get { return timeStamp; }
            set { timeStamp = value; }
        }

        public int ConseuctiveStreak
        {
            get { return conseuctiveStreak; }
            set { conseuctiveStreak = value; }
        }

        public double CurrentScore
        {
            get { return currentScore; }
            set { currentScore = value; }
        }
        #endregion

        public Score(bool IsCorrect, double Score, int Streak,
            DateTime Time, TimeSpan ResponseLength)
        {
            this.isCorrect = IsCorrect;
            currentScore = Score;
            timeStamp = Time;
            responseTime = ResponseLength;
            conseuctiveStreak = Streak;
        }

        public Score(XElement XScore)
        {
            XElement xScr;
            if (XScore.Name == "ScoreEntry")
                xScr = XScore;
            else if (XScore.Element("ScoreEntry") != null)
                xScr = XScore.Element("ScoreEntry");
            else
                throw new Exception("Input Element Is NOT a ScoreEntry.");

            XMLParser.ParseAttribute(XScore, "IsCorrect", ref isCorrect);
            XMLParser.ParseAttribute(XScore, "CurrentScore", ref currentScore);
            XMLParser.ParseAttribute(XScore, "ConsecutiveStreak", ref conseuctiveStreak);
            XMLParser.ParseAttribute(XScore, "TimeStamp", ref timeStamp);
            XMLParser.ParseAttribute(XScore, "ReponseTime", ref responseTime);

        }

        public XElement ToXElement()
        { 
            return (new XElement("ScoreEntry",
                new XAttribute("IsCorrect", isCorrect),
                new XAttribute("CurrentScore", currentScore),
                new XAttribute("ConsecutiveStreak", conseuctiveStreak),
                new XAttribute("TimeStamp", timeStamp),
                new XAttribute("ResponseTime", responseTime)
                ));
        }
    }
}
