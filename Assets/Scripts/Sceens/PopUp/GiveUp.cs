using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiveUp : BaseUI
{

    void Start()
	{
		GameManager.Instance.errorPopup.SetActive (false);
		//GameManager.Instance.GetComponentInChildren<Game>().startime = false;
		base.AddMouseDownEvent();
    }
	public void ClosePopup(){

		if(GameManager.Instance.menuManager.NavigationStackPeek() == UIManager.State.GiveUPPopUP){
			GameManager.Instance.menuManager.PopMenu();
			GameManager.Instance.GetComponentInChildren<Game>().startime = true;
		}
	}

    public void GiveUpGame()
    {
		if(GameManager.Instance.menuManager.NavigationStackPeek() == UIManager.State.GiveUPPopUP){
			GameManager.Instance.menuManager.PopMenuToState(UIManager.State.MainMenu);
			GameManager.Instance.Scroll.transform.parent.gameObject.SetActive(true);
		}
    }
	
}