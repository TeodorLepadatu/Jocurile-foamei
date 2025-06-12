using UnityEngine;
using System.Collections.Generic;

public class OutfitChanger : MonoBehaviour
{
	[Header("Outfit Changer Settings")]
	public SpriteRenderer bodyPart = null; // Reference to the SpriteRenderer component

	[Header("Outfit Options")]
	public List<Sprite> options = new List<Sprite>(); 

	private int currentOption = 0;

	public void NextOption()
	{
		currentOption++;
		if (currentOption >= options.Count)
			currentOption = 0;
		bodyPart.sprite = options[currentOption];
	}

	public void PreviousOption()
	{
		currentOption--;
		if (currentOption < 0)
			currentOption = options.Count - 1;
		bodyPart.sprite = options[currentOption];
	}

	public void Randomize()
	{
		currentOption = Random.Range(0, options.Count); 
		bodyPart.sprite = options[currentOption];
	}


	public int GetCurrentOption()
	{
		return currentOption;
	}

	public void SetOption(int optionIndex)
	{
		if (options.Count == 0)
			return;

		currentOption = Mathf.Clamp(optionIndex, 0, options.Count - 1);
		bodyPart.sprite = options[currentOption];
	}
}
