using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E8C RID: 3724
	public static class RelationsUtility
	{
		// Token: 0x06005765 RID: 22373 RVA: 0x001DBBB8 File Offset: 0x001D9DB8
		public static bool PawnsKnowEachOther(Pawn p1, Pawn p2)
		{
			return (p1.Faction != null && p1.Faction == p2.Faction) || (p1.RaceProps.IsFlesh && p1.relations.DirectRelations.Find((DirectPawnRelation x) => x.otherPawn == p2) != null) || (p2.RaceProps.IsFlesh && p2.relations.DirectRelations.Find((DirectPawnRelation x) => x.otherPawn == p1) != null) || RelationsUtility.HasAnySocialMemoryWith(p1, p2) || RelationsUtility.HasAnySocialMemoryWith(p2, p1);
		}

		// Token: 0x06005766 RID: 22374 RVA: 0x001DBC9C File Offset: 0x001D9E9C
		public static bool IsDisfigured(Pawn pawn, Pawn forPawn = null, bool ignoreSightSources = false)
		{
			bool flag = forPawn == null || forPawn.Ideo == null || forPawn.Ideo.RequiredScars == 0;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart;
				if ((hediffs[i].def != HediffDefOf.Scarification || flag) && hediffs[i].Part != null && hediffs[i].Part.def.beautyRelated && (hediffs[i] is Hediff_MissingPart || hediffs[i] is Hediff_Injury) && (!ignoreSightSources || (hediff_MissingPart = (hediffs[i] as Hediff_MissingPart)) == null || !hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.SightSource)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005767 RID: 22375 RVA: 0x001DBD78 File Offset: 0x001D9F78
		public static bool TryDevelopBondRelation(Pawn humanlike, Pawn animal, float baseChance)
		{
			if (!animal.RaceProps.Animal)
			{
				return false;
			}
			if (animal.Faction == Faction.OfPlayer && humanlike.IsQuestLodger())
			{
				return false;
			}
			if (animal.RaceProps.trainability.intelligenceOrder < TrainabilityDefOf.Intermediate.intelligenceOrder)
			{
				return false;
			}
			if (humanlike.relations.DirectRelationExists(PawnRelationDefOf.Bond, animal))
			{
				return false;
			}
			if (animal.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Bond, (Pawn x) => x.Spawned) != null)
			{
				return false;
			}
			if (humanlike.story.traits.HasTrait(TraitDefOf.Psychopath))
			{
				return false;
			}
			if (!new HistoryEvent(HistoryEventDefOf.Bonded, humanlike.Named(HistoryEventArgsNames.Doer), animal.Named(HistoryEventArgsNames.Victim)).DoerWillingToDo())
			{
				return false;
			}
			int num = 0;
			List<DirectPawnRelation> directRelations = animal.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].def == PawnRelationDefOf.Bond && !directRelations[i].otherPawn.Dead)
				{
					num++;
				}
			}
			int num2 = 0;
			List<DirectPawnRelation> directRelations2 = humanlike.relations.DirectRelations;
			for (int j = 0; j < directRelations2.Count; j++)
			{
				if (directRelations2[j].def == PawnRelationDefOf.Bond && !directRelations2[j].otherPawn.Dead)
				{
					num2++;
				}
			}
			if (num > 0)
			{
				baseChance *= Mathf.Pow(0.2f, (float)num);
			}
			if (num2 > 0)
			{
				baseChance *= Mathf.Pow(0.55f, (float)num2);
			}
			baseChance *= humanlike.GetStatValue(StatDefOf.BondAnimalChanceFactor, true);
			if (Rand.Value < baseChance)
			{
				humanlike.relations.AddDirectRelation(PawnRelationDefOf.Bond, animal);
				if (humanlike.Faction == Faction.OfPlayer || animal.Faction == Faction.OfPlayer)
				{
					TaleRecorder.RecordTale(TaleDefOf.BondedWithAnimal, new object[]
					{
						humanlike,
						animal
					});
				}
				bool flag = false;
				string value = null;
				if (animal.Name == null || animal.Name.Numerical)
				{
					flag = true;
					value = ((animal.Name == null) ? animal.LabelIndefinite() : animal.Name.ToStringFull);
					animal.Name = PawnBioAndNameGenerator.GeneratePawnName(animal, NameStyle.Full, null);
				}
				if (PawnUtility.ShouldSendNotificationAbout(humanlike) || PawnUtility.ShouldSendNotificationAbout(animal))
				{
					string text;
					if (flag)
					{
						text = "MessageNewBondRelationNewName".Translate(humanlike.LabelShort, value, animal.Name.ToStringFull, humanlike.Named("HUMAN"), animal.Named("ANIMAL")).AdjustedFor(animal, "PAWN", true).CapitalizeFirst();
					}
					else
					{
						text = "MessageNewBondRelation".Translate(humanlike.LabelShort, animal.LabelShort, humanlike.Named("HUMAN"), animal.Named("ANIMAL")).CapitalizeFirst();
					}
					Messages.Message(text, humanlike, MessageTypeDefOf.PositiveEvent, true);
				}
				return true;
			}
			return false;
		}

		// Token: 0x06005768 RID: 22376 RVA: 0x001DC098 File Offset: 0x001DA298
		public static string LabelWithBondInfo(Pawn humanlike, Pawn animal)
		{
			string text = humanlike.LabelShort;
			if (humanlike.relations.DirectRelationExists(PawnRelationDefOf.Bond, animal))
			{
				text += " " + "BondBrackets".Translate();
			}
			return text;
		}

		// Token: 0x06005769 RID: 22377 RVA: 0x001DC0E0 File Offset: 0x001DA2E0
		private static bool HasAnySocialMemoryWith(Pawn p, Pawn otherPawn)
		{
			if (p.Dead)
			{
				return false;
			}
			if (!p.RaceProps.Humanlike || !otherPawn.RaceProps.Humanlike || p.needs == null || p.needs.mood == null)
			{
				return false;
			}
			List<Thought_Memory> memories = p.needs.mood.thoughts.memories.Memories;
			for (int i = 0; i < memories.Count; i++)
			{
				Thought_MemorySocial thought_MemorySocial = memories[i] as Thought_MemorySocial;
				if (thought_MemorySocial != null && thought_MemorySocial.OtherPawn() == otherPawn)
				{
					return true;
				}
			}
			return false;
		}
	}
}
