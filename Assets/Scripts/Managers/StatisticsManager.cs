using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace zephkelly
{
  [CreateAssetMenu(menuName = "ScriptableObjects/StatisticsManager")]
  public class StatisticsManager : ScriptableObject 
  {
    private TextMeshProUGUI highScoreText;

    private float timeAliveTotal;
    private int furthestDistanceTravelled;
    private int asteroidsDestroyedTotal;
    private int pickupsCollectedTotal;
    private int creditsGainedTotal;
    private int creditsSpentTotal;

    private Dictionary<int, int> scores = new Dictionary<int, int>();

    public int FurthestDistanceTravelled { get => furthestDistanceTravelled; }
    public int AsteroidsDestroyed { get => asteroidsDestroyedTotal; }
    public int PickupsCollected { get => pickupsCollectedTotal; }
    public int CreditsGained { get => creditsGainedTotal; }
    public int CreditsSpent { get => creditsSpentTotal; }

    public void Init()
    {
      timeAliveTotal = 0;
      furthestDistanceTravelled = 0;
      asteroidsDestroyedTotal = 0;
      pickupsCollectedTotal = 0;
      creditsGainedTotal = 0;
      creditsSpentTotal = 0;

      highScoreText = GameObject.Find("HighScoreText").GetComponent<TextMeshProUGUI>();
    }

    public void IncrementTimeAlive()
    {
      timeAliveTotal += Time.deltaTime;
    }

    public void UpdateCurrentDistance(int distance)
    {
      var distanceScaled = distance / 7;

      if (furthestDistanceTravelled > distanceScaled) return;
      
      furthestDistanceTravelled = distanceScaled;
      GetScore();
    }

    public void IncrementAsteroidsDestroyed()
    {
      asteroidsDestroyedTotal++;
      GetScore();
    }

    public void IncrementPickupsCollected()
    {
      pickupsCollectedTotal++;
      GetScore();
    }

    public void IncrementCreditsGained(int amount)
    {
      creditsGainedTotal += amount;
      GetScore();
    }

    public void IncrementCreditsSpent(int amount)
    {
      creditsSpentTotal += amount;
      GetScore();
    }

    public int GetScore()
    {
      var asteroidsDestroyedScore = asteroidsDestroyedTotal * 40;
      var playerDistance = furthestDistanceTravelled * 10;
      var pickupsCollectedScore = pickupsCollectedTotal * 20;
      var creditsGainedScore = creditsGainedTotal * 100;
      var creditsSpentScore = creditsSpentTotal * 100;
      //var timeAliveScore = (int)timeAliveTotal * 10;

      int highScore = asteroidsDestroyedScore
        + playerDistance
        + pickupsCollectedScore
        + creditsGainedScore
        + creditsSpentScore;

      highScoreText.text = highScore.ToString();
      return highScore;
    }

    public void SaveScore()
    {
      var scoresLength = scores.Count + 1;
      var score = GetScore();

      scores.Add(scoresLength, score);
    }
  }
}