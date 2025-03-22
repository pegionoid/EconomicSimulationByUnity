using UnityEngine;

public class Person : MonoBehaviour
{
    // 基本的な生存パラメータ
    public float hunger; // 空腹度（0-100）
    public float hungerRate = 10f; // 1日あたりの空腹増加量
    public float healthMax = 100f;
    public float health; // 体力/健康度（0-100）
    
    // 所持品
    public float food; // 所持している食料の量
    public float foodConsumptionPerDay = 1f; // 1日あたりの食料消費量
    
    // 状態フラグ
    public bool isAlive = true;
    
    // 統計追跡用
    public int daysAlive = 0;
    public float foodConsumed = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // 初期化
        hunger = 0f;
        health = healthMax;
        food = 3f; // 初期食料（3日分）
        // 時間経過イベントをリッスン
        TimeManager.Instance.OnDayChanged += OnDayChanged;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive) return;
        
        // 時間経過による空腹増加（TimeManagerから時間の経過を取得する想定）
        float dayLengthInSeconds = TimeManager.Instance.dayLengthInSeconds;
        hunger += hungerRate * (Time.deltaTime / dayLengthInSeconds);
    }

    void OnDayChanged()
    {
            EatFood();
            UpdateHealth();
            daysAlive++;
    }

    void EatFood()
    {
        if (food >= foodConsumptionPerDay)
        {
            // 十分な食料がある場合
            food -= foodConsumptionPerDay;
            hunger = Mathf.Max(0, hunger - hungerRate);
            foodConsumed += foodConsumptionPerDay;
        }
        // 食料が足りない場合は空腹度が下がらない
    }
    
    void UpdateHealth()
    {
        // 空腹度に基づいて健康状態を更新
        if (hunger > 80f)
        {
            // 非常に空腹の場合、健康が急速に低下
            health -= (hunger - 80f) * 0.5f;
        }
        else if (hunger < 20f && health < healthMax)
        {
            // 空腹度が低く、回復の余地がある場合
            health += 5f;
            health = Mathf.Min(health, healthMax);
        }
        
        // 健康チェック
        if (health <= 0)
        {
            Die();
        }
    }
    
    void Die()
    {
        isAlive = false;
        Debug.Log(gameObject.name + " has died after " + daysAlive + " days.");
        // 死亡時の処理（見た目の変更、統計記録など）
    }
    
    // 食料を獲得するメソッド（後で実装する生産活動から呼び出される）
    public void AddFood(float amount)
    {
        food += amount;
    }
}
