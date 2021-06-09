using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200106F RID: 4207
	public class QuestPart_FactionRelationKind : QuestPartActivable
	{
		// Token: 0x17000E28 RID: 3624
		// (get) Token: 0x06005B80 RID: 23424 RVA: 0x0003F6C3 File Offset: 0x0003D8C3
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

		// Token: 0x06005B81 RID: 23425 RVA: 0x001D86A0 File Offset: 0x001D68A0
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.faction1 != null && this.faction2 != null && this.faction1.RelationKindWith(this.faction2) == this.relationKind)
			{
				base.Complete(this.faction1.Named("SUBJECT"));
			}
		}

		// Token: 0x06005B82 RID: 23426 RVA: 0x0003F6D3 File Offset: 0x0003D8D3
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction1, "faction1", false);
			Scribe_References.Look<Faction>(ref this.faction2, "faction2", false);
			Scribe_Values.Look<FactionRelationKind>(ref this.relationKind, "relationKind", FactionRelationKind.Hostile, false);
		}

		// Token: 0x06005B83 RID: 23427 RVA: 0x0003F70F File Offset: 0x0003D90F
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.faction1 = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			this.faction2 = Faction.OfPlayer;
			this.relationKind = FactionRelationKind.Neutral;
		}

		// Token: 0x04003D6E RID: 15726
		public Faction faction1;

		// Token: 0x04003D6F RID: 15727
		public Faction faction2;

		// Token: 0x04003D70 RID: 15728
		public FactionRelationKind relationKind;
	}
}
