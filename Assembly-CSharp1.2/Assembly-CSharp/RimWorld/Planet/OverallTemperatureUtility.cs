using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002036 RID: 8246
	public static class OverallTemperatureUtility
	{
		// Token: 0x170019C9 RID: 6601
		// (get) Token: 0x0600AEDD RID: 44765 RVA: 0x00071D13 File Offset: 0x0006FF13
		public static int EnumValuesCount
		{
			get
			{
				if (OverallTemperatureUtility.cachedEnumValuesCount < 0)
				{
					OverallTemperatureUtility.cachedEnumValuesCount = Enum.GetNames(typeof(OverallTemperature)).Length;
				}
				return OverallTemperatureUtility.cachedEnumValuesCount;
			}
		}

		// Token: 0x0600AEDE RID: 44766 RVA: 0x0032D144 File Offset: 0x0032B344
		public static SimpleCurve GetTemperatureCurve(this OverallTemperature overallTemperature)
		{
			switch (overallTemperature)
			{
			case OverallTemperature.VeryCold:
				return OverallTemperatureUtility.Curve_VeryCold;
			case OverallTemperature.Cold:
				return OverallTemperatureUtility.Curve_Cold;
			case OverallTemperature.LittleBitColder:
				return OverallTemperatureUtility.Curve_LittleBitColder;
			case OverallTemperature.LittleBitWarmer:
				return OverallTemperatureUtility.Curve_LittleBitWarmer;
			case OverallTemperature.Hot:
				return OverallTemperatureUtility.Curve_Hot;
			case OverallTemperature.VeryHot:
				return OverallTemperatureUtility.Curve_VeryHot;
			}
			return null;
		}

		// Token: 0x0400780D RID: 30733
		private static int cachedEnumValuesCount = -1;

		// Token: 0x0400780E RID: 30734
		private static readonly SimpleCurve Curve_VeryCold = new SimpleCurve
		{
			{
				new CurvePoint(-9999f, -9999f),
				true
			},
			{
				new CurvePoint(-50f, -75f),
				true
			},
			{
				new CurvePoint(-40f, -60f),
				true
			},
			{
				new CurvePoint(0f, -35f),
				true
			},
			{
				new CurvePoint(20f, -28f),
				true
			},
			{
				new CurvePoint(25f, -18f),
				true
			},
			{
				new CurvePoint(30f, -8.5f),
				true
			},
			{
				new CurvePoint(50f, -7f),
				true
			}
		};

		// Token: 0x0400780F RID: 30735
		private static readonly SimpleCurve Curve_Cold = new SimpleCurve
		{
			{
				new CurvePoint(-9999f, -9999f),
				true
			},
			{
				new CurvePoint(-50f, -70f),
				true
			},
			{
				new CurvePoint(-25f, -40f),
				true
			},
			{
				new CurvePoint(-20f, -25f),
				true
			},
			{
				new CurvePoint(-13f, -15f),
				true
			},
			{
				new CurvePoint(0f, -12f),
				true
			},
			{
				new CurvePoint(30f, -3f),
				true
			},
			{
				new CurvePoint(60f, 25f),
				true
			}
		};

		// Token: 0x04007810 RID: 30736
		private static readonly SimpleCurve Curve_LittleBitColder = new SimpleCurve
		{
			{
				new CurvePoint(-9999f, -9999f),
				true
			},
			{
				new CurvePoint(-20f, -22f),
				true
			},
			{
				new CurvePoint(-15f, -15f),
				true
			},
			{
				new CurvePoint(-5f, -13f),
				true
			},
			{
				new CurvePoint(40f, 30f),
				true
			},
			{
				new CurvePoint(9999f, 9999f),
				true
			}
		};

		// Token: 0x04007811 RID: 30737
		private static readonly SimpleCurve Curve_LittleBitWarmer = new SimpleCurve
		{
			{
				new CurvePoint(-9999f, -9999f),
				true
			},
			{
				new CurvePoint(-45f, -35f),
				true
			},
			{
				new CurvePoint(40f, 50f),
				true
			},
			{
				new CurvePoint(120f, 120f),
				true
			},
			{
				new CurvePoint(9999f, 9999f),
				true
			}
		};

		// Token: 0x04007812 RID: 30738
		private static readonly SimpleCurve Curve_Hot = new SimpleCurve
		{
			{
				new CurvePoint(-45f, -22f),
				true
			},
			{
				new CurvePoint(-25f, -12f),
				true
			},
			{
				new CurvePoint(-22f, 2f),
				true
			},
			{
				new CurvePoint(-10f, 25f),
				true
			},
			{
				new CurvePoint(40f, 57f),
				true
			},
			{
				new CurvePoint(120f, 120f),
				true
			},
			{
				new CurvePoint(9999f, 9999f),
				true
			}
		};

		// Token: 0x04007813 RID: 30739
		private static readonly SimpleCurve Curve_VeryHot = new SimpleCurve
		{
			{
				new CurvePoint(-45f, 25f),
				true
			},
			{
				new CurvePoint(0f, 40f),
				true
			},
			{
				new CurvePoint(33f, 80f),
				true
			},
			{
				new CurvePoint(40f, 88f),
				true
			},
			{
				new CurvePoint(120f, 120f),
				true
			},
			{
				new CurvePoint(9999f, 9999f),
				true
			}
		};
	}
}
