using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI; // pentru Text

public class OutfitChanger : MonoBehaviour
{
	[Header("Outfit Changer Settings")]
	public SpriteRenderer bodyPart = null;

	[Header("Outfit Options")]
	public List<Sprite> options = new List<Sprite>();
	public List<int> prices = new List<int>();

	private int currentOption = 0;
	private int lastBoughtOption = -1;

	public Text priceText;
	//keep track of bought options
	public HashSet<int> boughtOptions = new HashSet<int>();

	public int GetCurrentOption() { return currentOption; }

	//displayig the current option and its price
	public void NextOption()
	{
		currentOption++;
		if (currentOption >= options.Count)
			currentOption = 0;
		bodyPart.sprite = options[currentOption];
		UpdatePriceDisplay();
	}

	public void PreviousOption()
	{
		currentOption--;
		if (currentOption < 0)
			currentOption = options.Count - 1;
		bodyPart.sprite = options[currentOption];
		UpdatePriceDisplay();
	}

	public void Randomize()
	{
		currentOption = Random.Range(0, options.Count);
		bodyPart.sprite = options[currentOption];
		UpdatePriceDisplay();
	}

	// set the current option for the sprite renderer
	public void SetOption(int optionIndex)
	{
		if (options.Count == 0)
			return;
		currentOption = Mathf.Clamp(optionIndex, 0, options.Count - 1);
		bodyPart.sprite = options[currentOption];
		UpdatePriceDisplay();
	}

	//check if the current option is already bought
	public bool IsCurrentOptionBought()
	{
		return boughtOptions.Contains(currentOption);
	}

	public int GetCurrentOptionPrice()
	{
		if (boughtOptions.Contains(currentOption))
			return 0;
		return prices[currentOption];
	}

	//display the price of the current option
	public void UpdatePriceDisplay()
	{
		int priceToShow = GetCurrentOptionPrice();
		priceText.text = priceToShow.ToString();
	}

	public int BuyCurrentOption()
	{
		if (boughtOptions.Contains(currentOption)) // already bought
			return 0; // no cost

		int cost = prices[currentOption];
		boughtOptions.Add(currentOption); // mark as bought
		prices[currentOption] = 0; // set price to 0 after buying
		lastBoughtOption = currentOption; // save the last bought option
		return cost;
	}
}
