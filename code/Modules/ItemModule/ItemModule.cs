using Sandbox;
using System;
using System.Collections.Generic;
using System.Linq;
using Zombie.Entities;
using Zombie.Utils;

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
				Log.Info( $"Found item: {itemClass.ItemName} ({itemClass.ItemId})" );

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

		///
		/// Console Commands
		///

		[ServerCmd( "zom_itemmodule_reloaditems" )]
		public static void ReloadItemsCmd()
		{
			// I don't like this, but for now it's okay
			var _ = new ItemModule();
			_.LoadItems();
		}

		[ServerCmd( "zom_itemmodule_createitem" )]
		public static void CreateItemCmd( string item )
		{
			var owner = ConsoleSystem.Caller.Pawn;

			if ( owner == null )
				return;

			BaseItem itemClass = ItemUtil.GetItemById( item );

			// Check to make sure we've actually returned an item.
			if ( itemClass == null )
			{
				Log.Info( $"Error: {item} could not be found." );
				return;
			}

			Log.Info( $"{itemClass}" );

			Vector3 pos = Vector3.Zero;
			Angles ang = new Angles( 0, owner.EyeRot.Angles().yaw, 0 );

			var tr = Trace.Ray( owner.EyePos, owner.EyePos + owner.EyeRot.Forward * 200 )
				.UseHitboxes()
				.Ignore( owner )
				.Size( 2 )
				.Run();

			pos = tr.EndPos;

			CreatePhysicalItem( itemClass, pos, ang );
		}

		/// <summary>
		/// Lists all the items with a search filter so we can search
		/// for a specific item.
		/// </summary>
		/// <param name="filter">The filter we will search for</param>
		[ServerCmd( "zom_itemmodule_listitems" )]
		public static void ListItemsCmd( string filter = "" )
		{
			if ( filter == "" )
				Log.Info( "Item List:" );
			else
				Log.Info( $"Item List: (Filter: {filter})" );

			foreach ( BaseItem item in ItemUtil.GetAllItems( filter ) )
			{
				Log.Info( $"Item: {item.ItemName}, Id: {item.ItemId}" );
			}
		}
	}
}
