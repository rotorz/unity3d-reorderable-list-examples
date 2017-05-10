// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Games.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rotorz.Games.Examples.ReorderableList
{
    public class ReorderableListDemo : EditorWindow
    {
        [MenuItem("Window/List Demo (C#)")]
        static void ShowWindow()
        {
            GetWindow<ReorderableListDemo>("List Demo");
        }


        public List<string> shoppingList;
        public List<string> purchaseList;

        private Vector2 scrollPosition;


        private void OnEnable()
        {
            this.shoppingList = new List<string>();
            this.shoppingList.Add("Bread");
            this.shoppingList.Add("Carrots");
            this.shoppingList.Add("Beans");
            this.shoppingList.Add("Steak");
            this.shoppingList.Add("Coffee");
            this.shoppingList.Add("Fries");

            this.purchaseList = new List<string>();
            this.purchaseList.Add("Cheese");
            this.purchaseList.Add("Crackers");
        }

        private void OnGUI()
        {
            this.scrollPosition = GUILayout.BeginScrollView(this.scrollPosition);

            ReorderableListGUI.Title("Shopping List");
            ReorderableListGUI.ListField(this.shoppingList, PendingItemDrawer, DrawEmpty);

            ReorderableListGUI.Title("Purchased Items");
            ReorderableListGUI.ListField(this.purchaseList, PurchasedItemDrawer, DrawEmpty, ReorderableListFlags.HideAddButton | ReorderableListFlags.DisableReordering);

            GUILayout.EndScrollView();
        }


        private string PendingItemDrawer(Rect position, string itemValue)
        {
            // Text fields do not like null values!
            if (itemValue == null) {
                itemValue = "";
            }

            position.width -= 50;
            itemValue = EditorGUI.TextField(position, itemValue);

            position.x = position.xMax + 5;
            position.width = 45;
            if (GUI.Button(position, "Info")) {
            }

            return itemValue;
        }

        private string PurchasedItemDrawer(Rect position, string itemValue)
        {
            position.width -= 50;
            GUI.Label(position, itemValue);

            position.x = position.xMax + 5;
            position.width = 45;
            if (GUI.Button(position, "Info")) {
            }

            return itemValue;
        }

        private void DrawEmpty()
        {
            GUILayout.Label("No items in list.", EditorStyles.miniLabel);
        }
    }
}
