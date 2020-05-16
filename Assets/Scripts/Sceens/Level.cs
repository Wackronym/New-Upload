using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : BaseUI {
	
	void Start () {
		GameManager.Instance.Scroll.transform.parent.gameObject.SetActive (false);
	}

	public void MainMenu(){
		GameManager.Instance.menuManager.PopMenuToState (UIManager.State.MainMenu);
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
    }
	public void PlayLevel(){
		GameManager.Instance.menuManager.PushMenu (UIManager.State.Game);
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
    }
	public void Restart(){
		GameManager.Instance.menuManager.PopMenu ();
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
    }
}
