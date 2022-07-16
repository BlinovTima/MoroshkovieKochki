using System;
using UnityEngine;


namespace MoroshkovieKochki
{
    public sealed class InputListener : MonoBehaviour
    {
        private const float _rayDistance = 100f;
        private Camera _camera;
        private Vector3 _ray;

        public static event Action OnEscKeyGet = () => { };
        public static event Action<RaycastHit2D> OnLeftMouseButtonClick = x => { };
        

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) && !GameContext.HasGameState(GameState.CutScene))
            {
                OnEscKeyGet.Invoke();
            }

            if (Input.GetMouseButtonDown(0))
            {
                _ray = _camera.ScreenToWorldPoint(Input.mousePosition); 
                var raycast = Physics2D.Raycast(_ray, Vector3.zero, _rayDistance);
                
                if(raycast.collider)
                    OnLeftMouseButtonClick.Invoke(raycast);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(_ray, Vector3.forward * 200f);
        }
    }
}