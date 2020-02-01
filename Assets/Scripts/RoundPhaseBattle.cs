using NDream.AirConsole;
using UnityEngine;

public class RoundPhaseBattle : RoundPhase
{
    public GameObject battlePhasePlatform;

    public int startingScale;

    public int endingScale;

    private float scaleDeltaPerSecond;

    private float currentScale;

    private void Start()
    {
        currentScale = startingScale;
        float scaleDeltaPerPhase = (startingScale - endingScale) / GameManager.instance.numberOfRounds;
        scaleDeltaPerSecond = scaleDeltaPerPhase / roundTime;
    }

    void Update()
    {
        if (phaseAlive)
        {
            currentRoundTime -= Time.deltaTime;
            currentScale -= scaleDeltaPerSecond * Time.deltaTime;
            battlePhasePlatform.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
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
        // battlePhasePlatform.transform.localScale = new Vector3(10, 10, 10);
    }
}
