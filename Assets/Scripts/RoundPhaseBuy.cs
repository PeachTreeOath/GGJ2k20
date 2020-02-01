using NDream.AirConsole;
using UnityEngine;

public class RoundPhaseBuy : RoundPhase
{
    void Update()
    {
        if (phaseAlive)
        {
           // Debug.Log("In Buy Phase, current time: " + currentRoundTime);
            currentRoundTime -= Time.deltaTime;
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
        GameManager.instance.debugText.text = "BUY PHASE";
        AirConsole.instance.Broadcast("view:shop_view");
    }

    public override void EndPhase()
    {
        // do nothing for now i guess
    }
}
