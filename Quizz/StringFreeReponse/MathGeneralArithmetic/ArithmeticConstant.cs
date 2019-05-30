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
    public class ArithmeticConstant : ArithmeticVariable
    {

        public ArithmeticConstant(double Value)
            : base(string.Empty, Value)
        {
            baseValue = minimumValue = maximumValue = Value;
            base.exponent = base.root = 1;
        }

        public ArithmeticConstant(double Value, double Exponent)
            : base(string.Empty, Value)
        {
            baseValue = minimumValue = maximumValue = Value;
            base.exponent = Exponent;
            base.root = 1;
        }

        public ArithmeticConstant(double Value, double Exponent, double Root)
            : base(string.Empty, Value)
        {
            baseValue = minimumValue = maximumValue = Value;
            base.exponent = Exponent;
            base.root = Root;
        }

        public ArithmeticConstant(string Identifier, double value)
            : base(Identifier, value)
        { 

        }

    }
}
