using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum MapGenerationElement { Wall }
public class MapGenerator : MonoBehaviour 
{
    [Tooltip("Wall placement design texture")]
    [SerializeField]
    private Texture2D wallTexture;

    [Tooltip("Prefab used to represent a wall within the map")]
    [SerializeField]
    private GameObject wallPrefab;

	void Start () 
    {
        if (wallTexture) {
            InstantiateMapElement(wallTexture, MapGenerationElement.Wall);
        }
	}
    void InstantiateMapElement(Texture2D elementPositions, MapGenerationElement element)
    {
        for (int x = 0; x < elementPositions.height; x++)
        {
            for (int z = 0; z < elementPositions.width; z++) {
                Color pixel = elementPositions.GetPixel(x, z);
                //if the pixel is not white instanciate an object of corresponding type
                if (!(pixel.r == 1f && pixel.g == 1f && pixel.b == 1f))
                {
                    Vector3 position = new Vector3((x - elementPositions.height/2), 0.0f, (z - elementPositions.width/2));
                    switch (element) {
                        case MapGenerationElement.Wall:
                            GameObject mapElement = Instantiate(wallPrefab, gameObject.transform);
                            mapElement.transform.localPosition += position;
                            break;
                    }
                }
            }
        }
    }
}