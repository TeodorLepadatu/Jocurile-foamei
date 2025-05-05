using NUnit.Framework;
using UnityEditorInternal;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class OutfitChanger : MonoBehaviour
{
	[Header("Outfit Changer Settings")]
	public SpriteRenderer bodyPart = null; // Reference to the SpriteRenderer component

	[Header("Outfit Options")]
	public List<Sprite> options = new List<Sprite>(); // List of outfits to choose from

	private int currentOption = 0;
	public void NextOption()
	{
		currentOption++;
		if (currentOption >= options.Count)
		{
			currentOption = 0; // Loop back to the first option
		}
		bodyPart.sprite = options[currentOption]; // Change the sprite to the next option
	}

	public void PreviousOption()
	{
		currentOption--;
		if (currentOption < 0)
		{
			currentOption = options.Count - 1; // Loop back to the last option
		}
		bodyPart.sprite = options[currentOption]; // Change the sprite to the previous option
	}

	public void Randomize()
	{
		currentOption = Random.Range(0, options.Count - 1); // Get a random index
		bodyPart.sprite = options[currentOption]; // Change the sprite to the random option
	}
}