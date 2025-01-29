using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum GameState
{
    StartBoxSelection = 0,
    LineDirectionSelection = 1,
    Completion = 2
}

public class GameManager : MonoBehaviour
{
    [SerializeField] private ImageDB m_ImageDB;
    [SerializeField] private TextMeshProUGUI m_MovesText;
    [SerializeField] private Image m_ShapeImageDescriptor;

    private int m_MovesCount = 0;
    
    public GameState CurrentGameState { get; private set; }
    
    public static event System.Action<GameState> OnGameStateChange;
    
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Sprite shapeSprite = m_ImageDB.GetCurrentImageSprite();
        m_ShapeImageDescriptor.sprite = shapeSprite;
        m_ShapeImageDescriptor.enabled = true;
        
        m_MovesCount = 0;
        m_MovesText.text = $"{m_MovesCount}";
        
        CurrentGameState = GameState.StartBoxSelection;
        
        OnGameStateChange?.Invoke(CurrentGameState);
    }

    public void SwitchGameState()
    {
        CurrentGameState = (GameState)(int)CurrentGameState + 1;
        OnGameStateChange?.Invoke(CurrentGameState);
    }

    public void UpdateMovesCount()
    {
        m_MovesCount++;
        m_MovesText.text = $"{m_MovesCount}";
    }

    public void ReloadCurrentShape()
    {
        m_ShapeImageDescriptor.enabled = false;
        SceneManager.LoadScene(0);
    }

    public void LoadNextShape()
    {
        m_ShapeImageDescriptor.enabled = false;
        m_ImageDB.UpdateCurrentImageSprite();
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
