using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B43 RID: 2883
	public class QuestPart_FactionGoodwillLocked : QuestPartActivable
	{
		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06004371 RID: 17265 RVA: 0x00167C65 File Offset: 0x00165E65
		public override IEnumerable<Faction> InvolvedFactions
		{
			get
			{
				foreach (Faction faction in base.InvolvedFactions)
				{
					yield return faction;
				}
				IEnumerator<Faction> enumerator = null;
				if (this.faction1 != null)
				{
					yield return this.faction1;
				}
				if (this.faction2 != null)
				{
					yield return this.faction2;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06004372 RID: 17266 RVA: 0x00167C75 File Offset: 0x00165E75
		public override void Notify_FactionRemoved(Faction faction)
		{
			if (faction == this.faction1)
			{
				this.faction1 = null;
			}
			if (faction == this.faction2)
			{
				this.faction2 = null;
			}
		}

		// Token: 0x06004373 RID: 17267 RVA: 0x00167C97 File Offset: 0x00165E97
		public bool AppliesTo(Faction a, Faction b)
		{
			return base.State == QuestPartState.Enabled && ((this.faction1 == a && this.faction2 == b) || (this.faction1 == b && this.faction2 == a));
		}

		// Token: 0x06004374 RID: 17268 RVA: 0x00167CCC File Offset: 0x00165ECC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction1, "faction1", false);
			Scribe_References.Look<Faction>(ref this.faction2, "faction2", false);
		}

		// Token: 0x04002900 RID: 10496
		public Faction faction1;

		// Token: 0x04002901 RID: 10497
		public Faction faction2;
	}
}
