using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject _rockContainer, _grassContainer, _enemyContainer;
    [SerializeField]
    private GameObject _rockPrefab, _lootChestPrefab, _grassPrefab;
    [SerializeField]
    private GameObject[] enemies; //<- assign enemy types in the inspector

    [SerializeField]
    private GameObject _movingEnemy;

    private CreateGrid _createGrid;

    public static int _rockID = 0; 

    private void Start()
    {
        _createGrid = GameObject.Find("Grid").GetComponent<CreateGrid>();
        if(_createGrid == null)
        {
            Debug.Log("Grid is NULL.");
        }
        GameManager.onLevelChange += DePopulateGrid;
    }

    private void DePopulateGrid()
    {
        StartCoroutine(DestroyChildren());
    }

    private void OnDisable()
    {
        GameManager.onLevelChange -= DePopulateGrid;
    }

    private IEnumerator DestroyChildren()
    {
        while (_rockContainer.transform.childCount > 0)
        {
            Debug.Log(_rockContainer.transform.GetChild(0));
            Destroy(_rockContainer.transform.GetChild(0).gameObject);
            yield return null;
        }
        while(_grassContainer.transform.childCount > 0)
        {
            Destroy(_grassContainer.transform.GetChild(0).gameObject);
            yield return null;
        }
        while(_enemyContainer.transform.childCount > 0)
        {
            Destroy(_enemyContainer.transform.GetChild(0).gameObject);
            yield return null;
        }
    }

    public void PopulateTheGrid()
    {
        StartCoroutine(FillTheGridRoutine(3, 5, 3));
    }

    private IEnumerator FillTheGridRoutine(int grassToSpawn, int rocksToSpawn, int movingEnemyToSpawn)
    {
        
        while (grassToSpawn > 0 && CreateGrid._walkableTiles.Count > 0)
        {
            Vector3Int randomWalkableTileforGrass = _createGrid.GetGridPosition();
            SpawnGrass(randomWalkableTileforGrass);
            _createGrid.RemoveGridPosition(randomWalkableTileforGrass);
            grassToSpawn--;
            yield return null;
        }
        while (rocksToSpawn > 0 && CreateGrid._walkableTiles.Count > 0)
        {
            Vector3Int randomWalkableTileForRock = _createGrid.GetGridPosition();
            SpawnRock(randomWalkableTileForRock);
            _createGrid.RemoveGridPosition(randomWalkableTileForRock);
            _rockID++;
   
            rocksToSpawn--;
            yield return null;
        }
        if (Random.Range(0, 3) == 1 && CreateGrid._walkableTiles.Count > 0) //1 in 3 chance of getting a treasure chest
        {
            Vector3Int randomWalkableTileForLootChest = _createGrid.GetGridPosition();
            SpawnLootChest(randomWalkableTileForLootChest);
            _createGrid.RemoveGridPosition(randomWalkableTileForLootChest);
        }

        while (movingEnemyToSpawn > 0 && CreateGrid._walkableTiles.Count > 1)
        {
            Vector3Int randomWalkableTileForEnemy = _createGrid.GetGridPosition();


            foreach (var position in CreateGrid._walkableTiles)
            {
                if (randomWalkableTileForEnemy.x == position.x || randomWalkableTileForEnemy.y == position.y)
                {
                    Vector3Int randomWalkableTileForEnemyTarget = position;
                    SpawnMovingEnemy(randomWalkableTileForEnemy, randomWalkableTileForEnemyTarget);
                    _createGrid.RemoveGridPosition(randomWalkableTileForEnemy);
                    _createGrid.RemoveGridPosition(randomWalkableTileForEnemyTarget);
                    break;
                }
            }
            movingEnemyToSpawn--;
            Debug.Log(movingEnemyToSpawn);

            yield return null;
        }
    }

    public void SpawnRock(Vector3Int posToSpawn)
    {
       GameObject newRock = Instantiate(_rockPrefab, new Vector3(posToSpawn.x + 0.5f, posToSpawn.y + 0.5f, posToSpawn.z), Quaternion.identity);
        newRock.transform.parent = _rockContainer.transform;
    }

    public void SpawnLootChest(Vector3Int posToSpawn)
    {
        Instantiate(_lootChestPrefab, new Vector3(posToSpawn.x + 0.5f, posToSpawn.y + 0.5f, posToSpawn.z), Quaternion.identity);
    }

    public void SpawnGrass(Vector3Int posToSpawn)
    {
        GameObject newGrass = Instantiate(_grassPrefab, new Vector3(posToSpawn.x + 0.5f, posToSpawn.y + 0.5f, posToSpawn.z), Quaternion.identity);
        newGrass.transform.parent = _grassContainer.transform;
    }

    public void SpawnMovingEnemy(Vector3Int posToSpawn, Vector3Int targetPos)
    {
        GameObject newMovingEnemy = Instantiate(_movingEnemy, new Vector3(posToSpawn.x + 0.5f, posToSpawn.y + 0.5f, posToSpawn.z), Quaternion.identity);
        newMovingEnemy.transform.parent = _enemyContainer.transform;
        Transform movingTarget = newMovingEnemy.transform.Find("Point B");
        movingTarget.position = new Vector3(targetPos.x + 0.5f, targetPos.y + 0.5f, targetPos.z);
    }
}
