using Unity.Netcode;
using UnityEngine;

public class PrefabSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject hostPrefab;
    [SerializeField] private GameObject clientPrefab;

    public override void OnNetworkSpawn()
    {
        Debug.Log($"NetworkSpawn called. IsHost: {IsHost}, IsClient: {IsClient}");

        if (IsHost)
        {
            Debug.Log("Spawning host prefab.");
            SpawnHostPrefab();
        }
        else if (IsClient)
        {
            Debug.Log("Requesting server to spawn client prefab.");
            RequestClientPrefabSpawnServerRpc();
        }
    }

    private void SpawnHostPrefab()
    {
        Debug.Log("Host is spawning the hostPrefab.");

        if (hostPrefab == null)
        {
            Debug.LogError("HostPrefab is not assigned in the Inspector.");
            return;
        }

        GameObject hostObject = Instantiate(hostPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        NetworkObject networkObject = hostObject.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.Spawn();
            Debug.Log("HostPrefab spawned successfully.");
        }
        else
        {
            Debug.LogError("HostPrefab does not contain a NetworkObject component.");
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void RequestClientPrefabSpawnServerRpc(ServerRpcParams rpcParams = default)
    {
        ulong requestingClientId = rpcParams.Receive.SenderClientId;
        Debug.Log($"Received client prefab spawn request from client ID: {requestingClientId}");

        SpawnClientPrefab(requestingClientId);
    }

    private void SpawnClientPrefab(ulong clientId)
    {
        Debug.Log($"Spawning clientPrefab for client ID: {clientId}");

        if (clientPrefab == null)
        {
            Debug.LogError("ClientPrefab is not assigned in the Inspector.");
            return;
        }

        GameObject clientObject = Instantiate(clientPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        NetworkObject networkObject = clientObject.GetComponent<NetworkObject>();

        if (networkObject != null)
        {
            networkObject.SpawnWithOwnership(clientId);
            Debug.Log($"ClientPrefab spawned successfully with ownership assigned to client ID: {clientId}");
        }
        else
        {
            Debug.LogError("ClientPrefab does not contain a NetworkObject component.");
        }
    }

    private Vector2 GetRandomSpawnPosition()
    {
        Vector2 position = new Vector2(Random.Range(-5f, 5f), Random.Range(-3f, 3f));
        Debug.Log($"Generated random spawn position: {position}");
        return position;
    }
}
