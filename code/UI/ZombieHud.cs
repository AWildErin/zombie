using Sandbox;
using Sandbox.UI;
using Zombie.Modules.ItemModule;
using Zombie.Utils;

namespace Zombie.UI
{
	public partial class ZombieHud : HudEntity<RootPanel>
	{
		public ZombieHud()
		{
			if ( !IsClient )
				return;

			RootPanel.AddChild<ChatBox>();
		}
	}
}
