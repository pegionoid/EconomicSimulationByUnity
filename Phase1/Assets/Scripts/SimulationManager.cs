using UnityEngine;
using System.Collections.Generic;

public class SimulationManager : MonoBehaviour
{
    // シングルトンパターン
    public static SimulationManager Instance { get; private set; }
    
    // NPCプレハブと生成設定
    public GameObject personPrefab;
    public int initialPopulation = 10;
    public Vector2 spawnAreaSize = new Vector2(10f, 10f);
    
    // NPCリスト
    public List<GameObject> people = new List<GameObject>();
    
    // 統計データ
    public int totalPopulation = 0;
    public int alivePeople = 0;
    public int deadPeople = 0;
    public float totalFood = 0f;
    
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
    }
    
    void Start()
    {
        // 初期NPCの生成
        InitializePopulation();
        
        // 定期的な統計収集
        InvokeRepeating("CollectStatistics", 1f, 1f);
    }
    
    void InitializePopulation()
    {
        for (int i = 0; i < initialPopulation; i++)
        {
            CreatePerson(new Vector3(
                Random.Range(-spawnAreaSize.x/2, spawnAreaSize.x/2),
                Random.Range(-spawnAreaSize.y/2, spawnAreaSize.y/2),
                0f
            ));
        }
    }
    
    GameObject CreatePerson(Vector3 position)
    {
        GameObject person = Instantiate(personPrefab, position, Quaternion.identity);
        person.name = "Person_" + totalPopulation;
        
        // 職業の割り当て（今はシンプルに全員農民）
        person.AddComponent<Farmer>();
        person.GetComponent<Farmer>().person = person.GetComponent<Person>();
        
        // リストに追加
        people.Add(person);
        totalPopulation++;
        alivePeople++;
        
        return person;
    }

    void CollectStatistics()
    {
        // 人口統計の更新
        alivePeople = 0;
        deadPeople = 0;
        totalFood = 0f;
        
        foreach (GameObject personObj in people)
        {
            Person person = personObj.GetComponent<Person>();
            if (person.isAlive)
            {
                alivePeople++;
                totalFood += person.food;
            }
            else
            {
                deadPeople++;
            }
        }
        
        // 統計情報のログ出力（開発中のみ）
        Debug.Log($"Day {TimeManager.Instance.currentDay}: Population - {alivePeople} alive, {deadPeople} dead, Total Food: {totalFood:F1}");
    }

    // UIから呼び出せる統計情報取得メソッド
    public string GetPopulationStats()
    {
        return $"Population: {alivePeople}/{totalPopulation}\nDead: {deadPeople}";
    }

    public string GetFoodStats()
    {
        float avgFood = alivePeople > 0 ? totalFood / alivePeople : 0;
        return $"Total Food: {totalFood:F1}\nAvg Food per Person: {avgFood:F1}";
    }
}