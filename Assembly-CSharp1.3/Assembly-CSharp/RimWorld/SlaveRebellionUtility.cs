using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000E38 RID: 3640
	public static class SlaveRebellionUtility
	{
		// Token: 0x06005442 RID: 21570 RVA: 0x001C8B04 File Offset: 0x001C6D04
		public static float InitiateSlaveRebellionMtbDays(Pawn pawn)
		{
			if (!SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn))
			{
				return -1f;
			}
			Need_Suppression need_Suppression = pawn.needs.TryGetNeed<Need_Suppression>();
			if (need_Suppression == null)
			{
				return -1f;
			}
			float num = 45f;
			num /= SlaveRebellionUtility.MovingCapacityFactorCurve.Evaluate(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
			num /= SlaveRebellionUtility.SuppresionRebellionFactorCurve.Evaluate(need_Suppression.CurLevelPercentage);
			if (pawn.needs.mood != null)
			{
				num /= SlaveRebellionUtility.MoodRebellionFactorCurve.Evaluate(pawn.needs.mood.CurLevelPercentage);
			}
			if (SlaveRebellionUtility.InRoomTouchingMapEdge(pawn))
			{
				num /= 2f;
			}
			if (SlaveRebellionUtility.CanApplyWeaponFactor(pawn))
			{
				num /= 5f;
			}
			if (SlaveRebellionUtility.IsUnattendedByColonists(pawn.Map))
			{
				num /= 20f;
			}
			return num;
		}

		// Token: 0x06005443 RID: 21571 RVA: 0x001C8BD0 File Offset: 0x001C6DD0
		private static bool InRoomTouchingMapEdge(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			return room != null && room.TouchesMapEdge;
		}

		// Token: 0x06005444 RID: 21572 RVA: 0x001C8BF4 File Offset: 0x001C6DF4
		private static bool CanApplyWeaponFactor(Pawn pawn)
		{
			ThingWithComps primary = pawn.equipment.Primary;
			return (primary != null && primary.def.IsWeapon) || SlaveRebellionUtility.GoodWeaponInSameRoom(pawn);
		}

		// Token: 0x06005445 RID: 21573 RVA: 0x001C8C28 File Offset: 0x001C6E28
		public static bool IsUnattendedByColonists(Map map)
		{
			foreach (Pawn pawn in map.mapPawns.FreeColonistsSpawned)
			{
				if (!pawn.IsSlave && !pawn.Downed && !pawn.Dead)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06005446 RID: 21574 RVA: 0x001C8C98 File Offset: 0x001C6E98
		public static string GetSlaveRebellionMtbCalculationExplanation(Pawn pawn)
		{
			Need_Suppression need_Suppression = pawn.needs.TryGetNeed<Need_Suppression>();
			if (need_Suppression == null || !SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn))
			{
				return "";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(string.Format("{0}: {1}", "SuppressionBaseInterval".Translate(), 2700000.ToStringTicksToPeriodVague(true, true)));
			float f = 1f / SlaveRebellionUtility.MovingCapacityFactorCurve.Evaluate(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving));
			stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionMovingCapacityFactor".Translate(), f.ToStringPercent()));
			float f2 = 1f / SlaveRebellionUtility.SuppresionRebellionFactorCurve.Evaluate(need_Suppression.CurLevelPercentage);
			stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionFactor".Translate(), f2.ToStringPercent()));
			float f3 = 1f / SlaveRebellionUtility.MoodRebellionFactorCurve.Evaluate(pawn.needs.mood.CurLevelPercentage);
			stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionMoodFactor".Translate(), f3.ToStringPercent()));
			if (SlaveRebellionUtility.InRoomTouchingMapEdge(pawn))
			{
				float f4 = 0.5f;
				stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionEscapeFactor".Translate(), f4.ToStringPercent()));
			}
			if (SlaveRebellionUtility.CanApplyWeaponFactor(pawn))
			{
				float f5 = 0.2f;
				stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionWeaponProximityFactor".Translate(), f5.ToStringPercent()));
			}
			if (SlaveRebellionUtility.IsUnattendedByColonists(pawn.Map))
			{
				float f6 = 0.05f;
				stringBuilder.AppendLine(string.Format("{0}: x{1}", "SuppressionUnattendedByColonists".Translate(), f6.ToStringPercent()));
			}
			stringBuilder.AppendLine(string.Format("{0}: {1}", "SuppressionFinalInterval".Translate(), ((int)(SlaveRebellionUtility.InitiateSlaveRebellionMtbDays(pawn) * 60000f)).ToStringTicksToPeriod(true, false, true, true)));
			return stringBuilder.ToString();
		}

		// Token: 0x06005447 RID: 21575 RVA: 0x001C8EAC File Offset: 0x001C70AC
		private static bool GoodWeaponInSameRoom(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room == null || room.PsychologicallyOutdoors)
			{
				return false;
			}
			ThingRequest thingReq = ThingRequest.ForGroup(ThingRequestGroup.Weapon);
			return GenClosest.ClosestThingReachable(pawn.Position, pawn.Map, thingReq, PathEndMode.Touch, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false), 10f, (Thing t) => EquipmentUtility.CanEquip(t, pawn) && !PawnWeaponGenerator.IsDerpWeapon(t.def, t.Stuff) && t.GetRoom(RegionType.Set_All) == room, null, 0, -1, false, RegionType.Set_Passable, false) != null;
		}

		// Token: 0x06005448 RID: 21576 RVA: 0x001C8F40 File Offset: 0x001C7140
		public static bool CanParticipateInSlaveRebellion(Pawn pawn)
		{
			return !pawn.Downed && pawn.Spawned && pawn.IsSlave && !pawn.InMentalState && pawn.Awake() && !SlaveRebellionUtility.IsRebelling(pawn);
		}

		// Token: 0x06005449 RID: 21577 RVA: 0x001C8F78 File Offset: 0x001C7178
		public static bool IsRebelling(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_SlaveRebellion;
		}

		// Token: 0x0600544A RID: 21578 RVA: 0x001C8FA0 File Offset: 0x001C71A0
		public static bool StartSlaveRebellion(Pawn initiator, bool forceAggressive = false)
		{
			string str;
			string str2;
			LetterDef textLetterDef;
			LookTargets lookTargets;
			if (SlaveRebellionUtility.StartSlaveRebellion(initiator, out str, out str2, out textLetterDef, out lookTargets, forceAggressive))
			{
				Find.LetterStack.ReceiveLetter(str2, str, textLetterDef, lookTargets, null, null, null, null);
				return true;
			}
			return false;
		}

		// Token: 0x0600544B RID: 21579 RVA: 0x001C8FE0 File Offset: 0x001C71E0
		public static bool StartSlaveRebellion(Pawn initiator, out string letterText, out string letterLabel, out LetterDef letterDef, out LookTargets lookTargets, bool forceAggressive = false)
		{
			letterText = null;
			letterLabel = null;
			letterDef = null;
			lookTargets = null;
			if (!ModLister.CheckIdeology("Slave rebellion"))
			{
				return false;
			}
			SlaveRebellionUtility.rebellingSlaves.Clear();
			SlaveRebellionUtility.rebellingSlaves.Add(initiator);
			SlaveRebellionUtility.allPossibleRebellingSlaves.Clear();
			List<Pawn> slavesOfColonySpawned = initiator.Map.mapPawns.SlavesOfColonySpawned;
			for (int i = 0; i < slavesOfColonySpawned.Count; i++)
			{
				Pawn pawn = slavesOfColonySpawned[i];
				if (pawn != initiator && SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn))
				{
					SlaveRebellionUtility.allPossibleRebellingSlaves.Add(pawn);
				}
			}
			SlaveRebellionUtility.SlaveRebellionType slaveRebellionType = SlaveRebellionUtility.DecideSlaveRebellionType();
			if (slaveRebellionType != SlaveRebellionUtility.SlaveRebellionType.GrandRebellion)
			{
				if (slaveRebellionType == SlaveRebellionUtility.SlaveRebellionType.LocalRebellion)
				{
					for (int j = 0; j < SlaveRebellionUtility.allPossibleRebellingSlaves.Count; j++)
					{
						Pawn pawn2 = SlaveRebellionUtility.allPossibleRebellingSlaves[j];
						if (initiator.Position.DistanceTo(pawn2.Position) <= 35f)
						{
							SlaveRebellionUtility.rebellingSlaves.Add(pawn2);
						}
					}
				}
			}
			else
			{
				for (int k = 0; k < SlaveRebellionUtility.allPossibleRebellingSlaves.Count; k++)
				{
					SlaveRebellionUtility.rebellingSlaves.Add(SlaveRebellionUtility.allPossibleRebellingSlaves[k]);
				}
			}
			if (SlaveRebellionUtility.rebellingSlaves.Count == 1)
			{
				slaveRebellionType = SlaveRebellionUtility.SlaveRebellionType.SingleRebellion;
			}
			else if (SlaveRebellionUtility.rebellingSlaves.Count == SlaveRebellionUtility.allPossibleRebellingSlaves.Count)
			{
				slaveRebellionType = SlaveRebellionUtility.SlaveRebellionType.GrandRebellion;
			}
			IntVec3 exitPoint;
			if (!RCellFinder.TryFindRandomExitSpot(initiator, out exitPoint, TraverseMode.PassDoors))
			{
				return false;
			}
			IntVec3 groupUpLoc;
			if (!PrisonBreakUtility.TryFindGroupUpLoc(SlaveRebellionUtility.rebellingSlaves, exitPoint, out groupUpLoc))
			{
				return false;
			}
			bool flag = forceAggressive || Rand.Chance(0.5f);
			switch (slaveRebellionType)
			{
			case SlaveRebellionUtility.SlaveRebellionType.GrandRebellion:
				if (flag)
				{
					letterLabel = "LetterLabelGrandSlaveRebellion".Translate();
					letterText = "LetterGrandSlaveRebellion".Translate(GenLabel.ThingsLabel(SlaveRebellionUtility.rebellingSlaves, "  - "));
				}
				else
				{
					letterLabel = "LetterLabelGrandSlaveEscape".Translate();
					letterText = "LetterGrandSlaveEscape".Translate(GenLabel.ThingsLabel(SlaveRebellionUtility.rebellingSlaves, "  - "));
				}
				break;
			case SlaveRebellionUtility.SlaveRebellionType.LocalRebellion:
				if (flag)
				{
					letterLabel = "LetterLabelLocalSlaveRebellion".Translate();
					letterText = "LetterLocalSlaveRebellion".Translate(initiator, GenLabel.ThingsLabel(SlaveRebellionUtility.rebellingSlaves, "  - "));
				}
				else
				{
					letterLabel = "LetterLabelLocalSlaveEscape".Translate();
					letterText = "LetterLocalSlaveEscape".Translate(initiator, GenLabel.ThingsLabel(SlaveRebellionUtility.rebellingSlaves, "  - "));
				}
				break;
			case SlaveRebellionUtility.SlaveRebellionType.SingleRebellion:
				if (flag)
				{
					letterLabel = "LetterLabelSingleSlaveRebellion".Translate() + (": " + initiator.LabelShort);
					letterText = "LetterSingleSlaveRebellion".Translate(initiator);
				}
				else
				{
					letterLabel = "LetterLabelSingleSlaveEscape".Translate() + (": " + initiator.LabelShort);
					letterText = "LetterSingleSlaveEscape".Translate(initiator);
				}
				break;
			default:
				Log.Error(string.Format("Unkown slave rebellion type {0}", slaveRebellionType));
				break;
			}
			letterText += "\n\n" + "SlaveRebellionSuppressionExplanation".Translate();
			lookTargets = new LookTargets(SlaveRebellionUtility.rebellingSlaves);
			letterDef = LetterDefOf.ThreatBig;
			int sapperThingID = -1;
			if (Rand.Value < 0.5f)
			{
				sapperThingID = initiator.thingIDNumber;
			}
			for (int l = 0; l < SlaveRebellionUtility.rebellingSlaves.Count; l++)
			{
				Lord lord = SlaveRebellionUtility.rebellingSlaves[l].GetLord();
				if (lord != null)
				{
					lord.Notify_PawnLost(SlaveRebellionUtility.rebellingSlaves[l], PawnLostCondition.ForcedToJoinOtherLord, null);
				}
			}
			LordMaker.MakeNewLord(SlaveRebellionUtility.rebellingSlaves[0].Faction, new LordJob_SlaveRebellion(groupUpLoc, exitPoint, sapperThingID, !flag), initiator.Map, SlaveRebellionUtility.rebellingSlaves);
			for (int m = 0; m < SlaveRebellionUtility.rebellingSlaves.Count; m++)
			{
				if (!SlaveRebellionUtility.rebellingSlaves[m].Awake())
				{
					RestUtility.WakeUp(SlaveRebellionUtility.rebellingSlaves[m]);
				}
				if (SlaveRebellionUtility.rebellingSlaves[m].CurJob != null)
				{
					SlaveRebellionUtility.rebellingSlaves[m].jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				SlaveRebellionUtility.rebellingSlaves[m].Map.attackTargetsCache.UpdateTarget(SlaveRebellionUtility.rebellingSlaves[m]);
			}
			SlaveRebellionUtility.rebellingSlaves.Clear();
			return true;
		}

		// Token: 0x0600544C RID: 21580 RVA: 0x001C946D File Offset: 0x001C766D
		private static SlaveRebellionUtility.SlaveRebellionType DecideSlaveRebellionType()
		{
			return Enum.GetValues(typeof(SlaveRebellionUtility.SlaveRebellionType)).Cast<SlaveRebellionUtility.SlaveRebellionType>().RandomElement<SlaveRebellionUtility.SlaveRebellionType>();
		}

		// Token: 0x0600544D RID: 21581 RVA: 0x001C9488 File Offset: 0x001C7688
		public static Pawn FindSlaveForRebellion(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			SlaveRebellionUtility.tmpSlaves.Clear();
			List<Pawn> allPawnsSpawned = pawn.Map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Pawn pawn2 = allPawnsSpawned[i];
				if (pawn2.IsSlave && pawn2.SlaveFaction == pawn.SlaveFaction && pawn2 != pawn && !pawn2.Downed && !pawn2.InMentalState && !pawn2.IsBurning() && pawn2.Awake() && SlaveRebellionUtility.CanParticipateInSlaveRebellion(pawn2) && pawn.CanReach(pawn2, PathEndMode.Touch, Danger.Deadly, false, false, TraverseMode.ByPawn))
				{
					SlaveRebellionUtility.tmpSlaves.Add(pawn2);
				}
			}
			if (!SlaveRebellionUtility.tmpSlaves.Any<Pawn>())
			{
				return null;
			}
			Pawn result = SlaveRebellionUtility.tmpSlaves.RandomElement<Pawn>();
			SlaveRebellionUtility.tmpSlaves.Clear();
			return result;
		}

		// Token: 0x040031A2 RID: 12706
		private const int MaxRegionsForDoorCount = 25;

		// Token: 0x040031A3 RID: 12707
		private const float BaseInitiateSlaveRebellionMtbDays = 45f;

		// Token: 0x040031A4 RID: 12708
		private const float WeaponProximityMultiplier = 5f;

		// Token: 0x040031A5 RID: 12709
		private const float WeaponMaxDistanceForProximityMultiplier = 10f;

		// Token: 0x040031A6 RID: 12710
		private const float RoomTouchesMapEdge = 2f;

		// Token: 0x040031A7 RID: 12711
		private static readonly SimpleCurve MovingCapacityFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0.01f),
				true
			},
			{
				new CurvePoint(0.5f, 0.5f),
				true
			},
			{
				new CurvePoint(1f, 1f),
				true
			}
		};

		// Token: 0x040031A8 RID: 12712
		private static readonly SimpleCurve SuppresionRebellionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 5f),
				true
			},
			{
				new CurvePoint(0.333f, 1.5f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.25f),
				true
			}
		};

		// Token: 0x040031A9 RID: 12713
		private static readonly SimpleCurve MoodRebellionFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(0f, 2f),
				true
			},
			{
				new CurvePoint(0.5f, 1f),
				true
			},
			{
				new CurvePoint(1f, 0.8f),
				true
			}
		};

		// Token: 0x040031AA RID: 12714
		private const float UnattendedByColonistsMultiplier = 20f;

		// Token: 0x040031AB RID: 12715
		private const float SapperChance = 0.5f;

		// Token: 0x040031AC RID: 12716
		private const float LocalRebellionSearchDistance = 35f;

		// Token: 0x040031AD RID: 12717
		private const float AggressiveRebellionChance = 0.5f;

		// Token: 0x040031AE RID: 12718
		private static List<Pawn> rebellingSlaves = new List<Pawn>();

		// Token: 0x040031AF RID: 12719
		private static List<Pawn> allPossibleRebellingSlaves = new List<Pawn>();

		// Token: 0x040031B0 RID: 12720
		private static List<Pawn> tmpSlaves = new List<Pawn>();

		// Token: 0x020022AD RID: 8877
		private enum SlaveRebellionType
		{
			// Token: 0x04008444 RID: 33860
			GrandRebellion,
			// Token: 0x04008445 RID: 33861
			LocalRebellion,
			// Token: 0x04008446 RID: 33862
			SingleRebellion
		}
	}
}
