using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020017EB RID: 6123
	public class FocusStrengthOffset_GraveCorpseRelationship : FocusStrengthOffset
	{
		// Token: 0x1700151B RID: 5403
		// (get) Token: 0x0600878B RID: 34699 RVA: 0x0000A2A7 File Offset: 0x000084A7
		public override bool DependsOnPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600878C RID: 34700 RVA: 0x0005AF54 File Offset: 0x00059154
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_GraveCorpseRelatedAbstract".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x0600878D RID: 34701 RVA: 0x0005AE5B File Offset: 0x0005905B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x0600878E RID: 34702 RVA: 0x0027BDF4 File Offset: 0x00279FF4
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			Building_Grave building_Grave = parent as Building_Grave;
			return parent.Spawned && building_Grave != null && building_Grave.HasCorpse && building_Grave.Corpse.InnerPawn.RaceProps.Humanlike && building_Grave.Corpse.InnerPawn.relations.PotentiallyRelatedPawns.Contains(user);
		}
	}
}
