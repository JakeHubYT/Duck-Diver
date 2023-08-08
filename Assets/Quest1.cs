using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest1 : MonoBehaviour
{
    public Transform questLocation;
    public GameObject questItem;

    public void ActivateQuest()
    {
        questLocation.gameObject.SetActive(true);
        questItem.SetActive(true);
    }
}
