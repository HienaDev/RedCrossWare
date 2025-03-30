// First, define the IInteractable interface
using System;
using UnityEngine;
public interface IMicroGame
{
    [Serializable]
    public struct GameAnswers
    {
        public Sprite sprite;
        public Sprite worry;

        public string answer;
    }

    GameObject[] Controls { get; }
    string CatchPhrase { get; }
    bool TimeOverWin { get; }
    float TimeLimit { get; }
    void CheckGame(string answer);
    void StartGame();

    void InitializeGame();
    void WinGame();
    void LoseGame();
    void EndGame();

    void ResetGame();
}