using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public int numberOfRounds;
    public int currentRound = 1;
    public List<RoundPhase> roundPhases = new List<RoundPhase>();
    public int currentPhaseIndex = 0;
    public TextMeshProUGUI debugText;

	public List<BotBase> ActiveBots = new List<BotBase>();

    private List<GameObject> beyblades = new List<GameObject>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Game");
        }
    }

    public void StartGame()
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
        roundPhases[currentPhaseIndex].EndPhase();

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

    private Vector3 heightAdjust = new Vector3(0, 30, 0);

    // Factory for beyblade spawning
    public void SpawnBeyblades()
    {
        Vector3 center = transform.position;

        foreach (PlayerController player in ControllerManager.instance.players.Values)
        {
            Vector3 pos = RandomCircle(center, 8.0f) + heightAdjust;
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);
            GameObject bbObj = Instantiate(ResourceLoader.instance.beybladePrefab, pos, rot);
            beyblades.Add(bbObj);
            BotBase beyblade = bbObj.GetComponent<BotBase>();
            beyblade.playerName.text = player.nickname;
            beyblade.beybodyModel.material.color = Color.Lerp(player.playerColor, Color.white, .5f);
            foreach (MeshRenderer beyMesh in beyblade.beybladeModels)
            {
                beyMesh.material.color = player.playerColor;
            }

			ActiveBots.Add(beyblade);
			beyblade.Death += OnBotDeath;

            // read through playercontroller and spawn weapons onto it
        }
    }

	private void OnBotDeath(BotBase botThatDied)
	{
		ActiveBots.Remove(botThatDied);
		if (ActiveBots.Count < 2)
		{
			EndGame();
		}
	}

    public void DespawnBeyblades()
    {
        foreach (GameObject bb in beyblades)
        {
            Destroy(bb);
        }
    }

    private Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        pos.z = center.z;
        return pos;
    }
}