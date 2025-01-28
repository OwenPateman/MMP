using UnityEngine;
using Unity.Netcode;

public class PrefabSpawner : NetworkBehaviour
{
    [SerializeField] private GameObject hostPrefab;
    [SerializeField] private GameObject clientPrefab;

    public override void OnNetworkSpawn()
    {
        if (IsHost)
        {
            SpawnHostPrefab();
        }
        else if (IsClient)
        {
            SpawnClientPrefab();
        }
    }

    private void SpawnHostPrefab()
    {
        GameObject hostObject = Instantiate(hostPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        hostObject.GetComponent<NetworkObject>().Spawn(); 
    }

    private void SpawnClientPrefab()
    {
        GameObject clientObject = Instantiate(clientPrefab, GetRandomSpawnPosition(), Quaternion.identity);
        clientObject.GetComponent<NetworkObject>().SpawnWithOwnership(OwnerClientId);
    }

    private Vector2 GetRandomSpawnPosition()
    {
        // Example: Generate a random position within a 2D area
        return new Vector2(Random.Range(-5f, 5f), Random.Range(-3f, 3f));
    }
}
