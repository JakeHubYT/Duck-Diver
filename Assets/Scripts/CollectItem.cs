using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CollectItem : MonoBehaviour
{
    public UnityEvent onCollectBreadEvent;

  

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Collectable")
        {
            print("ss");
            onCollectBreadEvent.Invoke();
            Destroy(other.gameObject);
        }
    }
}
