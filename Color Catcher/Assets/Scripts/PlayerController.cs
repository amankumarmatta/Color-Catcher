using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 10f;

    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 dragStartPosition;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        Vector3 viewportPosition = mainCamera.WorldToViewportPoint(transform.position);
        viewportPosition.x = Mathf.Clamp01(viewportPosition.x);
        viewportPosition.y = Mathf.Clamp01(viewportPosition.y);
        transform.position = mainCamera.ViewportToWorldPoint(viewportPosition);

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    isDragging = true;
                    dragStartPosition = touch.position;
                    break;

                case TouchPhase.Ended:
                    isDragging = false;
                    break;
            }
        }

        if (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began))
        {
            isDragging = true;
            dragStartPosition = Input.mousePosition;
        }

        else if (Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Vector3 swipeDelta = Vector3.zero;

            if (Input.touchCount > 0)
            {
                swipeDelta = (Vector3)Input.GetTouch(0).deltaPosition;
            }
            else if (Input.GetMouseButton(0))
            {
                swipeDelta = (Vector3)Input.mousePosition - dragStartPosition;
            }

            if (Mathf.Abs(swipeDelta.x) > Mathf.Abs(swipeDelta.y))
            {
                transform.Translate(swipeDelta.x * moveSpeed * Time.deltaTime * Vector3.right);
            }

            dragStartPosition = Input.mousePosition;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Enemy")
        {
            Destroy(collision.gameObject);
        }
    }
}
