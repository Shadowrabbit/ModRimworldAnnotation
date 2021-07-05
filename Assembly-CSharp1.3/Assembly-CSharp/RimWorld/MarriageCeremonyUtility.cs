using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000895 RID: 2197
	public static class MarriageCeremonyUtility
	{
		// Token: 0x06003A3C RID: 14908 RVA: 0x00146720 File Offset: 0x00144920
		public static bool AcceptableGameConditionsToStartCeremony(Map map)
		{
			if (!GatheringsUtility.AcceptableGameConditionsToContinueGathering(map))
			{
				return false;
			}
			if (GenLocalDate.HourInteger(map) < 5 || GenLocalDate.HourInteger(map) > 16)
			{
				return false;
			}
			if (GatheringsUtility.AnyLordJobPreventsNewGatherings(map))
			{
				return false;
			}
			if (map.dangerWatcher.DangerRating != StoryDanger.None)
			{
				return false;
			}
			int num = 0;
			using (List<Pawn>.Enumerator enumerator = map.mapPawns.FreeColonistsSpawned.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Drafted)
					{
						num++;
					}
				}
			}
			return (float)num / (float)map.mapPawns.FreeColonistsSpawnedCount < 0.5f;
		}

		// Token: 0x06003A3D RID: 14909 RVA: 0x001467D0 File Offset: 0x001449D0
		public static bool FianceReadyToStartCeremony(Pawn pawn, Pawn otherPawn)
		{
			return MarriageCeremonyUtility.FianceCanContinueCeremony(pawn, otherPawn) && pawn.health.hediffSet.BleedRateTotal <= 0f && !HealthAIUtility.ShouldSeekMedicalRestUrgent(pawn) && !PawnUtility.WillSoonHaveBasicNeed(pawn) && !MarriageCeremonyUtility.IsCurrentlyMarryingSomeone(pawn) && pawn.GetLord() == null && (!pawn.Drafted && !pawn.InMentalState && pawn.Awake() && !pawn.IsBurning()) && !pawn.InBed();
		}

		// Token: 0x06003A3E RID: 14910 RVA: 0x00146854 File Offset: 0x00144A54
		public static bool FianceCanContinueCeremony(Pawn pawn, Pawn otherPawn)
		{
			return GatheringsUtility.PawnCanStartOrContinueGathering(pawn) && !pawn.HostileTo(otherPawn) && (pawn.Spawned && !pawn.Downed) && !pawn.InMentalState;
		}

		// Token: 0x06003A3F RID: 14911 RVA: 0x00146886 File Offset: 0x00144A86
		public static bool ShouldGuestKeepAttendingCeremony(Pawn p)
		{
			return GatheringsUtility.ShouldGuestKeepAttendingGathering(p);
		}

		// Token: 0x06003A40 RID: 14912 RVA: 0x00146890 File Offset: 0x00144A90
		public static void Married(Pawn firstPawn, Pawn secondPawn)
		{
			LovePartnerRelationUtility.ChangeSpouseRelationsToExSpouse(firstPawn);
			LovePartnerRelationUtility.ChangeSpouseRelationsToExSpouse(secondPawn);
			firstPawn.relations.RemoveDirectRelation(PawnRelationDefOf.Fiance, secondPawn);
			firstPawn.relations.TryRemoveDirectRelation(PawnRelationDefOf.ExSpouse, secondPawn);
			firstPawn.relations.AddDirectRelation(PawnRelationDefOf.Spouse, secondPawn);
			MarriageCeremonyUtility.AddNewlyMarriedThoughts(firstPawn, secondPawn);
			MarriageCeremonyUtility.AddNewlyMarriedThoughts(secondPawn, firstPawn);
			if (firstPawn.needs.mood != null)
			{
				firstPawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.DivorcedMe, secondPawn);
			}
			if (secondPawn.needs.mood != null)
			{
				secondPawn.needs.mood.thoughts.memories.RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDefOf.DivorcedMe, firstPawn);
			}
			if (firstPawn.relations.nextMarriageNameChange != secondPawn.relations.nextMarriageNameChange)
			{
				Log.Warning("Marriage name change is different on marrying pawns. This is weird, but not harmful.");
			}
			SpouseRelationUtility.ChangeNameAfterMarriage(firstPawn, secondPawn, firstPawn.relations.nextMarriageNameChange);
			LovePartnerRelationUtility.TryToShareBed(firstPawn, secondPawn);
			TaleRecorder.RecordTale(TaleDefOf.Marriage, new object[]
			{
				firstPawn,
				secondPawn
			});
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(firstPawn.GetHistoryEventForSpouseCount(), firstPawn.Named(HistoryEventArgsNames.Doer)), true);
			Find.HistoryEventsManager.RecordEvent(new HistoryEvent(secondPawn.GetHistoryEventForSpouseCount(), secondPawn.Named(HistoryEventArgsNames.Doer)), true);
		}

		// Token: 0x06003A41 RID: 14913 RVA: 0x001469DC File Offset: 0x00144BDC
		private static void AddNewlyMarriedThoughts(Pawn pawn, Pawn otherPawn)
		{
			if (pawn.needs.mood == null)
			{
				return;
			}
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.GotMarried, otherPawn, null);
			pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.HoneymoonPhase, otherPawn, null);
		}

		// Token: 0x06003A42 RID: 14914 RVA: 0x00146A3C File Offset: 0x00144C3C
		private static bool IsCurrentlyMarryingSomeone(Pawn p)
		{
			if (!p.Spawned)
			{
				return false;
			}
			List<Lord> lords = p.Map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_Joinable_MarriageCeremony lordJob_Joinable_MarriageCeremony = lords[i].LordJob as LordJob_Joinable_MarriageCeremony;
				if (lordJob_Joinable_MarriageCeremony != null && (lordJob_Joinable_MarriageCeremony.firstPawn == p || lordJob_Joinable_MarriageCeremony.secondPawn == p))
				{
					return true;
				}
			}
			return false;
		}
	}
}
