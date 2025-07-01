using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneReloader : MonoBehaviour
{
    [SerializeField] private string _gameSceneName = "GameScene";
    public static SceneReloader Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        GameEvents.OnStartNewGame += ReloadGameScene;
    }
    private void OnDisable()
    {
        GameEvents.OnStartNewGame -= ReloadGameScene;
    }

    public void ReloadGameScene()
    {
        SceneManager.LoadScene(_gameSceneName);
    }
}
