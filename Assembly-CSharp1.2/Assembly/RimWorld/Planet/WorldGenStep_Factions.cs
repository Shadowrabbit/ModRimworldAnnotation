using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200209B RID: 8347
	public class WorldGenStep_Factions : WorldGenStep
	{
		// Token: 0x17001A25 RID: 6693
		// (get) Token: 0x0600B0E8 RID: 45288 RVA: 0x00072FA2 File Offset: 0x000711A2
		public override int SeedPart
		{
			get
			{
				return 777998381;
			}
		}

		// Token: 0x0600B0E9 RID: 45289 RVA: 0x00072FA9 File Offset: 0x000711A9
		public override void GenerateFresh(string seed)
		{
			FactionGenerator.GenerateFactionsIntoWorld();
		}

		// Token: 0x0600B0EA RID: 45290 RVA: 0x00006A05 File Offset: 0x00004C05
		public override void GenerateWithoutWorldData(string seed)
		{
		}
	}
}
