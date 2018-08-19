using System;
using ItemSystem;
using UnityEngine;

    [Serializable]
    public struct Status
    {
        [SerializeField][Range(0,20)] private float health;

        [SerializeField][Range(0,20)] private float hunger;

        [SerializeField][Range(0,20)] private float rads;

        public float Health
        {
            get { return health; }
            set
            {
                if (value > 20) { health = 20; }
                else if(value <= 0) { health = 0; }
                else { health = value;}
            }
        }

        public float Hunger
        {
            get { return hunger; }
            set
            {
                if (value > 20) { hunger = 20; }
                else if (value < 0) { health -= 1; }
                else { hunger = value; }
            }
        }

        public float Rads
        {
            get { return rads; }
            set
            {
                if (value > 20) { health -= 1; }
                else if (value <= 0) { rads = 0; }
                else { rads = value; }
            }
        }

        public void UseItem(Item item)
        {
            Health -= item.damage;
            Health += item.healing;
            Hunger += item.hunger;
            Rads += item.radiation; 
        }

        public void ResetStatus()
        {
            Health = 20;
            Hunger = 20;
            Rads = 0;
        }
    }