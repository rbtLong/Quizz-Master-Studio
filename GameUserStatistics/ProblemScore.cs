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

namespace GameUserStatistics
{
    public class ProblemScore
    {
        private string problemId;
        List<Score> score = new List<Score>();

        public string ProblemId
        {
            get { return problemId; }
            set { problemId = value; }
        }

        public List<Score> Scores
        {
            get { return score; }
            set { score = value; }
        }

        public ProblemScore(string ProblemId, List<Score> Score)
        {
            this.problemId = ProblemId;
            this.score = Score;
        }

        public ProblemScore(XElement XScore)
        {
            XElement xScr;
            if (XScore.Name == "ProblemScore")
                xScr = XScore;
            else if (XScore.Element("ProblemScore") != null)
                xScr = XScore.Element("ProblemScore");
            else
                throw new Exception("ProblemScore XElement is invalid.");

            XMLParser.ParseAttribute(xScr, "ProblemId", ref problemId);
            foreach (XElement itm in XScore.Elements("ScoreEntry"))
                score.Add(new Score(itm));
        }

        public XElement ToXElement()
        {
            XElement[] scrs = (from c in score
                               select c.ToXElement()).ToArray<XElement>();

            return new XElement("ProblemScore",
                new XAttribute("ProblemId", problemId),
                scrs);
        }

    }
}
