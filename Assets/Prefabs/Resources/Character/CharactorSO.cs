using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Character", menuName = "SO/CharacterSO", order = 1)]
public class CharactorSO : ScriptableObject
{
    public int CharacterID;
    public string characterName;
    public float hp;
    public float ad;
    public float level;
    public float armor;
}
