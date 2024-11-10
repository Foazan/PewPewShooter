using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _enemyPrefab;
    [SerializeField]
    private GameObject _asteroidPrefab;
    [SerializeField]
    private GameObject[] _powerups;
    [SerializeField]
    private GameObject _enemyContainer;
    [SerializeField]
    private GameObject _asteroidContainer;
    private bool _stopSpawning = false;
    // Start is called before the first frame update
    void Start()
    {
		StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnAsteroidRoutine());
	}

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator SpawnEnemyRoutine()
    {
        while (_stopSpawning == false)
        {
            GameObject newEnemy = Instantiate(_enemyPrefab, new Vector3(Random.Range(-10f, 10f), 7, 0), Quaternion.identity);
            newEnemy.transform.parent = _enemyContainer.transform;
            yield return new WaitForSeconds(5);
		}
    }

    IEnumerator SpawnPowerupRoutine()
    {
        while (_stopSpawning == false) 
        {
            float randomSeconds = Random.Range(10, 15);
            int randomPowerup = Random.Range(0, 3);
            Instantiate(_powerups[randomPowerup], new Vector3(Random.Range(-10f, 10f), 7, 0), Quaternion.identity);
            yield return new WaitForSeconds(randomSeconds);
        }
    }

    IEnumerator SpawnAsteroidRoutine()
    {
        while (_stopSpawning == false)
        {
            float randomSeconds = Random.Range(10, 15);
            GameObject newAsteroid = Instantiate(_asteroidPrefab, new Vector3(Random.Range(-10f, 10f), 7, 0), Quaternion.identity);
            newAsteroid.transform.parent = _asteroidContainer.transform;
            yield return new WaitForSeconds(randomSeconds);
		}
    }


    public void onPlayerDeath()
    {
        _stopSpawning=true;
    }
}
