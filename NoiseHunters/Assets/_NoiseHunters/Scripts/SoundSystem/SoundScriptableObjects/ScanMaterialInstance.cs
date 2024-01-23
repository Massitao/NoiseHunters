using UnityEngine;

[CreateAssetMenu(fileName = "New Scan Material Instance", menuName = "NoiseHunters/Scanner Material Instance")]
public class ScanMaterialInstance : ScriptableObject
{
    [Header("Scanner Texture")]
    [SerializeField] private bool m_scanEnableHorizontalBars;
    public bool Scanner_EnableHorizontalBars
    {
        get { return m_scanEnableHorizontalBars; }
    }

    [SerializeField] private Texture m_scanTexture;
    public Texture Scanner_Texture
    {
        get { return m_scanTexture; }
    }



    [Header("Scanner Colors")]
    [SerializeField] private bool m_scanCustomSoundColors;
    public bool Scanner_CustomSoundColors
    {
        get { return m_scanCustomSoundColors; }
    }

    [SerializeField] private Color m_scanLeadingEdgeColor;
    public Color Scanner_LeadingEdgeColor
    {
        get { return m_scanLeadingEdgeColor; }
    }

    [SerializeField] private Color m_scanMidColor;
    public Color Scanner_MidColor
    {
        get { return m_scanMidColor; }
    }

    [SerializeField] private Color m_scanTrailColor;
    public Color Scanner_TrailColor
    {
        get { return m_scanTrailColor; }
    }


    [Space(10)]


    [SerializeField] private bool m_scanCustomTextureColor;
    public bool Scanner_CustomTextureColor
    {
        get { return m_scanCustomTextureColor; }
    }

    [SerializeField] private Color m_scanTextureColor;
    public Color Scanner_TextureColor
    {
        get { return m_scanTextureColor; }
    }
}