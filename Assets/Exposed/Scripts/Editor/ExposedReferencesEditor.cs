using System.Collections.Generic;
using System.Reflection;
using Exposed.Config;
using Exposed.Editor.Config;
using Exposed.Editor.DI;
using Autofac;
using UnityEditor;
using UnityEngine;

namespace Exposed.Editor
{
    [CustomEditor(typeof(ExposedReferences))]
    public class ExposedReferencesEditor : UnityEditor.Editor
    {
        private IComponentObtainer _componentObtainer;
        private IFieldsObtainer _fieldsObtainer;
        private IFieldsRenderer _fieldsRenderer;
        private IFieldsProcessor _fieldsProcessor;

        private ExposedReferences _exposedReferences;
        private List<ExposedMapping> _scriptMappings;
        private bool[] _configFoldouts;
        private GUIStyle _foldoutStyle;

        private bool _isPrefab;

        public void Awake()
        {
#if UNITY_4_5 || UNITY_4_6 || UNITY_4_7
            _foldoutStyle = new GUIStyle{normal = {background = null}};
#else
            _foldoutStyle = new GUIStyle(EditorStyles.foldout) {normal = {background = null}};
#endif
        }

        public void OnEnable()
        {
            //DI--------------
            _componentObtainer = ExposedEditorDi.Instance.Container.Resolve<IComponentObtainer>();
            _fieldsObtainer = ExposedEditorDi.Instance.Container.Resolve<IFieldsObtainer>();
            _fieldsRenderer = ExposedEditorDi.Instance.Container.Resolve<IFieldsRenderer>();
            _fieldsProcessor = ExposedEditorDi.Instance.Container.Resolve<IFieldsProcessor>();
            //----------------

            _exposedReferences = (ExposedReferences) target;
            _exposedReferences.PreInitializationEnabled = _exposedReferences.hideFlags == HideFlags.None;

            _scriptMappings = _componentObtainer.GetComponentMappingsOnSameGameObject(_exposedReferences);

            _configFoldouts = new bool[_scriptMappings.Count];
            for (int index = 0; index < _configFoldouts.Length; index++)
            {
                _configFoldouts[index] = true;
            }

            _isPrefab = _exposedReferences.hideFlags != HideFlags.None;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            Undo.RecordObject(_exposedReferences, "Exposed Change");

            //processing components added on gameobject
            for (int index = 0; index < _scriptMappings.Count; index++)
            {
                var scriptMapping = _scriptMappings[index];
                EditorGUILayout.BeginHorizontal();
                //EditorGUI.BeginDisabledGroup(true);
                //EditorGUILayout.ObjectField("", parentScriptObject, typeof (MonoScript), false);
                _exposedReferences.SetConfigForScriptObject(scriptMapping.AffectedComponent,
                    (ExposedConfiguration) EditorGUILayout.ObjectField(scriptMapping.AffectedComponent.GetType().Name,
                        _exposedReferences.GetConfigForScript(scriptMapping.AffectedComponent), typeof (ExposedConfiguration),
                        false));
                EditorGUILayout.EndHorizontal();


                var config = _exposedReferences.GetConfigForScript(scriptMapping.AffectedComponent);
                if (config != null)
                {
                    EditorGUILayout.BeginVertical(_foldoutStyle);
                    _configFoldouts[index] = EditorGUILayout.Foldout(_configFoldouts[index], "Variables with references");

                    List<FieldInfo> fields = _fieldsObtainer.GetReferenceFieldsForType(scriptMapping.Type);
                    if (_configFoldouts[index])
                    {
                        EditorGUI.BeginChangeCheck();
                        _fieldsRenderer.RenderFieldsForType(fields, scriptMapping.AffectedComponent, config);
                        Undo.RecordObject(config, "Exposed Change");
                        //Undo.RegisterCompleteObjectUndo(configuration, "Rules Change");
                        if (EditorGUI.EndChangeCheck())
                        {
                            EditorUtility.SetDirty(config);
                        }
                    }
                    //references update happens only in editor mode and on nonprefab objects
                    if (!EditorApplication.isPlaying && !_isPrefab)
                    {
                        _fieldsProcessor.ProcessFieldsForType(fields, scriptMapping.AffectedComponent, config);
                        EditorUtility.SetDirty(scriptMapping.AffectedComponent);
                    }
                    EditorGUILayout.EndVertical();
                }
            }

            //EditorGUILayout.LabelField(string.Format("Global caching is {0}", ExposedScriptConfigurationManagerAsset.Instance.PreInitializationEnabled ? "enabled" : "disabled"));
            if (_isPrefab)
            {
                _exposedReferences.LateUpdate = EditorGUILayout.Toggle("Late references update",
                    _exposedReferences.LateUpdate);
            }

            /*if (GUILayout.Button("Set as Test"))
            {
                var mainComponent = _exposedReferences.GetComponent<MainComponent>();
                var mainComponentTestVerifier = _exposedReferences.GetComponent<MainComponentTestVerifier>();

                mainComponentTestVerifier.Component = mainComponent.Component;
                mainComponentTestVerifier.ArrayOfComponents = mainComponent.ArrayOfComponents;
                mainComponentTestVerifier.ListOfComponents = mainComponent.ListOfComponents;

                mainComponentTestVerifier.ComponentGameObject = mainComponent.ComponentGameObject;
                mainComponentTestVerifier.ArrayOfGameObjects = mainComponent.ArrayOfGameObjects;
                mainComponentTestVerifier.ListOfGameObjects = mainComponent.ListOfGameObjects;
            }*/

            if (GUI.changed)
            {
                //Undo.RegisterCompleteObjectUndo(_exposedReferences, "Exposed Change");
                EditorUtility.SetDirty(_exposedReferences);
                //EditorApplication.MarkSceneDirty();
            }
        }

    }
}