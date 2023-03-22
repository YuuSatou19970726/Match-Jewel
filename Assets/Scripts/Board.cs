using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Board : MonoBehaviour
{
    public int width = 7;
    public int height = 7;

    public GameObject titleBG;

    public Gem[] gemObjects;

    public Gem[,] allGems;

    public float gameSpeed = 7f;

    FindMatcher findMatcher;

    private void Awake()
    {
        findMatcher = FindAnyObjectByType<FindMatcher>();
    }

    // Start is called before the first frame update
    void Start()
    {
        allGems = new Gem[width, height];
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        findMatcher.MatcherFind();
    }

    void Setup()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                GameObject tile = Instantiate(titleBG, new Vector2(x, y), Quaternion.identity);
                tile.transform.parent = transform;
                tile.name = $"Tile BG - {x}, {y}";

                int gemToUse = Random.Range(0, gemObjects.Length);

                while (Match(new Vector2Int(x, y), gemObjects[gemToUse]))
                {
                    gemToUse = Random.Range(0, gemObjects.Length);
                }
                SpawnGem(new Vector2Int(x, y), gemObjects[gemToUse]);
            }
        }
    }

    void SpawnGem(Vector2Int pos, Gem gem)
    {
        Gem spawnGem = Instantiate(gem, new Vector2(pos.x, pos
            .y), Quaternion.identity);
        spawnGem.transform.parent = transform;
        spawnGem.name = $"Gem - {pos.x}, {pos.y}";

        spawnGem.SetupGem(pos, this);
        allGems[pos.x, pos.y] = spawnGem;
    }
    bool Match(Vector2Int posToCheck, Gem gemToCheck)
    {
        if(posToCheck.x > 1 && gemToCheck.gemType == allGems[posToCheck.x - 1, posToCheck.y].gemType && gemToCheck.gemType == allGems[posToCheck.x - 2, posToCheck.y].gemType)
        {
            return true;
        } else if (posToCheck.y > 1 && gemToCheck.gemType == allGems[posToCheck.x, posToCheck.y - 1].gemType && gemToCheck.gemType == allGems[posToCheck.x, posToCheck.y - 2].gemType)
        {
            return true;
        }
        return false;
    }
}
