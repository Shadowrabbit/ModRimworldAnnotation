using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000BDA RID: 3034
	public static class GameConditionUtility
	{
		// Token: 0x06004756 RID: 18262 RVA: 0x00179A0D File Offset: 0x00177C0D
		public static float LerpInOutValue(GameCondition gameCondition, float lerpTime, float lerpTarget = 1f)
		{
			if (gameCondition.Permanent)
			{
				return GameConditionUtility.LerpInOutValue((float)gameCondition.TicksPassed, lerpTime + 1f, lerpTime, lerpTarget);
			}
			return GameConditionUtility.LerpInOutValue((float)gameCondition.TicksPassed, (float)gameCondition.TicksLeft, lerpTime, lerpTarget);
		}

		// Token: 0x06004757 RID: 18263 RVA: 0x00179A44 File Offset: 0x00177C44
		public static float LerpInOutValue(float timePassed, float timeLeft, float lerpTime, float lerpTarget = 1f)
		{
			float num = 1f;
			if (timePassed < lerpTime)
			{
				num = timePassed / lerpTime;
			}
			if (timeLeft < lerpTime)
			{
				num = Mathf.Min(num, timeLeft / lerpTime);
			}
			return Mathf.Lerp(0f, lerpTarget, num);
		}
	}
}
