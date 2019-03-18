using System;

namespace PwshKeePass.CoreExtensions
{
    public static class TypeExtension
    {
        public static bool HasProperty(this Type obj, string propertyName)
        {
            return obj.GetProperty(propertyName) != null;
        }
    }
}