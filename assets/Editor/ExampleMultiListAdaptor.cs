// Copyright (c) Rotorz Limited. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root.

using Rotorz.Games.Collections;
using Rotorz.Games.UnityEditorExtensions;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Rotorz.Games.Examples.ReorderableList
{
    public class ExampleMultiListAdaptor : GenericListAdaptor<string>, IReorderableListDropTarget
    {
        private const float MouseDragThresholdInPixels = 0.6f;

        // Static reference to the list adaptor of the selected item.
        private static ExampleMultiListAdaptor s_SelectedList;
        // Static reference limits selection to one item in one list.
        private static string s_SelectedItem;
        // Position in GUI where mouse button was anchored before dragging occurred.
        private static Vector2 s_MouseDownPosition;


        public ExampleMultiListAdaptor(IList<string> list) : base(list, null, 16f)
        {
        }


        public override void DrawItemBackground(Rect position, int index)
        {
            if (this == s_SelectedList && this.List[index] == s_SelectedItem) {
                Color restoreColor = GUI.color;
                GUI.color = ExtraEditorStyles.Instance.Skin.SelectedHighlightColor;
                GUI.DrawTexture(position, EditorGUIUtility.whiteTexture);
                GUI.color = restoreColor;
            }
        }

        public override void DrawItem(Rect position, int index)
        {
            string shoppingItem = this.List[index];

            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            switch (Event.current.GetTypeForControl(controlID)) {
                case EventType.MouseDown:
                    Rect totalItemPosition = ReorderableListGUI.CurrentItemTotalPosition;
                    if (totalItemPosition.Contains(Event.current.mousePosition)) {
                        // Select this list item.
                        s_SelectedList = this;
                        s_SelectedItem = shoppingItem;
                    }

                    // Calculate rectangle of draggable area of the list item.
                    // This example excludes the grab handle at the left.
                    Rect draggableRect = totalItemPosition;
                    draggableRect.x = position.x;
                    draggableRect.width = position.width;

                    if (Event.current.button == 0 && draggableRect.Contains(Event.current.mousePosition)) {
                        // Select this list item.
                        s_SelectedList = this;
                        s_SelectedItem = shoppingItem;

                        // Lock onto this control whilst left mouse button is held so
                        // that we can start a drag-and-drop operation when user drags.
                        GUIUtility.hotControl = controlID;
                        s_MouseDownPosition = Event.current.mousePosition;
                        Event.current.Use();
                    }
                    break;

                case EventType.MouseDrag:
                    if (GUIUtility.hotControl == controlID) {
                        GUIUtility.hotControl = 0;

                        // Begin drag-and-drop operation when the user drags the mouse
                        // pointer across the threshold. This threshold helps to avoid
                        // inadvertently starting a drag-and-drop operation.
                        if (Vector2.Distance(s_MouseDownPosition, Event.current.mousePosition) >= MouseDragThresholdInPixels) {
                            // Prepare data that will represent the item.
                            var item = new DraggedItem(this, index, shoppingItem);

                            // Start drag-and-drop operation with the Unity API.
                            DragAndDrop.PrepareStartDrag();
                            // Need to reset `objectReferences` and `paths` because `PrepareStartDrag`
                            // doesn't seem to reset these (at least, in Unity 4.x).
                            DragAndDrop.objectReferences = new Object[0];
                            DragAndDrop.paths = new string[0];

                            DragAndDrop.SetGenericData(DraggedItem.TypeName, item);
                            DragAndDrop.StartDrag(shoppingItem);
                        }

                        // Use this event so that the host window gets repainted with
                        // each mouse movement.
                        Event.current.Use();
                    }
                    break;

                case EventType.Repaint:
                    EditorStyles.label.Draw(position, shoppingItem, false, false, false, false);
                    break;
            }
        }

        public bool CanDropInsert(int insertionIndex)
        {
            if (!ReorderableListControl.CurrentListPosition.Contains(Event.current.mousePosition)) {
                return false;
            }

            // Drop insertion is possible if the current drag-and-drop operation contains
            // the supported type of custom data.
            return DragAndDrop.GetGenericData(DraggedItem.TypeName) is DraggedItem;
        }

        public void ProcessDropInsertion(int insertionIndex)
        {
            if (Event.current.type == EventType.DragPerform) {
                var draggedItem = DragAndDrop.GetGenericData(DraggedItem.TypeName) as DraggedItem;

                // Are we just reordering within the same list?
                if (draggedItem.SourceListAdaptor == this) {
                    Move(draggedItem.Index, insertionIndex);
                }
                else {
                    // Nope, we are moving the item!
                    this.List.Insert(insertionIndex, draggedItem.ShoppingItem);
                    draggedItem.SourceListAdaptor.Remove(draggedItem.Index);

                    // Ensure that the item remains selected at its new location!
                    s_SelectedList = this;
                }
            }
        }


        // Holds data representing the item that is being dragged.
        private class DraggedItem
        {
            public static readonly string TypeName = typeof(DraggedItem).FullName;


            public readonly ExampleMultiListAdaptor SourceListAdaptor;
            public readonly int Index;
            public readonly string ShoppingItem;


            public DraggedItem(ExampleMultiListAdaptor sourceList, int index, string shoppingItem)
            {
                this.SourceListAdaptor = sourceList;
                this.Index = index;
                this.ShoppingItem = shoppingItem;
            }
        }
    }
}
