using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 GetXZ(this Vector3 vec)
    {
        return new Vector3(vec.x, 0, vec.z);
    }
}
