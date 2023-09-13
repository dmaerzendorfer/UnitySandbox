using System.Linq;
using UnityEngine;

public abstract class BaseStats : ScriptableObject
{
    /// <summary>
    /// Generic getter for members using reflections. If possible do not use it, reflections are expensive.
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public T GetMember<T>(string name)
    {
        var type = this.GetType();
        var fields = type.GetFields();
        var field = fields.Where(x => x.FieldType == typeof(T) && x.Name == name).First();
        return (T)field.GetValue(this);
    }

    /// <summary>
    /// Generic setter for members using reflections. If possible do not use it, reflections are expensive.
    /// </summary>
    /// <param name="name"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public void SetMember<T>(string name, T value)
    {
        var type = this.GetType();
        var fields = type.GetFields();
        var field = fields.Where(x => x.FieldType == typeof(T) && x.Name == name).First();
        field.SetValue(this, value);
    }
}