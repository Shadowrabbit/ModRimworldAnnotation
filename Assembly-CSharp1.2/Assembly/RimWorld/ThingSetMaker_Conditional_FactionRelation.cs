using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200173E RID: 5950
	public class ThingSetMaker_Conditional_FactionRelation : ThingSetMaker_Conditional
	{
		// Token: 0x06008342 RID: 33602 RVA: 0x0026EA74 File Offset: 0x0026CC74
		protected override bool Condition(ThingSetMakerParams parms)
		{
			Faction faction = Find.FactionManager.FirstFactionOfDef(this.factionDef);
			if (faction == null)
			{
				return false;
			}
			switch (faction.RelationKindWith(Faction.OfPlayer))
			{
			case FactionRelationKind.Hostile:
				return this.allowHostile;
			case FactionRelationKind.Neutral:
				return this.allowNeutral;
			case FactionRelationKind.Ally:
				return this.allowAlly;
			default:
				throw new NotImplementedException();
			}
		}

		// Token: 0x0400550C RID: 21772
		public FactionDef factionDef;

		// Token: 0x0400550D RID: 21773
		public bool allowHostile;

		// Token: 0x0400550E RID: 21774
		public bool allowNeutral;

		// Token: 0x0400550F RID: 21775
		public bool allowAlly;
	}
}
