﻿using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001747 RID: 5959
	public static class OverallRainfallUtility
	{
		// Token: 0x17001668 RID: 5736
		// (get) Token: 0x060089B1 RID: 35249 RVA: 0x00316D18 File Offset: 0x00314F18
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

		// Token: 0x060089B2 RID: 35250 RVA: 0x00316D40 File Offset: 0x00314F40
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

		// Token: 0x04005751 RID: 22353
		private static int cachedEnumValuesCount = -1;

		// Token: 0x04005752 RID: 22354
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

		// Token: 0x04005753 RID: 22355
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

		// Token: 0x04005754 RID: 22356
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

		// Token: 0x04005755 RID: 22357
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

		// Token: 0x04005756 RID: 22358
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

		// Token: 0x04005757 RID: 22359
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
