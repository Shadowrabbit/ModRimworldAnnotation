using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020017E9 RID: 6121
	public class FocusStrengthOffset_ThroneSatisfiesRequirements : FocusStrengthOffset
	{
		// Token: 0x1700151A RID: 5402
		// (get) Token: 0x06008780 RID: 34688 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DependsOnPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06008781 RID: 34689 RVA: 0x0005AEEB File Offset: 0x000590EB
		public override string GetExplanation(Thing parent)
		{
			return this.GetExplanationAbstract(null);
		}

		// Token: 0x06008782 RID: 34690 RVA: 0x0005AEF4 File Offset: 0x000590F4
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_SatisfiesTitle".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x06008783 RID: 34691 RVA: 0x0005AE5B File Offset: 0x0005905B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x06008784 RID: 34692 RVA: 0x0027BCF8 File Offset: 0x00279EF8
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
