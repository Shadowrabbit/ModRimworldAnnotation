using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D22 RID: 7458
	public static class StatExtension
	{
		// Token: 0x0600A21F RID: 41503 RVA: 0x0006BC27 File Offset: 0x00069E27
		public static float GetStatValue(this Thing thing, StatDef stat, bool applyPostProcess = true)
		{
			return stat.Worker.GetValue(thing, applyPostProcess);
		}

		// Token: 0x0600A220 RID: 41504 RVA: 0x0006BC36 File Offset: 0x00069E36
		public static float GetStatValueForPawn(this Thing thing, StatDef stat, Pawn pawn, bool applyPostProcess = true)
		{
			return stat.Worker.GetValue(thing, pawn, applyPostProcess);
		}

		// Token: 0x0600A221 RID: 41505 RVA: 0x0006BC46 File Offset: 0x00069E46
		public static float GetStatValueAbstract(this BuildableDef def, StatDef stat, ThingDef stuff = null)
		{
			return stat.Worker.GetValueAbstract(def, stuff);
		}

		// Token: 0x0600A222 RID: 41506 RVA: 0x0006BC55 File Offset: 0x00069E55
		public static float GetStatValueAbstract(this AbilityDef def, StatDef stat)
		{
			return stat.Worker.GetValueAbstract(def);
		}

		// Token: 0x0600A223 RID: 41507 RVA: 0x002F4BEC File Offset: 0x002F2DEC
		public static bool StatBaseDefined(this BuildableDef def, StatDef stat)
		{
			if (def.statBases == null)
			{
				return false;
			}
			for (int i = 0; i < def.statBases.Count; i++)
			{
				if (def.statBases[i].stat == stat)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600A224 RID: 41508 RVA: 0x0006BC63 File Offset: 0x00069E63
		public static void SetStatBaseValue(this BuildableDef def, StatDef stat, float newBaseValue)
		{
			StatUtility.SetStatValueInList(ref def.statBases, stat, newBaseValue);
		}
	}
}
