using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Win : BaseUI {
	
	void Start () {
		base.AddMouseDownEvent();
    }

	public void MainMenu(){
		GameManager.Instance.menuManager.PopMenuToState (UIManager.State.MainMenu);
		GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
		GameManager.Instance.Scroll.transform.parent.GetChild(0).gameObject.SetActive (true);
    }
	public void PlayLevel(){
		GameManager.Instance.menuManager.PopMenuToState (UIManager.State.MainMenu);
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
        GameManager.Instance.menuManager.PushMenu (UIManager.State.Game);
	}
	public void Restart(){
		GameManager.Instance.menuManager.PopMenuToState (UIManager.State.MainMenu);
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
    }
}
