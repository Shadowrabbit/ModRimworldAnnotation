using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B8A RID: 2954
	public class QuestPart_Notify_PlayerRaidedSomeone : QuestPart
	{
		// Token: 0x06004514 RID: 17684 RVA: 0x0016E6C4 File Offset: 0x0016C8C4
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				IEnumerable<Pawn> enumerable = null;
				if (this.getRaidersFromMap != null)
				{
					enumerable = this.getRaidersFromMap.mapPawns.FreeColonistsSpawned;
				}
				if (this.getRaidersFromMapParent != null)
				{
					Map map = this.getRaidersFromMapParent.Map;
					if (map == null)
					{
						Log.Error("Cannot resolve map for QuestPart_NotifyPlayerRaidedSomeone.getRaidersFromWorldObjectMap!");
						return;
					}
					enumerable = map.mapPawns.FreeColonistsSpawned;
				}
				if (enumerable == null)
				{
					Log.Error("No raiders could be determined to notify ideoligons!");
					return;
				}
				IdeoUtility.Notify_PlayerRaidedSomeone(enumerable);
			}
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x0016E741 File Offset: 0x0016C941
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<Map>(ref this.getRaidersFromMap, "getRaidersFromMap", false);
			Scribe_References.Look<MapParent>(ref this.getRaidersFromMapParent, "getRaidersFromMapParent", false);
		}

		// Token: 0x040029F8 RID: 10744
		public string inSignal;

		// Token: 0x040029F9 RID: 10745
		public Map getRaidersFromMap;

		// Token: 0x040029FA RID: 10746
		public MapParent getRaidersFromMapParent;
	}
}
