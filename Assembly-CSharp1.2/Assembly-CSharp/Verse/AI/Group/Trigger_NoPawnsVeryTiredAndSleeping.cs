using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x02000B0F RID: 2831
	public class Trigger_NoPawnsVeryTiredAndSleeping : Trigger
	{
		// Token: 0x06004239 RID: 16953 RVA: 0x000315A6 File Offset: 0x0002F7A6
		public Trigger_NoPawnsVeryTiredAndSleeping(float extraRestThreshOffset = 0f)
		{
			this.extraRestThreshOffset = extraRestThreshOffset;
		}

		// Token: 0x0600423A RID: 16954 RVA: 0x001892D0 File Offset: 0x001874D0
		public override bool ActivateOn(Lord lord, TriggerSignal signal)
		{
			if (signal.type == TriggerSignalType.Tick)
			{
				for (int i = 0; i < lord.ownedPawns.Count; i++)
				{
					Need_Rest rest = lord.ownedPawns[i].needs.rest;
					if (rest != null && rest.CurLevelPercentage < 0.14f + this.extraRestThreshOffset && !lord.ownedPawns[i].Awake())
					{
						return false;
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x04002D69 RID: 11625
		private float extraRestThreshOffset;
	}
}
