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
using UserProfile;
using System.IO;
using LinqXMLTools;

namespace UserQuizzPreferences
{
    public class ProblemPreferences : Preferences
    {
        string questionId;
        QuizzSettings.QuestionSettings settings;
        DateTime lastChanged;

        public string QuestionId
        {
            get { return questionId; }
            set { questionId = value; }
        }

        public DateTime LastChanged
        {
            get { return lastChanged; }
            set { lastChanged = value; }
        }

        public QuizzSettings.QuestionSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        public ProblemPreferences(Profile User, string ProblemId, 
            QuizzSettings.QuestionSettings Settings)
            : base(User)
        {
            settings = Settings;
            lastChanged = DateTime.Now;
            this.questionId = ProblemId;

            string fPath = Path.Combine(
                User.DefaultPreferencesLocation, ProblemId + ".psp");

            if (File.Exists(fPath))
            {
                XDocument doc = XDocument.Load(fPath);
                if (doc.Element("QuestionPreferences") != null)
                {
                    XElement uPref = doc.Element("QuestionPreferences");
                    if (uPref != null)
                    {
                        XMLParser.ParseAttribute(uPref, "LastChanged", ref lastChanged);
                        if (uPref.Element("GameSettings") != null)
                        {
                            this.settings = new QuizzSettings.QuestionSettings(
                                uPref.Element("GameSettings"));
                        }
                    }
                }
            }
            else
                this.SaveToFile();
        }

        public XElement ToXElement()
        {
            return new XElement("QuestionPreferences",
                new XAttribute("QuestionId", questionId),
                new XAttribute("LastChanged", DateTime.Now),
                settings.ToXElement());
        }

        public void SaveToFile()
        {
            XDocument doc = new XDocument(new XDeclaration("1.0", "UTF-8", "yes"),
                ToXElement());
            doc.Save(Path.Combine(User.DefaultPreferencesLocation, 
                string.Format("{0}.psp", questionId)));
        }

    }
}
