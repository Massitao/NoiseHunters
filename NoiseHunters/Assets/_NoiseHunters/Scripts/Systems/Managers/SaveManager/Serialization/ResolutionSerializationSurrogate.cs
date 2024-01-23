using UnityEngine;
using System.Runtime.Serialization;

public class ResolutionSerializationSurrogate : ISerializationSurrogate
{
    public void GetObjectData(object obj, SerializationInfo info, StreamingContext context)
    {
        Resolution resolution = (Resolution)obj;
        info.AddValue("width", resolution.width);
        info.AddValue("height", resolution.height);
        info.AddValue("refreshRate", resolution.refreshRate);
    }

    public object SetObjectData(object obj, SerializationInfo info, StreamingContext context, ISurrogateSelector selector)
    {
        Resolution resolution = (Resolution)obj;
        resolution.width = (int)info.GetValue("width", typeof(int));
        resolution.height = (int)info.GetValue("height", typeof(int));
        resolution.refreshRate = (int)info.GetValue("refreshRate", typeof(int));
        obj = resolution;

        return obj;
    }
}
