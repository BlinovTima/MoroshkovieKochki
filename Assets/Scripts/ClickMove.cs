using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickMove : MonoBehaviour
{
    public float speed;

    private Vector3 TPosition;
    private bool isMoving = false;

    void Update()
    {
            if (Input.GetMouseButton(0))
            {
                TriggerPosition();
            }

            if (isMoving)
            {
                ItsMove();
            }
    }

    void TriggerPosition()
    {
            if (Input.GetMouseButtonDown(0))
            { // Нажали ЛКМ
                Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Луч от текущих координат мыши вперёд
                RaycastHit2D[] raycasts = Physics2D.RaycastAll(ray, Vector2.zero); // Все объекты под лучём

                if (raycasts != null && raycasts.Length > 0)
                {
                    TPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                    TPosition.z = transform.position.z;

                   
                }
            }
        isMoving = true;

    }

    void ItsMove()
    {
        //transform.rotation = Quaternion.LookRotation(Vector3.forward, TPosition);
        transform.position = Vector3.MoveTowards(transform.position, TPosition, speed * Time.deltaTime);

        if (transform.position == TPosition)
        {
            isMoving = false;
        }
    }
}
