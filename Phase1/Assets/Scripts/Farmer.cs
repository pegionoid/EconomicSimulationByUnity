using UnityEngine;

public class Farmer : MonoBehaviour
{
    public Person person; // 関連付けられた人物
    
    // 農業パラメータ
    public float farmingSkill = 1.0f; // 農業スキル（初期値1.0）
    public float farmingEfficiency = 1.0f; // 1日の労働で生産できる食料の基本量
    public float cropGrowthTime = 3.0f; // 作物が成長するのにかかる日数
    
    // 作物の状態
    private float cropProgress = 0.0f; // 作物の成長進捗（0-cropGrowthTime）
    private bool isPlanting = false; // 植え付け中フラグ
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // Personコンポーネントを取得
        if (person == null)
        {
            person = GetComponent<Person>();
        }
        
        // 時間経過イベントをリッスン
        TimeManager.Instance.OnDayChanged += OnDayChanged;
    }

    void OnDestroy()
    {
        // イベントリスナーを解除
        if (TimeManager.Instance != null)
        {
            TimeManager.Instance.OnDayChanged -= OnDayChanged;
        }
    }
    
    void OnDayChanged()
    {
        if (!person.isAlive) return;
        
        // 毎日の農作業
        if (!isPlanting)
        {
            // 植え付けを開始
            StartPlanting();
        }
        else
        {
            // 作物の成長を進める
            GrowCrops();
        }
    }
    
    void StartPlanting()
    {
        isPlanting = true;
        cropProgress = 0.0f;
        Debug.Log(gameObject.name + " started planting crops.");
    }
    
    void GrowCrops()
    {
        // 作物の成長を進める
        cropProgress += 1.0f;
        
        // 収穫の時期になったかチェック
        if (cropProgress >= cropGrowthTime)
        {
            HarvestCrops();
        }
    }
    
    void HarvestCrops()
    {
        // 収穫量を計算（スキルと効率に基づく）
        float harvestAmount = farmingEfficiency * farmingSkill;
        
        // 確率的な変動を加える（天候や運の要素）
        float randomFactor = Random.Range(0.8f, 1.2f);
        harvestAmount *= randomFactor;
        
        // 収穫した食料を人物に追加
        person.AddFood(harvestAmount);
        
        Debug.Log(gameObject.name + " harvested " + harvestAmount.ToString("F1") + " food.");
        
        // 収穫後、再び植え付けを開始
        isPlanting = false;
        
        // 成功した農作業によってわずかにスキルが向上
        farmingSkill += 0.01f;
    }
}
