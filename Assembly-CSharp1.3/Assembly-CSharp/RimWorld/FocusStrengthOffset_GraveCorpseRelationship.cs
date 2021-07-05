using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001157 RID: 4439
	public class FocusStrengthOffset_GraveCorpseRelationship : FocusStrengthOffset
	{
		// Token: 0x17001258 RID: 4696
		// (get) Token: 0x06006AAE RID: 27310 RVA: 0x000126F5 File Offset: 0x000108F5
		public override bool DependsOnPawn
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006AAF RID: 27311 RVA: 0x0023D67E File Offset: 0x0023B87E
		public override string GetExplanationAbstract(ThingDef def = null)
		{
			return "StatsReport_GraveCorpseRelatedAbstract".Translate() + ": " + this.offset.ToStringWithSign("0%");
		}

		// Token: 0x06006AB0 RID: 27312 RVA: 0x0023D32B File Offset: 0x0023B52B
		public override float GetOffset(Thing parent, Pawn user = null)
		{
			return this.offset;
		}

		// Token: 0x06006AB1 RID: 27313 RVA: 0x0023D6B0 File Offset: 0x0023B8B0
		public override bool CanApply(Thing parent, Pawn user = null)
		{
			Building_Grave building_Grave = parent as Building_Grave;
			return parent.Spawned && building_Grave != null && building_Grave.HasCorpse && building_Grave.Corpse.InnerPawn.RaceProps.Humanlike && building_Grave.Corpse.InnerPawn.relations.PotentiallyRelatedPawns.Contains(user);
		}
	}
}
