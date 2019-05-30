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

namespace Quizz.StringFreeReponse
{
    public class StringFreeResponseProblem : IFreeResponseProblem<string, string[]>,
        IEquatable<StringFreeResponseProblem>, ICloneable
    {
        protected event AnswersLoadedEventHandler OnAnswersLoaded;
        protected delegate void AnswersLoadedEventHandler(object sender, XElement Answer);

        protected event OnXMLBindingEventHandler OnSaveBindings;
        protected delegate void OnXMLBindingEventHandler(object sender, List<XElement> Entries);

        protected string name = string.Empty;
        protected string question = string.Empty;
        protected string[] answer = null;
        protected string id = null;
        protected ProblemType problemKind = ProblemType.FreeResponseProblem;

        #region Public Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Id
        {
            get { return id; }
        }

        public string[] Answer
        {
            get { return answer; }
            set { answer = value; }
        }

        public string Question
        {
            get { return question; }
            set { question = value; }
        }

        public ProblemType ProblemKind
        {
            get { return problemKind; }
        }

        public static bool operator==(StringFreeResponseProblem First,
            StringFreeResponseProblem Second)
        {
            if(object.ReferenceEquals(First,Second))
                return true;
            else if (!(First is StringFreeResponseProblem)
                && !(Second is StringFreeResponseProblem))
                return false;

            return First.Equals(Second);
        }

        public static bool operator !=(StringFreeResponseProblem First,
            StringFreeResponseProblem Second)
        {
            return !(First == Second);
        }
        #endregion

        protected StringFreeResponseProblem(StringFreeResponseProblem Prob)
        {
            this.problemKind = ProblemKind;
            this.question = Prob.question;
            this.answer = Prob.Answer;
            this.id = Prob.Id;
            this.name = Prob.Name;
        }

        public StringFreeResponseProblem()
        {
            id = (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString()).Replace("-","");
            answer = new string[0];
        }

        public StringFreeResponseProblem(string Name, string Question, string[] Answer,
            string Id)
        {
            this.name = Name;
            this.question = Question;
            this.answer = Answer;
            this.id = Id;
        }

        public StringFreeResponseProblem(string Name, string Question, string[] Answer)
        {
            this.name = Name;
            this.question = Question;
            this.answer = Answer;
            id = (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString()).Replace("-", "");
        }

        public StringFreeResponseProblem(string Question, string[] Answer)
        {
            this.question = Question;
            this.answer = Answer;
            id = (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString()).Replace("-","");
        }

        public StringFreeResponseProblem(string Question, string[] Answer, ProblemType Kind)
        {
            this.question = Question;
            this.answer = Answer;
            this.problemKind = Kind;
            id = (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString()).Replace("-","");
        }

        public StringFreeResponseProblem(XElement elmProblem)
        {
            ParseXElement(elmProblem);
        }
        
        public virtual bool CheckAnswer(string Answer)
        {
            foreach (string itm in this.answer)
                if (itm == Answer)
                    return true;
            return false;
        }

        public virtual System.Xml.Linq.XElement ToXML()
        {
            XElement answers = new XElement("Answers");
            foreach (string itm in answer)
                answers.Add(new XElement("Answer", itm));

            List<XElement> bindings = new List<XElement>();

            if(OnSaveBindings != null)
                OnSaveBindings(this, bindings);

            return new XElement(problemKind.ToString(),
                new XAttribute("Name", name),
                new XAttribute("Question", question),
                new XAttribute("Id", id == null
                    ? (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString()).Replace("-","")
                    : id),
                answers,
                bindings);
        }

        public virtual void ParseXElement(XElement elmFreeResponseProblem)
        {
            try
            {
                List<string> answers = new List<string>();
                foreach (XElement itm in elmFreeResponseProblem.Elements("Answers").Elements("Answer"))
                {
                    answers.Add(itm.Value);
                    if(OnAnswersLoaded != null)
                        OnAnswersLoaded(this, itm);
                }
                answer = answers.ToArray();
                question = elmFreeResponseProblem.Attribute("Question").Value;

                if (elmFreeResponseProblem.Attribute("Name") != null
                    && elmFreeResponseProblem.Attribute("Name").Value != null)
                    name = elmFreeResponseProblem.Attribute("Name").Value;

                if (elmFreeResponseProblem.Attribute("Id") != null
                    && elmFreeResponseProblem.Attribute("Id").Value != null)
                    id = elmFreeResponseProblem.Attribute("Id").Value;
                else
                    id = (DateTime.Now.GetHashCode().ToString("x") + System.Guid.NewGuid().ToString()).Replace("-", "");
                

            }
            catch (System.Xml.XmlException e)
            {
                System.Diagnostics.Debug.WriteLine("Error occurred when trying to parse a free response question [obj: {0} @ line: {1}]\n{2}]", 
                    e.Source, e.LineNumber, e.StackTrace);
            }
        }

        public override string ToString()
        {
            return string.Format("{0}", name == string.Empty 
                ? question 
                : name);
        }

        public virtual object Clone()
        {
            string[] ans = null;
            if (answer != null)
            {
                ans = new string[this.answer.Length];
                Array.Copy(this.answer, ans, this.answer.Length);
            }

            return new StringFreeResponseProblem(string.Copy(question), ans);
        }

        public override bool Equals(object obj)
        {
            return (obj is StringFreeResponseProblem) ? 
                base.Equals(obj) : false;
        }

        public bool Equals(StringFreeResponseProblem other)
        {
            if (answer == null && other == null)
                return true;
            if (answer == null || other == null)
                return false;

            if (this.answer.Length != other.answer.Length)
                return false;

            for (int i = 0; i < this.answer.Length; ++i)
                if (answer[i] != other.answer[i])
                    return false;
            
            return (this.question == other.question
                && this.ProblemKind == other.problemKind);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
