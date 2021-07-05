using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CFF RID: 3327
	public class WealthWatcher
	{
		// Token: 0x17000D63 RID: 3427
		// (get) Token: 0x06004DBC RID: 19900 RVA: 0x001A13EA File Offset: 0x0019F5EA
		public int HealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.totalHealth;
			}
		}

		// Token: 0x17000D64 RID: 3428
		// (get) Token: 0x06004DBD RID: 19901 RVA: 0x001A13F8 File Offset: 0x0019F5F8
		public float WealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems + this.wealthBuildings + this.wealthPawns;
			}
		}

		// Token: 0x17000D65 RID: 3429
		// (get) Token: 0x06004DBE RID: 19902 RVA: 0x001A1414 File Offset: 0x0019F614
		public float WealthItems
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems;
			}
		}

		// Token: 0x17000D66 RID: 3430
		// (get) Token: 0x06004DBF RID: 19903 RVA: 0x001A1422 File Offset: 0x0019F622
		public float WealthBuildings
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthBuildings;
			}
		}

		// Token: 0x17000D67 RID: 3431
		// (get) Token: 0x06004DC0 RID: 19904 RVA: 0x001A1430 File Offset: 0x0019F630
		public float WealthFloorsOnly
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthFloorsOnly;
			}
		}

		// Token: 0x17000D68 RID: 3432
		// (get) Token: 0x06004DC1 RID: 19905 RVA: 0x001A143E File Offset: 0x0019F63E
		public float WealthPawns
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthPawns;
			}
		}

		// Token: 0x06004DC2 RID: 19906 RVA: 0x001A144C File Offset: 0x0019F64C
		public static void ResetStaticData()
		{
			int num = -1;
			List<TerrainDef> allDefsListForReading = DefDatabase<TerrainDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				num = Mathf.Max(num, (int)allDefsListForReading[i].index);
			}
			WealthWatcher.cachedTerrainMarketValue = new float[num + 1];
			for (int j = 0; j < allDefsListForReading.Count; j++)
			{
				WealthWatcher.cachedTerrainMarketValue[(int)allDefsListForReading[j].index] = allDefsListForReading[j].GetStatValueAbstract(StatDefOf.MarketValue, null);
			}
		}

		// Token: 0x06004DC3 RID: 19907 RVA: 0x001A14C7 File Offset: 0x0019F6C7
		public WealthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004DC4 RID: 19908 RVA: 0x001A14EC File Offset: 0x0019F6EC
		private void RecountIfNeeded()
		{
			if ((float)Find.TickManager.TicksGame - this.lastCountTick > 5000f)
			{
				this.ForceRecount(false);
			}
		}

		// Token: 0x06004DC5 RID: 19909 RVA: 0x001A1510 File Offset: 0x0019F710
		public void ForceRecount(bool allowDuringInit = false)
		{
			if (!allowDuringInit && Current.ProgramState != ProgramState.Playing)
			{
				Log.Error("WealthWatcher recount in game mode " + Current.ProgramState);
				return;
			}
			this.wealthItems = this.CalculateWealthItems();
			this.wealthBuildings = 0f;
			this.wealthPawns = 0f;
			this.wealthFloorsOnly = 0f;
			this.totalHealth = 0;
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial);
			for (int i = 0; i < list.Count; i++)
			{
				Thing thing = list[i];
				if (thing.Faction == Faction.OfPlayer)
				{
					this.wealthBuildings += thing.GetStatValue(StatDefOf.MarketValueIgnoreHp, true);
					this.totalHealth += thing.HitPoints;
				}
			}
			this.wealthFloorsOnly = this.CalculateWealthFloors();
			this.wealthBuildings += this.wealthFloorsOnly;
			foreach (Pawn pawn in this.map.mapPawns.PawnsInFaction(Faction.OfPlayer))
			{
				if (!pawn.IsQuestLodger())
				{
					this.wealthPawns += pawn.MarketValue;
					if (pawn.IsFreeColonist)
					{
						this.totalHealth += Mathf.RoundToInt(pawn.health.summaryHealth.SummaryHealthPercent * 100f);
					}
				}
			}
			this.lastCountTick = (float)Find.TickManager.TicksGame;
		}

		// Token: 0x06004DC6 RID: 19910 RVA: 0x001A16AC File Offset: 0x0019F8AC
		public static float GetEquipmentApparelAndInventoryWealth(Pawn p)
		{
			float num = 0f;
			if (p.equipment != null)
			{
				List<ThingWithComps> allEquipmentListForReading = p.equipment.AllEquipmentListForReading;
				for (int i = 0; i < allEquipmentListForReading.Count; i++)
				{
					num += allEquipmentListForReading[i].MarketValue * (float)allEquipmentListForReading[i].stackCount;
				}
			}
			if (p.apparel != null)
			{
				List<Apparel> wornApparel = p.apparel.WornApparel;
				for (int j = 0; j < wornApparel.Count; j++)
				{
					num += wornApparel[j].MarketValue * (float)wornApparel[j].stackCount;
				}
			}
			if (p.inventory != null)
			{
				ThingOwner<Thing> innerContainer = p.inventory.innerContainer;
				for (int k = 0; k < innerContainer.Count; k++)
				{
					num += innerContainer[k].MarketValue * (float)innerContainer[k].stackCount;
				}
			}
			return num;
		}

		// Token: 0x06004DC7 RID: 19911 RVA: 0x001A1798 File Offset: 0x0019F998
		private float CalculateWealthItems()
		{
			this.tmpThings.Clear();
			ThingOwnerUtility.GetAllThingsRecursively<Thing>(this.map, ThingRequest.ForGroup(ThingRequestGroup.HaulableEver), this.tmpThings, false, delegate(IThingHolder x)
			{
				if (x is PassingShip || x is MapComponent)
				{
					return false;
				}
				Pawn pawn = x as Pawn;
				return (pawn == null || pawn.Faction == Faction.OfPlayer) && (pawn == null || !pawn.IsQuestLodger());
			}, true);
			float num = 0f;
			for (int i = 0; i < this.tmpThings.Count; i++)
			{
				if (this.tmpThings[i].SpawnedOrAnyParentSpawned && !this.tmpThings[i].PositionHeld.Fogged(this.map))
				{
					num += this.tmpThings[i].MarketValue * (float)this.tmpThings[i].stackCount;
				}
			}
			this.tmpThings.Clear();
			return num;
		}

		// Token: 0x06004DC8 RID: 19912 RVA: 0x001A1868 File Offset: 0x0019FA68
		private float CalculateWealthFloors()
		{
			TerrainDef[] topGrid = this.map.terrainGrid.topGrid;
			bool[] fogGrid = this.map.fogGrid.fogGrid;
			IntVec3 size = this.map.Size;
			float num = 0f;
			int i = 0;
			int num2 = size.x * size.z;
			while (i < num2)
			{
				if (!fogGrid[i])
				{
					num += WealthWatcher.cachedTerrainMarketValue[(int)topGrid[i].index];
				}
				i++;
			}
			return num;
		}

		// Token: 0x04002EE6 RID: 12006
		private Map map;

		// Token: 0x04002EE7 RID: 12007
		private float wealthItems;

		// Token: 0x04002EE8 RID: 12008
		private float wealthBuildings;

		// Token: 0x04002EE9 RID: 12009
		private float wealthPawns;

		// Token: 0x04002EEA RID: 12010
		private float wealthFloorsOnly;

		// Token: 0x04002EEB RID: 12011
		private int totalHealth;

		// Token: 0x04002EEC RID: 12012
		private float lastCountTick = -99999f;

		// Token: 0x04002EED RID: 12013
		private static float[] cachedTerrainMarketValue;

		// Token: 0x04002EEE RID: 12014
		private const int MinCountInterval = 5000;

		// Token: 0x04002EEF RID: 12015
		private List<Thing> tmpThings = new List<Thing>();
	}
}
