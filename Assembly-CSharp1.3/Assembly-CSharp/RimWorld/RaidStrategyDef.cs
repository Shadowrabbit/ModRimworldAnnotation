using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000AAD RID: 2733
	public class RaidStrategyDef : Def
	{
		// Token: 0x17000B67 RID: 2919
		// (get) Token: 0x060040EA RID: 16618 RVA: 0x0015E4D4 File Offset: 0x0015C6D4
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

		// Token: 0x0400263C RID: 9788
		public Type workerClass;

		// Token: 0x0400263D RID: 9789
		public SimpleCurve selectionWeightPerPointsCurve;

		// Token: 0x0400263E RID: 9790
		public float minPawns = 1f;

		// Token: 0x0400263F RID: 9791
		[MustTranslate]
		public string arrivalTextFriendly;

		// Token: 0x04002640 RID: 9792
		[MustTranslate]
		public string arrivalTextEnemy;

		// Token: 0x04002641 RID: 9793
		[MustTranslate]
		public string letterLabelEnemy;

		// Token: 0x04002642 RID: 9794
		[MustTranslate]
		public string letterLabelFriendly;

		// Token: 0x04002643 RID: 9795
		public SimpleCurve pointsFactorCurve;

		// Token: 0x04002644 RID: 9796
		public bool pawnsCanBringFood;

		// Token: 0x04002645 RID: 9797
		public List<PawnsArrivalModeDef> arriveModes;

		// Token: 0x04002646 RID: 9798
		public float raidLootValueFactor = 1f;

		// Token: 0x04002647 RID: 9799
		private RaidStrategyWorker workerInt;
	}
}
