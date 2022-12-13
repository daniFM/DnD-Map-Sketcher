// Copyright (c) Daniel Fernández 2022


using System;

public abstract class Range<T>
{
    public T max;
    public T min;

    public abstract T GetRandom();
}

[Serializable] public class RangeInt : Range<int>
{
    public override int GetRandom()
    {
        return UnityEngine.Random.Range(min, max);
    }
}

[Serializable] public class RangeFloat: Range<float>
{
    public override float GetRandom()
    {
        return UnityEngine.Random.Range(min, max);
    }
}