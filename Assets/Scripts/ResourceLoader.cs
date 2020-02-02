using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceLoader : Singleton<ResourceLoader>
{
    public GameObject playerPrefab;
    public GameObject beybladePrefab;
    public GameObject rocketExplosion;

    protected override void Awake()
    {
        base.Awake();

        LoadResources();
    }

    private void LoadResources()
    {
        playerPrefab = Resources.Load<GameObject>("Prefabs/Player");
        beybladePrefab = Resources.Load<GameObject>("Prefabs/BattlePhase/BotBase");
        rocketExplosion = Resources.Load<GameObject>("Prefabs/BattlePhase/Explosion");
    }
}
