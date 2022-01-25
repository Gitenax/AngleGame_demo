using Gitenax.AngleCheckers.Players;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Player), true)]
public class PlayerPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position = EditorGUI.PrefixLabel(position, new GUIContent("Игрок"));
        EditorGUI.PropertyField(position, property.FindPropertyRelative("_name"), GUIContent.none);
    }
}