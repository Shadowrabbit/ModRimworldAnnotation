using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000FAF RID: 4015
	public static class RitualUility
	{
		// Token: 0x06005ECD RID: 24269 RVA: 0x00207458 File Offset: 0x00205658
		public static bool IsRitualTarget(this Thing thing)
		{
			return thing.TargetOfRitual() != null;
		}

		// Token: 0x06005ECE RID: 24270 RVA: 0x00207464 File Offset: 0x00205664
		public static LordJob_Ritual TargetOfRitual(this Thing thing)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			using (List<Lord>.Enumerator enumerator = thing.Map.lordManager.lords.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					LordJob_Ritual lordJob_Ritual;
					if ((lordJob_Ritual = (enumerator.Current.LordJob as LordJob_Ritual)) != null && lordJob_Ritual.selectedTarget.Thing == thing)
					{
						LordToilData_Gathering lordToilData_Gathering = lordJob_Ritual.lord.CurLordToil.data as LordToilData_Gathering;
						if (lordToilData_Gathering != null && lordToilData_Gathering.presentForTicks.Any<KeyValuePair<Pawn, int>>())
						{
							return lordJob_Ritual;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06005ECF RID: 24271 RVA: 0x0020750C File Offset: 0x0020570C
		public static bool GoodSpectateCellForRitual(IntVec3 spot, Pawn p, Map map)
		{
			return !spot.ContainsStaticFire(map) && !spot.GetTerrain(map).avoidWander && !PawnUtility.KnownDangerAt(spot, map, p);
		}
	}
}
