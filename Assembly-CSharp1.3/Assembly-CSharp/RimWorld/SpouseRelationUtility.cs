using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E1D RID: 3613
	public static class SpouseRelationUtility
	{
		// Token: 0x06005368 RID: 21352 RVA: 0x001C3C08 File Offset: 0x001C1E08
		public static Pawn GetFirstSpouse(this Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			return pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Spouse, null);
		}

		// Token: 0x06005369 RID: 21353 RVA: 0x001C3C2C File Offset: 0x001C1E2C
		public static List<Pawn> GetSpouses(this Pawn pawn, bool includeDead)
		{
			SpouseRelationUtility.tmpSpouses.Clear();
			if (!pawn.RaceProps.IsFlesh)
			{
				return SpouseRelationUtility.tmpSpouses;
			}
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].def == PawnRelationDefOf.Spouse && (includeDead || !directRelations[i].otherPawn.Dead))
				{
					SpouseRelationUtility.tmpSpouses.Add(directRelations[i].otherPawn);
				}
			}
			return SpouseRelationUtility.tmpSpouses;
		}

		// Token: 0x0600536A RID: 21354 RVA: 0x001C3CB8 File Offset: 0x001C1EB8
		public static List<DirectPawnRelation> GetLoveRelations(this Pawn pawn, bool includeDead)
		{
			SpouseRelationUtility.tmpLoveRelations.Clear();
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (LovePartnerRelationUtility.IsLovePartnerRelation(directRelations[i].def) && (includeDead || !directRelations[i].otherPawn.Dead))
				{
					SpouseRelationUtility.tmpLoveRelations.Add(directRelations[i]);
				}
			}
			return SpouseRelationUtility.tmpLoveRelations;
		}

		// Token: 0x0600536B RID: 21355 RVA: 0x001C3D2C File Offset: 0x001C1F2C
		public static DirectPawnRelation GetMostLikedSpouseRelation(this Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			DirectPawnRelation directPawnRelation = null;
			int num = int.MinValue;
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].def == PawnRelationDefOf.Spouse && !directRelations[i].otherPawn.Dead && (directPawnRelation == null || pawn.relations.OpinionOf(directRelations[i].otherPawn) > num))
				{
					directPawnRelation = directRelations[i];
					num = pawn.relations.OpinionOf(directRelations[i].otherPawn);
				}
			}
			return directPawnRelation;
		}

		// Token: 0x0600536C RID: 21356 RVA: 0x001C3DD4 File Offset: 0x001C1FD4
		public static DirectPawnRelation GetLeastLikedSpouseRelation(this Pawn pawn)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return null;
			}
			DirectPawnRelation directPawnRelation = null;
			int num = int.MaxValue;
			List<DirectPawnRelation> directRelations = pawn.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].def == PawnRelationDefOf.Spouse && !directRelations[i].otherPawn.Dead && (directPawnRelation == null || pawn.relations.OpinionOf(directRelations[i].otherPawn) < num))
				{
					directPawnRelation = directRelations[i];
					num = pawn.relations.OpinionOf(directRelations[i].otherPawn);
				}
			}
			return directPawnRelation;
		}

		// Token: 0x0600536D RID: 21357 RVA: 0x001C3E7C File Offset: 0x001C207C
		public static int GetSpouseCount(this Pawn pawn, bool includeDead)
		{
			if (!pawn.RaceProps.IsFlesh)
			{
				return 0;
			}
			return pawn.relations.GetDirectRelationsCount(PawnRelationDefOf.Spouse, (Pawn x) => includeDead || !x.Dead);
		}

		// Token: 0x0600536E RID: 21358 RVA: 0x001C3EC4 File Offset: 0x001C20C4
		public static List<Pawn> GetLoveCluster(this Pawn pawn)
		{
			SpouseRelationUtility.tmpMarriageClusterPawns.Clear();
			SpouseRelationUtility.tmpMarriageClusterPawns.Add(pawn);
			SpouseRelationUtility.tmpStack.Clear();
			SpouseRelationUtility.tmpStack.Push(pawn);
			int num = 200;
			while (SpouseRelationUtility.tmpStack.Count > 0)
			{
				List<DirectPawnRelation> loveRelations = SpouseRelationUtility.tmpStack.Pop().GetLoveRelations(false);
				for (int i = 0; i < loveRelations.Count; i++)
				{
					if (!SpouseRelationUtility.tmpMarriageClusterPawns.Contains(loveRelations[i].otherPawn))
					{
						SpouseRelationUtility.tmpMarriageClusterPawns.Add(loveRelations[i].otherPawn);
						SpouseRelationUtility.tmpStack.Push(loveRelations[i].otherPawn);
					}
				}
				num--;
				if (num <= 0)
				{
					Log.ErrorOnce("GetMarriageCluster exceeded iterations limit.", 1462229);
					break;
				}
			}
			return SpouseRelationUtility.tmpMarriageClusterPawns;
		}

		// Token: 0x0600536F RID: 21359 RVA: 0x001C3F98 File Offset: 0x001C2198
		public static void RemoveSpousesAsForbiddenByIdeo(Pawn pawn)
		{
			SpouseRelationUtility.tmpDivorcedPawnNames.Clear();
			HistoryEvent ev = new HistoryEvent(pawn.GetHistoryEventForSpouseCount(), pawn.Named(HistoryEventArgsNames.Doer));
			int num = 200;
			while (!ev.DoerWillingToDo())
			{
				DirectPawnRelation leastLikedSpouseRelation = pawn.GetLeastLikedSpouseRelation();
				if (leastLikedSpouseRelation == null)
				{
					break;
				}
				SpouseRelationUtility.DoDivorce(pawn, leastLikedSpouseRelation.otherPawn);
				SpouseRelationUtility.tmpDivorcedPawnNames.Add(leastLikedSpouseRelation.otherPawn.NameShortColored.Resolve());
				ev.def = pawn.GetHistoryEventForSpouseCount();
				num--;
				if (num <= 0)
				{
					Log.ErrorOnce("RemoveSpousesAsForbiddenByIdeo exceeded iterations limit.", 18483836);
					break;
				}
			}
			if (SpouseRelationUtility.tmpDivorcedPawnNames.Any<string>() && PawnUtility.ShouldSendNotificationAbout(pawn))
			{
				Find.LetterStack.ReceiveLetter("LetterIdeoChangedDivorce".Translate() + ": " + pawn.LabelShortCap, "LetterIdeoChangedDivorcedPawns".Translate(pawn.Named("PAWN"), SpouseRelationUtility.tmpDivorcedPawnNames.ToLineList("  - ")), LetterDefOf.NeutralEvent, new LookTargets(pawn), null, null, null, null);
			}
			SpouseRelationUtility.tmpDivorcedPawnNames.Clear();
		}

		// Token: 0x06005370 RID: 21360 RVA: 0x001C40B0 File Offset: 0x001C22B0
		public static void DoDivorce(Pawn initiator, Pawn recipient)
		{
			initiator.relations.RemoveDirectRelation(PawnRelationDefOf.Spouse, recipient);
			initiator.relations.AddDirectRelation(PawnRelationDefOf.ExSpouse, recipient);
			SpouseRelationUtility.RemoveGotMarriedThoughts(initiator, recipient);
			if (initiator.ownership.OwnedBed != null && initiator.ownership.OwnedBed == recipient.ownership.OwnedBed)
			{
				((Rand.Value < 0.5f) ? initiator : recipient).ownership.UnclaimBed();
			}
			SpouseRelationUtility.ChangeNameAfterDivorce(initiator, -1f);
			SpouseRelationUtility.ChangeNameAfterDivorce(recipient, -1f);
			TaleRecorder.RecordTale(TaleDefOf.Breakup, new object[]
			{
				initiator,
				recipient
			});
		}

		// Token: 0x06005371 RID: 21361 RVA: 0x001C4158 File Offset: 0x001C2358
		public static void RemoveGotMarriedThoughts(Pawn initiator, Pawn recipient)
		{
			if (initiator.needs.mood != null)
			{
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
				initiator.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.HoneymoonPhase, recipient);
			}
			if (recipient.needs.mood != null)
			{
				recipient.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.DivorcedMe, initiator, null);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDef(ThoughtDefOf.GotMarried);
				recipient.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.HoneymoonPhase, initiator);
			}
		}

		// Token: 0x06005372 RID: 21362 RVA: 0x001C4220 File Offset: 0x001C2420
		public static HistoryEventDef GetHistoryEventForSpouseCount(this Pawn pawn)
		{
			int spouseCount = pawn.GetSpouseCount(false);
			if (spouseCount <= 1)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_OneOrFewer;
			}
			if (spouseCount <= 2)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Two;
			}
			if (spouseCount <= 3)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Three;
			}
			if (spouseCount <= 4)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Four;
			}
			return HistoryEventDefOf.GotMarried_SpouseCount_FiveOrMore;
		}

		// Token: 0x06005373 RID: 21363 RVA: 0x001C4264 File Offset: 0x001C2464
		public static HistoryEventDef GetHistoryEventForSpouseCountPlusOne(this Pawn pawn)
		{
			int spouseCount = pawn.GetSpouseCount(false);
			if (spouseCount == 0)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_OneOrFewer;
			}
			if (spouseCount < 2)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Two;
			}
			if (spouseCount < 3)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Three;
			}
			if (spouseCount < 4)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Four;
			}
			return HistoryEventDefOf.GotMarried_SpouseCount_FiveOrMore;
		}

		// Token: 0x06005374 RID: 21364 RVA: 0x001C42A8 File Offset: 0x001C24A8
		public static HistoryEventDef GetHistoryEventForSpouseAndFianceCountPlusOne(this Pawn pawn)
		{
			List<DirectPawnRelation> list = LovePartnerRelationUtility.ExistingLovePartners(pawn, false);
			int num = 0;
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].def != PawnRelationDefOf.Lover)
				{
					num++;
				}
			}
			if (num == 0)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_OneOrFewer;
			}
			if (num < 2)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Two;
			}
			if (num < 3)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Three;
			}
			if (num < 4)
			{
				return HistoryEventDefOf.GotMarried_SpouseCount_Four;
			}
			return HistoryEventDefOf.GotMarried_SpouseCount_FiveOrMore;
		}

		// Token: 0x06005375 RID: 21365 RVA: 0x001C4314 File Offset: 0x001C2514
		public static Pawn GetFirstSpouseOfOppositeGender(this Pawn pawn)
		{
			foreach (Pawn pawn2 in pawn.GetSpouses(true))
			{
				if (pawn.gender.Opposite() == pawn2.gender)
				{
					return pawn2;
				}
			}
			return null;
		}

		// Token: 0x06005376 RID: 21366 RVA: 0x001C437C File Offset: 0x001C257C
		public static MarriageNameChange Roll_NameChangeOnMarriage(Pawn pawn)
		{
			List<MarriageNameChange> list = new List<MarriageNameChange>();
			if (new HistoryEvent(HistoryEventDefOf.GotMarried_TookMansName, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				list.Add(MarriageNameChange.MansName);
			}
			if (new HistoryEvent(HistoryEventDefOf.GotMarried_TookWomansName, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				list.Add(MarriageNameChange.WomansName);
			}
			if (new HistoryEvent(HistoryEventDefOf.GotMarried_KeptName, pawn.Named(HistoryEventArgsNames.Doer)).DoerWillingToDo())
			{
				list.Add(MarriageNameChange.NoChange);
			}
			if (!list.Any<MarriageNameChange>())
			{
				return MarriageNameChange.NoChange;
			}
			return list.RandomElement<MarriageNameChange>();
		}

		// Token: 0x06005377 RID: 21367 RVA: 0x001C4408 File Offset: 0x001C2608
		public static bool Roll_BackToBirthNameAfterDivorce()
		{
			return Rand.Value < 0.6f;
		}

		// Token: 0x06005378 RID: 21368 RVA: 0x001C4418 File Offset: 0x001C2618
		public static void DetermineManAndWomanSpouses(Pawn firstPawn, Pawn secondPawn, out Pawn man, out Pawn woman)
		{
			if (firstPawn.gender == secondPawn.gender)
			{
				man = ((firstPawn.thingIDNumber < secondPawn.thingIDNumber) ? firstPawn : secondPawn);
				woman = ((firstPawn.thingIDNumber < secondPawn.thingIDNumber) ? secondPawn : firstPawn);
				return;
			}
			man = ((firstPawn.gender == Gender.Male) ? firstPawn : secondPawn);
			woman = ((firstPawn.gender == Gender.Female) ? firstPawn : secondPawn);
		}

		// Token: 0x06005379 RID: 21369 RVA: 0x001C447C File Offset: 0x001C267C
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

		// Token: 0x0600537A RID: 21370 RVA: 0x001C4504 File Offset: 0x001C2704
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

		// Token: 0x0600537B RID: 21371 RVA: 0x001C457C File Offset: 0x001C277C
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

		// Token: 0x0600537C RID: 21372 RVA: 0x001C45E9 File Offset: 0x001C27E9
		public static string GetRandomBirthName(Pawn forPawn)
		{
			return (PawnBioAndNameGenerator.GeneratePawnName(forPawn, NameStyle.Full, null) as NameTriple).Last;
		}

		// Token: 0x0600537D RID: 21373 RVA: 0x001C4600 File Offset: 0x001C2800
		public static void ResolveNameForSpouseOnGeneration(ref PawnGenerationRequest request, Pawn generated)
		{
			if (request.FixedLastName != null)
			{
				return;
			}
			MarriageNameChange marriageNameChange = SpouseRelationUtility.Roll_NameChangeOnMarriage(generated);
			if (marriageNameChange != MarriageNameChange.NoChange)
			{
				Pawn firstSpouse = generated.GetFirstSpouse();
				Pawn pawn;
				Pawn pawn2;
				SpouseRelationUtility.DetermineManAndWomanSpouses(generated, firstSpouse, out pawn, out pawn2);
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

		// Token: 0x040030F7 RID: 12535
		public const float NoNameChangeOnMarriageChance = 0.25f;

		// Token: 0x040030F8 RID: 12536
		public const float WomansNameChangeOnMarriageChance = 0.05f;

		// Token: 0x040030F9 RID: 12537
		public const float MansNameOnMarriageChance = 0.7f;

		// Token: 0x040030FA RID: 12538
		public const float ChanceForSpousesToHaveTheSameName = 0.75f;

		// Token: 0x040030FB RID: 12539
		private static List<Pawn> tmpSpouses = new List<Pawn>();

		// Token: 0x040030FC RID: 12540
		private static List<DirectPawnRelation> tmpLoveRelations = new List<DirectPawnRelation>();

		// Token: 0x040030FD RID: 12541
		private static List<Pawn> tmpMarriageClusterPawns = new List<Pawn>();

		// Token: 0x040030FE RID: 12542
		private static Stack<Pawn> tmpStack = new Stack<Pawn>();

		// Token: 0x040030FF RID: 12543
		private static readonly List<string> tmpDivorcedPawnNames = new List<string>();
	}
}
