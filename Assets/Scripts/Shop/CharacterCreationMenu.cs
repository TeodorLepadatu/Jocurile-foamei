using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour
{
	public GameObject characterPrefab; // Prefab of the character to be created
	public List<OutfitChanger> outfitChangers = new List<OutfitChanger>(); // List of OutfitChanger components

	public void RandomizeCharacter()
	{
		foreach (OutfitChanger changer in outfitChangers)
		{
			changer.Randomize();
		}
	}

	public void SaveCustomization()
	{
		for (int i = 0; i < outfitChangers.Count; i++)
		{
			PlayerPrefs.SetInt("outfit_" + i, outfitChangers[i].GetCurrentOption());
		}
		PlayerPrefs.Save();
	}

	public void Play()
	{
		SaveCustomization();
		SceneManager.LoadScene("CMinigame1");
	}
}
