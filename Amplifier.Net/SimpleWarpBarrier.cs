// Based on SimpleBarrier. David Hughes ("crakinshot''at""yahoo'"dotty'com), 2012

using System;
using System.Threading;
using System.Linq;

namespace info.jhpc.warp
{
    public class SimpleWarpBarrier
    {

        protected int[] count;
        protected int[] predicate_sum;
        protected int[] predicate_final;
        protected int[] initCount;
        protected object[] syncers;

        public SimpleWarpBarrier(int warps, int warpsize)
        {
            if (warps <= 0)
                throw new ArgumentException("Warp Barrier initialization specified non-positive value for Warp Count " + warps);
            if (warpsize <= 0)
                throw new ArgumentException("Warp Barrier initialization specified non-positive value for Warp Size" + warpsize);

            count = new int[warps];
            initCount = new int[warps];
            predicate_sum = new int[warps];
            predicate_final = new int[warps];
            syncers = new object[warps];
            for (var i = 0; i < warps; i++)
            {
                count[i] = initCount[i] = warpsize;
                predicate_sum[i] = 0;
                syncers[i] = new object();
            }
        }

        public virtual int gather_ballot(bool predicate, int warpid)
        {

            System.Threading.Monitor.Enter(syncers[warpid]);

            predicate_sum[warpid] += (predicate) ? 1 : 0;

            if (--count[warpid] > 0)

                System.Threading.Monitor.Wait(syncers[warpid]);

            else
            {

                count[warpid] = initCount[warpid];
                predicate_final[warpid] = predicate_sum[warpid];
                predicate_sum[warpid] = 0;

                System.Threading.Monitor.PulseAll(syncers[warpid]);

            }

            System.Threading.Monitor.Exit(syncers[warpid]);

            return predicate_final[warpid];
        }

        public virtual bool gather_any(bool predicate, int warpid)
        {

            System.Threading.Monitor.Enter(syncers[warpid]);

            if (count[warpid] == initCount[warpid])
                predicate_sum[warpid] = (predicate) ? 1 : 0;
            else
                predicate_sum[warpid] |= (predicate) ? 1 : 0;

            if (--count[warpid] > 0)

                System.Threading.Monitor.Wait(syncers[warpid]);

            else
            {
                count[warpid] = initCount[warpid];
                predicate_final[warpid] = predicate_sum[warpid];
                predicate_sum[warpid] = 0;

                System.Threading.Monitor.PulseAll(syncers[warpid]);

            }

            System.Threading.Monitor.Exit(syncers[warpid]);

            return predicate_final[warpid] > 0;
        }

        public virtual bool gather_all(bool predicate, int warpid)
        {

            System.Threading.Monitor.Enter(syncers[warpid]);

            if (count[warpid] == initCount[warpid])
                predicate_sum[warpid] = (predicate) ? 1 : 0;
            else
                predicate_sum[warpid] &= (predicate) ? 1 : 0;

            predicate_sum[warpid] |= (predicate) ? 1 : 0;

            if (--count[warpid] > 0)

                System.Threading.Monitor.Wait(syncers[warpid]);

            else
            {
                count[warpid] = initCount[warpid];
                predicate_final[warpid] = predicate_sum[warpid];
                predicate_sum[warpid] = 0;

                System.Threading.Monitor.PulseAll(syncers[warpid]);

            }

            System.Threading.Monitor.Exit(syncers[warpid]);

            return predicate_final[warpid] > 0;
        }


        /// <summary>
        /// Calls gather();
        /// </summary>
        public virtual bool SignalAnyPredicateAndWait(bool predicate, int warpId)
        {
            return gather_any(predicate, warpId);
        }

        /// <summary>
        /// Calls gather();
        /// </summary>
        public virtual bool SignalAllPredicateAndWait(bool predicate, int warpId)
        {
            return gather_all(predicate, warpId);
        }

        /// <summary>
        /// Calls gather();
        /// </summary>
        public virtual int SignalBallotPredicateAndWait(bool predicate, int warpId)
        {
            return gather_ballot(predicate, warpId);
        }
    }
}
