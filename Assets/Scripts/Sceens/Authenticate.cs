
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using BestHTTP;
using BestHTTP.JSON;
using System;
using System.Collections.Generic;
using UnityEngine.Networking;

public class Authenticate : BaseUI
{

    #region UI Variable Statement 
    //Ghilman
    [SerializeField] public GameObject updateProfilePanel;
    [SerializeField] public GameObject changePasswordPanel;

    [SerializeField] public InputField update_Text_fName;
    [SerializeField] public InputField update_Text_lName;
    [SerializeField] public InputField update_Text_email;
    [SerializeField] public InputField update_Text_username;

    [SerializeField] public InputField ChangePass_Text_CurrentPass;
    [SerializeField] public InputField ChangePass_Text_NewPass;
    [SerializeField] public InputField ChangePass_Text_VerifyPass;

    //Ghilman

    [SerializeField] public GameObject registerPanel; 
	[SerializeField] public GameObject loginPanel; 
	[SerializeField] public GameObject forgetPanel; 
	[SerializeField] private bool isConnecting; 
	[SerializeField] private int loginHudIndex = 0;
	[SerializeField] public InputField login_Text_email;
	[SerializeField] public InputField login_Text_password;
	
	[SerializeField] public InputField reg_Text_fName;
	[SerializeField] public InputField reg_Text_lName;
	[SerializeField] public InputField reg_Text_email;
	[SerializeField] public InputField reg_Text_password;
	[SerializeField] public InputField reg_Text_username;
	[SerializeField] public InputField forget_Text_email;
	public Text statusAuth;
	
	
	#endregion 
	void Start () {
		transform.parent = GameManager.Instance.topPopup.transform;
        //Ghilman
        SetAuthenticatePanel();
        //Ghilman
        //base.AddMouseDownEvent();
    }

    //Ghilman
    void SetAuthenticatePanel()
    {
        if (GameManager.Instance.authenticateState == AuthenticateState.Login)
        {
            registerPanel.SetActive(false);
            updateProfilePanel.SetActive(false);
            changePasswordPanel.SetActive(false);
            loginPanel.SetActive(true);
            
        }
        else if (GameManager.Instance.authenticateState == AuthenticateState.UpdateProfile)
        {
            registerPanel.SetActive(false);
            loginPanel.SetActive(false);
            changePasswordPanel.SetActive(false);

            updateProfilePanel.SetActive(true);
            Player _player = GameManager.Instance.player;

            update_Text_fName.text = _player.firstName;
            update_Text_lName.text = _player.lastName;
            update_Text_email.text = _player.email;
            update_Text_username.text = _player.displayName;

}
        else if(GameManager.Instance.authenticateState == AuthenticateState.Signup)
        {
            changePasswordPanel.SetActive(false);
            updateProfilePanel.SetActive(false);
            loginPanel.SetActive(false);
            registerPanel.SetActive(true);
        }
    }

    public void OnSignupButtonClick()
    {
        GameManager.Instance.authenticateState = AuthenticateState.Signup;
        loginPanel.SetActive(false);
        registerPanel.SetActive(true);
    }
    //Ghilman


    public void GoLogIn() {
		
		//string user = "";
		//string pass = "";
			
		//user = "ggg@gmail.com";
		//pass = "!N123456789n";
			
		//user = "frtnauma@gmail.com";
		//pass = "n123456789!N";
		
		
		isConnecting = true;
		loginHudIndex = 0;
        //Gdx.input.setOnscreenKeyboardVisible(false);
        
            //

        HTTPRequest www = new HTTPRequest(new Uri( GameManager.Instance.webURLPrefix + "auth/signin"),HTTPMethods.Post,(request, response) => {
			HTTPResponse res = (HTTPResponse)response;
            Debug.Log(res.DataAsText);
            Debug.Log("res is up");
            if (res.IsSuccess){
				string request_result = res.DataAsText;
				if (request_result.Length > 10) {	
					GameManager.Instance.header =  res.Headers;
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
					GameManager.Instance.menuManager.PopMenu();
					Profile p  = GameManager.Instance.topPopup.GetComponentInChildren<Profile>();
					if(p != null){
						p.RefresUI();
					}
					
					
					
					//if(Application.isEditor){
					//	PlayerPrefs.SetString("usernameOrEmail", user);
					//	PlayerPrefs.SetString("password", pass);
					//}
					//else{
						PlayerPrefs.SetString("usernameOrEmail", login_Text_email.text);
						PlayerPrefs.SetString("password", login_Text_password.text);
					//}


				}
                statusAuth.text = "successful!";
            }
			else{
				statusAuth.text = "Incorrect User!";
			}
			
			isConnecting = false;
		});

        //if(Application.isEditor){

        //www.AddField("usernameOrEmail", "frtnauma@gmail.com");
        //	www.AddField("password", pass);
        //}
        //else{
       
       // www.AddField("user", "ghilman44@gmail.com");
        //www.AddField("firstName", "Ghilman");
       // www.AddField("lastName", "Ghilman");
       // www.AddField("displayName", "ghilman44@gmail.com");
       // www.AddField("email", "Ghilman");

         www.AddField("usernameOrEmail", login_Text_email.text);
	     www.AddField("password", login_Text_password.text);
		//}
		
			
		www.Send();
	}
	public void Register() {
		isConnecting = true;
		loginHudIndex = 0;
		HTTPRequest www = new HTTPRequest(new Uri( GameManager.Instance.webURLPrefix + "auth/signup"),HTTPMethods.Post,(request, response) => {
			HTTPResponse res = (HTTPResponse)response;
			Debug.Log( res.DataAsText);
			if(res.IsSuccess){
				string request_result = res.DataAsText;
				if (request_result.Length > 10) {
					Dictionary<string, object> b = Json.Decode(request_result) as Dictionary<string, object>;
					GameManager.Instance.player = new Player();
					GameManager.Instance.player._id 					=  		b["_id"].ToString();
					GameManager.Instance.player.displayName 			=  		b["displayName"].ToString();
					GameManager.Instance.player.provider 				=  		b["provider"].ToString();
					GameManager.Instance.player.username 				=  		b["username"].ToString();
					GameManager.Instance.player.__v 					=  		int.Parse(b["__v"].ToString());
					GameManager.Instance.player.profileImageURL 		=  		b["profileImageURL"].ToString();
					GameManager.Instance.player.email 					=  		b["email"].ToString();
					GameManager.Instance.player.lastName 				=  		b["lastName"].ToString();
					GameManager.Instance.player.firstName 				=  		b["firstName"].ToString();
					GameManager.Instance.player.created 				=  		b["created"].ToString();
					Debug.Log( b["roles"].GetType());
					GameManager.Instance.player.roles  = new List<string>();
					foreach(object t in b["roles"] as List<object>){
						GameManager.Instance.player.roles.Add(t.ToString());
					}
					GameManager.Instance.menuManager.PopMenu();
					Profile p  = GameManager.Instance.topPopup.GetComponentInChildren<Profile>();
					if(p != null){
						p.RefresUI();
					}
					isConnecting = false;
                    
                }
                statusAuth.text = "successful!";
            }
			else{
				if( res.DataAsText.Contains("The password must be at least 10 characters long")){
					statusAuth.text = "The password must be at least 10 characters long.";
				}
				else if( res.DataAsText.Contains("The password may not contain sequences of three or more repeated characters.")){
					statusAuth.text = "The password may not contain sequences of three or more repeated characters.";
				}
				else if( res.DataAsText.Contains("Please enter a valid username: 3+ characters long, non restricted word, characters \"_-.\", no consecutive dots, does not begin or end with dots, letters a-z and numbers 0-9.")){
					statusAuth.text = "Please enter a valid username: 3+ characters long, non restricted word, characters \"_-.\", no consecutive dots, does not begin or end with dots, letters a-z and numbers 0-9.";
				}
				else{
					statusAuth.text = "Unknown Error.";
				}
			}
			isConnecting = false;
		});
		www.AddField("firstName", reg_Text_fName.text);
		www.AddField("lastName", reg_Text_lName.text);
		www.AddField("email", reg_Text_email.text);
		www.AddField("password", reg_Text_password.text);
		www.AddField("username", reg_Text_username.text);
		www.Send();
	}


    //Ghilman
    public void Reset() {
       StartCoroutine (Upload());
    }

    IEnumerator Upload()
    {
        Debug.Log("Upload forget request");
        WWWForm form = new WWWForm();
        form.AddField("usernameOrEmail", forget_Text_email.text);

        using (UnityWebRequest www = UnityWebRequest.Post("https://wackronym.net/api/auth/forgot", form))
            {
            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            yield return www.Send();

            if (www.isError)
            {
                Debug.Log(www.error);
                statusAuth.text = "error!";
            }
            else
            {
                statusAuth.text = "successful!";
                Debug.Log("POST successful!");
                // Print Body
                Debug.Log(www.downloadHandler.text);
                }
            }
    }

    public void OnUpdateProfileCloseButtonClick()
    {
        GameManager.Instance.menuManager.PopMenu();
    }
    public void OnChangePassPPButtonClick()
    {
        GameManager.Instance.authenticateState = AuthenticateState.changePassword;
        updateProfilePanel.SetActive(false);
        changePasswordPanel.SetActive(true);
    }
    public void OnChangePasswordCloseButtonClick()
    {
        GameManager.Instance.authenticateState = AuthenticateState.UpdateProfile;
        changePasswordPanel.SetActive(false);
        updateProfilePanel.SetActive(true);
    }
    public void OnUptateProfileButtonClick()
    {
        Debug.Log("this is the function for uptade profie");

        isConnecting = true;
        loginHudIndex = 0;
        HTTPRequest www = new HTTPRequest(new Uri("https://wackronym.net/api/users/mobileEditUser"), HTTPMethods.Post, (request, response) => {
            HTTPResponse res = (HTTPResponse)response;
            Debug.Log(res.DataAsText);
            if (res.IsSuccess)
            {
                string request_result = res.DataAsText;
                if (request_result.Length > 10)
                {
                    GameManager.Instance.header = res.Headers;
                }
                ProfileIsUpdated();
                statusAuth.text = "successful!";
            }
            else
            {
                statusAuth.text = "Field to updated information!";
            }
            isConnecting = false;
        });

        bool canSend = false;
        string errorMessage = "";

        www.AddField("user", GameManager.Instance.player._id);

        ///check for first name. 
        if (!string.IsNullOrEmpty(update_Text_fName.text)){
            www.AddField("firstName", update_Text_fName.text);
            canSend = true;
        }else{
            canSend = false;
            errorMessage = "Please enter the first name.";
        }

        /// check for last name. 
        if (!string.IsNullOrEmpty(update_Text_lName.text)){
            www.AddField("lastName", update_Text_lName.text);
            canSend = true;
        }else{
            canSend = false;
            errorMessage = "Please enter the last name.";
        }
        /// check for display name. 
        if (!string.IsNullOrEmpty(update_Text_username.text)){
            www.AddField("displayName", update_Text_username.text);
            canSend = true;
        }else{
            canSend = false;
            errorMessage = "Please enter the display name.";
        }

        /// check for email.
        if (!string.IsNullOrEmpty(update_Text_email.text)){
            www.AddField("email", update_Text_email.text);
            canSend = true;
        }else{
            canSend = false;
            errorMessage = "Please enter the email first.";
        }
       
        if (canSend){
            www.Send();
        }else{
            statusAuth.text = errorMessage;
        }
    }

    void ProfileIsUpdated()
    {
        PlayerPrefs.SetString("usernameOrEmail", login_Text_email.text);

        GameManager.Instance.player.firstName = update_Text_fName.text;
        GameManager.Instance.player.lastName = update_Text_lName.text;
        GameManager.Instance.player.displayName = update_Text_username.text;
        GameManager.Instance.player.email = update_Text_email.text;

        GameManager.Instance.menuManager.PopMenu();
        Profile p = GameManager.Instance.topPopup.GetComponentInChildren<Profile>();
        if (p != null)
        {
            p.RefresUI();
        }
    }
    public void OnChangePasswordButtonClick()
    {
        Debug.Log("please change the password");
        isConnecting = true;
        loginHudIndex = 0;
        HTTPRequest www = new HTTPRequest(new Uri("https://wackronym.net/api/users/password"), HTTPMethods.Post, (request, response) => {
            HTTPResponse res = (HTTPResponse)response;
            Debug.Log(res.DataAsText);
            Debug.Log("res is up");
            if (res.IsSuccess)
            {
                string request_result = res.DataAsText;
                if (request_result.Length > 10)
                {
                    GameManager.Instance.header = res.Headers;
                    Debug.Log(res.Data);
                    //GameManager.Instance.player = Json.Decode(request_result) as Player;
                    Dictionary<string, object> b = Json.Decode(request_result) as Dictionary<string, object>;
                    foreach (string key in b.Keys)
                    {
                        Debug.Log("key = " + key + " value = " + b[key]);
                    }
                    Debug.Log(res.Data);
                }
                OnChangePasswordCloseButtonClick();
                statusAuth.text = "successful!";
            }
            else
            {
                statusAuth.text = "Incorrect User!";
            }

            isConnecting = false;
        });
       // currentPassword, newPassword, verifyPassword, userId

        string _userId = GameManager.Instance.player._id;
        www.AddField("currentPassword", ChangePass_Text_CurrentPass.text);
        www.AddField("newPassword", ChangePass_Text_NewPass.text);
        www.AddField("verifyPassword", ChangePass_Text_VerifyPass.text);
        www.AddField("userId", _userId);
        www.Send();
    }
 

    //Ghilman
	public void CloseAuthBox(){
        //Ghilman
        GameManager.Instance.menuManager.PopMenuToState(UIManager.State.MainMenu);
        //Ghilman

        //if(GameManager.Instance.player.username==""){
        //	GameManager.Instance.menuManager.PopMenuToState(UIManager.State.MainMenu);
        //}
    }
}
