public enum CollectiblesID
{
    Stage1_1_Collectible1, Stage1_1_Collectible2,
    Stage1_2_Collectible1, Stage1_2_Collectible2,
    Stage1_3_Collectible1, Stage1_3_Collectible2, Stage1_3_Collectible3,
    Stage1_4_Collectible1, Stage1_4_Collectible2, Stage1_4_Collectible3,

    Stage2_1_Collectible1, Stage2_1_Collectible2, Stage2_1_Collectible3,
    Stage2_2_Collectible1, Stage2_2_Collectible2, Stage2_2_Collectible3, Stage2_2_Collectible4,
    Stage2_3_Collectible1, Stage2_3_Collectible2, Stage2_3_Collectible3,
    Stage2_4_Collectible1, Stage2_4_Collectible2, Stage2_4_Collectible3,
    Stage2_5_Collectible1, Stage2_5_Collectible2,
    Stage2_6_Collectible1, Stage2_6_Collectible2, Stage2_6_Collectible3, Stage2_6_Collectible4
}

public class CollectibleSet
{
    public static bool CheckIfCollectibleWasPicked(CollectiblesID collectibleID)
    {
        return SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(collectibleID.ToString());
    }

    public static void AddCollectibleToSet(CollectiblesID collectibleID)
    {
        if (!CheckIfCollectibleWasPicked(collectibleID))
        {
            SaveInstance._Instance.AddNewCollectible(collectibleID.ToString());

            switch (collectibleID)
            {
                #region Stage1_1 Collectible
                case CollectiblesID.Stage1_1_Collectible1:
                    CheckAllCollectiblesStage1_1();
                    break;
                case CollectiblesID.Stage1_1_Collectible2:
                    CheckAllCollectiblesStage1_1();
                    break;
                #endregion

                #region Stage1_2 Collectible
                case CollectiblesID.Stage1_2_Collectible1:
                    CheckAllCollectiblesStage1_2();
                    break;
                case CollectiblesID.Stage1_2_Collectible2:
                    CheckAllCollectiblesStage1_2();
                    break;
                #endregion

                #region Stage1_3 Collectible
                case CollectiblesID.Stage1_3_Collectible1:
                    CheckAllCollectiblesStage1_3();
                    break;
                case CollectiblesID.Stage1_3_Collectible2:
                    CheckAllCollectiblesStage1_3();
                    break;
                case CollectiblesID.Stage1_3_Collectible3:
                    CheckAllCollectiblesStage1_3();
                    break;
                #endregion

                #region Stage1_4 Collectible
                case CollectiblesID.Stage1_4_Collectible1:
                    CheckAllCollectiblesStage1_4();
                    break;
                case CollectiblesID.Stage1_4_Collectible2:
                    CheckAllCollectiblesStage1_4();
                    break;
                case CollectiblesID.Stage1_4_Collectible3:
                    CheckAllCollectiblesStage1_4();
                    break;
                #endregion


                #region Stage2_1 Collectible
                case CollectiblesID.Stage2_1_Collectible1:
                    CheckAllCollectiblesStage2_1();
                    break;
                case CollectiblesID.Stage2_1_Collectible2:
                    CheckAllCollectiblesStage2_1();
                    break;
                case CollectiblesID.Stage2_1_Collectible3:
                    CheckAllCollectiblesStage2_1();
                    break;
                #endregion

                #region Stage2_2 Collectible
                case CollectiblesID.Stage2_2_Collectible1:
                    CheckAllCollectiblesStage2_2();
                    break;
                case CollectiblesID.Stage2_2_Collectible2:
                    CheckAllCollectiblesStage2_2();
                    break;
                case CollectiblesID.Stage2_2_Collectible3:
                    CheckAllCollectiblesStage2_2();
                    break;
                case CollectiblesID.Stage2_2_Collectible4:
                    CheckAllCollectiblesStage2_2();
                    break;
                #endregion

                #region Stage2_3 Collectible
                case CollectiblesID.Stage2_3_Collectible1:
                    CheckAllCollectiblesStage2_3();
                    break;
                case CollectiblesID.Stage2_3_Collectible2:
                    CheckAllCollectiblesStage2_3();
                    break;
                case CollectiblesID.Stage2_3_Collectible3:
                    CheckAllCollectiblesStage2_3();
                    break;
                #endregion

                #region Stage2_4 Collectible
                case CollectiblesID.Stage2_4_Collectible1:
                    CheckAllCollectiblesStage2_4();
                    break;
                case CollectiblesID.Stage2_4_Collectible2:
                    CheckAllCollectiblesStage2_4();
                    break;
                case CollectiblesID.Stage2_4_Collectible3:
                    CheckAllCollectiblesStage2_4();
                    break;
                #endregion

                #region Stage2_5 Collectible
                case CollectiblesID.Stage2_5_Collectible1:
                    CheckAllCollectiblesStage2_5();
                    break;
                case CollectiblesID.Stage2_5_Collectible2:
                    CheckAllCollectiblesStage2_5();
                    break;
                #endregion

                #region Stage2_6 Collectible
                case CollectiblesID.Stage2_6_Collectible1:
                    CheckAllCollectiblesStage2_6();
                    break;
                case CollectiblesID.Stage2_6_Collectible2:
                    CheckAllCollectiblesStage2_6();
                    break;
                case CollectiblesID.Stage2_6_Collectible3:
                    CheckAllCollectiblesStage2_6();
                    break;
                case CollectiblesID.Stage2_6_Collectible4:
                    CheckAllCollectiblesStage2_6();
                    break;
                #endregion
            }
        }
    }

    private static void CheckAllCollectiblesStage1_1()
    {
        if (SaveInstance._Instance != null)
        {
            if (SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage1_1_Collectible1.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage1_1_Collectible2.ToString()))
            {
                SteamManager._Instance?.Stage1_1_Collected();
            }
        }      
    }
    private static void CheckAllCollectiblesStage1_2()
    {
        if (SaveInstance._Instance != null)
        {
            if (SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage1_2_Collectible1.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage1_2_Collectible2.ToString()))
            {
                SteamManager._Instance?.Stage1_2_Collected();
            }
        }      
    }
    private static void CheckAllCollectiblesStage1_3()
    {
        if (SaveInstance._Instance != null)
        {
            if (SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage1_3_Collectible1.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage1_3_Collectible2.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage1_3_Collectible3.ToString()))
            {
                SteamManager._Instance?.Stage1_3_Collected();
            }
        }      
    }
    private static void CheckAllCollectiblesStage1_4()
    {
        if (SaveInstance._Instance != null)
        {
            if (SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage1_4_Collectible1.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage1_4_Collectible2.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage1_4_Collectible3.ToString()))
            {
                SteamManager._Instance?.Stage1_4_Collected();
            }
        }      
    }

    private static void CheckAllCollectiblesStage2_1()
    {
        if (SaveInstance._Instance != null)
        {
            if (SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_1_Collectible1.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_1_Collectible2.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_1_Collectible3.ToString()))
            {
                SteamManager._Instance?.Stage2_1_Collected();
            }
        }      
    }
    private static void CheckAllCollectiblesStage2_2()
    {
        if (SaveInstance._Instance != null)
        {
            if (SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_2_Collectible1.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_2_Collectible2.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_2_Collectible3.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_2_Collectible4.ToString()))
            {
                SteamManager._Instance?.Stage2_2_Collected();
            }
        }      
    }
    private static void CheckAllCollectiblesStage2_3()
    {
        if (SaveInstance._Instance != null)
        {
            if (SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_3_Collectible1.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_3_Collectible2.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_3_Collectible3.ToString()))
            {
                SteamManager._Instance?.Stage2_3_Collected();
            }
        }      
    }
    private static void CheckAllCollectiblesStage2_4()
    {
        if (SaveInstance._Instance != null)
        {
            if (SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_4_Collectible1.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_4_Collectible2.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_4_Collectible3.ToString()))
            {
                SteamManager._Instance?.Stage2_4_Collected();
            }
        }      
    }
    private static void CheckAllCollectiblesStage2_5()
    {
        if (SaveInstance._Instance != null)
        {
            if (SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_5_Collectible1.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_5_Collectible2.ToString()))
            {
                SteamManager._Instance?.Stage2_5_Collected();
            }
        }      
    }
    private static void CheckAllCollectiblesStage2_6()
    {
        if (SaveInstance._Instance != null)
        {
            if (SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_6_Collectible1.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_6_Collectible2.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_6_Collectible3.ToString()) &&
                SaveInstance._Instance.currentLoadedSave.collectiblesPicked.Contains(CollectiblesID.Stage2_6_Collectible4.ToString()))
            {
                SteamManager._Instance?.Stage2_6_Collected();
            }
        }      
    }
}