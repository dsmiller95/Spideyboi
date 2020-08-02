
using UnityEditor;
using UnityEngine;

namespace Assets.SpideyWeb
{
    [CustomEditor(typeof(GraphManager))]
    public class GraphManagerEditor: Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            var graph = target as GraphManager;
            if (GUILayout.Button("Test"))
            {
                Debug.Log("It's alive: " + target.name);
            }
            if (GUILayout.Button("New Node"))
            {
                graph.CreateNewNodeWithWobble();
            }
            if (GUILayout.Button("New Connection"))
            {
                graph.CreatRandomConnection();
            }
        }
    }
}
