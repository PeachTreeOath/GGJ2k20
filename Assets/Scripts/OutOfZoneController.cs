using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutOfZoneController : MonoBehaviour
{
    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GameManager.instance.DespawnBeyblade(other.gameObject);
        }
        else
        {
            // Get rid of all other (should be just projectiles)
            Destroy(other.gameObject);
        }
    }
}
