using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CC6 RID: 3270
	public class MineStrikeManager : IExposable
	{
		// Token: 0x06004C10 RID: 19472 RVA: 0x00195D61 File Offset: 0x00193F61
		public void ExposeData()
		{
			Scribe_Collections.Look<StrikeRecord>(ref this.strikeRecords, "strikeRecords", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06004C11 RID: 19473 RVA: 0x00195D7C File Offset: 0x00193F7C
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

		// Token: 0x06004C12 RID: 19474 RVA: 0x00195EA8 File Offset: 0x001940A8
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

		// Token: 0x06004C13 RID: 19475 RVA: 0x00195F14 File Offset: 0x00194114
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

		// Token: 0x06004C14 RID: 19476 RVA: 0x00195F98 File Offset: 0x00194198
		public static bool MineableIsValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing != null && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 10f;
		}

		// Token: 0x06004C15 RID: 19477 RVA: 0x00195FE8 File Offset: 0x001941E8
		public static bool MineableIsVeryValuable(ThingDef mineableDef)
		{
			return mineableDef.mineable && mineableDef.building.mineableThing != null && mineableDef.building.mineableThing.GetStatValueAbstract(StatDefOf.MarketValue, null) * (float)mineableDef.building.mineableYield > 100f;
		}

		// Token: 0x06004C16 RID: 19478 RVA: 0x00196038 File Offset: 0x00194238
		public string DebugStrikeRecords()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (StrikeRecord strikeRecord in this.strikeRecords)
			{
				stringBuilder.AppendLine(strikeRecord.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04002E0D RID: 11789
		private List<StrikeRecord> strikeRecords = new List<StrikeRecord>();

		// Token: 0x04002E0E RID: 11790
		private const int RecentStrikeIgnoreRadius = 12;

		// Token: 0x04002E0F RID: 11791
		private static readonly int RadialVisibleCells = GenRadial.NumCellsInRadius(5.9f);
	}
}
