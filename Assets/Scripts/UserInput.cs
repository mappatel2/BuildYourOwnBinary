using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UserInput : MonoBehaviour
{
    [SerializeField] private Button m_ValidateBtn;
    [SerializeField] private Button m_ResetCurrentShapeBtn;
    [SerializeField] private Button m_LoadNextShapeBtn;
    [SerializeField] private Button m_EnableGridBtn;
    [SerializeField] private Button m_QuitGameBtn;
    [SerializeField] private TMP_InputField m_InputField;
    [SerializeField] private LineGenerator m_LineGenerator;
    [SerializeField] private GridBoxHandler m_GridBoxHandler;

    private void OnEnable()
    {
        m_ValidateBtn.onClick.AddListener(ValidateUserInput);
        m_ResetCurrentShapeBtn.onClick.AddListener(ResetCurrentShape);
        m_LoadNextShapeBtn.onClick.AddListener(LoadNextShape);
        m_QuitGameBtn.onClick.AddListener(QuitGame);
        m_EnableGridBtn.onClick.AddListener(EnableGridBoxes);
    }

    private void Start()
    {
        m_InputField.text = "";
    }

    private void OnDisable()
    {
        m_ValidateBtn.onClick.RemoveListener(ValidateUserInput);
        m_ResetCurrentShapeBtn.onClick.RemoveListener(ResetCurrentShape);
        m_LoadNextShapeBtn.onClick.RemoveListener(LoadNextShape);
        m_QuitGameBtn.onClick.RemoveListener(QuitGame);
        m_EnableGridBtn.onClick.RemoveListener(EnableGridBoxes);
    }

    private void ValidateUserInput()
    {
        DisableValidateBtn();
        
        m_ValidateBtn.enabled = false;
        
        string inputFieldStr = m_InputField.text;
        int decimalNumber = BinaryToDecimal(inputFieldStr);
        if (decimalNumber == -1)
        {
            Invoke(nameof(EnableValidateBtn), 0.2f);
            return;
        }

        switch (GameManager.Instance.CurrentGameState)
        {
            case GameState.StartBoxSelection:
                bool isBoxFound = m_GridBoxHandler.SetCurrentGridBox(decimalNumber);
                m_InputField.text = "";
                Invoke(nameof(EnableValidateBtn), 0.2f);
                if (isBoxFound)
                {
                    GameManager.Instance.SwitchGameState();
                }
                break;
            
            case GameState.LineDirectionSelection:
                bool hasLineExtended = m_LineGenerator.ExtendLine(decimalNumber);
                float waitTime = hasLineExtended ? 0.15f : 0.2f;
                m_InputField.text = "";
                if (hasLineExtended)
                {
                    GameManager.Instance.UpdateMovesCount();
                }
                Invoke(nameof(EnableValidateBtn), waitTime);
                break;
            
            case GameState.Completion:
            default:
                return;
        }
    }
    
    private int BinaryToDecimal(string binaryStr)
    {
        if (string.IsNullOrEmpty(binaryStr))
        {
            return -1;
        }

        int decimalValue = 0;
        int power = 0;
        
        for (int i = binaryStr.Length - 1; i >= 0; i--)
        {
            if (binaryStr[i] == '1')
            {
                decimalValue += (int)Mathf.Pow(2, power);
            }
            else if (binaryStr[i] == '0')
            {
                
            }
            else
            {
                Debug.LogError("You have probably input a character instead of 0 or 1, Please Try Again");
                return -1;
            }

            power++;
        }

        return decimalValue;
    }

    private void EnableValidateBtn()
    {
        m_ValidateBtn.enabled = true;
    }

    private void DisableValidateBtn()
    {
        m_ValidateBtn.enabled = false;
    }

    private void EnableGridBoxes()
    {
        m_GridBoxHandler.EnableGridBoxes();
    }

    private void ResetCurrentShape()
    {
        GameManager.Instance.ReloadCurrentShape();
    }

    private void LoadNextShape()
    {
        GameManager.Instance.LoadNextShape();
    }
    
    private void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }
    
#if UNITY_EDITOR
    
    [Space(32)]
    [SerializeField] private string m_BinaryStr;

    [ContextMenu("Test Binary To Decimal")]
    public void TestBinaryToDecimal()
    {
        int decimalNumber = BinaryToDecimal(m_BinaryStr);
        if (decimalNumber == -1)
        {
            Debug.LogError("There was some Issue");
        }
        else
        {
            Debug.Log($"Decimal Representation of Current Binary Number Is : {decimalNumber}");
        }
    }
    
#endif
}
