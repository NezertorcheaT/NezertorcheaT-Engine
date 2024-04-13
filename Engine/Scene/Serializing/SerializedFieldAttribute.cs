using System;

namespace Engine.Scene.Serializing
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SerializedFieldAttribute : Attribute
    {
    }
}