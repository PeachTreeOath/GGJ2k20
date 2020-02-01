using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CalvinBeyblade : MonoBehaviour
{

    public TextMeshProUGUI playerName;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Vector3.up, 10 * Time.deltaTime);
    }
}
