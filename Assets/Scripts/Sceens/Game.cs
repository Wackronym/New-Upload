using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;


public class Game : BaseUI
{
  
    public TMPro.TextMeshPro Coins;
    public RectTransform hours, minutes, seconds;
    private const float hoursToDegrees = 360f / 12f, minutesToDegrees = 360f / 60f, secondsToDegrees = 360f / 60f;
    public bool analog;

    private string timedis;
    public float sec;
    private int min;
    private int hou;
    public int xpLevel;
	public float chechTime;
    public bool startime = false;

    public Text TextTime;
	public string[] ss;
	public string actualWord = string.Empty;
	public string word = string.Empty;


	
	//LogicProperties
	public GameObject[] slotWords;
	public GameObject errorPopup;
	public SENTENCES s;
	public Text sentence;
	public Text slot;
	
	public Image ring;
	public Text round;
	public float totalRound;
	char[] characters;

    //Ghilman
    public InputField mainTextInputField; 
    public Text textText1;
    public Text textText2;

    private int focusCounter;
    private DateTime lastMinimize;
    private double minimizedSeconds;

    //Ghilman

    void Awake(){
		GameManager.Instance.errorPopup = errorPopup;
	}
	public void ShowKeyboard(){
		GameManager.Instance.Scroll.transform.parent.GetChild(3).gameObject.SetActive (true);
	}
	public static String GetRandomString()
	{
		var allowedChars = GameManager.Instance.client.CList[0].game_letters;
		var length = UnityEngine.Random.Range(3,6);

		var chars = new char[length];
		var rd = new System.Random();

		for (var i = 0; i < length; i++)
		{
			chars[i] = allowedChars[rd.Next(0, allowedChars.Length)];
		}

		return new String(chars);
	}

    void Start()
	{
        //Ghilman
        Application.runInBackground = true;
        OpenNativeKeyboard();
        Screen.sleepTimeout = (int)SleepTimeout.NeverSleep;
        //Ghilman

        Time.timeScale = 1;
	    CancelInvoke("DelayTime");
	    
	    if(GameManager.Instance.isFirstSession){
	    	GameManager.Instance.isFirstSession = false;
	    	CancelInvoke("DelayTime");
		    Invoke("DelayTime", 1);
	    }

		base.AddMouseDownEvent();
    }
	void HandleWrite()
	{
		//this.slot.text += VirtualKeyboadManager.instance.lastInputString;
	}
	
	void PopulateSLetters(){
		characters = GetRandomString().ToCharArray();
		foreach (GameObject wordSlot in slotWords)
		{
			wordSlot.SetActive(false);
		}
		GameObject wordSlotN = slotWords[characters.Length-3];
		wordSlotN.SetActive(true);
		if (wordSlotN.activeSelf)
		{
			for (int w = 0; w < wordSlotN.transform.childCount; w++)
			{
				GameObject Slot = wordSlotN.transform.GetChild(w).gameObject;
				Slot.SetActive(true);
				Slot.transform.GetChild(0).GetComponent<Image>().sprite =
					GameManager.Instance.atlasLoader.getAtlas(characters[w].ToString().ToUpper());
				Slot.transform.GetChild(0).GetComponent<Image>().preserveAspect = true;
				Slot.GetComponent<Image>().enabled = true;
				Slot.GetComponent<Outline>().enabled = false;
			}
		}
	}
	
	void OnEnable()
	{
      

        s = GameManager.Instance.client.GetRandomS();
		sentence.text = s.puzzle;
		totalRound = PlayerPrefs.GetInt("wRound", 1);
		ring.fillAmount = 0;
		GameManager.Instance.currentRound++;
		
		GameManager.Instance.Scroll.transform.parent.GetChild(0).gameObject.SetActive(false);
		GameManager.Instance.unlimited = false;
		switch ( PlayerPrefs.GetInt("wMode", 0)){
		case 0:
			chechTime = 30;
			break;
		case 1:
			chechTime = 60;
			break;
		case 2:
			chechTime = 90;
			break;
		case 3:
			chechTime = 300; 
			GameManager.Instance.unlimited = true;
			break;
		}
	    
		round.text = "Round " + GameManager.Instance.currentRound.ToString()+"/"+totalRound.ToString();
		PopulateSLetters();
        //Ghilman
        Debug.Log(DateTime.Now);
        GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].mDate = DateTime.Now.ToString();
        Debug.Log(GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count - 1].mDate);
        //Ghilman
		GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].mDuration = chechTime.ToString();
		GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].mId = GameManager.Instance.mItemDataList.Count-1;
		GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].mName = PlayerPrefs.GetString("mName","Solo");
		GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].mPic = "";
		GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].mChecked = false;
		RoundData rData = new RoundData();
		rData.mSenence = s.puzzle;
		rData.mCompleted = false;
		rData.reCheck = true;
		rData.mTotalRoundTime = chechTime.ToString();
		rData.time = DateTime.Now.TimeOfDay.ToString();
		GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].rData.Add(rData);
	}
	
	public void ValidateAnswer(){

        if (isCorrectAnswer()){
			if(GameManager.Instance.currentRound >= totalRound && GameManager.Instance.menuManager.NavigationStackPeek()!= UIManager.State.Win){
				GameManager.Instance.cardIndex = GameManager.Instance.mItemDataList.Count-1;
				GameManager.Instance.menuManager.PushMenu(UIManager.State.Win);
				startime = false;
				List<RoundData> r = GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].rData;
				r[r.Count-1].mUsedRoundTime = sec.ToString();
				r[r.Count-1].mCompleted = true;
				r[r.Count-1].reCheck = true;
                //r[r.Count-1].mAnswer = slot.text;
                //Ghilman
                r[r.Count - 1].mAnswer = mainTextInputField.text;
                //Ghilman
                r[r.Count-1].id = GameManager.Instance.currentRound;
				return;
			}
		
			if( GameManager.Instance.menuManager.NavigationStackPeek()== UIManager.State.Win){
			
				return;
			}
			
			GameManager.Instance.menuManager.PushMenu (UIManager.State.NewRound);//NextRound();
			List<RoundData> m = GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].rData;
			m[m.Count-1].mUsedRoundTime = sec.ToString();
			m[m.Count-1].mCompleted = true;
			m[m.Count-1].reCheck = true;
            //m[m.Count-1].mAnswer = slot.text;
            //Ghilman
            m[m.Count - 1].mAnswer = mainTextInputField.text;
            //Ghilman
            m[m.Count-1].id = GameManager.Instance.currentRound;
		}
		else{
			GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.wrong);
			Handheld.Vibrate();
		}
	}
	
	public bool isCorrectAnswer(){
		GameObject wordSlotN = slotWords[characters.Length-3];
        
        Debug.Log("slot Text = "+ mainTextInputField.text);
        Debug.Log("slot size before = " + mainTextInputField.text.Length);
        string[] ssize = mainTextInputField.text.Split(new char[0]);
        Debug.Log("slot size after = " + ssize.Length);

        Debug.Log(ssize.Length-1);
		Debug.Log(characters.Length);
		bool isAnswerCorrect = true;
		if(ssize.Length == characters.Length){
			for(int i=0; i < characters.Length; i++){
				
				if(Char.ToLower(characters[i])!=Char.ToLower(ssize[i][0])){
					isAnswerCorrect = false;
					wordSlotN.transform.GetChild(i).gameObject.GetComponent<Outline>().enabled = true;
				}
				else{
					wordSlotN.transform.GetChild(i).gameObject.GetComponent<Outline>().enabled = false;
				}
				Debug.Log(characters[i] + ": <> :"+ ssize[i][0]);
			}
		}
		else{
			isAnswerCorrect = false;
			for (int w = 0; w < wordSlotN.transform.childCount; w++)
			{
				wordSlotN.transform.GetChild(w).gameObject.GetComponent<Outline>().enabled = true;
			}
		}
		
		
		return isAnswerCorrect;
	}
	
	public void StartRoundPopup(){
		startime = false;
		GameManager.Instance.menuManager.PushMenu (UIManager.State.NewRound);
	}
	
	public void NextRound()
	{
        //Ghilman
       // SetMainInpufield();
        //Ghilman
        List<RoundData> r = GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].rData;
		if(GameManager.Instance.currentRound >= totalRound && GameManager.Instance.menuManager.NavigationStackPeek()!= UIManager.State.Win){
			GameManager.Instance.cardIndex = GameManager.Instance.mItemDataList.Count-1;
			r[r.Count-1].id = GameManager.Instance.currentRound-1;
			r[r.Count-1].reCheck = true;
			GameManager.Instance.menuManager.PushMenu(UIManager.State.Win);
			startime = false;
			return;
		}
		
		if( GameManager.Instance.menuManager.NavigationStackPeek()== UIManager.State.Win){
			
			return;
		}

		r[r.Count-1].id = GameManager.Instance.currentRound-1;
		r[r.Count-1].reCheck = true;
		sec= 0f;

		GameManager.Instance.currentRound++;
		
		GameManager.Instance.Scroll.transform.parent.GetChild(0).gameObject.SetActive(false);
		GameManager.Instance.unlimited = false;
		switch ( PlayerPrefs.GetInt("wMode", 0)){
		case 0:
			chechTime = 30;
			break;
		case 1:
			chechTime = 60;
			break;
		case 2:
			chechTime = 90;
			break;
		case 3:
			chechTime = 300; 
			GameManager.Instance.unlimited = true;
			break;
		}
		s = GameManager.Instance.client.GetRandomS();
		sentence.text = s.puzzle;
		totalRound = PlayerPrefs.GetInt("wRound", 1);
		round.text = "Round " + GameManager.Instance.currentRound.ToString()+"/"+totalRound.ToString();
		PopulateSLetters();
		ClearTextInSlot();
		RoundData rData = new RoundData();
		rData.mSenence = s.puzzle;
		rData.mCompleted = false;
		rData.mTotalRoundTime = chechTime.ToString();
		rData.time = DateTime.Now.TimeOfDay.ToString();
		rData.reCheck = true;
		GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].rData.Add(rData);
		
	}
	
	void DelayTime(){
		Time.timeScale = 0;
		chechTime = chechTime +1;
	}
	
	public void StopGame(){
		GameManager.Instance.menuManager.PushMenu(UIManager.State.GiveUPPopUP);
	}
	
	public void ClearTextInSlot(){
        mainTextInputField.text = "";
	}

    void Update()
    {

        if (GameManager.Instance.unlimited) {
			TextTime.fontSize = 32;
			TextTime.text = "Unlimited";
			return;
		}
	
        DateTime time = DateTime.Now;
        if (analog)
        {
            TimeSpan timespan = DateTime.Now.TimeOfDay;
            hours.localRotation =
                Quaternion.Euler(0f, 0f, (float) timespan.TotalHours * -hoursToDegrees);
            minutes.localRotation =
                Quaternion.Euler(0f, 0f, (float) timespan.TotalMinutes * -minutesToDegrees);
            seconds.localRotation =
                Quaternion.Euler(0f, 0f, (float) timespan.TotalSeconds * -secondsToDegrees);
        }
        else
        {
            hours.localRotation = Quaternion.Euler(0f, 0f, time.Hour * -hoursToDegrees);
            minutes.localRotation = Quaternion.Euler(0f, 0f, time.Minute * -minutesToDegrees);
            seconds.localRotation = Quaternion.Euler(0f, 0f, time.Second * -secondsToDegrees);
        }

        //Count time only whent this is true
	    if (startime == true) {
		    Time.timeScale = 1;
			//Adding seconds
			sec += Time.deltaTime;
			
			string min, sect;
			min = Mathf.Floor ((chechTime - Mathf.FloorToInt (sec)) / 60).ToString ("00");
			sect = Mathf.Floor ((chechTime - Mathf.FloorToInt (sec)) % 60).ToString ("00");
			if (min.Length < 2) {
				min = "0" + min;
			}

			if (sect.Length < 2) {
				min = "0" + min;
			}
			TextTime.text = min + ":" + sect;
		    ring.fillAmount = sec/chechTime;
			if (Mathf.FloorToInt (sec) > chechTime) {
				startime = false;
				TextTime.text = "00:00";
				GameManager.Instance.menuManager.PushMenu (UIManager.State.NewRound);
			}
		} else {
			//TextTime.text = "00:00";
		}
    }

    public void GiveUpPopUp()
	{
		if(errorPopup.activeSelf){
			return;
		}
		if(GameManager.Instance.giveupScreen.activeSelf)
			return;
        GameManager.Instance.menuManager.PushMenu(UIManager.State.GiveUPPopUP);
		GiveUp scr = GameManager.Instance.gameObject.GetComponentInChildren<GiveUp>();
		
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
    }

    public void UpdateUI()
    {
        Coins.text = GameManager.Instance.Coin().ToString();
        Invoke("CheckUI", .5f);
    }

    private void CheckUI()
    {
        Coins.enabled = false;
        Coins.enabled = true;
    }

    //Ghilman
    /// <summary>
    /// this fucntion will reset the inputfield.
    /// </summary>
    void SetMainInpufield()
    {
        mainTextInputField.text = "";
    }
    /// <summary>
    /// this fuction will onen the native keyboard after clicking on input field.
    /// </summary>
    void OpenNativeKeyboard()
    {
        mainTextInputField.Select();
        mainTextInputField.onValidateInput += delegate (string input, int charIndex, char addedChar) { return MyValidate(addedChar); };
    }
    /// <summary>
    /// this fuction will validate very charector, enter by the key boaed. it is a call back for input text validation.  
    /// </summary>
    /// <param name="charToValidate"></param>
    /// <returns></returns>
    private char MyValidate(char charToValidate)
    {
        string allowedSpecialChars = " , . ? : ; ! \" ";
        if (allowedSpecialChars.Contains(charToValidate.ToString()) || Char.IsLetterOrDigit(charToValidate))
        {
            if (Char.IsDigit(charToValidate) || charToValidate.Equals("'")) 
            {
                charToValidate = '\0';
            }
        }
        else
        {
            charToValidate = '\0';
        }
        return charToValidate;
    }
    /// <summary>
    /// this function will bubmit the text. send text to the Numan's code.  
    /// </summary>
    public void SubmitText()
    {
        Debug.Log("mainTextInputField Text = " + mainTextInputField.text);
        ValidateAnswer();
    }
    void OnApplicationPause(bool isGamePause)
    {
        if (isGamePause)
        {
            GoToMinimize();
        }
    }
    void OnApplicationFocus(bool isGameFocus)
    {
        if (isGameFocus)
        {
            focusCounter++;
            GoToMaximize();
        }
    }

    public void GoToMinimize()
    {
        lastMinimize = DateTime.Now;
    }

    public void GoToMaximize()
    {
        if (focusCounter >= 1)
        {
            minimizedSeconds = (DateTime.Now - lastMinimize).TotalSeconds;
            sec += (Int32)minimizedSeconds;
        }
    }
    //Ghilman
}