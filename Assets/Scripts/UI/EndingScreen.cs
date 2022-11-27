using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using TMPro;

public class EndingScreen : MonoBehaviour
{
    [SerializeField] private Transform screen;
    [SerializeField] private TextMeshProUGUI kills;
    [SerializeField] private TextMeshProUGUI time;
    [SerializeField] private GameObject retryButton;
    [SerializeField] private EnemyKilledSO enemyKilled;
    [SerializeField] private InventorySO playerInventory;
    [SerializeField] private InventorySO enemyInventory;
    [SerializeField] private ExperienceSO experience;
    [SerializeField] private DashSO dash;

    private void Restart() {
        enemyKilled.Restart();
        playerInventory.Restart();
        enemyInventory.Restart();
        experience.Restart();
        dash.Restart();
    }

    public void Retry() {
        Restart();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Quit() {
        SceneManager.LoadScene(0);
        Restart();
    }

    public void Show() {
        screen.gameObject.SetActive(true);
        kills.SetText(enemyKilled.TotalKills.ToString());
        time.SetText(FindObjectOfType<TimePanel>().TimeString());
        EventSystem.current.SetSelectedGameObject(retryButton);
        InputManager.Instance.SwitchTo(InputManager.Instance.Menu);
    }
}
