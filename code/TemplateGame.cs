using Sandbox;

namespace Template
{
    [Library( "template" )]
    public partial class TemplateGame : Game
    {
        public TemplateGame()
        {
            if ( IsServer )
            {
                // Add your HUD here
            }
        }

        public override void ClientJoined( Client client )
        {
			base.ClientJoined( client );

			var player = new TemplatePlayer();
			client.Pawn = player;

			player.Respawn();
        }
    }
}