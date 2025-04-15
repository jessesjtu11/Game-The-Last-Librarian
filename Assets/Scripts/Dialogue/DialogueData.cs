using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Dialogue/DialogueData",order=0)]
public class DialogueData : ScriptableObject
{
    [System.Serializable]
    public class DialogueSegment
    {
        public Sprite backgroundImage;
        [TextArea(3, 5)] public string dialogueText;
        public float textSpeed = 0.05f;
    }

    public List<DialogueSegment> dialogueSegments;
}