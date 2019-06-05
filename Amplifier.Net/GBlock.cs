/*
Amplifier.NET - LGPL 2.1 License
Please consider purchasing a commerical license - it helps development, frees you from LGPL restrictions
and provides you with support.  Thank you!
Copyright (C) 2011 Hybrid DSP Systems
http://www.hybriddsp.com

This library is free software; you can redistribute it and/or
modify it under the terms of the GNU Lesser General Public
License as published by the Free Software Foundation; either
version 2.1 of the License, or (at your option) any later version.

This library is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
Lesser General Public License for more details.

You should have received a copy of the GNU Lesser General Public
License along with this library; if not, write to the Free Software
Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301  USA
*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using info.jhpc.thread;
using info.jhpc.warp;
namespace Amplifier
{
    /// <summary>
    /// Represents an Cuda block.
    /// </summary>
    public class GBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GBlock"/> class.
        /// </summary>
        /// <param name="grid">The parent grid.</param>
        /// <param name="size">The size.</param>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public GBlock(GGrid grid, dim3 size, int x, int y)
        {
            Idx = new dim3(x, y);
            Dim = size;
            Barrier = new SimpleBarrier(size.x * size.y * size.z);
            WarpBarrier = new SimpleWarpBarrier((int)Math.Ceiling((size.x * size.y * size.z) / 32.0f), 32); 
            Grid = grid;
            _shared = new Dictionary<string, object>();
            _lock = new object();
        }

        /// <summary>
        /// Gets the id of this block.
        /// </summary>
        internal dim3 Idx { get; private set; }
        /// <summary>
        /// Gets the dimensions of this block.
        /// </summary>
        internal dim3 Dim { get; private set; }

        /// <summary>
        /// Gets the parent grid.
        /// </summary>
        internal GGrid Grid { get; private set; }

        /// <summary>
        /// Gets or sets the barrier used to synchronize threads in a block.
        /// </summary>
        /// <value>
        /// The barrier.
        /// </value>
        private SimpleBarrier Barrier { get; set; }
        private SimpleWarpBarrier WarpBarrier { get; set; }
        private Dictionary<string, object> _shared;

        private object _lock;

        /// <summary>
        /// Allocates a 1D array in shared memory.
        /// </summary>
        /// <typeparam name="T">Blittable type.</typeparam>
        /// <param name="varName">Key of the variable.</param>
        /// <param name="x">The x size.</param>
        /// <returns>Pointer to the shared memory.</returns>
        internal T[] AllocateShared<T>(string varName, int x)
        {
            lock (_lock)
            {
                if (!_shared.ContainsKey(varName))
                {
                    T[] devMem = new T[x];
                    _shared.Add(varName, devMem);
                }
                return (T[])_shared[varName];
            }
        }

        /// <summary>
        /// Allocates a 2D array in shared memory.
        /// </summary>
        /// <typeparam name="T">Blittable type.</typeparam>
        /// <param name="varName">Key of the variable.</param>
        /// <param name="x">The x size.</param>
        /// <param name="y">The y size.</param>
        /// <returns>Pointer to the shared memory.</returns>
        internal T[,] AllocateShared<T>(string varName, int x, int y)
        {         
            lock (_lock)
            {
                if (!_shared.ContainsKey(varName))
                {
                    T[,] devMem = new T[x,y];
                    _shared.Add(varName, devMem);
                }
                return (T[,])_shared[varName];
            }
        }

        /// <summary>
        /// Allocates a 2D array in shared memory.
        /// </summary>
        /// <typeparam name="T">Blittable type.</typeparam>
        /// <param name="varName">Key of the variable.</param>
        /// <param name="x">The x size.</param>
        /// <param name="y">The y size.</param>
        /// <param name="z">The z size.</param>
        /// <returns>Pointer to the shared memory.</returns>
        internal T[,,] AllocateShared<T>(string varName, int x, int y, int z)
        {
            lock (_lock)
            {
                if (!_shared.ContainsKey(varName))
                {
                    T[,,] devMem = new T[x, y, z];
                    _shared.Add(varName, devMem);
                }
                return (T[,,])_shared[varName];
            }
        }

        /// <summary>
        /// Syncs the threads in this block.
        /// </summary>
        internal void SyncThreads()
        {
            Barrier.SignalAndWait();
        }

        /// <summary>
        /// Syncs the threads in this block, returns number of threads that have true predicate in block
        /// </summary>
        internal int SyncThreadsCount(bool predicate)
        {
            return Barrier.SignalAndWaitAndCountPredicate(predicate);
        }

        /// <summary>
        /// Syncs the threads in the warp, returns true is any have true prediate
        /// </summary>
        internal bool Any(bool predicate, int warpId)
        {
            return WarpBarrier.SignalAnyPredicateAndWait(predicate, warpId);
        }

        /// <summary>
        /// Syncs the threads in the warp, returns true iff all threads in warp are have true predicate;
        /// </summary>
        internal bool All(bool predicate, int warpId)
        {
            return WarpBarrier.SignalAllPredicateAndWait(predicate, warpId);
        }

        /// <summary>
        /// Syncs the threads in the warp, returns number of threads with true predicate, in warp
        /// </summary>
        internal int Ballot(bool predicate, int warpId)
        {
            return WarpBarrier.SignalBallotPredicateAndWait(predicate, warpId);
        }
    }
}
