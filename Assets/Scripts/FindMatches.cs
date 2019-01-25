using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FindMatches : MonoBehaviour
{
    private Board board;
    public List<GameObject> currentMatches = new List<GameObject>();

    void Start()
    {
        board = FindObjectOfType<Board>();    
    }

    public void FindAllMatches()
    {
        StartCoroutine(FindAllMatchesCo());
    }

    private IEnumerator FindAllMatchesCo()
    {
        yield return new WaitForSeconds(.2f);

        for (int i = 0; i < board.width; ++i)
        {
            for (int j = 0; j < board.height; ++j)
            {
                GameObject currentDot = board.allDots[i, j];
                if (null != currentDot)
                {
                    //left, right
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];
                        if (null != leftDot && null != rightDot)
                        {
                            if (currentDot.GetComponent<Dot>().isRowBomb ||
                               leftDot.GetComponent<Dot>().isRowBomb ||
                               rightDot.GetComponent<Dot>().isColumnBomb)
                            {
                                currentMatches.Union(GetRowPieces(j));
                            }

                            if (leftDot.tag == currentDot.tag &&
                                rightDot.tag == currentDot.tag)
                            {
                                if (false == currentMatches.Contains(leftDot))
                                {
                                    currentMatches.Add(leftDot);
                                }
                                leftDot.GetComponent<Dot>().isMatched = true;

                                if (false == currentMatches.Contains(rightDot))
                                {
                                    currentMatches.Add(rightDot);
                                }                        
                                rightDot.GetComponent<Dot>().isMatched = true;

                                if (false == currentMatches.Contains(currentDot))
                                {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().isMatched = true;
                            }
                        }
                    }
                    //up, down
                    if (j > 0 && j < board.height - 1)
                    {
                        GameObject upDot = board.allDots[i, j + 1];
                        GameObject downDot = board.allDots[i, j - 1];
                        if (null != upDot && null != downDot)
                        {
                            if (upDot.tag == currentDot.tag &&
                                downDot.tag == currentDot.tag)
                            {
                                if (false == currentMatches.Contains(upDot))
                                {
                                    currentMatches.Add(upDot);
                                }
                                upDot.GetComponent<Dot>().isMatched = true;

                                if (false == currentMatches.Contains(downDot))
                                {
                                    currentMatches.Add(downDot);
                                }
                                downDot.GetComponent<Dot>().isMatched = true;

                                if (false == currentMatches.Contains(currentDot))
                                {
                                    currentMatches.Add(currentDot);
                                }
                                currentDot.GetComponent<Dot>().isMatched = true;
                            }
                        }
                    }
                }
            }
        }
    }

    List<GameObject> GetColumnPieces(int column)
    {
        List<GameObject> dots = new List<GameObject>();

        for (int i = 0; i < board.height; ++i)
        {
            if (null != board.allDots[column, i])
            {
                dots.Add(board.allDots[column, i]);
                board.allDots[column, i].GetComponent<Dot>().isMatched = true;
            }
        }
        return dots;
    }

    List<GameObject> GetRowPieces(int row)
    {
        List<GameObject> dots = new List<GameObject>();

        for (int i = 0; i < board.width; ++i)
        {
            if (null != board.allDots[i, row])
            {
                dots.Add(board.allDots[i, row]);
                board.allDots[i, row].GetComponent<Dot>().isMatched = true;
            }          
        }
        return dots;
    }
}
