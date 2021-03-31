using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(PlayerManager))]
public class PlayerSetup : NetworkBehaviour
{
    [SerializeField] Behaviour[] componentsToDisable;
    [SerializeField] string remoteLayerName = "RemotePlayer";
    Camera sceneCamera;

    void Start()
    {
        if (!isLocalPlayer)
        {
            DisableComponents();
            AssignRemoteLayer();
        }
        else
        {
            sceneCamera = Camera.main;
            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }
        }

        GetComponent<PlayerManager>().Setup();
        // RegisterPlayer();   ......This part of script is put in game manager script.
    }

    // void RegisterPlayer()
    // {
    //     string _ID = "Player" + GetComponent<NetworkIdentity>().netId;
    //     transform.name = _ID;
    // }
    public override void OnStartClient()
    {
        base.OnStartClient();

        string _netId = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerManager _player = GetComponent<PlayerManager>();

        GameManager.RegisterPlayer(_netId, _player);
    }

    void AssignRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName);
    }
    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }
    }
    void OnDisable()
    {
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
        GameManager.UnRegisterPlayer(transform.name);
    }
}
