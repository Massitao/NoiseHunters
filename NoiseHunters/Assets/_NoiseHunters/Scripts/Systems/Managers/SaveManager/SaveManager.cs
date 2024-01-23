using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    private static readonly string path = $"{Application.persistentDataPath}/memories/";
    private static readonly string playerSave = $"playerlog.hunt";
    private static readonly string playerConfig = $"userconfig.ini";
    private static readonly string gameVersion = $"NoiseHunters_Patch2";


    public static void SavePlayerData(PlayerSave playerSaveToSave)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        FileStream file = File.Create($"{path}{playerSave}");
        formatter.Serialize(file, playerSaveToSave);
        file.Close();
    }
    public static PlayerSave LoadPlayerData()
    {
        if (!File.Exists($"{path}{playerSave}"))
        {
            return ResetPlayerData();
        }

        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream file = File.Open($"{path}{playerSave}", FileMode.Open);

        try
        {
            PlayerSave save = (PlayerSave)formatter.Deserialize(file);
            file.Close();
            return save;
        }
        catch
        {
            Debug.LogError($"Can't load {path}{playerSave}");
            file.Close();
            return null;
        }
    }

    public static void SavePlayerConfig(UserSave playerConfigToSave)
    {
        BinaryFormatter formatter = GetBinaryFormatter();

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }

        FileStream file = File.Create($"{path}{playerConfig}");
        formatter.Serialize(file, playerConfigToSave);
        file.Close();
    }
    public static UserSave LoadPlayerConfig()
    {
        if (!File.Exists($"{path}{playerConfig}"))
        {
            return ResetPlayerConfig();
        }

        BinaryFormatter formatter = GetBinaryFormatter();
        FileStream file = File.Open($"{path}{playerConfig}", FileMode.Open);

        try
        {
            UserSave save = (UserSave)formatter.Deserialize(file);

            if (!CheckConfigVersionDifference(save))
            {
                Debug.Log("Change");
                file.Close();
                return ResetPlayerConfig();
            }

            file.Close();
            return save;
        }
        catch
        {
            Debug.LogError($"Can't load {path}{playerConfig}");
            file.Close();
            return null;
        }
    }
    

    public static bool CheckConfigVersionDifference(UserSave userConfig)
    {
        return (userConfig.saveVersion == gameVersion);
    }

    public static PlayerSave ResetPlayerData()
    {
        PlayerSave newDefaultPlayerSave = new PlayerSave();

        SavePlayerData(newDefaultPlayerSave);
        return newDefaultPlayerSave;
    }
    public static UserSave ResetPlayerConfig()
    {
        UserSave newDefaultUserSave = new UserSave();

        newDefaultUserSave.saveVersion = gameVersion;
        newDefaultUserSave.userResolution = Screen.currentResolution;
        newDefaultUserSave.userWindowedMode = false;
        newDefaultUserSave.userLanguage = GameLanguages.ENG;
        newDefaultUserSave.userMusicVolume = 0.75f;
        newDefaultUserSave.userSoundVolume = 0.75f;
        newDefaultUserSave.userSensibility = 0.5f;
        newDefaultUserSave.userAimingSensibility = 0.5f;
        newDefaultUserSave.userWaveBrightness = BrightnessEnum.Standard;

        SavePlayerConfig(newDefaultUserSave);
        return newDefaultUserSave;
    }

    public static void DeletePlayerData()
    {
        if (!Directory.Exists(path))
        {
            return;
        }

        PlayerPrefs.DeleteAll();

        DirectoryInfo directory = new DirectoryInfo(path);
        if (File.Exists($"{path}{playerSave}"))
        {
            File.Delete($"{path}{playerSave}");
        }
    }
    public static void DeletePlayerConfig()
    {
        if (!Directory.Exists(path))
        {
            return;
        }

        PlayerPrefs.DeleteAll();

        DirectoryInfo directory = new DirectoryInfo(path);
        if (File.Exists($"{path}{playerConfig}"))
        {
            File.Delete($"{path}{playerConfig}");
        }
    }


    private static BinaryFormatter GetBinaryFormatter()
    {
        BinaryFormatter formatter = new BinaryFormatter();

        SurrogateSelector selector = new SurrogateSelector();

        Vector3SerializationSurrogate vector3Surrogate = new Vector3SerializationSurrogate();
        QuaternionSerializationSurrogate quaternionSurrogate = new QuaternionSerializationSurrogate();
        ResolutionSerializationSurrogate resolutionSurrogate = new ResolutionSerializationSurrogate();

        selector.AddSurrogate(typeof(Vector3), new StreamingContext(StreamingContextStates.All), vector3Surrogate);
        selector.AddSurrogate(typeof(Quaternion), new StreamingContext(StreamingContextStates.All), quaternionSurrogate);
        selector.AddSurrogate(typeof(Resolution), new StreamingContext(StreamingContextStates.All), resolutionSurrogate);

        formatter.SurrogateSelector = selector;

        return formatter;
    }
}
