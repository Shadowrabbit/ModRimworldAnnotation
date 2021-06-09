using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000F50 RID: 3920
	public class SoundParamSource_TimeOfDay : SoundParamSource
	{
		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x0600563A RID: 22074 RVA: 0x0003BD0C File Offset: 0x00039F0C
		public override string Label
		{
			get
			{
				return "Time of day (hour)";
			}
		}

		// Token: 0x0600563B RID: 22075 RVA: 0x0003BD13 File Offset: 0x00039F13
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
