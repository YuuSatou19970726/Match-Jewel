using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Obsolete]
public class Board : MonoBehaviour
{
    public int width = 7;
    public int height = 7;

    public GameObject titleBG;

    public Gem[] gemObjects;

    public Gem[,] allGems;

    public float gameSpeed = 7f;

    public FindMatcher findMatcher;

    //score
    public int score = 0;

    public enum BoardState { move, wait};
    public BoardState state = BoardState.move;

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

    void IncrementScore()
    {
        this.score++;
    }

    public void DestroyMatches()
    {
        for(int i = 0; i < findMatcher.gemMatches.Count; i++)
        {
            DestroyMatchAt(findMatcher.gemMatches[i].pos);
        }

        StartCoroutine(DecreaseRowGem());
    }

    void DestroyMatchAt(Vector2Int pos)
    {
        if (allGems[pos.x, pos.y] != null && allGems[pos.x, pos.y].isMatched)
        {
            IncrementScore();
            Instantiate(allGems[pos.x, pos.y].destroyEffect, new Vector2(pos.x, pos.y), Quaternion.identity);
            Destroy(allGems[pos.x, pos.y].gameObject);
            allGems[pos.x, pos.y] = null;
        }
    }

    IEnumerator DecreaseRowGem()
    {
        yield return new WaitForSeconds(.5f);
        int nullCounter = 0;

        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                {
                    nullCounter++;
                } else if (nullCounter > 0){
                    allGems[x, y].pos.y -= nullCounter;
                    allGems[x, y - nullCounter] = allGems[x, y];
                    allGems[x, y] = null;
                }
            }
            nullCounter = 0;
        }

        StartCoroutine(FillBroadGem());
    }

    IEnumerator FillBroadGem()
    {
        yield return new WaitForSeconds(.5f);

        ReFillBoard();

        yield return new WaitForSeconds(.5f);

        findMatcher.MatcherFind();
        if(findMatcher.gemMatches.Count > 0)
        {
            yield return new WaitForSeconds(.5f);
            DestroyMatches();
        } else {
            yield return new WaitForSeconds(.5f);
            state = BoardState.move;
        }
    }

    void ReFillBoard()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (allGems[x, y] == null)
                {
                    int gemToUse = Random.Range(0, gemObjects.Length);
                    SpawnGem(new Vector2Int(x, y), gemObjects[gemToUse]);
                }
            }
        }
        CheckMisplaceGem();
    }

    void CheckMisplaceGem()
    {
        List<Gem> foundGems = new List<Gem>();
        foundGems.AddRange(FindObjectsOfType<Gem>());
        Debug.Log(foundGems.Count);
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                if (foundGems.Contains(allGems[x, y]))
                {
                    foundGems.Remove(allGems[x, y]);
                }
            }
        }

        foreach (var gem in foundGems)
        {
            Destroy(gem.gameObject);
        }
    }
}
