using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ExtendedText))]
public class ExtendedTextEditor : Editor
{
    private ExtendedText myTarget;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        myTarget = (ExtendedText)target;
     
        if (GUI.changed)
            myTarget.updateUnderline();
    }
}
