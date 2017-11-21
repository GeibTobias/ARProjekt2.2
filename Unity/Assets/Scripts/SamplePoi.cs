using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SamplePoi : MonoBehaviour {

	public GameObject panel;
	public Image poiImage;
	public Text poiText;
	public Button upButton;
	public Button downButton;
	public Button deleteButton;

	private Item item;
	private PoiScrollList scrollList;

	void Start () 
	{
		upButton.onClick.AddListener (HandleUpClick);
		downButton.onClick.AddListener (HandleDownClick);
		deleteButton.onClick.AddListener (HandleDeleteClick);
	}

	public void Setup(Item currentItem, PoiScrollList currentScrollList)
	{
		item = currentItem;
		poiText.text = item.itemName;
		poiImage.sprite = item.image;
		scrollList = currentScrollList;

	}

	public void HandleUpClick()
	{
		scrollList.MoveItemUp (item);
	}

	public void HandleDownClick()
	{
		scrollList.MoveItemDown (item);
	}

	public void HandleDeleteClick()
	{
		scrollList.RemoveItem (item);
	}
}