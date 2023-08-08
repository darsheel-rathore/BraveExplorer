using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private Character playerCharacter;
    private bool gameIsOver;

    private void Awake()
    {
        playerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    private void GameOver()
    {
        Debug.Log("Game Over");
    }

    public void GameIsFinished()
    {
        Debug.Log("Game is finished.");
    }


    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
            return;

        if(playerCharacter.currentState == Character.CharacterState.DEAD)
        {
            gameIsOver = true;
            GameOver();
        }
    }
}
