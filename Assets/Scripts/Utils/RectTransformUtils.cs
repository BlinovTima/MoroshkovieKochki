using UnityEngine;

public static class RectTransformUtils
{
    public static bool IsPointInRect(this RectTransform rt, Vector2 mouseScreenPoint)
    { 
        var point = rt.InverseTransformPoint(mouseScreenPoint);
        return rt.rect.Contains(point);
    }
}