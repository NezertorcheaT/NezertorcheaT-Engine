using System;

namespace Engine.Scene.Serializing
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SerializableAttribute : Attribute
    {
    }
}