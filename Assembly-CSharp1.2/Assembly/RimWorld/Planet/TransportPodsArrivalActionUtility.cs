using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.QuestGen;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200217E RID: 8574
	public static class TransportPodsArrivalActionUtility
	{
		// Token: 0x0600B6B3 RID: 46771 RVA: 0x0007685B File Offset: 0x00074A5B
		public static IEnumerable<FloatMenuOption> GetFloatMenuOptions<T>(Func<FloatMenuAcceptanceReport> acceptanceReportGetter, Func<T> arrivalActionGetter, string label, CompLaunchable representative, int destinationTile, Action<Action> uiConfirmationCallback = null) where T : TransportPodsArrivalAction
		{
			FloatMenuAcceptanceReport floatMenuAcceptanceReport = acceptanceReportGetter();
			if (floatMenuAcceptanceReport.Accepted || !floatMenuAcceptanceReport.FailReason.NullOrEmpty() || !floatMenuAcceptanceReport.FailMessage.NullOrEmpty())
			{
				if (!floatMenuAcceptanceReport.FailReason.NullOrEmpty())
				{
					yield return new FloatMenuOption(label + " (" + floatMenuAcceptanceReport.FailReason + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
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
					}, MenuOptionPriority.Default, null, null, 0f, null, null);
				}
			}
			yield break;
		}

		// Token: 0x0600B6B4 RID: 46772 RVA: 0x00076890 File Offset: 0x00074A90
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
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			yield break;
		}

		// Token: 0x0600B6B5 RID: 46773 RVA: 0x0034C8F8 File Offset: 0x0034AAF8
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

		// Token: 0x0600B6B6 RID: 46774 RVA: 0x0034C978 File Offset: 0x0034AB78
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

		// Token: 0x0600B6B7 RID: 46775 RVA: 0x0034C9F4 File Offset: 0x0034ABF4
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

		// Token: 0x0600B6B8 RID: 46776 RVA: 0x0034CA7C File Offset: 0x0034AC7C
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

		// Token: 0x0600B6B9 RID: 46777 RVA: 0x0034CAC4 File Offset: 0x0034ACC4
		public static Thing DropShuttle_NewTemp(List<ActiveDropPodInfo> pods, Map map, IntVec3 cell, Faction faction = null)
		{
			TransportPodsArrivalActionUtility.RemovePawnsFromWorldPawns(pods);
			Thing thing = QuestGen_Shuttle.GenerateShuttle(faction, null, null, false, false, false, 0, false, false, true, false, null, null, -1, null, false, true, null);
			thing.TryGetComp<CompShuttle>().hideControls = true;
			CompTransporter compTransporter = thing.TryGetComp<CompTransporter>();
			for (int i = 0; i < pods.Count; i++)
			{
				compTransporter.innerContainer.TryAddRangeOrTransfer(pods[i].innerContainer, true, false);
			}
			if (!cell.IsValid)
			{
				Thing thing2;
				cell = DropCellFinder.GetBestShuttleLandingSpot(map, Faction.OfPlayer, out thing2);
			}
			GenPlace.TryPlaceThing(SkyfallerMaker.MakeSkyfaller(ThingDefOf.ShuttleIncoming, thing), cell, map, ThingPlaceMode.Near, null, null, default(Rot4));
			return thing;
		}

		// Token: 0x0600B6BA RID: 46778 RVA: 0x000768BD File Offset: 0x00074ABD
		[Obsolete("Will be removed in the future, replaced with DropShuttle_NewTemp")]
		public static void DropShuttle(List<ActiveDropPodInfo> pods, Map map, IntVec3 cell, Faction faction = null)
		{
			TransportPodsArrivalActionUtility.DropShuttle_NewTemp(pods, map, cell, faction);
		}

		// Token: 0x0600B6BB RID: 46779 RVA: 0x0034CB68 File Offset: 0x0034AD68
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
