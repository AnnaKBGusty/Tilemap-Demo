using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using System.Linq;

public class CreateGrid : MonoBehaviour
{
    [SerializeField]
    private Tilemap world;
    [SerializeField]
    private Tilemap walls;
    [SerializeField]
    private RuleTile _tile;
    [SerializeField]
    private int _width, _height;

    [SerializeField]
    private Sprite[] wallSprites;

    [SerializeField]
    private Sprite walkableSprite;

    private Player _player;

    private Tile _playerTile;

    [SerializeField]
    public static List<Vector3Int> _walkableTiles = new List<Vector3Int>();

    private SpawnManager _spawnManager;

    void Start()
    {
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<SpawnManager>();
        if(_spawnManager == null)
        {
            Debug.Log("Spawn Manager is NULL.");
        }

        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        if(_player == null)
        {
            Debug.LogError("Player is NULL.");
        }
        CreateNewLevel();


        GameManager.onLevelChange += ClearEveryTileMap;
    }

    private void ClearEveryTileMap()
    {
        StartCoroutine(ClearTileMapsAndMakeANewOne());
    }

    private IEnumerator ClearTileMapsAndMakeANewOne()
    {
        world.ClearAllTiles();
        walls.ClearAllTiles();
        _walkableTiles.Clear();
        yield return new WaitForSeconds(0.1f);
        CreateNewLevel();
    }

    private void OnDisable()
    {
        GameManager.onLevelChange -= ClearEveryTileMap;
    }

    private void CreateNewLevel()
    {
        BuildGridUpAndRight(GetRandomX(), GetRandomY(), -3, 0);
        BuildGridDownandLeft(GetRandomX(), GetRandomY(), 3, 0);
        FindWallTiles(); //can only be called when grid is done, since tiles change as being drawn        
        FindWalkableTiles();
        StartCoroutine(WaitASecondAndPopulateGridRoutine());
    }

    public Vector3Int GetGridPosition()
    {     
        Vector3Int availableGridPos = _walkableTiles[Random.Range(0, _walkableTiles.Count)];
        return availableGridPos;    
    }

    public void RemoveGridPosition(Vector3Int posToRemove)
    {
        _walkableTiles.Remove(posToRemove);
    }

    /*private void GetPlayerTile()
    {
        Vector3Int playerPos = _player.GetPlayerPosition();  
        world.SetTile(playerPos, _playerTile);
        StartCoroutine(GoBackToRegularTileRoutine(playerPos));
    }

    private IEnumerator GoBackToRegularTileRoutine(Vector3Int playerPos)
    {
        yield return new WaitForSeconds(2.0f);
        world.SetTile(playerPos, _tile);
    }*/

    private void BuildGridUpAndRight(int width, int height, int startX, int startY)
    {
        for (int x = startX; x < width + startX; x++)
        {
            for (int y = startY; y < height + startY; y++)
            {
                world.SetTile(new Vector3Int(x, y, 0), _tile);
            }
        }
    }

    private void BuildGridDownandLeft(int width, int height, int startX, int startY)
    {
        for (int x = startX; x > (width - startX) * -1; x--)
        {
            for (int y = startY; y > (height - startY) * -1; y--)
            {
                world.SetTile(new Vector3Int(x, y, 0), _tile);               
            }
        }
    }

    private void FindWallTiles()
    {
        for (int x = (int)world.localBounds.min.x; x < world.localBounds.max.x; x++)
        {
            for (int y = (int)world.localBounds.min.y; y <world.localBounds.max.y; y++)
            {
                TileBase thisTile = world.GetTile(new Vector3Int(x, y, 0)); //get the tile
                Sprite thisTileSprite = world.GetSprite(new Vector3Int(x, y, 0)); //get the sprite
                foreach(var wall in wallSprites) //loop through wall sprites
                {
                    if (thisTileSprite == wall) //compare to current tile's sprite
                    {
                        Debug.Log("found a wall!!");
                        walls.SetTile(new Vector3Int(x, y, 0), thisTile); //set onto wall layer
                    }
                }
            }
        }
    }

    private void FindWalkableTiles()
    {
        for (int x = (int)world.localBounds.min.x; x < world.localBounds.max.x; x++)
        {
            for (int y = (int)world.localBounds.min.y; y < world.localBounds.max.y; y++)
            {
                Vector3Int thisTilePosition = new Vector3Int(x, y, 0);
                Sprite thisTileSprite = world.GetSprite(new Vector3Int(x, y, 0)); //get the sprite
                if (thisTileSprite == walkableSprite) //compare to current tile's sprite
                {
                    Debug.Log("found a walkable tile!!");
                    if (!_walkableTiles.Contains(thisTilePosition))
                    {
                        _walkableTiles.Add(thisTilePosition);
                    }
                }             
            }
        }
    }

    private int GetRandomX()
    {
        _width = Random.Range(10, 15);
        return _width;
    }

    private int GetRandomY()
    {
        _height = Random.Range(10, 15);
        return _height;
    }

    private IEnumerator WaitASecondAndPopulateGridRoutine()
    {
        yield return new WaitForEndOfFrame();
        _spawnManager.PopulateTheGrid();
    }
}