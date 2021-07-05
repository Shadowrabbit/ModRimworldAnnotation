using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B44 RID: 2884
	public class QuestPart_FactionRelationKind : QuestPartActivable
	{
		// Token: 0x17000BD3 RID: 3027
		// (get) Token: 0x06004377 RID: 17271 RVA: 0x00167CFE File Offset: 0x00165EFE
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

		// Token: 0x06004378 RID: 17272 RVA: 0x00167D10 File Offset: 0x00165F10
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.faction1 != null && this.faction2 != null && this.faction1.RelationKindWith(this.faction2) == this.relationKind)
			{
				base.Complete(this.faction1.Named("SUBJECT"));
			}
		}

		// Token: 0x06004379 RID: 17273 RVA: 0x00167D62 File Offset: 0x00165F62
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction1, "faction1", false);
			Scribe_References.Look<Faction>(ref this.faction2, "faction2", false);
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
		}

		// Token: 0x0600437A RID: 17274 RVA: 0x00167D9E File Offset: 0x00165F9E
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.faction1 = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			this.faction2 = Faction.OfPlayer;
			this.relationKind = FactionRelationKind.Neutral;
		}

		// Token: 0x04002902 RID: 10498
		public Faction faction1;

		// Token: 0x04002903 RID: 10499
		public Faction faction2;

		// Token: 0x04002904 RID: 10500
		public FactionRelationKind relationKind;
	}
}
