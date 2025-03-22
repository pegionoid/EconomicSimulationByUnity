using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SimulationUI : MonoBehaviour
{
    // UI要素への参照
    public TextMeshProUGUI timeText;
    public TextMeshProUGUI populationText;
    public TextMeshProUGUI foodText;
    public Slider timeSpeedSlider;
    
    // 時間スケール設定
    public float minTimeScale = 1f;
    public float maxTimeScale = 10f;
    
    void Start()
    {
        // 時間スケールスライダーの設定
        timeSpeedSlider.minValue = minTimeScale;
        timeSpeedSlider.maxValue = maxTimeScale;
        timeSpeedSlider.value = Time.timeScale;
        timeSpeedSlider.onValueChanged.AddListener(OnTimeScaleChanged);
    }
    
    void Update()
    {
        // UI要素の更新
        if (timeText != null && TimeManager.Instance != null)
        {
            timeText.text = "Time: " + TimeManager.Instance.GetTimeString();
        }
        
        if (populationText != null && SimulationManager.Instance != null)
        {
            populationText.text = SimulationManager.Instance.GetPopulationStats();
        }
        
        if (foodText != null && SimulationManager.Instance != null)
        {
            foodText.text = SimulationManager.Instance.GetFoodStats();
        }
    }
    
    void OnTimeScaleChanged(float value)
    {
        Time.timeScale = value;
    }
    
    // ボタンから呼び出せるメソッド
    public void PauseSimulation()
    {
        Time.timeScale = 0f;
        timeSpeedSlider.value = 0f;
    }
    
    public void ResumeSimulation()
    {
        Time.timeScale = 1f;
        timeSpeedSlider.value = 1f;
    }
}