using System;
using System.Linq;
using Zombie.Modules;

namespace Zombie.Utils
{
	public static class ModuleUtils
	{
		/// <summary>
		/// Check to see if the module is currently loaded. Allows you to check if the current module is loaded inside the gamemode.
		/// </summary>
		/// <param name="t">The full name of the type, example: Zombie.Modules.ItemSystem</param>
		/// <returns>Returns <see langword="true"/> if the module loaded, returns <see langword="false"/> if it is not loaded.</returns>
		public static bool IsModuleLoaded( string t )
		{
			return ZombieGame.Modules.Any( x => x.GetType().FullName == t );
		}
	}
}
