using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02001171 RID: 4465
	public static class GameConditionUtility
	{
		// Token: 0x06006254 RID: 25172 RVA: 0x00043A73 File Offset: 0x00041C73
		public static float LerpInOutValue(GameCondition gameCondition, float lerpTime, float lerpTarget = 1f)
		{
			if (gameCondition.Permanent)
			{
				return GameConditionUtility.LerpInOutValue((float)gameCondition.TicksPassed, lerpTime + 1f, lerpTime, lerpTarget);
			}
			return GameConditionUtility.LerpInOutValue((float)gameCondition.TicksPassed, (float)gameCondition.TicksLeft, lerpTime, lerpTarget);
		}

		// Token: 0x06006255 RID: 25173 RVA: 0x001EBA38 File Offset: 0x001E9C38
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
