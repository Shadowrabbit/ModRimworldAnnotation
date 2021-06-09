using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001885 RID: 6277
	public static class TransporterUtility
	{
		// Token: 0x06008B63 RID: 35683 RVA: 0x00289AD4 File Offset: 0x00287CD4
		public static void GetTransportersInGroup(int transportersGroup, Map map, List<CompTransporter> outTransporters)
		{
			outTransporters.Clear();
			if (transportersGroup < 0)
			{
				return;
			}
			List<Thing> list = map.listerThings.ThingsInGroup(ThingRequestGroup.Transporter);
			for (int i = 0; i < list.Count; i++)
			{
				CompTransporter compTransporter = list[i].TryGetComp<CompTransporter>();
				if (compTransporter.groupID == transportersGroup)
				{
					outTransporters.Add(compTransporter);
				}
			}
		}

		// Token: 0x06008B64 RID: 35684 RVA: 0x00289B28 File Offset: 0x00287D28
		public static Lord FindLord(int transportersGroup, Map map)
		{
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				LordJob_LoadAndEnterTransporters lordJob_LoadAndEnterTransporters = lords[i].LordJob as LordJob_LoadAndEnterTransporters;
				if (lordJob_LoadAndEnterTransporters != null && lordJob_LoadAndEnterTransporters.transportersGroup == transportersGroup)
				{
					return lords[i];
				}
			}
			return null;
		}

		// Token: 0x06008B65 RID: 35685 RVA: 0x00289B7C File Offset: 0x00287D7C
		public static bool WasLoadingCanceled(Thing transporter)
		{
			CompTransporter compTransporter = transporter.TryGetComp<CompTransporter>();
			return compTransporter != null && !compTransporter.LoadingInProgressOrReadyToLaunch;
		}

		// Token: 0x06008B66 RID: 35686 RVA: 0x00289BA0 File Offset: 0x00287DA0
		public static int InitiateLoading(IEnumerable<CompTransporter> transporters)
		{
			int nextTransporterGroupID = Find.UniqueIDsManager.GetNextTransporterGroupID();
			foreach (CompTransporter compTransporter in transporters)
			{
				compTransporter.groupID = nextTransporterGroupID;
			}
			return nextTransporterGroupID;
		}

		// Token: 0x06008B67 RID: 35687 RVA: 0x0005D7CD File Offset: 0x0005B9CD
		[Obsolete("Only used for mod compatibility. Will be removed in future version.")]
		public static IEnumerable<Pawn> AllSendablePawns(List<CompTransporter> transporters, Map map)
		{
			return TransporterUtility.AllSendablePawns_NewTmp(transporters, map, false);
		}

		// Token: 0x06008B68 RID: 35688 RVA: 0x0005D7D7 File Offset: 0x0005B9D7
		public static IEnumerable<Pawn> AllSendablePawns_NewTmp(List<CompTransporter> transporters, Map map, bool autoLoot)
		{
			CompShuttle shuttle = transporters[0].parent.TryGetComp<CompShuttle>();
			bool allowEvenIfDowned = true;
			int num = (transporters[0].Props.canChangeAssignedThingsAfterStarting && transporters[0].LoadingInProgressOrReadyToLaunch) ? transporters[0].groupID : -1;
			bool allowLodgers = shuttle != null;
			List<Pawn> pawns = CaravanFormingUtility.AllSendablePawns(map, allowEvenIfDowned, autoLoot, autoLoot, autoLoot, allowLodgers, num);
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (shuttle == null || shuttle.IsRequired(pawns[i]) || shuttle.IsAllowed(pawns[i]))
				{
					yield return pawns[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06008B69 RID: 35689 RVA: 0x0005D7F5 File Offset: 0x0005B9F5
		[Obsolete("Only used for mod compatibility. Will be removed in future version.")]
		public static IEnumerable<Thing> AllSendableItems(List<CompTransporter> transporters, Map map)
		{
			return TransporterUtility.AllSendableItems_NewTmp(transporters, map, false);
		}

		// Token: 0x06008B6A RID: 35690 RVA: 0x0005D7FF File Offset: 0x0005B9FF
		public static IEnumerable<Thing> AllSendableItems_NewTmp(List<CompTransporter> transporters, Map map, bool autoLoot)
		{
			List<Thing> items = CaravanFormingUtility.AllReachableColonyItems(map, autoLoot, transporters[0].Props.canChangeAssignedThingsAfterStarting && transporters[0].LoadingInProgressOrReadyToLaunch, autoLoot);
			CompShuttle shuttle = transporters[0].parent.TryGetComp<CompShuttle>();
			int num;
			for (int i = 0; i < items.Count; i = num + 1)
			{
				if (shuttle == null || shuttle.IsRequired(items[i]) || shuttle.IsAllowed(items[i]))
				{
					yield return items[i];
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06008B6B RID: 35691 RVA: 0x0005D81D File Offset: 0x0005BA1D
		public static IEnumerable<Thing> ThingsBeingHauledTo(List<CompTransporter> transporters, Map map)
		{
			List<Pawn> pawns = map.mapPawns.AllPawnsSpawned;
			int num;
			for (int i = 0; i < pawns.Count; i = num + 1)
			{
				if (pawns[i].CurJobDef == JobDefOf.HaulToTransporter && transporters.Contains(((JobDriver_HaulToTransporter)pawns[i].jobs.curDriver).Transporter) && pawns[i].carryTracker.CarriedThing != null)
				{
					yield return pawns[i].carryTracker.CarriedThing;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06008B6C RID: 35692 RVA: 0x00289BF4 File Offset: 0x00287DF4
		public static void MakeLordsAsAppropriate(List<Pawn> pawns, List<CompTransporter> transporters, Map map)
		{
			int groupID = transporters[0].groupID;
			Lord lord = null;
			IEnumerable<Pawn> enumerable = from x in pawns
			where x.IsColonist && !x.Downed && x.Spawned
			select x;
			if (enumerable.Any<Pawn>())
			{
				lord = map.lordManager.lords.Find((Lord x) => x.LordJob is LordJob_LoadAndEnterTransporters && ((LordJob_LoadAndEnterTransporters)x.LordJob).transportersGroup == groupID);
				if (lord == null)
				{
					lord = LordMaker.MakeNewLord(Faction.OfPlayer, new LordJob_LoadAndEnterTransporters(groupID), map, null);
				}
				foreach (Pawn pawn in enumerable)
				{
					if (!lord.ownedPawns.Contains(pawn))
					{
						Lord lord2 = pawn.GetLord();
						if (lord2 != null)
						{
							lord2.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord, null);
						}
						lord.AddPawn(pawn);
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
				for (int i = lord.ownedPawns.Count - 1; i >= 0; i--)
				{
					if (!enumerable.Contains(lord.ownedPawns[i]))
					{
						lord.Notify_PawnLost(lord.ownedPawns[i], PawnLostCondition.NoLongerEnteringTransportPods, null);
					}
				}
			}
			for (int j = map.lordManager.lords.Count - 1; j >= 0; j--)
			{
				LordJob_LoadAndEnterTransporters lordJob_LoadAndEnterTransporters = map.lordManager.lords[j].LordJob as LordJob_LoadAndEnterTransporters;
				if (lordJob_LoadAndEnterTransporters != null && lordJob_LoadAndEnterTransporters.transportersGroup == groupID && map.lordManager.lords[j] != lord)
				{
					map.lordManager.RemoveLord(map.lordManager.lords[j]);
				}
			}
		}
	}
}
