//  Copyright 2016 MaterialUI for Unity http://materialunity.com
//  Please see license file for terms and conditions of use, and more information.

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MaterialUI
{
	using BayatGames.SaveGamePro;
	using System.Collections.Generic;
    [AddComponentMenu("MaterialUI/Tab Item", 100)]
    public class TabItem : Selectable, IPointerClickHandler, ISubmitHandler
    {
        [SerializeField]
        private Text m_ItemText;
        public Text itemText
        {
            get { return m_ItemText; }
            set { m_ItemText = value; }
        }

        [SerializeField]
        private Graphic m_ItemIcon;
        public Graphic itemIcon
        {
            get { return m_ItemIcon; }
            set { m_ItemIcon = value; }
        }

        [SerializeField]
        private TabView m_TabView;
        public TabView tabView
        {
            get { return m_TabView; }
            set { m_TabView = value; }
        }

        private int m_Id;
        public int id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        private RectTransform m_RectTransform;
        public RectTransform rectTransform
        {
            get
            {
                if (m_RectTransform == null)
                {
                    m_RectTransform = (RectTransform)transform;
                }
                return m_RectTransform;
            }
        }

        private CanvasGroup m_CanvasGroup;
        public CanvasGroup canvasGroup
        {
            get
            {
                if (m_CanvasGroup == null)
                {
                    m_CanvasGroup = gameObject.GetComponent<CanvasGroup>();
                }
                return m_CanvasGroup;
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (interactable)
            {
                m_TabView.SetPage(id);
            }
        }
        
        
	    public void HomeTab(int _id)
	    {
			 m_TabView.SetPage(_id);
	    }

        public override void OnPointerDown(PointerEventData eventData)
        {
            base.OnPointerDown(eventData);
            if (interactable)
            {
	            // m_TabView.TabItemPointerDown(id);
	            GameManager.Instance.activeTabIndex = 0;
	            //Debug.Log(id);
	            m_TabView.SetPage(id);

                //Ghilman
                if (GameManager.Instance.shouldSaveFavorites)
                {
                    GameManager.Instance.shouldSaveFavorites = false;  
                    SaveGame.Save("Favorites", GameManager.Instance.favorite);
                }
                //Ghilman
                if (id == 0){
	            	
	            	if(transform.name == "My Friends"){
	            		GameManager.Instance.BroadcastMessage("Refresh");
	            		GameManager.Instance.BroadcastMessage("GetAllFriendsList");
	            	}
	            }
	            else if(id == 1){
	            	if(transform.name == "History"){
	            		if(SaveGame.Load<List<CardData>> ( "History" )!=null){
            				GameManager.Instance.history = SaveGame.Load<List<CardData>> ( "History" );
	            		}
	            		GameManager.Instance.mItemDataList = GameManager.Instance.history;
	            		History h = GameManager.Instance.GetComponentInChildren<History>(true);
		            	
		            	h.enabled = true;
		            	//Debug.Log(h.name);
		            	h.DoRefreshDataSource();
		            	GameManager.Instance.activeTabIndex = id;
	            	}
	            	if(transform.name == "Friend Requests"){
	            		GameManager.Instance.BroadcastMessage("Refresh");
	            		GameManager.Instance.BroadcastMessage("GetAllPendingFriends");
	            	}
	            }
	            else if(id == 2){
	            	if(transform.name == "Friend Requests"){
	            		GameManager.Instance.BroadcastMessage("Refresh");
	            		GameManager.Instance.BroadcastMessage("GetAllPendingFriends");
	            	}
	            	
	            	if(transform.name == "Add Friends"){
	            		GameManager.Instance.BroadcastMessage("Refresh");
	            		
	            	}
                    if (transform.name == "Favorites")
                    {
                        if (SaveGame.Load<List<CardData>>("Favorites") != null)
                        {
                            GameManager.Instance.dummyFavorite.Clear();
                            GameManager.Instance.favorite = SaveGame.Load<List<CardData>>("Favorites");
                            for (int i = 0; i < GameManager.Instance.favorite.Count; i++)
                            {
                                CardData c = GameManager.Instance.favorite[i];
                                for (int t = 0; t < c.rData.Count; t++)
                                {
                                    //Ghilman
                                    if (c.rData[t] != null)
                                    {
                                        RoundData r = c.rData[t];
                                        r.mainIndex = i;
                                        r.innerIndex = t;
                                        GameManager.Instance.dummyFavorite.Add(r);
                                    }
                                    //Ghilman
                                }
                            }
                        }
                        GameManager.Instance.mItemDataList = GameManager.Instance.favorite;
                        Favorites h = GameManager.Instance.GetComponentInChildren<Favorites>(true);

                        h.enabled = true;
                        h.DoRefreshDataSource();
                        GameManager.Instance.activeTabIndex = id;
                    }
            	}
	            else if(id == 3){
	            	GameManager.Instance.BroadcastMessage("Refresh");
	            	GameManager.Instance.BroadcastMessage("GetAllPendingFriends");
	            	GameManager.Instance.BroadcastMessage("GetAllFriendsList");
	            }
            }
        }
        
	    void Callme(){
	    	
	    }

        public void OnSubmit(BaseEventData eventData)
        {
            if (interactable)
            {
                m_TabView.SetPage(id);
            }
        }

        public void SetupGraphic(ImageDataType type)
        {
            if (gameObject.GetChildByName<Image>("Icon") == null || gameObject.GetChildByName<VectorImage>("Icon") == null) return;

            if (type == ImageDataType.Sprite)
            {
                m_ItemIcon = gameObject.GetChildByName<Image>("Icon");
                Destroy(gameObject.GetChildByName<VectorImage>("Icon").gameObject);
            }
            else
            {
                m_ItemIcon = gameObject.GetChildByName<VectorImage>("Icon");
                Destroy(gameObject.GetChildByName<Image>("Icon").gameObject);
            }
        }
    }
}