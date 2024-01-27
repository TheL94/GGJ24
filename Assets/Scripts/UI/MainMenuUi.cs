using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUi : MonoBehaviour
{
    
    public Scores scores;
    public GameObject higScorePrefab;
    public RectTransform ScoresParent;
    // Start is called before the first frame update
    string scoresPath = Path.Combine(Application.streamingAssetsPath, "HighScores.json");
    void Start()
    {
        if (File.Exists(scoresPath))
        {
            scores = JsonUtility.FromJson<Scores>(File.ReadAllText(scoresPath));
        }
        else
        {
            var c=File.Create(scoresPath);
            c.Close();
            File.WriteAllText(scoresPath, JsonUtility.ToJson(scores));
        }

        scores.highScores = scores.highScores.OrderBy(x=>x.Score).ToList();
        scores.highScores.Reverse();
        foreach (var item in scores.highScores)
        {
            var highScore = Instantiate(higScorePrefab, ScoresParent).GetComponent<UIHighScore>();
            highScore.Name.text = item.Name;
            highScore.Points.text = item.Score+"";
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
