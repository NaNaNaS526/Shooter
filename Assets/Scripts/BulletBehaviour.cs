using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    [SerializeField] private float bulletLifeTime = 3.0f;

    private void Start()
    {
        Destroy(gameObject, bulletLifeTime);
    }
}