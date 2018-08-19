using System;
using ItemSystem;
using UnityEngine;

public partial class PlayerControl
{
    [Serializable]
    public struct Status
    {
        [Range(0,20)]
        public float Health;

        [Range(0,20)]
        public float Hunger;

        [Range(0,20)]
        public float Rads;

        public void UseItem(Item item)
        {
            Health += Mathf.Clamp(item.healing, 0f, 20f);
            Hunger += Mathf.Clamp(item.hunger, 0f, 20f);
            Rads += Mathf.Clamp(item.radiation, 0f, 20f);
        }

        public void ResetStatus()
        {
            Health = 20;
            Hunger = 20;
            Rads = 0;
        }
    }

}