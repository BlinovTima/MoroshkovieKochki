using UnityEngine;

namespace Utils
{
    public static class CameraUtils
    {
        public static void MovePositionBehindFrustrum(this Transform point, float additionalDistance = 0f)
        {
            var pointPosition = point.position;
            var camera = Camera.main;
            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            var left = planes[0];
            var right = planes[1];
            
            var leftRay = new Ray(pointPosition, Vector3.left * 100f);
            left.Raycast(leftRay, out var leftDistance);   
            
            var rightRay = new Ray(pointPosition, Vector3.right * 100f);
            right.Raycast(rightRay, out var rightDistance);

            if (leftDistance < rightDistance)
                point.position = leftRay.GetPoint(leftDistance + additionalDistance);
            else
                point.position = rightRay.GetPoint(rightDistance  + additionalDistance);
        }
      
    }
}