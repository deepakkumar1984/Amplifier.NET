/*    
*    InvokeContext.cs
*
﻿*    Copyright (C) 2012 Jan-Arne Sobania, Frank Feinbube, Ralf Diestelkämper
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
*    jan-arne [dot] sobania [at] gmx [dot] net
*    Frank [at] Feinbube [dot] de
*    ralf [dot] diestelkaemper [at] hotmail [dot] com
*
*/


﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Hybrid.MsilToOpenCL
{
    internal class InvokeContext : IDisposable
    {
        private List<InvokeArgument> m_Arguments;
        public List<InvokeArgument> Arguments
        {
            get
            {
                return m_Arguments;
            }
        }

        private Dictionary<Type, string> m_ValueTypeMap;
        public Dictionary<Type, string> ValueTypeMap { get { return m_ValueTypeMap; } }

        public InvokeContext(HighLevel.HlGraph HLgraph)
            : this(HLgraph, GetRandomSeed())
        {
        }

        private static int GetRandomSeed()
        {
            return (new Random()).Next();
        }

        public InvokeContext(HighLevel.HlGraph HLgraph, int RandomSeed)
        {
            m_ValueTypeMap = HLgraph.ValueTypeMap;
            m_Arguments = new List<InvokeArgument>(HLgraph.Arguments.Count);
            for (int i = 0; i < HLgraph.Arguments.Count; i++)
            {
                System.Diagnostics.Debug.Assert(HLgraph.Arguments[i].Index == i);
                m_Arguments.Add(null);
            }

            if (!object.ReferenceEquals(HLgraph.RandomSeedArgument, null))
            {
                PutArgument(HLgraph.RandomSeedArgument, RandomSeed);
            }
        }

        public void PutArgument(HighLevel.ArgumentLocation Location, object Value)
        {
            int Index = Location.Index;

            System.Diagnostics.Debug.Assert(Index >= 0 && Index < m_Arguments.Count);
            System.Diagnostics.Debug.Assert(m_Arguments[Index] == null);

            InvokeArgument Argument = null;
            if (object.ReferenceEquals(Value, null))
            {
                Argument = new InvokeArgument.Int32Arg(0);
            }
            else if (Value is int)
            {
                Argument = new InvokeArgument.Int32Arg((int)Value);
            }
            else if (Value is uint)
            {
                Argument = new InvokeArgument.UInt32Arg((uint)Value);
            }
            else if (Value is long)
            {
                Argument = new InvokeArgument.Int64Arg((long)Value);
            }
            else if (Value is ulong)
            {
                Argument = new InvokeArgument.UInt64Arg((ulong)Value);
            }
            else if (Value is float)
            {
                Argument = new InvokeArgument.FloatArg((float)Value);
            }
            else if (Value is double)
            {
                Argument = new InvokeArgument.DoubleArg((double)Value);
            }
            else
            {
                Type Type = Value.GetType();
                if (Type.IsArray)
                {
                    bool ForRead = false, ForWrite = false;
                    if ((Location.Flags & HighLevel.LocationFlags.IndirectRead) != 0) { ForRead = true; }
                    if ((Location.Flags & HighLevel.LocationFlags.IndirectWrite) != 0) { ForWrite = true; }

                    if (!ForRead && !ForWrite)
                    {
                        ForRead = ForWrite = true;
                    }

                    Type ElementType = Type.GetElementType();
                    if (ElementType == typeof(byte) || ElementType == typeof(int) || ElementType == typeof(uint) || ElementType == typeof(long) || ElementType == typeof(ulong)
                        || ElementType == typeof(float) || ElementType == typeof(double))
                    {
                        Argument = new InvokeArgument.PrimitiveArrayArg((System.Array)Value, ForRead, ForWrite);
                    }
                    else if (ValueTypeMap.ContainsKey(ElementType))
                    {
                        Argument = new InvokeArgument.MarshalledArrayArg((System.Array)Value,ForRead,ForWrite);
                    }
                }
            }

            if (Argument == null)
            {
                throw new InvalidOperationException(string.Format("Sorry, argument type '{0}' cannot be marshalled for OpenCL.", Value.GetType()));
            }

            m_Arguments[Index] = Argument;
        }

        public void Complete()
        {
            int Index = m_Arguments.IndexOf(null);
            System.Diagnostics.Debug.Assert(Index < 0);
            if (Index >= 0)
            {
                throw new InvalidOperationException(string.Format("Argument {0} is not assigned.", Index));
            }
        }

        #region IDisposable Members

        public void Dispose()
        {
            //foreach (InvokeArgument Argument in m_Arguments)
            //{
            //    if (Argument != null)
            //    {
            //        Argument.Dispose();
            //    }
            //}
            m_Arguments.Clear();
            //System.GC.SuppressFinalize(this);
        }

        #endregion
    }
}
