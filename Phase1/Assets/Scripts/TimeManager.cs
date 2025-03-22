using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    // シングルトンパターン
    public static TimeManager Instance { get; private set; }
    
    // 時間設定
    public float dayLengthInSeconds = 60f; // 現実の60秒が1ゲーム内日
    public float currentTime = 0f; // 現在の時間（日単位）
    public int currentDay = 0; // 現在の日数
    
    // 日の変わり目イベント
    public event Action OnDayChanged;
    
    private float previousDay;

    void Awake()
    {
        // シングルトンの設定
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
        previousDay = Mathf.Floor(currentTime);
    }

    // Update is called once per frame
    void Update()
    {
        // 時間の経過
        currentTime += Time.deltaTime / dayLengthInSeconds;
        
        // 日が変わったかチェック
        int newDay = Mathf.FloorToInt(currentTime);
        if (newDay > previousDay)
        {
            currentDay = newDay;
            previousDay = newDay;
            
            // 日が変わったイベントを発火
            OnDayChanged?.Invoke();
        }
    }

    public float GetDayFraction()
    {
        return Time.deltaTime / dayLengthInSeconds;
    }
    
    // 日の終わりかどうかを判定
    public bool IsEndOfDay()
    {
        float dayFraction = currentTime - Mathf.Floor(currentTime);
        return dayFraction > 0.95f && dayFraction < 0.96f; // 日の95%経過時点でtrueを返す（1フレームのみ）
    }
    
    // 時間を文字列で取得
    public string GetTimeString()
    {
        int days = Mathf.FloorToInt(currentTime);
        float dayFraction = currentTime - days;
        int hours = Mathf.FloorToInt(dayFraction * 24);
        int minutes = Mathf.FloorToInt((dayFraction * 24 - hours) * 60);
        
        return $"Day {days}, {hours:00}:{minutes:00}";
    }
}
