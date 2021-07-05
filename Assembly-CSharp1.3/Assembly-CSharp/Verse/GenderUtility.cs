using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004C6 RID: 1222
	[StaticConstructorOnStartup]
	public static class GenderUtility
	{
		// Token: 0x06002539 RID: 9529 RVA: 0x000E85F4 File Offset: 0x000E67F4
		public static string GetGenderLabel(this Pawn pawn)
		{
			return pawn.gender.GetLabel(pawn.RaceProps.Animal);
		}

		// Token: 0x0600253A RID: 9530 RVA: 0x000E860C File Offset: 0x000E680C
		public static string GetLabel(this Gender gender, bool animal = false)
		{
			switch (gender)
			{
			case Gender.None:
				return "NoneLower".Translate();
			case Gender.Male:
				return animal ? "MaleAnimal".Translate() : "Male".Translate();
			case Gender.Female:
				return animal ? "FemaleAnimal".Translate() : "Female".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600253B RID: 9531 RVA: 0x000E8680 File Offset: 0x000E6880
		public static string GetPronoun(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "Proit".Translate();
			case Gender.Male:
				return "Prohe".Translate();
			case Gender.Female:
				return "Proshe".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x000E86D8 File Offset: 0x000E68D8
		public static string GetPossessive(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "Proits".Translate();
			case Gender.Male:
				return "Prohis".Translate();
			case Gender.Female:
				return "Proher".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600253D RID: 9533 RVA: 0x000E8730 File Offset: 0x000E6930
		public static string GetObjective(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return "ProitObj".Translate();
			case Gender.Male:
				return "ProhimObj".Translate();
			case Gender.Female:
				return "ProherObj".Translate();
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600253E RID: 9534 RVA: 0x000E8786 File Offset: 0x000E6986
		public static Texture2D GetIcon(this Gender gender)
		{
			switch (gender)
			{
			case Gender.None:
				return GenderUtility.GenderlessIcon;
			case Gender.Male:
				return GenderUtility.MaleIcon;
			case Gender.Female:
				return GenderUtility.FemaleIcon;
			default:
				throw new ArgumentException();
			}
		}

		// Token: 0x0600253F RID: 9535 RVA: 0x000E87B3 File Offset: 0x000E69B3
		public static Gender Opposite(this Gender gender)
		{
			if (gender == Gender.Male)
			{
				return Gender.Female;
			}
			if (gender == Gender.Female)
			{
				return Gender.Male;
			}
			return Gender.None;
		}

		// Token: 0x04001735 RID: 5941
		private static readonly Texture2D GenderlessIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Genderless", true);

		// Token: 0x04001736 RID: 5942
		private static readonly Texture2D MaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Male", true);

		// Token: 0x04001737 RID: 5943
		private static readonly Texture2D FemaleIcon = ContentFinder<Texture2D>.Get("UI/Icons/Gender/Female", true);
	}
}
