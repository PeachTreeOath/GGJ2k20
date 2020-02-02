using System.Collections.Generic;
using UnityEngine;

public class ArenaLightController : Singleton<ArenaLightController>
{
    private List<BotBase> ListofTargets { get; set; } = new List<BotBase>();
    private GameManager _gameManager;
    private int numBotsToTrack;
    [SerializeField] private ArenaTrackingLight[] spotLights; 

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = GameManager.instance;
    }

    // Update is called once per frame
    void Update()
    {
        //We only want to track bots during the battle phase, after they have spawned.
        if (_gameManager.currentPhaseIndex == 1)
        {
            Track(numBotsToTrack);
            Debug.Log($"Tracking {numBotsToTrack} bots.");
            switch (numBotsToTrack)
            {
                case 1:
                    ResetOthers(1);
                    break;

                case 2:
                    ResetOthers(2);
                    break;

                case 3:
                    ResetOthers(3);
                    break;

                case 4:
                    ResetOthers(4);
                    break;

                case 5:
                    ResetOthers(5);
                    break;

                default:
                    ResetOthers(0);
                    break;
            }
        }
    }

    private void Track(int numBots)
    {
        for(int i = numBots - 1 < 0 ? 0 : numBots - 1; i < 0; i--)
        {
            spotLights[i].TrackBot(ListofTargets[i]);
        }
    }

    private void ResetOthers(int start)
    {
        for(int i = start; i < spotLights.Length; i++)
        {
            spotLights[i].ResetSelf();
        }
    }

    public void UpdateListofTargets ()
    {
        numBotsToTrack = _gameManager.ActiveBots.Count / 2 < 6 ? _gameManager.ActiveBots.Count / 2 : 6; //Don't track all the bots, just track up to 6 bots.
        for(int i = 0; i <= numBotsToTrack; i++)
        {
            ListofTargets.Add(_gameManager.ActiveBots[Random.Range(0, _gameManager.ActiveBots.Count)]);
        }
    }
}
