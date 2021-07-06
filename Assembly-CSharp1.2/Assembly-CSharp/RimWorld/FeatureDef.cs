using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F8E RID: 3982
	public class FeatureDef : Def
	{
		// Token: 0x17000D79 RID: 3449
		// (get) Token: 0x06005763 RID: 22371 RVA: 0x0003C99D File Offset: 0x0003AB9D
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

		// Token: 0x0400390E RID: 14606
		public Type workerClass = typeof(FeatureWorker);

		// Token: 0x0400390F RID: 14607
		public float order;

		// Token: 0x04003910 RID: 14608
		public int minSize = 50;

		// Token: 0x04003911 RID: 14609
		public int maxSize = int.MaxValue;

		// Token: 0x04003912 RID: 14610
		public bool canTouchWorldEdge = true;

		// Token: 0x04003913 RID: 14611
		public RulePackDef nameMaker;

		// Token: 0x04003914 RID: 14612
		public int maxPossiblyAllowedSizeToTake = 30;

		// Token: 0x04003915 RID: 14613
		public float maxPossiblyAllowedSizePctOfMeToTake = 0.5f;

		// Token: 0x04003916 RID: 14614
		public List<BiomeDef> rootBiomes = new List<BiomeDef>();

		// Token: 0x04003917 RID: 14615
		public List<BiomeDef> acceptableBiomes = new List<BiomeDef>();

		// Token: 0x04003918 RID: 14616
		public int maxSpaceBetweenRootGroups = 5;

		// Token: 0x04003919 RID: 14617
		public int minRootGroupsInCluster = 3;

		// Token: 0x0400391A RID: 14618
		public int minRootGroupSize = 10;

		// Token: 0x0400391B RID: 14619
		public int maxRootGroupSize = int.MaxValue;

		// Token: 0x0400391C RID: 14620
		public int maxPassageWidth = 3;

		// Token: 0x0400391D RID: 14621
		public float maxPctOfWholeArea = 0.1f;

		// Token: 0x0400391E RID: 14622
		[Unsaved(false)]
		private FeatureWorker workerInt;
	}
}
