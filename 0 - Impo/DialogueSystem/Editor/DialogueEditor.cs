using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

public class DialogueEditor : EditorWindow
{
    Dialogue selectedDialogue = null;
    [NonSerialized] GUIStyle nodeStyle;
    [NonSerialized] DialogueNode draggingNode = null;
    [NonSerialized] Vector2 draggingOffset;
    [NonSerialized] DialogueNode creatingNode = null;

    [MenuItem("Window/Dialogue Editor")] //Window tab'ine ekleme
    public static void ShowEditorWindow()
    {
        GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
    }

    [OnOpenAsset(1)] //ScriptableObject'e çift týklayýnca diyalog penceresinin açýlmasý
    public static bool OnOpenAsset(int instanceID, int line)
    {
        Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
        if (dialogue != null)
        {
            ShowEditorWindow();
            return true;
        }

        return false;
    }

    private void OnEnable()
    {
        Selection.selectionChanged += OnSelectionChanged;

        nodeStyle = new GUIStyle();
        nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
        nodeStyle.normal.textColor = Color.white;
        nodeStyle.padding = new RectOffset(20, 20, 12, 6);
        nodeStyle.border = new RectOffset(12, 12, 12, 12);
    }

    private void OnSelectionChanged()
    {
        Dialogue newDialogue = Selection.activeObject as Dialogue;
        if (newDialogue != null)
        {
            selectedDialogue = newDialogue;
            Repaint(); //OnGUI'yi otomatik çalýþtýrýr
        }
    }

    private void OnGUI()
    {
        if (selectedDialogue == null)
        {
            EditorGUILayout.LabelField("Diyalog secili degil");
        }
        else
        {
            ProcessEvents();
            //connection'lar rect'lerin üstünden geçmesin diye 2 foreach döngüsü yaptýk
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawConnections(node);
            }
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                DrawNode(node);
            }
            if (creatingNode != null)
            {
                Undo.RecordObject(selectedDialogue, "Added Dialogue Node");
                selectedDialogue.CreateNode(creatingNode);
                creatingNode = null;
            }
        }

    }

    private void DrawConnections(DialogueNode node)
    {
        Vector3 startPosition = new Vector2(node.rect.xMax, node.rect.center.y);

        foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
        {
            Vector3 endPosition = new Vector2(childNode.rect.xMin, childNode.rect.center.y);
            Vector3 controlPointOffset = endPosition - startPosition;
            controlPointOffset.y = 0;
            controlPointOffset.x *= .8f;
            Handles.DrawBezier(startPosition, endPosition,
                startPosition + controlPointOffset, endPosition - controlPointOffset, Color.red, null, 4f);
        }
    }

    private void ProcessEvents()
    {
        if (Event.current.type == EventType.MouseDown && draggingNode == null)
        {
            draggingNode = GetNodeAtPoint(Event.current.mousePosition);
            if (draggingOffset != null)
            {
                draggingOffset = draggingNode.rect.position - Event.current.mousePosition;
            }
        }
        else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
        {
            Undo.RecordObject(selectedDialogue, "Move Dialogue Node");
            draggingNode.rect.position = Event.current.mousePosition + draggingOffset;
            GUI.changed = true;
        }
        else if (Event.current.type == EventType.MouseUp && draggingNode != null)
        {

            draggingNode = null;
            //selectedDialogue.GetRootNode().rect.position = Event.current.mousePosition;
        }
    }
    private void DrawNode(DialogueNode node)
    {
        GUILayout.BeginArea(node.rect, nodeStyle);
        EditorGUI.BeginChangeCheck();   //GUI deðiþikliklerini check eder


        string newText = EditorGUILayout.TextField(node.text);

        if (EditorGUI.EndChangeCheck())       // newText != node.text 'le ayný þey
        {
            Undo.RecordObject(selectedDialogue, "Update Dialogue Text"); //geri alma fonksiyonu

            node.text = newText;
        }
        //foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
        //{
        //    EditorGUILayout.LabelField(childNode.text);
        //}

        if (GUILayout.Button("Add"))
        {
            creatingNode = node;
            selectedDialogue.CreateNode(node);
        }
        GUILayout.EndArea();
    }
    private DialogueNode GetNodeAtPoint(Vector2 point)
    {
        DialogueNode foundNode = null;
        foreach (DialogueNode node in selectedDialogue.GetAllNodes())
        {
            if (node.rect.Contains(point))
            {
                foundNode = node;
            }
        }
        return foundNode;
    }
}
