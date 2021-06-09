using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001665 RID: 5733
	public abstract class Blueprint : ThingWithComps, IConstructible
	{
		// Token: 0x17001336 RID: 4918
		// (get) Token: 0x06007CE3 RID: 31971 RVA: 0x00053E73 File Offset: 0x00052073
		public override string Label
		{
			get
			{
				return this.def.entityDefToBuild.label + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x17001337 RID: 4919
		// (get) Token: 0x06007CE4 RID: 31972
		protected abstract float WorkTotal { get; }

		// Token: 0x06007CE5 RID: 31973 RVA: 0x0001AE06 File Offset: 0x00019006
		public override void Draw()
		{
			if (this.def.drawerType == DrawerType.RealtimeOnly)
			{
				base.Draw();
				return;
			}
			base.Comps_PostDraw();
		}

		// Token: 0x06007CE6 RID: 31974 RVA: 0x00053E99 File Offset: 0x00052099
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Gizmo selectMonumentMarkerGizmo = QuestUtility.GetSelectMonumentMarkerGizmo(this);
			if (selectMonumentMarkerGizmo != null)
			{
				yield return selectMonumentMarkerGizmo;
			}
			yield break;
			yield break;
		}

		// Token: 0x06007CE7 RID: 31975 RVA: 0x00255434 File Offset: 0x00253634
		public virtual bool TryReplaceWithSolidThing(Pawn workerPawn, out Thing createdThing, out bool jobEnded)
		{
			jobEnded = false;
			if (GenConstruct.FirstBlockingThing(this, workerPawn) != null)
			{
				workerPawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				jobEnded = true;
				createdThing = null;
				return false;
			}
			createdThing = this.MakeSolidThing();
			Map map = base.Map;
			GenSpawn.WipeExistingThings(base.Position, base.Rotation, createdThing.def, map, DestroyMode.Deconstruct);
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			if (createdThing.def.CanHaveFaction)
			{
				createdThing.SetFactionDirect(workerPawn.Faction);
			}
			GenSpawn.Spawn(createdThing, base.Position, map, base.Rotation, WipeMode.Vanish, false);
			return true;
		}

		// Token: 0x06007CE8 RID: 31976 RVA: 0x00053EA9 File Offset: 0x000520A9
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			map.blueprintGrid.Register(this);
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x06007CE9 RID: 31977 RVA: 0x00053EBF File Offset: 0x000520BF
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			map.blueprintGrid.DeRegister(this);
		}

		// Token: 0x06007CEA RID: 31978
		protected abstract Thing MakeSolidThing();

		// Token: 0x06007CEB RID: 31979
		public abstract List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x06007CEC RID: 31980
		public abstract ThingDef EntityToBuildStuff();

		// Token: 0x06007CED RID: 31981 RVA: 0x002554D0 File Offset: 0x002536D0
		public Thing BlockingHaulableOnTop()
		{
			if (this.def.entityDefToBuild.passability == Traversability.Standable)
			{
				return null;
			}
			foreach (IntVec3 c in this.OccupiedRect())
			{
				List<Thing> thingList = c.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Thing thing = thingList[i];
					if (thing.def.EverHaulable)
					{
						return thing;
					}
				}
			}
			return null;
		}

		// Token: 0x06007CEE RID: 31982 RVA: 0x00255570 File Offset: 0x00253770
		public override ushort PathFindCostFor(Pawn p)
		{
			if (base.Faction == null)
			{
				return 0;
			}
			if (this.def.entityDefToBuild is TerrainDef)
			{
				return 0;
			}
			if ((p.Faction == base.Faction || p.HostFaction == base.Faction) && (base.Map.reservationManager.IsReservedByAnyoneOf(this, p.Faction) || (p.HostFaction != null && base.Map.reservationManager.IsReservedByAnyoneOf(this, p.HostFaction))))
			{
				return Frame.AvoidUnderConstructionPathFindCost;
			}
			return 0;
		}

		// Token: 0x06007CEF RID: 31983 RVA: 0x00255604 File Offset: 0x00253804
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.Append("WorkLeft".Translate() + ": " + this.WorkTotal.ToStringWorkAmount());
			return stringBuilder.ToString();
		}
	}
}
