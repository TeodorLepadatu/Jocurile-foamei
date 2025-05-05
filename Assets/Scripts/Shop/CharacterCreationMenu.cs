using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CharacterCreation : MonoBehaviour
{

    public List<OutfitChanger> outfitChangers = new List<OutfitChanger>(); // List of OutfitChanger components

	public void RandomizeCharacter()
    {
        foreach (OutfitChanger changer in outfitChangers)
        {
			changer.Randomize(); // Call the Randomize method on each OutfitChanger
		}

    }

    public void Submit()
    {
        
        SceneManager.LoadScene(1); // Load the MainMenu scene
	}
}
