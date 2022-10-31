using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(EvaluationGenerate))]
public class EvaluationGenerateEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var obj = (EvaluationGenerate)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Generate"))
        {
            obj.Generate();
        }
    }
}
