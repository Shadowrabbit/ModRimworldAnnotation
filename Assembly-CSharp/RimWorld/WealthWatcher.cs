using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001334 RID: 4916
	public class WealthWatcher
	{
		// Token: 0x17001066 RID: 4198
		// (get) Token: 0x06006AA2 RID: 27298 RVA: 0x0004883C File Offset: 0x00046A3C
		public int HealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.totalHealth;
			}
		}

		// Token: 0x17001067 RID: 4199
		// (get) Token: 0x06006AA3 RID: 27299 RVA: 0x0004884A File Offset: 0x00046A4A
		public float WealthTotal
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems + this.wealthBuildings + this.wealthPawns;
			}
		}

		// Token: 0x17001068 RID: 4200
		// (get) Token: 0x06006AA4 RID: 27300 RVA: 0x00048866 File Offset: 0x00046A66
		public float WealthItems
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthItems;
			}
		}

		// Token: 0x17001069 RID: 4201
		// (get) Token: 0x06006AA5 RID: 27301 RVA: 0x00048874 File Offset: 0x00046A74
		public float WealthBuildings
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthBuildings;
			}
		}

		// Token: 0x1700106A RID: 4202
		// (get) Token: 0x06006AA6 RID: 27302 RVA: 0x00048882 File Offset: 0x00046A82
		public float WealthFloorsOnly
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthFloorsOnly;
			}
		}

		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x06006AA7 RID: 27303 RVA: 0x00048890 File Offset: 0x00046A90
		public float WealthPawns
		{
			get
			{
				this.RecountIfNeeded();
				return this.wealthPawns;
			}
		}

		// Token: 0x06006AA8 RID: 27304 RVA: 0x0020EF14 File Offset: 0x0020D114
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

		// Token: 0x06006AA9 RID: 27305 RVA: 0x0004889E File Offset: 0x00046A9E
		public WealthWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x06006AAA RID: 27306 RVA: 0x000488C3 File Offset: 0x00046AC3
		private void RecountIfNeeded()
		{
			if ((float)Find.TickManager.TicksGame - this.lastCountTick > 5000f)
			{
				this.ForceRecount(false);
			}
		}

		// Token: 0x06006AAB RID: 27307 RVA: 0x0020EF90 File Offset: 0x0020D190
		public void ForceRecount(bool allowDuringInit = false)
		{
			if (!allowDuringInit && Current.ProgramState != ProgramState.Playing)
			{
				Log.Error("WealthWatcher recount in game mode " + Current.ProgramState, false);
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

		// Token: 0x06006AAC RID: 27308 RVA: 0x0020F12C File Offset: 0x0020D32C
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

		// Token: 0x06006AAD RID: 27309 RVA: 0x0020F218 File Offset: 0x0020D418
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

		// Token: 0x06006AAE RID: 27310 RVA: 0x0020F2E8 File Offset: 0x0020D4E8
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

		// Token: 0x040046F0 RID: 18160
		private Map map;

		// Token: 0x040046F1 RID: 18161
		private float wealthItems;

		// Token: 0x040046F2 RID: 18162
		private float wealthBuildings;

		// Token: 0x040046F3 RID: 18163
		private float wealthPawns;

		// Token: 0x040046F4 RID: 18164
		private float wealthFloorsOnly;

		// Token: 0x040046F5 RID: 18165
		private int totalHealth;

		// Token: 0x040046F6 RID: 18166
		private float lastCountTick = -99999f;

		// Token: 0x040046F7 RID: 18167
		private static float[] cachedTerrainMarketValue;

		// Token: 0x040046F8 RID: 18168
		private const int MinCountInterval = 5000;

		// Token: 0x040046F9 RID: 18169
		private List<Thing> tmpThings = new List<Thing>();
	}
}
