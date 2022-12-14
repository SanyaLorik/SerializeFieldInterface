using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

namespace SanyaLorik.Tools
{
    [CustomEditor(typeof(UnityEngine.Object), true)]
    public class InterfaceEditor : Editor
    {
        private const BindingFlags _fieldFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;
        private readonly Type _type = typeof(SerializeFieldInterfaceAttribute);
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            foreach (FieldInfo field in Fields)
            {
                if (TryFitObjectField(field, out var gameObject) == false)
                    continue;

                SetValue(field, gameObject);
            }
        }

        private IEnumerable<FieldInfo> Fields
        {
            get => target.GetType().GetFields(_fieldFlags)
                .Where(field => field.CustomAttributes
                    .Count(attribute => attribute.AttributeType == _type) != 0)
                .Where(field => field.FieldType.IsInterface == true);
        }

        private bool TryFitObjectField(FieldInfo field, out GameObject gameObject)
        {
            var fieldValue = field.GetValue(target) as UnityEngine.Object;
            var fieldName = NamingEditor.Edit(field.Name);
            
            gameObject = EditorGUILayout.ObjectField(fieldName, fieldValue, typeof(object), true) as GameObject;
            
            return gameObject != null;
        }

        private void SetValue(FieldInfo field, GameObject gameObject)
        {
            var value = gameObject.GetComponent(field.FieldType);
            field.SetValue(target, value);
        }
    }
}