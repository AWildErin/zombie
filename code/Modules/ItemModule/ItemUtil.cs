using System;
using System.Collections.Generic;
using System.Linq;
using Zombie.Modules.ItemModule;

namespace Zombie.Utils
{
    public static class ItemUtil
    {
		/// <summary>
		/// Lists all the items in the gamemode or, if specified, lists all the items
		/// matching the filter.
		/// </summary>
		/// <param name="filter">Optional search filter</param>
		/// <returns>A list of the items within the filter</returns>
		public static IEnumerable<BaseItem> GetAllItems( string filter = "" )
		{
			return from item in ItemModule.Items
				   where item.ItemName.Contains( filter, StringComparison.OrdinalIgnoreCase ) || item.ItemId.Contains( filter, StringComparison.OrdinalIgnoreCase )
				   select item;
		}

		/// <summary>
		/// Finds the item we provide in the search variable.
		/// </summary>
		/// <param name="search">The item id we are searching for</param>
		/// <returns>The item class that we have found, returns null if no item was found</returns>
		public static BaseItem GetItemById( string search )
		{
			if ( !string.IsNullOrEmpty( search ) )
			{
				BaseItem foundItem = ItemModule.Items.Find( x => x.ItemId.Equals( search, StringComparison.OrdinalIgnoreCase ) );

				return foundItem;
			}

			return null;
		}
	}
}
