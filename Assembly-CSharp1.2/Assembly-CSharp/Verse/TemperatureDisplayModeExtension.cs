using System;

namespace Verse
{
	// Token: 0x02000889 RID: 2185
	public static class TemperatureDisplayModeExtension
	{
		// Token: 0x0600365B RID: 13915 RVA: 0x0015B9C4 File Offset: 0x00159BC4
		public static string ToStringHuman(this TemperatureDisplayMode mode)
		{
			switch (mode)
			{
			case TemperatureDisplayMode.Celsius:
				return "Celsius".Translate();
			case TemperatureDisplayMode.Fahrenheit:
				return "Fahrenheit".Translate();
			case TemperatureDisplayMode.Kelvin:
				return "Kelvin".Translate();
			default:
				throw new NotImplementedException();
			}
		}
	}
}
