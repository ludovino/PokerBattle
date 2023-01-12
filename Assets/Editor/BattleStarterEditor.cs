using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(BattleController))]
public class BattleControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var obj = (BattleController)target;
        base.OnInspectorGUI();
        if (GUILayout.Button("Start Battle"))
        {
            obj.Init();
        }
    }
}
