using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/ImageDB", fileName = "ImageDB")]
public class ImageDB : ScriptableObject
{
    [SerializeField] private Sprite[] m_ImageSpritesDB;
    private int m_CurrentImageIndex = 0;

    public Sprite GetCurrentImageSprite()
    {
        if (m_CurrentImageIndex == -1)
        {
            m_CurrentImageIndex = 0;
        }
        
        if (m_ImageSpritesDB.Length == 0)
        {
            Debug.LogError("No Images Found in the DB");
            return null;
        }
        
        return m_ImageSpritesDB[m_CurrentImageIndex];
    }

    public void UpdateCurrentImageSprite()
    {
        if (m_ImageSpritesDB.Length == 0)
        {
            Debug.LogError("No Images Found in the DB");
            return;
        }
        
        m_CurrentImageIndex++;
        if (m_CurrentImageIndex >= m_ImageSpritesDB.Length)
        {
            m_CurrentImageIndex = 0;
        }
    }
}
