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
    public class ArithmeticVariable : ICloneable
    {
        protected string identifier = string.Empty;
        protected double minimumValue,
                         maximumValue;
        protected double? baseValue = null;
        protected double exponent;
        protected double root;

        #region Public Properties
        public double Value
        {
            get
            {
                if (!baseValue.HasValue)
                    ReassignValue();

                return (Math.Pow(baseValue.Value, exponent/root)); 
            }
        }

        public double Root
        {
            get { return root; }
            set { root = value; }
        }

        public double Exponent
        {
            get { return exponent; }
            set { exponent = value; }
        }

        public double? BaseValue
        {
            get { return baseValue; }
            set { baseValue = value; }
        }

        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        public double MaximumValue
        {
            get { return maximumValue; }
            set { maximumValue = value; }
        }

        public double MinimumValue
        {
            get { return minimumValue; }
            set { minimumValue = value; }
        }
        #endregion
        #region Math Operators
        public static double operator +(ArithmeticVariable first, ArithmeticVariable second)
        {
            return first.Value + second.Value;
        }

        public static double operator -(ArithmeticVariable first, ArithmeticVariable second)
        {
            return first.Value - second.Value;
        }

        public static double operator /(ArithmeticVariable first, ArithmeticVariable second)
        {
            return first.Value / second.Value;
        }

        public static double operator *(ArithmeticVariable first, ArithmeticVariable second)
        {
            return first.Value * second.Value;
        }
        #endregion

        public ArithmeticVariable(string Identifier, double MinimumValue, double MaximumValue)
        {
            identifier = string.Format("{{{0}: {1}}}", Identifier, MinimumValue);
            if (MinimumValue != MaximumValue)
            {
                minimumValue = MinimumValue;
                maximumValue = MaximumValue;
                identifier = string.Format("{{{0}: {1}-{2}}}", Identifier, MinimumValue, MaximumValue);
            }
            else
                baseValue = minimumValue;

            exponent = 1;
            root = 1;
        }

        public ArithmeticVariable(string Identifier, double BaseValue)
        {
            identifier = Identifier;
            baseValue = BaseValue;
            minimumValue = maximumValue = BaseValue;

            exponent = 1;
            root = 1;
        }

        public ArithmeticVariable(string Identifier, double BaseValue, 
            double Exponent, double Root)
        {
            identifier = Identifier;
            baseValue = BaseValue;
            minimumValue = maximumValue = BaseValue;

            exponent = 1;
            root = 1;
        }

        public ArithmeticVariable(string Identifier, double? BaseValue,
            double Minimum, double Maximum, double Exponent, double Root)
        {
            identifier = Identifier;
            baseValue = BaseValue;
            minimumValue = Minimum;
            maximumValue = Maximum;

            exponent = Exponent;
            root = Root;
        }

        public void ReassignValue(int seed)
        {
            baseValue = (minimumValue == maximumValue) ? minimumValue 
                : new Random(seed).Next((int)minimumValue, (int)maximumValue);
        }

        public void ReassignValue()
        {
            baseValue = (minimumValue == maximumValue) ? minimumValue
                : new Random().Next((int)minimumValue, (int)maximumValue);
        }

        public override string ToString()
        {
            if (!baseValue.HasValue)
                return ToString("variable");

            string value = "";
            if (identifier == string.Empty || identifier == null)
            {
                if (!baseValue.HasValue)
                    ReassignValue();
                value = baseValue.ToString();
            }
            else
                value = identifier;


            return string.Format("{0}{1}", value, (exponent / root) != 1 
                ? ((root == 1) ? string.Format("^({0}/{1})", exponent, root) : string.Format("^({0})",exponent))
                : "");
        }

        public string ToString(string cmd)
        {

            if (cmd.ToLower() == "variable")
            {
                string value = "";
                if(identifier == string.Empty || identifier == null)
                {
                    if(!baseValue.HasValue)
                        ReassignValue();
                    value = baseValue.ToString();
                }
                else
                    value = identifier;

                return string.Format("{0}{1}", value, (exponent / root) != 1 
                    ? ((root == 1) ? string.Format("^({0}/{1})", exponent, root) : string.Format("^({0})", exponent))
                    : "");
            }
            else if (cmd.ToLower() == "d")
            {
                if (!baseValue.HasValue)
                    ReassignValue();
                return baseValue.ToString();
            }

            return ToString();
        }

        public object Clone()
        {
            return new ArithmeticVariable(String.Copy(identifier),
                minimumValue, maximumValue);
        }

        public void CopyTo(ArithmeticVariable Other)
        {
            Other.MaximumValue = maximumValue;
            Other.MinimumValue = minimumValue;
            Other.Identifier = String.Copy(identifier);
        }
    }
}
