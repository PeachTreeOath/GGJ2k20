using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using NDream.AirConsole;

public class GameManager : Singleton<GameManager>
{
    public int numberOfRounds;
    public int currentRound = 1;
    public List<RoundPhase> roundPhases = new List<RoundPhase>();
    public int currentPhaseIndex = 0;
    public TextMeshProUGUI debugText;

    public List<BotBase> ActiveBots = new List<BotBase>();

    public List<GameObject> beyblades = new List<GameObject>();

    public Canvas victoryCanvas;
    public TextMeshProUGUI winnersText;

    public GameObject spawnLocation;

    private bool endedGame = false; // uh poor game loop management, sometimes ends twice :X

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
            roundPhases[currentPhaseIndex].StartPhase();
        }
    }

    public void CurrentPhaseOver()
    {
        roundPhases[currentPhaseIndex].EndPhase();

        FindObjectOfType<DynamicCamera>().ReleaseCamera();

        currentPhaseIndex++;
        if (currentPhaseIndex >= roundPhases.Count)
        {
            // Next round
            currentRound++;
            if (currentRound > numberOfRounds || ActiveBots.Count < 2)
            {
                EndGame();
            }
            else
            {
                currentPhaseIndex = 0;
            }
        }
        if (currentRound <= numberOfRounds && currentPhaseIndex < roundPhases.Count)
        {
            roundPhases[currentPhaseIndex].StartPhase();
        }
    }

    public void EndGame()
    {
        if (!endedGame)
        {
            endedGame = true;
            victoryCanvas.gameObject.SetActive(true);
            string text = "";
            List<GameObject> beybladesNoNull = beyblades.Where(item => item != null).ToList();
            if (beybladesNoNull.Count == 1)
            {
                text = "Winner:\n";
            }
            else if (beybladesNoNull.Count > 1)
            {
                text = "Winners:\n";
            }
            else
            {
                text = "No Winners :(";
            }
            foreach (GameObject beyblade in beybladesNoNull)
            {
                //AirConsole.instance.Message(0, "view:victory_view"); // todo only send to the player that won
                BotBase botBase = beyblade.GetComponent<BotBase>();
                text += botBase.playerName.text + "\n";
            }
            winnersText.text = text;
        }
    }

    private Vector3 heightAdjust = new Vector3(0, 30, 0);

    // Factory for beyblade spawning
    public void SpawnBeyblades()
    {
        Vector3 center = transform.position;

        foreach (PlayerController player in ControllerManager.instance.players.Values)
        {
            Vector3 pos = RandomCircle(center, 6.0f) + heightAdjust;
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);

            Vector3 spawnPoint = spawnLocation.transform.position;
            Vector3 spawnBounds = spawnLocation.transform.localScale;
            Vector3 randomPosition = new Vector3(spawnPoint.x + Random.value * spawnBounds.x, 0, spawnPoint.z + Random.value * spawnBounds.z);
            randomPosition = randomPosition - spawnBounds / 2;

            GameObject bbObj = Instantiate(ResourceLoader.instance.beybladePrefab, pos, rot);
            beyblades.Add(bbObj);
            BotBase beyblade = bbObj.GetComponent<BotBase>();
            beyblade.playerName.text = player.nickname;
            beyblade.beybodyModel.material.color = Color.Lerp(player.playerColor, Color.white, .5f);
            player.Bot = beyblade;
            foreach (MeshRenderer beyMesh in beyblade.beybladeModels)
            {
                beyMesh.material.color = player.playerColor;
            }

            ActiveBots.Add(beyblade);
            beyblade.Death += OnBotDeath;

            ArenaLightController.instance.UpdateListofTargets();
            // read through playercontroller and spawn weapons onto it
        }
    }

    public void ActivateBeyblades()
    {
        Vector3 center = transform.position;

        foreach (BotBase bot in ActiveBots)
        {
            Vector3 pos = RandomCircle(center, 6.0f) + heightAdjust;
            Quaternion rot = Quaternion.FromToRotation(Vector3.forward, center - pos);

            Vector3 spawnPoint = spawnLocation.transform.position;
            Vector3 spawnBounds = spawnLocation.transform.localScale;
            Vector3 randomPosition = new Vector3(spawnPoint.x + Random.value * spawnBounds.x, 0, spawnPoint.z + Random.value * spawnBounds.z);
            randomPosition -= spawnBounds / 2;

            bot.gameObject.SetActive(true);
            bot.transform.position = pos;
        }

        ArenaLightController.instance.UpdateListofTargets();
    }

    private void OnBotDeath(BotBase botThatDied)
    {
        ActiveBots.Remove(botThatDied);
        ArenaLightController.instance.UpdateListofTargets();
        if (ActiveBots.Count < 2)
        {
            EndGame();
        }
    }

    public void DespawnBeyblade(GameObject bb)
    {
        beyblades.Remove(bb);
        HideGameObject(bb);

        BotBase beyblade = bb.GetComponent<BotBase>();
        if (ActiveBots.Contains(beyblade))
        {
            ActiveBots.Remove(beyblade);
            beyblade.Death -= OnBotDeath;
            beyblade.Death?.Invoke(beyblade);
        }

        if (currentRound == numberOfRounds)
        {
            //AirConsole.instance.Message(1, "view:dead_view"); // todo only send to the player that died
            if (beyblades.Count <= 1)
            {
                EndGame();
            }
        }
    }

    public void HideGameObject(GameObject bb)
    {
        bb.SetActive(false);
    }

    public void HideBeyblades()
    {
        foreach (GameObject bb in beyblades)
        {
            HideGameObject(bb);
        }
        //beyblades = new List<GameObject>();
    }

    private Vector3 RandomCircle(Vector3 center, float radius)
    {
        float ang = Random.value * 360;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}
