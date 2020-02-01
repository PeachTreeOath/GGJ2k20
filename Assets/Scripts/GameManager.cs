using NDream.AirConsole;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

    private GameObject testObj;
    private float timeLeft = 10f;
    private bool isDone;

    // Start is called before the first frame update
    void Start()
    {
        testObj = GameObject.Find("Sphere");
    }

    // Update is called once per frame
    void Update()
    {
        timeLeft -= Time.deltaTime;
        testObj.transform.localScale = new Vector3(timeLeft, timeLeft, timeLeft);
        if (timeLeft < 0 && !isDone)
        {
            isDone = true;
            AirConsole.instance.Message(AirConsole.instance.GetControllerDeviceIds()[0], "view:dead_view");
        }
    }
}
