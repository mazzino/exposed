using Assets.Exposed.Scripts.Prefs;
using Exposed.Prefs;
using UnityEditor;
using UnityEngine;

namespace Exposed.Editor.Prefs
{
    [CustomPropertyDrawer(typeof(ExposedInt))]
    public class ExposedIntPropertyDrawer : PropertyDrawer
    {
        // Draw the property inside the given rect
        public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
        {
            string prefKey = property.name;
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty (position, label, property);

            bool added = PrefsRegister.Instance.ContainsPref(prefKey);

            position = added ? AddedElementGUI(position, prefKey) : RemovedElementGUI(position, label);
            
            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            
            // Calculate rects
            float buttonWidth = 19;
            float spaceWidth = 3;
            var buttonRect = new Rect (position.x, position.y, buttonWidth, position.height);
            var valueRect = new Rect (position.x+buttonWidth+spaceWidth, position.y, position.width-buttonWidth-spaceWidth, position.height);

            EditorGUI.BeginChangeCheck();
            if (added) { RemoveButton(buttonRect, prefKey); } else { AddButton(buttonRect, prefKey);}
            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(PrefsRegister.Instance);
                AssetDatabase.SaveAssets();
            }
            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            //property.FindPropertyRelative("_prefId").stringValue = prefKey;
            EditorGUI.PropertyField (valueRect, property.FindPropertyRelative ("_value"), GUIContent.none);
            //int value = property.FindPropertyRelative("_value");
            /*int value = property.
            value = EditorGUI.IntField(valueRect, GUIContent.none, value);*/
            
            // Set indent back to what it was
            EditorGUI.indentLevel = indent;
            
            EditorGUI.EndProperty ();
        }

        private Rect AddedElementGUI(Rect position, string prefKey)
        {
            Rect originalRect = position;
            
            // Draw label
            position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), new GUIContent(" "));

            // Draw popup for visual control
            string[] prefKeys;
            int keyIndex = PrefsRegister.Instance.FillArrayAndGetIndexOfKey(out prefKeys, prefKey);
            Rect popupRect = new Rect(originalRect.x, originalRect.y, originalRect.width - position.width - 12, originalRect.height);
            EditorGUI.Popup(popupRect, keyIndex, prefKeys);

            return position;
        }

        private Rect RemovedElementGUI(Rect position, GUIContent label)
        {
            // Draw label
            position = EditorGUI.PrefixLabel (position, GUIUtility.GetControlID (FocusType.Passive), label);

            return position;
        }

        private void AddButton(Rect buttonRect, string id)
        {
            if (GUI.Button(buttonRect, "+"))
            {
                PrefsRegister.Instance.AddPref(id, new PrefInfo(PrefType.PlayerPrefs));       
            }
        }

        private void RemoveButton(Rect buttonRect, string id)
        {
            if (GUI.Button(buttonRect, "-"))
            {
                PrefsRegister.Instance.RemovePref(id);    
            }
        }
    }
}