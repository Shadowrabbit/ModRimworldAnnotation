using System;

namespace Verse.AI.Group
{
	// Token: 0x02000670 RID: 1648
	public class LordToilData_ExitMap : LordToilData
	{
		// Token: 0x06002EC1 RID: 11969 RVA: 0x00117020 File Offset: 0x00115220
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.interruptCurrentJob, "interruptCurrentJob", false, false);
		}

		// Token: 0x04001C9E RID: 7326
		public LocomotionUrgency locomotion;

		// Token: 0x04001C9F RID: 7327
		public bool canDig;

		// Token: 0x04001CA0 RID: 7328
		public bool interruptCurrentJob;
	}
}
