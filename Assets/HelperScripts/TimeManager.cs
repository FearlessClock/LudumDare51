using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public static float deltaTime => Time.deltaTime;
    public static float fixedDeltaTime => Time.fixedDeltaTime;
}
