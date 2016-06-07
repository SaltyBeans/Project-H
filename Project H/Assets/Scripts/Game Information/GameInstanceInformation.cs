using UnityEngine;

/// <summary>
/// All info needed to load and save the current instance of the game.
/// </summary>
public class GameInstanceInformation : MonoBehaviour
{
    public bool loadedGame;
    void Awake()
    {
        loadedGame = false;
    }

    public static float PlayerAttentionLevel { get; set; }
    public static Vector3[] MoneyLocations { get; set; }
    //public static Quaternion[] MoneyRotations { get; set; }
    public static float PlayerMoney { get; set; }
    public static bool SledgehammerBought { get; set; }
    public static bool BasementBought { get; set; }
    public static int SuccessfulWaves { get; set; }

}
