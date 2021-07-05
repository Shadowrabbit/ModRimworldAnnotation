using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001155 RID: 4437
	public class FocusStrengthOffset_ThroneSatisfiesRequirements : FocusStrengthOffset
	{
		// Token: 0x17001257 RID: 4695
		// (get) Token: 0x06006AA3 RID: 27299 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DependsOnPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006AA4 RID: 27300 RVA: 0x0023D519 File Offset: 0x0023B719
		public override string GetExplanation(Thing parent)
		{
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x06006AA5 RID: 27301 RVA: 0x0023D522 File Offset: 0x0023B722
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_SatisfiesTitle".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x06006AA6 RID: 27302 RVA: 0x0023D32B File Offset: 0x0023B52B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x06006AA7 RID: 27303 RVA: 0x0023D554 File Offset: 0x0023B754
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			if (user == null)
			{
				return false;
			}
			Pawn_RoyaltyTracker royalty = user.royalty;
			bool? flag = (royalty != null) ? new bool?(royalty.GetUnmetThroneroomRequirements(true, false).Any<string>()) : null;
			bool flag2 = false;
			return flag.GetValueOrDefault() == flag2 & flag != null;
		}
	}
}
