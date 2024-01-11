using System;

namespace ConsoleEngine
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SerializedFieldAttribute : Attribute
    {
    }
}