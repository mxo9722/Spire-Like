using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ScenePicker), true)]
public class ScenePickerEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

        int indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        SerializedProperty scenePathProperty = property.FindPropertyRelative("scenePath");

        SceneAsset oldScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePathProperty?.stringValue ?? "");

        EditorGUI.BeginChangeCheck();

        SceneAsset newScene = EditorGUI.ObjectField(position, oldScene, typeof(SceneAsset), false) as SceneAsset;

        if (EditorGUI.EndChangeCheck())
        {
            string newPath = AssetDatabase.GetAssetPath(newScene);
            scenePathProperty.stringValue = newPath;

            property.serializedObject.ApplyModifiedProperties();
        }

        EditorGUI.indentLevel = indent;

        EditorGUI.EndProperty();
    }
}