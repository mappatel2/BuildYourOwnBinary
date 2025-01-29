using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LineGenerator : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_MovesText;
    [SerializeField] private GameObject m_LineDirectionBinaryPanel;
    [SerializeField] private GridBoxHandler m_GridBoxHandler;
    [SerializeField] private LineRenderer m_LineRenderer;

    private struct LineIncrements
    {
        public int row;
        public int column;

        public LineIncrements(int row, int column)
        {
            this.row = row;
            this.column = column;
        }
    }
    
    // 0 - Right
    // 1 - Left
    // 2 - Up
    // 3 - Down
    // 4 - Up Right
    // 5 - Up Left
    // 6 - Down Right
    // 7 - Down Left
    private readonly Dictionary<int, LineIncrements> m_DecimalIncrementalDict = new Dictionary<int, LineIncrements>()
    {
        {0, new LineIncrements(0,1)}, {1, new LineIncrements(0,-1)}, 
        {2, new LineIncrements(-1,0)}, {3, new LineIncrements(1,0)}, 
        {4, new LineIncrements(-1,1)}, {5, new LineIncrements(-1,-1)}, 
        {6, new LineIncrements(1,1)}, {7, new LineIncrements(1,-1)}
    };

    private int m_MovesCount = 0;

    private void OnEnable()
    {
        GameManager.OnGameStateChange += OnGameStateChange;
    }

    private void Start()
    {
        m_LineRenderer.enabled = false;
    }

    private void OnDisable()
    {
        GameManager.OnGameStateChange -= OnGameStateChange;
    }

    public bool ExtendLine(int decimalNumber)
    {
        if (decimalNumber > 7)
        {
            Debug.LogError("Please Enter Number In Given Range");
            return false;
        }

        if (!m_DecimalIncrementalDict.ContainsKey(decimalNumber))
        {
            Debug.LogError("Line Direction For This Decimal Number Not Present In Dictionary Please Check");
            return false;
        }

        int rowIncrement = m_DecimalIncrementalDict[decimalNumber].row;
        int columnIncrement = m_DecimalIncrementalDict[decimalNumber].column;

        GridBox currentGridBox = m_GridBoxHandler.CurrentGridBox;
        GridBox nextGridBox = m_GridBoxHandler.GetGridBoxFromLineDirection(rowIncrement, columnIncrement);

        if (!nextGridBox)
        {
            return false;
        }

        StartCoroutine(DrawLine(currentGridBox, nextGridBox, 0.1f));
        
        return true;
    }

    private void OnGameStateChange(GameState currentGameState)
    {
        if (currentGameState == GameState.LineDirectionSelection)
        {
            m_LineDirectionBinaryPanel.SetActive(true);
            SetStartPointForLine();
        }
        else if(currentGameState == GameState.Completion)
        {
            m_LineDirectionBinaryPanel.SetActive(false);
        }
    }

    private void SetStartPointForLine()
    {
        m_LineRenderer.enabled = true;
        GridBox currentGridBox = m_GridBoxHandler.CurrentGridBox;
        m_LineRenderer.positionCount = 1;
        m_LineRenderer.SetPosition(0, currentGridBox.transform.position);
    }

    private IEnumerator DrawLine(GridBox currentGridBox, GridBox nextGridBox, float timeDuration, float waitTime = 0f)
    {
        if (waitTime > 0f)
        {
            yield return new WaitForSeconds(waitTime);
        }

        Vector3 startPoint = currentGridBox.transform.position;
        Vector3 endPoint = nextGridBox.transform.position;
        
        m_LineRenderer.positionCount++;
        int lastPositionIndex = m_LineRenderer.positionCount - 1;

        float timeElapsed = 0f;
        while (timeElapsed <= timeDuration)
        {
            Vector3 currentPoint = Vector3.Lerp(startPoint, endPoint, timeElapsed / timeDuration);
            timeElapsed += Time.deltaTime;
            m_LineRenderer.SetPosition(lastPositionIndex, currentPoint);
            yield return null;
        }
        
        m_LineRenderer.SetPosition(lastPositionIndex, endPoint);
        m_GridBoxHandler.SetCurrentGridBox(nextGridBox.GetDecimalNumber());
    }
}
