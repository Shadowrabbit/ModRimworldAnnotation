using System;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x02002196 RID: 8598
	public static class EnterCooldownCompUtility
	{
		// Token: 0x0600B79E RID: 47006 RVA: 0x0034EFF0 File Offset: 0x0034D1F0
		public static bool EnterCooldownBlocksEntering(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return component != null && component.BlocksEntering;
		}

		// Token: 0x0600B79F RID: 47007 RVA: 0x0034F010 File Offset: 0x0034D210
		public static float EnterCooldownDaysLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			if (component == null)
			{
				return 0f;
			}
			return component.DaysLeft;
		}

		// Token: 0x0600B7A0 RID: 47008 RVA: 0x0034F034 File Offset: 0x0034D234
		public static float EnterCooldownHoursLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			if (component == null)
			{
				return 0f;
			}
			return component.DaysLeft * 24f;
		}

		// Token: 0x0600B7A1 RID: 47009 RVA: 0x0034F060 File Offset: 0x0034D260
		public static int EnterCooldownTicksLeft(this MapParent worldObject)
		{
			EnterCooldownComp component = worldObject.GetComponent<EnterCooldownComp>();
			return Mathf.CeilToInt((component != null) ? (component.DaysLeft * 60000f) : 0f);
		}
	}
}
