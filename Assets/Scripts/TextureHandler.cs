// @author Zachary Kidd-Smith
// Handles the textures that need to be applied to a character/object, namely stretching the object to match the aspect
// ratio of the texture.

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextureHandler : MonoBehaviour
{
    public Texture texture = null;

    private Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        // Get renderer from GameObject.
        renderer = GetComponent<Renderer>();

        renderer.material.EnableKeyword("_NORMALMAP");
        renderer.material.EnableKeyword("_METALLICGLOSSMAP");

        // Apply texture to base map of material.
        renderer.material.SetTexture("_BaseMap", texture);

        
        // Resize object to texture dimensions.
        int width = texture.width;
        int height = texture.height;
        float aspectRatio = (float)width / (float)height;

        Vector3 newScale = transform.localScale;

        /*
        newScale.x = width * transform.localScale.x;
        newScale.z = height * transform.localScale.z;
        */

        newScale.x = newScale.z * aspectRatio;

        //Debug.Log("Aspect Ratio: " + aspectRatio + "\nz: " + newScale.z + "\n new x: " + newScale.x);

        transform.localScale = newScale;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
