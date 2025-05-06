using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEditor;

public class CharacterCreation : MonoBehaviour
{

	public GameObject characterPrefab; // Prefab of the character to be created
	public List<OutfitChanger> outfitChangers = new List<OutfitChanger>(); // List of OutfitChanger components

	public void RandomizeCharacter()
    {
        foreach (OutfitChanger changer in outfitChangers)
        {
			changer.Randomize(); // Call the Randomize method on each OutfitChanger
		}

    }

    public void Play()
    {
        SceneManager.UnloadScene("Shop");
        SceneManager.LoadScene("MainScene1");
    }

    public void Submit()
    {
        PrefabUtility.SaveAsPrefabAsset(gameObject, "Assets/Character/Prefabs/Player.prefab");
		SceneManager.LoadScene(1); // Load the MainMenu scene
	}
}
