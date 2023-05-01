using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using System.Linq;

[CustomPropertyDrawer(typeof(TeamRelationship.Relationship))]
public class RelationshipProperty : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		//position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		Rect tRect = new(position.x,       position.y, 100, position.height);
		Rect vRect = new(position.x + 105, position.y,  20, position.height);

		var tProp = property.FindPropertyRelative("teamMask");

		EditorGUI.BeginChangeCheck();
		int a = EditorGUI.MaskField(tRect, GUIContent.none, tProp.intValue, tProp.enumNames);
		if (EditorGUI.EndChangeCheck())
		{
			tProp.intValue = a;
		}
		EditorGUI.PropertyField(vRect, property.FindPropertyRelative("value"), GUIContent.none);

		EditorGUI.EndProperty();
	}
}