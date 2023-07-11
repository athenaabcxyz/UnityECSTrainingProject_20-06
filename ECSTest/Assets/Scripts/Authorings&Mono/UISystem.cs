using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Entities;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UISystem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI HP;
    [SerializeField] TextMeshProUGUI Score;
    [SerializeField] TextMeshProUGUI Level;
    [SerializeField] GameObject EndUI;
    [SerializeField] GameObject PlayUI;
    [SerializeField] TextMeshProUGUI ScoreEnd;
    [SerializeField] Image background;
    private int currentScore = 0;

    private void OnEnable()
    {
        var bulletCollideSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BulletCollideSystem>();
        bulletCollideSystem.OnUpdateScore += UpdateScore;

        var playerCalculateSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PlayerHPCalculatingSystem>();
        playerCalculateSystem.OnUpdateHP += UpdateHP;

        var levelUpdateSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<LevelUpdateSystem>();
        levelUpdateSystem.OnLevelUpdate += UpdateLevel;


    }
    
    private void OnDisable()
    {
        var bulletCollideSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<BulletCollideSystem>();
        bulletCollideSystem.OnUpdateScore -= UpdateScore;

        var playerCalculateSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<PlayerHPCalculatingSystem>();
        playerCalculateSystem.OnUpdateHP -= UpdateHP;

        var levelUpdateSystem = World.DefaultGameObjectInjectionWorld.GetExistingSystemManaged<LevelUpdateSystem>();
        levelUpdateSystem.OnLevelUpdate -= UpdateLevel;

    }

    public void GameOver()
    {
        EndUI.SetActive(true);
        PlayUI.SetActive(false);
        currentScore = 0;
        background.gameObject.SetActive(true);
        ScoreEnd.SetText("Score: " + currentScore);
    }
    public void UpdateHP(int hp)
    {
        HP.text = "HP: " + hp;
        if (hp <= 0)
            GameOver();
    }

    public void UpdateScore(int score)
    {
        currentScore += score;
        Score.text = "Score: " + currentScore;
    }
    private void UpdateLevel(int level)
    {
        Level.text = "Level: " + (level-1);
    }


}
