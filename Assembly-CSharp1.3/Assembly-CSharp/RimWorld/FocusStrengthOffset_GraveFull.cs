using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001156 RID: 4438
	public class FocusStrengthOffset_GraveFull : FocusStrengthOffset
	{
		// Token: 0x06006AA9 RID: 27305 RVA: 0x0023D5A4 File Offset: 0x0023B7A4
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				Building_Grave building_Grave = parent as Building_Grave;
				return "StatsReport_GraveFull".Translate(building_Grave.Corpse.InnerPawn.LabelShortCap) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x06006AAA RID: 27306 RVA: 0x0023D60F File Offset: 0x0023B80F
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_GraveFullAbstract".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x06006AAB RID: 27307 RVA: 0x0023D32B File Offset: 0x0023B52B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x06006AAC RID: 27308 RVA: 0x0023D640 File Offset: 0x0023B840
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			Building_Grave building_Grave;
			return parent.Spawned && (building_Grave = (parent as Building_Grave)) != null && building_Grave.HasCorpse && building_Grave.Corpse.InnerPawn.RaceProps.Humanlike;
		}
	}
}
