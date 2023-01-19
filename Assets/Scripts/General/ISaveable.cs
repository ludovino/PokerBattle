
using System;

public interface ISaveable<T>
{
    void Save(SaveManager saveManager);
    public T Load(SaveManager saveManager);
    public string UniqueName { get; }
}

