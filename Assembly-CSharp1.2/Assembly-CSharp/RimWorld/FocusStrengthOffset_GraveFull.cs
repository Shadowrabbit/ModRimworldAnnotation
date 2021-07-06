using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017EA RID: 6122
	public class FocusStrengthOffset_GraveFull : FocusStrengthOffset
	{
		// Token: 0x06008786 RID: 34694 RVA: 0x0027BD48 File Offset: 0x00279F48
		public override string GetExplanation(Thing parent)
		{
			if (this.CanApply(parent, null))
			{
				Building_Grave building_Grave = parent as Building_Grave;
				return "StatsReport_GraveFull".Translate(building_Grave.Corpse.InnerPawn.LabelShortCap) + ": " + this.GetOffset(parent, null).ToStringWithSign("0%");
			}
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x06008787 RID: 34695 RVA: 0x0005AF24 File Offset: 0x00059124
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_GraveFullAbstract".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x06008788 RID: 34696 RVA: 0x0005AE5B File Offset: 0x0005905B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x06008789 RID: 34697 RVA: 0x0027BDB4 File Offset: 0x00279FB4
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			Building_Grave building_Grave;
			return parent.Spawned && (building_Grave = (parent as Building_Grave)) != null && building_Grave.HasCorpse && building_Grave.Corpse.InnerPawn.RaceProps.Humanlike;
		}
	}
}
