using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class TextAssetField
{
    [SerializeField] private TextAsset textAsset;

    public static implicit operator TextAsset(TextAssetField textAssetField)
    {
        return textAssetField.textAsset;
    }
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(TextAssetField))]
public class TextAssetFieldPropertyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, GUIContent.none, property);
        SerializedProperty textAsset = property.FindPropertyRelative("textAsset");
        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);
        if (textAsset != null)
            textAsset.objectReferenceValue = EditorGUI.ObjectField(position, textAsset.objectReferenceValue, typeof(TextAsset), false);
        EditorGUI.EndProperty();
    }
}
#endif