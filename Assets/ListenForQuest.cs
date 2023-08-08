using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListenForQuest : MonoBehaviour
{   
    public NPCDialogue npcDialogue;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            npcDialogue.questToGive.isCompleted = true;
            Destroy(gameObject.transform.parent.gameObject);
        }
    }
}
