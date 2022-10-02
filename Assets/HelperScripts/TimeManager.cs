using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static float deltaTime => timeScale * Time.deltaTime;
    public static float fixedDeltaTime => timeScale * Time.fixedDeltaTime;

    private static float timeScale = 1f;
    public static void SetTimeScale(float scale) => timeScale = scale;
}
