using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017E2 RID: 6114
	public static class TransportPodsArrivalActionUtility
	{
		// Token: 0x06008E4E RID: 36430 RVA: 0x00331B59 File Offset: 0x0032FD59
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, CompLaunchable representative, int destinationTile, Action<Action> uiConfirmationCallback = null) where T : TransportPodsArrivalAction
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
			if (floatMenuAcceptanceReport.Accepted || !floatMenuAcceptanceReport.FailReason.NullOrEmpty() || !floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
			{
				if (!floatMenuAcceptanceReport.FailReason.NullOrEmpty())
				{
					yield return new FloatMenuOption(label + " (" + floatMenuAcceptanceReport.FailReason + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
				else
				{
					Action <>9__1;
					yield return new FloatMenuOption(label, delegate()
					{
						FloatMenuAcceptanceReport floatMenuAcceptanceReport2 = acceptanceReportGetter();
						if (!floatMenuAcceptanceReport2.Accepted)
						{
							if (!floatMenuAcceptanceReport2.FailMessage.NullOrEmpty())
							{
								Messages.Message(floatMenuAcceptanceReport2.FailMessage, new GlobalTargetInfo(destinationTile), MessageTypeDefOf.RejectInput, false);
							}
							return;
						}
						if (uiConfirmationCallback == null)
						{
							representative.TryLaunch(destinationTile, arrivalActionGetter());
							return;
						}
						Action<Action> uiConfirmationCallback2 = uiConfirmationCallback;
						Action obj;
						if ((obj = <>9__1) == null)
						{
							obj = (<>9__1 = delegate()
							{
								representative.TryLaunch(destinationTile, arrivalActionGetter());
							});
						}
						uiConfirmationCallback2(obj);
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				}
			}
			yield break;
		}

		// Token: 0x06008E4F RID: 36431 RVA: 0x00331B8E File Offset: 0x0032FD8E
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, Action<int, TransportPodsArrivalAction> launchAction, int destinationTile) where T : TransportPodsArrivalAction
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
			if (floatMenuAcceptanceReport.Accepted || !floatMenuAcceptanceReport.FailReason.NullOrEmpty() || !floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
			{
				if (!floatMenuAcceptanceReport.Accepted && !floatMenuAcceptanceReport.FailReason.NullOrEmpty())
				{
					label = label + " (" + floatMenuAcceptanceReport.FailReason + ")";
				}
				yield return new FloatMenuOption(label, delegate()
				{
					FloatMenuAcceptanceReport floatMenuAcceptanceReport2 = acceptanceReportGetter();
					if (floatMenuAcceptanceReport2.Accepted)
					{
						launchAction(destinationTile, arrivalActionGetter());
						return;
					}
					if (!floatMenuAcceptanceReport2.FailMessage.NullOrEmpty())
					{
						Messages.Message(floatMenuAcceptanceReport2.FailMessage, new GlobalTargetInfo(destinationTile), MessageTypeDefOf.RejectInput, false);
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			}
			yield break;
		}

		// Token: 0x06008E50 RID: 36432 RVA: 0x00331BBC File Offset: 0x0032FDBC
		public static bool AnyNonDownedColonist(IEnumerable<IThingHolder> pods)
		{
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					Pawn pawn = directlyHeldThings[i] as Pawn;
					if (pawn != null && pawn.IsColonist && !pawn.Downed)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008E51 RID: 36433 RVA: 0x00331C3C File Offset: 0x0032FE3C
		public static bool AnyPotentialCaravanOwner(IEnumerable<IThingHolder> pods, Faction faction)
		{
			foreach (IThingHolder thingHolder in pods)
			{
				ThingOwner directlyHeldThings = thingHolder.GetDirectlyHeldThings();
				for (int i = 0; i < directlyHeldThings.Count; i++)
				{
					Pawn pawn = directlyHeldThings[i] as Pawn;
					if (pawn != null && CaravanUtility.IsOwner(pawn, faction))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06008E52 RID: 36434 RVA: 0x00331CB8 File Offset: 0x0032FEB8
		public static Thing GetLookTarget(List<ActiveDropPodInfo> pods)
		{
			for (int i = 0; i < pods.Count; i++)
			{
				ThingOwner directlyHeldThings = pods[i].GetDirectlyHeldThings();
				for (int j = 0; j < directlyHeldThings.Count; j++)
				{
					Pawn pawn = directlyHeldThings[j] as Pawn;
					if (pawn != null && pawn.IsColonist)
					{
						return pawn;
					}
				}
			}
			for (int k = 0; k < pods.Count; k++)
			{
				Thing thing = pods[k].GetDirectlyHeldThings().FirstOrDefault<Thing>();
				if (thing != null)
				{
					return thing;
				}
			}
			return null;
		}

		// Token: 0x06008E53 RID: 36435 RVA: 0x00331D40 File Offset: 0x0032FF40
		public static void DropTravelingTransportPods(List<ActiveDropPodInfo> dropPods, IntVec3 near, Map map)
		{
			TransportPodsArrivalActionUtility.RemovePawnsFromWorldPawns(dropPods);
			for (int i = 0; i < dropPods.Count; i++)
			{
				IntVec3 c;
				DropCellFinder.TryFindDropSpotNear(near, map, out c, false, true, true, null);
				DropPodUtility.MakeDropPodAt(c, map, dropPods[i]);
			}
		}

		// Token: 0x06008E54 RID: 36436 RVA: 0x00331D88 File Offset: 0x0032FF88
		public static Thing DropShuttle(List<ActiveDropPodInfo> pods, Map map, IntVec3 cell, Faction faction = null)
		{
			TransportPodsArrivalActionUtility.RemovePawnsFromWorldPawns(pods);
			Thing thing = QuestGen_Shuttle.GenerateShuttle(faction, null, null, false, false, false, 0, false, false, true, false, null, null, -1, null, false, true, false, false);
			TransportShip transportShip = TransportShipMaker.MakeTransportShip(TransportShipDefOf.Ship_Shuttle, null, thing);
			CompTransporter compTransporter = thing.TryGetComp<CompTransporter>();
			for (int i = 0; i < pods.Count; i++)
			{
				compTransporter.innerContainer.TryAddRangeOrTransfer(pods[i].innerContainer, true, false);
			}
			if (!cell.IsValid)
			{
				cell = DropCellFinder.GetBestShuttleLandingSpot(map, Faction.OfPlayer);
			}
			transportShip.ArriveAt(cell, map.Parent);
			transportShip.AddJobs(new ShipJobDef[]
			{
				ShipJobDefOf.Unload,
				ShipJobDefOf.FlyAway
			});
			return thing;
		}

		// Token: 0x06008E55 RID: 36437 RVA: 0x00331E34 File Offset: 0x00330034
		public static void RemovePawnsFromWorldPawns(List<ActiveDropPodInfo> pods)
		{
			for (int i = 0; i < pods.Count; i++)
			{
				ThingOwner innerContainer = pods[i].innerContainer;
				for (int j = 0; j < innerContainer.Count; j++)
				{
					Pawn pawn = innerContainer[j] as Pawn;
					if (pawn != null && pawn.IsWorldPawn())
					{
						Find.WorldPawns.RemovePawn(pawn);
					}
				}
			}
		}
	}
}
