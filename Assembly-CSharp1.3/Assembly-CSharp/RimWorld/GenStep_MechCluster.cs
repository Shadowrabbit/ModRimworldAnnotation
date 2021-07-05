using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CBB RID: 3259
	public class GenStep_MechCluster : GenStep
	{
		// Token: 0x17000D18 RID: 3352
		// (get) Token: 0x06004BE6 RID: 19430 RVA: 0x001945EB File Offset: 0x001927EB
		public override int SeedPart
		{
			get
			{
				return 341176078;
			}
		}

		// Token: 0x06004BE7 RID: 19431 RVA: 0x001945F4 File Offset: 0x001927F4
		public override void Generate(Map map, GenStepParams parms)
		{
			MechClusterSketch sketch = MechClusterGenerator.GenerateClusterSketch(parms.sitePart.parms.points, map, true, this.forceNoConditionCauser);
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

		// Token: 0x04002DE9 RID: 11753
		[Obsolete]
		public const int ExtraRangeToRectOfInterest = 20;

		// Token: 0x04002DEA RID: 11754
		public bool forceNoConditionCauser;

		// Token: 0x04002DEB RID: 11755
		public int extraRangeToRectOfInterest = 20;
	}
}
