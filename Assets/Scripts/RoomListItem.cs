using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking.Match;

public class RoomListItem : MonoBehaviour
{
    public delegate void JoinRoomDelegate(MatchInfoSnapshot _match);
    public JoinRoomDelegate joinRoomCallback;
    [SerializeField] private Text roomNameText;
    public MatchInfoSnapshot match;

    public void MatchSetup(MatchInfoSnapshot _match, JoinRoomDelegate _joinRoomCallbck)
    {
        match = _match;
        joinRoomCallback = _joinRoomCallbck;
        roomNameText.text = match.name + "(" + match.currentSize + "/" + match.maxSize + ")";

    }

    public void JoinRoom()
    {
        joinRoomCallback.Invoke(match);
    }
}
