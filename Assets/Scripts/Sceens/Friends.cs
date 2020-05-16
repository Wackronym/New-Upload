
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperScrollView;
using BayatGames.SaveGamePro;
using System;
using UnityEngine.UI;
using BestHTTP;
using BestHTTP.JSON;
using UnityEngine.EventSystems;

	public class Friends : MonoBehaviour
	{
		
		
		public LoopListView2 mLoopListView;
		public Sprite[] spriteObjArray;
		System.Action mOnRefreshFinished = null;
		System.Action mOnLoadMoreFinished = null;
		int mLoadMoreCount = 20;
		float mDataLoadLeftTime = 0;
		bool mIsWaittingRefreshData = false;
		bool mIsWaitLoadingMoreData = false;
		public int mTotalDataCount = 10;
		Dictionary<string, Sprite> spriteObjDict = new Dictionary<string, Sprite>();
		public List<UserData> mItemDataList = new List<UserData>();
		public List<PendingFriendData> pList = new List<PendingFriendData>();
		public GameObject dialog;
		void Awake()
		{
			Init();
			InitData();
			
		}
		void Start()
		{
			mLoopListView.InitListView(TotalItemCount, InitScrollView);
		}
		
		void InitData()
		{
			spriteObjDict.Clear();
			foreach (Sprite sp in spriteObjArray)
			{
				spriteObjDict[sp.name] = sp;
			}
		}
		
		public void Init()
		{
			DoRefreshDataSource();
		}
		
		public void SetDataTotalCount(int count)
		{
			mTotalDataCount = count;
			DoRefreshDataSource();
		}
		void DoRefreshDataSource()
		{
			
		}
		
		public void GetAllPendingFriends()
		{
			dialog.SetActive(false);
			if(gameObject.name != "Friend Requests"){
				return;
			}
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
								if(t.Key.Contains("profileImageURL")){
									p.user.profileImageURL = t.Value.ToString();
								}
							}
							
							Debug.Log("Simple User");
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
							
							Debug.Log("To User");
						}
						
						
						if(a.ContainsKey("status")){
							p.status = a["status"].ToString();
							if(a["status"].ToString()=="pending"){
								s.status = "pending";
								mItemDataList.Add(s);
								
							}
						}
						
						
						/*UserData s = new UserData();
						if(a.ContainsKey("_id"))
							s._id = a["_id"].ToString();
						if(a.ContainsKey("displayName"))
							s.displayName = a["displayName"].ToString();
						if(a.ContainsKey("username"))
							s.username = a["username"].ToString();
						if(a.ContainsKey("profileImageURL"))
							s.profileImageURL = a["profileImageURL"].ToString();
						mItemDataList.Add(s);*/
						s.pd =p;
						pList.Add(p);
					}
					
					mTotalDataCount = mItemDataList.Count;
					mIsWaittingRefreshData = true;
					mDataLoadLeftTime = 1;
					mLoopListView.SetListItemCount(mTotalDataCount);
					//Debug.Log(mItemDataList.Count);
					//mLoopListView.RefreshAllShownItem();
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
		
		public void Refresh(){
			
			
			//mLoopListView.mTmpPooledItemList.Clear();
			//mLoopListView.mPooledItemList.Clear();
			//mLoopListView.mItemPoolList.Clear();
			//mLoopListView.mItemPoolDict.Clear();
			
			mLoopListView.RecycleAllItem();
			mLoopListView.ClearAllTmpRecycledItem();
			mItemDataList.Clear();
			pList.Clear();
			for(int a = 0 ;  a < mLoopListView.transform.GetChild(1).GetChild(0).childCount-1; a++){
				if(a>0 && mLoopListView.transform.GetChild(1).GetChild(0).GetChild(a).gameObject.name.Contains("Clone")){
					
					//Destroy(mLoopListView.transform.GetChild(1).GetChild(0).GetChild(a).gameObject);
					//if(mLoopListView.transform.GetChild(1).GetChild(0).GetChild(a).gameObject!=null)
						//Debug.Log(mLoopListView.transform.GetChild(1).GetChild(0).GetChild(a).gameObject);
					mLoopListView.transform.GetChild(1).GetChild(0).GetChild(a).GetChild(3).gameObject.GetComponent<EventTrigger>().enabled = true;
				}
			}
			//mLoopListView.RefreshAllShownItem();
			
		}
		
		
		public void GetAllFriendsList()
		{
			dialog.SetActive(false);
			if(gameObject.name != "My Friends"){
				return;
			}
			HTTPRequest www = new HTTPRequest(new Uri(  GameManager.Instance.webURLPrefix + "activeFriend?myId=" + GameManager.Instance.player._id), (request, response) => {
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
						
						
						/*UserData s = new UserData();
						if(a.ContainsKey("_id"))
						s._id = a["_id"].ToString();
						if(a.ContainsKey("displayName"))
						s.displayName = a["displayName"].ToString();
						if(a.ContainsKey("username"))
						s.username = a["username"].ToString();
						if(a.ContainsKey("profileImageURL"))
						s.profileImageURL = a["profileImageURL"].ToString();
						mItemDataList.Add(s);*/
						s.pd =p;
						pList.Add(p);
					}
					
					mTotalDataCount = mItemDataList.Count;
					mIsWaittingRefreshData = true;
					mDataLoadLeftTime = 1;
					mLoopListView.SetListItemCount(mTotalDataCount);
					//Debug.Log(mItemDataList.Count);
					//mLoopListView.RefreshAllShownItem();
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
		
		public void DoRefreshDataSource(InputField data)
		{
			GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
			//	if(TouchScreenKeyboard.visible)
			HTTPRequest www = new HTTPRequest(new Uri(  GameManager.Instance.webURLPrefix + "findFriend?search="+""), (request, response) => {
				HTTPResponse res = (HTTPResponse)response;
				if(res.IsSuccess){
					mItemDataList.Clear();
					List<object> dic = Json.Decode(res.DataAsText) as List<object>;
					//mItemDataList = new List<UserData>();
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
						mItemDataList.Add(s);
					}
					
					mTotalDataCount = mItemDataList.Count;
					mIsWaittingRefreshData = true;
					mDataLoadLeftTime = 1;
					mLoopListView.SetListItemCount(mTotalDataCount);
					//Debug.Log(mItemDataList.Count);
					//mLoopListView.RefreshAllShownItem();
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
		
		public void Search(InputField data)
		{
			GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
			Debug.Log(data.text);
			HTTPRequest www = new HTTPRequest(new Uri(  GameManager.Instance.webURLPrefix + "findFriend?search="+data.text), (request, response) => {
				HTTPResponse res = (HTTPResponse)response;
				if(res.IsSuccess){
					mItemDataList.Clear();
					Debug.Log(res.DataAsText);
					List<object> dic = Json.Decode(res.DataAsText) as List<object>;
					//mItemDataList = new List<UserData>();
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
						mItemDataList.Add(s);
					}
					
					mTotalDataCount = mItemDataList.Count;
					mIsWaittingRefreshData = true;
					mDataLoadLeftTime = 1;
					mLoopListView.SetListItemCount(mTotalDataCount);
					//Debug.Log(mItemDataList.Count);
					//mLoopListView.RefreshAllShownItem();
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
		public void Update()
		{
			if (mIsWaittingRefreshData)
			{
				mDataLoadLeftTime -= Time.deltaTime;
				if (mDataLoadLeftTime <= 0)
				{
					mIsWaittingRefreshData = false;
					DoRefreshDataSource();
					if (mOnRefreshFinished != null)
					{
						mOnRefreshFinished();
					}
				}
			}
			else if (mIsWaitLoadingMoreData)
			{
				mDataLoadLeftTime -= Time.deltaTime;
				if (mDataLoadLeftTime <= 0)
				{
					mIsWaitLoadingMoreData = false;
					DoRefreshDataSource();
					if (mOnLoadMoreFinished != null)
					{
						mOnLoadMoreFinished();
					}
				}
			}

		}
		public void SaveData(){
			SaveGame.Save ( "UserData", GameManager.Instance.mItemDataList );
		}
		public void MainMenu(){
			
			mItemDataList.RemoveAt(mItemDataList.Count-1);
			GameManager.Instance.menuManager.PopMenuToState (UIManager.State.MainMenu);
			GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
			GameManager.Instance.Scroll.transform.parent.GetChild(0).gameObject.SetActive (true);
		}
		LoopListViewItem2 InitScrollView(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= TotalItemCount)
			{
				return null;
			}

			UserData itemData = GetItemDataByIndex(index);
			if(itemData == null)
			{
				return null;
			}
			//get a new item. Every item can use a different prefab, the parameter of the NewListViewItem is the prefab’name. 
			//And all the prefabs should be listed in ItemPrefabList in LoopListView2 Inspector Setting
			LoopListViewItem2 item = listView.NewListViewItem("UserItem");
			UserItem itemScript = item.GetComponent<UserItem>();
			if (item.IsInitHandlerCalled == false)
			{
				item.IsInitHandlerCalled = true;
				itemScript.Init();
			}
			if(itemScript!=null)
			itemScript.SetItemData(itemData,index);
			return item;
		}

		void OnJumpBtnClicked(int count)
		{
			mLoopListView.MovePanelToItemIndex(count, 0);
		}

		void OnAddItemBtnClicked(int count)
		{
			
			mLoopListView.SetListItemCount(count, false);
		}

		void OnSetItemCountBtnClicked(int count)
		{
			mLoopListView.SetListItemCount(count, false);
		}
		
		public Sprite GetSpriteByName(string spriteName)
		{
			Sprite ret = null;
			if (spriteObjDict.TryGetValue(spriteName, out ret))
			{
				return ret;
			}
			return null;
		}

		public UserData GetItemDataByIndex(int index)
		{
			if (index < 0 || index >= mItemDataList.Count)
			{
				return null;
			}
			return mItemDataList[index];
		}

		public UserData GetItemDataById(string itemId)
		{
			int count = mItemDataList.Count;
			for (int i = 0; i < count; ++i)
			{
				if(mItemDataList[i]._id == itemId)
				{
					return mItemDataList[i];
				}
			}
			return null;
		}

		public int TotalItemCount
		{
			get
			{	if(mItemDataList== null)
			{
				return 0;
			}
				return mItemDataList.Count;
			}
		}

		public void RequestRefreshDataList(System.Action onReflushFinished)
		{
			mDataLoadLeftTime = 1;
			mOnRefreshFinished = onReflushFinished;
			mIsWaittingRefreshData = true;
		}

		public void RequestLoadMoreDataList(int loadCount,System.Action onLoadMoreFinished)
		{
			mLoadMoreCount = loadCount;
			mDataLoadLeftTime = 1;
			mOnLoadMoreFinished = onLoadMoreFinished;
			mIsWaitLoadingMoreData = true;
		}


		
		
		public string GetSpriteNameByIndex(int index)
		{
			if (index < 0 || index >= spriteObjArray.Length)
			{
				return "";
			}
			return spriteObjArray[index].name;
		}
		

		public void CheckAllItem()
		{
			int count = mItemDataList.Count;
			for (int i = 0; i < count; ++i)
			{
				mItemDataList[i].mChecked = true;
			}
		}

		public void UnCheckAllItem()
		{
			int count = mItemDataList.Count;
			for (int i = 0; i < count; ++i)
			{
				mItemDataList[i].mChecked = false;
			}
		}

		public bool DeleteAllCheckedItem()
		{
			int oldCount = mItemDataList.Count;
			mItemDataList.RemoveAll(it => it.mChecked);
			return (oldCount != mItemDataList.Count);
		}
		
	}
	
	[Serializable]
    public class UserData{
	    public string _id;
	    public string displayName;
	    public string username;
	    public string profileImageURL;
	    public bool mChecked;
	    public string status;
	    public string requestId;
	    public PendingFriendData pd;
    }
    [Serializable]
	  public class FriendData{
	      public string id;
	      public string displayName;
	      public string usernme;
	      public string profileImageURL;
	      //public bool status;
	  }
      [Serializable]
      public class User
		{
			public string _id ;
			public string displayName ;
			public string username ;
			public string profileImageURL;
			public string requestId;
		}
	[Serializable]
	public class Touser
	{
		public string _id ;
		public string displayName ;
		public string username ;
		public string profileImageURL;
	}
	[Serializable]
	public class PendingFriendData
	{
		public string _id ;
		public User user ;
		public Touser touser ;
		public string status ;
	}