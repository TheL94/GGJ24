using System.Collections.Concurrent;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SplitFace.ModularSpawnSystem.SpawnSystemEditor
{
    [CustomPropertyDrawer(typeof(Wave))]
    public class WavePropertyDrawer : PropertyDrawer
    {
        SerializedProperty unitsList;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property == null)
                return;

            unitsList = property.FindPropertyRelative("unitsToSpawn");

            EditorGUI.BeginProperty(position, label, property);

            var shuffleOnSpawn = property.FindPropertyRelative("shuffleOnSpawn");
            var waveName = property.FindPropertyRelative("waveName");

            waveName.stringValue = EditorGUI.TextField(new Rect(position.x,
                position.y,
                position.width,
                EditorGUIUtility.singleLineHeight),
                "Wave Name",
                waveName.stringValue);

            position.y += EditorGUIUtility.singleLineHeight;

            shuffleOnSpawn.boolValue = GUI.Toggle(new Rect(
                position.x,
                position.y,
                position.width / 2 - 5,
                EditorGUIUtility.singleLineHeight),
                shuffleOnSpawn.boolValue,
                "Shuffle on Spawn");

            position.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(position, unitsList, true);
            //UnitsList.DoList(position);

            property.serializedObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            unitsList = property.FindPropertyRelative("unitsToSpawn");

            return EditorGUI.GetPropertyHeight(unitsList) + 2 * EditorGUIUtility.singleLineHeight;
        }
    }
}