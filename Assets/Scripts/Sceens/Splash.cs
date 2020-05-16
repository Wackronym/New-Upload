using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Splash : BaseUI {


	void Start () {
		if (PlayerPrefs.HasKey("Unlimited"))
		{
			GameManager.Instance.unlimited = true;
		}

	    int coins = GameManager.Instance.Coin();
    }
	
	public void MainSceen(){
		
			GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
			GameManager.Instance.menuManager.PushMenu (UIManager.State.MainMenu);
		
		
	}
}
