using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200188B RID: 6283
	[StaticConstructorOnStartup]
	public class Command_LoadToTransporter : Command
	{
		// Token: 0x06008B8A RID: 35722 RVA: 0x0028A268 File Offset: 0x00288468
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			if (this.transporters == null)
			{
				this.transporters = new List<CompTransporter>();
			}
			if (!this.transporters.Contains(this.transComp))
			{
				this.transporters.Add(this.transComp);
			}
			CompLaunchable launchable = this.transComp.Launchable;
			if (launchable != null)
			{
				Building fuelingPortSource = launchable.FuelingPortSource;
				if (fuelingPortSource != null)
				{
					Map map = this.transComp.Map;
					Command_LoadToTransporter.tmpFuelingPortGivers.Clear();
					map.floodFiller.FloodFill(fuelingPortSource.Position, (IntVec3 x) => FuelingPortUtility.AnyFuelingPortGiverAt(x, map), delegate(IntVec3 x)
					{
						Command_LoadToTransporter.tmpFuelingPortGivers.Add(FuelingPortUtility.FuelingPortGiverAt(x, map));
					}, int.MaxValue, false, null);
					for (int i = 0; i < this.transporters.Count; i++)
					{
						Building fuelingPortSource2 = this.transporters[i].Launchable.FuelingPortSource;
						if (fuelingPortSource2 != null && !Command_LoadToTransporter.tmpFuelingPortGivers.Contains(fuelingPortSource2))
						{
							Messages.Message("MessageTransportersNotAdjacent".Translate(), fuelingPortSource2, MessageTypeDefOf.RejectInput, false);
							return;
						}
					}
				}
			}
			for (int j = 0; j < this.transporters.Count; j++)
			{
				if (this.transporters[j] != this.transComp && !this.transComp.Map.reachability.CanReach(this.transComp.parent.Position, this.transporters[j].parent, PathEndMode.Touch, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false)))
				{
					Messages.Message("MessageTransporterUnreachable".Translate(), this.transporters[j].parent, MessageTypeDefOf.RejectInput, false);
					return;
				}
			}
			Dialog_LoadTransporters dialog_LoadTransporters = new Dialog_LoadTransporters(this.transComp.Map, this.transporters);
			dialog_LoadTransporters.autoLoot = (this.transComp.Shuttle != null && this.transComp.Shuttle.CanAutoLoot);
			Find.WindowStack.Add(dialog_LoadTransporters);
		}

		// Token: 0x06008B8B RID: 35723 RVA: 0x0028A488 File Offset: 0x00288688
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			if (this.transComp.Props.max1PerGroup)
			{
				return false;
			}
			Command_LoadToTransporter command_LoadToTransporter = (Command_LoadToTransporter)other;
			if (command_LoadToTransporter.transComp.parent.def != this.transComp.parent.def)
			{
				return false;
			}
			if (this.transporters == null)
			{
				this.transporters = new List<CompTransporter>();
			}
			this.transporters.Add(command_LoadToTransporter.transComp);
			return false;
		}

		// Token: 0x0400596E RID: 22894
		public CompTransporter transComp;

		// Token: 0x0400596F RID: 22895
		private List<CompTransporter> transporters;

		// Token: 0x04005970 RID: 22896
		private static HashSet<Building> tmpFuelingPortGivers = new HashSet<Building>();
	}
}
