using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class Item
{
	public string poiID;
	public string itemName;
	public Sprite image;

	public Item (string poiID, string itemName, Sprite image){
		this.poiID = poiID;
		this.itemName = itemName;
		this.image = image;
	}
}

public class PoiScrollList : MonoBehaviour {

	public List<Item> itemList;
	public Transform contentPanel;
	public Text myItemCountDisplay;
	public SimpleObjectPool poiObjectPool;

    public RestConsumer restConsumer; 


	// Use this for initialization
	void Start () 
	{
        restConsumer = GameObject.Find("RestConsumer").GetComponent<RestConsumer>();

        // register events
        restConsumer.routeUpdate += RestConsumer_routeUpdate;

        // init itemList
        string[] pois = restConsumer.getPOIList();
        initItemList(pois); 

        RefreshDisplay ();
	}

    void RefreshDisplay()
	{
		myItemCountDisplay.text = "POI Count: " + itemList.Count.ToString () + "/10";
		RemovePois ();
		AddPois ();
	}

	private void RemovePois()
	{
		while (contentPanel.childCount > 0) 
		{
			GameObject toRemove = transform.GetChild(0).gameObject;
			poiObjectPool.ReturnObject(toRemove);
		}
	}

	private void AddPois()
	{
		for (int i = 0; i < itemList.Count; i++) 
		{
			Item item = itemList[i];
			GameObject newPoi = poiObjectPool.GetObject();
			newPoi.transform.SetParent(contentPanel);

			SamplePoi samplePoi = newPoi.GetComponent<SamplePoi>();
			samplePoi.Setup(item, this);
		}
	}

    private void initItemList(string[] pois)
    {
        List<POI> poiObjList = new List<POI>(GameObject.FindObjectsOfType<POI>());
        itemList.Clear(); 

        foreach (string poiId in pois)
        {
            POI p = poiObjList.Find(x => x.poiID == poiId);

            if (p != null )
            {
                itemList.Add(new Item(p.poiID, p.name, p.poiImage));
            }
        }

        RefreshDisplay();
    }

    private void RestConsumer_routeUpdate(string[] route)
    {
        initItemList(route); 
    }


    public void AddItem(Item itemToAdd)
	{
		if (itemList.Find(x => x.poiID == itemToAdd.poiID) == null) {
			itemList.Add (itemToAdd);

            // add poi to server list
            restConsumer.addPOI(itemToAdd.poiID); 
		}

		RefreshDisplay ();
	}

	public void UpdateItemList(Item[] itemsToAdd) {
		RemoveAllItems ();

		for (int i = 0; i < itemsToAdd.Length; i++) {
			AddItem (itemsToAdd [i]);
		}
	}

	public void RemoveItem(Item itemToRemove)
	{
		for (int i = itemList.Count - 1; i >= 0; i--) 
		{
			if (itemList[i] == itemToRemove)
			{
				itemList.RemoveAt(i);

                // remove item from poi list
                restConsumer.removePOI(itemList[i].poiID);
			}
		}

		RefreshDisplay ();
	}

	void RemoveAllItems() {
		itemList.Clear();

		RefreshDisplay ();
	}

	public void MoveItemUp(Item item) {
		int index = itemList.IndexOf (item);
		if (index > 0) {
			itemList.Remove (item);
			itemList.Insert (index - 1, item);

            restConsumer.addPOIList(this.getPoiIdArray());
		}

		RefreshDisplay ();
	}

	public void MoveItemDown(Item item) {
		int index = itemList.IndexOf (item);
		if (index < itemList.Count - 1) {
			itemList.Remove (item);
			itemList.Insert (index + 1, item);

            restConsumer.addPOIList(this.getPoiIdArray());
        }

		RefreshDisplay ();
	}

    private string[] getPoiIdArray()
    {
        string[] result = new string[0];
        if (itemList.Count != 0)
        {
            List<string> tmp = new List<string>(); 
            foreach (Item item in itemList)
            {
                tmp.Add(item.poiID); 
            }

            result = tmp.ToArray(); 
        }

        return result; 
    }
}