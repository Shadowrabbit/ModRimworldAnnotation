using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200107A RID: 4218
	public class QuestPart_PawnsAvailable : QuestPartActivable
	{
		// Token: 0x06005BD1 RID: 23505 RVA: 0x001D91B4 File Offset: 0x001D73B4
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

		// Token: 0x06005BD2 RID: 23506 RVA: 0x0003FAAA File Offset: 0x0003DCAA
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignalDecrement)
			{
				this.requiredCount--;
			}
		}

		// Token: 0x06005BD3 RID: 23507 RVA: 0x001D924C File Offset: 0x001D744C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.race, "race");
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<int>(ref this.requiredCount, "requiredCount", 0, false);
			Scribe_Values.Look<string>(ref this.inSignalDecrement, "inSignalChangeCount", null, false);
			Scribe_Values.Look<string>(ref this.outSignalPawnsNotAvailable, "outSignalPawnsNotAvailable", null, false);
		}

		// Token: 0x04003D93 RID: 15763
		public ThingDef race;

		// Token: 0x04003D94 RID: 15764
		public int requiredCount;

		// Token: 0x04003D95 RID: 15765
		public MapParent mapParent;

		// Token: 0x04003D96 RID: 15766
		public string inSignalDecrement;

		// Token: 0x04003D97 RID: 15767
		public string outSignalPawnsNotAvailable;

		// Token: 0x04003D98 RID: 15768
		private const int CheckInterval = 500;
	}
}
