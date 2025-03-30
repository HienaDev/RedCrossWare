using UnityEngine;

public class ElderBodyPartInteractable : MonoBehaviour, IInteractable
{

    [SerializeField] private Sprite[] bodyPartSprites;
    private ElderYogaGame elderYogaGame;
    [SerializeField] private ElderController elderController;
    [SerializeField] private int index = 0;
    private int currentAnswer = 0;

    [SerializeField] private SpriteRenderer bodyAnswerSprite;

    [SerializeField] private AudioClip pop;

    private bool canInteract = false;

    public void Initialize(ElderYogaGame game)
    {
        int part = Random.Range(0, bodyPartSprites.Length);
        // Set the sprite to a random body part
        currentAnswer = part;
        elderYogaGame = game;
        GetComponent<SpriteRenderer>().sprite = bodyPartSprites[currentAnswer];
        elderYogaGame.SendAnswer(index, currentAnswer);
    }
    public void Interact()
    {
        //if (!canInteract)
        //    return;
        currentAnswer++;
        AudioManager.Instance.PlaySound(pop, pitch:Random.Range(0.85f, 1.15f));
        if(currentAnswer >= bodyPartSprites.Length)
        {
            currentAnswer = 0;
        }
        GetComponent<SpriteRenderer>().sprite = bodyPartSprites[currentAnswer];
        elderYogaGame.SendAnswer(index, currentAnswer);

        elderYogaGame.CheckGame("test");
    }

    public void StartGameElder()
    {
        bodyAnswerSprite.sprite = bodyPartSprites[elderController.correctAnswer[index]];
        canInteract = true;
    }

    public void ResetInteractable()
    {
        throw new System.NotImplementedException();
    }


}
