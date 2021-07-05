using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000A34 RID: 2612
	public class SoundParamSource_TimeOfDay : SoundParamSource
	{
		// Token: 0x17000B07 RID: 2823
		// (get) Token: 0x06003F51 RID: 16209 RVA: 0x00158B18 File Offset: 0x00156D18
		public override string Label
		{
			get
			{
				return "Time of day (hour)";
			}
		}

		// Token: 0x06003F52 RID: 16210 RVA: 0x00158B1F File Offset: 0x00156D1F
		public override float ValueFor(Sample samp)
		{
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			return GenLocalDate.HourFloat(Find.CurrentMap);
		}
	}
}
