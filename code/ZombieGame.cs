using Sandbox;

namespace Zombie
{
	[Library( "zombie" )]
	public partial class ZombieGame : Game
	{
		public ZombieGame()
		{
			if ( IsServer )
			{
				// Add your HUD here
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
