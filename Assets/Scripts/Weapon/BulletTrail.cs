using UnityEngine;
using System.Collections.Generic;
public class BulletTrail : MonoBehaviour
{
    private Vector3 _endPosition;
    private LineRenderer _lineRenderer;
    [SerializeField] private float _speed = 30f;
    [SerializeField] private float _trailLength = 5f;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _endPosition, _speed * Time.deltaTime);
        _lineRenderer.SetPosition(0, transform.position);
        _lineRenderer.SetPosition(1, transform.position + transform.forward * _trailLength);

        if (Vector3.Distance(transform.position, _endPosition) < 1 
            || Vector3.Distance(transform.position + transform.forward * _trailLength, _endPosition) < 1)
        {
            ReturnToPool();
        }

    }


    public void Initialize(Vector3 startPosition, Vector3 endPosition)
    {
        _endPosition = endPosition;

        transform.position = startPosition;
        _lineRenderer.SetPosition(0, transform.position + transform.forward * _trailLength * 1);
        _lineRenderer.SetPosition(1, transform.position + transform.forward * _trailLength * 2);
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
    }
}
