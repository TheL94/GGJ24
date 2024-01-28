using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplitFace.ModularSpawnSystem;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    private static GameManager _instance;
   
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("GameManager NULL!");
            }
            return _instance;
        }
    }
    public Scores scores;
    string scoresPath = Path.Combine(Application.streamingAssetsPath, "HighScores.json");
    public int Points;

    public WaveSpawner enemySpawner;
    public WaveSpawner bigItemSpawner;
    public WaveSpawner mediumItemSpawner;
    public WaveSpawner smallItemSPawner;
    public GameObject itemFridge;
    public GameObject nonItemFridge;
    public enum GamePhase
    {
        STARTINGCUTSCENE,
        GAMESTART,
        PLAYNG,
        PAUSE,
        GAMEOVER,
    }
    public GamePhase gamePhase;

    private void Awake()
    {
        if (_instance)
            Destroy(gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(this);
    }
    // Start is called before the first frame update
    void Start()
    {
        gamePhase = GamePhase.STARTINGCUTSCENE;
        StartCoroutine(CoStartinCutscene());
        scores = JsonUtility.FromJson<Scores>(File.ReadAllText(scoresPath));
        scores.highScores = scores.highScores.OrderBy(x => x.Score).ToList();
        scores.highScores.Reverse();
    }

    public IEnumerator CoStartinCutscene()
    {
        Debug.Log("enjoy the spawning");
        yield return null;
        gamePhase = GamePhase.GAMESTART;
        enemySpawner.StartSpawner();
        bigItemSpawner.StartSpawner();
        mediumItemSpawner.StartSpawner();
        smallItemSPawner.StartSpawner();
        Debug.Log("staerted spawning");
        yield return null;
        gamePhase = GamePhase.PLAYNG;
        Debug.Log("game stared!!");
        yield return new WaitForSeconds(350);
        itemFridge.SetActive(true);
        nonItemFridge.SetActive(false);
        UIManager.Instance.stealTheFridge.gameObject.SetActive(true);
    }
    
    public IEnumerator CoGameOver()
    {
        //start end cutscene
        gamePhase = GamePhase.GAMEOVER;
        yield return null;
        //high score check

        if (Points >= scores.highScores[scores.highScores.Count-1].Score)
        {
            UIManager.Instance.highscoreInsertion.gameObject.SetActive(true);
            UIManager.Instance.finalScore.text = Points+"";
        }
    }

    public void OverwriteHighSore()
    {
        if (File.Exists(scoresPath))
        {
            scores = JsonUtility.FromJson<Scores>(File.ReadAllText(scoresPath));
        }
        else
        {
            var c = File.Create(scoresPath);
            c.Close();
          
        }

        File.WriteAllText(scoresPath, JsonUtility.ToJson(scores));

        SceneManager.LoadScene("MainMenu");
    }

}


[System.Serializable]
public class HighScore
{
   public string Name;
   public int Score;
}

[System.Serializable]
public class Scores
{
    public List<HighScore> highScores; 
}
