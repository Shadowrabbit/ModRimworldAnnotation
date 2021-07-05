using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C73 RID: 3187
	public abstract class ComplexThreatWorker_SleepingThreat : ComplexThreatWorker
	{
		// Token: 0x17000CE1 RID: 3297
		// (get) Token: 0x06004A5A RID: 19034 RVA: 0x00189900 File Offset: 0x00187B00
		private Faction Faction
		{
			get
			{
				return Find.FactionManager.FirstFactionOfDef(this.def.faction);
			}
		}

		// Token: 0x06004A5B RID: 19035 RVA: 0x00189917 File Offset: 0x00187B17
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			return base.CanResolveInt(parms) && !parms.room.NullOrEmpty<CellRect>() && this.Faction != null && this.GetPawnKindsForPoints(parms.points).Any<PawnKindDef>();
		}

		// Token: 0x06004A5C RID: 19036
		protected abstract IEnumerable<PawnKindDef> GetPawnKindsForPoints(float points);

		// Token: 0x06004A5D RID: 19037 RVA: 0x0018994C File Offset: 0x00187B4C
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			List<Pawn> list = this.SpawnThreatPawns(parms.room, parms.points, parms.map);
			for (int i = 0; i < list.Count; i++)
			{
				threatPointsUsed += list[i].kindDef.combatPower;
			}
			LordJob_SleepThenAssaultColony lordJob = new LordJob_SleepThenAssaultColony(this.Faction, true);
			Lord lord = LordMaker.MakeNewLord(this.Faction, lordJob, parms.map, null);
			lord.AddPawns(list);
			if (!parms.passive && Rand.Chance(ComplexThreatWorker_SleepingThreat.RadialTriggerChance))
			{
				parms.triggerSignal = ComplexUtility.SpawnRadialDistanceTrigger(list, parms.map, ComplexThreatWorker_SleepingThreat.RadiusWakeUpDistanceRange.RandomInRange);
			}
			SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = (SignalAction_DormancyWakeUp)ThingMaker.MakeThing(ThingDefOf.SignalAction_DormancyWakeUp, null);
			signalAction_DormancyWakeUp.lord = lord;
			signalAction_DormancyWakeUp.signalTag = parms.triggerSignal;
			if (parms.delayTicks != null)
			{
				signalAction_DormancyWakeUp.delayTicks = parms.delayTicks.Value;
				SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
				signalAction_Message.signalTag = parms.triggerSignal;
				signalAction_Message.lookTargets = list;
				signalAction_Message.messageType = MessageTypeDefOf.ThreatBig;
				signalAction_Message.message = "MessageSleepingThreatDelayActivated".Translate(this.Faction);
				GenSpawn.Spawn(signalAction_Message, parms.room[0].CenterCell, parms.map, WipeMode.Vanish);
			}
			GenSpawn.Spawn(signalAction_DormancyWakeUp, parms.map.Center, parms.map, WipeMode.Vanish);
		}

		// Token: 0x06004A5E RID: 19038 RVA: 0x00189AD0 File Offset: 0x00187CD0
		private List<Pawn> SpawnThreatPawns(List<CellRect> rects, float threatPoints, Map map)
		{
			List<Pawn> list = new List<Pawn>();
			ComplexThreatWorker_SleepingThreat.tmpCells.Clear();
			ComplexThreatWorker_SleepingThreat.tmpCells.AddRange(rects.SelectMany((CellRect r) => r.Cells));
			ComplexThreatWorker_SleepingThreat.tmpCells.Shuffle<IntVec3>();
			HashSet<IntVec3> hashSet = new HashSet<IntVec3>();
			rects.Sum((CellRect r) => r.Area);
			float num = threatPoints;
			for (int i = 0; i < ComplexThreatWorker_SleepingThreat.tmpCells.Count; i++)
			{
				if (!hashSet.Contains(ComplexThreatWorker_SleepingThreat.tmpCells[i]) && ComplexThreatWorker_SleepingThreat.CanSpawnAt(ComplexThreatWorker_SleepingThreat.tmpCells[i], map))
				{
					IntVec3 loc = ComplexThreatWorker_SleepingThreat.tmpCells[i];
					IEnumerable<PawnKindDef> pawnKindsForPoints = this.GetPawnKindsForPoints(num);
					if (!pawnKindsForPoints.Any<PawnKindDef>())
					{
						break;
					}
					PawnKindDef pawnKindDef = pawnKindsForPoints.RandomElement<PawnKindDef>();
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, this.Faction);
					GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
					list.Add(pawn);
					num -= pawnKindDef.combatPower;
				}
			}
			return list;
		}

		// Token: 0x06004A5F RID: 19039 RVA: 0x00189BE8 File Offset: 0x00187DE8
		private static bool CanSpawnAt(IntVec3 c, Map map)
		{
			return c.Standable(map) && c.GetFirstPawn(map) == null && c.GetDoor(map) == null;
		}

		// Token: 0x04002D2A RID: 11562
		private const string WakeUpSignalTag = "WakeUp";

		// Token: 0x04002D2B RID: 11563
		private static readonly IntRange RadiusWakeUpDistanceRange = new IntRange(1, 4);

		// Token: 0x04002D2C RID: 11564
		private static readonly float RadialTriggerChance = 0.2f;

		// Token: 0x04002D2D RID: 11565
		private static List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
