﻿using System.Collections.Generic;
using NDream.AirConsole;
using UnityEngine;
using TMPro;

public class RoundPhaseBattle : RoundPhase
{
    public float timeUntilShrinkStarts;

    public float timeUntilPhaseEndAfterShrink;

    // public GameObject battlePhasePlatform;

    public int startingScale;

    public int endingScale;

    public Animator[] wallAnimators;

    private float scaleDeltaPerSecond;

    private float currentScale;
    public TextMeshProUGUI playersLeft;

    private void Start()
    {
        // Setup current scale
        currentScale = startingScale;
        // battlePhasePlatform.transform.localScale = new Vector3(currentScale, currentScale, currentScale);

        // Determine scaling rate
        float scaleDeltaPerPhase = (startingScale - endingScale) / GameManager.instance.numberOfRounds;
        float shrinkingTime = roundTime - timeUntilShrinkStarts - timeUntilPhaseEndAfterShrink;
        if (shrinkingTime <= 0)
        {
            Debug.LogWarning("SHRINKING TIME IS ZERO OR LESS. Look at timeUntilX fields");
        }
        scaleDeltaPerSecond = scaleDeltaPerPhase / shrinkingTime;

        AudioManager.instance.PlayMusicWithIntro("RepairRoyale-intro", "RepairRoyale-loop");
    }

    void Update()
    {
        if (phaseAlive && GameManager.instance.currentRound <= GameManager.instance.numberOfRounds)
        {
            currentRoundTime -= Time.deltaTime;
            timerText.text = "Time: " + (int) currentRoundTime + "s";
            playersLeft.text = "Players: " + GameManager.instance.beyblades.Count;
            if (roundTime - currentRoundTime >= timeUntilShrinkStarts && currentRoundTime > timeUntilPhaseEndAfterShrink)
            {
                currentScale -= scaleDeltaPerSecond * Time.deltaTime;
                // battlePhasePlatform.transform.localScale = new Vector3(currentScale, currentScale, currentScale);
            }
            if (currentRoundTime < 0)
            {
                //AirConsole.instance.Message(AirConsole.instance.GetControllerDeviceIds()[0], "view:dead_view");
                GameManager.instance.CurrentPhaseOver();
                phaseAlive = false;
                this.gameObject.SetActive(false);
            }
        }
        else
        {
            List<BotBase> bots = new List<BotBase>();
            foreach(BotBase bot in FindObjectsOfType<BotBase>())
            {
                if(!bot.IsDead) bots.Add(bot);
            }
            if(bots.Count < 2)
            {
                //Send a message to the player's phone indicating that he/she won the game, then either return to the main menu, reload the scene, or quit.
                //TODO need a better way of getting the player object.
                foreach(PlayerController player in FindObjectsOfType<PlayerController>())
                {
                    if (player.nickname.Equals(bots[0].playerName)) AirConsole.instance.Message(player.deviceID, "view:victory_view");
                    //TODO action after the victory message.
                }
            }
        }
    }

    public override void BeforePhaseStartPrep()
    {
        playersLeft.gameObject.SetActive(true);
        GameManager.instance.debugText.text = "ROUND " + GameManager.instance.currentRound + " / " + GameManager.instance.numberOfRounds + "\nbattle phase";
        AirConsole.instance.Broadcast("view:alive_view");

        switch (GameManager.instance.currentRound)
        {
            case 1:
                GameManager.instance.SpawnBeyblades();
                break;
            case 2:
                GameManager.instance.ActivateBeyblades();
                wallAnimators[0].SetTrigger("Next Round");
                break;
            case 3:
                GameManager.instance.ActivateBeyblades();
                wallAnimators[1].SetTrigger("Next Round");
                break;
            case 4:
                GameManager.instance.ActivateBeyblades();
                wallAnimators[2].SetTrigger("Next Round");
                break;
            case 5:
                GameManager.instance.ActivateBeyblades();
                wallAnimators[3].SetTrigger("Next Round");
                break;
        }
    }

    public override void EndPhase()
    {
        if (GameManager.instance.currentRound != GameManager.instance.numberOfRounds)
        {
            GameManager.instance.HideBeyblades();
            playersLeft.gameObject.SetActive(false);
        }
    }
}
