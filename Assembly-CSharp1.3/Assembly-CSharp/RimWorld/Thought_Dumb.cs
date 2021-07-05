using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E93 RID: 3731
	public class Thought_Dumb : Thought
	{
		// Token: 0x17000F3D RID: 3901
		// (get) Token: 0x060057AC RID: 22444 RVA: 0x001DD836 File Offset: 0x001DBA36
		public override int CurStageIndex
		{
			get
			{
				return this.forcedStage;
			}
		}

		// Token: 0x060057AD RID: 22445 RVA: 0x001DD83E File Offset: 0x001DBA3E
		public void SetForcedStage(int stageIndex)
		{
			this.forcedStage = stageIndex;
		}

		// Token: 0x060057AE RID: 22446 RVA: 0x001DD847 File Offset: 0x001DBA47
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.forcedStage, "stageIndex", 0, false);
		}

		// Token: 0x040033C4 RID: 13252
		private int forcedStage;
	}
}
