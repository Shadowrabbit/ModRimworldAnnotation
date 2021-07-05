using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C9E RID: 3230
	public class GenStep_ScenParts : GenStep
	{
		// Token: 0x17000CFF RID: 3327
		// (get) Token: 0x06004B68 RID: 19304 RVA: 0x001908B1 File Offset: 0x0018EAB1
		public override int SeedPart
		{
			get
			{
				return 1561683158;
			}
		}

		// Token: 0x06004B69 RID: 19305 RVA: 0x001908B8 File Offset: 0x0018EAB8
		public override void Generate(Map map, GenStepParams parms)
		{
			Find.Scenario.GenerateIntoMap(map);
		}
	}
}
