using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001332 RID: 4914
	public class VoluntarilyJoinableLordsStarter : IExposable
	{
		// Token: 0x06006A96 RID: 27286 RVA: 0x00048724 File Offset: 0x00046924
		public VoluntarilyJoinableLordsStarter(Map map)
		{
			this.map = map;
		}

		// Token: 0x06006A97 RID: 27287 RVA: 0x0004873E File Offset: 0x0004693E
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

		// Token: 0x06006A98 RID: 27288 RVA: 0x0004877E File Offset: 0x0004697E
		public bool TryStartReigningSpeech(Pawn pawn)
		{
			if (!GatheringDefOf.ThroneSpeech.CanExecute(pawn.Map, pawn, true))
			{
				return false;
			}
			GatheringDefOf.ThroneSpeech.Worker.TryExecute(pawn.Map, pawn);
			this.lastLordStartTick = Find.TickManager.TicksGame;
			return true;
		}

		// Token: 0x06006A99 RID: 27289 RVA: 0x0020EDF0 File Offset: 0x0020CFF0
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

		// Token: 0x06006A9A RID: 27290 RVA: 0x000487BE File Offset: 0x000469BE
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

		// Token: 0x06006A9B RID: 27291 RVA: 0x000487EE File Offset: 0x000469EE
		public void VoluntarilyJoinableLordsStarterTick()
		{
			this.Tick_TryStartRandomGathering();
		}

		// Token: 0x06006A9C RID: 27292 RVA: 0x000487F6 File Offset: 0x000469F6
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.lastLordStartTick, "lastLordStartTick", 0, false);
			Scribe_Values.Look<bool>(ref this.startRandomGatheringASAP, "startPartyASAP", false, false);
		}

		// Token: 0x06006A9D RID: 27293 RVA: 0x0020EEA0 File Offset: 0x0020D0A0
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

		// Token: 0x040046E8 RID: 18152
		private Map map;

		// Token: 0x040046E9 RID: 18153
		private int lastLordStartTick = -999999;

		// Token: 0x040046EA RID: 18154
		private bool startRandomGatheringASAP;

		// Token: 0x040046EB RID: 18155
		private const int CheckStartGatheringIntervalTicks = 5000;

		// Token: 0x040046EC RID: 18156
		private const float StartGatheringMTBDays = 40f;

		// Token: 0x040046ED RID: 18157
		private static List<GatheringDef> tmpGatherings = new List<GatheringDef>();
	}
}
