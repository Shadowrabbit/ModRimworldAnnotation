using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020016CA RID: 5834
	public class Building_PlantGrower : Building, IPlantToGrowSettable
	{
		// Token: 0x170013E2 RID: 5090
		// (get) Token: 0x0600800D RID: 32781 RVA: 0x00055FC4 File Offset: 0x000541C4
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

		// Token: 0x170013E3 RID: 5091
		// (get) Token: 0x0600800E RID: 32782 RVA: 0x0025EF00 File Offset: 0x0025D100
		IEnumerable<IntVec3> IPlantToGrowSettable.Cells
		{
			get
			{
				return this.OccupiedRect().Cells;
			}
		}

		// Token: 0x0600800F RID: 32783 RVA: 0x00055FD4 File Offset: 0x000541D4
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

		// Token: 0x06008010 RID: 32784 RVA: 0x00055FE4 File Offset: 0x000541E4
		public override void PostMake()
		{
			base.PostMake();
			this.plantDefToGrow = this.def.building.defaultPlantToGrow;
		}

		// Token: 0x06008011 RID: 32785 RVA: 0x00056002 File Offset: 0x00054202
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.compPower = base.GetComp<CompPowerTrader>();
			PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.GrowingFood, KnowledgeAmount.Total);
		}

		// Token: 0x06008012 RID: 32786 RVA: 0x00056023 File Offset: 0x00054223
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.plantDefToGrow, "plantDefToGrow");
		}

		// Token: 0x06008013 RID: 32787 RVA: 0x0025EF1C File Offset: 0x0025D11C
		public override void TickRare()
		{
			if (this.compPower != null && !this.compPower.PowerOn)
			{
				foreach (Thing thing in this.PlantsOnMe)
				{
					DamageInfo dinfo = new DamageInfo(DamageDefOf.Rotting, 1f, 0f, -1f, null, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null);
					thing.TakeDamage(dinfo);
				}
			}
		}

		// Token: 0x06008014 RID: 32788 RVA: 0x0025EFA0 File Offset: 0x0025D1A0
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			foreach (Plant plant in this.PlantsOnMe.ToList<Plant>())
			{
				plant.Destroy(DestroyMode.Vanish);
			}
			base.DeSpawn(mode);
		}

		// Token: 0x06008015 RID: 32789 RVA: 0x0025F000 File Offset: 0x0025D200
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

		// Token: 0x06008016 RID: 32790 RVA: 0x0005603B File Offset: 0x0005423B
		public ThingDef GetPlantDefToGrow()
		{
			return this.plantDefToGrow;
		}

		// Token: 0x06008017 RID: 32791 RVA: 0x00056043 File Offset: 0x00054243
		public void SetPlantDefToGrow(ThingDef plantDef)
		{
			this.plantDefToGrow = plantDef;
		}

		// Token: 0x06008018 RID: 32792 RVA: 0x0005604C File Offset: 0x0005424C
		public bool CanAcceptSowNow()
		{
			return this.compPower == null || this.compPower.PowerOn;
		}

		// Token: 0x040052FE RID: 21246
		private ThingDef plantDefToGrow;

		// Token: 0x040052FF RID: 21247
		private CompPowerTrader compPower;
	}
}
