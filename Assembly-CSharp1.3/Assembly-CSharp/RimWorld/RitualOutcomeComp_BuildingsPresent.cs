using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F5D RID: 3933
	public abstract class RitualOutcomeComp_BuildingsPresent : RitualOutcomeComp_QualitySingleOffset
	{
		// Token: 0x06005D4F RID: 23887 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual int RequiredAmount(RitualRoleAssignments assignments)
		{
			return 1;
		}

		// Token: 0x06005D50 RID: 23888 RVA: 0x001FFF3C File Offset: 0x001FE13C
		protected virtual string LabelForPredictedOutcomeDesc(Precept_Ritual ritual)
		{
			return this.LabelForDesc;
		}

		// Token: 0x06005D51 RID: 23889
		protected abstract Thing LookForBuilding(IntVec3 cell, Map map, Precept_Ritual ritual);

		// Token: 0x06005D52 RID: 23890 RVA: 0x001FFF44 File Offset: 0x001FE144
		protected virtual int CountAvailable(Precept_Ritual ritual, TargetInfo ritualTarget)
		{
			RitualOutcomeComp_BuildingsPresent.<>c__DisplayClass4_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.ritualTarget = ritualTarget;
			CS$<>8__locals1.ritual = ritual;
			int result;
			try
			{
				bool usesLectern = CS$<>8__locals1.ritual.behavior.def.UsesLectern;
				RitualOutcomeComp_BuildingsPresent.<>c__DisplayClass4_1 CS$<>8__locals2;
				CS$<>8__locals2.lecternPos = CS$<>8__locals1.ritual.behavior.def.FirstLecternPosition;
				CS$<>8__locals2.allLecterns = (usesLectern ? CS$<>8__locals1.ritualTarget.Map.listerThings.ThingsOfDef(ThingDefOf.Lectern) : null);
				int num = 0;
				if (GatheringsUtility.UseWholeRoomAsGatheringArea(CS$<>8__locals1.ritualTarget.Cell, CS$<>8__locals1.ritualTarget.Map))
				{
					using (IEnumerator<IntVec3> enumerator = CS$<>8__locals1.ritualTarget.Cell.GetRoom(CS$<>8__locals1.ritualTarget.Map).Cells.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							IntVec3 cell = enumerator.Current;
							if (this.<CountAvailable>g__Check|4_0(cell, ref CS$<>8__locals1, ref CS$<>8__locals2))
							{
								num++;
							}
						}
						goto IL_144;
					}
				}
				foreach (IntVec3 cell2 in CellRect.CenteredOn(CS$<>8__locals1.ritualTarget.Cell, 18))
				{
					if (this.<CountAvailable>g__Check|4_0(cell2, ref CS$<>8__locals1, ref CS$<>8__locals2))
					{
						num++;
					}
				}
				IL_144:
				result = num;
			}
			finally
			{
				RitualOutcomeComp_BuildingsPresent.tmpThingsAlreadyCounted.Clear();
			}
			return result;
		}

		// Token: 0x06005D53 RID: 23891 RVA: 0x002000F4 File Offset: 0x001FE2F4
		private float CalcQualityOffset(int available, int required)
		{
			return this.qualityOffset * Mathf.Clamp01((float)available / (float)required);
		}

		// Token: 0x06005D54 RID: 23892 RVA: 0x00200107 File Offset: 0x001FE307
		public override float QualityOffset(LordJob_Ritual ritual, RitualOutcomeComp_Data data)
		{
			return this.CalcQualityOffset(this.CountAvailable(ritual.Ritual, ritual.selectedTarget), this.RequiredAmount(ritual.assignments));
		}

		// Token: 0x06005D55 RID: 23893 RVA: 0x0020012D File Offset: 0x001FE32D
		public override bool Applies(LordJob_Ritual ritual)
		{
			return this.CountAvailable(ritual.Ritual, ritual.selectedTarget) > 0;
		}

		// Token: 0x06005D56 RID: 23894 RVA: 0x00200144 File Offset: 0x001FE344
		public override ExpectedOutcomeDesc GetExpectedOutcomeDesc(Precept_Ritual ritual, TargetInfo ritualTarget, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			int num = this.RequiredAmount(assignments);
			int num2 = this.CountAvailable(ritual, ritualTarget);
			string count = null;
			if (num != 1)
			{
				count = num2 + " / " + num;
			}
			float quality = this.CalcQualityOffset(num2, num);
			return new ExpectedOutcomeDesc
			{
				label = this.LabelForPredictedOutcomeDesc(ritual),
				effect = this.ExpectedOffsetDesc(num2 > 0, quality),
				present = (num2 > 0),
				quality = quality,
				positive = (num2 > 0),
				priority = 1f,
				count = count
			};
		}

		// Token: 0x06005D59 RID: 23897 RVA: 0x002001E8 File Offset: 0x001FE3E8
		[CompilerGenerated]
		private bool <CountAvailable>g__Check|4_0(IntVec3 cell, ref RitualOutcomeComp_BuildingsPresent.<>c__DisplayClass4_0 A_2, ref RitualOutcomeComp_BuildingsPresent.<>c__DisplayClass4_1 A_3)
		{
			Thing thing = this.LookForBuilding(cell, A_2.ritualTarget.Map, A_2.ritual);
			if (thing != null && GatheringsUtility.InGatheringArea(cell, A_2.ritualTarget.Cell, A_2.ritualTarget.Map))
			{
				IntVec3? intVec = null;
				if (A_3.allLecterns != null)
				{
					foreach (Thing thing2 in A_3.allLecterns)
					{
						if (GatheringsUtility.InGatheringArea(thing2.Position, A_2.ritualTarget.Cell, A_2.ritualTarget.Map) && A_3.lecternPos.IsUsableThing(thing2, A_2.ritualTarget.Cell, A_2.ritualTarget))
						{
							intVec = new IntVec3?(thing2.Position);
							break;
						}
					}
				}
				if (intVec == null)
				{
					intVec = new IntVec3?(A_2.ritualTarget.Thing.Position);
				}
				if (thing.def.building == null || !thing.def.building.isSittable || SpectatorCellFinder.CorrectlyRotatedChairAt(thing.Position, thing.Map, CellRect.CenteredOn(intVec.Value, 1)))
				{
					return RitualOutcomeComp_BuildingsPresent.tmpThingsAlreadyCounted.Add(thing);
				}
			}
			return false;
		}

		// Token: 0x040035FC RID: 13820
		private static HashSet<Thing> tmpThingsAlreadyCounted = new HashSet<Thing>();
	}
}
