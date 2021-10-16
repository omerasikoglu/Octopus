using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ConversationPiece 
{
    public string id;
    [Multiline] public string text;
    public AudioClip audio;
    public List<ConversationOption> options;

    // public Quest quest;
}
