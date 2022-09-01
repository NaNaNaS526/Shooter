using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    private readonly Vector3 _cameraOffset = new Vector3(0.0f, 1.2f, -2.6f);
    private Transform _targetTransform;

    private void Start()
    {
        _targetTransform = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    private void LateUpdate()
    {
        FollowTheTarget();
    }

    private void FollowTheTarget()
    {
        transform.position = _targetTransform.TransformPoint(_cameraOffset);
        transform.LookAt(_targetTransform);
    }
}