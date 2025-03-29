using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.XR;

public class ElderYogaGame : MonoBehaviour, IMicroGame
{
    public bool TimeOverWin => timeOverWin;
    [SerializeField] private bool timeOverWin = false;

    public float TimeLimit => timeLimit;
    [SerializeField] private float timeLimit = 10f;

    [SerializeField] private int numberOfElders = 3;
    private int[] currentAnswer;  
    private int[] correctAnswer;

    [SerializeField] private ElderController elderPrefab;
    private List<ElderController> elders;

    [SerializeField] private GameManager gameManager;


    private bool gameOver = false;

    public bool gameStarted = false;

    [SerializeField] private Transform[] spawnPositions;

    private void Awake()
    {
        elders = new List<ElderController>();
    }

    public void SendAnswer(int index, int answerSlot)
    {
        currentAnswer[index] = answerSlot;
    }

    public void CheckGame(string answer)
    {
        bool win = true;

        for (int i = 0; i < numberOfElders * 4; i++)
        {
            if (currentAnswer[i] != correctAnswer[i])
            {
                win = false;
                break;
            }
        }

        if(win)
            WinGame();
    }

    public void EndGame()
    {
        if (gameOver)
            return;
        gameStarted = false;
        gameOver = true;
    }

    public void InitializeGame()
    {
        correctAnswer = new int[numberOfElders * 4];
        currentAnswer = new int[numberOfElders * 4];

        for (int i = 0; i < numberOfElders; i++)
        {
            ElderController elder = Instantiate(elderPrefab, spawnPositions[i]);
            

            elders.Add(elder);

            for (int j = 0; j < 4; j++)
            {
                correctAnswer[i * 4 + j] = Random.Range(0, 4);
            }

            elder.Initialize(correctAnswer[0 + i * 4],
                correctAnswer[1 + i * 4],
                correctAnswer[2 + i * 4],
                correctAnswer[3 + i * 4] , this);
        }
    }

    private void DestroyElders()
    {
        foreach (ElderController elder in elders)
        {
            Destroy(elder.gameObject);
        }
        elders.Clear();
    }

    public void LoseGame()
    {
        if (gameOver)
            return;
        gameStarted = false;
        gameOver = true;

        gameManager.GameLost();
    }

    public void ResetGame()
    {
        DestroyElders();

        gameStarted = false;
        gameOver = false;
    }

    public void StartGame()
    {
        foreach (ElderController elder in elders)
        {
            elder.StartElderGame();
        }
        gameStarted = true;
    }

    public void WinGame()
    {
        if (gameOver)
            return;
        gameStarted = false;
        gameOver = true;

        gameManager.GameWon();
    }


}
