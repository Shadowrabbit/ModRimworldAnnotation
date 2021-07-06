using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001DF4 RID: 7668
	public class HediffCompProperties_DissolveGearOnDeath : HediffCompProperties
	{
		// Token: 0x0600A62E RID: 42542 RVA: 0x0006DED4 File Offset: 0x0006C0D4
		public HediffCompProperties_DissolveGearOnDeath()
		{
			this.compClass = typeof(HediffComp_DissolveGearOnDeath);
		}

		// Token: 0x040070A4 RID: 28836
		public ThingDef mote;

		// Token: 0x040070A5 RID: 28837
		public int moteCount = 3;

		// Token: 0x040070A6 RID: 28838
		public FloatRange moteOffsetRange = new FloatRange(0.2f, 0.4f);

		// Token: 0x040070A7 RID: 28839
		public ThingDef filth;

		// Token: 0x040070A8 RID: 28840
		public int filthCount = 4;

		// Token: 0x040070A9 RID: 28841
		public HediffDef injuryCreatedOnDeath;

		// Token: 0x040070AA RID: 28842
		public IntRange injuryCount;

		// Token: 0x040070AB RID: 28843
		public SoundDef sound;
	}
}
