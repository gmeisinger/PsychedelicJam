using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Public
    public Color bodyColor, hairColor, shirtColor, pantsColor;
    public SpriteRenderer bodySprite, hairSprite, shirtSprite, pantsSprite, handsSprite;
    #endregion
    #region Private
    //private SpriteRenderer bodySprite, hairSprite, pantsSprite, shirtSprite, handsSprite;
    //private Animator bodyAnimator, hairAnimator, pantsAnimator, shirtAnimator, handsAnimator;
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
        bodySprite.color = bodyColor;
        handsSprite.color = bodyColor;
        hairSprite.color = hairColor;
        shirtSprite.color = shirtColor;
        pantsSprite.color = pantsColor;
    }

    private void Update()
    {
        syncSprites();
    }
    #endregion

    #region Animation
    // load sprites from resources
    public Dictionary<string, Sprite[]> loadSprites()
    {
        Dictionary<string, Sprite[]> spritesDict = new Dictionary<string, Sprite[]>();
        //Debug.Log(spritesDict);
        foreach (SpriteRenderer s in renderers)
        {
            string baseName = s.sprite.name.Split('_')[0];
            Sprite[] sprites = Resources.LoadAll<Sprite>("PlayerSprites/" + baseName);
            spritesDict.Add(baseName, sprites);
            //Debug.Log(sprites.Length);
        }
        return spritesDict;
    }

    // sync sprite frames
    public void syncSprites()
    {
        foreach (SpriteRenderer s in renderers)
        {
            string baseName = s.sprite.name.Split('_')[0];
            int index = int.Parse(bodySprite.sprite.name.Split('_')[1]);

            Sprite newSprite = this.sprites[baseName][index];
            s.sprite = newSprite;
            //Debug.Log(baseName + tail);
        }
    }
    #endregion
}
