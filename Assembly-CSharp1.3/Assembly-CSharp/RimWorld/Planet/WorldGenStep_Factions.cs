using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001787 RID: 6023
	public class WorldGenStep_Factions : WorldGenStep
	{
		// Token: 0x170016A6 RID: 5798
		// (get) Token: 0x06008AEC RID: 35564 RVA: 0x0031DF5A File Offset: 0x0031C15A
		public override int SeedPart
		{
			get
			{
				return 777998381;
			}
		}

		// Token: 0x06008AED RID: 35565 RVA: 0x0031DF61 File Offset: 0x0031C161
		public override void GenerateFresh(string seed)
		{
			FactionGenerator.GenerateFactionsIntoWorld(Current.CreatingWorld.info.factionCounts);
		}

		// Token: 0x06008AEE RID: 35566 RVA: 0x0000313F File Offset: 0x0000133F
		public override void GenerateWithoutWorldData(string seed)
		{
		}
	}
}
