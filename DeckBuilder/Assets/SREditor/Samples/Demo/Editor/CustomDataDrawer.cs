using Demo;

#if UNITY_EDITOR
using SerializeReferenceEditor.Editor;
using UnityEditor;
#endif
using UnityEngine;

namespace Demo.Editor
{
#if UNITY_EDITOR
	[CustomPropertyDrawer(typeof(CustomData), false)]
	public class CustomDataDrawer : PropertyDrawer
	{
		private SRDrawer _drawer = new();
		
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			var dataProperty = property.FindPropertyRelative("Data");
			_drawer.Draw(position, dataProperty, new GUIContent("Custom Title"));
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			var dataProperty = property.FindPropertyRelative("Data");
			return _drawer.GetPropertyHeight(dataProperty, label);
		}
	}
#endif
}