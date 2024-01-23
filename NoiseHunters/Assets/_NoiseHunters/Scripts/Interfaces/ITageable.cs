using UnityEngine;

public interface ITageable
{
    TagUI tag_UIScript { get; }
    Transform tag_Position { get; }

    void OnTag(float soundIntensity);
}
