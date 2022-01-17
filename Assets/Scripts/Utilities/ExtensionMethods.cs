using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static T Get<T>(this T[,] grid, Vector2Int address)
    {
        return grid[address.y, address.x];
    }

    public static void Set<T>(this T[,] grid, Vector2Int address, T value)
    {
        grid[address.y, address.x] = value;
    }
}
