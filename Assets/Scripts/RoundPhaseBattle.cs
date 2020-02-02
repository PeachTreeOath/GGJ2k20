using NDream.AirConsole;
using UnityEngine;

public class RoundPhaseBattle : RoundPhase
{
    public float timeUntilShrinkStarts;

    public float timeUntilPhaseEndAfterShrink;

    public GameObject battlePhasePlatform;

    public int startingScale;

    public int endingScale;

    public Animator[] wallAnimators;

    private float scaleDeltaPerSecond;

    private float currentScale;

    private void Start()
    {
        // Setup current scale
        currentScale = startingScale;
        battlePhasePlatform.transform.localScale = new Vector3(currentScale, currentScale, currentScale);

        // Determine scaling rate
        float scaleDeltaPerPhase = (startingScale - endingScale) / GameManager.instance.numberOfRounds;
        float shrinkingTime = roundTime - timeUntilShrinkStarts - timeUntilPhaseEndAfterShrink;
        if (shrinkingTime <= 0)
        {
            Debug.LogWarning("SHRINKING TIME IS ZERO OR LESS. Look at timeUntilX fields");
        }
        scaleDeltaPerSecond = scaleDeltaPerPhase / shrinkingTime;
    }

    void Update()
    {
        if (phaseAlive)
        {
            currentRoundTime -= Time.deltaTime;
            if (roundTime - currentRoundTime >= timeUntilShrinkStarts && currentRoundTime > timeUntilPhaseEndAfterShrink)
            {
                currentScale -= scaleDeltaPerSecond * Time.deltaTime;
                battlePhasePlatform.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            }
            if (currentRoundTime < 0)
            {
                //AirConsole.instance.Message(AirConsole.instance.GetControllerDeviceIds()[0], "view:dead_view");
                GameManager.instance.CurrentPhaseOver();
                phaseAlive = false;
                this.gameObject.SetActive(false);
            }
        }
    }

    public override void BeforePhaseStartPrep()
    {
        GameManager.instance.debugText.text = "BATTLE PHASE RD " + GameManager.instance.currentRound;
        GameManager.instance.SpawnBeyblades();
        AirConsole.instance.Broadcast("view:alive_view");

        switch(GameManager.instance.currentRound)
        {
            case 2:
                wallAnimators[0].SetTrigger("Next Round");
                break;
            case 3:
                wallAnimators[1].SetTrigger("Next Round");
                break;
            case 4:
                wallAnimators[2].SetTrigger("Next Round");
                break;
            case 5:
                wallAnimators[3].SetTrigger("Next Round");
                break;
        }
    }

    public override void EndPhase()
    {
        GameManager.instance.DespawnBeyblades();
    }
}
