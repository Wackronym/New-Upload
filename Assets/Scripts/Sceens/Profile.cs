using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using BestHTTP;
using System;

public class Profile : BaseUI {
    
	public InputField firstName;
	public InputField lastName;
	public InputField email;
	public InputField username;
	public Image profilePic;
	
	void Start () {
		transform.parent = GameManager.Instance.topPopup.transform;
		if(GameManager.Instance.player.username==""){
			if(GameManager.Instance.menuManager.NavigationStackPeek() == UIManager.State.Profile){
                //Ghilman
                 GameManager.Instance.authenticateState = AuthenticateState.Login; 
                //Ghilman
                GameManager.Instance.menuManager.PushMenu(UIManager.State.Authenticate);
			}
		}
		else{
			RefresUI();
		}
		base.AddMouseDownEvent();
	}
	
	
	public void RefresUI(){
		string url = "https://159.203.125.111/" + GameManager.Instance.player.profileImageURL.TrimStart('/');
		Debug.Log("WWW download >" + url);
		
		new HTTPRequest(new Uri(url), (request, response) =>
		{
			var tex = new Texture2D(0, 0);
			HTTPResponse res = (HTTPResponse)response;
			tex.LoadImage(res.Data);
			if(profilePic!=null)
			profilePic.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f), 40);
		}).Send();

        Debug.Log("user is login");
		firstName.text = "First Name: "+GameManager.Instance.player.firstName;
		lastName.text = "Last Name: "+GameManager.Instance.player.lastName;
		email.text = "Email : "+GameManager.Instance.player.email;
		username.text = GameManager.Instance.player.username;
	}

	public void LogOut(){
		GameManager.Instance.player.username = "";
		PlayerPrefs.DeleteKey("usernameOrEmail");
		PlayerPrefs.DeleteKey("password");
		GameManager.Instance.menuManager.PopMenu();
	}
    //Ghilman
    public void OnCloseButtonClick()
    {
        GameManager.Instance.menuManager.PopMenu();
    }

    public void OnEditProfileButtonClick()
    {
        GameManager.Instance.authenticateState = AuthenticateState.UpdateProfile;
        GameManager.Instance.menuManager.PushMenu(UIManager.State.Authenticate);
    }
    //Ghilman
}
