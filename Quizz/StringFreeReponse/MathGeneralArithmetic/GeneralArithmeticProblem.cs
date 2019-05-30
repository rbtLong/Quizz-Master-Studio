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

namespace Quizz.StringFreeReponse.MathGeneralArithmetic
{
    public sealed class GeneralArithmeticProblem : StringMathFreeResponse
    {
        ArithmeticExpression expression = null;

        public ArithmeticExpression Expression
        {
            get { return expression; }
            set { expression = value; }
        }

        public GeneralArithmeticProblem()
        {
            this.problemKind = ProblemType.MathGeneralArithmetic;
        }

        public GeneralArithmeticProblem(string Description, ArithmeticExpression MathExpression)
        {
            this.problemKind = ProblemType.MathGeneralArithmetic;
            this.description = Description;
            expression = MathExpression;

            if (expression != null)
            {
                this.question = MathExpression.ToString();
                this.answer = new string[] { MathExpression.Calculate().ToString() };
            }
        }

        public GeneralArithmeticProblem(XElement XmlElement)
            : base(XmlElement)
        {
            this.problemKind = ProblemType.MathGeneralArithmetic;
        }

        public override void ParseXElement(XElement elmMathFreeResponse)
        {
            base.ParseXElement(elmMathFreeResponse);
        }

        public override string ToString()
        {
            return String.Format("General Arithmetic: {0}", description == string.Empty ?
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

            return new GeneralArithmeticProblem(
                description == null ? null : string.Copy(description),
                expression == null ? null : expression.Clone() as ArithmeticExpression);
        }

        public override bool Equals(object obj)
        {
            return (obj is GeneralArithmeticProblem) ?
                base.Equals(obj) : false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public void CopyTo(GeneralArithmeticProblem Other)
        {
            expression.CopyTo(Other.expression);
            Other.Description = String.Copy(description);
            Array.Copy(answer, Other.Answer, answer.Length);
            Other.Question = String.Copy(Other.Question);
        }

        public void AppendExpression(ArithmeticExpression Expression)
        {
            if (expression == null)
            {
                expression = Expression;
            }
            else
            {
                Expression.NestedExpression = expression;
                expression = Expression;
            }
        }

    }
}
