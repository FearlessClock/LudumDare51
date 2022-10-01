using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[CreateAssetMenu(menuName = "SmashBlocks/Animation Settings/Single Animation with glitch Settings")]
public class SingleAnimationWithGlitchEffectSettings : AnimationSettings
{
    private bool isGlitching = false;
    [OnValueChanged("FillSprite"), Tooltip("Add animation spritesheet here to auto fill the sprites"), BoxGroup("AnimationInfo"), SerializeField]
    private Sprite normalSpritesheet = null;
    [BoxGroup("AnimationInfo"), PreviewField(50)]
    [SerializeField] private Sprite[] normalLoopSprites;
    [OnValueChanged("FillSprite"), Tooltip("Add animation spritesheet here to auto fill the sprites"), BoxGroup("AnimationInfo"), SerializeField]
    private Sprite glitchedSpritesheet = null;
    [BoxGroup("AnimationInfo"), PreviewField(50)]
    [SerializeField] private Sprite[] glitchLoopSprites;
    [BoxGroup("AnimationInfo"), Range(0, 1f)]
    [SerializeField] private float glitchChance = 1;


#if UNITY_EDITOR
    private void FillSprite()
    {
        if (normalSpritesheet == null)
        {
            normalLoopSprites = new Sprite[0];
            return;
        }
        string path = AssetDatabase.GetAssetPath(normalSpritesheet);
        List<Object> loadedSprites = new List<Object>();
        loadedSprites.AddRange(AssetDatabase.LoadAllAssetRepresentationsAtPath(path));
        normalLoopSprites = new Sprite[loadedSprites.Count];
        int skipped = 0;
        for (int i = 0; i < loadedSprites.Count; i++)  // Skip the first one as it is the full spritesheet
        {
            normalLoopSprites[i - skipped] = (Sprite)loadedSprites[i];
        }
        
        if (glitchedSpritesheet == null)
        {
            glitchLoopSprites = new Sprite[0];
            return;
        }
        path = AssetDatabase.GetAssetPath(glitchedSpritesheet);
        loadedSprites.Clear();
        loadedSprites = new List<Object>();
        loadedSprites.AddRange(AssetDatabase.LoadAllAssetRepresentationsAtPath(path));
        glitchLoopSprites = new Sprite[loadedSprites.Count];
        skipped = 0;
        for (int i = 0; i < loadedSprites.Count; i++)  // Skip the first one as it is the full spritesheet
        {
            normalLoopSprites[i - skipped] = (Sprite)loadedSprites[i];
        }
    }
#endif

    private Sprite GetGlitchSprite(ref int index)
    {
        index++;
        if (index >= glitchLoopSprites.Length)
        {
            index = 0;
            isGlitching = false;
            if (hasEndEvent)
            {
                OnEndEvent?.Invoke();
            }
            FinishedLoop();
        }
        return glitchLoopSprites[GetIndex(index)];
    }

    private Sprite GetNormalSprite(ref int index)
    {
        index++;
        if (index >= normalLoopSprites.Length)
        {
            index = 0;
            if (Random.Range(0,1f) < glitchChance)
            {
                isGlitching = true;
            }
            if (hasEndEvent)
            {
                OnEndEvent?.Invoke();
            }
            FinishedLoop();
        }
        return normalLoopSprites[GetIndex(index)];
    }

    private int GetIndex(int index)
    {
        return reverse ? normalLoopSprites.Length - index - 1 : index;
    }

    public override Sprite[] GetSprites()
    {
        return normalLoopSprites;
    }

    public override Sprite GetSprite(ref int index)
    {
        if (isGlitching)
        {
            return GetGlitchSprite(ref index);
        }
        else
        {
            return GetNormalSprite(ref index);
        }
    }
}
