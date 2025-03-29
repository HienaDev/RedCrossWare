using UnityEditor;
using UnityEngine;

public class ElderController : MonoBehaviour
{
    public ElderBodyPartInteractable[] bodyParts;
    public ElderYogaGame game;
    public int[] correctAnswer;


    public void Initialize(int answer0, int answer1, int answer2, int answer3, ElderYogaGame game)
    {
        this.game = game;
        foreach (var bodyPart in bodyParts)
        {
            bodyPart.Initialize(game);
        }

        correctAnswer = new int[] { answer0, answer1, answer2, answer3};
    }

    public void StartElderGame()
    {
        foreach (var bodyPart in bodyParts)
        {
            bodyPart.StartGameElder();
        }

    }

}
