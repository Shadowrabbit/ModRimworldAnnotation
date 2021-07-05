using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B90 RID: 2960
	public class QuestPart_RequiredShuttleThings : QuestPart
	{
		// Token: 0x06004537 RID: 17719 RVA: 0x0016EEB8 File Offset: 0x0016D0B8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				CompShuttle compShuttle = this.shuttle.TryGetComp<CompShuttle>();
				if (compShuttle != null)
				{
					compShuttle.requireAllColonistsOnMap = this.requireAllColonistsOnMap;
					compShuttle.requiredColonistCount = this.requiredColonistCount;
				}
			}
		}

		// Token: 0x06004538 RID: 17720 RVA: 0x0016EF08 File Offset: 0x0016D108
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Thing>(ref this.shuttle, "shuttle", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Values.Look<bool>(ref this.requireAllColonistsOnMap, "requireAllColonistsOnMap", false, false);
			Scribe_Values.Look<int>(ref this.requiredColonistCount, "requiredColonistCount", -1, false);
		}

		// Token: 0x04002A15 RID: 10773
		public Thing shuttle;

		// Token: 0x04002A16 RID: 10774
		public MapParent mapParent;

		// Token: 0x04002A17 RID: 10775
		public bool requireAllColonistsOnMap;

		// Token: 0x04002A18 RID: 10776
		public int requiredColonistCount = -1;

		// Token: 0x04002A19 RID: 10777
		public string inSignal;
	}
}
