using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200209C RID: 8348
	public class WorldGenStep_Features : WorldGenStep
	{
		// Token: 0x17001A26 RID: 6694
		// (get) Token: 0x0600B0EC RID: 45292 RVA: 0x00072FB0 File Offset: 0x000711B0
		public override int SeedPart
		{
			get
			{
				return 711240483;
			}
		}

		// Token: 0x0600B0ED RID: 45293 RVA: 0x00335AE4 File Offset: 0x00333CE4
		public override void GenerateFresh(string seed)
		{
			Find.World.features = new WorldFeatures();
			foreach (FeatureDef featureDef in from x in DefDatabase<FeatureDef>.AllDefsListForReading
			orderby x.order, x.index
			select x)
			{
				try
				{
					featureDef.Worker.GenerateWhereAppropriate();
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Could not generate world features of def ",
						featureDef,
						": ",
						ex
					}), false);
				}
			}
		}
	}
}
