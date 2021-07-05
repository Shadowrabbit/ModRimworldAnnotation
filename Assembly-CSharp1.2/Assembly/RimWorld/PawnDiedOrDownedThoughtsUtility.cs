using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020013EE RID: 5102
	public static class PawnDiedOrDownedThoughtsUtility
	{
		// Token: 0x06006E52 RID: 28242 RVA: 0x0021C3F0 File Offset: 0x0021A5F0
		public static void TryGiveThoughts(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			try
			{
				if (!PawnGenerator.IsBeingGenerated(victim))
				{
					if (Current.ProgramState == ProgramState.Playing)
					{
						PawnDiedOrDownedThoughtsUtility.GetThoughts(victim, dinfo, thoughtsKind, PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd, PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts);
						for (int i = 0; i < PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Count; i++)
						{
							PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd[i].Add();
						}
						if (PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Any<ThoughtToAddToAll>())
						{
							foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists)
							{
								if (pawn != victim)
								{
									for (int j = 0; j < PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Count; j++)
									{
										PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts[j].Add(pawn);
									}
								}
							}
						}
						PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Clear();
						PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Clear();
					}
				}
			}
			catch (Exception arg)
			{
				Log.Error("Could not give thoughts: " + arg, false);
			}
		}

		// Token: 0x06006E53 RID: 28243 RVA: 0x0021C508 File Offset: 0x0021A708
		public static void TryGiveThoughts(IEnumerable<Pawn> victims, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			foreach (Pawn victim in victims)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(victim, null, thoughtsKind);
			}
		}

		// Token: 0x06006E54 RID: 28244 RVA: 0x0021C558 File Offset: 0x0021A758
		public static void GetThoughts(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtToAddToAll> outAllColonistsThoughts)
		{
			outIndividualThoughts.Clear();
			outAllColonistsThoughts.Clear();
			if (victim.RaceProps.Humanlike)
			{
				PawnDiedOrDownedThoughtsUtility.AppendThoughts_ForHumanlike(victim, dinfo, thoughtsKind, outIndividualThoughts, outAllColonistsThoughts);
			}
			if (victim.relations != null && victim.relations.everSeenByPlayer)
			{
				PawnDiedOrDownedThoughtsUtility.AppendThoughts_Relations(victim, dinfo, thoughtsKind, outIndividualThoughts, outAllColonistsThoughts);
			}
		}

		// Token: 0x06006E55 RID: 28245 RVA: 0x0021C5AC File Offset: 0x0021A7AC
		public static void BuildMoodThoughtsListString(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, StringBuilder sb, string individualThoughtsHeader, string allColonistsThoughtsHeader)
		{
			PawnDiedOrDownedThoughtsUtility.GetThoughts(victim, dinfo, thoughtsKind, PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd, PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts);
			if (PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Any<ThoughtToAddToAll>())
			{
				if (!allColonistsThoughtsHeader.NullOrEmpty())
				{
					sb.Append(allColonistsThoughtsHeader);
					sb.AppendLine();
				}
				for (int i = 0; i < PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Count; i++)
				{
					ThoughtToAddToAll thoughtToAddToAll = PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts[i];
					if (sb.Length > 0)
					{
						sb.AppendLine();
					}
					sb.Append("  - " + thoughtToAddToAll.thoughtDef.stages[0].LabelCap + " " + Mathf.RoundToInt(thoughtToAddToAll.thoughtDef.stages[0].baseMoodEffect).ToStringWithSign());
				}
			}
			if (PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Any((IndividualThoughtToAdd x) => x.thought.MoodOffset() != 0f))
			{
				if (!individualThoughtsHeader.NullOrEmpty())
				{
					sb.Append(individualThoughtsHeader);
				}
				foreach (IGrouping<Pawn, IndividualThoughtToAdd> grouping in from x in PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd
				where x.thought.MoodOffset() != 0f
				group x by x.addTo)
				{
					if (sb.Length > 0)
					{
						sb.AppendLine();
						sb.AppendLine();
					}
					string value = grouping.Key.KindLabel.CapitalizeFirst() + " " + grouping.Key.LabelShort;
					sb.Append(value);
					sb.Append(":");
					foreach (IndividualThoughtToAdd individualThoughtToAdd in grouping)
					{
						sb.AppendLine();
						sb.Append("    " + individualThoughtToAdd.LabelCap);
					}
				}
			}
		}

		// Token: 0x06006E56 RID: 28246 RVA: 0x0021C7E0 File Offset: 0x0021A9E0
		public static void BuildMoodThoughtsListString(IEnumerable<Pawn> victims, PawnDiedOrDownedThoughtsKind thoughtsKind, StringBuilder sb, string individualThoughtsHeader, string allColonistsThoughtsHeader, string victimLabelKey)
		{
			foreach (Pawn pawn in victims)
			{
				PawnDiedOrDownedThoughtsUtility.GetThoughts(pawn, null, thoughtsKind, PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd, PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts);
				if (PawnDiedOrDownedThoughtsUtility.tmpIndividualThoughtsToAdd.Any<IndividualThoughtToAdd>() || PawnDiedOrDownedThoughtsUtility.tmpAllColonistsThoughts.Any<ThoughtToAddToAll>())
				{
					if (sb.Length > 0)
					{
						sb.AppendLine();
						sb.AppendLine();
					}
					string text = pawn.KindLabel.CapitalizeFirst() + " " + pawn.LabelShort;
					if (victimLabelKey.NullOrEmpty())
					{
						sb.Append(text + ":");
					}
					else
					{
						sb.Append(victimLabelKey.Translate(text));
					}
					PawnDiedOrDownedThoughtsUtility.BuildMoodThoughtsListString(pawn, null, thoughtsKind, sb, individualThoughtsHeader, allColonistsThoughtsHeader);
				}
			}
		}

		// Token: 0x06006E57 RID: 28247 RVA: 0x0021C8D8 File Offset: 0x0021AAD8
		private static void AppendThoughts_ForHumanlike(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtToAddToAll> outAllColonistsThoughts)
		{
			bool flag = dinfo != null && dinfo.Value.Def.execution;
			bool flag2 = victim.IsPrisonerOfColony && !victim.guilt.IsGuilty && !victim.InAggroMentalState;
			if (dinfo != null && dinfo.Value.Def.ExternalViolenceFor(victim) && dinfo.Value.Instigator != null && dinfo.Value.Instigator is Pawn)
			{
				Pawn pawn = (Pawn)dinfo.Value.Instigator;
				if (!pawn.Dead && pawn.needs.mood != null && pawn.story != null && pawn != victim)
				{
					if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died)
					{
						outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KilledHumanlikeBloodlust, pawn, null, 1f, 1f));
					}
					if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && victim.HostileTo(pawn) && victim.Faction != null && PawnUtility.IsFactionLeader(victim) && victim.Faction.HostileTo(pawn.Faction))
					{
						outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.DefeatedHostileFactionLeader, pawn, victim, 1f, 1f));
					}
				}
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && !flag)
			{
				foreach (Pawn pawn2 in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
				{
					if (pawn2 != victim && pawn2.needs != null && pawn2.needs.mood != null && (pawn2.MentalStateDef != MentalStateDefOf.SocialFighting || ((MentalState_SocialFighting)pawn2.MentalState).otherPawn != victim))
					{
						if (PawnDiedOrDownedThoughtsUtility.Witnessed(pawn2, victim))
						{
							bool flag3 = pawn2.Faction == Faction.OfPlayer && victim.IsQuestLodger();
							if (pawn2.Faction == victim.Faction && !flag3)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathAlly, pawn2, null, 1f, 1f));
							}
							else if (victim.Faction == null || !victim.Faction.HostileTo(pawn2.Faction) || flag3)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathNonAlly, pawn2, null, 1f, 1f));
							}
							if (pawn2.relations.FamilyByBlood.Contains(victim))
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathFamily, pawn2, null, 1f, 1f));
							}
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathBloodlust, pawn2, null, 1f, 1f));
						}
						else if (victim.Faction == Faction.OfPlayer && victim.Faction == pawn2.Faction && victim.HostFaction != pawn2.Faction && !victim.IsQuestLodger())
						{
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KnowColonistDied, pawn2, victim, 1f, 1f));
						}
						if (flag2 && pawn2.Faction == Faction.OfPlayer && !pawn2.IsPrisoner)
						{
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KnowPrisonerDiedInnocent, pawn2, victim, 1f, 1f));
						}
					}
				}
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Banished && victim.IsColonist)
			{
				outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.ColonistBanished, victim));
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.BanishedToDie)
			{
				if (victim.IsColonist)
				{
					outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.ColonistBanishedToDie, victim));
				}
				else if (victim.IsPrisonerOfColony)
				{
					outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.PrisonerBanishedToDie, victim));
				}
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost && victim.IsColonist && !victim.IsQuestLodger())
			{
				outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.ColonistLost, victim));
			}
		}

		// Token: 0x06006E58 RID: 28248 RVA: 0x0021CCB8 File Offset: 0x0021AEB8
		private static void AppendThoughts_Relations(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtToAddToAll> outAllColonistsThoughts)
		{
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Banished && victim.RaceProps.Animal)
			{
				List<DirectPawnRelation> directRelations = victim.relations.DirectRelations;
				for (int i = 0; i < directRelations.Count; i++)
				{
					if (directRelations[i].otherPawn.needs != null && directRelations[i].otherPawn.needs.mood != null && PawnUtility.ShouldGetThoughtAbout(directRelations[i].otherPawn, victim) && directRelations[i].def == PawnRelationDefOf.Bond)
					{
						outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.BondedAnimalBanished, directRelations[i].otherPawn, victim, 1f, 1f));
					}
				}
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died || thoughtsKind == PawnDiedOrDownedThoughtsKind.BanishedToDie || thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost)
			{
				bool flag = thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost;
				foreach (Pawn pawn in victim.relations.PotentiallyRelatedPawns)
				{
					if (pawn.needs != null && pawn.needs.mood != null && PawnUtility.ShouldGetThoughtAbout(pawn, victim))
					{
						PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(victim);
						if (mostImportantRelation != null)
						{
							ThoughtDef thoughtDef = flag ? mostImportantRelation.GetGenderSpecificLostThought(victim) : mostImportantRelation.GetGenderSpecificDiedThought(victim);
							if (thoughtDef != null)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(thoughtDef, pawn, victim, 1f, 1f));
							}
						}
					}
				}
				if (dinfo != null && thoughtsKind != PawnDiedOrDownedThoughtsKind.Lost)
				{
					Pawn pawn2 = dinfo.Value.Instigator as Pawn;
					if (pawn2 != null && pawn2 != victim)
					{
						foreach (Pawn pawn3 in victim.relations.PotentiallyRelatedPawns)
						{
							if (pawn2 != pawn3 && pawn3.needs != null && pawn3.needs.mood != null)
							{
								PawnRelationDef mostImportantRelation2 = pawn3.GetMostImportantRelation(victim);
								if (mostImportantRelation2 != null)
								{
									ThoughtDef genderSpecificKilledThought = mostImportantRelation2.GetGenderSpecificKilledThought(victim);
									if (genderSpecificKilledThought != null)
									{
										outIndividualThoughts.Add(new IndividualThoughtToAdd(genderSpecificKilledThought, pawn3, pawn2, 1f, 1f));
									}
								}
								if (pawn3.RaceProps.IsFlesh)
								{
									int num = pawn3.relations.OpinionOf(victim);
									if (num >= 20)
									{
										outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KilledMyFriend, pawn3, pawn2, 1f, victim.relations.GetFriendDiedThoughtPowerFactor(num)));
									}
									else if (num <= -20)
									{
										outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KilledMyRival, pawn3, pawn2, 1f, victim.relations.GetRivalDiedThoughtPowerFactor(num)));
									}
								}
							}
						}
					}
				}
				ThoughtDef thoughtDef2 = (thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost) ? ThoughtDefOf.PawnWithGoodOpinionLost : ThoughtDefOf.PawnWithGoodOpinionDied;
				ThoughtDef thoughtDef3 = (thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost) ? ThoughtDefOf.PawnWithBadOpinionLost : ThoughtDefOf.PawnWithBadOpinionDied;
				if (victim.RaceProps.Humanlike)
				{
					foreach (Pawn pawn4 in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
					{
						if (pawn4.needs != null && pawn4.RaceProps.IsFlesh && pawn4.needs.mood != null && PawnUtility.ShouldGetThoughtAbout(pawn4, victim))
						{
							int num2 = pawn4.relations.OpinionOf(victim);
							if (num2 >= 20)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(thoughtDef2, pawn4, victim, victim.relations.GetFriendDiedThoughtPowerFactor(num2), 1f));
							}
							else if (num2 <= -20)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(thoughtDef3, pawn4, victim, victim.relations.GetRivalDiedThoughtPowerFactor(num2), 1f));
							}
						}
					}
				}
			}
		}

		// Token: 0x06006E59 RID: 28249 RVA: 0x0021D08C File Offset: 0x0021B28C
		private static bool Witnessed(Pawn p, Pawn victim)
		{
			if (!p.Awake() || !p.health.capacities.CapableOf(PawnCapacityDefOf.Sight))
			{
				return false;
			}
			if (victim.IsCaravanMember())
			{
				return victim.GetCaravan() == p.GetCaravan();
			}
			return victim.Spawned && p.Spawned && p.Position.InHorDistOf(victim.Position, 12f) && GenSight.LineOfSight(victim.Position, p.Position, victim.Map, false, null, 0, 0);
		}

		// Token: 0x06006E5A RID: 28250 RVA: 0x0021D120 File Offset: 0x0021B320
		public static void RemoveDiedThoughts(Pawn pawn)
		{
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_Alive)
			{
				if (pawn2.needs != null && pawn2.needs.mood != null && pawn2 != pawn)
				{
					MemoryThoughtHandler memories = pawn2.needs.mood.thoughts.memories;
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.KnowColonistDied, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.KnowPrisonerDiedInnocent, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.PawnWithGoodOpinionDied, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.PawnWithBadOpinionDied, pawn);
					List<PawnRelationDef> allDefsListForReading = DefDatabase<PawnRelationDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						ThoughtDef genderSpecificDiedThought = allDefsListForReading[i].GetGenderSpecificDiedThought(pawn);
						if (genderSpecificDiedThought != null)
						{
							memories.RemoveMemoriesOfDefWhereOtherPawnIs(genderSpecificDiedThought, pawn);
						}
					}
				}
			}
		}

		// Token: 0x06006E5B RID: 28251 RVA: 0x0021D210 File Offset: 0x0021B410
		public static void RemoveLostThoughts(Pawn pawn)
		{
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_Alive)
			{
				if (pawn2.needs != null && pawn2.needs.mood != null && pawn2 != pawn)
				{
					MemoryThoughtHandler memories = pawn2.needs.mood.thoughts.memories;
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.ColonistLost, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.PawnWithGoodOpinionLost, pawn);
					memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.PawnWithBadOpinionLost, pawn);
					List<PawnRelationDef> allDefsListForReading = DefDatabase<PawnRelationDef>.AllDefsListForReading;
					for (int i = 0; i < allDefsListForReading.Count; i++)
					{
						ThoughtDef genderSpecificLostThought = allDefsListForReading[i].GetGenderSpecificLostThought(pawn);
						if (genderSpecificLostThought != null)
						{
							memories.RemoveMemoriesOfDefWhereOtherPawnIs(genderSpecificLostThought, pawn);
						}
					}
				}
			}
		}

		// Token: 0x06006E5C RID: 28252 RVA: 0x0021D2F0 File Offset: 0x0021B4F0
		public static void RemoveResuedRelativeThought(Pawn pawn)
		{
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_Alive)
			{
				if (pawn2.needs != null && pawn2.needs.mood != null && pawn2 != pawn)
				{
					pawn2.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.RescuedRelative, pawn);
				}
			}
		}

		// Token: 0x040048C1 RID: 18625
		private static List<IndividualThoughtToAdd> tmpIndividualThoughtsToAdd = new List<IndividualThoughtToAdd>();

		// Token: 0x040048C2 RID: 18626
		private static List<ThoughtToAddToAll> tmpAllColonistsThoughts = new List<ThoughtToAddToAll>();
	}
}
