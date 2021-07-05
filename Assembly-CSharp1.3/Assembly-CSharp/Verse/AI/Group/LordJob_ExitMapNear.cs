using System;

namespace Verse.AI.Group
{
	// Token: 0x02000664 RID: 1636
	public class LordJob_ExitMapNear : LordJob
	{
		// Token: 0x06002E78 RID: 11896 RVA: 0x0011623C File Offset: 0x0011443C
		public LordJob_ExitMapNear()
		{
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x0011624B File Offset: 0x0011444B
		public LordJob_ExitMapNear(IntVec3 near, LocomotionUrgency locomotion, float radius = 12f, bool canDig = false, bool useAvoidGridSmart = false)
		{
			this.near = near;
			this.locomotion = locomotion;
			this.radius = radius;
			this.canDig = canDig;
			this.useAvoidGridSmart = useAvoidGridSmart;
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x00116280 File Offset: 0x00114480
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

		// Token: 0x06002E7B RID: 11899 RVA: 0x001162C8 File Offset: 0x001144C8
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.near, "near", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.radius, "radius", 0f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.Jog, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<bool>(ref this.useAvoidGridSmart, "useAvoidGridSmart", false, false);
		}

		// Token: 0x04001C8B RID: 7307
		private IntVec3 near;

		// Token: 0x04001C8C RID: 7308
		private float radius;

		// Token: 0x04001C8D RID: 7309
		private LocomotionUrgency locomotion = LocomotionUrgency.Jog;

		// Token: 0x04001C8E RID: 7310
		private bool canDig;

		// Token: 0x04001C8F RID: 7311
		private bool useAvoidGridSmart;

		// Token: 0x04001C90 RID: 7312
		public const float DefaultRadius = 12f;
	}
}
