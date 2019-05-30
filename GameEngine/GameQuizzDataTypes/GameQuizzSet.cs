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
using System.IO;
using System.Xml;

namespace GameEngine.GameQuizzDataTypes
{
    public class GameQuizzSet : QuizzSet
    {
        QuizzSettings.QuizzSettings settings;
        List<GameProblem> problems;

        #region Public Properties
        new public List<GameProblem> Problems
        {
            get { return problems; }
            set { problems = value; }
        }

        public QuizzSettings.QuizzSettings Settings
        {
            get { return settings; }
            set { settings = value; }
        }

        public GameQuizzDataTypes.GameProblem this[int pos]
        {
            get 
            {
                return problems[pos];
            }
        }
        #endregion

        public GameQuizzSet(string Name)
            : base(Name)
        {
            loadComponents();
        }

        public GameQuizzSet(FileInfo File)
            : base(File, false)
        {
            loadComponents();
            base.LoadXDoc(XDocument.Load(File.FullName));
        }

        public GameQuizzSet(XDocument xDoc)
            : base(xDoc, false)
        {
            loadComponents();
            base.LoadXDoc(xDoc);
        }

        private void loadComponents()
        {
            settings = new QuizzSettings.QuizzSettings();
            problems = new List<GameProblem>();
            OnQuestionLoaded += new QuestionLoadedEventHandler(GameQuizzSet_OnQuestionLoaded);
            OnQuizzLoaded += new QuizzSetLoadedEventHandler(GameQuizzSet_OnQuizzLoaded);
        }

        void GameQuizzSet_ToXmlBinding(object sender, List<XElement> Bindings)
        {

        }

        void GameQuizzSet_OnQuizzLoaded(object sender, QuizzSet QSet, XElement Quizz)
        {
            this.settings = new QuizzSettings.QuizzSettings(Quizz);
        }

        void GameQuizzSet_OnQuestionLoaded(object sender, QuizzSet qSet, XElement xProblem, StringFreeResponseProblem Problem)
        {
            problems.Add(new GameProblem(Problem, xProblem));
        }

        public XElement ToElement()
        {
            List<XElement> bindings = new List<XElement>();
            foreach (GameProblem itm in problems)
                bindings.Add(itm.ToXML());

            bindings.Add(settings.ToXElement());

            return new XElement("QuizzSet",
                new XAttribute("Name", base.Name),
                new XAttribute("Id", base.Id 
                    ?? (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString()).Replace("-", "")),
                bindings);
        }

        public override void SaveFile()
        {
            XDocument xDoc = new XDocument(new XElement("StringQuizzMaster",
                ToElement()));
            xDoc.Save(base.FileId.FullName);
        }

        public override void SaveAsFile(string FilePath)
        {
            XDocument xDoc = new XDocument(new XElement("StringQuizzMaster",
                ToElement()));
            xDoc.Save(FilePath);
        }

        new public static IEnumerable<GameQuizzSet> GetQuizzSet(string Path)
        {
            string[] paths = GetQuizzFilePaths(Path);
            List<GameQuizzSet> sets = new List<GameQuizzSet>(paths.Length);
            foreach (string itm in paths)
            {
                try
                {
                    sets.Add(new GameQuizzSet(new FileInfo(itm)));
                }
                catch (IOException)
                {
                    throw new Exception("An error occurred while trying to access the file: \"" + itm + "\". (Perhaps it is being used by another application?)");
                }
                catch (XmlException exception2)
                {
                    throw new Exception(exception2.Message);
                }
            }

            return sets;
        }

        new public static IEnumerable<GameQuizzSet> GetQuizzSet()
        {
            string[] paths = GetQuizzFilePaths(QuizzSet.DefaultLocalPath);
            List<GameQuizzSet> sets = new List<GameQuizzSet>(paths.Length);
            foreach (string itm in paths)
            {
                try
                {
                    sets.Add(new GameQuizzSet(new FileInfo(itm)));
                }
                catch (IOException)
                {
                    throw new Exception("An error occurred while trying to access the file: \"" + itm + "\". (Perhaps it is being used by another application?)");
                }
                catch (XmlException exception2)
                {
                    throw new Exception(exception2.Message);
                }
            }

            return sets;
        }
    }
}
