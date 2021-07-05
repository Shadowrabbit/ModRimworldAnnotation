using System;
using System.Collections.Generic;

namespace RimWorld
{
	// Token: 0x020014F0 RID: 5360
	public static class StatUtility
	{
		// Token: 0x06007FB1 RID: 32689 RVA: 0x002D16E4 File Offset: 0x002CF8E4
		public static void SetStatValueInList(ref List<StatModifier> statList, StatDef stat, float value)
		{
			if (statList == null)
			{
				statList = new List<StatModifier>();
			}
			for (int i = 0; i < statList.Count; i++)
			{
				if (statList[i].stat == stat)
				{
					statList[i].value = value;
					return;
				}
			}
			StatModifier statModifier = new StatModifier();
			statModifier.stat = stat;
			statModifier.value = value;
			statList.Add(statModifier);
		}

		// Token: 0x06007FB2 RID: 32690 RVA: 0x002D1749 File Offset: 0x002CF949
		public static float GetStatFactorFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 1f);
		}

		// Token: 0x06007FB3 RID: 32691 RVA: 0x002D1757 File Offset: 0x002CF957
		public static float GetStatOffsetFromList(this List<StatModifier> modList, StatDef stat)
		{
			return modList.GetStatValueFromList(stat, 0f);
		}

		// Token: 0x06007FB4 RID: 32692 RVA: 0x002D1768 File Offset: 0x002CF968
		public static float GetStatValueFromList(this List<StatModifier> modList, StatDef stat, float defaultValue)
		{
			if (modList != null)
			{
				for (int i = 0; i < modList.Count; i++)
				{
					if (modList[i].stat == stat)
					{
						return modList[i].value;
					}
				}
			}
			return defaultValue;
		}

		// Token: 0x06007FB5 RID: 32693 RVA: 0x002D17A8 File Offset: 0x002CF9A8
		public static bool StatListContains(this List<StatModifier> modList, StatDef stat)
		{
			if (modList != null)
			{
				for (int i = 0; i < modList.Count; i++)
				{
					if (modList[i].stat == stat)
					{
						return true;
					}
				}
			}
			return false;
		}
	}
}
