using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C2 RID: 2498
	public class ThoughtWorker_RoyalTitleApparelRequirementNotMet : ThoughtWorker
	{
		// Token: 0x06003E15 RID: 15893 RVA: 0x001544AC File Offset: 0x001526AC
		private static RoyalTitle Validate(Pawn p)
		{
			if (p.royalty == null || !p.royalty.allowApparelRequirements)
			{
				return null;
			}
			foreach (RoyalTitle royalTitle in p.royalty.AllTitlesInEffectForReading)
			{
				if (royalTitle.def.requiredApparel != null && royalTitle.def.requiredApparel.Count > 0)
				{
					for (int i = 0; i < royalTitle.def.requiredApparel.Count; i++)
					{
						ApparelRequirement apparelRequirement = royalTitle.def.requiredApparel[i];
						if (apparelRequirement.IsActive(p) && !apparelRequirement.IsMet(p))
						{
							return royalTitle;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06003E16 RID: 15894 RVA: 0x0015457C File Offset: 0x0015277C
		private static IEnumerable<string> GetAllRequiredApparelPerGroup(Pawn p)
		{
			if (p.royalty == null || !p.royalty.allowApparelRequirements)
			{
				yield break;
			}
			foreach (RoyalTitle t in p.royalty.AllTitlesInEffectForReading)
			{
				if (t.def.requiredApparel != null && t.def.requiredApparel.Count > 0)
				{
					int num;
					for (int i = 0; i < t.def.requiredApparel.Count; i = num + 1)
					{
						ApparelRequirement apparelRequirement = t.def.requiredApparel[i];
						if (apparelRequirement.IsActive(p) && !apparelRequirement.IsMet(p))
						{
							IEnumerable<ThingDef> enumerable = apparelRequirement.AllRequiredApparelForPawn(p, false, false);
							foreach (ThingDef thingDef in enumerable)
							{
								yield return thingDef.LabelCap;
							}
							IEnumerator<ThingDef> enumerator2 = null;
						}
						num = i;
					}
				}
				t = null;
			}
			List<RoyalTitle>.Enumerator enumerator = default(List<RoyalTitle>.Enumerator);
			yield return "ApparelRequirementAnyPrestigeArmor".Translate();
			yield return "ApparelRequirementAnyPsycasterApparel".Translate();
			yield break;
			yield break;
		}

		// Token: 0x06003E17 RID: 15895 RVA: 0x0015458C File Offset: 0x0015278C
		public override string PostProcessLabel(Pawn p, string label)
		{
			RoyalTitle royalTitle = ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return label.Formatted(royalTitle.Named("TITLE"), p.Named("PAWN")).CapitalizeFirst();
		}

		// Token: 0x06003E18 RID: 15896 RVA: 0x001545D4 File Offset: 0x001527D4
		public override string PostProcessDescription(Pawn p, string description)
		{
			RoyalTitle royalTitle = ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return description.Formatted(ThoughtWorker_RoyalTitleApparelRequirementNotMet.GetAllRequiredApparelPerGroup(p).Distinct<string>().ToLineList("- ", false), royalTitle.Named("TITLE"), p.Named("PAWN")).CapitalizeFirst();
		}

		// Token: 0x06003E19 RID: 15897 RVA: 0x00154635 File Offset: 0x00152835
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate(p) == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}
	}
}
