using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "SmashBlocks/Animation Settings/Single Animation Settings")]
public class SingleAnimationSettings : AnimationSettings
{
    [BoxGroup("AnimationInfo")]
#if UNITY_EDITOR
    [OnValueChanged("FillSprite"), Tooltip("Add animation spritesheet here to auto fill the sprites")]
    [SerializeField] private Sprite spritesheet = null;
    private void FillSprite()
    {
        if(spritesheet == null)
        {
            sprites = new Sprite[0];
            return;
        }
        string path = AssetDatabase.GetAssetPath(spritesheet);
        List<Object> loadedSprites = new List<Object>();
        loadedSprites.AddRange(AssetDatabase.LoadAllAssetRepresentationsAtPath(path));
        sprites = new Sprite[loadedSprites.Count];
        int skipped = 0;
        for (int i = 0; i < loadedSprites.Count; i++)  // Skip the first one as it is the full spritesheet
        {
            sprites[i - skipped] = (Sprite)loadedSprites[i];
        }
    }
#endif
    [SerializeField] private Sprite[] sprites;

    public override Sprite GetSprite(ref int index)
    {
        index++;
        if (index >= sprites.Length)
        {
            index = 0;
            if (hasEndEvent)
            {
                OnEndEvent?.Invoke();
            }
            FinishedLoop();
        }
        return sprites[reverse? sprites.Length - index - 1 : index];
    }

    public override Sprite[] GetSprites()
    {
        return sprites;
    }
}
