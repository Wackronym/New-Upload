using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SuperScrollView;
using BayatGames.SaveGamePro;
using System;
using UnityEngine.UI;
	[Serializable]
	public class CardData
    {
	    public int mId; // Ghilman,, Numan is saving card id in this var.
	    public string mName; // Ghilman Numan may be saving mode of the game in var. 
	    public string mDate; // Ghilman Numan is saving date and time in this var.
	    public string mDuration;// Ghilman does not know what is this.
	    public string mPic; // 
	    public bool mChecked;
	    public int mainIndex;
	    public List<RoundData> rData = new List<RoundData>();
	    
    }
[Serializable]
public class RoundData{
    public string mAnswer;
    public string mSenence;
    public bool mCompleted;
    public string mTotalRoundTime;
    public string mUsedRoundTime;
    public string time;
    public float id;
    public bool reCheck = false; //Ghilman,,, Numan is using this variable as isNotFavorite.
    public int mainIndex;
    public int innerIndex;
}
    

	public class Card : BaseUI
	{
    //Ghilman
    CardData currentCardData;
    //Ghilman
		public int mId;
		public Text mName;
		public Text mRound;
		public Text mStats;
		public Image mPic;
		public GameObject saveButton;
		public LoopListView2 mLoopListView;
		public int mainCardIndex;
		public Sprite[] spriteObjArray;
		System.Action mOnRefreshFinished = null;
		System.Action mOnLoadMoreFinished = null;
		int mLoadMoreCount = 20;
		float mDataLoadLeftTime = 0;
		bool mIsWaittingRefreshData = false;
		bool mIsWaitLoadingMoreData = false;
		public int mTotalDataCount = 10;
		
		private bool saveOnExit = false;
		Dictionary<string, Sprite> spriteObjDict = new Dictionary<string, Sprite>();
		static Card instance = null;
		public List<int> indexRemove;
		
		public bool isHistory;
		public static Card Get
		{
			get
			{
				if (instance == null)
				{
					instance = UnityEngine.Object.FindObjectOfType<Card>();
				}
				return instance;
			}

		}

    void Awake()
    {
        Init();
        InitData();
 
       
    }
    void OnEnable()
    {
        GameManager.Instance.Scroll.transform.parent.GetChild(0).gameObject.SetActive(false);
        //Ghilman
        if (GameManager.Instance.menuManager.previousState == UIManager.State.MainMenu)
        {
            Debug.Log("this is the main menu");
            HideSaveButton();
        }
        //Ghilman
    }
    void Start()
    {
        mLoopListView.InitListView(Card.Get.TotalItemCount, InitScrollView);
        mLoopListView.mOnEndDragAction = OnEndDrag;

        //Ghilman
        // just follow the numana code but in my why. which easy and right way to do.
        currentCardData = GameManager.Instance.mItemDataList[GameManager.Instance.cardIndex];
        if (isHistory)
        {
            if (currentCardData.mDuration.Equals("90"))
            {
                mStats.text = "Posted:" + currentCardData.mDate.Substring(0, 10);// + " | Duration:" + GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].mDuration+"s"; 
            }
            else if (currentCardData.mDuration.Equals("60"))
            {
                mStats.text = "Posted:" + currentCardData.mDate.Substring(0, 10);// + " | Duration:" + GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].mDuration+"s"; 
            }
            else if (currentCardData.mDuration.Equals("30"))
            {
                mStats.text = "Posted:" + currentCardData.mDate.Substring(0, 10);// + " | Duration:" + GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].mDuration+"s"; 
            }
            else
            {
                mStats.text = "Posted:" + currentCardData.mDate.Substring(0, 10);// + " | Duration: Unlimited"; 
            }
            //mName.text = "Mode: " + currentCardData.mName;
            mName.text = "Mode: Solo";
            mRound.text = "Total Rounds: " + currentCardData.rData.Count.ToString();
        }
        else
        {
            // Numan's code
            if (GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count - 1].mDuration.Equals("90"))
            {
                mStats.text = "Posted:" + GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count - 1].mDate.Substring(0, 10);// + " | Duration:" + GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].mDuration+"s"; 
            }
            else if (GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count - 1].mDuration.Equals("60"))
            {
                mStats.text = "Posted:" + GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count - 1].mDate.Substring(0, 10);// + " | Duration:" + GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].mDuration+"s"; 
            }
            else if (GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count - 1].mDuration.Equals("30"))
            {
                mStats.text = "Posted:" + GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count - 1].mDate.Substring(0, 10);// + " | Duration:" + GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].mDuration+"s"; 
            }
            else
            {
                mStats.text = "Posted:" + GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count - 1].mDate.Substring(0, 10);// + " | Duration: Unlimited"; 
            }

            mName.text = "Mode: " + GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count - 1].mName;
            mRound.text = "Total Rounds: " + GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count - 1].rData.Count.ToString();
            // Numan's code
        }
        //Ghilman
        base.AddMouseDownEvent();
		}
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
			//SaveGame.Save ( "mItemDataList", mItemDataList );
			//GameManager.Instance.mItemDataList = SaveGame.Load<List<CardData>> ( "mItemDataList" );
			
		}
		
		public void SaveData(){
			saveOnExit = true;
			MainMenu();
			
		}
    public void MainMenu()
    {
        indexRemove = new List<int>();
        CardData c = GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1];
        foreach (Toggle t in GetComponentsInChildren<Toggle>(true))
        {
            if (t.isOn)
            {
                RoundItem item = t.transform.parent.GetComponent<RoundItem>();
                foreach (RoundData d in c.rData)
                {
                    if (d.id ==  item.roundData.id)
                    {
                        Debug.Log((int)d.id);
                        if (t.transform.parent.gameObject.name.Contains("Clone"))
                        {
                            indexRemove.Add((int)d.id);
                        }
                    }
                }
            }
        }
        if (saveOnExit){
            if (SaveGame.Load<List<CardData>> ( "History" )!=null){
                GameManager.Instance.history = SaveGame.Load<List<CardData>> ( "History" );
            }
            GameManager.Instance.history.Add(GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1]);
            Debug.Log(GameManager.Instance.history[GameManager.Instance.history.Count - 1].mDate);
            SaveGame.Save ( "History", GameManager.Instance.history);
        }


        //GameManager.Instance.mItemDataList = SaveGame.Load<List<CardData>> ( "mItemDataList" );
        if (GameManager.Instance.menuManager.previousState == UIManager.State.MainMenu)
        {
            //Ghilman
            Debug.Log("this is the card data = "+currentCardData.rData.Count);
            if (isHistory)
            {
                
            }
            //Ghilman
            GameManager.Instance.menuManager.PopMenu();
        }
        else
        {
            //Ghilman
            Screen.sleepTimeout = SleepTimeout.SystemSetting;
            //Ghilman
            foreach (Toggle t in GetComponentsInChildren<Toggle>(true))
            {
                if (t.isOn)
                {
                    RoundItem item = t.transform.parent.GetComponent<RoundItem>();
                    //if(item.mItemDataIndex == c.rD){
                    foreach (RoundData d in c.rData){
                        if (d.id ==  item.roundData.id){
                            Debug.Log((int)d.id);
                            d.reCheck = false;
                        }
                    }
                }
            }
            foreach (int cDD in indexRemove)
            {
                Debug.Log(cDD);
                for (int a = 0; a<  c.rData.Count ; a++)
                {
                    if (a == cDD)
                    {
                        c.rData[a].reCheck = false ;
                    }
                    else
                    {
                        //c.rData[a].reCheck = true ;
                    }
                }
            }
            c.rData.RemoveAll(it => it.reCheck);
            if (indexRemove.Count>0)
            {
                if (SaveGame.Load<List<CardData>> ( "Favorites" )!=null)
                {
                    GameManager.Instance.favorite = SaveGame.Load<List<CardData>> ( "Favorites" );
                }
                GameManager.Instance.favorite.Add(GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1]);
                SaveGame.Save ( "Favorites", GameManager.Instance.favorite);
            }
            GameManager.Instance.mItemDataList.Clear();
            GameManager.Instance.menuManager.PopMenuToState (UIManager.State.MainMenu);
        }
        GameManager.Instance.GetComponent<AudioSource>().PlayOneShot(GameManager.Instance.click);
        GameManager.Instance.Scroll.transform.parent.GetChild(0).gameObject.SetActive (true);
    }
		LoopListViewItem2 InitScrollView(LoopListView2 listView, int index)
		{
			if (index < 0 || index >= Card.Get.TotalItemCount)
			{
				return null;
			}

			RoundData itemData = Card.Get.GetItemDataByIndex(index);
			if(itemData == null)
			{
				return null;
			}
			//get a new item. Every item can use a different prefab, the parameter of the NewListViewItem is the prefab’name. 
			//And all the prefabs should be listed in ItemPrefabList in LoopListView2 Inspector Setting
			LoopListViewItem2 item = listView.NewListViewItem("RoundItem");
			RoundItem itemScript = item.GetComponent<RoundItem>();
			if (item.IsInitHandlerCalled == false)
			{
				item.IsInitHandlerCalled = true;
				itemScript.Init();
			}
			itemScript.SetItemData(itemData, isHistory);
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
			if (index < 0 || index >= GameManager.Instance.mItemDataList[GameManager.Instance.cardIndex].rData.Count)
			{
				return null;
			}
        //Ghilman
        Debug.Log("count = " + GameManager.Instance.mItemDataList[GameManager.Instance.cardIndex].rData.Count);
        //Ghilman
        return GameManager.Instance.mItemDataList[GameManager.Instance.cardIndex].rData[index];
		}

		public RoundData GetItemDataById(int itemId)
		{
			int count = GameManager.Instance.mItemDataList[GameManager.Instance.cardIndex].rData.Count;
			for (int i = 0; i < count; ++i)
			{
				if(GameManager.Instance.mItemDataList[i].mId == itemId)
				{
					return GameManager.Instance.mItemDataList[GameManager.Instance.cardIndex].rData[i];
				}
			}
			return null;
		}
		public void HideSaveButton(){
			saveButton.SetActive(false);
		}
		public int TotalItemCount
		{
			get
			{	if(GameManager.Instance.mItemDataList== null)
				{
					return 0;
				}
				return GameManager.Instance.mItemDataList[GameManager.Instance.mItemDataList.Count-1].rData.Count;
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
			int count = GameManager.Instance.mItemDataList.Count;
			for (int i = 0; i < count; ++i)
			{
				GameManager.Instance.mItemDataList[i].mChecked = true;
			}
		}

		public void UnCheckAllItem()
		{
			int count = GameManager.Instance.mItemDataList.Count;
			for (int i = 0; i < count; ++i)
			{
				GameManager.Instance.mItemDataList[i].mChecked = false;
			}
		}

		public bool DeleteAllCheckedItem()
		{
			int oldCount = GameManager.Instance.mItemDataList.Count;
			GameManager.Instance.mItemDataList.RemoveAll(it => it.mChecked);
			return (oldCount != GameManager.Instance.mItemDataList.Count);
		}

	}