using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RoundPhase : MonoBehaviour
{
    public float roundTime;

    protected float currentRoundTime;

    protected bool phaseAlive = false;

    public string phaseName;

    public void StartPhase()
    {
        phaseAlive = true;
        currentRoundTime = roundTime;
        this.BeforePhaseStartPrep();
        this.gameObject.SetActive(true);
    }

    public abstract void BeforePhaseStartPrep();
}