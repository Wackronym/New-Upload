using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using SuperScrollView;

public class FavoriteItem : MonoBehaviour {
	
	
	public Text mTitle;
	public Image mStatus;
	public Text mBody;
	public GameObject mContentRootObj;
	public int mItemDataIndex = -1;
	public LoopListView2 mLoopListView;
	public RoundData data;
	public Text underline;
	public int FavListIndex;
	public int RoundIndex;
	public void Init()
	{
   
	}
	
	public void SetItemData(RoundData itemData,int itemIndex, bool isHistory, int _FavListIndex, int _RoundIndex)
	{
		RoundIndex = _RoundIndex;
		FavListIndex = _FavListIndex;
		underline = mBody.transform.GetChild(0).GetComponent<Text>();
		data = itemData;
		data.id = itemIndex;
		mItemDataIndex = itemIndex;
		if(data.mTotalRoundTime.Equals("90")){
			mTitle.text = "Wack " + data.time.Substring(0, 8)  + " ("+data.mTotalRoundTime+"s)";
		}
		else if( data.mTotalRoundTime.Equals("60")){
			mTitle.text = "Wack " + data.time.Substring(0, 8)  + " ("+data.mTotalRoundTime+"s)";
		}
		else if( data.mTotalRoundTime.Equals("30")){
			mTitle.text = "Wack " + data.time.Substring(0, 8)  + " ("+data.mTotalRoundTime+"s)";
		}
		else{
			mTitle.text = "Wack " + data.time.Substring(0, 8)  + " (Unlimited)";
		}
		string dataStr = data.mSenence.Replace("__","");
		mBody.text =  dataStr +data.mAnswer;
		
		
		underline.text = "<color=white>"; 
		
		
		int totalLength = dataStr.Length-1;
		
		if(data.mAnswer!=null){
			totalLength = totalLength - 5;
		}
		for(int i=0; i<totalLength;i++){
			underline.text += "_";
		}
		
		underline.text += "</color>";
		
		if(data.mAnswer!=null){
			underline.text += "<color=black>"; 
			for(int i=0; i<data.mAnswer.Length-1;i++){
				underline.text += "_";
			}
			underline.text += "</color>";
		}
		
		
		if(data.mCompleted){
			mStatus.sprite = Card.Get.GetSpriteByName("checked");	
		}
		else{
			mStatus.sprite = Card.Get.GetSpriteByName("unchecked");	
		}
		
		if(GameManager.Instance.menuManager.previousState == UIManager.State.MainMenu){
			Toggle t = GetComponentInChildren<Toggle>();
			if(t!=null){
				t.isOn = true;
				t.enabled = false;	
				if(isHistory){
					t.gameObject.SetActive(false);
				}
			}
			
		}
	}

}
