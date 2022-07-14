using UnityEngine;

namespace DefaultNamespace
{
    public sealed class Character : MonoBehaviour
    {
        public void SetLocalPosition(Vector3 localPosition)
        {
            transform.localPosition = localPosition;
        }
    }
}