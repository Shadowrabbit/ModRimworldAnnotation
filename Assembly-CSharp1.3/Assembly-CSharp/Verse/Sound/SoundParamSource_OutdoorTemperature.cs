using System;

namespace Verse.Sound
{
	// Token: 0x02000550 RID: 1360
	public class SoundParamSource_OutdoorTemperature : SoundParamSource
	{
		// Token: 0x170007F2 RID: 2034
		// (get) Token: 0x06002895 RID: 10389 RVA: 0x000F6F59 File Offset: 0x000F5159
		public override string Label
		{
			get
			{
				return "Outdoor temperature";
			}
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000F6F60 File Offset: 0x000F5160
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
