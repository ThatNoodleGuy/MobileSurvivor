using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropRandomizer : MonoBehaviour
{
    [SerializeField] private List<GameObject> propsSpawnPoints;
    [SerializeField] private List<GameObject> propsPrefabs;

    private void Start()
    {
        SpawnProps();
    }

    private void Update()
    {
        
    }

    private void SpawnProps()
    {
        foreach (GameObject spawnPoint in propsSpawnPoints)
        {
            int random = Random.Range(0, propsPrefabs.Count);
            GameObject prop = Instantiate(propsPrefabs[random], spawnPoint.transform.position, Quaternion.identity);
            prop.transform.parent = spawnPoint.transform;
        }
    }
}