using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E36 RID: 3638
	public static class PrisonBreakUtility
	{
		// Token: 0x06005434 RID: 21556 RVA: 0x001C8058 File Offset: 0x001C6258
		public static float InitiatePrisonBreakMtbDays(Pawn pawn, StringBuilder sb = null)
		{
			if (!pawn.Awake())
			{
				return -1f;
			}
			if (!PrisonBreakUtility.CanParticipateInPrisonBreak(pawn))
			{
				return -1f;
			}
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room == null || !room.IsPrisonCell)
			{
				return -1f;
			}
			float num = 60f;
			float num2 = Mathf.Clamp(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving), 0.01f, 1f);
			num /= num2;
			if (sb != null && num2 != 1f)
			{
				sb.AppendLineIfNotEmpty();
				sb.Append("FactorForMovement".Translate() + ": " + num2.ToStringPercent());
			}
			float num3 = 0f;
			PrisonBreakUtility.tmpRegions.Clear();
			foreach (Region region in room.Regions)
			{
				foreach (RegionLink regionLink in region.links)
				{
					Region otherRegion = regionLink.GetOtherRegion(region);
					if (otherRegion.type == RegionType.Portal && !PrisonBreakUtility.tmpRegions.Contains(otherRegion))
					{
						PrisonBreakUtility.tmpRegions.Add(otherRegion);
						for (int i = 0; i < otherRegion.links.Count; i++)
						{
							Region regionA = otherRegion.links[i].RegionA;
							Region regionB = otherRegion.links[i].RegionB;
							if ((regionA.Room != room && regionA != otherRegion) || (regionB.Room != room && regionB != otherRegion))
							{
								num3 += 1f;
								break;
							}
						}
					}
				}
			}
			if (num3 > 0f)
			{
				num /= num3;
				if (sb != null && num3 > 1f)
				{
					sb.AppendLineIfNotEmpty();
					sb.Append("FactorForDoorCount".Translate() + ": " + num3.ToStringPercent());
				}
			}
			if (pawn.guest.everParticipatedInPrisonBreak)
			{
				float x = (float)(Find.TickManager.TicksGame - pawn.guest.lastPrisonBreakTicks) / 60000f;
				num *= PrisonBreakUtility.PrisonBreakMTBFactorForDaysSincePrisonBreak.Evaluate(x);
			}
			return num;
		}

		// Token: 0x06005435 RID: 21557 RVA: 0x001C82C8 File Offset: 0x001C64C8
		public static bool CanParticipateInPrisonBreak(Pawn pawn)
		{
			return !pawn.Downed && pawn.IsPrisoner && !PrisonBreakUtility.IsPrisonBreaking(pawn);
		}

		// Token: 0x06005436 RID: 21558 RVA: 0x001C82EC File Offset: 0x001C64EC
		public static bool IsPrisonBreaking(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_PrisonBreak;
		}

		// Token: 0x06005437 RID: 21559 RVA: 0x001C8314 File Offset: 0x001C6514
		public static void StartPrisonBreak(Pawn initiator)
		{
			string str;
			string str2;
			LetterDef textLetterDef;
			PrisonBreakUtility.StartPrisonBreak(initiator, out str, out str2, out textLetterDef);
			if (!str.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(str2, str, textLetterDef, initiator, null, null, null, null);
			}
		}

		// Token: 0x06005438 RID: 21560 RVA: 0x001C8358 File Offset: 0x001C6558
		public static void StartPrisonBreak(Pawn initiator, out string letterText, out string letterLabel, out LetterDef letterDef)
		{
			PrisonBreakUtility.participatingRooms.Clear();
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(initiator.Position, 20f, true))
			{
				if (intVec.InBounds(initiator.Map))
				{
					Room room = intVec.GetRoom(initiator.Map);
					if (room != null && PrisonBreakUtility.IsOrCanBePrisonCell(room))
					{
						PrisonBreakUtility.participatingRooms.Add(room);
					}
				}
			}
			PrisonBreakUtility.RemoveRandomRooms(PrisonBreakUtility.participatingRooms, initiator);
			int sapperThingID = -1;
			if (Rand.Value < 0.5f)
			{
				sapperThingID = initiator.thingIDNumber;
			}
			PrisonBreakUtility.allEscapingPrisoners.Clear();
			foreach (Room room2 in PrisonBreakUtility.participatingRooms)
			{
				PrisonBreakUtility.StartPrisonBreakIn(room2, PrisonBreakUtility.allEscapingPrisoners, sapperThingID, PrisonBreakUtility.participatingRooms);
			}
			PrisonBreakUtility.participatingRooms.Clear();
			if (PrisonBreakUtility.allEscapingPrisoners.Any<Pawn>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < PrisonBreakUtility.allEscapingPrisoners.Count; i++)
				{
					stringBuilder.AppendLine("  - " + PrisonBreakUtility.allEscapingPrisoners[i].NameShortColored.Resolve());
				}
				letterText = "LetterPrisonBreak".Translate(stringBuilder.ToString().TrimEndNewlines());
				letterLabel = "LetterLabelPrisonBreak".Translate();
				letterDef = LetterDefOf.ThreatBig;
				PrisonBreakUtility.allEscapingPrisoners.Clear();
			}
			else
			{
				letterText = null;
				letterLabel = null;
				letterDef = null;
			}
			Find.TickManager.slower.SignalForceNormalSpeed();
		}

		// Token: 0x06005439 RID: 21561 RVA: 0x001C851C File Offset: 0x001C671C
		private static bool IsOrCanBePrisonCell(Room room)
		{
			if (room.IsPrisonCell)
			{
				return true;
			}
			if (room.TouchesMapEdge)
			{
				return false;
			}
			bool result = false;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Pawn pawn = containedAndAdjacentThings[i] as Pawn;
				if (pawn != null && pawn.IsPrisoner)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600543A RID: 21562 RVA: 0x001C8574 File Offset: 0x001C6774
		private static void RemoveRandomRooms(HashSet<Room> participatingRooms, Pawn initiator)
		{
			Room room = initiator.GetRoom(RegionType.Set_All);
			PrisonBreakUtility.tmpToRemove.Clear();
			foreach (Room room2 in participatingRooms)
			{
				if (room2 != room && Rand.Value >= 0.5f)
				{
					PrisonBreakUtility.tmpToRemove.Add(room2);
				}
			}
			for (int i = 0; i < PrisonBreakUtility.tmpToRemove.Count; i++)
			{
				participatingRooms.Remove(PrisonBreakUtility.tmpToRemove[i]);
			}
			PrisonBreakUtility.tmpToRemove.Clear();
		}

		// Token: 0x0600543B RID: 21563 RVA: 0x001C861C File Offset: 0x001C681C
		private static void StartPrisonBreakIn(Room room, List<Pawn> outAllEscapingPrisoners, int sapperThingID, HashSet<Room> participatingRooms)
		{
			PrisonBreakUtility.escapingPrisonersGroup.Clear();
			PrisonBreakUtility.AddPrisonersFrom(room, PrisonBreakUtility.escapingPrisonersGroup);
			if (!PrisonBreakUtility.escapingPrisonersGroup.Any<Pawn>())
			{
				return;
			}
			foreach (Room room2 in participatingRooms)
			{
				if (room2 != room && PrisonBreakUtility.RoomsAreCloseToEachOther(room, room2))
				{
					PrisonBreakUtility.AddPrisonersFrom(room2, PrisonBreakUtility.escapingPrisonersGroup);
				}
			}
			IntVec3 exitPoint;
			if (!RCellFinder.TryFindRandomExitSpot(PrisonBreakUtility.escapingPrisonersGroup[0], out exitPoint, TraverseMode.PassDoors))
			{
				return;
			}
			IntVec3 groupUpLoc;
			if (!PrisonBreakUtility.TryFindGroupUpLoc(PrisonBreakUtility.escapingPrisonersGroup, exitPoint, out groupUpLoc))
			{
				return;
			}
			LordMaker.MakeNewLord(PrisonBreakUtility.escapingPrisonersGroup[0].Faction, new LordJob_PrisonBreak(groupUpLoc, exitPoint, sapperThingID), room.Map, PrisonBreakUtility.escapingPrisonersGroup);
			for (int i = 0; i < PrisonBreakUtility.escapingPrisonersGroup.Count; i++)
			{
				Pawn pawn = PrisonBreakUtility.escapingPrisonersGroup[i];
				if (pawn.CurJob != null && pawn.GetPosture().Laying())
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				else
				{
					pawn.jobs.CheckForJobOverride();
				}
				pawn.guest.everParticipatedInPrisonBreak = true;
				pawn.guest.lastPrisonBreakTicks = Find.TickManager.TicksGame;
				outAllEscapingPrisoners.Add(pawn);
			}
			PrisonBreakUtility.escapingPrisonersGroup.Clear();
		}

		// Token: 0x0600543C RID: 21564 RVA: 0x001C8780 File Offset: 0x001C6980
		private static void AddPrisonersFrom(Room room, List<Pawn> outEscapingPrisoners)
		{
			foreach (Thing thing in room.ContainedAndAdjacentThings)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && PrisonBreakUtility.CanParticipateInPrisonBreak(pawn) && !outEscapingPrisoners.Contains(pawn))
				{
					outEscapingPrisoners.Add(pawn);
				}
			}
		}

		// Token: 0x0600543D RID: 21565 RVA: 0x001C87EC File Offset: 0x001C69EC
		public static bool TryFindGroupUpLoc(List<Pawn> escapingPrisoners, IntVec3 exitPoint, out IntVec3 groupUpLoc)
		{
			groupUpLoc = IntVec3.Invalid;
			Map map = escapingPrisoners[0].Map;
			using (PawnPath pawnPath = map.pathFinder.FindPath(escapingPrisoners[0].Position, exitPoint, TraverseParms.For(escapingPrisoners[0], Danger.Deadly, TraverseMode.PassDoors, false, false, false), PathEndMode.OnCell, null))
			{
				if (!pawnPath.Found)
				{
					Log.Warning("Prison break: could not find path for prisoner " + escapingPrisoners[0] + " to the exit point.");
					return false;
				}
				Func<IntVec3, bool> <>9__0;
				for (int i = 0; i < pawnPath.NodesLeftCount; i++)
				{
					IntVec3 intVec = pawnPath.Peek(pawnPath.NodesLeftCount - i - 1);
					Room room = intVec.GetRoom(map);
					if (room != null && !room.IsPrisonCell)
					{
						if (!room.TouchesMapEdge && !room.IsHuge)
						{
							IEnumerable<IntVec3> cells = room.Cells;
							Func<IntVec3, bool> predicate;
							if ((predicate = <>9__0) == null)
							{
								predicate = (<>9__0 = ((IntVec3 x) => x.Standable(map)));
							}
							if (cells.Count(predicate) < 5)
							{
								goto IL_10F;
							}
						}
						groupUpLoc = CellFinder.RandomClosewalkCellNear(intVec, map, 3, null);
					}
					IL_10F:;
				}
			}
			if (!groupUpLoc.IsValid)
			{
				groupUpLoc = escapingPrisoners[0].Position;
			}
			return true;
		}

		// Token: 0x0600543E RID: 21566 RVA: 0x001C8954 File Offset: 0x001C6B54
		private static bool RoomsAreCloseToEachOther(Room a, Room b)
		{
			IntVec3 anyCell = a.FirstRegion.AnyCell;
			IntVec3 anyCell2 = b.FirstRegion.AnyCell;
			if (a.Map != b.Map)
			{
				return false;
			}
			if (!anyCell.WithinRegions(anyCell2, a.Map, 18, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false), RegionType.Set_Passable))
			{
				return false;
			}
			bool result;
			using (PawnPath pawnPath = a.Map.pathFinder.FindPath(anyCell, anyCell2, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false), PathEndMode.OnCell, null))
			{
				if (!pawnPath.Found)
				{
					result = false;
				}
				else
				{
					result = (pawnPath.NodesLeftCount < 24);
				}
			}
			return result;
		}

		// Token: 0x04003198 RID: 12696
		private const float BaseInitiatePrisonBreakMtbDays = 60f;

		// Token: 0x04003199 RID: 12697
		private const float DistanceToJoinPrisonBreak = 20f;

		// Token: 0x0400319A RID: 12698
		private const float ChanceForRoomToJoinPrisonBreak = 0.5f;

		// Token: 0x0400319B RID: 12699
		private const float SapperChance = 0.5f;

		// Token: 0x0400319C RID: 12700
		private static readonly SimpleCurve PrisonBreakMTBFactorForDaysSincePrisonBreak = new SimpleCurve
		{
			{
				new CurvePoint(0f, 20f),
				true
			},
			{
				new CurvePoint(5f, 1.5f),
				true
			},
			{
				new CurvePoint(10f, 1f),
				true
			}
		};

		// Token: 0x0400319D RID: 12701
		private static HashSet<Region> tmpRegions = new HashSet<Region>();

		// Token: 0x0400319E RID: 12702
		private static HashSet<Room> participatingRooms = new HashSet<Room>();

		// Token: 0x0400319F RID: 12703
		private static List<Pawn> allEscapingPrisoners = new List<Pawn>();

		// Token: 0x040031A0 RID: 12704
		private static List<Room> tmpToRemove = new List<Room>();

		// Token: 0x040031A1 RID: 12705
		private static List<Pawn> escapingPrisonersGroup = new List<Pawn>();
	}
}
