using UnityEngine;

public class UtilityTests2 : MonoBehaviour
{
    [ContextMenu("Save Unlock")]
    public void SaveUnlock()
    {
        SaveInstance._Instance.SetNewScene("Nier", LevelList.FirstCinematic_Scene);
        SaveInstance._Instance.SetNewScene("Nier", LevelList.SecondCinematic_Scene);
        SaveInstance._Instance.SetNewScene("Nier", LevelList.Stage1_2_Scene);
        SaveInstance._Instance.SetNewScene("Nier", LevelList.Stage1_3_Scene);
        SaveInstance._Instance.SetNewScene("Nier", LevelList.Stage1_4_Scene);

        SaveInstance._Instance.AddNewCollectible(CollectiblesID.Stage1_1_Collectible1.ToString());
        SaveInstance._Instance.AddNewCollectible(CollectiblesID.Stage1_1_Collectible2.ToString());
        SaveInstance._Instance.AddNewCollectible(CollectiblesID.Stage1_2_Collectible1.ToString());
        SaveInstance._Instance.AddNewCollectible(CollectiblesID.Stage1_2_Collectible2.ToString());
        SaveInstance._Instance.AddNewCollectible(CollectiblesID.Stage1_3_Collectible1.ToString());
        SaveInstance._Instance.AddNewCollectible(CollectiblesID.Stage1_3_Collectible2.ToString());
        SaveInstance._Instance.AddNewCollectible(CollectiblesID.Stage1_3_Collectible3.ToString());
    }
}