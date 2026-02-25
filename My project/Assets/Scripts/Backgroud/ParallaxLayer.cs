using UnityEngine;

public class ParallaxLayer : MonoBehaviour
{
    [Header("Parallax")]
    [SerializeField] private float parallaxFactor = 0.5f;
    
    [Header("Infinite Scrolling")]
    [SerializeField] private bool infiniteScrolling = true;

    private Transform _cam;
    private Vector3 _lastCamPosition;
    private float _spriteWidth;

    private void Awake()
    {
        _cam = Camera.main.transform;
        _lastCamPosition = _cam.position;
        _spriteWidth = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    private void LateUpdate()
    {
        Vector3 camDelta = _cam.position - _lastCamPosition;

        transform.position += new Vector3(camDelta.x * parallaxFactor, 0, 0);
        _lastCamPosition = _cam.position;

        if (!infiniteScrolling) return;

        float distanceFromCam = _cam.position.x - transform.position.x;

        if (Mathf.Abs(distanceFromCam) >= _spriteWidth)
        {
            float direction = Mathf.Sign(distanceFromCam);
            transform.position += new Vector3(_spriteWidth * direction, 0, 0);
        }
    }
}