// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Games.Collections;
using UnityEditor;

namespace Rotorz.Games.Examples.ReorderableList
{
    [CustomEditor(typeof(DemoBehaviour))]
    public class DemoBehaviourEditor : Editor
    {
        private SerializedProperty wishlistProperty;
        private SerializedProperty pointsProperty;


        private void OnEnable()
        {
            this.wishlistProperty = this.serializedObject.FindProperty("wishlist");
            this.pointsProperty = this.serializedObject.FindProperty("points");
        }

        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();

            ReorderableListGUI.Title("Wishlist");
            ReorderableListGUI.ListField(this.wishlistProperty);

            ReorderableListGUI.Title("Points");
            ReorderableListGUI.ListField(this.pointsProperty);

            this.serializedObject.ApplyModifiedProperties();
        }
    }
}
