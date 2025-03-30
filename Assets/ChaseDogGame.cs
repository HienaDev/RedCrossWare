using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;

public class ChaseDogGame : MonoBehaviour, IMicroGame
{
    [SerializeField] private float timeLimit = 15f;
    public float TimeLimit => timeLimit;

    [SerializeField] private GameObject dogPrefab;

    [SerializeField] private GameObject buildingParent;
    [SerializeField] private int buildingNumber;
    [SerializeField] private GameObject[] buildingPrefab;
    private List<GameObject> buildings = new List<GameObject>();

    [SerializeField] private GameManager gameManager;

    [SerializeField] private GameObject player;
    private Vector3 playerStartPosition;

    private bool gameOver = false;

    public string CatchPhrase => catchPhrase;
    [SerializeField] private string catchPhrase;

    public bool TimeOverWin => gameWinsOnGameOver;
    [SerializeField] private bool gameWinsOnGameOver = false;

    [SerializeField] private HopAnimation hopAnimation;
    [SerializeField] private Jump2D playerController;

    [SerializeField] private GameObject cloudPrefab;

    public GameObject[] Controls => controls;
    [SerializeField] private GameObject[] controls;
    public void StartGame()
    {
        buildingParent.GetComponent<MovingBuildings>().enabled = true;
        hopAnimation.StartHopping();
        playerController.ToggleAnimator(true);
        playerController.canJump = true;
    }

    public void InitializeGame()
    {
        playerStartPosition = player.transform.position;
        InstanciateBuildings();
    }

    private void InstanciateBuildings()
    {
        for (int i = 0; i < buildingNumber; i++)
        {
            GameObject building = Instantiate(buildingPrefab[Random.Range(0, buildingPrefab.Length)], buildingParent.transform);
            building.transform.position = new Vector3(i * 10 - (15f), 0 + Random.Range(-1f, 1f), 0);
            building.transform.localScale = new Vector3(building.transform.localScale.x * Random.Range(1f, 2f),
                                                        building.transform.localScale.y * 1, 
                                                        building.transform.localScale.z * 1);
            buildings.Add(building);

            GameObject cloud = Instantiate(cloudPrefab, buildingParent.transform);
            cloud.transform.position = new Vector3(i * 10 - (15f) - Random.Range(-2f, 2f), 0 + Random.Range(-3f, 3f) + 2f, 0);
            cloud.transform.localScale *= Random.Range(0.8f, 1.2f);


            buildings.Add(cloud);

            if (i == buildingNumber - 1)
            {
                GameObject dog = Instantiate(dogPrefab, buildingParent.transform);
                dog.transform.position = building.transform.position;
                buildings.Add(dog);
            }
        }
    }

    public void CheckGame(string answer)
    {
        throw new System.NotImplementedException();
    }

    public void EndGame()
    {
        if (gameOver)
            return;

        gameOver = true;

        buildingParent.GetComponent<MovingBuildings>().enabled = false;


    }

    public void LoseGame()
    {
        if (gameOver)
            return;
        playerController.canJump = false;
        playerController.ToggleAnimator(false);
        hopAnimation.StopHopping();
        gameOver = true;
        buildingParent.GetComponent<MovingBuildings>().enabled = false;
        gameManager.GameLost();
    }

    public void ResetGame()
    {
        buildingParent.GetComponent<MovingBuildings>().enabled = false;
        buildingParent.GetComponent<MovingBuildings>().transform.position = Vector3.zero;

        foreach (GameObject building in buildings)
        {
            Destroy(building);
        }

        buildings.Clear();

        player.transform.position = playerStartPosition;

        gameOver = false;
    }



    public void WinGame()
    {
        if (gameOver)
            return;
        playerController.canJump = false;
        playerController.ToggleAnimator(false);
        hopAnimation.StopHopping();
        gameOver = true;
        buildingParent.GetComponent<MovingBuildings>().enabled = false;
        gameManager.GameWon();
    }


}
