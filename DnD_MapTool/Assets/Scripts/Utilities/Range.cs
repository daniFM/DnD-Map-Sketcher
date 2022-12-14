// Copyright (c) 2022 Daniel Fernández Marqués
// Licensed under the GNU General Public License (GPL) version 3. See the LICENSE file for more details.

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