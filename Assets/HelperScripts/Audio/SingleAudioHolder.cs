using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

[CreateAssetMenu(menuName = "SmashBlocks/Audio/Audio Holder")]
public class SingleAudioHolder : AbstractAudioHolder
{
    public AudioClip clip = null;
    public float volume = 1;

    public override AudioClip GetAudioClip()
    {
        return clip;
    }

    public override float GetVolume()
    {
        return volume;
    }

    public override bool IsValid
    {
        get { return clip != null;  }
    }
}
