using System;

namespace ConsoleEngine.Scene.Serializing
{
    [AttributeUsage(AttributeTargets.Struct | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class SerializableAttribute : Attribute
    {
    }
}