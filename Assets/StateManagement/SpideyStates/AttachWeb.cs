using QuikGraph;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.SpideyActions.SpideyStates
{
    public class AttachWeb : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToOnsuccess;
        public AttachWeb(GenericStateHandler<SpiderCrawly> returnToOnsuccess)
        {
            this.returnToOnsuccess = returnToOnsuccess;
        }

        public GenericStateHandler<SpiderCrawly> HandleState(SpiderCrawly crawly)
        {
            var totalDelay = .7f;
            if (!crawly.draggingLineRenderer.gameObject.activeInHierarchy)
            {
                // it broke!
                return new Waiting(returnToOnsuccess, totalDelay);
            }

            var graph = crawly.graphManager;
            var lastNode = crawly.lastNode;
            var currentNode = crawly.currentConnectionForInspector.GetOtherVertex(lastNode);

            var otherNode = crawly.currentDraggingNode;
            if (otherNode == null)
            {
                Debug.LogError("Error: attempted to attach a connection when none was dragged");
                return returnToOnsuccess;
            }

            var currentConnectionLength = (currentNode.transform.position - otherNode.transform.position).magnitude;

            var connection = graph.CreateConnection(currentNode, otherNode, currentConnectionLength);

            crawly.draggingLineRenderer.InstantClearConnection();

            var actionSeries = new List<(float, Action)>();

            var lengthSteps = 10;
            for (var i = 0; i <= lengthSteps; i++)
            {
                var targetDistance = Mathf.Lerp(currentConnectionLength, graph.defaultConnectionLength, (float)i / lengthSteps);
                actionSeries.Add((totalDelay / lengthSteps, () =>
                {
                    connection.targetDistance = targetDistance;
                }
                ));
                //connection.targetDistance = Mathf.Lerp(currentConnectionLength, graph.defaultConnectionLength, (float)i / lengthSteps);
            }

            actionSeries.Add((0, () =>
            {
                crawly.extraIgnoreConnections.Add(connection);
            }
            ));

            return new Waiting(returnToOnsuccess, totalDelay / lengthSteps, actionSeries);

        }

        public void TransitionIntoState(SpiderCrawly data)
        {
        }

        public void TransitionOutOfState(SpiderCrawly data)
        {
        }
    }
}