/*    
*    ExampleTestsBase.cs
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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Hybrid;
using Hybrid.Examples;
using Hybrid.Examples.Upcrc2010.MatrixMultiplication;
using Hybrid.Examples.CudaByExample;
using Hybrid.Examples.Upcrc2010;
using Hybrid.Examples.Functionality;

namespace Hybrid.Tests
{
    [TestClass]
    abstract public class ExampleTestBase
    {
        //Functionality Tests

        [TestMethod, Ignore]
        public void Lists()
        {
            testExample(new Lists());
        }

        [TestMethod]
        public void LocalFunctionCall()
        {
            testExample(new LocalFunctionCall());
        }

        [TestMethod]
        public void StaticFunctionCall()
        {
            testExample(new StaticFunctionCall());
        }

        [TestMethod]
        public void Switch()
        {
            testExample(new Switch());
        }

        //CUDA Book Examples
        
        [TestMethod]
        public void AverageTest()
        {
            testExample(new Average());
        }
        
        [TestMethod]
        public void DotProductTest()
        {
            testExample(new DotProduct());
        }

        [TestMethod]
        public void HeatTransferTest()
        {
            testExample(new HeatTransfer());
        }

        [TestMethod]
        public void HistogramTest()
        {
            testExample(new Histogram());
        }

        [TestMethod,Ignore]
        public void JuliaSetTest()
        {
            testExample(new JuliaSet());
        }

        [TestMethod,Ignore]
        public void RayTracingTest()
        {
            testExample(new RayTracing());
        }

        [TestMethod]
        public void RippleTest()
        {
            testExample(new Ripple());
        }

        [TestMethod]
        public void SummingVectorsTest()
        {
            testExample(new SummingVectors());
        }


        //UPCRC 2010

        [TestMethod]
        public void MatrixMultiplicationTest()
        {
            testExample(new MatrixMultiplication0());
            testExample(new MatrixMultiplication1());
            testExample(new MatrixMultiplication2());
            testExample(new MatrixMultiplication3());
            testExample(new MatrixMultiplication4());
            testExample(new MatrixMultiplication5());
        }

        [TestMethod]
        public void ConvolutionTest()
        {
            testExample(new Convolution());
        }

        [TestMethod]
        public void MatrixVectorMultiplicationTest()
        {
            testExample(new MatrixVectorMultiplication());
        }

        [TestMethod]
        public void MinimumSpanningTreeTest()
        {
            testExample(new MinimumSpanningTree());
        }

        [TestMethod]
        public void PrefixScanTest()
        {
            testExample(new PrefixScan());
        }

        [TestMethod]
        public void QuickSortTest()
        {
            testExample(new QuickSort());
        }

        // Further Examples

        [TestMethod]
        public void SudokuValidatorTest()
        {
            testExample(new SudokuValidator());
        }

        [TestMethod]
        public void SudokuValidator2DTest()
        {
            testExample(new SudokuValidator2D());
        }

        [TestMethod]
        public void SudokuValidatorInvalidColumnTest()
        {
            testExample(new SudokuValidatorInvalidColumn());
        }

        [TestMethod]
        public void SudokuValidatorInvalidNumbersTest()
        {
            testExample(new SudokuValidatorInvalidNumbers());
        }

        [TestMethod]
        public void SudokuValidatorInvalidRowTest()
        {
            testExample(new SudokuValidatorInvalidRow());
        }

        [TestMethod]
        public void SudokuValidatorInvalidSubfieldTest()
        {
            testExample(new SudokuValidatorInvalidSubfield());
        }

        [TestMethod, Ignore]
        public void Crypt3Test()
        {
            testExample(new Crypt3());
        }
        
        [TestMethod, Ignore]
        public void GameOfLifeTest()
        {
            testExample(new GameOfLife());
        }

        [TestMethod]
        public void MergeTest()
        {
            testExample(new Merge());
        }

        [TestMethod, Ignore]
        public void ParGrepTest()
        {
            testExample(new ParGrep());
        }

        [TestMethod, Ignore]
        public void WatorTest()
        {
            testExample(new Wator());
        }

        [TestMethod]
        public void MathematicalFunctionsTest()
        {
            testExample(new MathematicalFunctions());
        }

        [TestMethod]
        public void ForLoopTest()
        {
            testExample(new ForLoop());
        }

        [TestMethod]
        public void ConvolutionNPPTest()
        {
            testExample(new ConvolutionNPP());
        }
        protected abstract void testExample(ExampleBase example);

        
    }
}
