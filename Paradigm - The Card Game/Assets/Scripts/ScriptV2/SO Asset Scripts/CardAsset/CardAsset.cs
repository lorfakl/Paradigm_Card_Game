using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AbilityTypes
{
    Mandatory ,
    Optional,
    Reckless,
    Patient,
    Limited
}
public enum TargetingOptions
{
    NoTarget,
    AllCreatures, 
    EnemyCreatures,
    YourCreatures, 
    AllCharacters, 
    EnemyCharacters,
    YourCharacters,
    AllMechanisms,
    YourMechanisms,
    EnemyMechanisms,
    AllCards,
    YourCards,
    EnemyCards,
    AllElements,
    YourElements,
    EnemyElements,
    AllLandscapes,
    YourLandscapes,
    EnemyLandscapes,
    AllMajesty,
    YourMajesty,
    EnemyMajesty,
    AllPhantoms,
    YourPhantoms,
    EnemyPhantoms,
    AllPhilosophers,
    YourPhilosophers,
    EnemyPhilosophers,
    AllGraves,
    YourGraves,
    EnemyGraves,
    AllHands,
    YourHands,
    EnemyHands,
    AllDecks,
    YourDecks,
    EnemyDecks,
    AllDZ,
    YourDZ,
    EnemyDZ,
    AllBZ,
    YourBZ,
    EnemyBZ,
    AllSC,
    YourSC,
    EnemySC,
    AllLZ,
    YourLZ,
    EnemyLZ,
    AllFields,
    YourField,
    EnemyField


}

public class CardAsset : ScriptableObject 
{
    // this object will hold the info about the most general card
    [Header("General info")]
    public CharacterAsset characterAsset;  // if this is null, it`s a neutral card not in a family
    [TextArea(2,3)]
    public string Description;  // Description for spell or character
    public Sprite CardImage;
    public int ManaCost;
    public int Id;
   

    [Header("Creature Info")]
    public int MaxHealth;   // =0 => spell card
    public int Attack;
    public int AttacksForOneTurn = 1;

    public bool Charge;
    public string CreatureScriptName;
    public int specialCreatureAmount; 

    [Header("SpellInfo")]
    public string SpellScriptName;
    public int specialSpellAmount;
    public TargetingOptions Targets;
    public bool Lingering;
    public bool Imediate;
    



}
