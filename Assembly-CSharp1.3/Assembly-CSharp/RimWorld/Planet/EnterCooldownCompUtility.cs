using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x020017F2 RID: 6130
	public static class EnterCooldownCompUtility
	{
		// Token: 0x06008EFF RID: 36607 RVA: 0x0033478C File Offset: 0x0033298C
		public static bool EnterCooldownBlocksEntering(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return component != null && component.BlocksEntering;
		}

		// Token: 0x06008F00 RID: 36608 RVA: 0x003347AC File Offset: 0x003329AC
		public static float EnterCooldownDaysLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			if (component == null)
			{
				return 0f;
			}
			return component.DaysLeft;
		}

		// Token: 0x06008F01 RID: 36609 RVA: 0x003347D0 File Offset: 0x003329D0
		public static float EnterCooldownHoursLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			if (component == null)
			{
				return 0f;
			}
			return component.DaysLeft * 24f;
		}

		// Token: 0x06008F02 RID: 36610 RVA: 0x003347FC File Offset: 0x003329FC
		public static int EnterCooldownTicksLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return Mathf.CeilToInt((component != null) ? (component.DaysLeft * 60000f) : 0f);
		}
	}
}
