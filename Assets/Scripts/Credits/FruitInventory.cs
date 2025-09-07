using System;
using System.Collections.Generic;
using UnityEngine;

public class FruitInventory : SingletonManager<FruitInventory>
{
    FruitInventory()
    {
        EnablePersistence = false;
    }

    [Header("Settings Fruits")]
    [SerializeField] private List<FruitSettings> fruitSettings = new();
}

[Serializable]
public class FruitSettings
{
    public string nameFruit;
    public int valueFruit;
    public FruitType fruitType;
}

public enum FruitType
{
    none = 0,
    Apple = 1,
    Banana = 2,
    Cherries = 3,
    Melon = 4,
    Pineapple = 5,
}
