// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Games.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rotorz.Games.Examples.ReorderableList
{
    public class MultiListEditorWindowDemo : EditorWindow
    {
        [MenuItem("Window/Multi List Demo (C#)")]
        private static void ShowWindow()
        {
            GetWindow<MultiListEditorWindowDemo>("Multi List");
        }


        private List<string> shoppingList;
        private ExampleMultiListAdaptor shoppingListAdaptor;

        private List<string> purchaseList;
        private ExampleMultiListAdaptor purchaseListAdaptor;


        private void OnEnable()
        {
            this.shoppingList = new List<string>() { "Bread", "Carrots", "Beans", "Steak", "Coffee", "Fries" };
            this.shoppingListAdaptor = new ExampleMultiListAdaptor(this.shoppingList);

            this.purchaseList = new List<string>() { "Cheese", "Crackers" };
            this.purchaseListAdaptor = new ExampleMultiListAdaptor(this.purchaseList);
        }

        private void OnGUI()
        {
            GUILayout.BeginHorizontal();

            var columnWidth = GUILayout.Width(position.width / 2f - 6);

            // Draw list control on left side of the window.
            GUILayout.BeginVertical(columnWidth);
            ReorderableListGUI.Title("Shopping List");
            ReorderableListGUI.ListField(this.shoppingListAdaptor);
            GUILayout.EndVertical();

            // Draw list control on right side of the window.
            GUILayout.BeginVertical(columnWidth);
            ReorderableListGUI.Title("Purchase List");
            ReorderableListGUI.ListField(this.purchaseListAdaptor);
            GUILayout.EndVertical();

            GUILayout.EndHorizontal();
        }
    }
}
