using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C9F RID: 3231
	public class GenStep_Snow : GenStep
	{
		// Token: 0x17000D00 RID: 3328
		// (get) Token: 0x06004B6B RID: 19307 RVA: 0x001908C5 File Offset: 0x0018EAC5
		public override int SeedPart
		{
			get
			{
				return 306693816;
			}
		}

		// Token: 0x06004B6C RID: 19308 RVA: 0x001908CC File Offset: 0x0018EACC
		public override void Generate(Map map, GenStepParams parms)
		{
			int num = 0;
			for (int i = (int)(GenLocalDate.Twelfth(map) - Twelfth.Third); i <= (int)GenLocalDate.Twelfth(map); i++)
			{
				int num2 = i;
				if (num2 < 0)
				{
					num2 += 12;
				}
				Twelfth twelfth = (Twelfth)num2;
				if (GenTemperature.AverageTemperatureAtTileForTwelfth(map.Tile, twelfth) < 0f)
				{
					num++;
				}
			}
			float num3 = 0f;
			switch (num)
			{
			case 0:
				return;
			case 1:
				num3 = 0.3f;
				break;
			case 2:
				num3 = 0.7f;
				break;
			case 3:
				num3 = 1f;
				break;
			}
			if (map.mapTemperature.SeasonalTemp > 0f)
			{
				num3 *= 0.4f;
			}
			if ((double)num3 < 0.3)
			{
				return;
			}
			foreach (IntVec3 c in map.AllCells)
			{
				if (!c.Roofed(map))
				{
					map.steadyEnvironmentEffects.AddFallenSnowAt(c, num3);
				}
			}
		}
	}
}
