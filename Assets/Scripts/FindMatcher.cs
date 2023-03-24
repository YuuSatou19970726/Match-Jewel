using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatcher : MonoBehaviour
{
    Board board;

    public List<Gem> gemMatches = new List<Gem>();

    private void Awake()
    {
        board = FindAnyObjectByType<Board>();
    }

    public void MatcherFind()
    {
        gemMatches.Clear();

        for (int x = 0; x < board.width; x++)
        {
            for (int y = 0; y < board.height; y++)
            {
                Gem currentGem = board.allGems[x, y];

                if (currentGem != null)
                {
                    if (x > 0 && x < board.width - 1)
                    {
                        Gem leftGem = board.allGems[x - 1, y];
                        Gem rightGem = board.allGems[x + 1, y];
                        if (leftGem != null && rightGem != null)
                        {
                            if (currentGem.gemType == leftGem.gemType && currentGem.gemType == rightGem.gemType)
                            {
                                currentGem.isMatched = true;
                                leftGem.isMatched = true;
                                rightGem.isMatched = true;

                                gemMatches.Add(currentGem);
                                gemMatches.Add(leftGem);
                                gemMatches.Add(rightGem);
                            }
                        }
                        
                    }

                    if (y > 0 && y < board.height - 1)
                    {
                        Gem aboveGem = board.allGems[x, y + 1];
                        Gem belowGem = board.allGems[x, y - 1];
                        if (aboveGem != null && aboveGem != null)
                        {
                            if (currentGem.gemType == aboveGem.gemType && currentGem.gemType == belowGem.gemType)
                            {
                                currentGem.isMatched = true;
                                aboveGem.isMatched = true;
                                belowGem.isMatched = true;

                                gemMatches.Add(currentGem);
                                gemMatches.Add(aboveGem);
                                gemMatches.Add(belowGem);
                            }
                        }
                    }
                }
            }
        }

        if (gemMatches.Count > 0)
        {
            gemMatches = gemMatches.Distinct().ToList();
        }
    }
}
