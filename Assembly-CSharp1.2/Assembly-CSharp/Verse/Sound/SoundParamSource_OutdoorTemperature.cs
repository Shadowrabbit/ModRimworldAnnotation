using System;

namespace Verse.Sound
{
	// Token: 0x02000925 RID: 2341
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		// Token: 0x1700092E RID: 2350
		// (get) Token: 0x060039D0 RID: 14800 RVA: 0x0002C9B5 File Offset: 0x0002ABB5
		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		// Token: 0x060039D1 RID: 14801 RVA: 0x0002C9BC File Offset: 0x0002ABBC
		public override float ValueFor(Sample samp)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return 0f;
			}
			if (Find.CurrentMap == null)
			{
				return 0f;
			}
			return Find.CurrentMap.mapTemperature.OutdoorTemp;
		}
	}
}
