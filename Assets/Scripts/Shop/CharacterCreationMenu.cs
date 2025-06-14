using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CharacterCreation : MonoBehaviour
{
	public GameObject characterPrefab;
	// one outfit changer for each body part
	public List<OutfitChanger> outfitChangers = new List<OutfitChanger>();

	public void RandomizeCharacter()
	{
		foreach (OutfitChanger changer in outfitChangers)
		{
			changer.Randomize();
		}
	}

	public void SaveCustomization()
	{   // for each outfit changer, check if the current option is bought and if not, try to buy it
		for (int i = 0; i < outfitChangers.Count; i++)
		{
			var changer = outfitChangers[i];
			int currentOption = changer.GetCurrentOption();
			int price = changer.GetCurrentOptionPrice();

			if (!changer.IsCurrentOptionBought() && price > 0)
			{
				if (CurrencyHolder.getCurrency() >= price)
				{
					CurrencyHolder.addCurrency(-price);
					changer.BuyCurrentOption();
					PlayerPrefs.SetInt("outfit_" + i, currentOption);
					Debug.Log($"Componenta {i} cumparata pentru {price} bani. Bani ramasi: {CurrencyHolder.getCurrency()}");
				}
				else
				{
					Debug.Log($"NU ai destui bani pentru componenta {i}. Componenta NU a fost cumparata!");
					
				}
			}
			else
			{
			
				PlayerPrefs.SetInt("outfit_" + i, currentOption);
			}
		}

		PlayerPrefs.Save();
	}

	public void Play()
	{
		SaveCustomization();
		SceneManager.LoadScene("MainMenu");
	}
}
