using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class GameUIManager : MonoBehaviour
{
    [SerializeField] private Slider healthSlider;
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private GameManager gameManager;

    //private Character character;
    //private Health health;

    private enum GAME_UI_STATE
    {
        GAMEPLAY, PAUSE, GAMEOVER, GAME_IS_FINISHED
    }

    private void Start()
    {
        SwitchUIState(GAME_UI_STATE.GAMEPLAY);
    }

    private GAME_UI_STATE currentState;

    public GameObject UI_Pause;
    public GameObject UI_GameOver;
    public GameObject UI_GameIsFinished;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        healthSlider.value = gameManager.playerCharacter.health.currentHealthPercentage;

        coinText.text = gameManager.playerCharacter.coin.ToString();
    }

    private void SwitchUIState(GAME_UI_STATE state)
    {
        UI_Pause.SetActive(false);
        UI_GameOver.SetActive(false);
        UI_GameIsFinished.SetActive(false);

        Time.timeScale = 1;

        switch (state)
        {
            case GAME_UI_STATE.GAMEPLAY:
                break;

            case GAME_UI_STATE.PAUSE:
                Time.timeScale = 0;
                UI_Pause.SetActive(true);
                break;

            case GAME_UI_STATE.GAMEOVER:
                UI_GameOver.SetActive(true);
                break;

            case GAME_UI_STATE.GAME_IS_FINISHED:
                UI_GameIsFinished.SetActive(true);
                break;
        }

        currentState = state;
    }

    public void TogglePauseUI()
    {
        if (currentState == GAME_UI_STATE.GAMEPLAY)
            SwitchUIState(GAME_UI_STATE.PAUSE);
        else if (currentState == GAME_UI_STATE.PAUSE)
            SwitchUIState(GAME_UI_STATE.GAMEPLAY);

    }

    public void Button_Restart()
    {
        gameManager.Restart();
    }

    public void Button_MainMenu()
    {
        gameManager.ReturnToMainMenu();
    }

    public void Button_Pause()
    {
        TogglePauseUI();
    }

    public void ShowGameOver_UI()
    {
        SwitchUIState(GAME_UI_STATE.GAMEOVER);
    }

    public void ShowGameIsFinishedUI()
    {
        SwitchUIState(GAME_UI_STATE.GAME_IS_FINISHED);
    }
}
