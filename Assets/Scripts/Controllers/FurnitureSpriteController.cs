using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class FurnitureSpriteController : MonoBehaviour
{
    public Sprite treeSprite;

    Dictionary<FurnitureObject, GameObject> furnitureGameObjectMap;
    Dictionary<string, List<Sprite>> furnitureSpritesMap;


    // Start is called before the first frame update
    void Start()
    {
        //Create GameObjects to display for each tile
        furnitureGameObjectMap = new Dictionary<FurnitureObject, GameObject>();
        furnitureSpritesMap = SpritesMap.Load("Furniture");

    }

    void OnFurnitureAdded(FurnitureObject furn)
    {
        // add mapping of game object -> tile data to the dictionary
        if (furn != null)
        {
            GameObject furn_go = new GameObject();

            furnitureGameObjectMap.Add(furn, furn_go);

            furn_go.name = furn.FurnitureType + "_" + furn.Tile.X + "_" + furn.Tile.Y;
            furn_go.transform.position = new Vector3(furn.Tile.X, furn.Tile.Y);
            furn_go.transform.SetParent(this.transform, true);

            // TODO: implement proper Furniture sprite selection
            furn_go.AddComponent<SpriteRenderer>().sprite = treeSprite;
            furn_go.AddComponent<SortingGroup>().sortingLayerName = "Furniture";

            ApplyRandomRotationToGameObjectSprite(furn_go);
        }
    }

    void OnFurnitureRemoved(FurnitureObject obj)
    {
        // add mapping of game object -> tile data to the dictionary
        if (obj != null)
        {
            GameObject obj_go = furnitureGameObjectMap[obj];
            //Debug.Log($"WorldController.OnFurnitureRemoved: Destroying {obj_go.name}");
            Destroy(obj_go);
            furnitureGameObjectMap.Remove(obj);
        }
    }

    void ApplyRandomRotationToGameObjectSprite(GameObject go)
    {
        System.Random rng = new System.Random();
        Vector3 randomSpriteRotation = new Vector3(0.0f, 0.0f, 90.0f * rng.Next(3));
        go.GetComponent<SpriteRenderer>().transform.Rotate(randomSpriteRotation, Space.Self);
    }
}
