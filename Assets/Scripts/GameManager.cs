using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
    public int numberOfRounds;
    public int currentRound = 1;
    public List<RoundPhase> roundPhases = new List<RoundPhase>();
    public int currentPhaseIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (roundPhases.Count <= 0)
        {
            Debug.Log("NO ROUND PHASES SELECTED.");
        }
        else
        {
            Debug.Log("STRT GAME. ROUND: " + currentRound + " PHASE: " + currentPhaseIndex);
            roundPhases[currentPhaseIndex].StartPhase();
        }
    }

    public void CurrentPhaseOver()
    {
        currentPhaseIndex++;
        if (currentPhaseIndex >= roundPhases.Count)
        {
            // Next round
            currentRound++;
            if (currentRound > numberOfRounds)
            {
                this.EndGame();
            }
            else
            {
                currentPhaseIndex = 0;
            }
        }
        if (currentRound <= numberOfRounds && currentPhaseIndex < roundPhases.Count)
        {
            Debug.Log("ROUND: " + currentRound + " PHASE: " + currentPhaseIndex);
            roundPhases[currentPhaseIndex].StartPhase();
        }
    }

    public void EndGame()
    {
        Debug.Log("END GAME");
    }
}