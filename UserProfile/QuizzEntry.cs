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

namespace UserProfile
{
    public class QuizzEntry
    {
        string name;
        double percent;
        DateTime started,
                 ended;

        #region Public Properties
        public double Percent
        {
            get { return percent; }
            set { percent = value; }
        }

        public DateTime Ended
        {
            get { return ended; }
            set { ended = value; }
        }

        public DateTime Started
        {
            get { return started; }
            set { started = value; }
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        #endregion

        public QuizzEntry(string Name, double PercentScore, DateTime Started, DateTime Ended)
        {
            this.name = Name;
            this.percent = PercentScore;
            this.started = Started;
            this.ended = Ended;
        }

        public QuizzEntry(XElement Entry)
        {
            name = Entry.Element("Name").Value;
            percent = double.Parse(Entry.Element("Percent").Value);
            started = DateTime.Parse(Entry.Element("Started").Value);
            ended = DateTime.Parse(Entry.Element("Ended").Value);
        }

        public static QuizzEntry[] LoadQuizzes(XElement Entries)
        {
            List<QuizzEntry> quizzes = new List<QuizzEntry>();
            XElement[] xelmQuizzes = Entries.Elements("QuizzEntry").ToArray<XElement>();
            foreach(XElement itm in xelmQuizzes)
                quizzes.Add(new QuizzEntry(itm));
            return quizzes.ToArray();
        }

        public XElement ToXML()
        {
            return new XElement("QuizzEntry",
                new XAttribute("Name", name),
                new XAttribute("Percent", percent),
                new XAttribute("Started", started),
                new XAttribute("Ended", ended));
        }
    }
}
