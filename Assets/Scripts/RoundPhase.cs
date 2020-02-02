using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public abstract class RoundPhase : MonoBehaviour
{
    public float roundTime;

    protected float currentRoundTime;

    protected bool phaseAlive = false;

    public string phaseName;

    public TextMeshProUGUI timerText;

    public void StartPhase()
    {
        phaseAlive = true;
        currentRoundTime = roundTime;
        timerText.text = "Time: " + (int)currentRoundTime + "s";
        this.BeforePhaseStartPrep();
        this.gameObject.SetActive(true);
    }

    public abstract void BeforePhaseStartPrep();

    public abstract void EndPhase();
}