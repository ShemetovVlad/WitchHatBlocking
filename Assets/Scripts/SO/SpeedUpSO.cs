using UnityEngine;

[CreateAssetMenu(fileName = "SpeedUpSO", menuName = "Scriptable Objects/SpeedUpSO")]
public class SpeedUpSO : ScriptableObject
{
    public float speedMultiplier = 2f;
    public string perkName = "Speed x2";
    public Sprite icon;
    public Color activeColor = Color.green;
    public Color inactiveColor = Color.white;
    public bool isActive = false;
}
