using System;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020006AD RID: 1709
	public class Trigger_NoPawnsVeryTiredAndSleeping : Trigger
	{
		// Token: 0x06002F67 RID: 12135 RVA: 0x00118D72 File Offset: 0x00116F72
		public Trigger_NoPawnsVeryTiredAndSleeping(float extraRestThreshOffset = 0f)
		{
			this.extraRestThreshOffset = extraRestThreshOffset;
		}

		// Token: 0x06002F68 RID: 12136 RVA: 0x00118D84 File Offset: 0x00116F84
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

		// Token: 0x04001CF4 RID: 7412
		private float extraRestThreshOffset;
	}
}
