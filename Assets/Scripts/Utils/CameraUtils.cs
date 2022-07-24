using UnityEngine;

namespace Utils
{
    public static class CameraUtils
    {
        public static void GetCameraPanels(Camera camera)
        {
            var planes = GeometryUtility.CalculateFrustumPlanes(camera);
            var left = planes[0];
            var right = planes[1];
            
         //  left.
        }
      
    }
}