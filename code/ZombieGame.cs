using Sandbox;
using System.Collections.Generic;
using System.Linq;
using Zombie.Modules;
using Zombie.UI;

namespace Zombie
{
	[Library( "zombie" )]
	public partial class ZombieGame : Game
	{
		public static List<BaseModule> Modules = new List<BaseModule>();

		public ZombieGame()
		{
			// Init our modules first so the hud can use stuff from it.
			var allModules = from module in Library.GetAll<BaseModule>()
							 where !module.IsAbstract
							 select module;
			foreach ( var module in allModules )
			{
				var moduleClass = Library.Create<BaseModule>( module.FullName );
				Log.Info( $"Initialising: {module.FullName}" );

				if ( moduleClass.Init() )
				{
					Modules.Add( moduleClass );
				}
			}

			if ( IsServer )
			{
				new ZombieHud();
			}
		}

		public override void ClientJoined( Client client )
		{
			base.ClientJoined( client );

			var player = new ZombiePlayer();
			client.Pawn = player;

			player.Respawn();
		}
	}
}
