using Sandbox;
using System.Collections.Generic;

namespace Zombie.Modules.ItemModule
{
	public abstract class BaseItem : NetworkComponent
	{
		public abstract string ItemId { get; }

		public virtual string ItemName => "BaseItem";
		public virtual string ItemModel => "models/dev/error.vmdl";
		public virtual string ItemDescription => "This item has no description, contact a programmer.";

		// Weight in kilograms
		public float Weight { get; set; } = 1f;
		public int Stack { get; set; }

		public virtual Dictionary<string, ItemFunction> ItemFunctions { get; set; }
	}
}
