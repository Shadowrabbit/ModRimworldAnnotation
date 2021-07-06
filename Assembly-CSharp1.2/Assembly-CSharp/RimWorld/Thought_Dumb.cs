using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001564 RID: 5476
	public class Thought_Dumb : Thought
	{
		// Token: 0x17001261 RID: 4705
		// (get) Token: 0x060076C3 RID: 30403 RVA: 0x00050230 File Offset: 0x0004E430
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x060076C4 RID: 30404 RVA: 0x00050238 File Offset: 0x0004E438
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060076C5 RID: 30405 RVA: 0x00050241 File Offset: 0x0004E441
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		// Token: 0x04004E5C RID: 20060
		private int forcedStage;
	}
}
