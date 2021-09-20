using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    #region Public
    public Color bodyColor, hairColor, shirtColor, pantsColor;
    public SpriteRenderer bodySprite, hairSprite, shirtSprite, pantsSprite, handsSprite;
    #endregion
    #region Private
    private SpriteRenderer[] renderers;
    private Dictionary<string, Sprite[]> sprites;
    #endregion

    #region Monobehaviors
    private void Awake()
    {
        renderers = new SpriteRenderer[] { shirtSprite, pantsSprite, hairSprite, handsSprite };
        sprites = loadSprites();
    }

    private void Start()
    {
        UpdateColors();
    }

    private void LateUpdate()
    {
        syncSprites();
    }
    #endregion

    #region Animation
    /// <summary>
    /// Loads sprites from resources folder into a dictionary, keyed by the base name of the sprite.
    /// </summary>
    /// <returns></returns>
    public Dictionary<string, Sprite[]> loadSprites()
    {
        Dictionary<string, Sprite[]> spritesDict = new Dictionary<string, Sprite[]>();
        foreach (SpriteRenderer s in renderers)
        {
            string baseName = s.sprite.name.Split('_')[0];
            Sprite[] sprites = Resources.LoadAll<Sprite>("PlayerSprites/" + baseName);
            spritesDict.Add(baseName, sprites);
        }
        return spritesDict;
    }

    /// <summary>
    /// syncs all sprites to the body sprite, allowing us to use a single animator
    /// </summary>
    public void syncSprites()
    {
        foreach (SpriteRenderer s in renderers)
        {
            string baseName = s.sprite.name.Split('_')[0];
            int index = int.Parse(bodySprite.sprite.name.Split('_')[1]);

            Sprite newSprite = this.sprites[baseName][index];
            s.sprite = newSprite;
        }
    }

    public void UpdateColors()
    {
        bodySprite.color = bodyColor;
        handsSprite.color = bodyColor;
        hairSprite.color = hairColor;
        shirtSprite.color = shirtColor;
        pantsSprite.color = pantsColor;
    }
    #endregion
}
