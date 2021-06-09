using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002034 RID: 8244
	public static class OverallRainfallUtility
	{
		// Token: 0x170019C8 RID: 6600
		// (get) Token: 0x0600AEDA RID: 44762 RVA: 0x00071CEE File Offset: 0x0006FEEE
		public static int EnumValuesCount
		{
			get
			{
				if (OverallRainfallUtility.cachedEnumValuesCount < 0)
				{
					OverallRainfallUtility.cachedEnumValuesCount = Enum.GetNames(typeof(OverallRainfall)).Length;
				}
				return OverallRainfallUtility.cachedEnumValuesCount;
			}
		}

		// Token: 0x0600AEDB RID: 44763 RVA: 0x0032CDDC File Offset: 0x0032AFDC
		public static SimpleCurve GetRainfallCurve(this OverallRainfall overallRainfall)
		{
			switch (overallRainfall)
			{
			case OverallRainfall.AlmostNone:
				return OverallRainfallUtility.Curve_AlmostNone;
			case OverallRainfall.Little:
				return OverallRainfallUtility.Curve_Little;
			case OverallRainfall.LittleBitLess:
				return OverallRainfallUtility.Curve_LittleBitLess;
			case OverallRainfall.LittleBitMore:
				return OverallRainfallUtility.Curve_LittleBitMore;
			case OverallRainfall.High:
				return OverallRainfallUtility.Curve_High;
			case OverallRainfall.VeryHigh:
				return OverallRainfallUtility.Curve_VeryHigh;
			}
			return null;
		}

		// Token: 0x040077FE RID: 30718
		private static int cachedEnumValuesCount = -1;

		// Token: 0x040077FF RID: 30719
		private static readonly SimpleCurve Curve_AlmostNone = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1500f, 120f),
				true
			},
			{
				new CurvePoint(3500f, 180f),
				true
			},
			{
				new CurvePoint(6000f, 200f),
				true
			},
			{
				new CurvePoint(12000f, 250f),
				true
			}
		};

		// Token: 0x04007800 RID: 30720
		private static readonly SimpleCurve Curve_Little = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1500f, 300f),
				true
			},
			{
				new CurvePoint(6000f, 1100f),
				true
			},
			{
				new CurvePoint(12000f, 1400f),
				true
			}
		};

		// Token: 0x04007801 RID: 30721
		private static readonly SimpleCurve Curve_LittleBitLess = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1000f, 700f),
				true
			},
			{
				new CurvePoint(5000f, 4700f),
				true
			},
			{
				new CurvePoint(12000f, 12000f),
				true
			},
			{
				new CurvePoint(99999f, 99999f),
				true
			}
		};

		// Token: 0x04007802 RID: 30722
		private static readonly SimpleCurve Curve_LittleBitMore = new SimpleCurve
		{
			{
				new CurvePoint(0f, 50f),
				true
			},
			{
				new CurvePoint(5000f, 5300f),
				true
			},
			{
				new CurvePoint(12000f, 12000f),
				true
			},
			{
				new CurvePoint(99999f, 99999f),
				true
			}
		};

		// Token: 0x04007803 RID: 30723
		private static readonly SimpleCurve Curve_High = new SimpleCurve
		{
			{
				new CurvePoint(0f, 500f),
				true
			},
			{
				new CurvePoint(150f, 950f),
				true
			},
			{
				new CurvePoint(500f, 2000f),
				true
			},
			{
				new CurvePoint(1000f, 2800f),
				true
			},
			{
				new CurvePoint(5000f, 6000f),
				true
			},
			{
				new CurvePoint(12000f, 12000f),
				true
			},
			{
				new CurvePoint(99999f, 99999f),
				true
			}
		};

		// Token: 0x04007804 RID: 30724
		private static readonly SimpleCurve Curve_VeryHigh = new SimpleCurve
		{
			{
				new CurvePoint(0f, 750f),
				true
			},
			{
				new CurvePoint(125f, 2000f),
				true
			},
			{
				new CurvePoint(500f, 3000f),
				true
			},
			{
				new CurvePoint(1000f, 3800f),
				true
			},
			{
				new CurvePoint(5000f, 7500f),
				true
			},
			{
				new CurvePoint(12000f, 12000f),
				true
			},
			{
				new CurvePoint(99999f, 99999f),
				true
			}
		};
	}
}
