using UnityEngine;

[CreateAssetMenu(fileName = "LayerMaskVariable", menuName = "UnityHelperScripts/Variables/LayerMaskVariable", order = 0)]
public class LayerMaskVariable : ScriptableObject {
    public LayerMask value;

    public static implicit operator LayerMask(LayerMaskVariable reference)
    {
            return reference.value;
    }
}