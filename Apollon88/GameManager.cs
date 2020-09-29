using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class GameManager : MonoBehaviour
{
    Scene currentScene;
    public static GameManager Instance { get; private set; } // to call the gamemanager in other script

    [SerializeField] GameObject PlayerPawn = null; // used for the player to spawned

    GameObject Player = null; // after the player spawned it will set the player to this

    public GameObject GetPlayer() { return Player; } // get the player from the WaveSpawner

    [SerializeField] Transform PlayerStart = null;

    public MulticastOneParam OnEnemyCountChanged; // not get subscribed to anything so its fine
    public MulticastOneParam OnEnemyKilled; // subscribed to ComboKill and BarUI, so once death, set OnEnemyKilled to null

    int numberEnemyKilled = 0;

    [SerializeField] int maxEnemies = 100;

    //For OnSceneLoaded reference. (Could be private, but right now I just want to see who's referencing who in inspector)
    public BarUI barUI; 
    public GameObject deathUI, gameUI, crosshair;
    public TextMeshProUGUI finalScore, highScore;

    public Button pauseRetryButton, pauseMainButton, deadRetryButton, deadMainButton;

    public Slider masterSlider;
    public Slider effectSlider;
    public AudioMixer masterMixer;

    //----------

    int GetMaxEnemies() { return maxEnemies; } // not being used but maybe future


    int EnemyCounter = 0;   // this is how many enemy alive

    public int GetEnemyCount() { return Instance.EnemyCounter; } // function that returns EnemyCounter

    private void Awake()
    {
        // LIKE I SAID FROM KG, THIS BEAUTY IS A SINGLETON
        if(Instance == null) // if there is no instance
        {
            Instance = this; // then this instance will be this
            DontDestroyOnLoad(gameObject); // will not destroy this gameObject when loading new scene
        }
        else if(Instance != null) // if the instance is not null, more than 1 instance
        {
            Destroy(gameObject); // destroy it
            return;
        }

    }

    private void OnEnable() // will start when we play
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void Start()
    {
        //SET IT at start, so that it will use the last audio
        masterMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MasterSlider", 0));
        masterMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("EffectSlider", 0));
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = scene;
        if(currentScene.buildIndex != 1)
        {
            Debug.Log("This is Menu scene");
            //GET ALL GAMEOBJECT
            //THIS ONE IS FOR HIGHSCORE (SAME NAME as the one on Game scene)
            highScore = GameObject.Find("HighScoreText").GetComponent<TextMeshProUGUI>();
            highScore.text = "HighScore: \n" + PlayerPrefs.GetInt("HighScore", 0).ToString();

            //THIS ONE IS FOR CHOICES TEXT
            finalScore = GameObject.Find("ChoicesText").GetComponent<TextMeshProUGUI>();
            finalScore.text = "Are you sure you want to\nreset? It will set your score\nback to 0.";

            //THIS ONE IS FOR TRANSITION
            deathUI = GameObject.FindObjectOfType<TransitionToGame>().gameObject;
            deathUI.SetActive(false);

            //THIS ONE IS FOR RESET MENU (To get Reset function)
            crosshair = GameObject.Find("ResetChoice");

            //THIS ONE IS FOR SETTINGS
            gameUI = GameObject.Find("SettingsMenu");

            //------------------
            //GET ALL BUTTON
            //THIS ONE IS FOR PLAY
            deadRetryButton = GameObject.Find("PlayButton").GetComponent<Button>();
            deadRetryButton.onClick.AddListener(() => Play());

            //THIS ONE IS FOR QUIT
            deadMainButton = GameObject.Find("QuitButton").GetComponent<Button>();
            deadMainButton.onClick.AddListener(() => Quit());
            
            //THIS ONE IS FOR RESET
            pauseRetryButton = GameObject.Find("YesButton").GetComponent<Button>();
            pauseRetryButton.onClick.AddListener(() => ResetScore());
            crosshair.SetActive(false); // That way we could insert Reset onClick function first, then disable it. Don't know why it won't set to false if I place below MASTER VOLUME

            //---------------------
            //GET ALL SLIDER
            //THIS ONE IS FOR MASTER VOLUME
            masterSlider = GameObject.Find("MasterSlider").GetComponent<Slider>();
            masterSlider.onValueChanged.AddListener(SetMusicLvl);

            //THIS ONE IS FOR EFFECT VOLUME
            effectSlider = GameObject.Find("EffectSlider").GetComponent<Slider>();
            effectSlider.onValueChanged.AddListener(SetEffectLvl);
            //-----------------

            gameUI.SetActive(false);
            masterSlider.value = PlayerPrefs.GetFloat("MasterSlider", 0); // It seems that I have to copy the value to here again as it will not take the slider value
            effectSlider.value = PlayerPrefs.GetFloat("EffectSlider", 0);
        }
        else
        {
            Debug.Log("This is Game scene");

            //SET THE COUNTER ALL TO 0    (Not going to broke something, but it was suppose to reset before creating singleton, so yeah
            EnemyCounter = 0;
            numberEnemyKilled = 0;

            //GET ALL GAMEOBJECT TYPE
            barUI = GameObject.FindObjectOfType<BarUI>();
            deathUI = GameObject.Find("Death Canvas");
            
            finalScore = GameObject.Find("Score text").GetComponent<TextMeshProUGUI>();
            highScore = GameObject.Find("HighScoreText").GetComponent<TextMeshProUGUI>();

            gameUI = GameObject.Find("Game Canvas");
            crosshair = GameObject.Find("Crosshair");
            PlayerStart = GameObject.Find("PlayerStart").transform;
            //------------------------
            //GET ALL BUTTON
            pauseRetryButton = GameObject.Find("PauseRetryButton").GetComponent<Button>();
            pauseRetryButton.onClick.AddListener(() => Retry());
            pauseMainButton = GameObject.Find("PauseMainMenuButton").GetComponent<Button>();
            pauseMainButton.onClick.AddListener(() => MainMenu());
            deadRetryButton = GameObject.Find("DeadRetryButton").GetComponent<Button>();
            deadRetryButton.onClick.AddListener(() => Retry());
            deadMainButton = GameObject.Find("DeadMainMenuButton").GetComponent<Button>();
            deadMainButton.onClick.AddListener(() => MainMenu());

            //------------------------
            //GET ALL SLIDER (SAME NAME as the one on Main scene)
            masterSlider = GameObject.Find("MasterSlider").GetComponent<Slider>();
            masterSlider.onValueChanged.AddListener(SetMusicLvl);
            effectSlider = GameObject.Find("EffectSlider").GetComponent<Slider>();
            effectSlider.onValueChanged.AddListener(SetEffectLvl);
            //-----------------

            //-----------------
            deathUI.SetActive(false); // set false on here, that way all gameobject and button related to this can get before set false
             // I don't set pauseUI off here since I did that on PauseUI script


        }
        SpawnPlayer(); // either way, it will check again for every scene changes
        highScore.text = PlayerPrefs.GetInt("HighScore", 0).ToString();

        //Get the slider value and set the audio again, works when changing scene except main scene
        masterSlider.value = PlayerPrefs.GetFloat("MasterSlider", 0);
        effectSlider.value = PlayerPrefs.GetFloat("EffectSlider", 0);

        masterMixer.SetFloat("MusicVol", PlayerPrefs.GetFloat("MasterSlider", 0));
        masterMixer.SetFloat("SFXVol", PlayerPrefs.GetFloat("EffectSlider", 0));

    }

    public void ModifyEnemyCount(bool spawned) // basically this is where the true, false on the AIBase Start() and Death()
    {
        if (spawned)
        {
            Instance.EnemyCounter++;
            Debug.Log("Spawn");
        }
        else
        {
            Debug.Log("Death");
            Instance.EnemyCounter--;

            numberEnemyKilled++; // what we do is that we get the value (numberEnemyKilled) and put it onto the OnEnemyKilled since it takes an int
            OnEnemyKilled?.Invoke(numberEnemyKilled);  // call the delegate (OnEnemyKilled) while passing through the date (numberEnemyKilled)
        }

        //OnEnemyCountChanged?.Invoke(manager.EnemyCounter);    MulticastOneParam(int value)

        if(OnEnemyCountChanged != null) // this one the same than the top but with this you can check else
        {
            OnEnemyCountChanged(Instance.EnemyCounter);
        }
        else // what I know is that it will call this on every enemy spawned
        {
            Debug.LogError(name + " No functions bound to OnEnemyCountChanged");
        }
    }

    void SpawnPlayer()
    {
        GameObject _Player;

        //player                          PlayerStartPosition    PlayerStartRotation
        _Player = Instantiate(PlayerPawn, PlayerStart.position, PlayerStart.rotation);

        Player = _Player;
    }
    public void Death()
    {
        Cursor.visible = true; // enable mouse cursor
        crosshair.SetActive(false); // crosshair off
        gameUI.SetActive(false); // set Death UI On
        deathUI.SetActive(true); // disable other UI (remember to enable it later)

        finalScore.text = "Score: " + barUI.playerScore;
        if(barUI.playerScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", barUI.playerScore);
            highScore.text = "CONGRATULATIONS, NEW RECORD";
        }
        else
        {
            highScore.text = "HighScore: " + PlayerPrefs.GetInt("HighScore", 0).ToString();
        }

        //THE IMPORTANT OF ALL, This thing took my stress to a whole new level
        //NEED TO SET IT TO NULL
        // 1. Since this is from Gamemanager delegate, it will not destroy, so it still get subscribed from a function 
        // that is already destroyed once we change / reset scene.
        // 2. Once we reset, we get subscribe AGAIN, they will check the first one which is empty
        // since the first subscriber has been destroyed
        // Set it to null, that way it will get subscribe again from the script (ComboKill & BarUI) Start()
        OnEnemyKilled = null;
    }
    
    public void ResetScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        finalScore.text = "High Score has been Reset."; // the text will stay like this until it changes scene which is fine since it will tell the player that you have reset the highscore.

        //highScore.text = "0"; // could do this, but for safety so I prefer get int
        highScore.text = "HighScore: \n" + PlayerPrefs.GetInt("HighScore", 0).ToString();
    }

    public void Retry()
    {
        OnEnemyKilled = null; // RESET delegate when press retry
        Time.timeScale = 1f; // Make sure it doesn't stop
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Menu");
        Cursor.visible = true;
    }

    public void Play()
    {
        OnEnemyKilled = null; // RESET delegate when press play / also back to menu and play
        Time.timeScale = 1f; // Make sure it doesn't stop

        deathUI.SetActive(true);
    }

    public void Quit()
    {
        Debug.Log("Quit");
        Application.Quit();
    }

    public void SetMusicLvl(float volume)
    {
        //Firstly I set the min value from -80 to -40. Reason why do this, if we make -80, when we use the slider, the sound will already go low when it's on mid (-40), so we set it to -80 once we reach -40. also change the min value to -40
        if(volume <= -40)
        {
            volume = -80;
        }
        masterMixer.SetFloat("MusicVol", volume);
        PlayerPrefs.SetFloat("MasterSlider", volume);
    }

    public void SetEffectLvl(float volume)
    {
        if (volume <= -40)
        {
            volume = -80;
        }
        masterMixer.SetFloat("SFXVol", volume);
        PlayerPrefs.SetFloat("EffectSlider", volume);
    }
}
