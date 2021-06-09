using System;

namespace Zombie.Modules.ItemModule
{
	public class ItemFunction
	{
		public string FunctionName { get; set; } = "";
		public string FunctionTip { get; set; } = "";
		public string FunctionIcon { get; set; } = "";

		public Action FunctionAction { get; set; }

	}
}
