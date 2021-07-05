using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001042 RID: 4162
	public class Blueprint_Install : Blueprint
	{
		// Token: 0x170010B8 RID: 4280
		// (get) Token: 0x0600625C RID: 25180 RVA: 0x002159F7 File Offset: 0x00213BF7
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

		// Token: 0x170010B9 RID: 4281
		// (get) Token: 0x0600625D RID: 25181 RVA: 0x00215A21 File Offset: 0x00213C21
		public Thing ThingToInstall
		{
			get
			{
				return this.MiniToInstallOrBuildingToReinstall.GetInnerIfMinified();
			}
		}

		// Token: 0x170010BA RID: 4282
		// (get) Token: 0x0600625E RID: 25182 RVA: 0x00215A30 File Offset: 0x00213C30
		public override Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					Graphic graphic = this.ThingToInstall.def.installBlueprintDef.graphic;
					this.cachedGraphic = this.ThingToInstall.Graphic.ExtractInnerGraphicFor(this.ThingToInstall).GetColoredVersion(graphic.Shader, graphic.Color, graphic.ColorTwo);
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x0600625F RID: 25183 RVA: 0x00215A94 File Offset: 0x00213C94
		protected override float WorkTotal
		{
			get
			{
				return 150f;
			}
		}

		// Token: 0x06006260 RID: 25184 RVA: 0x00215A9B File Offset: 0x00213C9B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MinifiedThing>(ref this.miniToInstall, "miniToInstall", false);
			Scribe_References.Look<Building>(ref this.buildingToReinstall, "buildingToReinstall", false);
		}

		// Token: 0x06006261 RID: 25185 RVA: 0x00215AC5 File Offset: 0x00213CC5
		public override ThingDef EntityToBuildStuff()
		{
			return this.ThingToInstall.Stuff;
		}

		// Token: 0x06006262 RID: 25186 RVA: 0x00215AD2 File Offset: 0x00213CD2
		public override List<ThingDefCountClass> MaterialsNeeded()
		{
			Log.Error("Called MaterialsNeeded on a Blueprint_Install.");
			return new List<ThingDefCountClass>();
		}

		// Token: 0x06006263 RID: 25187 RVA: 0x00215AE3 File Offset: 0x00213CE3
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			map.listerBuildings.RegisterInstallBlueprint(this);
		}

		// Token: 0x06006264 RID: 25188 RVA: 0x00215AF9 File Offset: 0x00213CF9
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			base.Map.listerBuildings.DeregisterInstallBlueprint(this);
			base.DeSpawn(mode);
		}

		// Token: 0x06006265 RID: 25189 RVA: 0x00215B13 File Offset: 0x00213D13
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

		// Token: 0x06006266 RID: 25190 RVA: 0x00215B3C File Offset: 0x00213D3C
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

		// Token: 0x06006267 RID: 25191 RVA: 0x00215B88 File Offset: 0x00213D88
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
				foreach (Command command2 in BuildRelatedCommandUtility.RelatedBuildCommands(this.ThingToInstall.def))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006268 RID: 25192 RVA: 0x00215B98 File Offset: 0x00213D98
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

		// Token: 0x06006269 RID: 25193 RVA: 0x00215C3D File Offset: 0x00213E3D
		internal void SetThingToInstallFromMinified(MinifiedThing itemToInstall)
		{
			this.miniToInstall = itemToInstall;
			this.buildingToReinstall = null;
		}

		// Token: 0x0600626A RID: 25194 RVA: 0x00215C4D File Offset: 0x00213E4D
		internal void SetBuildingToReinstall(Building buildingToReinstall)
		{
			if (!buildingToReinstall.def.Minifiable)
			{
				Log.Error("Tried to reinstall non-minifiable building.");
				return;
			}
			this.miniToInstall = null;
			this.buildingToReinstall = buildingToReinstall;
		}

		// Token: 0x040037DE RID: 14302
		private MinifiedThing miniToInstall;

		// Token: 0x040037DF RID: 14303
		private Building buildingToReinstall;

		// Token: 0x040037E0 RID: 14304
		[Unsaved(false)]
		private Graphic cachedGraphic;
	}
}
