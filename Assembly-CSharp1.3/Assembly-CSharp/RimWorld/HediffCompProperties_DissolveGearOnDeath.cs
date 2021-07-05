using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200156B RID: 5483
	public class HediffCompProperties_DissolveGearOnDeath : HediffCompProperties
	{
		// Token: 0x060081C6 RID: 33222 RVA: 0x002DDEFB File Offset: 0x002DC0FB
		public HediffCompProperties_DissolveGearOnDeath()
		{
			this.compClass = typeof(HediffComp_DissolveGearOnDeath);
		}

		// Token: 0x040050BD RID: 20669
		public FleckDef fleck;

		// Token: 0x040050BE RID: 20670
		public ThingDef mote;

		// Token: 0x040050BF RID: 20671
		public int moteCount = 3;

		// Token: 0x040050C0 RID: 20672
		public FloatRange moteOffsetRange = new FloatRange(0.2f, 0.4f);

		// Token: 0x040050C1 RID: 20673
		public ThingDef filth;

		// Token: 0x040050C2 RID: 20674
		public int filthCount = 4;

		// Token: 0x040050C3 RID: 20675
		public HediffDef injuryCreatedOnDeath;

		// Token: 0x040050C4 RID: 20676
		public IntRange injuryCount;

		// Token: 0x040050C5 RID: 20677
		public SoundDef sound;
	}
}
