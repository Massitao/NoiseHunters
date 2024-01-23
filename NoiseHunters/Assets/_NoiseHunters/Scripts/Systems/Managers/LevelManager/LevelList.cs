using System.Collections.Generic;
using UnityEngine;

public class LevelList : MonoBehaviour
{
    // All Noise Hunters Scenes - If there's a scene Missing, write new strings.
    public static string Intro_Scene;
    public static string MainMenu_Scene = "NH_MainMenu";
    public static string Loading_Scene = "LoadScene";
    public static string Credits_Scene = "__Credits";

    public static string FirstCinematic_Scene = "FirstCinematic";
    public static string SecondCinematic_Scene = "SecondCinematic";
    public static string ThirdCinematic_Scene = "ThirdCinematic";

    public static string HopeCinematic_Scene = "_HopeCinematic";
    public static string RequestCinematic_Scene = "_RequestCinematic";
    public static string FailureCinematic_Scene = "_FailureCinematic";

    public static string Stage1_1_Scene = "Stage1.1";
    public static string Stage1_2_Scene = "Stage1.2";
    public static string Stage1_3_Scene = "Stage1.3";
    public static string Stage1_4_Scene = "Stage1.4";

    public static string Stage2_1_Scene = "Stage2.1";
    public static string Stage2_2_Scene = "Stage2.2";
    public static string Stage2_3_Scene = "Stage2.3";
    public static string Stage2_4_Scene = "Stage2.4";
    public static string Stage2_5_Scene = "Stage2.5";
    public static string Stage2_6_Scene = "Stage2.6";

    // All Playable Scenes - Important for checkpoint system
    public static List<string> PlayableScene_List = new List<string>
    {
        Stage1_1_Scene, Stage1_2_Scene, Stage1_3_Scene, Stage1_4_Scene,
        Stage2_1_Scene, Stage2_2_Scene, Stage2_3_Scene, Stage2_4_Scene, Stage2_5_Scene, Stage2_6_Scene
    };
    public static List<string> CinematicScene_List = new List<string>
    {
        FirstCinematic_Scene, SecondCinematic_Scene, ThirdCinematic_Scene
    };

    public static string ReturnSceneString(string sceneRef)
    {
        switch (sceneRef)
        {
            case "MainMenu_Scene":
                return MainMenu_Scene;

            case "Stage1_1_Scene":
                return Stage1_1_Scene;

            case "Stage1_2_Scene":
                return Stage1_2_Scene;

            case "Stage1_3_Scene":
                return Stage1_3_Scene;

            case "Stage1_4_Scene":
                return Stage1_4_Scene;

            case "Stage2_1_Scene":
                return Stage2_1_Scene;

            case "Stage2_2_Scene":
                return Stage2_2_Scene;

            case "Stage2_3_Scene":
                return Stage2_3_Scene;

            case "Stage2_4_Scene":
                return Stage2_4_Scene;

            case "Stage2_5_Scene":
                return Stage2_5_Scene;

            case "Stage2_6_Scene":
                return Stage2_6_Scene;


            case "FirstCinematic":
                return FirstCinematic_Scene;

            case "SecondCinematic":
                return SecondCinematic_Scene;

            case "ThirdCinematic":
                return ThirdCinematic_Scene;


            case "Credits":
                return Credits_Scene;


            default:
                return MainMenu_Scene;
        }
    }

    public static bool IsCurrentScenePlayable(string currentLevel)
    {
        return PlayableScene_List.Contains(currentLevel);
    }
    public static bool IsSceneCinematic(string currentLevel)
    {
        return CinematicScene_List.Contains(currentLevel);
    }
}
