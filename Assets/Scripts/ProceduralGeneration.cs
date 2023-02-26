using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ProceduralGeneration : MonoBehaviour
{
    [SerializeField] int width;
    [SerializeField] int minStoneheight, maxStoneHeight;
    //[SerializeField] GameObject dirt, grass, stone;
    [SerializeField] Tilemap dirtTilemap, grassTilemap, stoneTilemap;
    [SerializeField] Tile dirt, grass, stone;
    [Range(0,100)]
    [SerializeField] float heightValue, smoothness;
    [SerializeField]float seed;
    void Start()
    {
        seed = Random.Range(-1000000, 1000000);
        heightValue = Random.Range(0, 100);
        smoothness = Random.Range(heightValue - 25, heightValue + 25);
        Generation();
    }

    void Generation()
    {
        for (int x = 0; x < width; x++)//This will help spawn a tile on the x axis
        {
           int height = Mathf.RoundToInt (heightValue * Mathf.PerlinNoise(x / smoothness, seed));
            int minStoneSpawnDistance = height - minStoneheight;
            int maxStoneSpawnDistance = height - maxStoneHeight;
            int totalStoneSpawnDistance = Random.Range(minStoneSpawnDistance, maxStoneSpawnDistance);
            //Perlin noise.
            for (int y = 0; y < height; y++)//This will help spawn a tile on the y axis
            {
                if (y < totalStoneSpawnDistance)
                {
                    //spawnObj(stone, x, y);
                    stoneTilemap.SetTile(new Vector3Int(x, y, 0), stone);
                }
                else
                {
                    // spawnObj(dirt, x, y);
                    dirtTilemap.SetTile(new Vector3Int(x, y, 0), dirt);
                }

            }
            if (totalStoneSpawnDistance == height)
            {
                // spawnObj(stone, x, height);
                stoneTilemap.SetTile(new Vector3Int(x, height, 0), stone);
            }
            else
            {
                //spawnObj(grass, x, height);
                grassTilemap.SetTile(new Vector3Int(x, height, 0), grass);
            }

        }
    }

}
