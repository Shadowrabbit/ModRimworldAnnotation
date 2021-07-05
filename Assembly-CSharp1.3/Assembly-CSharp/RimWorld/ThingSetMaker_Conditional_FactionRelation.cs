using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D8 RID: 4312
	public class ThingSetMaker_Conditional_FactionRelation : ThingSetMaker_Conditional
	{
		// Token: 0x06006736 RID: 26422 RVA: 0x0022DFEC File Offset: 0x0022C1EC
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

		// Token: 0x04003A41 RID: 14913
		public FactionDef factionDef;

		// Token: 0x04003A42 RID: 14914
		public bool allowHostile;

		// Token: 0x04003A43 RID: 14915
		public bool allowNeutral;

		// Token: 0x04003A44 RID: 14916
		public bool allowAlly;
	}
}
