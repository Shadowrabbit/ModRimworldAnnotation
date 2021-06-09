using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000ECC RID: 3788
	public class ThoughtWorker_RoyalTitleApparelRequirementNotMet : ThoughtWorker
	{
		// Token: 0x060053EB RID: 21483 RVA: 0x0000C32E File Offset: 0x0000A52E
		[Obsolete("Will be removed in the future")]
		private static RoyalTitleDef Validate(Pawn p)
		{
			return null;
		}

		// Token: 0x060053EC RID: 21484 RVA: 0x001C2258 File Offset: 0x001C0458
		private static RoyalTitle Validate_NewTemp(Pawn p)
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
						if (!royalTitle.def.requiredApparel[i].IsMet(p))
						{
							return royalTitle;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x060053ED RID: 21485 RVA: 0x0003A55F File Offset: 0x0003875F
		[Obsolete("Only used for mod compatibility. Will be removed in a future update.")]
		private static IEnumerable<string> GetFirstRequiredApparelPerGroup(Pawn p)
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
						RoyalTitleDef.ApparelRequirement apparelRequirement = t.def.requiredApparel[i];
						if (!apparelRequirement.IsMet(p))
						{
							yield return apparelRequirement.AllRequiredApparelForPawn(p, false, false).First<ThingDef>().LabelCap;
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

		// Token: 0x060053EE RID: 21486 RVA: 0x0003A56F File Offset: 0x0003876F
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
						RoyalTitleDef.ApparelRequirement apparelRequirement = t.def.requiredApparel[i];
						if (!apparelRequirement.IsMet(p))
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

		// Token: 0x060053EF RID: 21487 RVA: 0x001C231C File Offset: 0x001C051C
		public override string PostProcessLabel(Pawn p, string label)
		{
			RoyalTitle royalTitle = ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate_NewTemp(p);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return label.Formatted(royalTitle.Named("TITLE"), p.Named("PAWN")).CapitalizeFirst();
		}

		// Token: 0x060053F0 RID: 21488 RVA: 0x001C2364 File Offset: 0x001C0564
		public override string PostProcessDescription(Pawn p, string description)
		{
			RoyalTitle royalTitle = ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate_NewTemp(p);
			if (royalTitle == null)
			{
				return string.Empty;
			}
			return description.Formatted(ThoughtWorker_RoyalTitleApparelRequirementNotMet.GetAllRequiredApparelPerGroup(p).Distinct<string>().ToLineList("- ", false), royalTitle.Named("TITLE"), p.Named("PAWN")).CapitalizeFirst();
		}

		// Token: 0x060053F1 RID: 21489 RVA: 0x0003A57F File Offset: 0x0003877F
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (ThoughtWorker_RoyalTitleApparelRequirementNotMet.Validate_NewTemp(p) == null)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}
	}
}
