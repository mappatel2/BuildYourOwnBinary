using System.Collections.Generic;
using UnityEngine;

public class GridBoxHandler : MonoBehaviour
{
    private static float s_GridAdjacentDistance = 2;
    
    [SerializeField] private GameObject m_GridBoxBinaryPanel;
    [SerializeField] private GameObject m_CurrentBoxIndicator;
    
    [Space(4)]
    [SerializeField] private int m_GridSize;
    [SerializeField] private GameObject m_GridBoxPrefab;
    [SerializeField] private Transform m_GridStartTF;
    [SerializeField] private Transform m_GridBoxParent;
    
    private List<GridBox> m_GridBoxes = new List<GridBox>();
    
    private Dictionary<int, GridBox> m_DecimalGridBoxDict = new Dictionary<int, GridBox>();
    private Dictionary<string, GridBox> m_RowColumnGridBoxDict = new Dictionary<string, GridBox>();
    
    public GridBox CurrentGridBox { get; private set; }
    
    private void OnEnable()
    {
        GameManager.OnGameStateChange += OnGameStateChange;
    }

    private void Start()
    {
        m_CurrentBoxIndicator.transform.position = new Vector3(100f, 100f, -2f);
        
        if (m_GridBoxes.Count == 0)
        {
            if (m_GridBoxParent.childCount == 0)
            {
                Debug.LogError("No Grid Present");
            }
            else
            {
                for (int i = 0; i < m_GridBoxParent.childCount; i++)
                {
                    GameObject gridBoxGo = m_GridBoxParent.GetChild(i).gameObject;
                    GridBox gridBox = gridBoxGo.GetComponent<GridBox>();
                    if (!m_GridBoxes.Contains(gridBox))
                    {
                        m_GridBoxes.Add(gridBox);
                    }
                }
            }
        }

        PopulateDictionary();
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= OnGameStateChange;
    }

    private void PopulateDictionary()
    {
        int decimalNumber = 0;
        for (int i = 0; i < m_GridBoxes.Count; i++)
        {
            if (m_DecimalGridBoxDict.ContainsKey(decimalNumber))
            {
                continue;
            }
            
            m_DecimalGridBoxDict.Add(decimalNumber, m_GridBoxes[i]);
            
            int row = m_GridBoxes[i].GetRowNumber();
            int column = m_GridBoxes[i].GetColumnNumber();
            string rowColumnKey = $"{row}_{column}";

            if (m_RowColumnGridBoxDict.ContainsKey(rowColumnKey))
            {
                continue;
            }
            m_RowColumnGridBoxDict.Add(rowColumnKey, m_GridBoxes[i]);

            decimalNumber++;
        }
    }
    
    private void OnGameStateChange(GameState currentGameState)
    {
        if (currentGameState == GameState.StartBoxSelection)
        {
            m_GridBoxBinaryPanel.SetActive(true);
        }
        else
        {
            m_GridBoxBinaryPanel.SetActive(false);
        }
    }

    public bool SetCurrentGridBox(int decimalNumber)
    {
        if (decimalNumber > 15)
        {
            Debug.LogError("Please Enter Number Below 15");
            return false;
        }

        GridBox startGridBox = GetGridBoxForDecimalNumber(decimalNumber);
        if (startGridBox)
        {
            CurrentGridBox = startGridBox;
            
            Vector3 currentGridBoxPosition = m_CurrentBoxIndicator.transform.position;
            currentGridBoxPosition.x = CurrentGridBox.transform.position.x;
            currentGridBoxPosition.y = CurrentGridBox.transform.position.y;
            m_CurrentBoxIndicator.transform.position = currentGridBoxPosition;
            return true;
        }

        return false;
    }

    public GridBox GetGridBoxFromLineDirection(int rowIncrement, int columnIncrement)
    {
        int currentRow = CurrentGridBox.GetRowNumber();
        int currentColumn = CurrentGridBox.GetColumnNumber();

        int nextRow = currentRow + rowIncrement;
        int nextColumn = currentColumn + columnIncrement;

        if (nextRow < 0 || nextRow > m_GridSize)
        {
            return null;
        }

        if (nextColumn < 0 || nextColumn > m_GridSize)
        {
            return null;
        }

        string rowColumnKey = $"{nextRow}_{nextColumn}";

        if (!m_RowColumnGridBoxDict.ContainsKey(rowColumnKey))
        {
            return null;
        }
        
        return m_RowColumnGridBoxDict[rowColumnKey];
    }
    
    private GridBox GetGridBoxForDecimalNumber(int decimalNumber)
    {
        if (m_GridBoxParent.childCount <= 0)
        {
            Debug.LogWarning("No Grid Boxes Generated To Return A Box");
            return null;
        }
        
        if (!m_DecimalGridBoxDict.ContainsKey(decimalNumber))
        {
            Debug.LogError($"Please Input Again, Grid Position Not Present For Decimal Number {decimalNumber}");
            return null;
        }

        return m_DecimalGridBoxDict[decimalNumber];
    }

    public void EnableGridBoxes()
    {
        if (m_GridBoxes.Count == 0)
        {
            Debug.LogError("No Grid Boxes Found to Enable");
            return;
        }

        bool currentEnableStatus = m_GridBoxes[0].GetComponent<SpriteRenderer>().enabled;
        currentEnableStatus = !currentEnableStatus;

        for (int i = 0; i < m_GridBoxes.Count; i++)
        {
            m_GridBoxes[i].GetComponent<SpriteRenderer>().enabled = currentEnableStatus;
        }
    }

#if UNITY_EDITOR
    
    [Space(32)]
    [SerializeField] private int m_DecimalForGridStart;

    [SerializeField] private string m_BinaryStr;

    [ContextMenu("Get Grid Box From Decimal Number")]
    public void GetGridBoxForDecimalNumber()
    {
        GetGridBoxForDecimalNumber(m_DecimalForGridStart);
    }

    [ContextMenu("Remove Grid Boxes")]
    public void RemoveGridBoxes()
    {
        if (m_GridBoxParent.childCount <= 0)
        {
            return;
        }

        int loopBreaker = 3000;
        while (m_GridBoxParent.childCount != 0 && loopBreaker > 0) 
        {
            GameObject gridBoxToRemove = m_GridBoxParent.GetChild(0).gameObject;
            DestroyImmediate(gridBoxToRemove);
            loopBreaker--;
        }

        if (loopBreaker == 0)
        {
            Debug.LogError("Possible Infinite Loop, Please Check");
            return;
        }
        
        m_GridBoxes.Clear();
        m_DecimalGridBoxDict.Clear();
    }

    [ContextMenu("Generate Grid Boxes")]
    public void GenerateGridBoxes()
    {
        if (m_GridBoxParent.childCount > 0)
        {
            Debug.Log("Clear Grid Boxes First Before Generating New Boxes");
            return;
        }
        
        float currentXPosition;
        float currentYPosition = m_GridStartTF.position.y;
        int decimalNumber = 0;

        for (int i = 0; i < m_GridSize; i++)
        {
            currentXPosition = m_GridStartTF.position.x;
            
            for (int j = 0; j < m_GridSize; j++)
            {
                GameObject gridBoxGO = Instantiate(m_GridBoxPrefab, m_GridBoxParent);
                gridBoxGO.name = $"{i + 1}_{j + 1}";
                
                Transform gridBoxTF = gridBoxGO.transform;
                GridBox gridBox = gridBoxGO.GetComponent<GridBox>();
                
                gridBox.SetGridBoxData(i+1, j+1, decimalNumber);
                gridBoxTF.position = new Vector2(currentXPosition, currentYPosition);

                decimalNumber++;

                currentXPosition += s_GridAdjacentDistance;
            }

            currentYPosition -= s_GridAdjacentDistance;;
        }
    }
    
#endif
}
