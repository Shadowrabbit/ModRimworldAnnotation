using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001081 RID: 4225
	public class Building_PlantGrower : Building, IPlantToGrowSettable
	{
		// Token: 0x17001137 RID: 4407
		// (get) Token: 0x06006490 RID: 25744 RVA: 0x0021E5A2 File Offset: 0x0021C7A2
		public IEnumerable<Plant> PlantsOnMe
		{
			get
			{
				if (!base.Spawned)
				{
					yield break;
				}
				foreach (IntVec3 c in this.OccupiedRect())
				{
					List<Thing> thingList = base.Map.thingGrid.ThingsListAt(c);
					int num;
					for (int i = 0; i < thingList.Count; i = num + 1)
					{
						Plant plant = thingList[i] as Plant;
						if (plant != null)
						{
							yield return plant;
						}
						num = i;
					}
					thingList = null;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17001138 RID: 4408
		// (get) Token: 0x06006491 RID: 25745 RVA: 0x0021E5B4 File Offset: 0x0021C7B4
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return this.OccupiedRect().Cells;
			}
		}

		// Token: 0x06006492 RID: 25746 RVA: 0x0021E5CF File Offset: 0x0021C7CF
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return PlantToGrowSettableUtility.SetPlantToGrowCommand(this);
			yield break;
			yield break;
		}

		// Token: 0x06006493 RID: 25747 RVA: 0x0021E5DF File Offset: 0x0021C7DF
		public override void PostMake()
		{
			base.PostMake();
			this.plantDefToGrow = this.def.building.defaultPlantToGrow;
		}

		// Token: 0x06006494 RID: 25748 RVA: 0x0021E5FD File Offset: 0x0021C7FD
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compPower = base.GetComp<CompPowerTrader>();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
		}

		// Token: 0x06006495 RID: 25749 RVA: 0x0021E61E File Offset: 0x0021C81E
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
		}

		// Token: 0x06006496 RID: 25750 RVA: 0x0021E638 File Offset: 0x0021C838
		public override void TickRare()
		{
			if (this.compPower != null && !this.compPower.PowerOn)
			{
				foreach (Thing thing in this.PlantsOnMe)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Rotting, 1f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true);
					thing.TakeDamage(dinfo);
				}
			}
		}

		// Token: 0x06006497 RID: 25751 RVA: 0x0021E6BC File Offset: 0x0021C8BC
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			foreach (Plant plant in this.PlantsOnMe.ToList<Plant>())
			{
				plant.Destroy(DestroyMode.Vanish);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x06006498 RID: 25752 RVA: 0x0021E71C File Offset: 0x0021C91C
		public override string GetInspectString()
		{
			string text = base.GetInspectString();
			if (base.Spawned)
			{
				if (PlantUtility.GrowthSeasonNow(base.Position, base.Map, true))
				{
					text += "\n" + "GrowSeasonHereNow".Translate();
				}
				else
				{
					text += "\n" + "CannotGrowBadSeasonTemperature".Translate();
				}
			}
			return text;
		}

		// Token: 0x06006499 RID: 25753 RVA: 0x0021E78F File Offset: 0x0021C98F
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x0600649A RID: 25754 RVA: 0x0021E797 File Offset: 0x0021C997
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x0600649B RID: 25755 RVA: 0x0021E7A0 File Offset: 0x0021C9A0
		public bool CanAcceptSowNow()
		{
			return this.compPower == null || this.compPower.PowerOn;
		}

		// Token: 0x040038A7 RID: 14503
		private ThingDef plantDefToGrow;

		// Token: 0x040038A8 RID: 14504
		private CompPowerTrader compPower;
	}
}
