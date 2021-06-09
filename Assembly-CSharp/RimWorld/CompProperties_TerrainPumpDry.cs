using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200187C RID: 6268
	public class CompProperties_TerrainPumpDry : CompProperties_TerrainPump
	{
		// Token: 0x06008B0F RID: 35599 RVA: 0x0005D48A File Offset: 0x0005B68A
		public CompProperties_TerrainPumpDry()
		{
			this.compClass = typeof(CompTerrainPumpDry);
		}

		// Token: 0x04005920 RID: 22816
		public SoundDef soundWorking;
	}
}
