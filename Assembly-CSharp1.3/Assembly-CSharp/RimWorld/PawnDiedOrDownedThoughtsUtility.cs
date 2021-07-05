using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000DB3 RID: 3507
	public static class PawnDiedOrDownedThoughtsUtility
	{
		// Token: 0x06005126 RID: 20774 RVA: 0x001B27E4 File Offset: 0x001B09E4
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
						if ((dinfo == null || !dinfo.Value.Def.execution) && thoughtsKind == PawnDiedOrDownedThoughtsKind.Died && victim.IsPrisonerOfColony)
						{
							Pawn arg = PawnDiedOrDownedThoughtsUtility.FindResponsibleColonist(victim, dinfo);
							if (!victim.guilt.IsGuilty && !victim.InAggroMentalState)
							{
								Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.InnocentPrisonerDied, arg.Named(HistoryEventArgsNames.Doer)), true);
							}
							else
							{
								Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.GuiltyPrisonerDied, arg.Named(HistoryEventArgsNames.Doer)), true);
							}
							Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.PrisonerDied, arg.Named(HistoryEventArgsNames.Doer)), true);
						}
					}
				}
			}
			catch (Exception arg2)
			{
				Log.Error("Could not give thoughts: " + arg2);
			}
		}

		// Token: 0x06005127 RID: 20775 RVA: 0x001B29CC File Offset: 0x001B0BCC
		public static void TryGiveThoughts(IEnumerable<Pawn> victims, PawnDiedOrDownedThoughtsKind thoughtsKind)
		{
			foreach (Pawn victim in victims)
			{
				PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(victim, null, thoughtsKind);
			}
		}

		// Token: 0x06005128 RID: 20776 RVA: 0x001B2A1C File Offset: 0x001B0C1C
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

		// Token: 0x06005129 RID: 20777 RVA: 0x001B2A70 File Offset: 0x001B0C70
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

		// Token: 0x0600512A RID: 20778 RVA: 0x001B2CA4 File Offset: 0x001B0EA4
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

		// Token: 0x0600512B RID: 20779 RVA: 0x001B2D9C File Offset: 0x001B0F9C
		private static void AppendThoughts_ForHumanlike(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtToAddToAll> outAllColonistsThoughts)
		{
			bool flag = dinfo != null && dinfo.Value.Def.execution;
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
						if (ThoughtUtility.Witnessed(pawn2, victim))
						{
							bool flag2 = pawn2.Faction == Faction.OfPlayer && victim.IsQuestLodger();
							bool flag3 = victim.IsSlave && victim.HomeFaction != pawn2.HomeFaction;
							if (pawn2.Faction == victim.Faction && !flag2 && !flag3)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathAlly, pawn2, null, 1f, 1f));
							}
							else if (victim.Faction == null || !victim.Faction.HostileTo(pawn2.Faction) || flag2 || flag3)
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathNonAlly, pawn2, null, 1f, 1f));
							}
							if (pawn2.relations.FamilyByBlood.Contains(victim))
							{
								outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathFamily, pawn2, null, 1f, 1f));
							}
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.WitnessedDeathBloodlust, pawn2, null, 1f, 1f));
						}
						else if (victim.Faction == Faction.OfPlayer && victim.Faction == pawn2.Faction && victim.HostFaction != pawn2.Faction && !victim.IsQuestLodger() && !victim.IsSlave)
						{
							outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KnowColonistDied, pawn2, victim, 1f, 1f));
						}
					}
				}
			}
			if (victim.guilt == null || !victim.guilt.IsGuilty)
			{
				if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Banished && victim.IsColonist && !victim.IsSlave)
				{
					outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.ColonistBanished, victim));
				}
				if (thoughtsKind == PawnDiedOrDownedThoughtsKind.DeniedJoining)
				{
					outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.DeniedJoining, victim));
				}
				if (thoughtsKind == PawnDiedOrDownedThoughtsKind.BanishedToDie)
				{
					if (victim.IsColonist && !victim.IsSlave)
					{
						outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.ColonistBanishedToDie, victim));
					}
					else if (victim.IsPrisonerOfColony || victim.IsSlaveOfColony)
					{
						outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.PrisonerBanishedToDie, victim));
					}
				}
				if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost && victim.IsColonist && !victim.IsQuestLodger() && !victim.IsSlave)
				{
					outAllColonistsThoughts.Add(new ThoughtToAddToAll(ThoughtDefOf.ColonistLost, victim));
				}
			}
		}

		// Token: 0x0600512C RID: 20780 RVA: 0x001B31A0 File Offset: 0x001B13A0
		private static void AppendThoughts_Relations(Pawn victim, DamageInfo? dinfo, PawnDiedOrDownedThoughtsKind thoughtsKind, List<IndividualThoughtToAdd> outIndividualThoughts, List<ThoughtToAddToAll> outAllColonistsThoughts)
		{
			PawnDiedOrDownedThoughtsUtility.<>c__DisplayClass8_0 CS$<>8__locals1;
			CS$<>8__locals1.victim = victim;
			CS$<>8__locals1.outIndividualThoughts = outIndividualThoughts;
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Banished && CS$<>8__locals1.victim.RaceProps.Animal)
			{
				PawnDiedOrDownedThoughtsUtility.<AppendThoughts_Relations>g__GiveThoughtsForAnimalBond|8_0(ThoughtDefOf.BondedAnimalBanished, ref CS$<>8__locals1);
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.ReleasedToWild && CS$<>8__locals1.victim.RaceProps.Animal)
			{
				PawnDiedOrDownedThoughtsUtility.<AppendThoughts_Relations>g__GiveThoughtsForAnimalBond|8_0(ThoughtDefOf.BondedAnimalReleased, ref CS$<>8__locals1);
			}
			if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Died || thoughtsKind == PawnDiedOrDownedThoughtsKind.BanishedToDie || thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost)
			{
				foreach (Pawn pawn in CS$<>8__locals1.victim.relations.PotentiallyRelatedPawns)
				{
					if (pawn.needs != null && pawn.needs.mood != null && PawnUtility.ShouldGetThoughtAbout(pawn, CS$<>8__locals1.victim))
					{
						PawnRelationDef mostImportantRelation = pawn.GetMostImportantRelation(CS$<>8__locals1.victim);
						if (mostImportantRelation != null)
						{
							ThoughtDef genderSpecificThought = mostImportantRelation.GetGenderSpecificThought(CS$<>8__locals1.victim, thoughtsKind);
							if (genderSpecificThought != null)
							{
								CS$<>8__locals1.outIndividualThoughts.Add(new IndividualThoughtToAdd(genderSpecificThought, pawn, CS$<>8__locals1.victim, 1f, 1f));
							}
						}
					}
				}
				if (dinfo != null && thoughtsKind != PawnDiedOrDownedThoughtsKind.Lost)
				{
					Pawn pawn2 = dinfo.Value.Instigator as Pawn;
					if (pawn2 != null && pawn2 != CS$<>8__locals1.victim)
					{
						foreach (Pawn pawn3 in CS$<>8__locals1.victim.relations.PotentiallyRelatedPawns)
						{
							if (pawn2 != pawn3 && pawn3.needs != null && pawn3.needs.mood != null)
							{
								PawnRelationDef mostImportantRelation2 = pawn3.GetMostImportantRelation(CS$<>8__locals1.victim);
								if (mostImportantRelation2 != null)
								{
									ThoughtDef genderSpecificKilledThought = mostImportantRelation2.GetGenderSpecificKilledThought(CS$<>8__locals1.victim);
									if (genderSpecificKilledThought != null)
									{
										CS$<>8__locals1.outIndividualThoughts.Add(new IndividualThoughtToAdd(genderSpecificKilledThought, pawn3, pawn2, 1f, 1f));
									}
								}
								if (pawn3.RaceProps.IsFlesh)
								{
									int num = pawn3.relations.OpinionOf(CS$<>8__locals1.victim);
									if (num >= 20)
									{
										CS$<>8__locals1.outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KilledMyFriend, pawn3, pawn2, 1f, CS$<>8__locals1.victim.relations.GetFriendDiedThoughtPowerFactor(num)));
									}
									else if (num <= -20)
									{
										CS$<>8__locals1.outIndividualThoughts.Add(new IndividualThoughtToAdd(ThoughtDefOf.KilledMyRival, pawn3, pawn2, 1f, CS$<>8__locals1.victim.relations.GetRivalDiedThoughtPowerFactor(num)));
									}
								}
							}
						}
					}
				}
				if (CS$<>8__locals1.victim.RaceProps.Humanlike)
				{
					ThoughtDef thoughtDef;
					ThoughtDef thoughtDef2;
					if (thoughtsKind == PawnDiedOrDownedThoughtsKind.Lost)
					{
						thoughtDef = ThoughtDefOf.PawnWithGoodOpinionLost;
						thoughtDef2 = ThoughtDefOf.PawnWithBadOpinionLost;
					}
					else
					{
						thoughtDef = ThoughtDefOf.PawnWithGoodOpinionDied;
						thoughtDef2 = ThoughtDefOf.PawnWithBadOpinionDied;
					}
					foreach (Pawn pawn4 in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
					{
						if (pawn4.needs != null && pawn4.RaceProps.IsFlesh && pawn4.needs.mood != null && PawnUtility.ShouldGetThoughtAbout(pawn4, CS$<>8__locals1.victim))
						{
							int num2 = pawn4.relations.OpinionOf(CS$<>8__locals1.victim);
							if (num2 >= 20)
							{
								CS$<>8__locals1.outIndividualThoughts.Add(new IndividualThoughtToAdd(thoughtDef, pawn4, CS$<>8__locals1.victim, CS$<>8__locals1.victim.relations.GetFriendDiedThoughtPowerFactor(num2), 1f));
							}
							else if (num2 <= -20)
							{
								CS$<>8__locals1.outIndividualThoughts.Add(new IndividualThoughtToAdd(thoughtDef2, pawn4, CS$<>8__locals1.victim, CS$<>8__locals1.victim.relations.GetRivalDiedThoughtPowerFactor(num2), 1f));
							}
						}
					}
				}
			}
		}

		// Token: 0x0600512D RID: 20781 RVA: 0x001B35A0 File Offset: 0x001B17A0
		private static Pawn FindResponsibleColonist(Pawn victim, DamageInfo? dinfo)
		{
			Pawn pawn;
			if (dinfo != null && (pawn = (dinfo.Value.Instigator as Pawn)) != null && pawn.IsColonist)
			{
				return pawn;
			}
			if (victim.Spawned)
			{
				if (victim.Map.mapPawns.FreeColonistsSpawned.Any((Pawn x) => !x.Downed))
				{
					return (from x in victim.Map.mapPawns.FreeColonistsSpawned
					where !x.Downed
					select x).MinBy((Pawn x) => x.Position.DistanceToSquared(victim.Position));
				}
				if (victim.Map.mapPawns.FreeColonistsSpawned.Any<Pawn>())
				{
					return victim.Map.mapPawns.FreeColonistsSpawned.MinBy((Pawn x) => x.Position.DistanceToSquared(victim.Position));
				}
			}
			List<Pawn> allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists = PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_FreeColonists;
			if (allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.Any<Pawn>())
			{
				return allMapsCaravansAndTravelingTransportPods_Alive_FreeColonists.First<Pawn>();
			}
			return null;
		}

		// Token: 0x0600512E RID: 20782 RVA: 0x001B36D4 File Offset: 0x001B18D4
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

		// Token: 0x0600512F RID: 20783 RVA: 0x001B37C4 File Offset: 0x001B19C4
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

		// Token: 0x06005130 RID: 20784 RVA: 0x001B38A4 File Offset: 0x001B1AA4
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

		// Token: 0x06005131 RID: 20785 RVA: 0x001B3928 File Offset: 0x001B1B28
		public static void GiveVeneratedAnimalDiedThoughts(Pawn victim, Map map)
		{
			if (ModsConfig.IdeologyActive && victim.Faction != null && map != null)
			{
				foreach (Pawn pawn in map.mapPawns.PawnsInFaction(victim.Faction))
				{
					if (pawn != victim && pawn.Ideo != null)
					{
						Pawn_NeedsTracker needs = pawn.needs;
						bool flag;
						if (needs == null)
						{
							flag = (null != null);
						}
						else
						{
							Need_Mood mood = needs.mood;
							flag = (((mood != null) ? mood.thoughts : null) != null);
						}
						if (flag && pawn.Ideo.IsVeneratedAnimal(victim))
						{
							Thought_TameVeneratedAnimalDied thought_TameVeneratedAnimalDied = (Thought_TameVeneratedAnimalDied)ThoughtMaker.MakeThought(ThoughtDefOf.TameVeneratedAnimalDied);
							thought_TameVeneratedAnimalDied.animalKindLabel = victim.KindLabel;
							foreach (Precept precept in pawn.Ideo.PreceptsListForReading)
							{
								Precept_Animal precept_Animal;
								if ((precept_Animal = (precept as Precept_Animal)) != null && precept_Animal.ThingDef == victim.def)
								{
									thought_TameVeneratedAnimalDied.sourcePrecept = precept;
									break;
								}
							}
							pawn.needs.mood.thoughts.memories.TryGainMemory(thought_TameVeneratedAnimalDied, null);
						}
					}
				}
			}
		}

		// Token: 0x06005133 RID: 20787 RVA: 0x001B3A9C File Offset: 0x001B1C9C
		[CompilerGenerated]
		internal static void <AppendThoughts_Relations>g__GiveThoughtsForAnimalBond|8_0(ThoughtDef thoughtDef, ref PawnDiedOrDownedThoughtsUtility.<>c__DisplayClass8_0 A_1)
		{
			List<DirectPawnRelation> directRelations = A_1.victim.relations.DirectRelations;
			for (int i = 0; i < directRelations.Count; i++)
			{
				if (directRelations[i].otherPawn.needs != null && directRelations[i].otherPawn.needs.mood != null && PawnUtility.ShouldGetThoughtAbout(directRelations[i].otherPawn, A_1.victim) && directRelations[i].def == PawnRelationDefOf.Bond)
				{
					A_1.outIndividualThoughts.Add(new IndividualThoughtToAdd(thoughtDef, directRelations[i].otherPawn, A_1.victim, 1f, 1f));
				}
			}
		}

		// Token: 0x04003012 RID: 12306
		private static List<IndividualThoughtToAdd> tmpIndividualThoughtsToAdd = new List<IndividualThoughtToAdd>();

		// Token: 0x04003013 RID: 12307
		private static List<ThoughtToAddToAll> tmpAllColonistsThoughts = new List<ThoughtToAddToAll>();
	}
}
