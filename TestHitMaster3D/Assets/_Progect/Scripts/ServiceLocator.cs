using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServiceLocator : MonoBehaviour
{
	private Dictionary<System.Type, Object> cache = new Dictionary<System.Type, Object>();

	private static ServiceLocator _singleton;
	private static ServiceLocator singleton
	{
		get
		{
			if (!_singleton)
			{
				GameObject go = new GameObject();
				go.name = "ServiceLocator";
				_singleton = go.AddComponent<ServiceLocator>();
			}
			return _singleton;
		}
	}

	/**
	 * Add Services as public static Get methods.
	 * Example:
	 * public static ChatManager GetChatManager(){
	 *     return GetService<ChatManager>("ChatManager");
	 * }	 
	 */

	public static T GetService<T>() where T : Object
	{
		Object cached;
		System.Type key = typeof(T);
		singleton.cache.TryGetValue(key, out cached);
		if (!cached)
		{
			cached = GameObject.FindObjectOfType<T>();
			if (!cached)
			{
				Debug.LogError("Could not find the Service :" + key.Name);
				return null;
			}
			else
			{
				Debug.Log("Caching Service: " + key.Name);
				singleton.cache.Add(key, cached);
			}
		}
		return cached as T;
	}
}
