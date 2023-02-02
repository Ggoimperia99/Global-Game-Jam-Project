using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[CreateAssetMenu]
public class DialogueInfo : ScriptableObject
{
    [TextArea]
    public string dialogueText;
    public bool playerSpeaks;
    public int dialogueOrderIndex;
}
