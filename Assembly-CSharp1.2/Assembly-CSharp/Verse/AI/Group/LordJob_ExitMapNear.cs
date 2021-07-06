using System;

namespace Verse.AI.Group
{
	// Token: 0x02000AC9 RID: 2761
	public class LordJob_ExitMapNear : LordJob
	{
		// Token: 0x0600415B RID: 16731 RVA: 0x00030C06 File Offset: 0x0002EE06
		public LordJob_ExitMapNear()
		{
		}

		// Token: 0x0600415C RID: 16732 RVA: 0x00030C15 File Offset: 0x0002EE15
		public LordJob_ExitMapNear(IntVec3 near, LocomotionUrgency locomotion, float radius = 12f, bool canDig = false, bool useAvoidGridSmart = false)
		{
			this.near = near;
			this.locomotion = locomotion;
			this.radius = radius;
			this.canDig = canDig;
			this.useAvoidGridSmart = useAvoidGridSmart;
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x001875D0 File Offset: 0x001857D0
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			LordToil_ExitMapNear lordToil_ExitMapNear = new LordToil_ExitMapNear(this.near, this.radius, this.locomotion, this.canDig);
			if (this.useAvoidGridSmart)
			{
				lordToil_ExitMapNear.useAvoidGrid = true;
			}
			stateGraph.AddToil(lordToil_ExitMapNear);
			return stateGraph;
		}

		// Token: 0x0600415E RID: 16734 RVA: 0x00187618 File Offset: 0x00185818
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.near, "near", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
		}

		// Token: 0x04002D1B RID: 11547
		private IntVec3 near;

		// Token: 0x04002D1C RID: 11548
		private float radius;

		// Token: 0x04002D1D RID: 11549
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04002D1E RID: 11550
		private bool canDig;

		// Token: 0x04002D1F RID: 11551
		private bool useAvoidGridSmart;

		// Token: 0x04002D20 RID: 11552
		public const float DefaultRadius = 12f;
	}
}
