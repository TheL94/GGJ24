using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Cinemachine;
using DG.Tweening;
using UnityEngine.Events;
using UnityEngine.UI;

public class MainMenuUi : MonoBehaviour
{
    public Scores scores;
    public GameObject higScorePrefab;
    public RectTransform ScoresParent;

    public CanvasGroup MainMenu;
    public CanvasGroup ScoreMenu;

    public CinemachineVirtualCamera mainCam;
    public CinemachineVirtualCamera scoreCam;
    public CinemachineBrain brain;

    private CinemachineVirtualCamera activeCamera;
    private Coroutine currentRoutine;

    string scoresPath = Path.Combine(Application.streamingAssetsPath, "HighScores.json");

    void Start()
    {
        if (File.Exists(scoresPath))
        {
            scores = JsonUtility.FromJson<Scores>(File.ReadAllText(scoresPath));
        }
        else
        {
            var c = File.Create(scoresPath);
            c.Close();
            File.WriteAllText(scoresPath, JsonUtility.ToJson(scores));
        }

        scores.highScores = scores.highScores.OrderBy(x => x.Score).ToList();
        scores.highScores.Reverse();
        foreach (var item in scores.highScores)
        {
            var highScore = Instantiate(higScorePrefab, ScoresParent).GetComponent<UIHighScore>();
            highScore.Name.text = item.Name;
            highScore.Points.text = item.Score + "";
        }

        scoreCam.gameObject.SetActive(false);
        ScoreMenu.alpha = 0;
    }

    public void StartGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ShowScore()
    {
        MainMenu.DOFade(0, 1f);
        ActivateCamera(scoreCam,() => { ScoreMenu.DOFade(1, .5f); });
    }

    public void ShowMenu()
    {
        ScoreMenu.DOFade(0, 1f);
        ActivateCamera(mainCam, () => { MainMenu.DOFade(1, .5f); });
    }

    void ActivateCamera(CinemachineVirtualCamera cam, UnityAction onComplete = null)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(WaitForCameraTransition(cam, onComplete));
    }

    IEnumerator WaitForCameraTransition(CinemachineVirtualCamera cam, UnityAction onComplete)
    {
        var previusZone = activeCamera;
        activeCamera = cam;
        activeCamera.gameObject.SetActive(true);
        yield return new WaitUntil(() => !brain.IsBlending);
        if (previusZone != null)
            previusZone.gameObject.SetActive(false);
        onComplete?.Invoke();
        currentRoutine = null;
    }
}