using System.Collections.Generic;

[System.Serializable]
public class PlayerSave
{
    // Last scene played. If last scene played is different,  it will erase the other properties
    public string lastPlayedScene;

    // Levels played. For Level Selection
    public HashSet<string> unlockedLevels = new HashSet<string>();

    // Cinematics played. For Content Selection
    public HashSet<string> unlockedCinematics = new HashSet<string>();

    // Collectables picked. For Achievements
    public HashSet<string> collectiblesPicked = new HashSet<string>();
}