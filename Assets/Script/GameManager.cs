using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("HP Settings")]
    [SerializeField] private float maxHp = 100f;
    private float currentHp;
    [SerializeField] private float regenRate = 1f; // 1 HP per second regeneration

    [Header("UI Reference")]
    [SerializeField] private Text hpText; // Drag your UI Text here

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        currentHp = maxHp;
        UpdateUI();
    }

    private void Update()
    {
        // Health Regeneration (1 HP/sec), capped at 100
        if (currentHp < maxHp)
        {
            currentHp += regenRate * Time.deltaTime;
            currentHp = Mathf.Clamp(currentHp, 0f, maxHp);
            UpdateUI();
        }
    }

    public void TakeDamage(float amount)
    {
        currentHp -= amount;
        UpdateUI();

        if (currentHp <= 0f)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    private void UpdateUI()
    {
        if (hpText != null)
        {
            hpText.text = "HP: " + Mathf.Ceil(currentHp);
        }
    }
}