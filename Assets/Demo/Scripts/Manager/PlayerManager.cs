using System;
using Photon.Pun;
using Tools;
using UnityEngine;

public class PlayerManager : Singleton<PlayerManager>
{
    [SerializeField] 
    private GameObject playerPrefab;

    public static GameObject LocalPlayerInstance = null;
    
    private void Start()
    {
        Vector2 position = new Vector2(0, 0);
        try
        {
            SpawnPlayerAt(position);
        }
        catch (NullReferenceException exception)
        {
            Debug.LogError(exception.Message);
        }
        catch (Exception exception)
        {
            Debug.LogError(exception.Message);
        }
    }

    public void SpawnPlayerAt(Vector2 pos)
    {
        GameObject instance = PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        PhotonView photonView = instance.GetComponent<PhotonView>();
        if (photonView == null) throw new NullReferenceException("failed to get PhotonView component from player prefab");

        if (photonView.IsMine)
        {
            LocalPlayerInstance = instance;
        }
    }
}
