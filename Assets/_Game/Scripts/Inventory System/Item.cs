using System;
using UnityEngine;

namespace ItemSystem
{
    [Serializable]
    public class Item : ItemBase
    {
        [Range(-50, 50), Header("Unique properties")]
        public int damage;

        [Range(-20, 20)]
        public int healing, radiation, hunger, price;

        [Range(1, 20)]
        public int stackCount = 1;
        [FMODUnity.EventRef]
        public string useSfx;

        public override void UpdateUniqueProperties(ItemBase i)
        {
            Item it = (Item)i;

            damage = it.damage;
            healing = it.healing;
            radiation = it.radiation;
            hunger = it.hunger;
            price = it.price;
            useSfx = it.useSfx;
            stackCount = it.stackCount;
        }
    }
}