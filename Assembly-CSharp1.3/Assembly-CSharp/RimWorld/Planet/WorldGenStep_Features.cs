using System;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001788 RID: 6024
	public class WorldGenStep_Features : WorldGenStep
	{
		// Token: 0x170016A7 RID: 5799
		// (get) Token: 0x06008AF0 RID: 35568 RVA: 0x0031DF77 File Offset: 0x0031C177
		public override int SeedPart
		{
			get
			{
				return 711240483;
			}
		}

		// Token: 0x06008AF1 RID: 35569 RVA: 0x0031DF80 File Offset: 0x0031C180
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
					}));
				}
			}
		}
	}
}
