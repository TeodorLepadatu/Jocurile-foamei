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
	public HashSet<int> boughtOptions = new HashSet<int>();

	public int GetCurrentOption() { return currentOption; }

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

	public void SetOption(int optionIndex)
	{
		if (options.Count == 0)
			return;
		currentOption = Mathf.Clamp(optionIndex, 0, options.Count - 1);
		bodyPart.sprite = options[currentOption];
		UpdatePriceDisplay();
	}

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

	public void UpdatePriceDisplay()
	{
		int priceToShow = GetCurrentOptionPrice();
		priceText.text = priceToShow.ToString();
	}

	public int BuyCurrentOption()
	{
		if (boughtOptions.Contains(currentOption))
			return 0;

		int cost = prices[currentOption];
		boughtOptions.Add(currentOption);
		prices[currentOption] = 0;
		lastBoughtOption = currentOption;
		return cost;
	}
}
