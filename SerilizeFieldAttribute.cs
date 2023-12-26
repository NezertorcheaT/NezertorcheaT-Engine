using System;

namespace ConsoleEngine
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SerilizeFieldAttribute : Attribute
    {
        public SerilizeFieldAttribute()
        {
        }
    }
}