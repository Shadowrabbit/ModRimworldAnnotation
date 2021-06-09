using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FCB RID: 4043
	public class RaidStrategyDef : Def
	{
		// Token: 0x17000DA7 RID: 3495
		// (get) Token: 0x06005866 RID: 22630 RVA: 0x0003D6DF File Offset: 0x0003B8DF
		public RaidStrategyWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (RaidStrategyWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04003A7A RID: 14970
		public Type workerClass;

		// Token: 0x04003A7B RID: 14971
		public SimpleCurve selectionWeightPerPointsCurve;

		// Token: 0x04003A7C RID: 14972
		public float minPawns = 1f;

		// Token: 0x04003A7D RID: 14973
		[MustTranslate]
		public string arrivalTextFriendly;

		// Token: 0x04003A7E RID: 14974
		[MustTranslate]
		public string arrivalTextEnemy;

		// Token: 0x04003A7F RID: 14975
		[MustTranslate]
		public string letterLabelEnemy;

		// Token: 0x04003A80 RID: 14976
		[MustTranslate]
		public string letterLabelFriendly;

		// Token: 0x04003A81 RID: 14977
		public SimpleCurve pointsFactorCurve;

		// Token: 0x04003A82 RID: 14978
		public bool pawnsCanBringFood;

		// Token: 0x04003A83 RID: 14979
		public List<PawnsArrivalModeDef> arriveModes;

		// Token: 0x04003A84 RID: 14980
		public float raidLootValueFactor = 1f;

		// Token: 0x04003A85 RID: 14981
		private RaidStrategyWorker workerInt;
	}
}
