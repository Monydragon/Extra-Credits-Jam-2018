using UnityEngine;
using System.Collections.Generic;

namespace ItemSystem
{//#VID-ISNB
}//#VID-ISNE

namespace ItemSystem
{//#VID-2ISNB
	public enum ItemItems
	{
		None = 0,
		MoldyBread = -1315527237,
		SlightlyMoldyBread = 2051191829,
		GoodBread = -200021338,
		Carrot = 1156997932,
		Pudding = -259876393,
		Sandwich = 174344502,
		Water = 2092244667,
		RadiationPills = 1397697909,
	}
}//#VID-2ISNE

namespace ItemSystem.Database
{
    public class VIDItemListsV32 : ScriptableObject
    {
        /*Do NOT change the formatting of anything between comments starting with '#VID-'*/

        public static readonly string itemListsName = "VIDItemListsV32";

        //#VID-ICB
		public List<Item> autoItem = new List<Item>();
        //#VID-ICE

        /*Those two lists are 'parallel', one shouldn't be changed without the other*/
        /// <summary>Stores taken IDs</summary>
        [HideInInspector]
        public List<int> usedIDs = new List<int>();

        /// <summary>Stores the types of taken IDs</summary>
        [HideInInspector]
        public List<ItemType> typesOfUsedIDs = new List<ItemType>();

        [HideInInspector]
        public List<ItemSubtypeV25> subtypes = new List<ItemSubtypeV25>();
        [HideInInspector]
        public List<ItemTypeGroup> typeGroups = new List<ItemTypeGroup>();
    }
}
