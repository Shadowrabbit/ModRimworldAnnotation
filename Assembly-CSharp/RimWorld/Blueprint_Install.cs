using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200166A RID: 5738
	public class Blueprint_Install : Blueprint
	{
		// Token: 0x1700133E RID: 4926
		// (get) Token: 0x06007D11 RID: 32017 RVA: 0x00054043 File Offset: 0x00052243
		public Thing MiniToInstallOrBuildingToReinstall
		{
			get
			{
				if (this.miniToInstall != null)
				{
					return this.miniToInstall;
				}
				if (this.buildingToReinstall != null)
				{
					return this.buildingToReinstall;
				}
				throw new InvalidOperationException("Nothing to install.");
			}
		}

		// Token: 0x1700133F RID: 4927
		// (get) Token: 0x06007D12 RID: 32018 RVA: 0x0005406D File Offset: 0x0005226D
		public Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		// Token: 0x17001340 RID: 4928
		// (get) Token: 0x06007D13 RID: 32019 RVA: 0x0005407A File Offset: 0x0005227A
		public override Graphic Graphic
		{
			get
			{
				return this.ThingToInstall.def.installBlueprintDef.graphic.ExtractInnerGraphicFor(this.ThingToInstall);
			}
		}

		// Token: 0x17001341 RID: 4929
		// (get) Token: 0x06007D14 RID: 32020 RVA: 0x0005409C File Offset: 0x0005229C
		protected override float WorkTotal
		{
			get
			{
				return 150f;
			}
		}

		// Token: 0x06007D15 RID: 32021 RVA: 0x000540A3 File Offset: 0x000522A3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MinifiedThing>(ref this.miniToInstall, "miniToInstall", false);
			Scribe_References.Look<Building>(ref this.buildingToReinstall, "buildingToReinstall", false);
		}

		// Token: 0x06007D16 RID: 32022 RVA: 0x000540CD File Offset: 0x000522CD
		public override ThingDef EntityToBuildStuff()
		{
			return this.ThingToInstall.Stuff;
		}

		// Token: 0x06007D17 RID: 32023 RVA: 0x000540DA File Offset: 0x000522DA
		public override List<ThingDefCountClass> MaterialsNeeded()
		{
			Log.Error("Called MaterialsNeeded on a Blueprint_Install.", false);
			return new List<ThingDefCountClass>();
		}

		// Token: 0x06007D18 RID: 32024 RVA: 0x000540EC File Offset: 0x000522EC
		protected override Thing MakeSolidThing()
		{
			Thing thingToInstall = this.ThingToInstall;
			if (this.miniToInstall != null)
			{
				this.miniToInstall.InnerThing = null;
				this.miniToInstall.Destroy(DestroyMode.Vanish);
			}
			return thingToInstall;
		}

		// Token: 0x06007D19 RID: 32025 RVA: 0x00255B10 File Offset: 0x00253D10
		public override bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
		{
			Map map = base.Map;
			bool flag = base.TryReplaceWithSolidThing(workerPawn, out createdThing, out jobEnded);
			if (flag)
			{
				SoundDefOf.Building_Complete.PlayOneShot(new TargetInfo(base.Position, map, false));
				workerPawn.records.Increment(RecordDefOf.ThingsInstalled);
			}
			return flag;
		}

		// Token: 0x06007D1A RID: 32026 RVA: 0x00054114 File Offset: 0x00052314
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Command command = BuildCopyCommandUtility.BuildCopyCommand(this.ThingToInstall.def, this.ThingToInstall.Stuff);
			if (command != null)
			{
				yield return command;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command command2 in BuildFacilityCommandUtility.BuildFacilityCommands(this.ThingToInstall.def))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06007D1B RID: 32027 RVA: 0x00255B5C File Offset: 0x00253D5C
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.buildingToReinstall != null)
			{
				GenDraw.DrawLineBetween(this.buildingToReinstall.TrueCenter(), this.TrueCenter());
			}
			if (this.ThingToInstall.def.drawPlaceWorkersWhileInstallBlueprintSelected && this.ThingToInstall.def.PlaceWorkers != null)
			{
				List<PlaceWorker> placeWorkers = this.ThingToInstall.def.PlaceWorkers;
				for (int i = 0; i < placeWorkers.Count; i++)
				{
					placeWorkers[i].DrawGhost(this.ThingToInstall.def, base.Position, base.Rotation, Color.white, this.ThingToInstall);
				}
			}
		}

		// Token: 0x06007D1C RID: 32028 RVA: 0x00054124 File Offset: 0x00052324
		internal void SetThingToInstallFromMinified(MinifiedThing itemToInstall)
		{
			this.miniToInstall = itemToInstall;
			this.buildingToReinstall = null;
		}

		// Token: 0x06007D1D RID: 32029 RVA: 0x00054134 File Offset: 0x00052334
		internal void SetBuildingToReinstall(Building buildingToReinstall)
		{
			if (!buildingToReinstall.def.Minifiable)
			{
				Log.Error("Tried to reinstall non-minifiable building.", false);
				return;
			}
			this.miniToInstall = null;
			this.buildingToReinstall = buildingToReinstall;
		}

		// Token: 0x040051A1 RID: 20897
		private MinifiedThing miniToInstall;

		// Token: 0x040051A2 RID: 20898
		private Building buildingToReinstall;
	}
}
