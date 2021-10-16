using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ConversationOption
{
    public string text;
    public Sprite image;
    public AudioClip audio;
    public string targetId;
    public bool enabled;
}
