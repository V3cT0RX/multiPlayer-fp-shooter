using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public MatchSettings matchSettings;
    [SerializeField] private GameObject sceneCamera;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("More than One Game Manager in Scene");
        }
        else
        {
            instance = this;
        }
    }

    public void SetSceneCameraActive(bool isActive)
    {
        if (sceneCamera != null)
            return;

        sceneCamera.gameObject.SetActive(true);
    }

    #region PLayer Trackking
    private const string PLAYER_ID_PREFIX = "Player ";
    private static Dictionary<string, PlayerManager> players = new Dictionary<string, PlayerManager>();
    public static void RegisterPlayer(string _netId, PlayerManager _player)
    {
        string _playerID = PLAYER_ID_PREFIX + _netId;
        players.Add(_playerID, _player);
        _player.transform.name = _playerID;

    }

    public static void UnRegisterPlayer(string _playerID)
    {
        players.Remove(_playerID);
    }

    public static PlayerManager GetPlayer(string _playerID)
    {
        return players[_playerID];
    }
    // void OnGUI()
    // {
    //     GUILayout.BeginArea(new Rect(200, 200, 200, 500));
    //     GUILayout.BeginVertical();

    //     foreach (string _playerID in players.Keys)
    //     {
    //         GUILayout.Label(_playerID + " " + players[_playerID].transform.name);
    //     }
    //     GUILayout.EndVertical();
    //     GUILayout.EndArea();
    // }
    #endregion


}
