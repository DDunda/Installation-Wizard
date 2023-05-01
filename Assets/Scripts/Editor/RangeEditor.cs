using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Range<>))]
public class Range_Property : PropertyDrawer
{
	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		var minRect = new Rect(position.x + 00, position.y, 50, position.height);
		//var txtRect = new Rect(position.x + 53, position.y, 10, position.height);
		var maxRect = new Rect(position.x + 55, position.y, 50, position.height);

		var indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;

		EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"), GUIContent.none);
		//EditorGUI.LabelField(txtRect, "-");
		EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"), GUIContent.none);

		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}
}