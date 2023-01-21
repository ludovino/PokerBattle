
using System;

public interface ISaveable<T> where T : class
{
    T Save();
    public void Load(T saveManager);
    public string UniqueName { get; }
}

