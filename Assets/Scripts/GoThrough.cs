using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoThrough : MonoBehaviour
{
    bool goThrough = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
        {
            goThrough = true;
        }

        if (Input.GetKeyUp(KeyCode.W))
        {
            goThrough = false;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Door" && goThrough == true)
        {
            DoorScript doorScript = other.GetComponent<DoorScript>();
            Debug.Log("GO THROUGH " + doorScript);
            // Get the DoorScript component from the collided object


            // Teleport the parent of this script (assuming it is attached to the player) to the exitTransform
            transform.position = doorScript.exit.position;
            transform.rotation = doorScript.exit.rotation;
            goThrough = false;
        }
    }

}
