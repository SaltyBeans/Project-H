using UnityEngine;
public static class SaveInformation
{
    /// <summary>
    /// Save all the info about the current instance of the game.
    /// </summary>
    public static void SaveAllInformation()
    {
        PlayerPrefs.SetFloat("PlayerAttentionLevel", GameInstanceInformation.PlayerAttentionLevel);
        PlayerPrefsX.SetVector3Array("MoneyLocations", GameInstanceInformation.MoneyLocations);
        //PlayerPrefsX.SetQuaternionArray("MoneyRotations", GameInstanceInformation.MoneyRotations);
        PlayerPrefs.SetFloat("PlayerMoney", GameInstanceInformation.PlayerMoney);
        PlayerPrefsX.SetBool("SledgehammerBought", GameInstanceInformation.SledgehammerBought);
        PlayerPrefsX.SetBool("BasementBought", GameInstanceInformation.BasementBought);
        PlayerPrefs.SetInt("SuccessfulWaves", GameInstanceInformation.SuccessfulWaves);
    }

}