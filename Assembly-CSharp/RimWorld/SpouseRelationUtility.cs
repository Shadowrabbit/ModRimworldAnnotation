using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AE RID: 5294
	public static class SpouseRelationUtility
	{
		// Token: 0x060071EF RID: 29167 RVA: 0x0004C8EB File Offset: 0x0004AAEB
		public static Pawn GetSpouse(this Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			return pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
		}

		// Token: 0x060071F0 RID: 29168 RVA: 0x0022DFA4 File Offset: 0x0022C1A4
		public static Pawn GetSpouseOppositeGender(this Pawn pawn)
		{
			Pawn spouse = pawn.GetSpouse();
			if (spouse == null)
			{
				return null;
			}
			if ((pawn.gender == Gender.Male && spouse.gender == Gender.Female) || (pawn.gender == Gender.Female && spouse.gender == Gender.Male))
			{
				return spouse;
			}
			return null;
		}

		// Token: 0x060071F1 RID: 29169 RVA: 0x0022DFE4 File Offset: 0x0022C1E4
		public static MarriageNameChange Roll_NameChangeOnMarriage()
		{
			float value = Rand.Value;
			if (value < 0.25f)
			{
				return MarriageNameChange.NoChange;
			}
			if (value < 0.3f)
			{
				return MarriageNameChange.WomansName;
			}
			return MarriageNameChange.MansName;
		}

		// Token: 0x060071F2 RID: 29170 RVA: 0x0004C90D File Offset: 0x0004AB0D
		public static bool Roll_BackToBirthNameAfterDivorce()
		{
			return Rand.Value < 0.6f;
		}

		// Token: 0x060071F3 RID: 29171 RVA: 0x0004C91B File Offset: 0x0004AB1B
		public static void DetermineManAndWomanSpouses(Pawn firstPawn, Pawn secondPawn, out Pawn man, out Pawn woman)
		{
			if (firstPawn.gender == secondPawn.gender)
			{
				man = firstPawn;
				woman = secondPawn;
				return;
			}
			man = ((firstPawn.gender == Gender.Male) ? firstPawn : secondPawn);
			woman = ((firstPawn.gender == Gender.Female) ? firstPawn : secondPawn);
		}

		// Token: 0x060071F4 RID: 29172 RVA: 0x0022E00C File Offset: 0x0022C20C
		public static bool ChangeNameAfterMarriage(Pawn firstPawn, Pawn secondPawn, MarriageNameChange changeName)
		{
			if (changeName == MarriageNameChange.NoChange)
			{
				return false;
			}
			Pawn pawn = null;
			Pawn pawn2 = null;
			SpouseRelationUtility.DetermineManAndWomanSpouses(firstPawn, secondPawn, out pawn, out pawn2);
			NameTriple nameTriple = pawn.Name as NameTriple;
			NameTriple nameTriple2 = pawn2.Name as NameTriple;
			if (nameTriple == null || nameTriple2 == null)
			{
				return false;
			}
			string last = (changeName == MarriageNameChange.MansName) ? nameTriple.Last : nameTriple2.Last;
			pawn.Name = new NameTriple(nameTriple.First, nameTriple.Nick, last);
			pawn2.Name = new NameTriple(nameTriple2.First, nameTriple2.Nick, last);
			return true;
		}

		// Token: 0x060071F5 RID: 29173 RVA: 0x0022E094 File Offset: 0x0022C294
		public static bool ChangeNameAfterDivorce(Pawn pawn, float chance = -1f)
		{
			NameTriple nameTriple = pawn.Name as NameTriple;
			if (nameTriple != null && pawn.story != null && pawn.story.birthLastName != null && nameTriple.Last != pawn.story.birthLastName && SpouseRelationUtility.Roll_BackToBirthNameAfterDivorce())
			{
				pawn.Name = new NameTriple(nameTriple.First, nameTriple.Nick, pawn.story.birthLastName);
				return true;
			}
			return false;
		}

		// Token: 0x060071F6 RID: 29174 RVA: 0x0022E10C File Offset: 0x0022C30C
		public static void Notify_PawnRegenerated(Pawn regenerated)
		{
			if (regenerated.relations != null)
			{
				Pawn firstDirectRelationPawn = regenerated.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
				if (firstDirectRelationPawn != null && regenerated.Name is NameTriple && firstDirectRelationPawn.Name is NameTriple)
				{
					NameTriple nameTriple = firstDirectRelationPawn.Name as NameTriple;
					firstDirectRelationPawn.Name = new NameTriple(nameTriple.First, nameTriple.Nick, nameTriple.Last);
				}
			}
		}

		// Token: 0x060071F7 RID: 29175 RVA: 0x0004C950 File Offset: 0x0004AB50
		public static string GetRandomBirthName(Pawn forPawn)
		{
			return (PawnBioAndNameGenerator.GeneratePawnName(forPawn, NameStyle.Full, null) as NameTriple).Last;
		}

		// Token: 0x060071F8 RID: 29176 RVA: 0x0022E17C File Offset: 0x0022C37C
		public static void ResolveNameForSpouseOnGeneration(ref PawnGenerationRequest request, Pawn generated)
		{
			if (request.FixedLastName != null)
			{
				return;
			}
			MarriageNameChange marriageNameChange = SpouseRelationUtility.Roll_NameChangeOnMarriage();
			if (marriageNameChange != MarriageNameChange.NoChange)
			{
				Pawn spouse = generated.GetSpouse();
				Pawn pawn;
				Pawn pawn2;
				SpouseRelationUtility.DetermineManAndWomanSpouses(generated, spouse, out pawn, out pawn2);
				NameTriple nameTriple = pawn.Name as NameTriple;
				NameTriple nameTriple2 = pawn2.Name as NameTriple;
				if (generated == pawn2 && marriageNameChange == MarriageNameChange.WomansName)
				{
					pawn.Name = new NameTriple(nameTriple.First, nameTriple.Nick, nameTriple.Last);
					if (pawn.story != null)
					{
						pawn.story.birthLastName = SpouseRelationUtility.GetRandomBirthName(pawn);
					}
					request.SetFixedLastName(nameTriple.Last);
					return;
				}
				if (generated == pawn && marriageNameChange == MarriageNameChange.WomansName)
				{
					request.SetFixedLastName(nameTriple2.Last);
					request.SetFixedBirthName(SpouseRelationUtility.GetRandomBirthName(pawn));
					return;
				}
				if (generated == pawn2 && marriageNameChange == MarriageNameChange.MansName)
				{
					request.SetFixedLastName(nameTriple.Last);
					request.SetFixedBirthName(SpouseRelationUtility.GetRandomBirthName(pawn2));
					return;
				}
				if (generated == pawn && marriageNameChange == MarriageNameChange.MansName)
				{
					pawn2.Name = new NameTriple(nameTriple2.First, nameTriple2.Nick, nameTriple2.Last);
					if (pawn2.story != null)
					{
						pawn2.story.birthLastName = SpouseRelationUtility.GetRandomBirthName(pawn);
					}
					request.SetFixedLastName(nameTriple2.Last);
				}
			}
		}

		// Token: 0x04004AF2 RID: 19186
		public const float NoNameChangeOnMarriageChance = 0.25f;

		// Token: 0x04004AF3 RID: 19187
		public const float WomansNameChangeOnMarriageChance = 0.05f;

		// Token: 0x04004AF4 RID: 19188
		public const float MansNameOnMarriageChance = 0.7f;

		// Token: 0x04004AF5 RID: 19189
		public const float ChanceForSpousesToHaveTheSameName = 0.75f;
	}
}
