using System;

namespace ConsoleEngine.Scene.Serializing
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SerializedFieldAttribute : Attribute
    {
    }
}