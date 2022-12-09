using UnityEngine;

[CreateAssetMenu(menuName = "Resources/New Resource")]
public class CustomResource : ScriptableObject
{
    public string resourceID = "Resource ID";
    public Sprite resourceIcon = null;
    
    public int maxResourceAmount = 100;
    public int startResourceAmount = 1;
}