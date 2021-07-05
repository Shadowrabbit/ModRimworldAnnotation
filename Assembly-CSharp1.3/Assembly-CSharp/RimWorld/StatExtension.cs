using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020014BF RID: 5311
	public static class StatExtension
	{
		// Token: 0x06007ECC RID: 32460 RVA: 0x002CE89C File Offset: 0x002CCA9C
		public static float GetStatValue(this Thing thing, StatDef stat, bool applyPostProcess = true)
		{
			return stat.Worker.GetValue(thing, applyPostProcess);
		}

		// Token: 0x06007ECD RID: 32461 RVA: 0x002CE8AB File Offset: 0x002CCAAB
		public static float GetStatValueForPawn(this Thing thing, StatDef stat, Pawn pawn, bool applyPostProcess = true)
		{
			return stat.Worker.GetValue(thing, pawn, applyPostProcess);
		}

		// Token: 0x06007ECE RID: 32462 RVA: 0x002CE8BB File Offset: 0x002CCABB
		public static float GetStatValueAbstract(this BuildableDef def, StatDef stat, ThingDef stuff = null)
		{
			return stat.Worker.GetValueAbstract(def, stuff);
		}

		// Token: 0x06007ECF RID: 32463 RVA: 0x002CE8CA File Offset: 0x002CCACA
		public static float GetStatValueAbstract(this AbilityDef def, StatDef stat, Pawn forPawn = null)
		{
			return stat.Worker.GetValueAbstract(def, forPawn);
		}

		// Token: 0x06007ED0 RID: 32464 RVA: 0x002CE8DC File Offset: 0x002CCADC
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

		// Token: 0x06007ED1 RID: 32465 RVA: 0x002CE920 File Offset: 0x002CCB20
		public static void SetStatBaseValue(this BuildableDef def, StatDef stat, float newBaseValue)
		{
			StatUtility.SetStatValueInList(ref def.statBases, stat, newBaseValue);
		}
	}
}
