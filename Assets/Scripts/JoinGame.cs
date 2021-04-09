using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour
{
    List<GameObject> roomList = new List<GameObject>();
    [SerializeField] private Text status;
    [SerializeField] private GameObject roomlistItemPrefab;
    [SerializeField] private Transform roomListParent;
    private NetworkManager networkManager;

    void Start()
    {
        networkManager = NetworkManager.singleton;
        if (networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        ClearRoomList();
        networkManager.matchMaker.ListMatches(0, 20, "", true, 0, 0, OnMatchList);
        status.text = "Loading...";
    }

    // public void OnMatchList(ListMatchResponse matchList)
    public void OnMatchList(bool success, string extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        status.text = "";
        if (/*!success || */ matchList == null)
        {
            status.text = "Couldn't get matches";
            return;
        }

        foreach (MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGo = Instantiate(roomlistItemPrefab);
            _roomListItemGo.transform.SetParent(roomListParent);

            // get comp roomlistitem    ... work remaining
            RoomListItem _roomListItem = _roomListItemGo.GetComponent<RoomListItem>();
            if (_roomListItem != null)
            {
                _roomListItem.MatchSetup(match, JoinRoom);
            }

            roomList.Add(_roomListItemGo);
        }

        if (roomList.Count == 0)
        {
            status.text = "No Rooms available.";
        }
    }

    private void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);
        }
        roomList.Clear();
    }

    public void JoinRoom(MatchInfoSnapshot _match)
    {
        Debug.Log("Joining " + _match.name);
        networkManager.matchMaker.JoinMatch(_match.networkId, "", "", "", 0, 0, networkManager.OnMatchJoined);
        ClearRoomList();
        status.text = "JOINING " + _match.name + " ...";
    }
}
