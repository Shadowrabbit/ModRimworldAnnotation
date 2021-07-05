using System;

namespace Verse
{
	// Token: 0x020004DD RID: 1245
	public static class TemperatureDisplayModeExtension
	{
		// Token: 0x060025CB RID: 9675 RVA: 0x000EA4F0 File Offset: 0x000E86F0
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
