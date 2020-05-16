using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SocialPlatforms.GameCenter;
using System;
using BayatGames.SaveGamePro;

public class GameManager : Singleton<GameManager>
{
    public UIManager menuManager;
    public int currentLevel;
    public bool isProcessing = false;
	public AtlasLoader atlasLoader;
   
    public GameObject[] slots;

	public bool isFirstSession = true;
    public bool isFirstClick = true;
    public AudioClip wrong;
    public AudioClip good;
    public AudioClip win;
    public AudioClip lose;
    public AudioClip click;
    public AudioClip beginLevel;
    public AudioClip storeS;

    public bool isUIDirty = false;
	public bool unlimited;
	public GameObject Scroll;
    public GameObject giveupScreen;
	public GameObject errorPopup;

	public GameObject backButton;
	public Client client;
	public float currentRound;
	public List<CardData> mItemDataList = new List<CardData>();
	public List<CardData> history = new List<CardData>();
	public List<CardData> favorite = new List<CardData>();
	public List<RoundData> dummyFavorite = new List<RoundData>();
	public int cardIndex; //Ghilman,, Numan is using this variable as currentCardIndex.
	public int favMIndex;
	public int favRIndex;
	public int favTempIndex;
	public GameObject favObj;
	public int activeTabIndex;
	
	public string webURLPrefix = "https://wackronym.net/api/";
	
	public GameObject topPopup;
	public Player player;
	public Dictionary<string, List<string>> header;

    //Ghilman
    public AuthenticateState authenticateState = AuthenticateState.Login;
    public bool shouldSaveFavorites = false; // used if rounds are deleted from favortites.
    //Ghilman

    public void PopSetting(){
		if(GameManager.Instance.menuManager.navigationStack.Count >=2){
			GameManager.Instance.menuManager.PopMenu();
		}
		else if(GameManager.Instance.menuManager.navigationStack.Count ==1){
			GameManager.Instance.menuManager.GetComponentInChildren<Menu>().Home();
		}
	}


    private void OnHideUnity(bool isGameShown)
    {
        if (!isGameShown)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }
 
    public int CurrentLevel()
    {
        return PlayerPrefs.GetInt("level", 1);
    }

    public void IncreaseLevel()
    {
        PlayerPrefs.SetInt("level", PlayerPrefs.GetInt("level", 1) + 1);
    }

    public int Coin()
    {
        if (!PlayerPrefs.HasKey("Coin"))
        {
            IncreaseCoin(0);
        }
        return PlayerPrefs.GetInt("Coin", 100);
    }

    public void IncreaseCoin(int val)
    {
        PlayerPrefs.SetInt("Coin", PlayerPrefs.GetInt("Coin", 100) + val);
        GameManager.Instance.BroadcastMessage("UpdateUI");
    }

    public int Score()
    {
        return PlayerPrefs.GetInt("Score", 0);
    }

    public void ResetScore()
    {
        PlayerPrefs.SetInt("Score", 0);
    }

    public void IncreaseScore(int val)
    {
        if (val < 5)
        {
            val = 5;
        }
        PlayerPrefs.SetInt("Score", PlayerPrefs.GetInt("Score", 0) + val);
        GameManager.Instance.BroadcastMessage("UpdateUI");
    }

	public void SetTimeScale()
	{
		Time.timeScale = 1;
	}

    void Start()
	{
		if (PlayerPrefs.HasKey ("isSoundEnabled")) {
			if (PlayerPrefs.GetInt ("isSoundEnabled") == 0) {
				AudioListener.volume = 0f;

			} else {
				AudioListener.volume = 1f;
			}
		} else {
			PlayerPrefs.SetInt ("isSoundEnabled", 1);
		}
		atlasLoader = new AtlasLoader("Font");
        DontDestroyOnLoad(gameObject);
        menuManager.OnStateChange += UpdateState;
    }
	
	public void ShowProfile(){
		if(GameManager.Instance.menuManager.NavigationStackPeek() != UIManager.State.Profile){
			GameManager.Instance.menuManager.PushMenu(UIManager.State.Profile);
		}		
	}
	
    public void UpdateState(UIManager.State g)
    {
    }

    void Update()
    {
       
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameManager.Instance.menuManager.navigationStack.Count > 1)
            {
                if (GameManager.Instance.menuManager.transform
                        .GetChild(GameManager.Instance.menuManager.transform.childCount - 1).GetComponent<BaseUI>() !=
                    null)
                {
                    GameManager.Instance.menuManager.transform
                        .GetChild(GameManager.Instance.menuManager.transform.childCount - 1)
                        .SendMessage("IsProcessing", SendMessageOptions.DontRequireReceiver);
                    GameManager.Instance.menuManager.transform
                        .GetChild(GameManager.Instance.menuManager.transform.childCount - 1).GetComponent<BaseUI>()
                        .BackBtnPressed();
                }
                else
                {
                    GameManager.Instance.menuManager.transform
                        .GetChild(GameManager.Instance.menuManager.transform.childCount - 2)
                        .SendMessage("IsProcessing", SendMessageOptions.DontRequireReceiver);
                    GameManager.Instance.menuManager.transform
                        .GetChild(GameManager.Instance.menuManager.transform.childCount - 2).GetComponent<BaseUI>()
                        .BackBtnPressed();
                }
            }
            else
            {
                Application.Quit();
            }
        }
    }
}

public class AtlasLoader
{
    public Dictionary<string, Sprite> spriteDic = new Dictionary<string, Sprite>();

    //Creates new Instance only, Manually call the loadSprite function later on 
    public AtlasLoader()
    {
    }

    //Creates new Instance and Loads the provided sprites
    public AtlasLoader(string spriteBaseName)
    {
        loadSprite(spriteBaseName);
    }

    //Loads the provided sprites
    public void loadSprite(string spriteBaseName)
    {
        Sprite[] allSprites = Resources.LoadAll<Sprite>(spriteBaseName);
        if (allSprites == null || allSprites.Length <= 0)
        {
            Debug.LogError("The Provided Base-Atlas Sprite `" + spriteBaseName + "` does not exist!");
            return;
        }
	    // Debug.Log(allSprites.Length);
        for (int i = 0; i < allSprites.Length; i++)
        {
            spriteDic.Add(allSprites[i].name, allSprites[i]);
        }
    }

    //Get the provided atlas from the loaded sprites
    public Sprite getAtlas(string atlasName)
    {
        Sprite tempSprite;

        if (!spriteDic.TryGetValue(atlasName, out tempSprite))
        {
            Debug.LogError("The Provided atlas `" + atlasName + "` does not exist!");
            return null;
        }
        return tempSprite;
    }

    //Returns number of sprites in the Atlas
    public int atlasCount()
    {
        return spriteDic.Count;
    }
}
