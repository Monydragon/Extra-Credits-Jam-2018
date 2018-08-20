using System;
using ItemSystem;
using Sirenix.OdinInspector;
using UnityEngine;

    [Serializable]
    public struct Status
    {
        [SerializeField][Range(0,100)] private float health, rads;

        public float Health
        {
            get { return health; }
            set
            {
                if (value > 100) { health = 100; }
                else if(value <= 0) { health = 0; }
                else { health = value;}
            }
        }

        public float Rads
        {
            get { return rads; }
            set
            {
                if (value > 50) { health -= 1; rads = value;}
                else if (value <= 0) { rads = 0; }
                else { rads = value; }
            }
        }

        public void UseItem(Item item)
        {
            Health -= item.damage;
            Health += item.healing;
            Rads += item.radiation; 
        }

        public void ResetStatus()
        {
            Health = 100;
            Rads = 0;
        }
    }