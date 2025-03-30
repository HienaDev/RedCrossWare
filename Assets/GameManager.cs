using DG.Tweening;
using System;
using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] microGames;

    private IMicroGame currentGame;
    private bool currentGameComplete = false;
    private bool currentGameWon = false;
    private int currentGameIndex = 0;


    [Header("Transition Settings")]
    [SerializeField] private Transform doorPivot;
    [SerializeField] private Transform leftDoor;
    [SerializeField] private Transform rightDoor;
    public float transitionTime = 0.5f;  // Speed of closing/opening
    public float holdTime = 0.5f;        // How long they stay closed
    private Vector3 leftStartPos, rightStartPos; // Store original positions

    [Header("Game Settings")]
    [SerializeField] private float timerBeforeGameStarts = 3f;
    [SerializeField] private float gameCelebrationTime = 2f; // Adjust as needed
    [SerializeField] private Image imageTimer;
    [SerializeField] private int lives = 3;
    private int currentLives;

    private Coroutine currentGameCoroutine;

    private int currentGradient;

    [SerializeField] private Material courtainMaterial;
    [SerializeField] private Color[] colors;
    [SerializeField] private Texture[] texture;

    [SerializeField] private PopOutAndSlideDown PopOutAndSlideDown;

    [SerializeField] private PopOutAndSlideDown countdownTimer;
    private TextMeshProUGUI countdownTimerText;

    [SerializeField] private GameObject fuse;
    [ SerializeField] private GameObject bomb;

    private int miniGamesCleared = 0;

    [SerializeField] private GameObject gameOverScreen;
    private TextMeshProUGUI gameOverScreenText;
    private bool gameOver = false;

    [SerializeField] private GameObject[] livesImages;

    [SerializeField] private AudioClip fuseSound;
    [SerializeField] private AudioClip yay;
    [SerializeField] private GameObject confeti;   
    [SerializeField] private AudioClip sad;
    [SerializeField] private AudioClip countdown;
    [SerializeField] private AudioClip explosion;
    [SerializeField] private AudioClip whistle;

    [SerializeField] private GameObject menu;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        menu.SetActive(true);

        countdownTimerText = countdownTimer.GetComponent<TextMeshProUGUI>();
        gameOverScreenText = gameOverScreen.GetComponent<TextMeshProUGUI>();
        gameOverScreen.SetActive(false);
        gameOverScreenText.text = "";
        //StartGame();
    }

    public void StartGame()
    {
        TurnGradientOff();
        currentGradient = UnityEngine.Random.Range(1, 6);
        courtainMaterial.SetInt("_Gradient" + currentGradient.ToString(), 1);
        courtainMaterial.SetColor("_BallColor", colors[UnityEngine.Random.Range(0, colors.Length)]);
        courtainMaterial.SetTexture("_Texture", texture[UnityEngine.Random.Range(0, texture.Length)]);
        currentLives = lives;
        Shuffle(microGames);
        Transition();
    }

    private void TurnGradientOff()
    {
        for (int i = 0; i <= 5; i++)
        {
            courtainMaterial.SetInt("_Gradient" + i.ToString(), 0);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void GameLost()
    {
        AudioManager.Instance.PlaySound(sad);
        //StopCoroutine(currentGameCoroutine);
        currentGameWon = false;
        currentGameComplete = true;
        currentLives--;

        for (int i = 0; i < lives; i++)
        {
            livesImages[i].gameObject.SetActive(false);
        }
        for (int i = 0; i < currentLives; i++)
        {
            livesImages[i].gameObject.SetActive(true);
        }

        if (currentLives <= 0)
        {
            Debug.Log("Game Over");
            gameOver = true;

            if (currentGame != null)
            {
                currentGame.EndGame();
                currentGame.ResetGame();

                foreach (GameObject control in currentGame.Controls)
                {
                    control.SetActive(false);
                }
                microGames[currentGameIndex].gameObject.SetActive(false);


            }

            Transition();
        }
        else
        {
            NextGame();
        }
    }

    private void GameOver()
    {
        confeti.SetActive(true);
        gameOverScreen.SetActive(true);
        gameOverScreenText.text = $"You've cleared {miniGamesCleared} minigames!\nThank you for your help!";
    }

    public void GameWon()
    {
        AudioManager.Instance.PlaySound(yay);
        confeti.SetActive(true);
        //StopCoroutine(currentGameCoroutine);
        currentGameWon = true;
        currentGameComplete = true;
        miniGamesCleared++;
        NextGame();
    }

    public void NextGame() => StartCoroutine(NextGameCR());
    public IEnumerator NextGameCR()
    {
        yield return new WaitForSeconds(gameCelebrationTime);
        confeti.SetActive(false);
        Transition();
    }



    private void SelectNewGame()
    {

        if (currentGame != null)
        {
            currentGame.EndGame();
            currentGame.ResetGame();

            foreach (GameObject control in currentGame.Controls)
            {
                control.SetActive(false);
            }
            microGames[currentGameIndex].gameObject.SetActive(false);


        }

        currentGameIndex++;

        if (currentGameIndex >= microGames.Length)
        {
            currentGameIndex = 0;
            Shuffle(microGames);
        }

        currentGameComplete = false;

        currentGame = microGames[currentGameIndex] as IMicroGame;

        microGames[currentGameIndex].gameObject.SetActive(true);
        currentGame.InitializeGame();
    }

    public void Transition()
    {

        TurnGradientOff();


        currentGradient = UnityEngine.Random.Range(1, 6);
        courtainMaterial.SetInt("_Gradient" + currentGradient.ToString(), 1);
        courtainMaterial.SetColor("_BallColor", colors[UnityEngine.Random.Range(0, colors.Length)]);
        courtainMaterial.SetTexture("_Texture", texture[UnityEngine.Random.Range(0, texture.Length)]);


        bomb.SetActive(false);
        imageTimer.enabled = false;
        doorPivot.rotation = UnityEngine.Random.rotation;
        doorPivot.eulerAngles = new Vector3(0, 0, doorPivot.eulerAngles.z); // Lock the rotation to Z-axis only
        // Save starting positions
        leftStartPos = leftDoor.position;
        rightStartPos = rightDoor.position;
        // Calculate the center of the screen (world position)
        Vector3 centerPosition = Vector3.zero;

        // Create the DOTween sequence
        Sequence transitionSequence = DOTween.Sequence();

        // Move doors to the center (closing)
        transitionSequence.Append(leftDoor.DOMove(centerPosition, transitionTime).SetEase(Ease.InOutQuad));
        transitionSequence.Join(rightDoor.DOMove(centerPosition, transitionTime).SetEase(Ease.InOutQuad));

        if(!gameOver)
            transitionSequence.AppendCallback(SelectNewGame);

        transitionSequence.AppendCallback(ShowCatchPhrase);

        // Optional: Hold the doors closed for effect
        transitionSequence.AppendInterval(holdTime);

        // Move doors back to their original world positions (opening)
        transitionSequence.Append(leftDoor.DOMove(leftStartPos, transitionTime).SetEase(Ease.InOutQuad));
        transitionSequence.Join(rightDoor.DOMove(rightStartPos, transitionTime).SetEase(Ease.InOutQuad)).onComplete += () => {
            if (!gameOver) StartTimer();
        };

        transitionSequence.onComplete += () =>
        {
            for (int i = 0; i < currentLives; i++)
            {
                livesImages[i].gameObject.SetActive(true);
            }

            if(gameOver)
            {
                GameOver();
            }
            //Transition();
            //// Deactivate all microgames
            //foreach (var game in microGames)
            //{
            //    game.gameObject.SetActive(false);
            //}
            //// Activate the next microgame
            //microGames[0].gameObject.SetActive(true);
            //(microGames[0] as IMicroGame).StartGame();
        };
    }

    private void ShowCatchPhrase()
    {
        PopOutAndSlideDown.gameObject.SetActive(true);
        Debug.Log(currentGame.CatchPhrase);
        if(!gameOver)
        {
            PopOutAndSlideDown.AnimatePopAndSlide(currentGame.CatchPhrase);
            foreach (GameObject control in currentGame.Controls)
            {
                control.SetActive(true);
            }
        }

        else
            PopOutAndSlideDown.AnimatePopAndSlide("Game Over!");
    }

    private void StartTimer()
    {
        currentGameCoroutine = StartCoroutine(StartTimerCR());
    }

    private IEnumerator StartTimerCR()
    {

        float timer = 0;
        float countdownNumber = 0;
        countdownTimerText.gameObject.SetActive(true);
        float thisPitch = UnityEngine.Random.Range(0.8f, 0.9f);
        AudioManager.Instance.PlaySound(countdown, pitch: thisPitch);
        countdownTimer.AnimatePopAndSlide(Mathf.FloorToInt(timerBeforeGameStarts - timer + 1).ToString());

        

        while (timer < timerBeforeGameStarts)
        {
            countdownNumber += Time.deltaTime;

            countdownTimerText.text = Mathf.FloorToInt(timerBeforeGameStarts - timer + 1).ToString();

            if (countdownNumber > 1)
            {
                thisPitch += 0.15f;
                AudioManager.Instance.PlaySound(countdown, pitch: thisPitch);
                countdownNumber = 0;
                countdownTimer.AnimatePopAndSlide(Mathf.FloorToInt(timerBeforeGameStarts - timer + 1).ToString());
            }

            timer += Time.deltaTime;
            yield return null;
        }

        AudioManager.Instance.PlaySound(whistle, pitch: UnityEngine.Random.Range(0.9f, 1.1f));
        countdownTimerText.gameObject.SetActive(true);

        imageTimer.enabled = true;

        currentGame.StartGame();

        timer = 0;
        bomb.SetActive(true);
        fuse.SetActive(true);
        AudioManager.Instance.PlayLoopingSound(fuseSound, volume: 0.3f);

        float timeLimit = currentGame.TimeLimit;

        while (timer < timeLimit && !currentGameComplete)
        {
            fuse.transform.position = new Vector3(Map01ToRange(1 - (timer / timeLimit)), fuse.transform.position.y, fuse.transform.position.z);
            timer += Time.deltaTime;
            imageTimer.fillAmount = 1 - (timer / timeLimit);
            yield return null;
        }
        
        fuse.SetActive(false);
        AudioManager.Instance.StopLoopingSound(fuseSound);
        Debug.Log("Time's up!");
        if (!currentGameComplete)
        {
            if (currentGame.TimeOverWin)
                currentGame.WinGame();
            else
            {
                currentGame.LoseGame();
                AudioManager.Instance.PlaySound(explosion);
            }
                
        }

        //imageTimer.enabled = false;
        currentGameComplete = false;

    }

    float Map01ToRange(float value01)
    {
        return (value01 * 10.8f) - 5.4f;
    }

    void Shuffle<T>(T[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);
            (array[i], array[randomIndex]) = (array[randomIndex], array[i]); // Swap elements
        }
    }
}
