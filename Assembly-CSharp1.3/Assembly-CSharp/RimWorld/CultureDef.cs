using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A5C RID: 2652
	public class CultureDef : Def
	{
		// Token: 0x17000B23 RID: 2851
		// (get) Token: 0x06003FC5 RID: 16325 RVA: 0x0015A15C File Offset: 0x0015835C
		public Texture2D Icon
		{
			get
			{
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x06003FC6 RID: 16326 RVA: 0x0015A184 File Offset: 0x00158384
		public RulePackDef GetPawnNameMaker(Gender gender)
		{
			if (gender == Gender.Female && this.pawnNameMakerFemale != null)
			{
				return this.pawnNameMakerFemale;
			}
			return this.pawnNameMaker;
		}

		// Token: 0x06003FC7 RID: 16327 RVA: 0x0015A19F File Offset: 0x0015839F
		public override IEnumerable<string> ConfigErrors()
		{
			if (!this.thingStyleCategories.NullOrEmpty<ThingStyleCategoryWithPriority>())
			{
				foreach (ThingStyleCategoryWithPriority thingStyleCategoryWithPriority in this.thingStyleCategories)
				{
					if (thingStyleCategoryWithPriority.priority <= 0f)
					{
						yield return "style category " + thingStyleCategoryWithPriority.category.LabelCap + " has <= 0 priority. It must be positive.";
					}
				}
				List<ThingStyleCategoryWithPriority>.Enumerator enumerator = default(List<ThingStyleCategoryWithPriority>.Enumerator);
			}
			if (this.styleItemTags.NullOrEmpty<StyleItemTagWeighted>())
			{
				yield return "no style item tags defined.";
			}
			if (!this.allowedPlaceTags.Any<string>())
			{
				yield return "at least one allowedPlaceTags must be defined.";
			}
			yield break;
			yield break;
		}

		// Token: 0x04002384 RID: 9092
		public RulePackDef pawnNameMaker;

		// Token: 0x04002385 RID: 9093
		public RulePackDef pawnNameMakerFemale;

		// Token: 0x04002386 RID: 9094
		public RulePackDef ideoNameMaker;

		// Token: 0x04002387 RID: 9095
		public RulePackDef deityNameMaker;

		// Token: 0x04002388 RID: 9096
		public RulePackDef deityTypeMaker;

		// Token: 0x04002389 RID: 9097
		public RulePackDef leaderTitleMaker;

		// Token: 0x0400238A RID: 9098
		public RulePackDef festivalNameMaker;

		// Token: 0x0400238B RID: 9099
		public List<ThingStyleCategoryWithPriority> thingStyleCategories;

		// Token: 0x0400238C RID: 9100
		public List<StyleItemTagWeighted> styleItemTags;

		// Token: 0x0400238D RID: 9101
		public IdeoWeaponClassPair preferredWeaponClasses;

		// Token: 0x0400238E RID: 9102
		[NoTranslate]
		public List<string> allowedPlaceTags = new List<string>();

		// Token: 0x0400238F RID: 9103
		public string iconPath;

		// Token: 0x04002390 RID: 9104
		public Color iconColor = Color.white;

		// Token: 0x04002391 RID: 9105
		private Texture2D icon;
	}
}
