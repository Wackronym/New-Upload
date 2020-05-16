using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BayatGames.SaveGamePro;

public class Settings : BaseUI {
	
	public Toggle option_1;
	public Toggle option_2;
	public Toggle option_3;
	public Toggle option_4;
	public enum GameMode
	{
		Thirty = 0,
		Sixty = 1,
		Ninty = 2,
		Infinity = 3,
	}
	
	public GameMode gamemode = GameMode.Thirty;
	
	public Text roundText;
	private int round;
	void Start () {
		round = PlayerPrefs.GetInt("wRound", 1);
		roundText.text = round.ToString();
		GameManager.Instance.backButton.SetActive(true);
		GameManager.Instance.unlimited = false;
		switch ( PlayerPrefs.GetInt("wMode", 0)){
		case 0:
			gamemode =  GameMode.Thirty;
			option_1.isOn = true;
				break;
		case 1:
			gamemode =  GameMode.Sixty;
			option_2.isOn = true;
				break;
		case 2:
			gamemode =  GameMode.Ninty;
			option_3.isOn = true;
			
				break;
		case 3:
			gamemode =  GameMode.Infinity;
			option_4.isOn = true;
			GameManager.Instance.unlimited = true;
				break;
		}
		
		base.AddMouseDownEvent();
		
	}
	
	public void IncreaseRound()
	{
		if(round<10){
			round++;
			PlayerPrefs.SetInt("wRound", round);
			roundText.text = round.ToString();
		}
	}
	public void DecreaseRound(){
		if(round>1){
			round--;
			PlayerPrefs.SetInt("wRound", round);
			roundText.text = round.ToString();
		}
	}
	public void Play(){
		GameManager.Instance.mItemDataList = SaveGame.Load<List<CardData>> ( "mItemDataList" );
		CardData c = new CardData();
		if(GameManager.Instance.mItemDataList==null){
			GameManager.Instance.mItemDataList = new List<CardData>();
		}
		GameManager.Instance.mItemDataList.Add(c);
		GameManager.Instance.currentRound = 0;
		GameManager.Instance.menuManager.PushMenu(UIManager.State.Game); 
		GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.beginLevel);
		
		
	}
	public void SetGameMode(int mode){
		switch (mode){
		case 0:
			PlayerPrefs.SetInt("wMode", 0);
			gamemode =  GameMode.Thirty;
			break;
		case 1:
			PlayerPrefs.SetInt("wMode", 1);
			gamemode =  GameMode.Sixty;
			break;
		case 2:
			PlayerPrefs.SetInt("wMode", 2);
			gamemode =  GameMode.Ninty;
			break;
		case 3:
			PlayerPrefs.SetInt("wMode", 3);
			gamemode =  GameMode.Infinity;
			break;
		}
	}

	public void MainMenu(){
		GameManager.Instance.menuManager.PopMenuToState (UIManager.State.MainMenu);
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
    }
	public void OpenPrivacy(){
        Application.OpenURL("http://www.inarigames.com/privacy-policy/");
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
    }
	public void Restart(){
		GameManager.Instance.menuManager.PopMenu ();
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
    }
	public void Back(){
		GameManager.Instance.menuManager.PopMenu ();
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
    }
}
