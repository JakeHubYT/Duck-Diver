using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NPCDialogue : MonoBehaviour
{
    [SerializeField] private List<string> dialog = new List<string>();
    [SerializeField] private List<string> winDialogue = new List<string>();

    List<string> currentDialog;

    [SerializeField] private float interactionRange = 2.0f;
    [SerializeField] private GameObject npcInteractUI;

    [SerializeField] private LayerMask playerLayerMask;
  //  public GameObject dialogueBox;

    [SerializeField] private int currentDialogueListIndex = 0;
    [SerializeField] private bool isInDialogue = false;

    public TextMeshProUGUI text;
    private Transform player;


    [SerializeField] public Quest questToGive;
    public bool hasCollectedItem = false;

    private void Start()
    {
        InitializeReferences();
    }


    public void SetDialogueText(string textToSet)
    {
        text.text = textToSet;
    }

    private void Update()
    {
        HandlePlayerInteraction();
        HandleDialogueInput();
    }

    private void InitializeReferences()
    {
        currentDialog = dialog;
        player = GameObject.FindGameObjectWithTag("Player").transform;
       // dialogueBox = GameObject.FindGameObjectWithTag("dialogueBox");
      //  dialogueBox.SetActive(false);
     //   Debug.Log(dialogueBox);
      //  SetDialogueText(currentDialog[currentDialogueListIndex]);
        npcInteractUI.SetActive(false);

        Instantiate(npcInteractUI, transform.position + new Vector3(0, 3, 0), Quaternion.identity);
    }

    private void HandlePlayerInteraction()
    {
        bool isPlayerInRange = Physics.CheckSphere(transform.position, interactionRange, playerLayerMask);
        npcInteractUI.SetActive(isPlayerInRange);

        if(isPlayerInRange) { Debug.Log(isPlayerInRange + "  = in range"); }

        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            npcInteractUI.SetActive(false);

            EnterDialogue();
            
        }
    }
    private void CheckForCollectedItem()
    {
        if (!hasCollectedItem)
        {
            // Assuming you have a way to track collected items (e.g., using tags)
            Collider[] colliders = Physics.OverlapSphere(player.position, interactionRange, playerLayerMask);

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.CompareTag("CollectibleItem"))
                {
                    hasCollectedItem = true;
                    // Remove the collected item from the scene if needed
                    Destroy(collider.gameObject);
                    break;
                }
            }
        }

        if (hasCollectedItem)
        {
            EnterDialogue();
        }
        else
        {
            // Display dialogue about needing to collect the item first
            SetDialogueText("You need to collect the item before returning to me.");
        }
    }

    private void HandleDialogueInput()
    {
        if (!isInDialogue) return;

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            HandleMouse1Input();
        }
    }

    private void HandleMouse1Input()
    {
        HandleDialog(); 
    }

    private void HandleDialog()
    {
        currentDialogueListIndex++; // Increment the index

        if (currentDialogueListIndex < currentDialog.Count) // Check if index is within range
        {
            SetDialogueText(currentDialog[currentDialogueListIndex]);
        }
        else
        {
            ExitDialogue();
        }
    }

    void SwitchDialogueList()
    {
        currentDialogueListIndex = 0;
        currentDialog = winDialogue;
    }

    private void EnterDialogue()
    {
        currentDialogueListIndex = 0;
        if (questToGive.isCompleted)
        {
            SwitchDialogueList();
        }

        SetDialogueText(currentDialog[currentDialogueListIndex]);
        //dialogueBox.SetActive(true);
        isInDialogue = true;

      


    }

    private void ExitDialogue()
    {
       
        isInDialogue = false;
        //dialogueBox.SetActive(false);
        currentDialogueListIndex = 0;



        if (questToGive != null && !questToGive.isCompleted)
        {
            if (questToGive.isCollectQuest && hasCollectedItem)
            {
                CompleteCollectQuest();
            }
            else
            {
                GiveQuestToPlayer();
            }
        }

  
    }

    private void CompleteCollectQuest()
    {
     
    }


    private void GiveQuestToPlayer()
    {
      
        questToGive.itemToCollect.SetActive(true);

        if (questToGive.isCollectQuest)
        {
            hasCollectedItem = false;
        }

        Debug.Log("Quest given to player: " + questToGive.questName);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}

[System.Serializable]
public class Quest
{
    public string questName;
    public string questDescription;
    public bool isCompleted;
    public bool isCollectQuest;
    public GameObject itemToCollect; 
}
