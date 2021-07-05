using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200103F RID: 4159
	public abstract class Blueprint : ThingWithComps, IConstructible
	{
		// Token: 0x170010B2 RID: 4274
		// (get) Token: 0x06006240 RID: 25152 RVA: 0x002154C8 File Offset: 0x002136C8
		public override string Label
		{
			get
			{
				return this.def.entityDefToBuild.label + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x170010B3 RID: 4275
		// (get) Token: 0x06006241 RID: 25153
		protected abstract float WorkTotal { get; }

		// Token: 0x06006242 RID: 25154 RVA: 0x002154EE File Offset: 0x002136EE
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

		// Token: 0x06006243 RID: 25155 RVA: 0x00215500 File Offset: 0x00213700
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

		// Token: 0x06006244 RID: 25156 RVA: 0x0021559A File Offset: 0x0021379A
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			map.blueprintGrid.Register(this);
			base.SpawnSetup(map, respawningAfterLoad);
		}

		// Token: 0x06006245 RID: 25157 RVA: 0x002155B0 File Offset: 0x002137B0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			map.blueprintGrid.DeRegister(this);
		}

		// Token: 0x06006246 RID: 25158
		protected abstract Thing MakeSolidThing();

		// Token: 0x06006247 RID: 25159
		public abstract List<ThingDefCountClass> MaterialsNeeded();

		// Token: 0x06006248 RID: 25160
		public abstract ThingDef EntityToBuildStuff();

		// Token: 0x06006249 RID: 25161 RVA: 0x002155CC File Offset: 0x002137CC
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

		// Token: 0x0600624A RID: 25162 RVA: 0x0021566C File Offset: 0x0021386C
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

		// Token: 0x0600624B RID: 25163 RVA: 0x00215700 File Offset: 0x00213900
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			stringBuilder.AppendLineIfNotEmpty();
			stringBuilder.Append("WorkLeft".Translate() + ": " + this.WorkTotal.ToStringWorkAmount());
			return stringBuilder.ToString();
		}
	}
}
