using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001554 RID: 5460
	public static class RelationsUtility
	{
		// Token: 0x06007668 RID: 30312 RVA: 0x0024121C File Offset: 0x0023F41C
		public static bool PawnsKnowEachOther(Pawn p1, Pawn p2)
		{
			return (p1.Faction != null && p1.Faction == p2.Faction) || (p1.RaceProps.IsFlesh && p1.relations.DirectRelations.Find((DirectPawnRelation x) => x.otherPawn == p2) != null) || (p2.RaceProps.IsFlesh && p2.relations.DirectRelations.Find((DirectPawnRelation x) => x.otherPawn == p1) != null) || RelationsUtility.HasAnySocialMemoryWith(p1, p2) || RelationsUtility.HasAnySocialMemoryWith(p2, p1);
		}

		// Token: 0x06007669 RID: 30313 RVA: 0x00241300 File Offset: 0x0023F500
		public static bool IsDisfigured(Pawn pawn)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if (hediffs[i].Part != null && hediffs[i].Part.def.beautyRelated && (hediffs[i] is Hediff_MissingPart || hediffs[i] is Hediff_Injury))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600766A RID: 30314 RVA: 0x00241374 File Offset: 0x0023F574
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

		// Token: 0x0600766B RID: 30315 RVA: 0x0024165C File Offset: 0x0023F85C
		public static string LabelWithBondInfo(Pawn humanlike, Pawn animal)
		{
			string text = humanlike.LabelShort;
			if (humanlike.relations.DirectRelationExists(PawnRelationDefOf.Bond, animal))
			{
				text += " " + "BondBrackets".Translate();
			}
			return text;
		}

		// Token: 0x0600766C RID: 30316 RVA: 0x002416A4 File Offset: 0x0023F8A4
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
