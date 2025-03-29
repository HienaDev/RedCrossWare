using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class ElderBodyPartInteractable : MonoBehaviour, IInteractable
{

    [SerializeField] private Sprite[] bodyPartSprites;
    private ElderYogaGame elderYogaGame;
    [SerializeField] private ElderController elderController;
    [SerializeField] private int index = 0;
    private int currentAnswer = 0;

    [SerializeField] private SpriteRenderer bodyAnswerSprite;


    public void Initialize(ElderYogaGame game)
    {
        int part = Random.Range(0, bodyPartSprites.Length);
        // Set the sprite to a random body part
        currentAnswer = part;
        elderYogaGame = game;
        GetComponent<SpriteRenderer>().sprite = bodyPartSprites[Random.Range(0, bodyPartSprites.Length)];
        elderYogaGame.SendAnswer(index, currentAnswer);
    }
    public void Interact()
    {
        currentAnswer++;
        if(currentAnswer >= bodyPartSprites.Length)
        {
            currentAnswer = 0;
        }
        GetComponent<SpriteRenderer>().sprite = bodyPartSprites[Random.Range(0, bodyPartSprites.Length)];
        elderYogaGame.SendAnswer(index, currentAnswer);

        elderYogaGame.CheckGame("test");
    }

    public void StartGameElder()
    {
        bodyAnswerSprite.sprite = bodyPartSprites[elderController.correctAnswer[index]];

    }

    public void ResetInteractable()
    {
        throw new System.NotImplementedException();
    }


}
