using Sandbox;
using System.Collections.Generic;
using System.Linq;
using Zombie.Entities;

namespace Zombie.Modules.ItemModule
{
	public partial class ItemModule : BaseModule
	{
		public static List<BaseItem> Items = new List<BaseItem>();

		public override bool Init()
		{
			LoadItems();

			return true;
		}

		public override void Shutdown()
		{
			
		}

		/// <summary>
		/// Finds all of our item classes and adds them to the global list of items.
		/// </summary>
		private void LoadItems()
		{
			// Clear the list of previous items, just in case.
			Items.Clear();

			// Find all our item classes and ignore those listed as abstract
			var foundItems = from item in Library.GetAll<BaseItem>()
							 where !item.IsAbstract
							 select item;
			foreach ( var item in foundItems )
			{

				var itemClass = Library.Create<BaseItem>( item.FullName );
				Log.Info( $"Found item: {itemClass.ItemName}" );

				IEnumerable<BaseItem> Duplicates = Items.Where( x => x.ItemId == itemClass.ItemId );

				// Interate through the duplicates and Log them.yeah
				foreach ( var item2 in Duplicates )
				{

					// If we have the same Id we error out to make it known
					Log.Error( $"Failed to load items: Item '{itemClass.ItemName} ({itemClass.ItemId})' has the same ItemId as '{item2.ItemName} ({item2.ItemId})'" );
					return;
				}

				Items.Add( itemClass );
			}
		}

		public static void CreatePhysicalItem( BaseItem item, Vector3 pos, Angles ang )
		{
			if ( item == null )
				return;

			var ent = new ItemEntity();
			ent.SetItem( item );

			ent.Position = pos;
			ent.Rotation = Rotation.From( ang );

			// Todo: Think about moving this to the item instead;
			ent.SetModel( ent.Item.ItemModel );


			Log.Info( $"Item is: {item}, Position: {pos}, Angles: {ang}" );
		}
	}
}
