using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B8 RID: 4536
	public class CompProperties_TerrainPumpDry : CompProperties_TerrainPump
	{
		// Token: 0x06006D47 RID: 27975 RVA: 0x0024A13F File Offset: 0x0024833F
		public CompProperties_TerrainPumpDry()
		{
			this.compClass = typeof(CompTerrainPumpDry);
		}

		// Token: 0x04003CB2 RID: 15538
		public SoundDef soundWorking;
	}
}
