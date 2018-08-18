﻿using System;
using UnityEngine;

namespace ItemSystem
{
    [Serializable]
    public class Item : ItemBase
    {
        [Range(-50, 50), Header("Unique properties")]
        public int damage;
        [Range(-20, 20)]
        public int healing, radiation, hunger, thirst, price;
        public AudioClip useSfx;

        public override void UpdateUniqueProperties(ItemBase i)
        {
            Item it = (Item)i;
            
            damage = it.damage;
            healing = it.healing;
            radiation = it.radiation;
            hunger = it.hunger;
            thirst = it.thirst;
            price = it.price;
        }
    }
}