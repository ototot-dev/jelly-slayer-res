using UnityEngine;
using UnityEditor;

public class YourClassAsset
{
	[MenuItem("POLYSOFT/Humans/Items/item")]
	public static void CreateItemAsset ()
	{
		ScriptableObjectUtility.CreateItemAsset<polysoft_Item> ();
	}
    [MenuItem("POLYSOFT/Humans/Items/hair")]
    public static void CreateHairAsset()
    {
        ScriptableObjectUtility.CreateHairAsset<polysoft_Hair>();
    }
    [MenuItem("POLYSOFT/Humans/Items/underwear")]
    public static void CreateUnderwearAsset()
    {
        ScriptableObjectUtility.CreateUnderwearAsset<polysoft_Underwear>();
    }
}