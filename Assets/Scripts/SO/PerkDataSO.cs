using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu()]
public abstract class PerkDataSO : ScriptableObject
{
    public string perkName;
    public Sprite icon;
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.white;
    public bool isActive = false;

    public UnityEvent OnActivated;
    public UnityEvent OnDeactivated;

    public abstract void ApplyEffect(bool isActive); 
    
}
