using UnityEngine;

public static class LoadInformation
{
    /// <summary>
    /// Load all info about the game to the GameInstanceInformation.
    /// </summary>
    public static void LoadAllInformation()
    {
        GameInstanceInformation.PlayerAttentionLevel = PlayerPrefs.GetFloat("PlayerAttentionLevel");
        GameInstanceInformation.MoneyLocations = PlayerPrefsX.GetVector3Array("MoneyLocations");
        //GameInstanceInformation.MoneyRotations = PlayerPrefsX.GetQuaternionArray("MoneyRotations");
        GameInstanceInformation.PlayerMoney = PlayerPrefs.GetFloat("PlayerMoney");
        GameInstanceInformation.SledgehammerBought = PlayerPrefsX.GetBool("SledgehammerBought");
        GameInstanceInformation.BasementBought = PlayerPrefsX.GetBool("BasementBought");
        GameInstanceInformation.SuccessfulWaves = PlayerPrefs.GetInt("SuccessfulWaves");
    }
}
