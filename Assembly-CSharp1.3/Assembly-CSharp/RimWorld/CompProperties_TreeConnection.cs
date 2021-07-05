using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011C7 RID: 4551
	public class CompProperties_TreeConnection : CompProperties
	{
		// Token: 0x06006DB7 RID: 28087 RVA: 0x0024C100 File Offset: 0x0024A300
		public CompProperties_TreeConnection()
		{
			this.compClass = typeof(CompTreeConnection);
		}

		// Token: 0x04003CE7 RID: 15591
		public float spawnDays = 8f;

		// Token: 0x04003CE8 RID: 15592
		public PawnKindDef pawnKind;

		// Token: 0x04003CE9 RID: 15593
		public float initialConnectionStrength = 0.35f;

		// Token: 0x04003CEA RID: 15594
		public float connectionStrengthLossPerDryadDeath = 0.1f;

		// Token: 0x04003CEB RID: 15595
		public float radiusToBuildingForConnectionStrengthLoss = 7.9f;

		// Token: 0x04003CEC RID: 15596
		public int maxDryadsWild;

		// Token: 0x04003CED RID: 15597
		public SimpleCurve maxDryadsPerConnectionStrengthCurve;

		// Token: 0x04003CEE RID: 15598
		public SimpleCurve connectionLossPerLevelCurve;

		// Token: 0x04003CEF RID: 15599
		public SimpleCurve connectionLossDailyPerBuildingDistanceCurve;

		// Token: 0x04003CF0 RID: 15600
		public SimpleCurve connectionStrengthGainPerPlantSkill;

		// Token: 0x04003CF1 RID: 15601
		public float connectionStrengthGainPerHourPruningBase = 0.01f;

		// Token: 0x04003CF2 RID: 15602
		public Vector3 spawningPodOffset;

		// Token: 0x04003CF3 RID: 15603
		public FloatRange spawningPodSizeRange = FloatRange.One;
	}
}
