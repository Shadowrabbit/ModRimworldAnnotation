using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020013BD RID: 5053
	public class MedicalRecipesUtility
	{
		// Token: 0x06006D98 RID: 28056 RVA: 0x0004A782 File Offset: 0x00048982
		public static bool IsCleanAndDroppable(Pawn pawn, BodyPartRecord part)
		{
			return !pawn.Dead && !pawn.RaceProps.Animal && part.def.spawnThingOnRemoved != null && MedicalRecipesUtility.IsClean(pawn, part);
		}

		// Token: 0x06006D99 RID: 28057 RVA: 0x00219424 File Offset: 0x00217624
		public static bool IsClean(Pawn pawn, BodyPartRecord part)
		{
			return !pawn.Dead && !(from x in pawn.health.hediffSet.hediffs
			where x.Part == part
			select x).Any<Hediff>();
		}

		// Token: 0x06006D9A RID: 28058 RVA: 0x0004A7B3 File Offset: 0x000489B3
		public static void RestorePartAndSpawnAllPreviousParts(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, pos, map);
			MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, pos, map);
			pawn.health.RestorePart(part, null, true);
		}

		// Token: 0x06006D9B RID: 28059 RVA: 0x0004A7D6 File Offset: 0x000489D6
		public static Thing SpawnNaturalPartIfClean(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, part))
			{
				return GenSpawn.Spawn(part.def.spawnThingOnRemoved, pos, map, WipeMode.Vanish);
			}
			return null;
		}

		// Token: 0x06006D9C RID: 28060 RVA: 0x00219474 File Offset: 0x00217674
		public static void SpawnThingsFromHediffs(Pawn pawn, BodyPartRecord part, IntVec3 pos, Map map)
		{
			if (!pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(part))
			{
				return;
			}
			foreach (Hediff hediff in from x in pawn.health.hediffSet.hediffs
			where x.Part == part
			select x)
			{
				if (hediff.def.spawnThingOnRemoved != null)
				{
					GenSpawn.Spawn(hediff.def.spawnThingOnRemoved, pos, map, WipeMode.Vanish);
				}
			}
			for (int i = 0; i < part.parts.Count; i++)
			{
				MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part.parts[i], pos, map);
			}
		}

		// Token: 0x06006D9D RID: 28061 RVA: 0x0004A7F6 File Offset: 0x000489F6
		public static IEnumerable<BodyPartRecord> GetFixedPartsToApplyOn(RecipeDef recipe, Pawn pawn, Func<BodyPartRecord, bool> validator = null)
		{
			int num;
			for (int i = 0; i < recipe.appliedOnFixedBodyParts.Count; i = num)
			{
				BodyPartDef part = recipe.appliedOnFixedBodyParts[i];
				List<BodyPartRecord> bpList = pawn.RaceProps.body.AllParts;
				for (int j = 0; j < bpList.Count; j = num + 1)
				{
					BodyPartRecord bodyPartRecord = bpList[j];
					if (bodyPartRecord.def == part && (validator == null || validator(bodyPartRecord)))
					{
						yield return bodyPartRecord;
					}
					num = j;
				}
				part = null;
				bpList = null;
				num = i + 1;
			}
			for (int i = 0; i < recipe.appliedOnFixedBodyPartGroups.Count; i = num)
			{
				BodyPartGroupDef group = recipe.appliedOnFixedBodyPartGroups[i];
				List<BodyPartRecord> bpList = pawn.RaceProps.body.AllParts;
				for (int j = 0; j < bpList.Count; j = num + 1)
				{
					BodyPartRecord bodyPartRecord2 = bpList[j];
					if (bodyPartRecord2.groups != null && bodyPartRecord2.groups.Contains(group) && (validator == null || validator(bodyPartRecord2)))
					{
						yield return bodyPartRecord2;
					}
					num = j;
				}
				group = null;
				bpList = null;
				num = i + 1;
			}
			yield break;
		}
	}
}
