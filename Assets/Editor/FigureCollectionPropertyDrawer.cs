using PlayArea;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomPropertyDrawer(typeof(LogicArrayLayout))]
    public class FigureCollectionPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            position.height = 0;
            position.x = 0;
            
            var newPosition = position;
            newPosition.y += 8f;
            
            var rows = property.FindPropertyRelative("Rows");
            
            for (var i = 0; i < rows.arraySize; i++)
            {
                var columns = rows.GetArrayElementAtIndex(i).FindPropertyRelative("Columns");
                
                newPosition.width = position.width / columns.arraySize;
                for (var j = 0; j < columns.arraySize; j++)
                {
                    EditorGUI.PropertyField(newPosition, columns.GetArrayElementAtIndex(j), GUIContent.none);
                    newPosition.x += newPosition.width;
                }
                newPosition.x = position.x;
                newPosition.y += 18f;
            }
            newPosition.y += 18f;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var rows = property.FindPropertyRelative("Rows");
            return 18f * rows.arraySize;
        }
    }
}