/*    
*    SudokuValidatorInvalidColumn.cs
*
﻿*    Copyright (C) 2012  Frank Feinbube, Jan-Arne Sobania, Ralf Diestelkämper
*
*    This library is free software: you can redistribute it and/or modify
*    it under the terms of the GNU Lesser General Public License as published by
*    the Free Software Foundation, either version 3 of the License, or
*    (at your option) any later version.
*
*    This library is distributed in the hope that it will be useful,
*    but WITHOUT ANY WARRANTY; without even the implied warranty of
*    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*    GNU Lesser General Public License for more details.
*
*    You should have received a copy of the GNU Lesser General Public License
*    along with this program.  If not, see <http://www.gnu.org/licenses/>.
*
*    Frank [at] Feinbube [dot] de
*    jan-arne [dot] sobania [at] gmx [dot] net
*    ralf [dot] diestelkaemper [at] hotmail [dot] com
*
*/


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hybrid.Examples
{
    public class SudokuValidatorInvalidColumn : SudokuValidator
    {
        protected override void setup()
        {
            n = this.sizeX;

            field = new int[n * n * n * n];
            generateFieldWithInvalidColumn();
        }

        protected override bool  fieldIsValid(int[] invalidFieldIndicator)
        {
            return //invalidFieldIndicator[0] == 0 &&
               //invalidFieldIndicator[1] == 0 &&
               invalidFieldIndicator[2] == 1;// &&
               //invalidFieldIndicator[3] == 0;
        }
    }
}
