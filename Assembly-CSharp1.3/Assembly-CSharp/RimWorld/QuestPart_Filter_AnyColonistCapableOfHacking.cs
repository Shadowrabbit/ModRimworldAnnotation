using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B2C RID: 2860
	public class QuestPart_Filter_AnyColonistCapableOfHacking : QuestPart_Filter
	{
		// Token: 0x06004319 RID: 17177 RVA: 0x00166924 File Offset: 0x00164B24
		protected override bool Pass(SignalArgs args)
		{
			if (this.mapParent == null || !this.mapParent.HasMap)
			{
				return false;
			}
			using (List<Pawn>.Enumerator enumerator = this.mapParent.Map.mapPawns.FreeColonistsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (HackUtility.IsCapableOfHacking(enumerator.Current))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x0600431A RID: 17178 RVA: 0x001669A4 File Offset: 0x00164BA4
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x040028D2 RID: 10450
		public MapParent mapParent;
	}
}
