using NDream.AirConsole;
using UnityEngine;
using TMPro;

public class RoundPhaseBuy : RoundPhase
{
    public GameObject buyGUI;

    void Update()
    {
        if (phaseAlive)
        {
           // Debug.Log("In Buy Phase, current time: " + currentRoundTime);
            currentRoundTime -= Time.deltaTime;
            timerText.text = "Time: " + (int)currentRoundTime + "s";
            if (currentRoundTime < 0)
            {
                // AirConsole.instance.Message(AirConsole.instance.GetControllerDeviceIds()[0], "view:dead_view");
                GameManager.instance.CurrentPhaseOver();
                phaseAlive = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    public override void BeforePhaseStartPrep()
    {
        GameManager.instance.debugText.text = "ROUND " + GameManager.instance.currentRound + " / " + GameManager.instance.numberOfRounds + "\nbuy phase";
        AirConsole.instance.Broadcast("view:shop_view:" + roundTime);
        buyGUI.SetActive(true);
        buyGUI.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
    }

    public override void EndPhase()
    {
        buyGUI.SetActive(false);
    }
}
