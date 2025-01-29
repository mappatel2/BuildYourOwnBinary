using UnityEngine;

[System.Serializable]
public class GridBoxData
{
    public int rowNumber;
    public int columnNumber;
    public int decimalNumber;
}

public class GridBox : MonoBehaviour
{
    [SerializeField] private GridBoxData m_GridBoxData;

    public int GetRowNumber()
    {
        return m_GridBoxData.rowNumber;
    }

    public int GetColumnNumber()
    {
        return m_GridBoxData.columnNumber;
    }

    public int GetDecimalNumber()
    {
        return m_GridBoxData.decimalNumber;
    }
    
#if UNITY_EDITOR

    public void SetGridBoxData(int rowNumber, int columnNumber, int decimalNumber)
    {
        m_GridBoxData.columnNumber = columnNumber;
        m_GridBoxData.rowNumber = rowNumber;
        m_GridBoxData.decimalNumber = decimalNumber;
    }
    
#endif
}
