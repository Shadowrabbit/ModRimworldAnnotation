using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B80 RID: 2944
	public class QuestPart_InvolvedFactions : QuestPart
	{
		// Token: 0x17000C10 RID: 3088
		// (get) Token: 0x060044D6 RID: 17622 RVA: 0x0016C969 File Offset: 0x0016AB69
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				foreach (Faction faction2 in this.factions)
				{
					yield return faction2;
				}
				List<Faction>.Enumerator enumerator2 = default(List<Faction>.Enumerator);
				yield break;
				yield break;
			}
		}

		// Token: 0x060044D7 RID: 17623 RVA: 0x0016C979 File Offset: 0x0016AB79
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Faction>(ref this.factions, "factions", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x040029C4 RID: 10692
		public List<Faction> factions = new List<Faction>();
	}
}
