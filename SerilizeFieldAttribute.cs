using System;

namespace Engine
{
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class SerilizeFieldAttribute : Attribute
    {
        public SerilizeFieldAttribute()
        {
        }
    }
}