using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperScrollView;
using BayatGames.SaveGamePro;
using System;
using UnityEngine.UI;

	public class History : MonoBehaviour
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
			//DoRefreshDataSource();
		}
		
		public void SetDataTotalCount(int count)
		{
			mTotalDataCount = count;
			DoRefreshDataSource();
		}

		public void DoRefreshDataSource()
		{
			//SaveGame.Save ( "history", history );
			//Debug.Log("Called");
			
			if(GameManager.Instance.history==null){
				return;
				if(GameManager.Instance.history.Count == 0){
					return;
				}
			}
			mTotalDataCount = GameManager.Instance.history.Count;
			mIsWaittingRefreshData = true;
			mDataLoadLeftTime = 1;
			//mLoopListView.SetListItemCount(mTotalDataCount);
			
		}
		public void Update()
		{
			if (mIsWaittingRefreshData)
			{
				mDataLoadLeftTime -= Time.deltaTime;
				if (mDataLoadLeftTime <= 0)
				{
					mIsWaittingRefreshData = false;
					//DoRefreshDataSource();
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
		/*public void SaveData(){
			if(gameObject.name == "Favorites"){
				SaveGame.Save ( "Favorites", GameManager.Instance.history );
			}else{
				SaveGame.Save ( "history", GameManager.Instance.history );
			}
				
			
		}*/
		public void MainMenu(){
			
			GameManager.Instance.history.RemoveAt(GameManager.Instance.history.Count-1);
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

			CardData itemData = GetItemDataByIndex(index);
			if(itemData == null)
			{
				return null;
			}
			
			//get a new item. Every item can use a different prefab, the parameter of the NewListViewItem is the prefab’name. 
			//And all the prefabs should be listed in ItemPrefabList in LoopListView2 Inspector Setting
			LoopListViewItem2 item = listView.NewListViewItem("CardItem");
			CardItem itemScript = item.GetComponent<CardItem>();
			if (item.IsInitHandlerCalled == false)
			{
				item.IsInitHandlerCalled = true;
				itemScript.Init();
			}
			itemScript.SetItemData(itemData,index, true);
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

		public CardData GetItemDataByIndex(int index)
		{
			if (index < 0 || index >= GameManager.Instance.history.Count)
			{
				return null;
			}
			return GameManager.Instance.history[index];
		}

		public CardData GetItemDataById(int itemId)
		{
			int count = GameManager.Instance.history.Count;
			for (int i = 0; i < count; ++i)
			{
				if(GameManager.Instance.history[i].mId == itemId)
				{
					return GameManager.Instance.history[i];
				}
			}
			return null;
		}

		public int TotalItemCount
		{
			get
			{	if(GameManager.Instance.history== null)
			{
				return 0;
			}
				return GameManager.Instance.history.Count;
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
			int count = GameManager.Instance.history.Count;
			for (int i = 0; i < count; ++i)
			{
				GameManager.Instance.history[i].mChecked = true;
			}
		}

		public void UnCheckAllItem()
		{
			int count = GameManager.Instance.history.Count;
			for (int i = 0; i < count; ++i)
			{
				GameManager.Instance.history[i].mChecked = false;
			}
		}

		public bool DeleteAllCheckedItem()
		{
			int oldCount = GameManager.Instance.history.Count;
			GameManager.Instance.history.RemoveAll(it => it.mChecked);
			return (oldCount != GameManager.Instance.history.Count);
		}

	}