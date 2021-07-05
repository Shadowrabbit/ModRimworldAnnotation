using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CFE RID: 3326
	public class VoluntarilyJoinableLordsStarter : IExposable
	{
		// Token: 0x06004DB4 RID: 19892 RVA: 0x001A1201 File Offset: 0x0019F401
		public VoluntarilyJoinableLordsStarter(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004DB5 RID: 19893 RVA: 0x001A121B File Offset: 0x0019F41B
		public bool TryStartMarriageCeremony(Pawn firstFiance, Pawn secondFiance)
		{
			if (!GatheringDefOf.MarriageCeremony.CanExecute(firstFiance.Map, firstFiance, true))
			{
				return false;
			}
			GatheringDefOf.MarriageCeremony.Worker.TryExecute(firstFiance.Map, firstFiance);
			this.lastLordStartTick = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x06004DB6 RID: 19894 RVA: 0x001A125C File Offset: 0x0019F45C
		public bool TryStartRandomGathering(bool forceStart = false)
		{
			VoluntarilyJoinableLordsStarter.tmpGatherings.Clear();
			foreach (GatheringDef gatheringDef in DefDatabase<GatheringDef>.AllDefsListForReading)
			{
				if (gatheringDef.IsRandomSelectable && gatheringDef.CanExecute(this.map, null, forceStart))
				{
					VoluntarilyJoinableLordsStarter.tmpGatherings.Add(gatheringDef);
				}
			}
			GatheringDef gatheringDef2;
			return VoluntarilyJoinableLordsStarter.tmpGatherings.TryRandomElementByWeight((GatheringDef def) => def.randomSelectionWeight, out gatheringDef2) && this.TryStartGathering(gatheringDef2);
		}

		// Token: 0x06004DB7 RID: 19895 RVA: 0x001A130C File Offset: 0x0019F50C
		public bool TryStartGathering(GatheringDef gatheringDef)
		{
			if (!gatheringDef.Worker.TryExecute(this.map, null))
			{
				return false;
			}
			this.lastLordStartTick = Find.TickManager.TicksGame;
			this.startRandomGatheringASAP = false;
			return true;
		}

		// Token: 0x06004DB8 RID: 19896 RVA: 0x001A133C File Offset: 0x0019F53C
		public void VoluntarilyJoinableLordsStarterTick()
		{
			this.Tick_TryStartRandomGathering();
		}

		// Token: 0x06004DB9 RID: 19897 RVA: 0x001A1344 File Offset: 0x0019F544
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLordStartTick, "lastLordStartTick", 0, false);
			Scribe_Values.Look<bool>(ref this.startRandomGatheringASAP, "startPartyASAP", false, false);
		}

		// Token: 0x06004DBA RID: 19898 RVA: 0x001A136C File Offset: 0x0019F56C
		private void Tick_TryStartRandomGathering()
		{
			if (!this.map.IsPlayerHome)
			{
				return;
			}
			if (Find.TickManager.TicksGame % 5000 == 0)
			{
				if (Rand.MTBEventOccurs(40f, 60000f, 5000f))
				{
					this.startRandomGatheringASAP = true;
				}
				if (this.startRandomGatheringASAP && Find.TickManager.TicksGame - this.lastLordStartTick >= 600000)
				{
					this.TryStartRandomGathering(false);
				}
			}
		}

		// Token: 0x04002EE0 RID: 12000
		private Map map;

		// Token: 0x04002EE1 RID: 12001
		private int lastLordStartTick = -999999;

		// Token: 0x04002EE2 RID: 12002
		private bool startRandomGatheringASAP;

		// Token: 0x04002EE3 RID: 12003
		private const int CheckStartGatheringIntervalTicks = 5000;

		// Token: 0x04002EE4 RID: 12004
		private const float StartGatheringMTBDays = 40f;

		// Token: 0x04002EE5 RID: 12005
		private static List<GatheringDef> tmpGatherings = new List<GatheringDef>();
	}
}
