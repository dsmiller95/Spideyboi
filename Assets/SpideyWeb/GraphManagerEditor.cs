
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
        }
    }
}
