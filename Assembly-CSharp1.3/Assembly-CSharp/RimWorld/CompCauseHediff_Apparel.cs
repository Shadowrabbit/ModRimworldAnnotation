using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200111A RID: 4378
	public class CompCauseHediff_Apparel : ThingComp
	{
		// Token: 0x170011FA RID: 4602
		// (get) Token: 0x06006922 RID: 26914 RVA: 0x002375FB File Offset: 0x002357FB
		private CompProperties_CauseHediff_Apparel Props
		{
			get
			{
				return (CompProperties_CauseHediff_Apparel)this.props;
			}
		}

		// Token: 0x06006923 RID: 26915 RVA: 0x00237608 File Offset: 0x00235808
		public override void Notify_Equipped(Pawn pawn)
		{
			if (pawn.health.hediffSet.GetFirstHediffOfDef(this.Props.hediff, false) == null)
			{
				HediffComp_RemoveIfApparelDropped hediffComp_RemoveIfApparelDropped = pawn.health.AddHediff(this.Props.hediff, pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).FirstOrFallback((BodyPartRecord p) => p.def == this.Props.part, null), null, null).TryGetComp<HediffComp_RemoveIfApparelDropped>();
				if (hediffComp_RemoveIfApparelDropped != null)
				{
					hediffComp_RemoveIfApparelDropped.wornApparel = (Apparel)this.parent;
				}
			}
		}
	}
}
