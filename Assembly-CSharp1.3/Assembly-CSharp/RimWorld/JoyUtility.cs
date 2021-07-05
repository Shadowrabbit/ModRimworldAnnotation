using System;
using System.Collections.Generic;
using System.Text;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020007EC RID: 2028
	public static class JoyUtility
	{
		// Token: 0x06003649 RID: 13897 RVA: 0x00133814 File Offset: 0x00131A14
		public static bool EnjoyableOutsideNow(Map map, StringBuilder outFailReason = null)
		{
			if (map.weatherManager.RainRate >= 0.25f)
			{
				if (outFailReason != null)
				{
					outFailReason.Append(map.weatherManager.curWeather.label);
				}
				return false;
			}
			GameConditionDef gameConditionDef;
			if (!map.gameConditionManager.AllowEnjoyableOutsideNow(map, out gameConditionDef))
			{
				if (outFailReason != null)
				{
					outFailReason.Append(gameConditionDef.label);
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600364A RID: 13898 RVA: 0x00133874 File Offset: 0x00131A74
		public static bool EnjoyableOutsideNow(Pawn pawn, StringBuilder outFailReason = null)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld == null)
			{
				return true;
			}
			if (!JoyUtility.EnjoyableOutsideNow(mapHeld, outFailReason))
			{
				return false;
			}
			if (!pawn.ComfortableTemperatureRange().Includes(mapHeld.mapTemperature.OutdoorTemp))
			{
				if (outFailReason != null)
				{
					outFailReason.Append("NotEnjoyableOutsideTemperature".Translate());
				}
				return false;
			}
			return true;
		}

		// Token: 0x0600364B RID: 13899 RVA: 0x001338D0 File Offset: 0x00131AD0
		public static bool JoyTickCheckEnd(Pawn pawn, JoyTickFullJoyAction fullJoyAction = JoyTickFullJoyAction.EndJob, float extraJoyGainFactor = 1f, Building joySource = null)
		{
			Job curJob = pawn.CurJob;
			if (curJob.def.joyKind == null)
			{
				Log.Warning("This method can only be called for jobs with joyKind.");
				return false;
			}
			if (joySource != null)
			{
				if (joySource.def.building.joyKind != null && pawn.CurJob.def.joyKind != joySource.def.building.joyKind)
				{
					Log.ErrorOnce("Joy source joyKind and jobDef.joyKind are not the same. building=" + joySource.ToStringSafe<Building>() + ", jobDef=" + pawn.CurJob.def.ToStringSafe<JobDef>(), joySource.thingIDNumber ^ 876598732);
				}
				extraJoyGainFactor *= joySource.GetStatValue(StatDefOf.JoyGainFactor, true);
			}
			if (pawn.needs.joy == null)
			{
				pawn.jobs.curDriver.EndJobWith(JobCondition.InterruptForced);
				return false;
			}
			pawn.needs.joy.GainJoy(extraJoyGainFactor * curJob.def.joyGainRate * 0.36f / 2500f, curJob.def.joyKind);
			if (curJob.def.joySkill != null)
			{
				pawn.skills.GetSkill(curJob.def.joySkill).Learn(curJob.def.joyXpPerTick, false);
			}
			if (!curJob.ignoreJoyTimeAssignment && !pawn.GetTimeAssignment().allowJoy)
			{
				pawn.jobs.curDriver.EndJobWith(JobCondition.InterruptForced);
				return true;
			}
			if (pawn.needs.joy.CurLevel > 0.9999f)
			{
				if (fullJoyAction == JoyTickFullJoyAction.EndJob)
				{
					pawn.jobs.curDriver.EndJobWith(JobCondition.Succeeded);
					return true;
				}
				if (fullJoyAction == JoyTickFullJoyAction.GoToNextToil)
				{
					pawn.jobs.curDriver.ReadyForNextToil();
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600364C RID: 13900 RVA: 0x00133A70 File Offset: 0x00131C70
		public static void TryGainRecRoomThought(Pawn pawn)
		{
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				int scoreStageIndex = RoomStatDefOf.Impressiveness.GetScoreStageIndex(room.GetStat(RoomStatDefOf.Impressiveness));
				if (pawn.needs.mood != null && ThoughtDefOf.AteInImpressiveDiningRoom.stages[scoreStageIndex] != null)
				{
					pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtMaker.MakeThought(ThoughtDefOf.JoyActivityInImpressiveRecRoom, scoreStageIndex), null);
				}
			}
		}

		// Token: 0x0600364D RID: 13901 RVA: 0x00133AE4 File Offset: 0x00131CE4
		public static bool LordPreventsGettingJoy(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && !lord.CurLordToil.AllowSatisfyLongNeeds;
		}

		// Token: 0x0600364E RID: 13902 RVA: 0x00133B0B File Offset: 0x00131D0B
		public static bool TimetablePreventsGettingJoy(Pawn pawn)
		{
			return !((pawn.timetable == null) ? TimeAssignmentDefOf.Anything : pawn.timetable.CurrentAssignment).allowJoy;
		}

		// Token: 0x0600364F RID: 13903 RVA: 0x00133B34 File Offset: 0x00131D34
		public static int JoyKindsOnMapCount(Map map)
		{
			List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(map);
			int count = list.Count;
			list.Clear();
			return count;
		}

		// Token: 0x06003650 RID: 13904 RVA: 0x00133B54 File Offset: 0x00131D54
		public static List<JoyKindDef> JoyKindsOnMapTempList(Map map)
		{
			for (int i = 0; i < DefDatabase<JoyKindDef>.AllDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = DefDatabase<JoyKindDef>.AllDefsListForReading[i];
				if (!joyKindDef.needsThing)
				{
					JoyUtility.tempKindList.Add(joyKindDef);
				}
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				if (building.def.building.joyKind != null && !JoyUtility.tempKindList.Contains(building.def.building.joyKind))
				{
					JoyUtility.tempKindList.Add(building.def.building.joyKind);
				}
			}
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Drug))
			{
				if (thing.def.IsIngestible && thing.def.ingestible.joyKind != null && !JoyUtility.tempKindList.Contains(thing.def.ingestible.joyKind) && !thing.Position.Fogged(map))
				{
					JoyUtility.tempKindList.Add(thing.def.ingestible.joyKind);
				}
			}
			foreach (Thing thing2 in map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				if (thing2.def.IsIngestible && thing2.def.ingestible.joyKind != null && !JoyUtility.tempKindList.Contains(thing2.def.ingestible.joyKind) && !thing2.Position.Fogged(map))
				{
					JoyUtility.tempKindList.Add(thing2.def.ingestible.joyKind);
				}
			}
			return JoyUtility.tempKindList;
		}

		// Token: 0x06003651 RID: 13905 RVA: 0x00133D80 File Offset: 0x00131F80
		public static string JoyKindsOnMapString(Map map)
		{
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < DefDatabase<JoyKindDef>.AllDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = DefDatabase<JoyKindDef>.AllDefsListForReading[i];
				if (!joyKindDef.needsThing)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, null, joyKindDef, map);
				}
			}
			foreach (Building building in map.listerBuildings.allBuildingsColonist)
			{
				if (building.def.building.joyKind != null)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, building, building.def.building.joyKind, map);
				}
			}
			foreach (Thing thing in map.listerThings.ThingsInGroup(ThingRequestGroup.Drug))
			{
				if (thing.def.IsIngestible && thing.def.ingestible.joyKind != null)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, thing, thing.def.ingestible.joyKind, map);
				}
			}
			foreach (Thing thing2 in map.listerThings.ThingsInGroup(ThingRequestGroup.FoodSourceNotPlantOrTree))
			{
				if (thing2.def.IsIngestible && thing2.def.ingestible.joyKind != null)
				{
					JoyUtility.CheckAppendJoyKind(stringBuilder, thing2, thing2.def.ingestible.joyKind, map);
				}
			}
			JoyUtility.listedJoyKinds.Clear();
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06003652 RID: 13906 RVA: 0x00133F4C File Offset: 0x0013214C
		private static void CheckAppendJoyKind(StringBuilder sb, Thing t, JoyKindDef kind, Map map)
		{
			if (JoyUtility.listedJoyKinds.Contains(kind))
			{
				return;
			}
			sb.Append("  - ");
			if (t == null)
			{
				sb.AppendLine(kind.LabelCap);
			}
			else
			{
				if (t.def.category == ThingCategory.Item && t.Position.Fogged(map))
				{
					return;
				}
				sb.AppendLine(kind.LabelCap + " (" + t.def.label + ")");
			}
			JoyUtility.listedJoyKinds.Add(kind);
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x00133FE8 File Offset: 0x001321E8
		public static string JoyKindsNotOnMapString(Map map)
		{
			List<JoyKindDef> allDefsListForReading = DefDatabase<JoyKindDef>.AllDefsListForReading;
			List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(map);
			if (allDefsListForReading.Count == list.Count)
			{
				return "(" + "None".Translate() + ")";
			}
			string text = "";
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = allDefsListForReading[i];
				if (!list.Contains(joyKindDef))
				{
					text += "  - " + joyKindDef.LabelCap + "\n";
				}
			}
			list.Clear();
			return text.TrimEndNewlines();
		}

		// Token: 0x04001EE4 RID: 7908
		private static List<JoyKindDef> tempKindList = new List<JoyKindDef>();

		// Token: 0x04001EE5 RID: 7909
		private static List<JoyKindDef> listedJoyKinds = new List<JoyKindDef>();
	}
}
