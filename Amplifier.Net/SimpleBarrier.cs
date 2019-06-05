/*

Copyright (c) 2000, Thomas W. Christopher and George K. Thiruvathukal



Java and High Performance Computing (JHPC) Organzization

Tools of Computing LLC



All rights reserved.



Redistribution and use in source and binary forms, with or without

modification, are permitted provided that the following conditions are

met:



Redistributions of source code must retain the above copyright notice,

this list of conditions and the following disclaimer.



Redistributions in binary form must reproduce the above copyright

notice, this list of conditions and the following disclaimer in the

documentation and/or other materials provided with the distribution.



The names Java and High-Performance Computing (JHPC) Organization,

Tools of Computing LLC, and/or the names of its contributors may not

be used to endorse or promote products derived from this software

without specific prior written permission.



THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS

"AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT

LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR

A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL THE REGENTS OR

CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,

EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO,

PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR

PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF

LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING

NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS

SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.



This license is based on version 2 of the BSD license. For more

information on Open Source licenses, please visit

http://opensource.org.

*/



using System;

using System.Threading;





namespace info.jhpc.thread

{

	/**

	 * Allows multiple threads to gather at a point before proceeding.

	 *

	 * @author Thomas W. Christopher (Tools of Computing LLC)

	 * @version 0.2 Beta

	 */

	

	public class SimpleBarrier {

		/**

		 * Number of threads that still must gather.

		 */

		protected int count;
        /**

		 * Number of threads that still must gather.

		 */

        protected int predicate_sum;
	    protected int predicate_sum_final;

		/**

		 * Total number of threads that must gather.

		 */

		protected int initCount;

	

		/**

		 * Creates a Barrier at which n threads may repeatedly gather.

		 *

		 * @param n total number of threads that must gather.

		 */

	

		public SimpleBarrier(int n) {

			if (n <= 0)

				throw new ArgumentException("Barrier initialization specified non-positive value " + n);



			initCount = count = n;
            predicate_sum = 0;
		}

	

		/**

		 * Is called by a thread to wait for the rest of the n threads to gather

		 * before the set of threads may continue executing.

		 *

		 * @throws InterruptedException If interrupted while waiting.

		 */

		public virtual int gather(bool predicate) {

			System.Threading.Monitor.Enter(this);
            predicate_sum += (predicate) ? 1 : 0;
			if (--count > 0)

				System.Threading.Monitor.Wait(this);

			else {

				count = initCount;
                predicate_sum_final = predicate_sum; 
                predicate_sum = 0;
				System.Threading.Monitor.PulseAll(this);

			}

			System.Threading.Monitor.Exit(this);
            return predicate_sum_final;
		}

        public virtual void gather()
        {
            System.Threading.Monitor.Enter(this);
            if (--count > 0)

                System.Threading.Monitor.Wait(this);

            else
            {
                count = initCount;
                System.Threading.Monitor.PulseAll(this);

            }

            System.Threading.Monitor.Exit(this);
        }

		/// <summary>
		/// Calls gather();
		/// </summary>
		public virtual void SignalAndWait()
		{
			gather();
		}

         /// <summary>
        /// Calls gather(predicate); Returns sum of true predicates within block
        /// </summary>
        public virtual int SignalAndWaitAndCountPredicate(bool predicate)
        {
            return gather(predicate);
        }

	}

}

