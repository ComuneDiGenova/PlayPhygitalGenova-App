using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;

[CustomEditor(typeof(CameraOrbitRT))]
public class CameraOrbitRTEditor : Editor
{
    //private SerializedProperty prop;

    void OnEnable(){
        //prop = serializedObject.FindProperty("prop");
    }

    public override void OnInspectorGUI()
    {

        //base.OnInspectorGUI();
        //EditorGUILayout.PropertyField(prop,true);
    }
}

#endif