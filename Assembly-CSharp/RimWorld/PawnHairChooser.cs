using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001412 RID: 5138
	public static class PawnHairChooser
	{
		// Token: 0x06006F03 RID: 28419 RVA: 0x002208DC File Offset: 0x0021EADC
		public static HairDef RandomHairDefFor(Pawn pawn, FactionDef factionType)
		{
			IEnumerable<string> enumerable = PawnHairChooser.HairTagsFromBackstory(pawn);
			IEnumerable<string> enumerable2 = PawnHairChooser.HairTagsFromPawnKind(pawn);
			IEnumerable<string> chosen = (enumerable2.Count<string>() > 0) ? enumerable2 : enumerable;
			if (chosen.Count<string>() == 0)
			{
				chosen = factionType.hairTags;
			}
			return (from hair in DefDatabase<HairDef>.AllDefs
			where hair.hairTags.SharesElementWith(chosen)
			select hair).RandomElementByWeight((HairDef hair) => PawnHairChooser.HairChoiceLikelihoodFor(hair, pawn));
		}

		// Token: 0x06006F04 RID: 28420 RVA: 0x0004B256 File Offset: 0x00049456
		private static IEnumerable<string> HairTagsFromBackstory(Pawn pawn)
		{
			foreach (Backstory b in pawn.story.AllBackstories)
			{
				if (b.hairTags != null)
				{
					int num;
					for (int i = 0; i < b.hairTags.Count; i = num + 1)
					{
						yield return b.hairTags[i];
						num = i;
					}
				}
				b = null;
			}
			IEnumerator<Backstory> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006F05 RID: 28421 RVA: 0x0004B266 File Offset: 0x00049466
		private static IEnumerable<string> HairTagsFromPawnKind(Pawn pawn)
		{
			if (pawn.kindDef.hairTags != null)
			{
				int num;
				for (int i = 0; i < pawn.kindDef.hairTags.Count; i = num + 1)
				{
					yield return pawn.kindDef.hairTags[i];
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x06006F06 RID: 28422 RVA: 0x00220964 File Offset: 0x0021EB64
		private static float HairChoiceLikelihoodFor(HairDef hair, Pawn pawn)
		{
			if (pawn.gender == Gender.None)
			{
				return 100f;
			}
			if (pawn.gender == Gender.Male)
			{
				switch (hair.hairGender)
				{
				case HairGender.Male:
					return 70f;
				case HairGender.MaleUsually:
					return 30f;
				case HairGender.Any:
					return 60f;
				case HairGender.FemaleUsually:
					return 5f;
				case HairGender.Female:
					return 1f;
				}
			}
			if (pawn.gender == Gender.Female)
			{
				switch (hair.hairGender)
				{
				case HairGender.Male:
					return 1f;
				case HairGender.MaleUsually:
					return 5f;
				case HairGender.Any:
					return 60f;
				case HairGender.FemaleUsually:
					return 30f;
				case HairGender.Female:
					return 70f;
				}
			}
			Log.Error(string.Concat(new object[]
			{
				"Unknown hair likelihood for ",
				hair,
				" with ",
				pawn
			}), false);
			return 0f;
		}
	}
}
