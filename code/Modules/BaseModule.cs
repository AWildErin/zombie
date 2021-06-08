using Sandbox;
using System.Collections.Generic;

namespace Zombie.Modules
{
	public abstract class BaseModule : NetworkComponent
	{
		public static List<BaseModule> Modules = new List<BaseModule>();

		/// <summary>
		/// Called when the game is created.
		/// </summary>
		/// <returns>Returns <see langword="true"/> if the module loaded, returns <see langword="false"/> if it could not load.</returns>
		public abstract bool Init();

		/// <summary>
		/// Called whenever the module is shutdown.
		/// </summary>
		public virtual void Shutdown() { }
	}
}
