using UnityEditor;
using UnityEngine;
using System.Collections;

namespace RopePhysics {

    [CustomEditor(typeof(PointMass))]
    public class PointMassEditor : Editor {
    	public const string PROP_PARENT = "parent";
    	public const string PROP_MASS = "Mass";
    	public const string PROP_KINEMATIC = "Kinematic";

    	private SerializedProperty _parentProp;

    	void OnEnable() {
    		_parentProp = serializedObject.FindProperty(PROP_PARENT);
    	}

    	public override void OnInspectorGUI () {
    		serializedObject.Update();

    		var pmass = target as PointMass;
    		EditorGUILayout.PropertyField(_parentProp);
    		pmass.Mass = EditorGUILayout.FloatField(PROP_MASS, pmass.Mass);
    		pmass.Kinematic = EditorGUILayout.Toggle(PROP_KINEMATIC, pmass.Kinematic);

    		if (GUI.changed)
    			EditorUtility.SetDirty(pmass);

    		serializedObject.ApplyModifiedProperties();
    	}
    }
}