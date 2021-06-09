using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020012C9 RID: 4809
	public class GenStep_MechCluster : GenStep
	{
		// Token: 0x1700100D RID: 4109
		// (get) Token: 0x06006834 RID: 26676 RVA: 0x00046F34 File Offset: 0x00045134
		public override int SeedPart
		{
			get
			{
				return 341176078;
			}
		}

		// Token: 0x06006835 RID: 26677 RVA: 0x002025DC File Offset: 0x002007DC
		public override void Generate(Map map, GenStepParams parms)
		{
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch_NewTemp(GenStep_MechCluster.DefaultPointsRange.RandomInRange, map, true, this.forceNoConditionCauser);
			IntVec3 center = IntVec3.Invalid;
			CellRect cellRect;
			if (MapGenerator.TryGetVar<CellRect>("RectOfInterest", out cellRect))
			{
				center = cellRect.ExpandedBy(this.extraRangeToRectOfInterest).MaxBy((IntVec3 x) => MechClusterUtility.GetClusterPositionScore(x, map, sketch));
			}
			if (!center.IsValid)
			{
				center = MechClusterUtility.FindClusterPosition(map, sketch, 100, 0f);
			}
			List<Thing> list = MechClusterUtility.SpawnCluster(center, map, sketch, false, false, null);
			List<Pawn> list2 = new List<Pawn>();
			foreach (Thing thing in list)
			{
				if (thing is Pawn)
				{
					list2.Add((Pawn)thing);
				}
			}
			if (!list2.Any<Pawn>())
			{
				return;
			}
			GenStep_SleepingMechanoids.SendMechanoidsToSleepImmediately(list2);
		}

		// Token: 0x0400455F RID: 17759
		[Obsolete]
		public const int ExtraRangeToRectOfInterest = 20;

		// Token: 0x04004560 RID: 17760
		public static readonly FloatRange DefaultPointsRange = new FloatRange(750f, 2500f);

		// Token: 0x04004561 RID: 17761
		public bool forceNoConditionCauser;

		// Token: 0x04004562 RID: 17762
		public int extraRangeToRectOfInterest = 20;
	}
}
