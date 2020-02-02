using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalvinBeyblade : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.forward, 10000 * Time.deltaTime);
    }
}
