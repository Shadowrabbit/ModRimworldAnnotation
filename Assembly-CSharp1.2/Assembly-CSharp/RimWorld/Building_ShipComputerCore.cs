using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001686 RID: 5766
	public class Building_ShipComputerCore : Building
	{
		// Token: 0x17001371 RID: 4977
		// (get) Token: 0x06007DFD RID: 32253 RVA: 0x00054B69 File Offset: 0x00052D69
		private bool CanLaunchNow
		{
			get
			{
				return !ShipUtility.LaunchFailReasons(this).Any<string>();
			}
		}

		// Token: 0x06007DFE RID: 32254 RVA: 0x00054B79 File Offset: 0x00052D79
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in ShipUtility.ShipStartupGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			Command_Action command_Action = new Command_Action();
			command_Action.action = new Action(this.TryLaunch);
			command_Action.defaultLabel = "CommandShipLaunch".Translate();
			command_Action.defaultDesc = "CommandShipLaunchDesc".Translate();
			if (!this.CanLaunchNow)
			{
				command_Action.Disable(ShipUtility.LaunchFailReasons(this).First<string>());
			}
			if (ShipCountdown.CountingDown)
			{
				command_Action.Disable(null);
			}
			command_Action.hotKey = KeyBindingDefOf.Misc1;
			command_Action.icon = ContentFinder<Texture2D>.Get("UI/Commands/LaunchShip", true);
			yield return command_Action;
			yield break;
			yield break;
		}

		// Token: 0x06007DFF RID: 32255 RVA: 0x00054B89 File Offset: 0x00052D89
		public void ForceLaunch()
		{
			ShipCountdown.InitiateCountdown(this);
			if (base.Spawned)
			{
				QuestUtility.SendQuestTargetSignals(base.Map.Parent.questTags, "LaunchedShip");
			}
		}

		// Token: 0x06007E00 RID: 32256 RVA: 0x00054BB3 File Offset: 0x00052DB3
		private void TryLaunch()
		{
			if (this.CanLaunchNow)
			{
				this.ForceLaunch();
			}
		}
	}
}
