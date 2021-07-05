using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B4B RID: 2891
	public class QuestPart_PawnsAvailable : QuestPartActivable
	{
		// Token: 0x060043A4 RID: 17316 RVA: 0x00168590 File Offset: 0x00166790
		public override void QuestPartTick()
		{
			if (this.requiredCount > 0 && Find.TickManager.TicksAbs % 500 == 0)
			{
				int num = 0;
				List<Pawn> allPawnsSpawned = this.mapParent.Map.mapPawns.AllPawnsSpawned;
				for (int i = 0; i < allPawnsSpawned.Count; i++)
				{
					if (allPawnsSpawned[i].def == this.race && allPawnsSpawned[i].Faction == null)
					{
						num++;
					}
				}
				if (num < this.requiredCount)
				{
					Find.SignalManager.SendSignal(new Signal(this.outSignalPawnsNotAvailable));
				}
			}
		}

		// Token: 0x060043A5 RID: 17317 RVA: 0x00168625 File Offset: 0x00166825
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalDecrement)
			{
				this.requiredCount--;
			}
		}

		// Token: 0x060043A6 RID: 17318 RVA: 0x00168650 File Offset: 0x00166850
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.race, "race");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.requiredCount, "requiredCount", 0, false);
			Scribe_Values.Look<string>(ref this.inSignalDecrement, "inSignalChangeCount", null, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnsNotAvailable, "outSignalPawnsNotAvailable", null, false);
		}

		// Token: 0x04002914 RID: 10516
		public ThingDef race;

		// Token: 0x04002915 RID: 10517
		public int requiredCount;

		// Token: 0x04002916 RID: 10518
		public MapParent mapParent;

		// Token: 0x04002917 RID: 10519
		public string inSignalDecrement;

		// Token: 0x04002918 RID: 10520
		public string outSignalPawnsNotAvailable;

		// Token: 0x04002919 RID: 10521
		private const int CheckInterval = 500;
	}
}
