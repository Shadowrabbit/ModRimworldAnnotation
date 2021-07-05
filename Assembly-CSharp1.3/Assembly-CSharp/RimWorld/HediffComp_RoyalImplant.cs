using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001573 RID: 5491
	public class HediffComp_RoyalImplant : HediffComp
	{
		// Token: 0x170015F8 RID: 5624
		// (get) Token: 0x060081DE RID: 33246 RVA: 0x002DE6F1 File Offset: 0x002DC8F1
		public HediffCompProperties_RoyalImplant Props
		{
			get
			{
				return (HediffCompProperties_RoyalImplant)this.props;
			}
		}

		// Token: 0x060081DF RID: 33247 RVA: 0x002DE700 File Offset: 0x002DC900
		public static int GetImplantLevel(Hediff implant)
		{
			Hediff_Level hediff_Level = implant as Hediff_Level;
			if (hediff_Level != null)
			{
				return hediff_Level.level;
			}
			return 0;
		}

		// Token: 0x060081E0 RID: 33248 RVA: 0x002DE71F File Offset: 0x002DC91F
		public bool IsViolatingRulesOf(Faction faction, int violationSourceLevel = -1)
		{
			return ThingRequiringRoyalPermissionUtility.IsViolatingRulesOf(base.Def, this.parent.pawn, faction, (violationSourceLevel == -1) ? HediffComp_RoyalImplant.GetImplantLevel(this.parent) : violationSourceLevel);
		}

		// Token: 0x060081E1 RID: 33249 RVA: 0x002DE74C File Offset: 0x002DC94C
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
