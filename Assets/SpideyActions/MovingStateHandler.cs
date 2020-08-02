using QuikGraph;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Utilities
{
    public class MovingStateHandler : GenericStateHandler<SpiderCrawly>
    {

        public Task<GenericStateHandler<SpiderCrawly>> HandleState(SpiderCrawly data)
        {
            if (data.lastNode == null || data.currentConnection == null)
            {
                return Task.FromResult<GenericStateHandler<SpiderCrawly>>(new WaitForValidStateHandler(this));
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
            Vector2 b = data.currentConnection.GetOtherVertex(data.lastNode).transform.position;

            if (data.distanceAlongConnection >= 1)
            {
                var nextState = data.GetNextAction();
                return Task.FromResult(nextState.StateHandlerFactory(this));
            }

            Vector2 a = data.lastNode.transform.position;
            var diff = b - a;

            var scaledDiff = diff * data.distanceAlongConnection;
            data.transform.position = a + scaledDiff;
            switch (data.whichSide)
            {
                case TraversalSide.LEFTHAND:
                    data.transform.position = (Vector2)data.transform.position + (data.sideOffset * diff.normalized.Rotate(90));
                    break;
                case TraversalSide.RIGHTHAND:
                    data.transform.position = (Vector2)data.transform.position + (-data.sideOffset * diff.normalized.Rotate(90));
                    break;
            }
            return Task.FromResult<GenericStateHandler<SpiderCrawly>>(this);
        }

        public void TransitionIntoState(SpiderCrawly data)
        {
            if (data.distanceAlongConnection >= 1)
            {
                var currentNode = data.currentConnection.GetOtherVertex(data.lastNode);
                var nextConnection = data.PickNextConnection();
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