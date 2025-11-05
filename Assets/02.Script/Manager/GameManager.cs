using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    public enum GameState { GameReady, Ongoing, GameClear, GameOver }

    public static GameManager instance { get; private set; }
    public GameState globalGameState { get; private set; }


    [SerializeField] private PlayerController player;


    private void Awake()
    {
        if(instance == null) instance = this;
        else Destroy(gameObject);
    }
    private void Start()
    {
        Time.timeScale = 0f;
        SetState(GameState.GameReady);
        SoundManager.instance.intro.Play();
    }

    private void Update()
    {
        switch (globalGameState)
        {
            case GameState.GameReady:
                if (Input.GetMouseButtonDown(0))
                {
                    SoundManager.instance.intro.Stop();

                    SetState(GameState.Ongoing);
                    Time.timeScale = 1f;
                    SoundManager.instance.BGM.Play();

                }
                break;

            case GameState.Ongoing:

                if (player != null && player.isDead)
                {
                    SetState(GameState.GameOver);
                    Time.timeScale = 0f;
                }
                break;
            case GameState.GameClear:
                RestartGame();
                break;

            case GameState.GameOver:
                RestartGame();
                break;
        }
    }
    public void SetState(GameState newState)
    {

        globalGameState = newState;


        switch (newState)
        {
            case GameState.GameReady:
                UIManager.Instance.ShowGameStartUI();
                UIManager.Instance.HideGameOverUI();
                UIManager.Instance.HideGameClearUI();
                break;

            case GameState.Ongoing:
                UIManager.Instance.HideGameStartUI();
                UIManager.Instance.HideGameOverUI();
                UIManager.Instance.HideGameClearUI();

                break;
            case GameState.GameClear:
                SoundManager.instance.BGM.Stop();
                SoundManager.instance.GameClear.Play();
                UIManager.Instance.HideGameStartUI();
                UIManager.Instance.HideGameOverUI();
                UIManager.Instance.ShowGameClearUI();
                break;

            case GameState.GameOver:
                SoundManager.instance.BGM.Stop();
                UIManager.Instance.HideGameStartUI();
                UIManager.Instance.HideGameClearUI();
                UIManager.Instance.ShowGameOverUI();
                break;


        }
    }

    public void RestartGame()
    {
        if (Input.GetKeyUp(KeyCode.R))
        {
            SoundManager.instance.GameClear.Stop();

            Time.timeScale = 1f;

            SceneManager.LoadScene("SampleScene");
            SetState(GameState.GameReady);
        }
    }
}
