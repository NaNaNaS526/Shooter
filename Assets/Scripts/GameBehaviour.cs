using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class GameBehaviour : MonoBehaviour
{
    private const int MaxItems = 4;

    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private TextMeshProUGUI itemsCollectedText;
    [SerializeField] private TextMeshProUGUI notificationText;
    [SerializeField] private GameObject endGamePanel;
    [SerializeField] private Button newGameButton;
    [SerializeField] private GameObject enemy;

    private List<Vector3> _enemyPositions = new List<Vector3>(4)
    {
        new Vector3(0.0f, 1.0f, 13.0f),
        new Vector3(13.0f, 1.0f, 0.0f),
        new Vector3(0.0f, 1.0f, -13.0f),
        new Vector3(-13.0f, 1.0f, 0.0f)
    };

    private int _itemsCollected;

    public int Items
    {
        get => _itemsCollected;
        set
        {
            if (Hp < 10)
            {
                Hp += 1;
            }

            _itemsCollected = value;
            itemsCollectedText.text = $"Items Collected: {_itemsCollected}";
            if (_itemsCollected >= MaxItems)
            {
                ShowNotification("You have found all the items!", ref notificationText);
                ShowEndPanel();
            }
            else
            {
                ShowNotification($"Item found, only " + (MaxItems - _itemsCollected) + " more to go!",
                    ref notificationText);
            }
        }
    }

    private int _playerHp = 10;

    public int Hp
    {
        get => _playerHp;
        set
        {
            _playerHp = value;
            hpText.text = $"Player health: {_playerHp}";
            if (_playerHp <= 0)
            {
                ShowEndPanel();
            }
            else
            {
                ShowNotification("Ouch... that's got hurt", ref notificationText);
            }
        }
    }

    private void Start()
    {
        newGameButton.onClick.AddListener(Utilities.RestartLevel);
        StartCoroutine(EnemySpawn());
    }

    private IEnumerator EnemySpawn()
    {
        while (true)
        {
            Instantiate(enemy, _enemyPositions[Random.Range(0, 4)], Quaternion.identity);
            yield return new WaitForSeconds(10f);
        }
    }

    private void ShowEndPanel()
    {
        endGamePanel.SetActive(true);
        Time.timeScale = 0;
    }

    private static void ShowNotification(string notification, ref TextMeshProUGUI text)
    {
        text.text = notification;
    }
}