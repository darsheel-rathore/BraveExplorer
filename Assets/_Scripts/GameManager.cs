using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Character playerCharacter;
    public GameUIManager gameUIManager;
    private bool gameIsOver;

    private void Awake()
    {
        playerCharacter = GameObject.FindWithTag("Player").GetComponent<Character>();
        gameUIManager = FindObjectOfType<GameUIManager>();
    }

    private void GameOver()
    {
        gameUIManager.ShowGameOver_UI();
    }

    public void GameIsFinished()
    {
        gameUIManager.ShowGameIsFinishedUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsOver)
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            gameUIManager.TogglePauseUI();
        }

        if (playerCharacter.currentState == Character.CharacterState.DEAD)
        {
            gameIsOver = true;
            GameOver();
        }
    }

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
