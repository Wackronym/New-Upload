using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsPopup : BaseUI
{
    public GameObject close;
	// Use this for initialization
	void Start () {
        Time.timeScale = 1;
        Invoke("ShowClose", 5f);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OpenLink()
    {
        Application.OpenURL("https://www.amazon.com/Inari-Games-Charlies-Heist/dp/B079647KJW/ref=sr_1_1?s=digital-skills&ie=UTF8&qid=1518026041&sr=1-1&keywords=charlie%27s+heist");
    }

    public void ShowClose()
    {
        close.SetActive(true);
        
    }

   
}
