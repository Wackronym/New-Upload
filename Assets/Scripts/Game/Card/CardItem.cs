using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;
using System;
using UnityEngine.EventSystems;

public class CardItem : MonoBehaviour {

    //ghilman
    public Color[] colorForItem; // solo, 1 to 1, group
    public Sprite[] iconForItem; // solo, 1 to 1, group
    public Text mainText;
    public GameObject itemObj;
    //ghilman
	public Text mNameText;
	public Image mIcon;
    public CardData data;
    bool isHistory;
    int mItemDataIndex = -1;

    //public Text mDescText;
    //public GameObject mContentRootObj;
    //public LoopListView2 mLoopListView;

	public void Init()
	{
   
	}
	public void SetItemData(CardData itemData,int itemIndex , bool _isHistory)
	{
		data = itemData;
		isHistory = _isHistory;

		data.mId = itemIndex;
		mItemDataIndex = itemIndex;
		System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-GB");
        DateTime d = DateTime.Parse(data.mDate);
        //Ghilman
        mainText.text = d.ToString("dddd, MMM dd yyyy, hh:mm tt");
        //Debug.Log(mainText.text);
        //Ghilman


        mNameText.text = "Mode: " + itemData.mName;

        if (itemData.mName == "Solo")
        {
            mIcon.sprite = iconForItem[0];
            itemObj.GetComponent<Image>().color = colorForItem[0];
        }
        else if (itemData.mName == "1to1")
        {
            mIcon.sprite = iconForItem[1];
            itemObj.GetComponent<Image>().color = colorForItem[1];
        }
        else if (itemData.mName == "Group")
        {
            mIcon.sprite = iconForItem[2];
            itemObj.GetComponent<Image>().color = colorForItem[2];
        }
        itemObj.transform.GetComponent<Button>().onClick.RemoveAllListeners();
        itemObj.transform.GetComponent<Button>().onClick.AddListener(OnBtnClicked);

        //mNameText.text 
        //mIcon.sprite = Card.Get.GetSpriteByName(data.mPic);
        //mIcon.gameObject.SetActive(false);
        //mNameText.transform.parent.GetComponent<Button>().onClick.AddListener(OnBtnClicked);
        //Destroy(mNameText.transform.parent.GetComponent<EventTrigger>());
        //mDescText.text = data.mDate + " | " + data.mDuration;
        //mNameText.transform.parent.GetComponent<EventTrigger>().triggers[0].callback.AddListener( (eventData) => {OnBtnClicked();});
    }
    void OnBtnClicked()
	{
		if(GameManager.Instance.menuManager.NavigationStackPeek()!=UIManager.State.Win){
			GameManager.Instance.cardIndex = data.mId;
            GameManager.Instance.menuManager.PushMenu(UIManager.State.Win);
			GameManager.Instance.menuManager.GetComponentInChildren<Card>().HideSaveButton();
			GameManager.Instance.menuManager.GetComponentInChildren<Card>().isHistory = isHistory;
		}
	}

}
