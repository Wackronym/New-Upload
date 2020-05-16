using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using System;
using UnityEngine.EventSystems;
using BestHTTP;
using BestHTTP.JSON;

public class UserItem : MonoBehaviour {

	public Text mNameText;
	public Image mIcon;
	public Text mDescText;
	public GameObject mContentRootObj;
	int mItemDataIndex = -1;
	public LoopListView2 mLoopListView;
	public UserData data;
	public Text buttonName;
	public GameObject dialog;
	public List<UserData> mItemDataList = new List<UserData>();
	public List<PendingFriendData> pList = new List<PendingFriendData>();
	GameObject go;
	public void Init()
	{
		mIcon.gameObject.SetActive(false);
	}
	public void SetItemData(UserData itemData,int itemIndex)
	{
		if(go!=null){
			buttonName.transform.parent.localScale = new Vector3(1,1,1);
			go.SetActive(false);
		}
		mIcon.gameObject.SetActive(false);
		data = itemData;
		EventTrigger trigger = buttonName.GetComponentInParent<EventTrigger>();
		if(trigger!=null){
			trigger.triggers.Clear();
			EventTrigger.Entry entry = new EventTrigger.Entry();
			entry.eventID = EventTriggerType.PointerDown;
			entry.callback.AddListener( (eventData) => OnBtnClicked(buttonName)  );
			trigger.triggers.Add(entry);
		}
		mNameText.text = data.displayName;
		mDescText.text = data.username;
		buttonName.text = "Invite";
		buttonName.transform.parent.gameObject.SetActive(true);
		buttonName.transform.parent.GetComponent<EventTrigger>().enabled = true;
		GetAllFriendsList();
		if(data.status == "pending"){
			buttonName.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 50);
			buttonName.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-120, 0);
			buttonName.text = "Cancel Request"; 
			HTTPRequest www = new HTTPRequest(new Uri(  GameManager.Instance.webURLPrefix + "findFriend?search="+data.pd.user.username), (request, response) => {
				HTTPResponse res = (HTTPResponse)response;
				if(res.IsSuccess){
					List<object> dic = Json.Decode(res.DataAsText) as List<object>;
					List<UserData> pp = new List<UserData>();
					foreach(Dictionary<string, object> a in dic){
						UserData s = new UserData();
						if(a.ContainsKey("_id"))
							s._id = a["_id"].ToString();
						if(a.ContainsKey("displayName"))
							s.displayName = a["displayName"].ToString();
						if(a.ContainsKey("username"))
							s.username = a["username"].ToString();
						if(a.ContainsKey("profileImageURL"))
							s.profileImageURL = a["profileImageURL"].ToString();
							
						if(a.ContainsKey("pending")){
							s.status = "pending";
						}
						pp.Add(s);
					}
					
					string url = "https://159.203.125.111/" + pp[0].profileImageURL.TrimStart('/');
					Debug.Log("WWW download >" + url);
					new HTTPRequest(new Uri(url), (request1, response1) =>
					{
						var tex = new Texture2D(0, 0);
						HTTPResponse ret = (HTTPResponse)response1;
						tex.LoadImage(ret.Data);
						mIcon.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f), 40);
						mIcon.gameObject.SetActive(true);
					}).Send();
				}
			}).Send(); 	
		}
		
		if(data._id == GameManager.Instance.player._id && data.status == "pending"){
			buttonName.text = "Accept";
			mNameText.text = data.pd.user.displayName;
			mDescText.text = data.pd.user.username;
			buttonName.transform.parent.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(180, 50);
			buttonName.transform.parent.gameObject.GetComponent<RectTransform>().anchoredPosition = new Vector2(-90, 0);
			go = Instantiate(buttonName.transform.parent.gameObject);
			go.SetActive(true);
			go.transform.parent = buttonName.transform.parent.parent;
			go.transform.position = buttonName.transform.parent.position;
			go.transform.localScale = buttonName.transform.parent.localScale;
			go.transform.localScale =  new Vector3(1f/1.5f,1,1);
			buttonName.transform.parent.localScale =  new Vector3(1f/1.5f,1,1);
			go.GetComponentInChildren<Text>().text = "Decline";
			go.GetComponent<RectTransform>().anchoredPosition = new Vector2(-210, 0);
			EventTrigger trigger1 = go.GetComponent<EventTrigger>();
			trigger1.triggers.Clear();
			EventTrigger.Entry entry1 = new EventTrigger.Entry();
			entry1.eventID = EventTriggerType.PointerDown;
			entry1.callback.AddListener( (eventData1) => OnBtnClickedCancel(buttonName)  );
			trigger1.triggers.Add(entry1);
			HTTPRequest www = new HTTPRequest(new Uri(  GameManager.Instance.webURLPrefix + "findFriend?search="+data.pd.user.username), (request, response) => {
				HTTPResponse res = (HTTPResponse)response;
				if(res.IsSuccess){
					List<object> dic = Json.Decode(res.DataAsText) as List<object>;
					List<UserData> pp = new List<UserData>();
					foreach(Dictionary<string, object> a in dic){
						UserData s = new UserData();
						if(a.ContainsKey("_id"))
							s._id = a["_id"].ToString();
						if(a.ContainsKey("displayName"))
							s.displayName = a["displayName"].ToString();
						if(a.ContainsKey("username"))
							s.username = a["username"].ToString();
						if(a.ContainsKey("profileImageURL"))
							s.profileImageURL = a["profileImageURL"].ToString();
							
						if(a.ContainsKey("pending")){
							s.status = "pending";
						}
						pp.Add(s);
					}
					string url = "https://159.203.125.111/" + pp[0].profileImageURL.TrimStart('/');
					Debug.Log("WWW download >" + url);
					new HTTPRequest(new Uri(url), (request1, response1) =>
					{
						var tex = new Texture2D(0, 0);
						HTTPResponse ret = (HTTPResponse)response1;
						tex.LoadImage(ret.Data);
						mIcon.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f), 40);
						mIcon.gameObject.SetActive(true);
					}).Send();
				}
			}).Send(); 	
		}
		if( data.status == "accept"){
			buttonName.text = "Unfriend";
			if(data._id == GameManager.Instance.player._id){
				mNameText.text = data.pd.user.displayName;
				mDescText.text = data.pd.user.username;
				HTTPRequest www = new HTTPRequest(new Uri(  GameManager.Instance.webURLPrefix + "findFriend?search="+data.pd.user.username), (request, response) => {
					HTTPResponse res = (HTTPResponse)response;
					if(res.IsSuccess){
						
						List<object> dic = Json.Decode(res.DataAsText) as List<object>;
						List<UserData> pp = new List<UserData>();
						foreach(Dictionary<string, object> a in dic){
							UserData s = new UserData();
							if(a.ContainsKey("_id"))
								s._id = a["_id"].ToString();
							if(a.ContainsKey("displayName"))
								s.displayName = a["displayName"].ToString();
							if(a.ContainsKey("username"))
								s.username = a["username"].ToString();
							if(a.ContainsKey("profileImageURL")){
								s.profileImageURL = a["profileImageURL"].ToString();	
							}
							if(a.ContainsKey("pending")){
								s.status = "pending";
							}
							pp.Add(s);
						}
						
						string url = "https://159.203.125.111/" + pp[0].profileImageURL.TrimStart('/');
						Debug.Log("WWW download >" + url);
						new HTTPRequest(new Uri(url), (request1, response1) =>
						{
							var tex = new Texture2D(0, 0);
							HTTPResponse ret = (HTTPResponse)response1;
							tex.LoadImage(ret.Data);
							mIcon.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f), 40);
							mIcon.gameObject.SetActive(true);
						}).Send();
					}
				}).Send(); 	
			}
			else{
				Search(data.pd.touser.username);
			}
		}

		if( data.profileImageURL!=null){
			string url = "https://159.203.125.111/" + data.profileImageURL.TrimStart('/');
			Debug.Log("WWW download >" + url);
			new HTTPRequest(new Uri(url), (request, response) =>
			{
				var tex = new Texture2D(0, 0);
				HTTPResponse res = (HTTPResponse)response;
				tex.LoadImage(res.Data);
				mIcon.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f), 40);
				mIcon.gameObject.SetActive(true);
			}).Send();
		}
	}
	
	public void DelRecordYes(Text b){
		HTTPRequest www = new HTTPRequest(new Uri(GameManager.Instance.webURLPrefix + "friends/" + data.requestId),HTTPMethods.Delete,(request, response) => {
			HTTPResponse res = (HTTPResponse)response;
			if(res.IsSuccess){
				string request_result = res.DataAsText;
				if (request_result.Length > 10) {	
					Debug.Log( res.DataAsText);
					b.text = "Friend";
					gameObject.SetActive(false);
					b.transform.parent.GetComponent<EventTrigger>().enabled = false;
				}
			}
			else{
				Debug.Log(res.StatusCode);
				Debug.Log(res.Message);
				Debug.Log(res.DataAsText);
			}
		});
		for(int i = 0; i < GameManager.Instance.header.Count; ++i)
		{
			foreach(KeyValuePair<string, List<string>> item in GameManager.Instance.header)
			{   	
				foreach(string val in item.Value){
					
				}
				www.AddField(item.Key, item.Value[0]);
			}
		}
		
		www.Send(); 
		return;
	}
	
	public void DeclineYes(Text b){
		HTTPRequest www = new HTTPRequest(new Uri(GameManager.Instance.webURLPrefix + "friends/" + data.requestId),HTTPMethods.Put,(request, response) => {
			HTTPResponse res = (HTTPResponse)response;
			
			if(res.IsSuccess){
				string request_result = res.DataAsText;
				if (request_result.Length > 10) {	
					Debug.Log( res.DataAsText);
					b.text = "Friend";
					gameObject.SetActive(false);
					b.transform.parent.GetComponent<EventTrigger>().enabled = false;
					DelRecordYes(b);
				}
			}
			else{
				Debug.Log(res.StatusCode);
				Debug.Log(res.Message);
				Debug.Log(res.DataAsText);
			}
		});
		www.AddField("status", "accept");
		www.Send();
		return;
	}

	public void OnBtnClickedCancel(Text b){
		DeclineYes(b);
	}
	
	public void UnFriend(Text b){
		
		Debug.Log(GameManager.Instance.webURLPrefix + "https://wackronym.net/api/friends/" + data.requestId);
		HTTPRequest www = new HTTPRequest(new Uri( "https://wackronym.net/api/" + "friends/" + data.requestId),HTTPMethods.Delete,(request, response) => {
			HTTPResponse res = (HTTPResponse)response;
			
			if(res.IsSuccess){
				string request_result = res.DataAsText;
				if (request_result.Length > 10) {	
					Debug.Log( res.DataAsText);
							
					if(data.status == "accept"){
						gameObject.SetActive(false);
						dialog.SetActive(false);
					}
					else{
						if(b.text != "Cancel Request"){
							b.text = "Pending";
						}
						else{
							gameObject.SetActive(false);
						}
								
					}
					b.transform.parent.GetComponent<EventTrigger>().enabled = false;
				}
			}
			else{
				Debug.Log(res.StatusCode);
				Debug.Log(res.Message);
				Debug.Log(res.DataAsText);
			}
		});
		www.Send();
		return;
	}
	
	
	public void OnBtnClicked(Text b)
	{
		GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
		if(GameManager.Instance.menuManager.NavigationStackPeek()!=UIManager.State.Win){
			Debug.Log(data.status);
			Debug.Log(data.displayName);
			if(buttonName.text == "Accept"){
				HTTPRequest www = new HTTPRequest(new Uri(GameManager.Instance.webURLPrefix + "friends/" + data.requestId),HTTPMethods.Put,(request, response) => {
					HTTPResponse res = (HTTPResponse)response;
					if(res.IsSuccess){
						
						string request_result = res.DataAsText;
						if (request_result.Length > 10) {	
							Debug.Log( res.DataAsText);
							b.text = "Friend";
							gameObject.SetActive(false);
							b.transform.parent.GetComponent<EventTrigger>().enabled = false;
						}
					}
					else{
						Debug.Log(res.StatusCode);
						Debug.Log(res.Message);
						Debug.Log(res.DataAsText);
					}
				});
				www.AddField("status", "accept");
				www.Send();
				return;
			}//if(){
			else if(data.status == "pending" || data.status == "accept"){
				
				if( data.status == "accept"){
					dialog.SetActive(true);
					EventTrigger[] tt = dialog.transform.GetChild(1).GetComponentsInChildren<EventTrigger>(true);
					foreach(EventTrigger trigger in tt){
						if(trigger.gameObject.name == "Stop"){
							trigger.triggers.Clear();
							EventTrigger.Entry entry = new EventTrigger.Entry();
							entry.eventID = EventTriggerType.PointerDown;
							entry.callback.AddListener( (eventData) => UnFriend(buttonName)  );
							trigger.triggers.Add(entry);
						}
		
					}
					return;
				}
				
				else if(data.status == "pending"){
					Debug.Log(GameManager.Instance.webURLPrefix + "https://wackronym.net/api/friends/" + data.requestId);
					HTTPRequest www = new HTTPRequest(new Uri( "https://wackronym.net/api/" + "friends/" + data.requestId),HTTPMethods.Delete,(request, response) => {
						HTTPResponse res = (HTTPResponse)response;
						if(res.IsSuccess){
							string request_result = res.DataAsText;
							if (request_result.Length > 10) {	
								Debug.Log( res.DataAsText);
							
								if(data.status == "accept"){
									gameObject.SetActive(false);
									dialog.SetActive(false);
								}
								else{
									if(buttonName.text != "Cancel Request"){
										b.text = "Pending";
									}
									else{
										gameObject.SetActive(false);
									}
								}
								b.transform.parent.GetComponent<EventTrigger>().enabled = false;
							}
						}
						else{
							Debug.Log(res.StatusCode);
							Debug.Log(res.Message);
							Debug.Log(res.DataAsText);
						}
					});
					www.Send();
					return;
				}
			}
			if(b.text == "Pending"){
				return;
			}
			
			HTTPRequest www2 = new HTTPRequest(new Uri( GameManager.Instance.webURLPrefix + "friends"),HTTPMethods.Post,(request, response) => {
				HTTPResponse res = (HTTPResponse)response;
			
				if(res.IsSuccess){
					string request_result = res.DataAsText;
					if (request_result.Length > 10) {	
						Debug.Log( res.DataAsText);
						b.text = "Pending";
						GameManager.Instance.BroadcastMessage("Refresh");
						//b.transform.parent.GetComponent<EventTrigger>().enabled = false;
					}
				}
				else{
					Debug.Log(res.StatusCode);
					Debug.Log(res.Message);
					Debug.Log(res.DataAsText);
				}
			});
			www2.AddField("user", GameManager.Instance.player._id);
			www2.AddField("touser", data._id);
			www2.AddField("status", "pending");
			www2.Send();
		}
	}
	public void GetAllFriendsList()
	{
		string tabName = buttonName.transform.parent.parent.parent.parent.parent.parent.parent.name;
		//Debug.LogError(tabName);
		if(tabName != "Add Friends"){
			return;
		}
		buttonName.transform.parent.gameObject.SetActive(false);
		HTTPRequest www = new HTTPRequest(new Uri(  GameManager.Instance.webURLPrefix + "activeFriend?myId=" + GameManager.Instance.player._id), (request, response) => {
			HTTPResponse res = (HTTPResponse)response;
			if(res.IsSuccess){
				Debug.Log(res.DataAsText);
				List<object> dic = Json.Decode(res.DataAsText) as List<object>;
				pList = new List<PendingFriendData>();
				foreach(Dictionary<string, object> a in dic){
					UserData s = new UserData();
					PendingFriendData p = new PendingFriendData();
						
						
					Debug.Log(a["_id"].ToString());
					if(a.ContainsKey("_id")){
						p._id = a["_id"].ToString();
					}
					if(a.ContainsKey("user")){
						p.user = new User();
						if(a.ContainsKey("_id")){
							s.requestId = a["_id"].ToString();
						}
							
						foreach(KeyValuePair<string, object> t in a["user"] as Dictionary<string, object>){
							if(t.Key.Contains("_id")){
								p.user._id = t.Value.ToString();
							}
							if(t.Key.Contains("displayName")){
								p.user.displayName = t.Value.ToString();
							}
							if(t.Key.Contains("username")){
								p.user.username = t.Value.ToString();
							}
						}
					}
						
					if(a.ContainsKey("touser")){
						p.touser = new Touser();
						foreach(KeyValuePair<string, object> t in a["touser"] as Dictionary<string, object>){
							if(t.Key.Contains("_id")){
								p.touser._id = t.Value.ToString();
							}
							if(t.Key.Contains("displayName")){
								p.touser.displayName = t.Value.ToString();
							}
							if(t.Key.Contains("username")){
								p.touser.username = t.Value.ToString();
							}
								
							if(t.Key.Contains("profileImageURL")){
								p.touser.profileImageURL = t.Value.ToString();
							}
								
							if(t.Key.Contains("_id"))
								s._id = t.Value.ToString();
							if(t.Key.Contains("displayName"))
								s.displayName = t.Value.ToString();
							if(t.Key.Contains("username"))
								s.username = t.Value.ToString();
							if(t.Key.Contains("profileImageURL"))
								s.profileImageURL = t.Value.ToString();
						}
					}
						
						
					if(a.ContainsKey("status")){
						p.status = a["status"].ToString();
						if(a["status"].ToString()=="accept"){
							s.status = "accept";
							mItemDataList.Add(s);
						}
					}
					s.pd =p;
					pList.Add(p);
				}
			}
			bool showUI = true;
			foreach(PendingFriendData u in pList){
				if(u.status == "accept" && (u.touser.username == data.username || u.user.username == data.username)){
					buttonName.transform.parent.gameObject.SetActive(false);
					showUI = false;
				}
			
			}
			if(showUI){
				GetAllPendingFriends();
			}
			buttonName.transform.parent.gameObject.SetActive(showUI);
		});
		if(GameManager.Instance.header==null){
			return;
		}
		for(int i = 0; i < GameManager.Instance.header.Count; ++i)
		{
			foreach(KeyValuePair<string, List<string>> item in GameManager.Instance.header)
			{   	
				//Debug.Log(string.Format("{0}: {1}", item.Key, item.Value[0]));
				foreach(string val in item.Value){
					//Debug.Log(val);
				}
				www.AddField(item.Key, item.Value[0]);
			}
		}
		www.Send(); 			
	}
	public void Search(string search)
	{
		HTTPRequest www = new HTTPRequest(new Uri(  GameManager.Instance.webURLPrefix + "findFriend?search="+search), (request, response) => {
			HTTPResponse res = (HTTPResponse)response;
			if(res.IsSuccess){
				List<object> dic = Json.Decode(res.DataAsText) as List<object>;
				foreach(Dictionary<string, object> a in dic){
					UserData s = new UserData();
					if(a.ContainsKey("_id")){
						if(data.pd.touser._id == a["_id"].ToString())
						if(a.ContainsKey("profileImageURL")){
							s.profileImageURL = a["profileImageURL"].ToString();
							string url = "https://159.203.125.111/" + s.profileImageURL.TrimStart('/');
							Debug.Log("WWW download >" + url);
							new HTTPRequest(new Uri(url), (request1, response1) =>
							{
								var tex = new Texture2D(0, 0);
								HTTPResponse ret = (HTTPResponse)response1;
								tex.LoadImage(ret.Data);
								mIcon.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f,0.5f), 40);
								mIcon.gameObject.SetActive(true);
							}).Send();
						}
					}
				}
			}
				
		});
		if(GameManager.Instance.header==null){
			return;
		}
		for(int i = 0; i < GameManager.Instance.header.Count; ++i)
		{
			foreach(KeyValuePair<string, List<string>> item in GameManager.Instance.header)
			{   	
				//Debug.Log(string.Format("{0}: {1}", item.Key, item.Value[0]));
				foreach(string val in item.Value){
					//Debug.Log(val);
				}
				www.AddField(item.Key, item.Value[0]);
			}
		}
		www.Send(); 	
	}
	
	public void GetAllPendingFriends()
	{
		HTTPRequest www = new HTTPRequest(new Uri(  GameManager.Instance.webURLPrefix + "pendingFriend?myId=" + GameManager.Instance.player._id), (request, response) => {
			HTTPResponse res = (HTTPResponse)response;
			if(res.IsSuccess){
				Debug.Log(res.DataAsText);
				mItemDataList.Clear();
				pList.Clear();
				List<object> dic = Json.Decode(res.DataAsText) as List<object>;
				pList = new List<PendingFriendData>();
				foreach(Dictionary<string, object> a in dic){
					UserData s = new UserData();
					PendingFriendData p = new PendingFriendData();
					if(a.ContainsKey("_id")){
						p._id = a["_id"].ToString();
					}
					if(a.ContainsKey("user")){
						p.user = new User();
						if(a.ContainsKey("_id")){
							s.requestId = a["_id"].ToString();
						}
							
						foreach(KeyValuePair<string, object> t in a["user"] as Dictionary<string, object>){
							if(t.Key.Contains("_id")){
								p.user._id = t.Value.ToString();
							}
							if(t.Key.Contains("displayName")){
								p.user.displayName = t.Value.ToString();
							}
							if(t.Key.Contains("username")){
								p.user.username = t.Value.ToString();
							}
							if(t.Key.Contains("profileImageURL")){
								p.user.profileImageURL = t.Value.ToString();
							}
						}
					}
						
					if(a.ContainsKey("touser")){
						p.touser = new Touser();
						foreach(KeyValuePair<string, object> t in a["touser"] as Dictionary<string, object>){
							if(t.Key.Contains("_id")){
								p.touser._id = t.Value.ToString();
							}
							if(t.Key.Contains("displayName")){
								p.touser.displayName = t.Value.ToString();
							}
							if(t.Key.Contains("username")){
								p.touser.username = t.Value.ToString();
							}
								
							if(t.Key.Contains("profileImageURL")){
								p.touser.profileImageURL = t.Value.ToString();
							}
								
							if(t.Key.Contains("_id"))
								s._id = t.Value.ToString();
							if(t.Key.Contains("displayName"))
								s.displayName = t.Value.ToString();
							if(t.Key.Contains("username"))
								s.username = t.Value.ToString();
							if(t.Key.Contains("profileImageURL"))
								s.profileImageURL = t.Value.ToString();
						}
					}
						
						
					if(a.ContainsKey("status")){
						p.status = a["status"].ToString();
						if(a["status"].ToString()=="pending"){
							s.status = "pending";
							mItemDataList.Add(s);	
						}
					}
					s.pd =p;
					pList.Add(p);
				}
			}
			
			foreach(PendingFriendData u in pList){
				if(u.status == "pending" && (u.touser.username == data.username || u.user.username == data.username)){
					buttonName.text = "Pending";
					buttonName.transform.parent.GetComponent<EventTrigger>().enabled = false;
				}
			
			}
		});
		if(GameManager.Instance.header==null){
			return;
		}
		for(int i = 0; i < GameManager.Instance.header.Count; ++i)
		{
			foreach(KeyValuePair<string, List<string>> item in GameManager.Instance.header)
			{   	
				//Debug.Log(string.Format("{0}: {1}", item.Key, item.Value[0]));
				foreach(string val in item.Value){
					//Debug.Log(val);
				}
				www.AddField(item.Key, item.Value[0]);
			}
		}
		www.Send(); 			
	}

}
