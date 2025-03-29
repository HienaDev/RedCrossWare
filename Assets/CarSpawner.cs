using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CarSpawner : MonoBehaviour
{
    public GameObject carPrefab;  // Car prefab to spawn
    public float spawnRate = 2f;  // Time between spawns
    public float carSpeed = 5f;   // Speed at which cars move downward
    public int spawnWidth = 5; // Random horizontal variation

    private bool spawning = false;

    [SerializeField] private Vector2 carsPerRow = new Vector2(1, 3);

    private List<GameObject> cars = new List<GameObject>();
    public void ToggleSpawning(bool toggle) => spawning = toggle;

    private Coroutine carSpawningCR;

    void Start()
    {

       
    }

    public void StartSpawning()
    {
        carSpawningCR = StartCoroutine(SpawnCar());
    }

    public void DeleteCars()
    {
        StopCoroutine(carSpawningCR);
        foreach (GameObject car in cars)
        {
            Destroy(car);
        }

        cars.Clear();
    }

    private IEnumerator SpawnCar()
    {
        while(!spawning) yield return null;

        Debug.Log("spawning:" + spawning);  
        int numberOfCars = Random.Range((int)carsPerRow.x, (int)carsPerRow.y + 1);

        for (int i = 0; i < numberOfCars; i++)
        {
            SpawnSingleCar(); yield return new WaitForSeconds(Random.Range(0.1f, 0.2f));
        }

        yield return new WaitForSeconds(Random.Range(1f, 2f));

        StartCoroutine(SpawnCar());
    }

    private void SpawnSingleCar()
    {
        // Random horizontal position within the spawn width
        int randomX = Random.Range(-spawnWidth, spawnWidth);

        if (randomX % 2 == 0)
        {
            randomX += 1;
        }

        Vector3 spawnPos = new Vector3(transform.position.x + randomX, transform.position.y, 0);

        // Spawn the car
        GameObject newCar = Instantiate(carPrefab, spawnPos, Quaternion.identity);

        // Add downward movement to the car
        Rigidbody2D rb = newCar.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(0, Random.Range(-carSpeed - 2f, -carSpeed + 2f)); // Move downwards
        }
    }
}
