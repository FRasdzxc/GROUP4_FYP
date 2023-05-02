using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Map : MonoBehaviour
{
    public BiomePreset[] biomes;
    public GameObject tilePrefab;

    [Header("Dimensions")]
    public int width = 50;
    public int height = 50;
    public float scale = 1.0f;
    public Vector2 offset;

    [Header("Height Map")]
    public Wave[] heightWaves;
    public Gradient heightDebugColors;
    public float[,] heightMap;

    [Header("Moisture Map")]
    public Wave[] moistureWaves;
    public Gradient moistureDebugColors;
    private float[,] moistureMap;

    [Header("Heat Map")]
    public Wave[] heatWaves;
    public Gradient heatDebugColors;
    private float[,] heatMap;

    private float last;

    void Start ()
    {
        Random.InitState(DateTime.Now.Millisecond);
        foreach (var wave in heightWaves)
            wave.seed = Random.Range(1f, 1000f);
        foreach (var wave in moistureWaves)
            wave.seed = Random.Range(1f, 1000f);
        foreach (var wave in heatWaves)
            wave.seed = Random.Range(1f, 1000f);

        ////GenerateMap();
    }

    void GenerateMap ()
    {
/*        // height map
        heightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset);
        HashSet<Vector2Int> newHeightMap = NoiseGenerator.Generate(width, height, scale, heightWaves, offset);
        // moisture map
        moistureMap = NoiseGenerator.Generate(width, height, scale, moistureWaves, offset);

        // heat map
        heatMap = NoiseGenerator.Generate(width, height, scale, heatWaves, offset);*/

        Color[] pixels = new Color[width * height];
        for(int x = 0; x < width; ++x)
        {
            for(int y = 0; y < height; ++y)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(x, y, 0), Quaternion.identity);
                tile.GetComponent<SpriteRenderer>().sprite = GetBiome(heightMap[x, y], moistureMap[x, y], heatMap[x, y]).GetTleSprite();
            }
        }
    }

    public class BiomeTempData
    {
        public BiomePreset biome;

        public BiomeTempData (BiomePreset preset)
        {
            biome = preset;
        }
        
        public float GetDiffValue (float height, float moisture, float heat)
        {
            return (height - biome.minHeight) + (moisture - biome.minMoisture) + (heat - biome.minHeat);
        }
    }

    BiomePreset GetBiome (float height, float moisture, float heat)
    {
        BiomePreset biomeToReturn = null;
        List<BiomeTempData> biomeTemp = new List<BiomeTempData>();

        foreach(BiomePreset biome in biomes)
        {
            if(biome.MatchCondition(height, moisture, heat))
            {
                biomeTemp.Add(new BiomeTempData(biome));
            }
        }

        float curVal = 0.0f;

        foreach(BiomeTempData biome in biomeTemp)
        {
            if(biomeToReturn == null)
            {
                biomeToReturn = biome.biome;
                curVal = biome.GetDiffValue(height, moisture, heat);
            }
            else
            {
                if(biome.GetDiffValue(height, moisture, heat) < curVal)
                {
                    biomeToReturn = biome.biome;
                    curVal = biome.GetDiffValue(height, moisture, heat);
                }
            }
        }

        if(biomeToReturn == null)
            biomeToReturn = biomes[0];

        return biomeToReturn;
    }
}