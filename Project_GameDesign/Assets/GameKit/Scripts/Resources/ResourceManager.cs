using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceManager : MonoBehaviour
{
	[System.Serializable] public class DisplayedResource
	{
		[HideInInspector]public string id;
		public CustomResource customResource;

		public Text resourceAmountText = null;
		public Image resourceAmountImage = null;
	}
	
	[System.Serializable] public class ResourceData
	{
		public Sprite resourceIcon = null;

		[Header("Values")]
		public int maxResourceAmount = 100;
		public int currentResourceAmount = 1;

		[Header("Display")]
		public Text resourceAmountText = null;
		public Image resourceAmountImage = null;
	}

	public List<string> idList = new List<string>();

	[SerializeField] public List<DisplayedResource> displayedResources = new List<DisplayedResource>();

	public Dictionary<string, ResourceData> resources = new Dictionary<string, ResourceData>();
	public bool hasBegun = false;

	private void LoadResources()
	{
		foreach (DisplayedResource displayedResource in displayedResources)
		{
			ResourceData data = new ResourceData();
			data.resourceIcon = displayedResource.customResource.resourceIcon;
			data.maxResourceAmount = displayedResource.customResource.maxResourceAmount;
			data.currentResourceAmount = displayedResource.customResource.startResourceAmount;
			data.resourceAmountText = displayedResource.resourceAmountText;
			data.resourceAmountImage = displayedResource.resourceAmountImage;
			
			resources.Add(displayedResource.customResource.resourceID, data);
			
			DisplayDicoResources(displayedResource.customResource.resourceID);
		}

		hasBegun = true;
		displayedResources = null;
	}

	private void OnValidate()
	{
		LoadIDArray();
	}

	#region ARRAY
	
	private  void DisplayResources (int index)
	{
		if (displayedResources[index].resourceAmountText != null)
		{
			displayedResources[index].resourceAmountText.text = displayedResources[index].customResource.startResourceAmount.ToString();
		}

		if (displayedResources[index].resourceAmountImage != null)
		{
			displayedResources[index].resourceAmountImage.sprite = displayedResources[index].customResource.resourceIcon;
		}
	}
	
	public bool ResourceCheck(int index, int amountRequired)
	{
		if(amountRequired > displayedResources[index].customResource.maxResourceAmount)
		{
			Debug.LogWarning("Amount required superior to maximum resource amount ! Condition will never be met", gameObject);
			return false;
		}
		
		return displayedResources[index].customResource.startResourceAmount >= amountRequired;
	}
	
	public bool ChangeResourceAmount (int index, int amount)
	{
		//If Array Index not Out of Range
		if(index < displayedResources.Count)
		{
			//Are we adding or substracting resources ?
			if(amount < 0)
			{
				//Do we have enough resources to substract ?
				if(displayedResources[index].customResource.startResourceAmount + amount < 0)
				{
					Debug.LogWarning("Trying to remove more resources than you currently have.", gameObject);
					return false;
				}

				displayedResources[index].customResource.startResourceAmount = Mathf.Clamp(displayedResources[index].customResource.startResourceAmount + amount, 0, displayedResources[index].customResource.maxResourceAmount);

				DisplayResources(index);
				return true;
			}

			displayedResources[index].customResource.startResourceAmount = Mathf.Clamp(displayedResources[index].customResource.startResourceAmount + amount, 0, displayedResources[index].customResource.maxResourceAmount);
			DisplayResources(index);
			return true;
			
		}

		Debug.LogWarning("Wrong Index ! Remember that indexes start at 0, not 1", gameObject);
		return false;
		
	}

	#endregion

	#region DICTIONARY

	private void LoadIDArray()
	{
		int arraySize = displayedResources.Count;
		
		idList.Clear();

		if (arraySize == 0)
		{
			return;
		}
		
		for (int i = 0; i < arraySize; i++)
		{
			if (displayedResources[i].customResource == null || idList.Contains(displayedResources[i].customResource.resourceID)) continue;

			displayedResources[i].id = displayedResources[i].customResource.resourceID;
			idList.Add(displayedResources[i].customResource.resourceID);
		}
	}
		public bool ChangeDicoResourceAmount (string id, int amount)
		{
			if(amount < 0 && resources[id].currentResourceAmount + amount < 0)
			{
				Debug.LogWarning("Trying to remove more resources than you currently have.", gameObject);
				return false;
				
			}

			resources[id].currentResourceAmount = Mathf.Clamp(resources[id].currentResourceAmount + amount, 0, resources[id].maxResourceAmount);
			DisplayDicoResources(id);
			return true;
		}
		
		public bool ResourceDicoCheck(string id, int amountRequired)
		{
			if (amountRequired <= resources[id].maxResourceAmount)
				return resources[id].currentResourceAmount >= amountRequired;
			
			Debug.LogWarning("Amount required superior to maximum resource amount ! Condition will never be met", gameObject);
			return false;

		}

		private void DisplayDicoResources (string id)
		{
			if (resources[id].resourceAmountText != null)
			{
				resources[id].resourceAmountText.text = resources[id].currentResourceAmount.ToString();
			}

			if (resources[id].resourceAmountImage != null)
			{
				resources[id].resourceAmountImage.sprite = resources[id].resourceIcon;
			}
		}
	#endregion

	private void Awake ()
	{
		LoadResources();
		
		/*for (int i = 0; i < displayedResources.Length; i++)
		{
			DisplayResources(i);
		}*/
	}
}
