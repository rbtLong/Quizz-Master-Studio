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

namespace Quizz.StringFreeReponse.MathGeneralArithmetic
{
    public class ArithmeticExpression : ICloneable
    {
        ArithmeticVariable arg1;
        ArithmeticOperator operation;
        ArithmeticExpression nestedExpression = null;

        #region Public Properties
        public string CurrentOperator
        {
            get { return opToString(operation); }
        }
        public ArithmeticExpression NestedExpression
        {
            get { return nestedExpression; }
            set { nestedExpression = value; }
        }

        public ArithmeticOperator Operation
        {
            get { return operation; }
            set { operation = value; }
        }

        public ArithmeticVariable Arg1
        {
            get { return arg1; }
            set { arg1 = value; }
        }
        #endregion
        #region Math Operators
        public static double operator +(ArithmeticVariable first, ArithmeticExpression second)
        {
            return first.Value + second.Calculate();
        }

        public static double operator +(ArithmeticExpression second, ArithmeticVariable first)
        {
            return first.Value + second.Calculate();
        }

        public static double operator -(ArithmeticVariable first, ArithmeticExpression second)
        {
            return first.Value - second.Calculate();
        }

        public static double operator -(ArithmeticExpression first, ArithmeticVariable second)
        {
            return first.Calculate() - second.Value;
        }

        public static double operator /(ArithmeticVariable first, ArithmeticExpression second)
        {
            return first.Value / second.Calculate();
        }

        public static double operator /(ArithmeticExpression first, ArithmeticVariable second)
        {
            return first.Calculate() / second.Value;
        }

        public static double operator *(ArithmeticVariable first, ArithmeticExpression second)
        {
            return first.Value * second.Calculate();
        }

        public static double operator +(ArithmeticExpression first, ArithmeticExpression second)
        {
            return first.Calculate() + second.Calculate();
        }

        public static double operator -(ArithmeticExpression first, ArithmeticExpression second)
        {
            return first.Calculate() - second.Calculate();
        }

        public static double operator /(ArithmeticExpression first, ArithmeticExpression second)
        {
            return first.Calculate() / second.Calculate();
        }

        public static double operator *(ArithmeticExpression first, ArithmeticExpression second)
        {
            return first.Calculate() * second.Calculate();
        }
        #endregion


        public ArithmeticExpression(ArithmeticVariable Arg1, 
            ArithmeticExpression NestedExpression, ArithmeticOperator Operator)
        {
            arg1 = Arg1;
            operation = Operator;
            nestedExpression = NestedExpression;
        }

        public ArithmeticExpression(ArithmeticVariable Arg1)
            : this(Arg1, null, ArithmeticOperator.Blank)
        {
        }

        public ArithmeticExpression(ArithmeticVariable Arg1, ArithmeticOperator Operator)
            : this(Arg1, null, Operator)
        {
        }

        public double Calculate()
        {
            double value = 0;

            if (nestedExpression == null)
                return arg1.Value;
            else
                switch (Operation)
                { 
                    case ArithmeticOperator.Add:
                        value = arg1 + NestedExpression;
                        break;
                    case ArithmeticOperator.Divide:
                        value = arg1 / NestedExpression;
                        break;
                    case ArithmeticOperator.Multiply:
                        value = arg1 * NestedExpression;
                        break;
                    case ArithmeticOperator.Subtract:
                        value = arg1 - NestedExpression;
                        break;
                }

            return value;
        }

        public double CalculateReassign()
        {
            double value = 0;
            arg1.ReassignValue();

            if (nestedExpression == null)
                return arg1.Value;
            else
                switch (Operation)
                {
                    case ArithmeticOperator.Add:
                        value = arg1 + NestedExpression;
                        break;
                    case ArithmeticOperator.Divide:
                        value = arg1 / NestedExpression;
                        break;
                    case ArithmeticOperator.Multiply:
                        value = arg1 * NestedExpression;
                        break;
                    case ArithmeticOperator.Subtract:
                        value = arg1 - NestedExpression;
                        break;
                }

            return value;
        }

        public ArithmeticVariable CalculateArithmeticVariable()
        {
            double value = Calculate();
            return new ArithmeticVariable("", value);
        }
        
        public override string ToString()
        {
            if (operation == ArithmeticOperator.Blank)
                return arg1.ToString();
            else
                return string.Format("{0} {1} ", arg1.ToString(),
                    opToString(operation), nestedExpression.ToString("variable"));
        }

        public string AllEquationsToString()
        {
            if (operation == ArithmeticOperator.Blank && nestedExpression == null)
                return arg1.ToString();
            else
                return string.Format("({0} {1} {2})", arg1.ToString(),
                    opToString(operation), nestedExpression.AllEquationsToString());
        }

        public string AllEquationsToString(string cmd)
        {
            if (cmd.ToLower() == "d")
            {
                if (operation == ArithmeticOperator.Blank && nestedExpression == null)
                    return arg1.ToString();
                else
                    return string.Format("({0} {1} {2})", arg1.ToString("d"),
                        opToString(operation), nestedExpression.AllEquationsToString("d"));
            }

            return AllEquationsToString();
        }

        public string ToString(string cmd)
        {
            if (cmd.ToLower() == "variable")
            {
                if (operation == ArithmeticOperator.Blank)
                    return arg1.ToString("variable");

                return string.Format("{0} {1}",
                    arg1.ToString("variable"),
                    opToString(operation));

            }
            return ToString();
        }

        private string opToString(ArithmeticOperator operation)
        {
            if (this.operation == ArithmeticOperator.Blank)
                return arg1.ToString("variable");

            switch (operation)
            {
                case ArithmeticOperator.Add:
                    return "+";
                case ArithmeticOperator.Divide:
                    return "/";
                case ArithmeticOperator.Multiply:
                    return "*";
                case ArithmeticOperator.Subtract:
                    return "-";
            }

            return "";
        }

        public object Clone()
        {
            return new ArithmeticExpression(
                arg1 == null? null : arg1.Clone() as ArithmeticVariable, 
                nestedExpression==null? null : nestedExpression.Clone() as ArithmeticExpression, 
                operation);
        }

        public void CopyTo(ArithmeticExpression Other)
        {
            if(arg1 != null)
                arg1.CopyTo(Other.arg1);
            if(nestedExpression != null)
                nestedExpression.CopyTo(Other.nestedExpression);
            Other.Operation = this.operation;
        }
    }
}
