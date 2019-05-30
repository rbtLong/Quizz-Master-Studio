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
using System.Xml;

namespace Quizz.StringFreeReponse.MathFormulaResponse
{
    public sealed class MathFormulaProblem : StringMathFreeResponse
    {

        public MathFormulaProblem()
        {
            this.problemKind = ProblemType.MathFormulaProblem;
        }

        public MathFormulaProblem(string Description, string Question, string[] Answers)
            : base(Description, Question, Answers)
        {
            this.problemKind = ProblemType.MathFormulaProblem;
        }

        public MathFormulaProblem(XElement XmlElement)
            : base(XmlElement)
        {
            this.problemKind = ProblemType.MathFormulaProblem;
        }

        public void RemoveAnswer(int idx)
        {
            if (idx > -1 && idx < answer.Length)
            {
                string[] answers = new string[answer.Length == 1 ? 1
                    : answer.Length-1];
                for (int i = 0, j=0; i < answer.Length; ++i)
                { 
                    if(idx != i)
                    {
                        answers[j] = answer[i];
                        j++;
                    }
                }
                answer = answers;
            }
            else
                throw new Exception("Out of Range Error when trying to remove an answer.");
        }

        public override bool CheckAnswer(string Answer)
        {
            foreach (string itm in answer)
                if (itm.ToLower().Replace(" ","") == Answer.ToLower().Replace(" ",""))
                    return true;
            return false;
        }

        public override string ToString()
        {
            return String.Format("Formula: {0}", description == string.Empty ?
                "<NO DESCRIPT>" : description);
        }

        public override object Clone()
        {
            string[] ans = null;
            if (answer != null)
            {
                ans = new string[this.answer.Length];
                Array.Copy(this.answer, ans, this.answer.Length);
            }

            return new MathFormulaProblem(string.Copy(description), 
                string.Copy(this.question), ans);
        }

        public override bool Equals(object obj)
        {
            return (obj is MathFormulaProblem) ?
                base.Equals(obj) : false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void CopyTo(MathFormulaProblem prob)
        {
            prob.answer = this.answer;
            prob.description = this.description;
            prob.question = this.question;
            prob.problemKind = this.problemKind;
            prob.id = this.id;
        }

    }
}
