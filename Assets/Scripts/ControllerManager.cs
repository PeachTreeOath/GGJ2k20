using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;


public class ControllerManager : MonoBehaviour
{
    public GameObject playerPrefab;

    public Dictionary<int, PlayerController> players = new Dictionary<int, PlayerController>();

    private List<Color> playerColorsList = new List<Color>
   {
      new Color(217f/255f, 28f/255f, 28f/255f), // red
      new Color(0f/255f, 246f/255f, 255f/255f), // cyan
      new Color(3f/255f, 154f/255f, 120f/255f), // dark teal
      new Color(255f/255f, 228f/255f, 1f/255f), // sun yellow
      new Color(173f/255f, 36f/255f, 178f/255f), // purple
      new Color(0f/255f, 10f/255f, 17f/255f), // black
      new Color(255f/255f, 151f/255f, 197f/255f), // pink
      new Color(114f/255f, 114f/255f, 114f/255f), // grey
      new Color(83f/255f, 30f/255f, 255f/255f), // royal blue
     
      new Color(113f/255f, 50f/255f, 12f/255f), // brown
      new Color(255f/255f, 174f/255f, 1f/255f), // orange
      new Color(245f/255f, 222f/255f, 179f/255f), // wheat
      new Color(146f/255f, 161f/255f, 255f/255f), // sky blue
      new Color(253f/255f, 74f/255f, 217f/255f), // fuschia pink
      new Color(25f/255f, 255f/255f, 128f/255f), // neon green
      new Color(175f/255f, 20f/255f, 255f/255f), // bright purple
      new Color(181f/255f, 78f/255f, 16f/255f), // brown red
      new Color(231f/255f, 187f/255f, 254f/255f), // lavender
      new Color(184f/255f, 229f/255f, 2f/255f), // sick green
      
      new Color(3f/255f, 239f/255f, 164f/255f), // aqua green
      new Color(253f/255f, 206f/255f, 183f/255f), // pale orange
      new Color(97f/255f, 14f/255f, 175f/255f), // indigo
      new Color(195f/255f, 206f/255f, 197f/255f), // light grey
      new Color(26f/255f, 128f/255f, 255f/255f), // azure blue
      new Color(251f/255f, 99f/255f, 24f/255f), // orange red
      new Color(112f/255f, 182f/255f, 184f/255f), // teal blue
      Color.white, // white
      new Color(255f/255f, 254f/255f, 168f/255f), // pale yellow
      new Color(2f/255f, 171f/255f, 186f/255f), // teal green
      new Color(242f/255f, 132f/255f, 3f/255f), // brighter orange
      new Color(192f/255f, 255f/255f, 141f/255f), // pale green
         new Color(19f/255f, 74f/255f, 117f/255f)    // sea blue
   };

    private int playerSpawnNumber = 0;

    public float spawnHeight = 2f;

    void Awake()
    {
        AirConsole.instance.onMessage += OnMessage;
        AirConsole.instance.onReady += OnReady;
        AirConsole.instance.onConnect += OnConnect;
        AirConsole.instance.onDeviceStateChange += OnDeviceStateChange;
    }

    void OnReady(string code)
    {
        //Since people might be coming to the game from the AirConsole store once the game is live, 
        //I have to check for already connected devices here and cannot rely only on the OnConnect event 
        List<int> connectedDevices = AirConsole.instance.GetControllerDeviceIds();
        foreach (int deviceID in connectedDevices)
        {
            AddNewPlayer(deviceID);
        }
    }

    void OnConnect(int device)
    {
        AddNewPlayer(device);
    }

    private void AddNewPlayer(int deviceID)
    {

        if (players.ContainsKey(deviceID))
        {
            return;
        }

        //Instantiate player prefab, store device id + player script in a dictionary
        GameObject newPlayer = Instantiate(ResourceLoader.instance.playerPrefab, Vector3.zero, Quaternion.identity) as GameObject;

        string nickname = AirConsole.instance.GetNickname(deviceID);
        PlayerController pc = newPlayer.GetComponent<PlayerController>();

        if (nickname != null)
        {
            pc.nickname = nickname;
        }
        pc.nickname = nickname;
        pc.deviceID = deviceID;
        players.Add(deviceID, pc);
        //newPlayer.GetComponent<SpriteRenderer>().color = GetPlayerColor();
        // increases after player joins the level to be used for player colors
        playerSpawnNumber++;
    }

    void OnMessage(int from, JToken data)
    {
        Debug.Log("message: " + data);

        //When I get a message, I check if it's from any of the devices stored in my device Id dictionary
        if (players.ContainsKey(from) && data["action"] != null)
        {
            //I forward the command to the relevant player script, assigned by device ID
            players[from].ButtonInput(data["action"].ToString());
        }
    }
    void OnDeviceStateChange(int device, JToken data)
    {
        if (!players[device].nickname.Equals(AirConsole.instance.GetNickname(device)))
        {
            players[device].nickname = AirConsole.instance.GetNickname(device);
            //players[device].GetComponentInChildren<TextMeshProUGUI>().text = players[device].nickname; calvin bring this back in when we have names over the head
        }
    }

    void OnDestroy()
    {
        if (AirConsole.instance != null)
        {
            AirConsole.instance.onMessage -= OnMessage;
            AirConsole.instance.onReady -= OnReady;
            AirConsole.instance.onConnect -= OnConnect;
            AirConsole.instance.onDeviceStateChange -= OnDeviceStateChange;
        }
    }

    // get a player color from the list of player colors
    private Color GetPlayerColor()
    {
        if (playerSpawnNumber < playerColorsList.Count)
        {
            return playerColorsList[playerSpawnNumber];
        }
        else
        {
            return new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
        }
    }
}

