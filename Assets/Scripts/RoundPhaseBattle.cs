using NDream.AirConsole;
using UnityEngine;

public class RoundPhaseBattle : RoundPhase
{
    public GameObject battlePhasePlatform;

    void Update()
    {
        if (phaseAlive)
        {
            currentRoundTime -= Time.deltaTime;
            battlePhasePlatform.transform.localScale = new Vector3(currentRoundTime, currentRoundTime, currentRoundTime);
            if (currentRoundTime < 0)
            {
                AirConsole.instance.Message(AirConsole.instance.GetControllerDeviceIds()[0], "view:dead_view");
                GameManager.instance.CurrentPhaseOver();
                phaseAlive = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    public override void BeforePhaseStartPrep()
    {
        battlePhasePlatform.transform.localScale = new Vector3(10, 10, 10);
    }
}
