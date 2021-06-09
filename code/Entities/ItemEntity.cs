using Sandbox;
using Zombie.Modules.ItemModule;

namespace Zombie.Entities
{
	[Library( "ent_item" )]
	public partial class ItemEntity : ModelEntity, IUse
	{
		[Net]
		public BaseItem Item { get; set; }

		public override void Spawn()
		{
			base.Spawn();
			SetupPhysicsFromModel( PhysicsMotionType.Dynamic, false );
		}

		// Figure out a better way to do this
		public void SetItem(BaseItem newItem)
		{
			Item = newItem;
		}

		public bool IsUsable( Entity user )
		{
			throw new System.NotImplementedException();
		}

		public bool OnUse( Entity user )
		{
			throw new System.NotImplementedException();
		}
	}
}
