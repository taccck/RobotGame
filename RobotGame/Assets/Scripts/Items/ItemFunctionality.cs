using UnityEngine;

public class ItemFunctionality : ScriptableObject
{
    public enum ItemIds { none, SteelRod, Knife };
    public enum ItemType { all, Weapon, Scrap };
    public GameObject[] allItems;
    public GameObject[] allItemDrops;
    public GameObject[] allItemIcons;

    public GameObject GetItemPrefab(ItemIds findMe) //gets a prefab for hand
    {
        foreach (GameObject item in allItems)
        {
            if (item.GetComponent<ItemScript>().id == findMe)
            {
                return item;
            }
            else
            {
                continue;
            }
        }
        return null;
    }

    public GameObject GetDropItemPrefab(ItemIds findMe) //gets a prefab for loot
    {
        foreach (GameObject item in allItemDrops)
        {
            if (item.GetComponent<ItemScript>().id == findMe)
            {
                return item;
            }
            else
            {
                continue;
            }
        }
        return null;
    }

    public GameObject GetIconItemPrefab(ItemIds findMe) //gets a prefab for ui
    {
        foreach (GameObject item in allItemIcons)
        {
            if (item.GetComponent<ItemScript>().id == findMe)
            {
                return item;
            }
            else
            {
                continue;
            }
        }
        return null;
    }

    public GameObject SpawnItem(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent, IndividualItem individualItem) //spawns hand
    {
        GameObject spawnedItem = Instantiate(prefab, parent);
        spawnedItem.transform.localRotation = rotation;
        spawnedItem.transform.localPosition = position;
        ItemScript itemScript = spawnedItem.GetComponent<ItemScript>();
        if (itemScript)
        {
            itemScript.SetIndividualItem(individualItem);
            individualItem.SetItemScript(itemScript);
            return spawnedItem;
        }
        else
        {
            Debug.LogError("Prefab doesn't contain an item script");
            return null;
        }
    }

    public GameObject DropItem(GameObject prefab, Vector3 position, Quaternion rotation, IndividualItem individualItem) //spawns loot
    {
        GameObject spawnedItem = Instantiate(prefab, position, rotation);
        ItemScript itemScript = spawnedItem.GetComponent<ItemScript>();
        if (itemScript)
        {
            itemScript.SetIndividualItem(individualItem);
            individualItem.SetItemScript(itemScript);
            return spawnedItem;
        }
        else
        {
            Debug.LogError("Prefab doesn't contain an item script");
            return null;
        }
    }
}
