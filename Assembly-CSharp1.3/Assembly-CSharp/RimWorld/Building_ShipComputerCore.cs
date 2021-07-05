using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001057 RID: 4183
	public class Building_ShipComputerCore : Building
	{
		// Token: 0x170010E3 RID: 4323
		// (get) Token: 0x0600631F RID: 25375 RVA: 0x002190CF File Offset: 0x002172CF
		private bool CanLaunchNow
		{
			get
			{
				return !ShipUtility.LaunchFailReasons(this).Any<string>();
			}
		}

		// Token: 0x06006320 RID: 25376 RVA: 0x002190DF File Offset: 0x002172DF
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

		// Token: 0x06006321 RID: 25377 RVA: 0x002190EF File Offset: 0x002172EF
		public void ForceLaunch()
		{
			ShipCountdown.InitiateCountdown(this);
			if (base.Spawned)
			{
				QuestUtility.SendQuestTargetSignals(base.Map.Parent.questTags, "LaunchedShip");
			}
		}

		// Token: 0x06006322 RID: 25378 RVA: 0x00219119 File Offset: 0x00217319
		private void TryLaunch()
		{
			if (this.CanLaunchNow)
			{
				this.ForceLaunch();
			}
		}
	}
}
