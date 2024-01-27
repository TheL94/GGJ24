using UnityEditor;
using UnityEngine;

namespace SplitFace.ModularSpawnSystem.SpawnSystemEditor
{
    [CustomPropertyDrawer(typeof(WaveUnit))]
    public class WaveUnitPropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position.y += 2;

            EditorGUI.PropertyField(
                new Rect(position.x, position.y, position.width - 40, EditorGUIUtility.singleLineHeight),
                property.FindPropertyRelative("slotsOccupied"),
                new GUIContent("Slots Occupied"));

            EditorGUI.LabelField(
                new Rect(position.x, position.y + 20, position.width - 60 - 60, EditorGUIUtility.singleLineHeight),
                new GUIContent("Prefab"));

            EditorGUI.PropertyField(
                new Rect(position.x + 50, position.y + 20, position.width - 120, EditorGUIUtility.singleLineHeight),
                property.FindPropertyRelative("prefab"),
                GUIContent.none);

            EditorGUI.LabelField(
                new Rect(position.x + position.width - 70, position.y + 20, 60, EditorGUIUtility.singleLineHeight),
                new GUIContent("Count"));

            EditorGUI.PropertyField(
                new Rect(position.x + position.width - 30, position.y + 20, 30, EditorGUIUtility.singleLineHeight),
                property.FindPropertyRelative("amountToSpawn"),
                GUIContent.none);

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return 2.5f * EditorGUIUtility.singleLineHeight;
        }
    }
}