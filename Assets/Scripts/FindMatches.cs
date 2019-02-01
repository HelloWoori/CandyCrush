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

    private void IsAdjacentBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        if (dot1.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot1.column, dot1.row));
        }

        if (dot2.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot2.column, dot2.row));
        }

        if (dot3.isAdjacentBomb)
        {
            currentMatches.Union(GetAdjacentPieces(dot3.column, dot3.row));
        }
    }

    private void IsRowBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        if (dot1.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot1.row));
        }

        if (dot2.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot2.row));
        }

        if (dot3.isRowBomb)
        {
            currentMatches.Union(GetRowPieces(dot3.row));
        }
    }

    private void IsColumnBomb(Dot dot1, Dot dot2, Dot dot3)
    {
        if (dot1.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot1.column));
        }

        if (dot2.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot2.column));
        }

        if (dot3.isColumnBomb)
        {
            currentMatches.Union(GetColumnPieces(dot3.column));
        }
    }

    private void AddToListAndMatch(GameObject dot)
    {
        if (false == currentMatches.Contains(dot))
        {
            currentMatches.Add(dot);
        }
        dot.GetComponent<Dot>().isMatched = true;
    }

    private void GetNearbyPieces(GameObject dot1, GameObject dot2, GameObject dot3)
    {
        AddToListAndMatch(dot1);
        AddToListAndMatch(dot2);
        AddToListAndMatch(dot3);
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
                    Dot currentDotDot = currentDot.GetComponent<Dot>();

                    //left, right
                    if (i > 0 && i < board.width - 1)
                    {
                        GameObject leftDot = board.allDots[i - 1, j];
                        GameObject rightDot = board.allDots[i + 1, j];

                        if (null != leftDot && null != rightDot)
                        {
                            Dot leftDotDot = leftDot.GetComponent<Dot>();
                            Dot rightDotDot = rightDot.GetComponent<Dot>();

                            if (leftDot.tag == currentDot.tag &&
                                rightDot.tag == currentDot.tag)
                            {
                                IsRowBomb(currentDotDot, leftDotDot, rightDotDot);
                                IsColumnBomb(currentDotDot, leftDotDot, rightDotDot);
                                IsAdjacentBomb(currentDotDot, leftDotDot, rightDotDot);
                                GetNearbyPieces(leftDot, currentDot, rightDot);
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
                            Dot upDotDot = upDot.GetComponent<Dot>();
                            Dot downDotDot = downDot.GetComponent<Dot>();

                            if (upDot.tag == currentDot.tag &&
                                downDot.tag == currentDot.tag)
                            {
                                IsColumnBomb(currentDotDot, upDotDot, downDotDot);
                                IsRowBomb(currentDotDot, upDotDot, downDotDot);
                                IsAdjacentBomb(currentDotDot, upDotDot, downDotDot);
                                GetNearbyPieces(upDot, currentDot, downDot);
                            }
                        }
                    }
                }
            }
        }
    }

    public void MatchPiecesOfColor(string color)
    {
        for (int i = 0; i < board.width; ++i)
        {
            for (int j = 0; j < board.height; ++j)
            {
                //Check if that pieces exists
                if (null != board.allDots[i, j])
                {
                    //Check the tag on that dot
                    if (board.allDots[i, j].tag == color)
                    {
                        //Set that dot to be matched
                        board.allDots[i, j].GetComponent<Dot>().isMatched = true;
                    }
                }
            }
        }
    }

    List<GameObject> GetAdjacentPieces(int column, int row)
    {
        List<GameObject> dots = new List<GameObject>();

        for (int i = column - 1; i <= column + 1; ++i)
        {
            for (int j = row - 1; j <= row + 1; ++j)
            {
                //Check if the pieces is inside the board
                if (i >= 0 && i < board.width && j >= 0 && j < board.height)
                {
                    dots.Add(board.allDots[i, j]);
                    board.allDots[i, j].GetComponent<Dot>().isMatched = true;                      
                }
            }
        }

        return dots;
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

    public void CheckBombs()
    {
        //Did the player move something?
        if (null != board.currentDot)
        {
            //Is the piece they moved matched?
            if (board.currentDot.isMatched)
            {
                //Make it unmatched
                board.currentDot.isMatched = false;
                //Decide what kind of bomb to make
                /*
                int typeOfBomb = Random.Range(0, 100);
                if (typeOfBomb < 50)
                {
                    //Make a row bomb
                    board.currentDot.MakeRowBomb();
                }
                else if (typeOfBomb >= 50)
                {
                    //Make a column bomb
                    board.currentDot.MakeColumnBomb();
                }
                */
                if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45) || //right swipe
                    (board.currentDot.swipeAngle < -135 && board.currentDot.swipeAngle >= 135)) //left swipe
                {
                    board.currentDot.MakeRowBomb();
                }
                else //up, down swipe
                {
                    board.currentDot.MakeColumnBomb();
                }
            }
            //Is the other piece matched?
            else if (null != board.currentDot.otherDot)
            {
                Dot otherDot = board.currentDot.otherDot.GetComponent<Dot>();
                //Is the other Dot matched?
                if (otherDot.isMatched)
                {
                    //Make it unmatched
                    otherDot.isMatched = false;
                    //Decide what kind of bomb to make
                    /*
                    int typeOfBomb = Random.Range(0, 100);
                    if (typeOfBomb < 50)
                    {
                        //Make a row bomb
                        otherDot.MakeRowBomb();
                    }
                    else if (typeOfBomb >= 50)
                    {
                        //Make a column bomb
                        otherDot.MakeColumnBomb();
                    }
                    */
                    if ((board.currentDot.swipeAngle > -45 && board.currentDot.swipeAngle <= 45) || //right swipe
                        (board.currentDot.swipeAngle < -135 && board.currentDot.swipeAngle >= 135)) //left swipe
                    {
                        otherDot.MakeRowBomb();
                    }
                    else //up, down swipe
                    {
                        otherDot.MakeColumnBomb();
                    }
                }
            }
        }
    }
}
