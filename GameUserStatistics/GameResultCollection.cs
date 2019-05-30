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
using System.IO;
using LinqXMLTools;

namespace GameUserStatistics
{
    public class GameResultCollection
    {
        private List<GameResult> results = new List<GameResult>();
        string quizName;
        string quizId;

        public string QuizName
        {
            get { return quizName; }
            set { quizName = value; }
        }

        public string QuizId
        {
            get { return quizId; }
            set { quizId = value; }
        }

        public List<GameResult> Results
        {
            get { return results; }
            set { results = value; }
        }

        public GameResultCollection(string QuizName, string QuizId)
        {
            quizName = QuizName;
            quizId = QuizId;
        }

        public GameResultCollection(string QuizName, string QuizId,
            IEnumerable<GameResult> Results)
            : this(QuizName, QuizId)
        {
            this.results = Results.ToList<GameResult>();
        }

        public GameResultCollection(string QuizName, string QuizId, GameResult Result)
        {
            this.quizId = QuizId;
            this.quizName = QuizName;
            results.Add(Result);
        }

        public GameResultCollection(FileInfo FileInf)
        {
            XDocument doc = XDocument.Load(FileInf.FullName);
            XElement xGrc = doc.Element("GameResultCollection");

            XMLParser.ParseAttribute(xGrc, "QuizName", ref quizName);
            XMLParser.ParseAttribute(xGrc, "QuizId", ref quizId);
            XElement[] scrs = xGrc.Elements("GameResult").ToArray<XElement>();

            foreach (XElement itm in scrs)
                results.Add(new GameResult(itm));
        }

        public GameResultCollection(string QuizId, DirectoryInfo Dir)
            : this(new FileInfo(Path.Combine(Dir.FullName, QuizId + ".result")))
        {
        }

        public XElement ToXElement()
        {
            List<XElement> res = new List<XElement>();
            foreach (GameResult itm in results)
                res.Add(itm.ToXElement());

            return new XElement("GameResultCollection",
                new XAttribute("QuizName", quizName),
                new XAttribute("QuizId", quizId),
                res);
        }

        public void SaveToFile(DirectoryInfo Directory)
        {
            XDocument doc = null;
            string fi = Path.Combine(Directory.FullName, quizId + ".result");

            if (File.Exists(fi)
                && XDocument.Load(fi).Element("GameResultCollection") != null)
            {
                List<XElement> res = new List<XElement>();
                foreach (GameResult itm in results)
                    res.Add(itm.ToXElement());

                doc = XDocument.Load(fi);
                doc.Element("GameResultCollection").Add(res);

                if (doc.Element("GameResultCollection").Attribute("QuizName") == null)
                    doc.Element("GameResultCollection").Add(new XAttribute("QuizName", quizName));
                if (doc.Element("GameResultCollection").Attribute("QuizId") == null)
                    doc.Element("GameResultCollection").Add(new XAttribute("QuizId", quizId));
            }
            else
                doc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                        ToXElement());

            doc.Save(fi);
        }
    }
}
