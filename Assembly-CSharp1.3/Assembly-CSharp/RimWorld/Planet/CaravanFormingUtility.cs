using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld.Planet
{
	// Token: 0x020017A5 RID: 6053
	[StaticConstructorOnStartup]
	public static class CaravanFormingUtility
	{
		// Token: 0x06008C42 RID: 35906 RVA: 0x003255ED File Offset: 0x003237ED
		public static void FormAndCreateCaravan(IEnumerable<Pawn> pawns, Faction faction, int exitFromTile, int directionTile, int destinationTile)
		{
			CaravanExitMapUtility.ExitMapAndCreateCaravan(pawns, faction, exitFromTile, directionTile, destinationTile, true);
		}

		// Token: 0x06008C43 RID: 35907 RVA: 0x003255FC File Offset: 0x003237FC
		public static void StartFormingCaravan(List<Pawn> pawns, List<Pawn> downedPawns, Faction faction, List<TransferableOneWay> transferables, IntVec3 meetingPoint, IntVec3 exitSpot, int startingTile, int destinationTile)
		{
			if (startingTile < 0)
			{
				Log.Error("Can't start forming caravan because startingTile is invalid.");
				return;
			}
			if (!pawns.Any<Pawn>())
			{
				Log.Error("Can't start forming caravan with 0 pawns.");
				return;
			}
			if (pawns.Any((Pawn x) => x.Downed))
			{
				Log.Warning("Forming a caravan with a downed pawn. This shouldn't happen because we have to create a Lord.");
			}
			List<TransferableOneWay> list = transferables.ToList<TransferableOneWay>();
			list.RemoveAll((TransferableOneWay x) => x.CountToTransfer <= 0 || !x.HasAnyThing || x.AnyThing is Pawn);
			for (int i = 0; i < pawns.Count; i++)
			{
				Lord lord = pawns[i].GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawns[i], PawnLostCondition.ForcedToJoinOtherLord, null);
				}
			}
			LordJob_FormAndSendCaravan lordJob = new LordJob_FormAndSendCaravan(list, downedPawns, meetingPoint, exitSpot, startingTile, destinationTile);
			LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, pawns[0].MapHeld, pawns);
			for (int j = 0; j < pawns.Count; j++)
			{
				Pawn pawn = pawns[j];
				if (pawn.Spawned)
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x06008C44 RID: 35908 RVA: 0x00325724 File Offset: 0x00323924
		public static void StopFormingCaravan(Lord lord)
		{
			CaravanFormingUtility.tmpPawnsInCancelledCaravan.Clear();
			CaravanFormingUtility.tmpPawnsInCancelledCaravan.AddRange(lord.ownedPawns);
			bool isPlayerHome = lord.Map.IsPlayerHome;
			CaravanFormingUtility.SetToUnloadEverything(lord);
			lord.lordManager.RemoveLord(lord);
			if (isPlayerHome)
			{
				CaravanFormingUtility.LeadAnimalsToPen(CaravanFormingUtility.tmpPawnsInCancelledCaravan);
			}
			CaravanFormingUtility.tmpPawnsInCancelledCaravan.Clear();
		}

		// Token: 0x06008C45 RID: 35909 RVA: 0x00325780 File Offset: 0x00323980
		public static void LeadAnimalsToPen(List<Pawn> pawns)
		{
			CaravanFormingUtility.tmpRopers.Clear();
			CaravanFormingUtility.tmpNeedRoping.Clear();
			foreach (Pawn pawn in pawns)
			{
				if (pawn.Spawned && !pawn.Downed && !pawn.Dead && !pawn.Drafted)
				{
					if (AnimalPenUtility.NeedsToBeManagedByRope(pawn))
					{
						CaravanFormingUtility.tmpNeedRoping.Add(pawn);
					}
					else if (pawn.IsColonist)
					{
						CaravanFormingUtility.tmpRopers.Add(pawn);
					}
				}
			}
			if (CaravanFormingUtility.tmpNeedRoping.Any<Pawn>() && CaravanFormingUtility.tmpRopers.Any<Pawn>())
			{
				using (List<Pawn>.Enumerator enumerator = CaravanFormingUtility.tmpNeedRoping.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn ropee = enumerator.Current;
						if (!ropee.roping.IsRoped)
						{
							CaravanFormingUtility.tmpRopers.MinBy((Pawn p) => p.Position.DistanceToSquared(ropee.Position)).roping.RopePawn(ropee);
						}
					}
				}
				CaravanFormingUtility.StartReturnedLord((from p in CaravanFormingUtility.tmpRopers
				where p.roping.IsRopingOthers
				select p).Concat(CaravanFormingUtility.tmpNeedRoping).ToList<Pawn>());
			}
			CaravanFormingUtility.tmpRopers.Clear();
			CaravanFormingUtility.tmpNeedRoping.Clear();
		}

		// Token: 0x06008C46 RID: 35910 RVA: 0x00325910 File Offset: 0x00323B10
		private static void StartReturnedLord(List<Pawn> pawns)
		{
			foreach (Pawn pawn in pawns)
			{
				Lord lord = pawn.GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord, null);
				}
			}
			LordJob_ReturnedCaravan lordJob = new LordJob_ReturnedCaravan(pawns[0].Position);
			LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, pawns[0].MapHeld, pawns);
			foreach (Pawn pawn2 in pawns)
			{
				if (pawn2.Spawned)
				{
					pawn2.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x06008C47 RID: 35911 RVA: 0x003259F0 File Offset: 0x00323BF0
		public static void RemovePawnFromCaravan(Pawn pawn, Lord lord, bool removeFromDowned = true)
		{
			bool flag = false;
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				Pawn pawn2 = lord.ownedPawns[i];
				if (pawn2 != pawn && CaravanUtility.IsOwner(pawn2, Faction.OfPlayer))
				{
					flag = true;
					break;
				}
			}
			bool flag2 = true;
			TaggedString taggedString = "MessagePawnLostWhileFormingCaravan".Translate(pawn).CapitalizeFirst();
			if (!flag)
			{
				CaravanFormingUtility.StopFormingCaravan(lord);
				taggedString += " " + "MessagePawnLostWhileFormingCaravan_AllLost".Translate();
			}
			else
			{
				pawn.inventory.UnloadEverything = true;
				if (lord.ownedPawns.Contains(pawn))
				{
					lord.Notify_PawnLost(pawn, PawnLostCondition.ForcedByPlayerAction, null);
					flag2 = false;
				}
				LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lord.LordJob as LordJob_FormAndSendCaravan;
				if (lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.downedPawns.Contains(pawn))
				{
					if (!removeFromDowned)
					{
						flag2 = false;
					}
					else
					{
						lordJob_FormAndSendCaravan.downedPawns.Remove(pawn);
					}
				}
				if (pawn.jobs != null)
				{
					pawn.jobs.EndCurrentJob(JobCondition.Incompletable, true, true);
				}
			}
			if (flag2)
			{
				Messages.Message(taggedString, pawn, MessageTypeDefOf.NegativeEvent, true);
			}
		}

		// Token: 0x06008C48 RID: 35912 RVA: 0x00325B10 File Offset: 0x00323D10
		private static void SetToUnloadEverything(Lord lord)
		{
			for (int i = 0; i < lord.ownedPawns.Count; i++)
			{
				lord.ownedPawns[i].inventory.UnloadEverything = true;
			}
		}

		// Token: 0x06008C49 RID: 35913 RVA: 0x00325B4C File Offset: 0x00323D4C
		public static List<Thing> AllReachableColonyItems(Map map, bool allowEvenIfOutsideHomeArea = false, bool allowEvenIfReserved = false, bool canMinify = false)
		{
			List<Thing> list = new List<Thing>();
			List<Thing> allThings = map.listerThings.AllThings;
			for (int i = 0; i < allThings.Count; i++)
			{
				Thing thing = allThings[i];
				bool flag = canMinify && thing.def.Minifiable;
				if ((flag || thing.def.category == ThingCategory.Item) && (allowEvenIfOutsideHomeArea || map.areaManager.Home[thing.Position] || thing.IsInAnyStorage()) && !thing.Position.Fogged(thing.Map) && (allowEvenIfReserved || !map.reservationManager.IsReservedByAnyoneOf(thing, Faction.OfPlayer)) && (flag || thing.def.EverHaulable) && thing.GetInnerIfMinified().def.canLoadIntoCaravan)
				{
					list.Add(thing);
				}
			}
			return list;
		}

		// Token: 0x06008C4A RID: 35914 RVA: 0x00325C2C File Offset: 0x00323E2C
		public static List<Pawn> AllSendablePawns(Map map, bool allowEvenIfDowned = false, bool allowEvenIfInMentalState = false, bool allowEvenIfPrisonerNotSecure = false, bool allowCapturableDownedPawns = false, bool allowLodgers = false, int allowLoadAndEnterTransportersLordForGroupID = -1)
		{
			List<Pawn> list = new List<Pawn>();
			List<Pawn> allPawnsSpawned = map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn = allPawnsSpawned[i];
				if ((allowEvenIfDowned || !pawn.Downed) && (allowEvenIfInMentalState || !pawn.InMentalState) && (pawn.Faction == Faction.OfPlayer || pawn.IsPrisonerOfColony || (allowCapturableDownedPawns && CaravanFormingUtility.CanListAsAutoCapturable(pawn))) && pawn.RaceProps.allowedOnCaravan && !pawn.IsQuestHelper() && (!pawn.IsQuestLodger() || allowLodgers) && (allowEvenIfPrisonerNotSecure || !pawn.IsPrisoner || pawn.guest.PrisonerIsSecure) && (pawn.GetLord() == null || pawn.GetLord().LordJob is LordJob_VoluntarilyJoinable || pawn.GetLord().LordJob.IsCaravanSendable || (allowLoadAndEnterTransportersLordForGroupID >= 0 && pawn.GetLord().LordJob is LordJob_LoadAndEnterTransporters && ((LordJob_LoadAndEnterTransporters)pawn.GetLord().LordJob).transportersGroup == allowLoadAndEnterTransportersLordForGroupID)))
				{
					list.Add(pawn);
				}
			}
			return list;
		}

		// Token: 0x06008C4B RID: 35915 RVA: 0x00325D58 File Offset: 0x00323F58
		private static bool CanListAsAutoCapturable(Pawn p)
		{
			return p.Downed && !p.mindState.WillJoinColonyIfRescued && CaravanUtility.ShouldAutoCapture(p, Faction.OfPlayer);
		}

		// Token: 0x06008C4C RID: 35916 RVA: 0x00325D7C File Offset: 0x00323F7C
		public static IEnumerable<Gizmo> GetGizmos(Pawn pawn)
		{
			CaravanFormingUtility.<>c__DisplayClass15_0 CS$<>8__locals1 = new CaravanFormingUtility.<>c__DisplayClass15_0();
			CS$<>8__locals1.pawn = pawn;
			if (CaravanFormingUtility.IsFormingCaravanOrDownedPawnToBeTakenByCaravan(CS$<>8__locals1.pawn))
			{
				CaravanFormingUtility.<>c__DisplayClass15_1 CS$<>8__locals2 = new CaravanFormingUtility.<>c__DisplayClass15_1();
				CS$<>8__locals2.CS$<>8__locals1 = CS$<>8__locals1;
				CS$<>8__locals2.lord = CaravanFormingUtility.GetFormAndSendCaravanLord(CS$<>8__locals2.CS$<>8__locals1.pawn);
				yield return new Command_Action
				{
					defaultLabel = "CommandCancelFormingCaravan".Translate(),
					defaultDesc = "CommandCancelFormingCaravanDesc".Translate(),
					icon = TexCommand.ClearPrioritizedWork,
					activateSound = SoundDefOf.Tick_Low,
					action = delegate()
					{
						CaravanFormingUtility.StopFormingCaravan(CS$<>8__locals2.lord);
					},
					hotKey = KeyBindingDefOf.Designator_Cancel
				};
				yield return new Command_Action
				{
					defaultLabel = "CommandRemoveFromCaravan".Translate(),
					defaultDesc = "CommandRemoveFromCaravanDesc".Translate(),
					icon = CaravanFormingUtility.RemoveFromCaravanCommand,
					action = delegate()
					{
						CaravanFormingUtility.RemovePawnFromCaravan(CS$<>8__locals2.CS$<>8__locals1.pawn, CS$<>8__locals2.lord, true);
					},
					hotKey = KeyBindingDefOf.Misc6
				};
				CS$<>8__locals2 = null;
			}
			else if (CS$<>8__locals1.pawn.Spawned)
			{
				bool flag = false;
				for (int i = 0; i < CS$<>8__locals1.pawn.Map.lordManager.lords.Count; i++)
				{
					Lord lord = CS$<>8__locals1.pawn.Map.lordManager.lords[i];
					if (lord.faction == Faction.OfPlayer && lord.LordJob is LordJob_FormAndSendCaravan)
					{
						flag = true;
						break;
					}
				}
				if (flag && Dialog_FormCaravan.AllSendablePawns(CS$<>8__locals1.pawn.Map, false).Contains(CS$<>8__locals1.pawn))
				{
					yield return new Command_Action
					{
						defaultLabel = "CommandAddToCaravan".Translate(),
						defaultDesc = "CommandAddToCaravanDesc".Translate(),
						icon = CaravanFormingUtility.AddToCaravanCommand,
						action = delegate()
						{
							List<Lord> list = new List<Lord>();
							for (int j = 0; j < CS$<>8__locals1.pawn.Map.lordManager.lords.Count; j++)
							{
								Lord lord2 = CS$<>8__locals1.pawn.Map.lordManager.lords[j];
								if (lord2.faction == Faction.OfPlayer && lord2.LordJob is LordJob_FormAndSendCaravan)
								{
									list.Add(lord2);
								}
							}
							if (list.Count == 0)
							{
								return;
							}
							if (list.Count == 1)
							{
								CaravanFormingUtility.LateJoinFormingCaravan(CS$<>8__locals1.pawn, list[0]);
								SoundDefOf.Click.PlayOneShotOnCamera(null);
								return;
							}
							List<FloatMenuOption> list2 = new List<FloatMenuOption>();
							for (int k = 0; k < list.Count; k++)
							{
								Lord caravanLocal = list[k];
								string label = "Caravan".Translate() + " " + (k + 1);
								list2.Add(new FloatMenuOption(label, delegate()
								{
									if (CS$<>8__locals1.pawn.Spawned && CS$<>8__locals1.pawn.Map.lordManager.lords.Contains(caravanLocal) && Dialog_FormCaravan.AllSendablePawns(CS$<>8__locals1.pawn.Map, false).Contains(CS$<>8__locals1.pawn))
									{
										CaravanFormingUtility.LateJoinFormingCaravan(CS$<>8__locals1.pawn, caravanLocal);
									}
								}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
							}
							Find.WindowStack.Add(new FloatMenu(list2));
						},
						hotKey = KeyBindingDefOf.Misc7
					};
				}
			}
			yield break;
		}

		// Token: 0x06008C4D RID: 35917 RVA: 0x00325D8C File Offset: 0x00323F8C
		private static void LateJoinFormingCaravan(Pawn pawn, Lord lord)
		{
			Lord lord2 = pawn.GetLord();
			if (lord2 != null)
			{
				lord2.Notify_PawnLost(pawn, PawnLostCondition.ForcedToJoinOtherLord, null);
			}
			if (pawn.Downed)
			{
				((LordJob_FormAndSendCaravan)lord.LordJob).downedPawns.Add(pawn);
			}
			else
			{
				lord.AddPawn(pawn);
			}
			if (pawn.Spawned)
			{
				pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06008C4E RID: 35918 RVA: 0x00325DF4 File Offset: 0x00323FF4
		public static bool IsFormingCaravan(this Pawn p)
		{
			Lord lord = p.GetLord();
			return lord != null && lord.LordJob is LordJob_FormAndSendCaravan;
		}

		// Token: 0x06008C4F RID: 35919 RVA: 0x00325E1B File Offset: 0x0032401B
		public static bool IsFormingCaravanOrDownedPawnToBeTakenByCaravan(Pawn p)
		{
			return CaravanFormingUtility.GetFormAndSendCaravanLord(p) != null;
		}

		// Token: 0x06008C50 RID: 35920 RVA: 0x00325E28 File Offset: 0x00324028
		public static Lord GetFormAndSendCaravanLord(Pawn p)
		{
			if (p.IsFormingCaravan())
			{
				return p.GetLord();
			}
			if (p.Spawned)
			{
				List<Lord> lords = p.Map.lordManager.lords;
				for (int i = 0; i < lords.Count; i++)
				{
					LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lords[i].LordJob as LordJob_FormAndSendCaravan;
					if (lordJob_FormAndSendCaravan != null && lordJob_FormAndSendCaravan.downedPawns.Contains(p))
					{
						return lords[i];
					}
				}
			}
			return null;
		}

		// Token: 0x06008C51 RID: 35921 RVA: 0x00325E9C File Offset: 0x0032409C
		public static float CapacityLeft(LordJob_FormAndSendCaravan lordJob)
		{
			float num = CollectionsMassCalculator.MassUsageTransferables(lordJob.transferables, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
			CaravanFormingUtility.tmpCaravanPawns.Clear();
			for (int i = 0; i < lordJob.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = lordJob.lord.ownedPawns[i];
				CaravanFormingUtility.tmpCaravanPawns.Add(new ThingCount(pawn, pawn.stackCount));
			}
			num += CollectionsMassCalculator.MassUsage(CaravanFormingUtility.tmpCaravanPawns, IgnorePawnsInventoryMode.IgnoreIfAssignedToUnload, false, false);
			float num2 = CollectionsMassCalculator.Capacity(CaravanFormingUtility.tmpCaravanPawns, null);
			CaravanFormingUtility.tmpCaravanPawns.Clear();
			return num2 - num;
		}

		// Token: 0x06008C52 RID: 35922 RVA: 0x00325F2C File Offset: 0x0032412C
		public static string AppendOverweightInfo(string text, float capacityLeft)
		{
			if (capacityLeft < 0f)
			{
				text += " (" + "OverweightLower".Translate() + ")";
			}
			return text;
		}

		// Token: 0x04005911 RID: 22801
		private static readonly Texture2D RemoveFromCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/RemoveFromCaravan", true);

		// Token: 0x04005912 RID: 22802
		private static readonly Texture2D AddToCaravanCommand = ContentFinder<Texture2D>.Get("UI/Commands/AddToCaravan", true);

		// Token: 0x04005913 RID: 22803
		private static List<Pawn> tmpPawnsInCancelledCaravan = new List<Pawn>();

		// Token: 0x04005914 RID: 22804
		private static List<Pawn> tmpRopers = new List<Pawn>();

		// Token: 0x04005915 RID: 22805
		private static List<Pawn> tmpNeedRoping = new List<Pawn>();

		// Token: 0x04005916 RID: 22806
		private static List<ThingCount> tmpCaravanPawns = new List<ThingCount>();
	}
}
