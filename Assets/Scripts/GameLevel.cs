using System;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class GameLevel : MonoBehaviour
    {
        [SerializeField] private int _id;
        [SerializeField] private RectTransform _characterParent;

        public RectTransform CharacterParent => _characterParent;


        public abstract void Init(Action onLevelComplete);

    }
}