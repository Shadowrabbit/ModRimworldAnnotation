using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AA7 RID: 2727
	public class PreceptApparelRequirement : IExposable
	{
		// Token: 0x060040CB RID: 16587 RVA: 0x0015DE3C File Offset: 0x0015C03C
		public bool CanAddRequirement(Precept precept, List<PreceptApparelRequirement> currentRequirements, out string cannotAddReason, FactionDef generatingFor = null)
		{
			if (!this.Compatible(precept.ideo, generatingFor))
			{
				cannotAddReason = "FactionOrIdeoIncompatible".Translate();
				return false;
			}
			if (currentRequirements.SelectMany((PreceptApparelRequirement r) => r.requirement.bodyPartGroupsMatchAny).Intersect(this.requirement.bodyPartGroupsMatchAny).Any<BodyPartGroupDef>())
			{
				cannotAddReason = "BodyPartAlreadyCovered".Translate();
				return false;
			}
			cannotAddReason = null;
			return true;
		}

		// Token: 0x060040CC RID: 16588 RVA: 0x0015DEC0 File Offset: 0x0015C0C0
		public bool Compatible(Ideo ideo, FactionDef forFaction)
		{
			if (!this.requirement.IsValid)
			{
				return false;
			}
			if (!this.<Compatible>g__CheckFaction|5_0(forFaction))
			{
				return false;
			}
			if (Find.World != null && Find.FactionManager != null)
			{
				foreach (Faction faction in Find.FactionManager.AllFactions)
				{
					if (faction.def != forFaction && faction.ideos != null && faction.ideos.PrimaryIdeo == ideo && !this.<Compatible>g__CheckFaction|5_0(faction.def))
					{
						return false;
					}
				}
			}
			return this.anyMemeRequired == null || !ideo.memes.Any((MemeDef m) => this.anyMemeRequired.Contains(m));
		}

		// Token: 0x060040CD RID: 16589 RVA: 0x0015DF8C File Offset: 0x0015C18C
		public void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.allowedFactionCategoryTags, "allowedFactionCategoryTags", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.disallowedFactionCategoryTags, "disallowedFactionCategoryTags", LookMode.Value, Array.Empty<object>());
			Scribe_Collections.Look<MemeDef>(ref this.anyMemeRequired, "anyMemeRequired", LookMode.Def, Array.Empty<object>());
			Scribe_Deep.Look<ApparelRequirement>(ref this.requirement, "requirement", Array.Empty<object>());
		}

		// Token: 0x060040CF RID: 16591 RVA: 0x0015DFF0 File Offset: 0x0015C1F0
		[CompilerGenerated]
		private bool <Compatible>g__CheckFaction|5_0(FactionDef faction)
		{
			return (faction == null || this.allowedFactionCategoryTags == null || this.allowedFactionCategoryTags.Contains(faction.categoryTag)) && (faction == null || this.disallowedFactionCategoryTags == null || !this.disallowedFactionCategoryTags.Contains(faction.categoryTag));
		}

		// Token: 0x040025B8 RID: 9656
		public List<string> allowedFactionCategoryTags;

		// Token: 0x040025B9 RID: 9657
		public List<string> disallowedFactionCategoryTags;

		// Token: 0x040025BA RID: 9658
		public List<MemeDef> anyMemeRequired;

		// Token: 0x040025BB RID: 9659
		public ApparelRequirement requirement;
	}
}
