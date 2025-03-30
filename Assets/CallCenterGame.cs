using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static IMicroGame;

public class CallCenterGame : MonoBehaviour, IMicroGame
{
    public float TimeLimit => timeLimit;

    [SerializeField] private IMicroGame.GameAnswers[] Answers;
    [SerializeField] private int buttonNumber = 3;
    [SerializeField] private Transform[] buttonPositions;

    [SerializeField] private float timeLimit = 10f;

    private GameAnswers currentAnswer;

    [SerializeField] private ButtonInteractable buttonPrefab;
    private List<ButtonInteractable> buttons;

    [SerializeField] private PhoneInteractable phone;

    [SerializeField] private GameManager gameManager;

    private bool gameOver = false;

    public bool TimeOverWin => gameWinsOnGameOver;

    public string CatchPhrase => catchPhrase;
    [SerializeField] private string catchPhrase;

    [SerializeField] private bool gameWinsOnGameOver = false;

    public GameObject[] Controls => controls;
    [SerializeField] private GameObject[] controls;

    private void Awake()
    {
        buttons = new List<ButtonInteractable>();
    }

    public void StartGame()
    {


        //phone.colliderInteract.enabled = (true);
        phone.StartRinging();
    }


    public void InitializeGame()
    {
        InstantiateButtons();
    }

    private void InstantiateButtons()
    {
        Shuffle(Answers);
        if(buttonNumber > Answers.Length)
        {
            buttonNumber = Answers.Length;
        }
        for (int i = 0; i < buttonNumber; i++)
        {
            ButtonInteractable button = Instantiate(buttonPrefab, transform);
            buttons.Add(button);
            button.transform.position = buttonPositions[i].position;
            button.Initialize(Answers[i].sprite, Answers[i].answer, this);
        }

        SelectAnswer();
    }

    private void SelectAnswer()
    {
        currentAnswer = Answers[Random.Range(0, Answers.Length)];
    }

    public void CheckGame(string answer)
    {
        if (gameOver)
            return;


        if (currentAnswer.answer == answer)
        {
            WinGame();
        }
        else
        {
            LoseGame();
        }

        gameOver = true;
    }

    public Sprite GetAnswerSprite()
    {
        foreach (var button in buttons)
        {
            button.colliderInteract.enabled = true;
        }

        return currentAnswer.worry;
    }

    public void WinGame()
    {
        if (gameOver)
            return;

        gameOver = true;
        Debug.Log("You win!");
        gameManager.GameWon();
        
    }

    public void LoseGame()
    {
        if (gameOver)
            return;

        gameOver = true;
        Debug.Log("You lose!");
        gameManager.GameLost();
        
    }
    public void EndGame()
    {
        

    }

    void Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = Random.Range(0, i + 1);
            (array[i], array[randomIndex]) = (array[randomIndex], array[i]); // Swap elements
        }
    }

    public void ResetGame()
    {
        Debug.Log("Resetting call center game");
        phone.ResetInteractable();

        foreach (var button in buttons)
        {
            Destroy(button.gameObject);
        }

        buttons.Clear();

        gameOver = false;

    }

}
