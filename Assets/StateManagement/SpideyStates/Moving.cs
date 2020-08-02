using QuikGraph;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.SpideyActions.SpideyStates
{
    public class Moving : GenericStateHandler<SpiderCrawly>
    {

        public Task<GenericStateHandler<SpiderCrawly>> HandleState(SpiderCrawly data)
        {
            if (data.lastNode == null || data.currentConnection == null)
            {
                return Task.FromResult<GenericStateHandler<SpiderCrawly>>(new WaitForValid(this));
            }
            if (data.isMoving)
            {
                foreach (var winZone in data.winZones)
                {
                    if (winZone.TryTriggerwin(data))
                    {
                        data.StopMoving();
                        return Task.FromResult<GenericStateHandler<SpiderCrawly>>(this);
                    }
                }

                data.distanceAlongConnection += Time.deltaTime * data.movementSpeed;
            }
            if (data.distanceAlongConnection >= 1)
            {
                var nextState = data.GetNextAction();
                return Task.FromResult(nextState.StateHandlerFactory(this));
            }
            return Task.FromResult<GenericStateHandler<SpiderCrawly>>(this);
        }

        public void TransitionIntoState(SpiderCrawly data)
        {
            if (data.distanceAlongConnection >= 1)
            {
                var currentNode = data.currentConnection.GetOtherVertex(data.lastNode);
                var nextConnection = data.PickNextConnection(data.extraIgnoreConnections);
                data.extraIgnoreConnections.Clear();
                data.lastNode = currentNode;
                data.currentConnection = nextConnection;

                data.distanceAlongConnection = 0;
            }
        }

        public void TransitionOutOfState(SpiderCrawly data)
        {
        }
    }
}