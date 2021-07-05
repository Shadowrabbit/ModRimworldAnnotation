using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A66 RID: 2662
	public class FeatureDef : Def
	{
		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06003FF3 RID: 16371 RVA: 0x0015AA4F File Offset: 0x00158C4F
		public FeatureWorker Worker
		{
			get
			{
				if (this.workerInt == null)
				{
					this.workerInt = (FeatureWorker)Activator.CreateInstance(this.workerClass);
					this.workerInt.def = this;
				}
				return this.workerInt;
			}
		}

		// Token: 0x04002425 RID: 9253
		public Type workerClass = typeof(FeatureWorker);

		// Token: 0x04002426 RID: 9254
		public float order;

		// Token: 0x04002427 RID: 9255
		public int minSize = 50;

		// Token: 0x04002428 RID: 9256
		public int maxSize = int.MaxValue;

		// Token: 0x04002429 RID: 9257
		public bool canTouchWorldEdge = true;

		// Token: 0x0400242A RID: 9258
		public RulePackDef nameMaker;

		// Token: 0x0400242B RID: 9259
		public int maxPossiblyAllowedSizeToTake = 30;

		// Token: 0x0400242C RID: 9260
		public float maxPossiblyAllowedSizePctOfMeToTake = 0.5f;

		// Token: 0x0400242D RID: 9261
		public List<BiomeDef> rootBiomes = new List<BiomeDef>();

		// Token: 0x0400242E RID: 9262
		public List<BiomeDef> acceptableBiomes = new List<BiomeDef>();

		// Token: 0x0400242F RID: 9263
		public int maxSpaceBetweenRootGroups = 5;

		// Token: 0x04002430 RID: 9264
		public int minRootGroupsInCluster = 3;

		// Token: 0x04002431 RID: 9265
		public int minRootGroupSize = 10;

		// Token: 0x04002432 RID: 9266
		public int maxRootGroupSize = int.MaxValue;

		// Token: 0x04002433 RID: 9267
		public int maxPassageWidth = 3;

		// Token: 0x04002434 RID: 9268
		public float maxPctOfWholeArea = 0.1f;

		// Token: 0x04002435 RID: 9269
		[Unsaved(false)]
		private FeatureWorker workerInt;
	}
}
