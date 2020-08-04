using QuikGraph;
using UnityEngine;

namespace Assets.SpideyActions.SpideyStates
{
    public class Moving : GenericStateHandler<SpiderCrawly>
    {

        public GenericStateHandler<SpiderCrawly> HandleState(SpiderCrawly data)
        {
            if (data.lastNode == null || data.CurrentConnection == null)
            {
                return new WaitForValid(this);
            }
            if (data.isMoving)
            {
                foreach (var winZone in data.winZones)
                {
                    if (winZone.TryTriggerwin(data))
                    {
                        data.StopMoving();
                        return this;
                    }
                }

                data.DistanceAlongConnection += Time.deltaTime * data.movementSpeed;
            }
            if (data.DistanceAlongConnection >= 1)
            {
                var nextState = data.GetNextAction();
                return nextState.StateHandlerFactory(this);
            }
            return this;
        }

        public void TransitionIntoState(SpiderCrawly data)
        {
            if (data.DistanceAlongConnection >= 1)
            {
                var currentNode = data.CurrentConnection.GetOtherVertex(data.lastNode);
                var nextConnection = data.PickNextConnection(data.extraIgnoreConnections);
                data.extraIgnoreConnections.Clear();
                data.lastNode = currentNode;
                data.CurrentConnection = nextConnection;

                data.DistanceAlongConnection = 0;
            }
        }

        public void TransitionOutOfState(SpiderCrawly data)
        {
        }
    }
}