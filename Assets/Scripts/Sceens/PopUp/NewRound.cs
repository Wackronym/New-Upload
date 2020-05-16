using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class NewRound : BaseUI{
	public Text TextTime;
	public int chechTime;
	public bool startime = false;
	public float sec;
	
	void Start(){
		base.AddMouseDownEvent();
	}
	
	void OnEnable(){
		//startime = true;
	}
	void Update()
	{
		
		
		DateTime time = DateTime.Now;
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

			if (Mathf.FloorToInt (sec) > chechTime) {
				startime = false;
				GameManager.Instance.menuManager.PopMenuToState (UIManager.State.Game);
			}
		} else {

		}
	}
	
	public void Pause(){
		//startime = false;
	}
	
	public void Resume(){
		if(GameManager.Instance.currentRound >= GameManager.Instance.menuManager.GetComponentInChildren<Game>().totalRound && GameManager.Instance.menuManager.NavigationStackPeek()!= UIManager.State.Win){
			//ring.fillAmount = GameManager.Instance.currentRound/totalRound;
			GameManager.Instance.cardIndex = GameManager.Instance.mItemDataList.Count-1;
			GameManager.Instance.menuManager.PushMenu(UIManager.State.Win);
			
			return;
		}
		GameManager.Instance.menuManager.PopMenuToState (UIManager.State.Game);
		//startime = true;
	}
	
	public void StopGame(){
		GameManager.Instance.menuManager.PushMenu(UIManager.State.GiveUPPopUP);
		//GameManager.Instance.menuManager.PopMenuToState (UIManager.State.MainMenu);
		GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
		//GameManager.Instance.Scroll.transform.parent.GetChild(0).gameObject.SetActive (true);
	}
}
