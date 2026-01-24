// <copyright file="DOTSClipEditor.cs" company="BovineLabs">
//     Copyright (c) BovineLabs. All rights reserved.
// </copyright>

namespace BovineLabs.Timeline.Editor
{
    using System;
    using System.Collections.Generic;
    using BovineLabs.Core.Editor.Helpers;
    using BovineLabs.Core.Editor.Inspectors;
    using BovineLabs.Timeline.Authoring;
    using UnityEditor;
    using UnityEngine.UIElements;

    [CustomEditor(typeof(DOTSClip), true)]
    public class DOTSClipEditor : ElementEditor
    {
        private const string OverridePrefix = "override";

        private readonly HashSet<string> overridePaths = new();
        private readonly HashSet<string> valuePaths = new();

        protected override bool PreElementCreation(VisualElement root)
        {
            this.CacheOverridePairs();
            return base.PreElementCreation(root);
        }

        protected override VisualElement CreateElement(SerializedProperty property)
        {
            if (this.overridePaths.Contains(property.propertyPath))
            {
                return new ToggleOption(this.serializedObject, property.propertyPath, GetValuePath(property));
            }

            if (this.valuePaths.Contains(property.propertyPath))
            {
                return null;
            }

            return base.CreateElement(property);
        }

        private void CacheOverridePairs()
        {
            this.overridePaths.Clear();
            this.valuePaths.Clear();

            foreach (var property in SerializedHelper.IterateAllChildren(this.serializedObject, false))
            {
                if (property.propertyType != SerializedPropertyType.Boolean)
                {
                    continue;
                }

                if (!property.name.StartsWith(OverridePrefix, StringComparison.Ordinal))
                {
                    continue;
                }

                var valuePath = GetValuePath(property);
                if (string.IsNullOrEmpty(valuePath))
                {
                    continue;
                }

                var valueProperty = this.serializedObject.FindProperty(valuePath);
                if (valueProperty == null)
                {
                    continue;
                }

                this.overridePaths.Add(property.propertyPath);
                this.valuePaths.Add(valuePath);
            }
        }

        private static string GetValuePath(SerializedProperty property)
        {
            if (property.name.Length <= OverridePrefix.Length)
            {
                return string.Empty;
            }

            var trimmed = property.name.Substring(OverridePrefix.Length);
            var valueName = char.ToLowerInvariant(trimmed[0]) + trimmed.Substring(1);

            var path = property.propertyPath;
            var lastDot = path.LastIndexOf('.');
            if (lastDot >= 0)
            {
                return path.Substring(0, lastDot + 1) + valueName;
            }

            return valueName;
        }
    }
}
