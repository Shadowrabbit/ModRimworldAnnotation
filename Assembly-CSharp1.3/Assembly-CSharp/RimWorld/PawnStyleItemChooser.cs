using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DBF RID: 3519
	public static class PawnStyleItemChooser
	{
		// Token: 0x0600516D RID: 20845 RVA: 0x001B6840 File Offset: 0x001B4A40
		public static HairDef RandomHairFor(Pawn pawn)
		{
			if (pawn.kindDef.forcedHair != null)
			{
				return pawn.kindDef.forcedHair;
			}
			return PawnStyleItemChooser.ChooseStyleItem<HairDef>(pawn, null);
		}

		// Token: 0x0600516E RID: 20846 RVA: 0x001B6878 File Offset: 0x001B4A78
		public static bool WantsToUseStyle(Pawn pawn, StyleItemDef styleItemDef, TattooType? tattooType = null)
		{
			TattooDef tattooDef;
			if ((tattooDef = (styleItemDef as TattooDef)) != null)
			{
				if (!ModLister.CheckIdeology("Tattoos"))
				{
					return false;
				}
				if (tattooType != null && tattooDef.tattooType != tattooType.Value)
				{
					return false;
				}
			}
			return pawn.Ideo == null || pawn.Ideo.style.GetFrequency(styleItemDef) > StyleItemFrequency.Never;
		}

		// Token: 0x0600516F RID: 20847 RVA: 0x001B68D8 File Offset: 0x001B4AD8
		public static T ChooseStyleItem<T>(Pawn pawn, TattooType? tattooType = null) where T : StyleItemDef
		{
			IEnumerable<T> source = from item in DefDatabase<T>.AllDefs
			where PawnStyleItemChooser.WantsToUseStyle(pawn, item, tattooType)
			select item;
			if (!source.Any<T>())
			{
				Log.Error("Error finding style item for " + pawn.LabelShort);
				return default(T);
			}
			return source.RandomElementByWeight((T s) => PawnStyleItemChooser.TotalStyleItemLikelihood(s, pawn));
		}

		// Token: 0x06005170 RID: 20848 RVA: 0x001B694E File Offset: 0x001B4B4E
		private static float GetFrequencyFromIdeo(Pawn pawn, StyleItemDef styleItem)
		{
			if (pawn.Ideo == null)
			{
				return 1f;
			}
			return pawn.Ideo.style.GetFrequency(styleItem).GetFrequencyFloat();
		}

		// Token: 0x06005171 RID: 20849 RVA: 0x001B6974 File Offset: 0x001B4B74
		public static float StyleItemChoiceLikelihoodFromTags(StyleItemDef styleItem, List<StyleItemTagWeighted> tags)
		{
			PawnStyleItemChooser.<>c__DisplayClass10_0 CS$<>8__locals1 = new PawnStyleItemChooser.<>c__DisplayClass10_0();
			CS$<>8__locals1.styleItem = styleItem;
			StyleItemTagWeighted styleItemTagWeighted = new StyleItemTagWeighted("", 0f, 1f);
			int i;
			int j;
			for (i = 0; i < CS$<>8__locals1.styleItem.styleTags.Count; i = j + 1)
			{
				StyleItemTagWeighted styleItemTagWeighted2 = tags.Find((StyleItemTagWeighted x) => x.Tag == CS$<>8__locals1.styleItem.styleTags[i]);
				if (styleItemTagWeighted2 != null)
				{
					styleItemTagWeighted.Add(styleItemTagWeighted2);
				}
				j = i;
			}
			return Mathf.Max(0f, styleItemTagWeighted.TotalWeight);
		}

		// Token: 0x06005172 RID: 20850 RVA: 0x001B6A18 File Offset: 0x001B4C18
		public static float GetFrequencyFloat(this StyleItemFrequency styleItemFrequency)
		{
			switch (styleItemFrequency)
			{
			case StyleItemFrequency.Never:
				return 0f;
			case StyleItemFrequency.Rare:
				return 0.1f;
			case StyleItemFrequency.Uncommon:
				return 0.5f;
			case StyleItemFrequency.Normal:
				return 1f;
			case StyleItemFrequency.Common:
				return 2f;
			case StyleItemFrequency.Frequent:
				return 10f;
			default:
				Log.Error("Unknown StyleItemFrequency value.");
				return 0f;
			}
		}

		// Token: 0x06005173 RID: 20851 RVA: 0x001B6A78 File Offset: 0x001B4C78
		public static string GetLabel(this StyleItemFrequency styleItemFrequency)
		{
			switch (styleItemFrequency)
			{
			case StyleItemFrequency.Never:
				return "StyleItemFrequencyNever".Translate();
			case StyleItemFrequency.Rare:
				return "StyleItemFrequencyRare".Translate();
			case StyleItemFrequency.Uncommon:
				return "StyleItemFrequencyUncommon".Translate();
			case StyleItemFrequency.Normal:
				return "StyleItemFrequencyNormal".Translate();
			case StyleItemFrequency.Common:
				return "StyleItemFrequencyCommon".Translate();
			case StyleItemFrequency.Frequent:
				return "StyleItemFrequencyFrequent".Translate();
			default:
				return "Unknown label";
			}
		}

		// Token: 0x06005174 RID: 20852 RVA: 0x001B6B0A File Offset: 0x001B4D0A
		public static StyleItemFrequency GetStyleItemFrequency(float freq)
		{
			if (freq <= 0f)
			{
				return StyleItemFrequency.Never;
			}
			if (freq <= 0.1f)
			{
				return StyleItemFrequency.Rare;
			}
			if (freq <= 0.5f)
			{
				return StyleItemFrequency.Uncommon;
			}
			if (freq <= 1f)
			{
				return StyleItemFrequency.Normal;
			}
			if (freq <= 2f)
			{
				return StyleItemFrequency.Common;
			}
			return StyleItemFrequency.Frequent;
		}

		// Token: 0x06005175 RID: 20853 RVA: 0x001B6B3F File Offset: 0x001B4D3F
		public static float TotalStyleItemLikelihood(StyleItemDef styleItem, Pawn pawn)
		{
			return PawnStyleItemChooser.StyleItemChoiceLikelihoodFor(styleItem, pawn) * PawnStyleItemChooser.GetFrequencyFromIdeo(pawn, styleItem);
		}

		// Token: 0x06005176 RID: 20854 RVA: 0x001B6B50 File Offset: 0x001B4D50
		public static float StyleItemChoiceLikelihoodFor(StyleItemDef styleItem, Pawn pawn)
		{
			if (pawn.gender == Gender.None || pawn.Ideo == null)
			{
				return 100f;
			}
			StyleGender gender = pawn.Ideo.style.GetGender(styleItem);
			if (pawn.gender == Gender.Male)
			{
				switch (gender)
				{
				case StyleGender.Male:
					return 70f;
				case StyleGender.MaleUsually:
					return 30f;
				case StyleGender.Any:
					return 60f;
				case StyleGender.FemaleUsually:
					return 5f;
				case StyleGender.Female:
					return 1f;
				}
			}
			if (pawn.gender == Gender.Female)
			{
				switch (gender)
				{
				case StyleGender.Male:
					return 1f;
				case StyleGender.MaleUsually:
					return 5f;
				case StyleGender.Any:
					return 60f;
				case StyleGender.FemaleUsually:
					return 30f;
				case StyleGender.Female:
					return 70f;
				}
			}
			Log.Error(string.Concat(new object[]
			{
				"Unknown hair likelihood for ",
				styleItem,
				" with ",
				pawn
			}));
			return 0f;
		}

		// Token: 0x04003041 RID: 12353
		public const float StyleItemFrequencyNever = 0f;

		// Token: 0x04003042 RID: 12354
		public const float StyleItemFrequencyRare = 0.1f;

		// Token: 0x04003043 RID: 12355
		public const float StyleItemFrequencyUncommon = 0.5f;

		// Token: 0x04003044 RID: 12356
		public const float StyleItemFrequencyNormal = 1f;

		// Token: 0x04003045 RID: 12357
		public const float StyleItemFrequencyCommon = 2f;

		// Token: 0x04003046 RID: 12358
		public const float StyleItemFrequencyFrequent = 10f;
	}
}
