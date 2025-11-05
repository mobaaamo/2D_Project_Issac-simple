using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Panels")]
    [SerializeField] private GameObject gameStartUI;
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private GameObject gameClearUI;
    private float gameOverDelay = 2.5f;

    GameManager.GameState gameStateCache;

    private void Awake()
    {
            Instance = this;

    }

    public void ShowGameStartUI()
    {
        if (gameStartUI != null) gameStartUI.SetActive(true);
    }
    public void HideGameStartUI()
    {
        if(gameStartUI != null)  gameStartUI.SetActive(false); 
    }

    public void ShowGameOverUI() 
    {
        StartCoroutine(ShowGameOverImagesWithDelay());
    }
    public void HideGameOverUI()
    {
        if(gameOverUI !=null) gameOverUI.SetActive(false);
    }
    public void ShowGameClearUI()
    {
        StartCoroutine(ShowGameClearImageWithDelay());
    }

    public void HideGameClearUI()
    {
        if(gameClearUI !=null) gameClearUI.SetActive(false);
    }

    private IEnumerator ShowGameOverImagesWithDelay()
    {
        HideGameOverUI();
        yield return new WaitForSecondsRealtime(gameOverDelay);
        if (gameOverUI != null) gameOverUI.SetActive(true);

    }

    private IEnumerator ShowGameClearImageWithDelay()
    {
        HideGameClearUI();
        yield return new WaitForSecondsRealtime(gameOverDelay);
        if(gameClearUI != null) gameClearUI.SetActive(true) ;
    }
}

