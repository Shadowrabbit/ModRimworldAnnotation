using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020012DE RID: 4830
	public class MineStrikeManager : IExposable
	{
		// Token: 0x06006875 RID: 26741 RVA: 0x000471D1 File Offset: 0x000453D1
		public void ExposeData()
		{
			Scribe_Collections.Look<StrikeRecord>(ref this.strikeRecords, "strikeRecords", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06006876 RID: 26742 RVA: 0x00203744 File Offset: 0x00201944
		public void CheckStruckOre(IntVec3 justMinedPos, ThingDef justMinedDef, Thing miner)
		{
			if (miner.Faction != Faction.OfPlayer)
			{
				return;
			}
			for (int i = 0; i < 4; i++)
			{
				IntVec3 intVec = justMinedPos + GenAdj.CardinalDirections[i];
				if (intVec.InBounds(miner.Map))
				{
					Building edifice = intVec.GetEdifice(miner.Map);
					if (edifice != null && edifice.def != justMinedDef && MineStrikeManager.MineableIsValuable(edifice.def) && !this.AlreadyVisibleNearby(intVec, miner.Map, edifice.def) && !this.RecentlyStruck(intVec, edifice.def))
					{
						StrikeRecord item = default(StrikeRecord);
						item.cell = intVec;
						item.def = edifice.def;
						item.ticksGame = Find.TickManager.TicksGame;
						this.strikeRecords.Add(item);
						Messages.Message("StruckMineable".Translate(edifice.def.label), edifice, MessageTypeDefOf.PositiveEvent, true);
						TaleRecorder.RecordTale(TaleDefOf.StruckMineable, new object[]
						{
							miner,
							edifice
						});
					}
				}
			}
		}

		// Token: 0x06006877 RID: 26743 RVA: 0x00203870 File Offset: 0x00201A70
		public bool AlreadyVisibleNearby(IntVec3 center, Map map, ThingDef mineableDef)
		{
			CellRect cellRect = CellRect.CenteredOn(center, 1);
			for (int i = 1; i < MineStrikeManager.RadialVisibleCells; i++)
			{
				IntVec3 c = center + GenRadial.RadialPattern[i];
				if (c.InBounds(map) && !c.Fogged(map) && !cellRect.Contains(c))
				{
					Building edifice = c.GetEdifice(map);
					if (edifice != null && edifice.def == mineableDef)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006878 RID: 26744 RVA: 0x002038DC File Offset: 0x00201ADC
		private bool RecentlyStruck(IntVec3 cell, ThingDef def)
		{
			for (int i = this.strikeRecords.Count - 1; i >= 0; i--)
			{
				StrikeRecord strikeRecord = this.strikeRecords[i];
				if (strikeRecord.Expired)
				{
					this.strikeRecords.RemoveAt(i);
				}
				else if (this.strikeRecords[i].def == def)
				{
					strikeRecord = this.strikeRecords[i];
					if (strikeRecord.cell.InHorDistOf(cell, 12f))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06006879 RID: 26745 RVA: 0x00203960 File Offset: 0x00201B60
		public static bool MineableIsValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing != null && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 10f;
		}

		// Token: 0x0600687A RID: 26746 RVA: 0x002039B0 File Offset: 0x00201BB0
		public static bool MineableIsVeryValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing != null && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 100f;
		}

		// Token: 0x0600687B RID: 26747 RVA: 0x00203A00 File Offset: 0x00201C00
		public string DebugStrikeRecords()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (StrikeRecord strikeRecord in this.strikeRecords)
			{
				stringBuilder.AppendLine(strikeRecord.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04004592 RID: 17810
		private List<StrikeRecord> strikeRecords = new List<StrikeRecord>();

		// Token: 0x04004593 RID: 17811
		private const int RecentStrikeIgnoreRadius = 12;

		// Token: 0x04004594 RID: 17812
		private static readonly int RadialVisibleCells = GenRadial.NumCellsInRadius(5.9f);
	}
}
