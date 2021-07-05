﻿using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001749 RID: 5961
	public static class OverallTemperatureUtility
	{
		// Token: 0x17001669 RID: 5737
		// (get) Token: 0x060089B4 RID: 35252 RVA: 0x003170A7 File Offset: 0x003152A7
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

		// Token: 0x060089B5 RID: 35253 RVA: 0x003170CC File Offset: 0x003152CC
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

		// Token: 0x04005760 RID: 22368
		private static int cachedEnumValuesCount = -1;

		// Token: 0x04005761 RID: 22369
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

		// Token: 0x04005762 RID: 22370
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

		// Token: 0x04005763 RID: 22371
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

		// Token: 0x04005764 RID: 22372
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

		// Token: 0x04005765 RID: 22373
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

		// Token: 0x04005766 RID: 22374
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
