using QuikGraph;
using System.Threading.Tasks;

namespace Assets.SpideyActions.SpideyStates
{
    public class DragWeb : GenericStateHandler<SpiderCrawly>
    {
        private GenericStateHandler<SpiderCrawly> returnToOnsuccess;
        public DragWeb(GenericStateHandler<SpiderCrawly> returnToOnsuccess)
        {
            this.returnToOnsuccess = returnToOnsuccess;
        }

        public async Task<GenericStateHandler<SpiderCrawly>> HandleState(SpiderCrawly crawly)
        {
            var lastNode = crawly.lastNode;
            var currentNode = crawly.currentConnection.GetOtherVertex(lastNode);
            crawly.draggingLineRenderer.gameObject.SetActive(true);
            crawly.draggingLineRenderer.Source = crawly.gameObject;
            crawly.draggingLineRenderer.Target = currentNode.gameObject;

            crawly.currentDraggingConnection = currentNode;
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