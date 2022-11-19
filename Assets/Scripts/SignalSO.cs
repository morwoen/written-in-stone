using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Signal", menuName = "ScriptableObjects/Signal")]
public class SignalSO : ScriptableObject
{
    public delegate void Signal();
    public event Signal trigger;
}
