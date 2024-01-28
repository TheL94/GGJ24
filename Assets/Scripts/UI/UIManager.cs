using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;


    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                Debug.LogError("UIManager NULL!");
            }
            return _instance;
        }
    }


    public RectTransform highscoreInsertion;
    public TextMeshProUGUI finalScore;
    public TextMeshProUGUI currentScore;
    public TMP_InputField Name;

    public TextMeshProUGUI stealTheFridge;
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
        
    }

    // Update is called once per frame
    void Update()
    {
        currentScore.text = GameManager.Instance.Points+"";
    }
}
