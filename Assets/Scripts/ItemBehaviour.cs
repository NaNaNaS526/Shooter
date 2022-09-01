using UnityEngine;

public class ItemBehaviour : MonoBehaviour
{
    private GameBehaviour _gameManager;

    private void Start()
    {
        _gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameBehaviour>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;

        Destroy(transform.parent.gameObject);

        _gameManager.Items += 1;
    }
}