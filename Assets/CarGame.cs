using Unity.VisualScripting;
using UnityEngine;

public class CarGame : MonoBehaviour, IMicroGame
{
    public float TimeLimit => timeLimit;
    [SerializeField] private float timeLimit = 15;

    public bool TimeOverWin => gameWinsOnGameOver;
    [SerializeField] private bool gameWinsOnGameOver = true;

    [SerializeField] private CarController carController;
    [SerializeField] private CarSpawner carSpawner;

    [SerializeField] private GameManager gameManager;

    private Vector3 carStartPosition;

    private bool gameOver = false;

    public bool gameStarted = false;

    public string CatchPhrase => catchPhrase;

    public GameObject[] Controls => controls;
    [SerializeField] private GameObject[] controls;

    [SerializeField] private string catchPhrase;

    public void CheckGame(string answer)
    {
        throw new System.NotImplementedException();
    }

    public void EndGame()
    {
        if (gameOver)
            return;
        
        gameStarted = false;
        gameOver = true;
        carSpawner.ToggleSpawning(false);

    }

    public void InitializeGame()
    {
        carSpawner.StartSpawning();

        carController.canMove = true;
        carController.transform.position = carStartPosition;
    }

    public void LoseGame()
    {
        if (gameOver)
            return;
        gameStarted = false;
        gameOver = true;
        
        carSpawner.ToggleSpawning(false);
        gameManager.GameLost();
    }

    public void ResetGame()
    {
        carController.transform.position = carStartPosition;
        carSpawner.DeleteCars();
        carController.canMove = true;
        gameStarted = false;
        gameOver = false;
    }

    public void StartGame()
    {
        carSpawner.ToggleSpawning(true);
        gameStarted = true;
    }

    public void WinGame()
    {
        if (gameOver)
            return;
        gameStarted = false;
        gameOver = true;
        carSpawner.ToggleSpawning(false);

        gameManager.GameWon();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        carStartPosition = carController.transform.position;
    }
}
