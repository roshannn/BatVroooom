using System;

public static class Utility 
{
    public static T GetRandomEnumValue<T>() where T : Enum {
        Array values = Enum.GetValues(typeof(T));
        Random _random = new Random();
        return (T)values.GetValue(_random.Next(values.Length));
    }
}
