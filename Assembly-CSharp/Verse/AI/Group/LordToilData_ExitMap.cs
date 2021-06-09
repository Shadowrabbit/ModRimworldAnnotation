using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AD5 RID: 2773
	public class LordToilData_ExitMap : LordToilData
	{
		// Token: 0x0600419E RID: 16798 RVA: 0x00030DEA File Offset: 0x0002EFEA
		public override void ExposeData()
		{
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.interruptCurrentJob, "interruptCurrentJob", false, false);
		}

		// Token: 0x04002D2D RID: 11565
		public LocomotionUrgency locomotion;

		// Token: 0x04002D2E RID: 11566
		public bool canDig;

		// Token: 0x04002D2F RID: 11567
		public bool interruptCurrentJob;
	}
}
