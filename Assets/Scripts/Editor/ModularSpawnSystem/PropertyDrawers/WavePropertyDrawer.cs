using System.Collections.Concurrent;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace SplitFace.ModularSpawnSystem.SpawnSystemEditor
{
    [CustomPropertyDrawer(typeof(Wave))]
    public class WavePropertyDrawer : PropertyDrawer
    {
        bool editToggle = false;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.PropertyField(new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight + 2f), property, label);

            if (property.objectReferenceValue == null)
            {
                return;
            }

            position.y += EditorGUIUtility.singleLineHeight;

            var assetObject = new SerializedObject(property.objectReferenceValue);

            EditorGUI.BeginProperty(position, label, property);

            var shuffleOnSpawn = assetObject.FindProperty("shuffleOnSpawn");
            var isInfinite = assetObject.FindProperty("isInfinite");
            var waveName = assetObject.FindProperty("waveName");

            if (!editToggle)
            {
                GUI.Box(new Rect(position.x, position.y + 1f, position.width / 2 - 5, EditorGUIUtility.singleLineHeight + 2f), waveName.stringValue);
                editToggle = GUI.Button(new Rect(position.x + position.width - position.width / 2, position.y + 3f, position.width / 2 - 5, EditorGUIUtility.singleLineHeight), "Edit");
                return;
            }

            editToggle = !GUI.Button(new Rect(position.x, position.y, position.width / 2 - 5, EditorGUIUtility.singleLineHeight), "Done");

            position.y += EditorGUIUtility.singleLineHeight + 2f;

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

            isInfinite.boolValue = GUI.Toggle(new Rect(
                position.x,
                position.y,
                position.width / 2 - 5,
                EditorGUIUtility.singleLineHeight),
                isInfinite.boolValue,
                "Is Infinite");

            position.y += EditorGUIUtility.singleLineHeight;

            EditorGUI.PropertyField(position, assetObject.FindProperty("unitsToSpawn"), true);

            property.serializedObject.ApplyModifiedProperties();
            assetObject.ApplyModifiedProperties();

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue == null)
                return EditorGUIUtility.singleLineHeight;

            var assetObject = new SerializedObject(property.objectReferenceValue);
            var unitsList = assetObject.FindProperty("unitsToSpawn");

            return editToggle ? EditorGUI.GetPropertyHeight(unitsList) + 5f * EditorGUIUtility.singleLineHeight : (EditorGUIUtility.singleLineHeight + 2f) * 2f;
        }
    }
}