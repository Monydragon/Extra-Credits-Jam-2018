using System;
using ItemSystem;
using Sirenix.OdinInspector;
using UnityEngine;

    [Serializable]
    public struct Status
    {
        [SerializeField][Range(0,20)] private float health;


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
            Rads += item.radiation; 
        }

        [Button("Reset Status", ButtonSizes.Medium, ButtonStyle.Box)]
        public void ResetStatus()
        {
            Health = 20;
            Rads = 0;
        }
    }