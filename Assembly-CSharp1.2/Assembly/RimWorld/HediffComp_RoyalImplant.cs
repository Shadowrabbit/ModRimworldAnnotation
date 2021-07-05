using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DFC RID: 7676
	public class HediffComp_RoyalImplant : HediffComp
	{
		// Token: 0x17001966 RID: 6502
		// (get) Token: 0x0600A646 RID: 42566 RVA: 0x0006DFE8 File Offset: 0x0006C1E8
		public HediffCompProperties_RoyalImplant Props
		{
			get
			{
				return (HediffCompProperties_RoyalImplant)this.props;
			}
		}

		// Token: 0x0600A647 RID: 42567 RVA: 0x00303618 File Offset: 0x00301818
		public static int GetImplantLevel(Hediff implant)
		{
			Hediff_ImplantWithLevel hediff_ImplantWithLevel = implant as Hediff_ImplantWithLevel;
			if (hediff_ImplantWithLevel != null)
			{
				return hediff_ImplantWithLevel.level;
			}
			return 0;
		}

		// Token: 0x0600A648 RID: 42568 RVA: 0x0006DFF5 File Offset: 0x0006C1F5
		public bool IsViolatingRulesOf(Faction faction, int violationSourceLevel = -1)
		{
			return ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(base.Def, this.parent.pawn, faction, (violationSourceLevel == -1) ? HediffComp_RoyalImplant.GetImplantLevel(this.parent) : violationSourceLevel);
		}

		// Token: 0x0600A649 RID: 42569 RVA: 0x00303638 File Offset: 0x00301838
		public override void Notify_ImplantUsed(string violationSourceName, float detectionChance, int violationSourceLevel = -1)
		{
			base.Notify_ImplantUsed(violationSourceName, detectionChance, -1);
			if (this.parent.pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (!Rand.Chance(detectionChance))
			{
				return;
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (this.IsViolatingRulesOf(faction, violationSourceLevel))
				{
					faction.Notify_RoyalThingUseViolation(this.parent.def, base.Pawn, violationSourceName, detectionChance, violationSourceLevel);
				}
			}
		}
	}
}
