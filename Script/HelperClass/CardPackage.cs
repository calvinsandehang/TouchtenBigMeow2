using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GlobalDefine;


public class CardPackage
{
    public HandType CardPackageType{ get; set; }
    public HandRank CardPackageRank { get; set; }
    public List<CardModel> CardPackageContent { get; set; } = new List<CardModel>();
}

[Serializable]
public class AiCardInfo
{
    public List<CardPackage> CardPackages = new List<CardPackage>();

    public void AddCardPackage(CardPackage cardPackage)
    {
        CardPackages.Add(cardPackage);
    }

    public void RemoveCardPackage(CardPackage cardPackage)
    {
        CardPackages.RemoveAll(package => package == cardPackage);
    }


    public void ClearCardPackages() 
    {
        CardPackages.Clear();
    }



}
