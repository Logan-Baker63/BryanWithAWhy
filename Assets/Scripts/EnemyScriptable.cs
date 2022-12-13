using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyType")]
public class EnemyScriptable : ScriptableObject
{
    public GameObject prefab;
    public enum Rarity
    {
        Common, // <12.5
        Uncommon, // <8
        Rare, // <4.5
        Epic, // < 2.5
        Legendary, // <1
    }
    public Rarity rarity = Rarity.Common;

}
