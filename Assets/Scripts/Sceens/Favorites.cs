using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperScrollView;
using BayatGames.SaveGamePro;
using System;
using UnityEngine.UI;

public class Favorites : MonoBehaviour
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
			transform.parent.GetChild(3).gameObject.SetActive(false);
			Init();
			InitData();
		}
		
		void OnEnable(){
			//GameManager.Instance.Scroll.transform.parent.GetChild(0).gameObject.SetActive(false);
			transform.parent.GetChild(3).gameObject.SetActive(false);
		}
		void Start()
		{
			mLoopListView.InitListView(TotalItemCount, InitScrollView);
			mLoopListView.mOnEndDragAction = OnEndDrag;
			transform.parent.GetChild(3).gameObject.SetActive(false);
		
		}
		
		//public void RemoveFavorite(){
		//	GameManager.Instance.favObj.SetActive(false);
		//	GameManager.Instance.favorite[GameManager.Instance.favMIndex].rData.RemoveAt(GameManager.Instance.favRIndex);
		//	if(GameManager.Instance.favorite[GameManager.Instance.favMIndex].rData.Count==0){
		//		GameManager.Instance.favorite.RemoveAt(GameManager.Instance.favMIndex);
		//	}
		//	GameManager.Instance.dummyFavorite.RemoveAt(GameManager.Instance.favTempIndex);
		//	SaveGame.Save ( "Favorites", GameManager.Instance.favorite);
		//}
		
		void OnEndDrag()
		{
			if (mLoopListView.ShownItemCount == 0)
			{
				return;
			}
			
			LoopListViewItem2 item = mLoopListView.GetShownItemByItemIndex(0);
			if (item == null)
			{
				return;
			}
			mLoopListView.OnItemSizeChanged(item.ItemIndex);
			transform.parent.GetChild(3).gameObject.SetActive(false);
		}
		void InitData()
		{
			spriteObjDict.Clear();
			foreach (Sprite sp in spriteObjArray)
			{
				spriteObjDict[sp.name] = sp;
			}
			transform.parent.GetChild(3).gameObject.SetActive(false);
		}
		
		public void Init()
		{
			DoRefreshDataSource();
			transform.parent.GetChild(3).gameObject.SetActive(false);
		}
		
		public void SetDataTotalCount(int count)
		{
			mTotalDataCount = count;
			DoRefreshDataSource();
		}

		public void DoRefreshDataSource()
		{
			//SaveGame.Save ( "favorite", favorite );
			//Debug.Log("Called");
			
			if(GameManager.Instance.dummyFavorite==null){
				return;
				if(GameManager.Instance.favorite.Count == 0){
					return;
				}
			}
			mTotalDataCount = GameManager.Instance.favorite.Count;
			mIsWaittingRefreshData = true;
			mDataLoadLeftTime = 1;
			//mLoopListView.SetListItemCount(mTotalDataCount);

		}
		
		public void SaveData(){
			MainMenu();
			transform.parent.GetChild(3).gameObject.SetActive(false);
		}
    public void MainMenu(){
        GameManager.Instance.favorite.RemoveAt(GameManager.Instance.favorite.Count-1);
        GameManager.Instance.menuManager.PopMenuToState (UIManager.State.MainMenu);
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
        GameManager.Instance.Scroll.transform.parent.GetChild(0).gameObject.SetActive (true);
    }
    public void ResetMyUI()
    {
        mLoopListView.InitListView(TotalItemCount, InitScrollView);
        mLoopListView.mOnEndDragAction = OnEndDrag;
        transform.parent.GetChild(3).gameObject.SetActive(false);
    }
    LoopListViewItem2 InitScrollView(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= TotalItemCount)
			{
				return null;
			}

			RoundData itemData = GetItemDataByIndex(index);
			if(itemData == null)
			{
				return null;
			}
			//get a new item. Every item can use a different prefab, the parameter of the NewListViewItem is the prefab’name. 
			//And all the prefabs should be listed in ItemPrefabList in LoopListView2 Inspector Setting
			LoopListViewItem2 item = listView.NewListViewItem("RoundItem");
            //Debug.Log("item name = "+item.name);
			RoundItem itemScript = item.GetComponent<RoundItem>();
			if (item.IsInitHandlerCalled == false)
			{
				item.IsInitHandlerCalled = true;
				itemScript.Init();
			}
			itemScript.SetItemData(itemData, false);
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

		public RoundData GetItemDataByIndex(int index)
		{
			if (index < 0 || index >= GameManager.Instance.dummyFavorite.Count)
			{
				return null;
			}
			return GameManager.Instance.dummyFavorite[index];
		}

		public RoundData GetItemDataById(int itemId)
		{
			int count = GameManager.Instance.dummyFavorite.Count;
			for (int i = 0; i < count; ++i)
			{
				if(GameManager.Instance.dummyFavorite[i].id == itemId)
				{
					return GameManager.Instance.dummyFavorite[i];
				}
			}
			return null;
		}

		public int TotalItemCount
		{
			get
			{	if(GameManager.Instance.dummyFavorite== null)
			{
				return 0;
			}
				return GameManager.Instance.dummyFavorite.Count;
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
		
	}