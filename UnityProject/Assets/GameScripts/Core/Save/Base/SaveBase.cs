using System;
using System.Reflection;

namespace GameScripts.Core
{
    [Serializable]
    public class SaveBase
    {
        public void Clear()
        {
            foreach (PropertyInfo pi in this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic))
            {
                var type = pi.PropertyType;
                if (pi.PropertyType == typeof(string))
                {
                    pi.SetValue(this, string.Empty, null);
                }
                else if (pi.PropertyType == typeof(int))
                {
                    pi.SetValue(this, (int)0, null);
                }
                else if (pi.PropertyType == typeof(long))
                {
                    pi.SetValue(this, (long)0, null);
                }
                else if (pi.PropertyType == typeof(uint))
                {
                    pi.SetValue(this, (uint)0, null);
                }
                else if (pi.PropertyType == typeof(ulong))
                {
                    pi.SetValue(this, (ulong)0, null);
                }
                else if (pi.PropertyType == typeof(float))
                {
                    pi.SetValue(this, (float)0, null);
                }
                else if (pi.PropertyType == typeof(double))
                {
                    pi.SetValue(this, (double)0, null);
                }
                else if (pi.PropertyType == typeof(bool))
                {
                    pi.SetValue(this, (bool)false, null);
                }
                else if (type.IsSubclassOf(typeof(SaveBase)))
                {
                    var prop = pi.GetValue(this, null);
                    var methodInfo = prop.GetType().GetMethod("Clear");
                    methodInfo.Invoke(prop, new object[] { });
                }
                else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SaveList<>))
                {
                    var prop = pi.GetValue(this, null);
                    var methodInfo = prop.GetType().GetMethod("Clear");
                    methodInfo.Invoke(prop, new object[] { });
                }
                
                else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(SaveDictionary<,>))
                {
                    var prop = pi.GetValue(this, null);
                    var methodInfo = prop.GetType().GetMethod("Clear");
                    methodInfo.Invoke(prop, new object[] { });
                }
            }
        }
        
    }
}