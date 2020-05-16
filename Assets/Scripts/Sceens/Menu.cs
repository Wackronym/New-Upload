using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SocialPlatforms.GameCenter;
using MaterialUI;
using BestHTTP;
using BestHTTP.JSON;
public class Menu : BaseUI {
	public TextMesh XP;
    public TextMesh Clock;
    public TextMesh Coins;
    public TextMesh Level;
	public TextMesh Scores;
	
	void Start () {
		GameManager.Instance.menuManager.previousState = UIManager.State.MainMenu;
		GameManager.Instance.menuManager.currentState = UIManager.State.MainMenu;
		AudioListener.pause = false;
        Time.timeScale = 1;
	    
	    if (PlayerPrefs.HasKey("Unlimited"))
	    {
		    GameManager.Instance.unlimited = true;
	    }
		GameManager.Instance.backButton.transform.parent.gameObject.SetActive(true);
		base.AddMouseDownEvent();
		if(PlayerPrefs.HasKey("usernameOrEmail")){
			GoLogIn();
		}
	}
    
    
	public void GoLogIn() {

		//Gdx.input.setOnscreenKeyboardVisible(false);
		HTTPRequest www = new HTTPRequest(new Uri( GameManager.Instance.webURLPrefix + "auth/signin"),HTTPMethods.Post,(request, response) => {
			HTTPResponse res = (HTTPResponse)response;
			
			if(res.IsSuccess){
				string request_result = res.DataAsText;
				if (request_result.Length > 10) {
					GameManager.Instance.header =  res.Headers;
					Debug.Log( res.DataAsText);
					Debug.Log(res.Data);
					
					//GameManager.Instance.player = Json.Decode(request_result) as Player;
					Dictionary<string, object> b = Json.Decode(request_result) as Dictionary<string, object>;
					GameManager.Instance.player = new Player();
					if(b.ContainsKey("_id"))
						GameManager.Instance.player._id 					=  		b["_id"].ToString();
					if(b.ContainsKey("displayName"))
						GameManager.Instance.player.displayName 			=  		b["displayName"].ToString();
					if(b.ContainsKey("provider"))
						GameManager.Instance.player.provider 				=  		b["provider"].ToString();
					if(b.ContainsKey("username"))
						GameManager.Instance.player.username 				=  		b["username"].ToString();
					if(b.ContainsKey("__v"))
						GameManager.Instance.player.__v 					=  		int.Parse(b["__v"].ToString());
					if(b.ContainsKey("resetPasswordToken"))
						GameManager.Instance.player.resetPasswordToken 		=  		b["resetPasswordToken"].ToString();
					if(b.ContainsKey("resetPasswordExpires"))
						GameManager.Instance.player.resetPasswordExpires 	=  		b["resetPasswordExpires"].ToString();
					if(b.ContainsKey("profileImageURL"))
						GameManager.Instance.player.profileImageURL 		=  		b["profileImageURL"].ToString();
					if(b.ContainsKey("email"))
						GameManager.Instance.player.email 					=  		b["email"].ToString();
					if(b.ContainsKey("lastName"))
						GameManager.Instance.player.lastName 				=  		b["lastName"].ToString();
					if(b.ContainsKey("firstName"))
						GameManager.Instance.player.firstName 				=  		b["firstName"].ToString();
					if(b.ContainsKey("created"))
						GameManager.Instance.player.created 				=  		b["created"].ToString();
					if(b.ContainsKey("roles")){
						Debug.Log( b["roles"].GetType());
						GameManager.Instance.player.roles  = new List<string>();
						foreach(object t in b["roles"] as List<object>){
							GameManager.Instance.player.roles.Add(t.ToString());
						}
					}
					//GameManager.Instance.menuManager.PopMenu();
					//Profile p  = GameManager.Instance.topPopup.GetComponentInChildren<Profile>();
					//if(p != null){
					//	p.RefresUI();
					//}
					
					//PlayerPrefs.SetString("usernameOrEmail", login_Text_email.text);
					//PlayerPrefs.SetString("password", login_Text_password.text);
				}
			}
		});
		
		
		www.AddField("usernameOrEmail", PlayerPrefs.GetString("usernameOrEmail"));
		www.AddField("password", PlayerPrefs.GetString("password"));
		www.Send();
		
		
		
			
		
	}

	void Update () {
		DateTime time = DateTime.Now;
	}

	public void Home(){
		GetComponentsInChildren<TabItem>(true)[0].HomeTab(0);
	}
	
	public void Play(){
 
	    GameManager.Instance.menuManager.PushMenu(UIManager.State.Settings);
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.beginLevel);
    }

	public void Setting(){
		if(GameManager.Instance.menuManager.NavigationStackPeek() == UIManager.State.Settings){
			return;
		}
		GameManager.Instance.menuManager.PushMenu (UIManager.State.Settings);
	}
    
	public void PopSetting(){
		if(GameManager.Instance.menuManager.NavigationStackPeek() == UIManager.State.Settings){
			GameManager.Instance.menuManager.PopMenuToState(UIManager.State.MainMenu);
		}
	}
    public void UpdateUI()
    {
        Coins.text = GameManager.Instance.Coin().ToString();
    }
}
