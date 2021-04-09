using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;
public class PauseMenu : MonoBehaviour
{
    public static bool IsOn = false;
    private NetworkManager networkManager;
    void Start()
    {
        networkManager = NetworkManager.singleton;
    }
    public void LeaveRoom()
    {
        Debug.Log("Dissconnected ..");
        MatchInfo matchInfo = networkManager.matchInfo;
        networkManager.matchMaker.DropConnection(matchInfo.networkId, matchInfo.nodeId, 0, networkManager.OnDropConnection);
        networkManager.StopHost();
    }

}
