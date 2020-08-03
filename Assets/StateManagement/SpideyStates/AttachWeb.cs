﻿using QuikGraph;
using System.Threading.Tasks;
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

        public async Task<GenericStateHandler<SpiderCrawly>> HandleState(SpiderCrawly crawly)
        {
            var totalDelay = 700;
            if (!crawly.draggingLineRenderer.gameObject.activeInHierarchy)
            {
                // it broke!
                return new Waiting(this.returnToOnsuccess, totalDelay / 1000f);
            }

            var graph = crawly.graphManager;
            var lastNode = crawly.lastNode;
            var currentNode = crawly.currentConnectionForInspector.GetOtherVertex(lastNode);

            var otherNode = crawly.currentDraggingConnection;
            if(otherNode == null)
            {
                Debug.LogError("Error: attempted to attach a connection when none was dragged");
                return returnToOnsuccess;
            }

            var currentConnectionLength = (currentNode.transform.position - otherNode.transform.position).magnitude;

            var connection = graph.CreateConnection(currentNode, otherNode, currentConnectionLength);

            crawly.draggingLineRenderer.gameObject.SetActive(false);

            var lengthSteps = 10;
            for (var i = 0; i <= lengthSteps; i++)
            {
                await Task.Delay(totalDelay / lengthSteps);
                connection.targetDistance = Mathf.Lerp(currentConnectionLength, graph.defaultConnectionLength, (float)i / lengthSteps);
            }

            crawly.extraIgnoreConnections.Add(connection);
            //var connectionsSpring = connection.GetComponentInChildren<RealSpring>();
            //if(connectionsSpring != null)
            //{
            //    connectionsSpring.ImpulseOnNext(1);
            //}

            return returnToOnsuccess;
        }

        public void TransitionIntoState(SpiderCrawly data)
        {
        }

        public void TransitionOutOfState(SpiderCrawly data)
        {
        }
    }
}