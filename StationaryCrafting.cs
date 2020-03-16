using System.Collections.Generic;

namespace Oxide.Plugins
{
	[Info("StationaryCrafting", "NubbbZ", "1.0.0")]
	[Description("Craft only when standing next to a workbench!")]
	class StationaryCrafting : CovalencePlugin
	{
		#region Variables
		private bool InWorkbenchRadius = false;
		#endregion

		#region Setup
		protected override void LoadDefaultConfig()
		{
			LogWarning("Creating a new configuration file");
			Config["ShowMessages"] = false;
		}

		protected override void LoadDefaultMessages()
		{
			lang.RegisterMessages(new Dictionary<string, string>
			{
				["inofrange"] = "Stay in the workbench radius, or your crafting queue will be canceled!",
				["outofrange"] = "You cant craft if you arent in workbench radius!",
				["canceled"] = "You have left the workbench range so your crafting queue was canceled!"
			}, this);
		}
		#endregion

		#region Hooks
		private void OnEntityEnter(TriggerWorkbench triggerWorkbench, BasePlayer player)
		{
			InWorkbenchRadius = true;
			if ((bool)Config["ShowMessages"] == true)
			{
				player.IPlayer.Reply(lang.GetMessage("inofrange", this, player.IPlayer.Id));
			}
		}

		bool CanCraft(ItemCrafter itemCrafter, ItemBlueprint bp, int amount)
		{
			BasePlayer player = itemCrafter.GetComponent<BasePlayer>();

			if (InWorkbenchRadius == false)
			{
				if ((bool)Config["ShowMessages"] == true)
				{
					player.IPlayer.Reply(lang.GetMessage("outofrange", this, player.IPlayer.Id));
				}
				return false;
			}
			return true;
		}

		private void OnEntityLeave(TriggerWorkbench triggerWorkbench, BasePlayer player)
		{
			InWorkbenchRadius = false;
			if ((bool)Config["ShowMessages"] == true)
			{
				player.IPlayer.Reply(lang.GetMessage("canceled", this, player.IPlayer.Id));
			}
			player.inventory.crafting.CancelAll(true);
		}
		#endregion
	}
}
