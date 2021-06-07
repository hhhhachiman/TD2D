using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteSorting : MonoBehaviour
{
    //static will no  change order on update,only on start
    public bool isStatic;
    //multiplier for accuracy increasing
    public float rangeFactor = 100f;

    //Sprites list for this object an children
    private Dictionary<SpriteRenderer, int> sprites = new Dictionary<SpriteRenderer, int>();

     void Awake()
    {
        foreach (SpriteRenderer sprite in GetComponentsInChildren<SpriteRenderer>())
        {
            sprites.Add(sprite, sprite.sortingOrder);
        }
    }

    void Start()
    {
        UpdateSortingOrder();
    }

    private void UpdateSortingOrder()
    {
        foreach (KeyValuePair<SpriteRenderer,int> sprite in sprites)
        {
            sprite.Key.sortingOrder = sprite.Value - (int)(transform.position.y * rangeFactor);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isStatic==false)
        {
            UpdateSortingOrder();
        }
    }
}
