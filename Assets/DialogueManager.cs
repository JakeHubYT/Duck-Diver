using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI text;


    public void SetDialogueText(string textToSet)
    {
        text.text = textToSet;  
    }
}
