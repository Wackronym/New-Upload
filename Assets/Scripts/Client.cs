using UnityEngine;
using System;
using BestHTTP;
using System.Collections.Generic;
using LitJson;
using BestHTTP.SocketIO.JsonEncoders;
using BestHTTP.JSON;
public class Client : MonoBehaviour {
	
	/*******************Constants************************/

	//Header Attribute
	private const string CONTENT_TYPE_KEY = "Content-Type";
	private const string CONTENT_TYPE_JSON = "application/json";
	
	//Parse REST API Base URL

	public static string SENTENCES = "sentences";
	public static string CONFIGS = "configs";
	public IJsonEncoder JsonEncoder { get; set; }
	private string results;
	public List<SENTENCES> SList;
	public List<CONFIGS> CList;
	void Start(){
		
		HTTPRequest www = new HTTPRequest(new Uri(  GameManager.Instance.webURLPrefix + Client.SENTENCES), (request, response) => {
			HTTPResponse res = (HTTPResponse)response;
			if(res.IsSuccess){
				
				List<object> dic = Json.Decode(res.DataAsText) as List<object>;
				SList = new List<SENTENCES>();
				foreach(Dictionary<string, object> a in dic){
					SENTENCES s = new SENTENCES();
					s._id = a["_id"].ToString();
					s.__v = a["__v"].ToString();
					s.puzzle = a["puzzle"].ToString();
					SList.Add(s);
				}
				
				HTTPRequest conf = new HTTPRequest(new Uri(  GameManager.Instance.webURLPrefix + Client.CONFIGS), (requestC, responseC) => {
					HTTPResponse resConf = (HTTPResponse)responseC;
					if(resConf.IsSuccess){
				
						List<object> conRes = Json.Decode(resConf.DataAsText) as List<object>;
						CList = new List<CONFIGS>();
						foreach(Dictionary<string, object> b in conRes){
							CONFIGS c = new CONFIGS();
							c._id 						= 		b["_id"].ToString();			
							c.intro_text 				= 		b["intro_text"].ToString();
							c.group_size_min 			= 		b["group_size_min"].ToString();
							c.group_size_max 			= 		b["group_size_max"].ToString();
							c.game_session_time_limit 	= 		b["game_session_time_limit"].ToString();
							c.game_rounds_min 			= 		b["game_rounds_min"].ToString();
							c.game_rounds_max 			= 		b["game_rounds_max"].ToString();
							c.game_letters 				= 		b["game_letters"].ToString();
							c.loop_time = new List<string>();
							foreach(object t in b["loop_time"] as List<object>){
								c.loop_time.Add(t.ToString());
							}
							c.leters_round_min 			= 		b["leters_round_min"].ToString();
							c.leters_round_max 			= 		b["leters_round_max"].ToString();
							c.round_countdown_time		= 		b["round_countdown_time"].ToString();
							c.__v						= 		b["__v"].ToString();
							c.created					= 		b["created"].ToString();
							c.name						= 		b["name"].ToString();
							
							CList.Add(c);
						}
					}
				}).Send(); 
			}
		}).Send(); 
	}
	
	public SENTENCES GetRandomS()
	{
		return SList[UnityEngine.Random.Range(0, SList.Count-1)];
	}
}

[Serializable]
public class SENTENCES
{	
	[SerializeField]
	public string _id;
	[SerializeField]
	public string __v;
	[SerializeField]
	public string puzzle;
}
[Serializable]
public class CONFIGS
{
	[SerializeField]
	public string _id;
	[SerializeField]
	public string intro_text;
	[SerializeField]
	public string group_size_min;
	[SerializeField]
	public string group_size_max;
	[SerializeField]
	public string game_session_time_limit;
	[SerializeField]
	public string game_rounds_min;
	[SerializeField]
	public string game_rounds_max;
	[SerializeField]
	public string game_letters;
	[SerializeField]
	public List<string> loop_time;
	[SerializeField]
	public string leters_round_min;
	[SerializeField]
	public string leters_round_max;
	[SerializeField]
	public string round_countdown_time;
	[SerializeField]
	public string __v;
	[SerializeField]
	public string created;
	[SerializeField]
	public string name;
}
[Serializable]
public class Player
{
	public string _id ;
	public string displayName;
	public string provider;
	public string username;
	public int __v;
	public string resetPasswordExpires;
	public string resetPasswordToken;
	public string created;
	public List<string> roles;
	public string profileImageURL;
	public string email;
	public string lastName;
	public string firstName;
}



