using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Math
{
    public static int Mod(int t, int length)
    {
        int r = t % length;
        return r < 0 ? r + length : r;
    }
}
