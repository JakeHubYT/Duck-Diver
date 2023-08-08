using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public Transform questWayPoint;



    private void Start()
    {
        questWayPoint.gameObject.SetActive(false);
    }

    public void GiveQuest()
    {
        questWayPoint.gameObject.SetActive(true);
    }
}
