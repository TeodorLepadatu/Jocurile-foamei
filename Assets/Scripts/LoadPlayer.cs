using UnityEngine;
using System.Collections.Generic;

public class LoadPlayer : MonoBehaviour
{
	public List<OutfitChanger> outfitChangers;

	void Start()
	{   // setting the player outfit options based on saved preferences
		for (int i = 0; i < outfitChangers.Count; i++)
		{
			int savedOption = PlayerPrefs.GetInt("outfit_" + i, 0); 
			outfitChangers[i].SetOption(savedOption);
		}
	}
}
