using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020010DB RID: 4315
	public class QuestPart_InvolvedFactions : QuestPart
	{
		// Token: 0x17000EA2 RID: 3746
		// (get) Token: 0x06005E28 RID: 24104 RVA: 0x000413DB File Offset: 0x0003F5DB
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

		// Token: 0x06005E29 RID: 24105 RVA: 0x000413EB File Offset: 0x0003F5EB
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<Faction>(ref this.factions, "factions", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x04003EF2 RID: 16114
		public List<Faction> factions = new List<Faction>();
	}
}
