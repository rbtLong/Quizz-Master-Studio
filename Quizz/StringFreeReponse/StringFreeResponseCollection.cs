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
using System.Collections;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Quizz.StringFreeReponse
{
    public class StringFreeResponseCollection : IEnumerable, ICloneable
    {
        List<StringFreeResponseProblem> problems;

        #region Public Properties
        public List<StringFreeResponseProblem> Problems
        {
          get { return problems; }
          set 
          {
              if (value == problems)
                  return;

              problems = value;
          }
        }
        public IEnumerator GetEnumerator()
        {
            return problems.ToArray().GetEnumerator();

        }
        public StringFreeResponseProblem this[int idx]
        {
            get { return problems[idx]; }
            set { problems[idx] = value; }
        }
        #endregion

        public StringFreeResponseCollection()
        {
            problems = new List<StringFreeResponseProblem>();
        }

        public StringFreeResponseCollection(IEnumerable<StringFreeResponseProblem> Problems)
        {
            problems = new List<StringFreeResponseProblem>(Problems);
        }

        public StringFreeResponseCollection(XElement Questions)
            : this()
        {

            problems = (from prob in Questions.Elements()
                        where prob.Name.ToString() == ProblemType.FreeResponseProblem.ToString()
                        || prob.Name.ToString() == ProblemType.MathFormulaProblem.ToString()
                        let answers = (from ans in prob.Element("Answers").Elements("Answer")
                                       select ans.Value).ToArray<string>()
                        select new StringFreeResponseProblem(
                            (prob as XElement).Attribute("Question").Value,
                            answers,
                            (ProblemType)Enum.Parse(typeof(ProblemType), prob.Name.ToString())) 
                            { 
                                Name = ((prob as XElement).Attribute("Name").Value == null
                                ? string.Empty
                                : ((prob as XElement).Attribute("Name").Value)),
                            }
                         ).ToList<StringFreeResponseProblem>();
        }

        /// <summary>
        /// Converts all problems into an array of XML formed
        /// FreeResponseQuestion element.
        /// </summary>
        /// <returns>Returns an array of XElements, null if none exists.</returns>
        public XElement[] ToXML()
        {
            List<XElement> xElm = new List<XElement>();
            foreach (StringFreeResponseProblem itm in problems)
            {
                switch (itm.ProblemKind)
                {
                    case ProblemType.MathFormulaProblem:
                        xElm.Add((itm as MathFormulaResponse.MathFormulaProblem).ToXML());
                        break;

                    case ProblemType.FreeResponseProblem:
                        xElm.Add(itm.ToXML());
                        break;
                }
            }

            return xElm.ToArray();
        }

        public void AddProblem(StringFreeResponseProblem itm)
        {
            problems.Add(itm);
        }

        public void AddProblems(StringFreeResponseProblem[] itm)
        {
            problems.AddRange(itm);
        }

        public void AddProblems(IEnumerable<StringFreeResponseProblem> itm)
        {
            problems.AddRange(itm);
        }

        /// <summary>
        /// Finds the position of the equatable problem in this set.  
        /// If none is found, -1 will be returned.
        /// </summary>
        /// <param name="itm">Problem to find in the set.</param>
        /// <returns>The position of the equivilent input is returned,
        /// otherwise -1 will return if no results are found.</returns>
        public int FindPositionByProblem(StringFreeResponseProblem itm)
        {
            for (int i = 0; i < problems.Count; ++i)
                if (problems[i] == itm)
                    return i;
            return -1;
        }

        public object Clone()
        {
            List<StringFreeResponseProblem> probs = 
                new List<StringFreeResponseProblem>();

            foreach (StringFreeResponseProblem itm in problems)
            {
                if (itm is MathFormulaResponse.MathFormulaProblem)
                {
                    string[] ans = null;
                    if (itm.Answer != null)
                    {
                        ans = new string[itm.Answer.Length];
                        Array.Copy(itm.Answer, ans, itm.Answer.Length);
                    }

                    MathFormulaResponse.MathFormulaProblem math = itm
                        as MathFormulaResponse.MathFormulaProblem;
                    probs.Add(new MathFormulaResponse.MathFormulaProblem(
                        string.Copy(math.Description),
                        string.Copy(math.Question), ans));
                }
                else
                    probs.Add(new StringFreeResponseProblem(
                        string.Copy(itm.Question),
                        itm.Answer == null
                            ? null 
                            : (from c in itm.Answer select string.Copy(c)).ToArray<string>(),
                        itm.ProblemKind));
            }

            return new StringFreeResponseCollection(probs);                
        }

        public void CopyFrom(IEnumerable<StringFreeResponseProblem> Item)
        {
            CopyFrom(Item, false);
        }

        public void CopyFrom(IEnumerable<StringFreeResponseProblem> Item, bool AllowDupelicates)
        {
            List<StringFreeResponseProblem> newCopy =
                new List<StringFreeResponseProblem>();

            foreach (StringFreeResponseProblem itmOther in Item)
                if (AllowDupelicates && IndexOf(itmOther) < 0)
                    newCopy.Add(itmOther.Clone() as StringFreeResponseProblem);
            problems = newCopy;
        }

        public int IndexOf(StringFreeResponseProblem itm)
        {
            for (int i = 0; i < problems.Count; ++i)
                if (itm.Equals(problems[i]))
                    return i;
            return -1;
        }
    }
}
