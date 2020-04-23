using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TAICTOE : MonoBehaviour
{
    public Text score;
    public Image youIMG;
    public Image taictoeIMG;
    public Sprite empty;
    public Transform board;
    public Button play;

    private float[] cellsVal = new float[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };

    private bool youTurn = false; // Is it the player turn or not
    private bool victory = false; // Is it finished
    private int[] scoreV = new int[2] { 0, 0 }; // The score of both player and AI

    void Start()
    {
        play.gameObject.SetActive(false);
        youTurn = true;
        cellsVal = new float[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < board.childCount; i++)
        {
            board.GetChild(i).GetComponent<Image>().sprite = empty;
        }
    }

    /// <summary>
    /// Start a new round
    /// </summary>
    public void reStart()
    {
        victory = false;
        play.gameObject.SetActive(false);
        youTurn = (UnityEngine.Random.Range(0, 2) == 1); // Randomize who start
        cellsVal = new float[9] { 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        for (int i = 0; i < board.childCount; i++)
        {
            board.GetChild(i).GetComponent<Image>().sprite = empty;
        }
    }

    /// <summary>
    /// Let the ai make his choice if it's his turn
    /// </summary>
    void Update()
    {
        if (!youTurn && !victory)
        {
            float bestScore = -Mathf.Infinity;
            int move = 0;
            for (int i = 0; i < cellsVal.Length; i++)
            {
                if (cellsVal[i] == 0)
                {
                    cellsVal[i] = 1;
                    float score = minmax(cellsVal, 0, true);
                    cellsVal[i] = 0;
                    if (score > bestScore)
                    {
                        bestScore = score;
                        move = i;
                    }
                }
            }
            round(move);
        } // AI calculate best move

        if (checkWinner(-1) == -1 && !victory)
        {
            victory = true;
            scoreV[0]++;
            play.gameObject.SetActive(true);
        } // Player won
        else if (checkWinner(1) == 1 && !victory)
        {
            victory = true;
            scoreV[1]++;
            play.gameObject.SetActive(true);
        } // AI won
        else if (checkWinner(1) == 0)
        {
            victory = true;
            play.gameObject.SetActive(true);
        } // Tie
        score.text = scoreV[0] + " - " + scoreV[1];
    }

    /// <summary>
    /// Check the best next move to do to win or to do a tie
    /// by calculating the score resulting for each possible move
    /// and returning only the best
    /// </summary>
    /// <param name="_cellsVal"></param>
    /// <param name="depth"></param>
    /// <param name="_youTurn"></param>
    /// <returns></returns>
    int minmax(float[] _cellsVal, int depth, bool _youTurn)
    {
        int result = checkWinner(_youTurn ? -1 : 1);
        if (result != 42)
        {
            return result;
        }

        if (_youTurn)
        {
            float bestScore = Mathf.Infinity;
            for (int i = 0; i < _cellsVal.Length; i++)
            {
                if (_cellsVal[i] == 0)
                {
                    _cellsVal[i] = -1;
                    float score = minmax(_cellsVal, depth + 1, false);
                    _cellsVal[i] = 0;
                    bestScore = Math.Min(score, bestScore);
                }
            }
            return (int)bestScore;
        }
        else
        {
            float bestScore = -Mathf.Infinity;
            for (int i = 0; i < _cellsVal.Length; i++)
            {
                if (_cellsVal[i] == 0)
                {
                    _cellsVal[i] = 1;
                    float score = minmax(_cellsVal, depth + 1, true);
                    _cellsVal[i] = 0;
                    bestScore = Math.Max(score, bestScore);
                }
            }
            return (int)bestScore;
        }
    }

    /// <summary>
    /// Place a cross on the board
    /// </summary>
    /// <param name="n"></param>
    public void cross(int n)
    {
        if (youTurn && cellsVal[n] == 0 && !victory)
        {
            board.GetChild(n).GetComponent<Image>().sprite = youIMG.sprite;
            cellsVal[n] = -1;

            checkWinner(-1);

            youTurn = !youTurn;
        }
    }

    /// <summary>
    /// Place a round on the board
    /// </summary>
    /// <param name="n"></param>
    void round(int n)
    {
        if (!youTurn && cellsVal[n] == 0 && !victory)
        {
            board.GetChild(n).GetComponent<Image>().sprite = taictoeIMG.sprite;
            cellsVal[n] = 1;

            checkWinner(1);

            youTurn = !youTurn;
        }
    }

    /// <summary>
    /// Check if there's a winner, a tie or nothing
    /// </summary>
    /// <param name="who"></param>
    int checkWinner(int who)
    {
        // Check if there's still empty cells
        int k = 0;
        for (int i = 0; i < cellsVal.Length; i++)
        {
            k += (cellsVal[i] == 0)? 0 : 1;
        }
        
        // Win outcome
        if (cellsVal[0] == who && cellsVal[1] == who && cellsVal[2] == who) { return who; }
        else if (cellsVal[3] == who && cellsVal[4] == who && cellsVal[5] == who) { return who; }
        else if (cellsVal[6] == who && cellsVal[7] == who && cellsVal[8] == who) { return who; }
        else if (cellsVal[0] == who && cellsVal[3] == who && cellsVal[6] == who) { return who; }
        else if (cellsVal[1] == who && cellsVal[4] == who && cellsVal[7] == who) { return who; }
        else if (cellsVal[2] == who && cellsVal[5] == who && cellsVal[8] == who) { return who; }
        else if (cellsVal[0] == who && cellsVal[4] == who && cellsVal[8] == who) { return who; }
        else if (cellsVal[6] == who && cellsVal[4] == who && cellsVal[2] == who) { return who; }

        // Tie outcome
        else if (k >= cellsVal.Length)
        {
            return 0;
        }

        // Still empty cells
        else { return 42; }
    }

    /// <summary>
    /// Return to the menu
    /// </summary>
    public void menu()
    {
        SceneManager.LoadScene(0);
    }
}