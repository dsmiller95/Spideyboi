using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SpideyActions.SpideyStates
{
    public class Waiting : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToState;
        private float delay;


        public Waiting(GenericStateHandler<SpiderCrawly> returnToState, float time)
        {
            delay = time;
            this.returnToState = returnToState;
        }
        IList<(float, Action)> WaitSections;
        /// <summary>
        /// Wait <paramref name="time"/> seconds, then dequeue an item from WaitSections and execute it's action. Then wait the corresponding amoutn of time, and repeat
        /// </summary>
        /// <param name="returnToState"></param>
        /// <param name="time"></param>
        /// <param name="WaitSections"></param>
        public Waiting(GenericStateHandler<SpiderCrawly> returnToState, float time, IList<(float, Action)> WaitSections)
        {
            this.delay = time;
            this.WaitSections = WaitSections;
            this.returnToState = returnToState;
        }


        public GenericStateHandler<SpiderCrawly> HandleState(SpiderCrawly data)
        {
            if (nextTriggerTime < Time.time)
            {
                if (pendingActions.Count <= 0)
                {
                    return returnToState;
                }

                var nextAction = pendingActions.Dequeue();
                nextTriggerTime = nextAction.Item1 + Time.time;
                nextAction.Item2?.Invoke();
                return this;
            }
            return this;
        }

        private Queue<(float, Action)> pendingActions;
        private float nextTriggerTime;

        public void TransitionIntoState(SpiderCrawly data)
        {
            if (WaitSections != null)
            {
                pendingActions = new Queue<(float, Action)>(WaitSections);
            }
            else
            {
                pendingActions = new Queue<(float, Action)>();
            }
            nextTriggerTime = Time.time + delay;
        }
        public void TransitionOutOfState(SpiderCrawly data)
        {
        }
    }
}