using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012AA RID: 4778
	public class GenStep_ScenParts : GenStep
	{
		// Token: 0x17001002 RID: 4098
		// (get) Token: 0x060067CD RID: 26573 RVA: 0x00046BE9 File Offset: 0x00044DE9
		public override int SeedPart
		{
			get
			{
				return 1561683158;
			}
		}

		// Token: 0x060067CE RID: 26574 RVA: 0x00046BF0 File Offset: 0x00044DF0
		public override void Generate(Map map, GenStepParams parms)
		{
			Find.Scenario.GenerateIntoMap(map);
		}
	}
}
