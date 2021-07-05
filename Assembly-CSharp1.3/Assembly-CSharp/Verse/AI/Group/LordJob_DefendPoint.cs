using System;

namespace Verse.AI.Group
{
	// Token: 0x02000662 RID: 1634
	public class LordJob_DefendPoint : LordJob
	{
		// Token: 0x170008B4 RID: 2228
		// (get) Token: 0x06002E6E RID: 11886 RVA: 0x0011607F File Offset: 0x0011427F
		public override bool IsCaravanSendable
		{
			get
			{
				return this.isCaravanSendable;
			}
		}

		// Token: 0x170008B5 RID: 2229
		// (get) Token: 0x06002E6F RID: 11887 RVA: 0x00116087 File Offset: 0x00114287
		public override bool AddFleeToil
		{
			get
			{
				return this.addFleeToil;
			}
		}

		// Token: 0x06002E70 RID: 11888 RVA: 0x0011608F File Offset: 0x0011428F
		public LordJob_DefendPoint()
		{
		}

		// Token: 0x06002E71 RID: 11889 RVA: 0x00116097 File Offset: 0x00114297
		public LordJob_DefendPoint(IntVec3 point, float? wanderRadius = null, bool isCaravanSendable = false, bool addFleeToil = true)
		{
			this.point = point;
			this.wanderRadius = wanderRadius;
			this.isCaravanSendable = isCaravanSendable;
			this.addFleeToil = addFleeToil;
		}

		// Token: 0x06002E72 RID: 11890 RVA: 0x001160BC File Offset: 0x001142BC
		public override StateGraph CreateGraph()
		{
			StateGraph stateGraph = new StateGraph();
			stateGraph.AddToil(new LordToil_DefendPoint(this.point, 28f, this.wanderRadius));
			return stateGraph;
		}

		// Token: 0x06002E73 RID: 11891 RVA: 0x001160E0 File Offset: 0x001142E0
		public override void ExposeData()
		{
			Scribe_Values.Look<IntVec3>(ref this.point, "point", default(IntVec3), false);
			Scribe_Values.Look<float?>(ref this.wanderRadius, "wanderRadius", null, false);
			Scribe_Values.Look<bool>(ref this.isCaravanSendable, "isCaravanSendable", false, false);
			Scribe_Values.Look<bool>(ref this.addFleeToil, "addFleeToil", false, false);
		}

		// Token: 0x04001C84 RID: 7300
		private IntVec3 point;

		// Token: 0x04001C85 RID: 7301
		private float? wanderRadius;

		// Token: 0x04001C86 RID: 7302
		private bool isCaravanSendable;

		// Token: 0x04001C87 RID: 7303
		private bool addFleeToil;
	}
}
