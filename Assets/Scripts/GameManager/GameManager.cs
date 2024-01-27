using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SplitFace.ModularSpawnSystem;
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

    public int Points;

    public WaveSpawner enemySpawner;
    public WaveSpawner bigItemSpawner;
    public WaveSpawner mediumItemSpawner;
    public WaveSpawner smallItemSPawner;

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
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    IEnumerator CoStartinCutscene()
    {
        Debug.Log("enjoy the spawning");
        yield return null;
        gamePhase = GamePhase.GAMESTART;
      //  enemySpawner.StartSpawner();
        bigItemSpawner.StartSpawner();
       //  mediumItemSpawner.StartSpawner();
       // smallItemSPawner.StartSpawner();
        Debug.Log("staerted spawning");
        yield return null;
        gamePhase = GamePhase.PLAYNG;
        Debug.Log("game stared!!");

    }
}
