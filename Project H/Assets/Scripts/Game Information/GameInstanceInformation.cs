using UnityEngine;
using System.Collections;

/// <summary>
/// All info needed to load and save the current instance of the game.
/// </summary>
public class GameInstanceInformation : MonoBehaviour
{

    public static float PlayerAttentionLevel { get; set; }
    public static Vector3[] MoneyLocations { get; set; }
    public static Quaternion[] MoneyRotations { get; set; }
    public static float PlayerMoneyInAssets { get; set; }

}
