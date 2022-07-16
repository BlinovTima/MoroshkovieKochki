using System;
using UnityEngine;


namespace MoroshkovieKochki
{
    public sealed class InputListener : MonoBehaviour
    {
        private const float _rayDistance = 100f;
        private Camera _camera;

        public static event Action OnEscKeyGet = () => { };
        public static event Action<RaycastHit2D> OnLeftMouseButtonClick = x => { };
        

        private void Awake()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape) 
                && !GameContext.HasGameState(GameState.CutScene))
            {
                OnEscKeyGet.Invoke();
            }

            if (Input.GetMouseButtonDown(0) 
                && !GameContext.HasGameState(GameState.CutScene)
                && !GameContext.HasGameState(GameState.Menu))
            {
                var ray = _camera.ScreenToWorldPoint(Input.mousePosition); 
                var raycast = Physics2D.Raycast(ray, Vector3.zero, _rayDistance);
                
                if(raycast.collider)
                    OnLeftMouseButtonClick.Invoke(raycast);
            }
        }
    }
}