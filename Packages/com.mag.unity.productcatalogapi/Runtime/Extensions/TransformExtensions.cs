using UnityEngine;

public static class TransformExtensions
{
    /// <summary>
    /// Includes self as well. Recursively looks for the first child in all children with the exact given name.
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="name"></param>
    /// <returns></returns>
    public static Transform FindDeepChild(this Transform transform, string name)
    {
        if (transform.name == name)
            return transform;

        var result = transform.Find(name);
        if (result != null)
            return result;
        foreach (Transform child in transform)
        {
            result = child.FindDeepChild(name);
            if (result != null)
                return result;
        }
        return null;
    }
}