using System;
using LitJson;
using UnityEngine;
using System.Collections.Generic;

public class Language
{
		public enum language
		{
				CN = 0,
				EN
		}

		private static Dictionary<string, string> languageDic = new Dictionary<string, string> ();
		private static language _language = language.CN;

		public static void LoadLanguage (string path)
		{
				if (languageDic.Count > 0) 
						return;

				TextAsset s = Resources.Load (path) as TextAsset;
				string strData = s.text;  

				JsonReader reader = new JsonReader (strData);
				JsonData data = JsonMapper.ToObject (reader);

				foreach (KeyValuePair<string, JsonData> kvp in data.Collection) {
						languageDic.Add (kvp.Key, kvp.Value.ToString ());
				}
		}

		public static string GetContentOfKey (string key)
		{
				if (languageDic.Count < 1) {
				LoadLanguage ("Language/"+_language.ToString());
		}

				if (languageDic.ContainsKey (key)) {
						return languageDic [key];
				} else {
					//	Debug.LogError ("Content of Key ---" + key + " ---Not Found!");
						return "Anna";
				}
		}

		public static void InitLanguage ()
		{
			if (PlayerPrefs.HasKey ("Language")) 
			{
				_language = (language)PlayerPrefs.GetInt ("Language");
			} 
			else 
			{

#if UNITY_IPHONE
			if (Application.systemLanguage.ToString() == "Chinese")
				_language = language.CN;
			else
				_language = language.EN;
#endif
			PlayerPrefs.SetInt ("Language", (int)_language);
			}
		}

		public static void ChangeLanguage (language lan)
		{
			_language = lan;			
			languageDic = new Dictionary<string, string> ();
			LoadLanguage ("Language/"+_language.ToString());
		}
}


