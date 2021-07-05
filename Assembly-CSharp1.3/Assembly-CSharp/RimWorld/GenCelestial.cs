using System;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013F2 RID: 5106
	public static class GenCelestial
	{
		// Token: 0x170015B1 RID: 5553
		// (get) Token: 0x06007C5C RID: 31836 RVA: 0x002C1FD8 File Offset: 0x002C01D8
		private static int TicksAbsForSunPosInWorldSpace
		{
			get
			{
				if (Current.ProgramState != ProgramState.Entry)
				{
					return GenTicks.TicksAbs;
				}
				int startingTile = Find.GameInitData.startingTile;
				float longitude = (startingTile >= 0) ? Find.WorldGrid.LongLatOf(startingTile).x : 0f;
				return Mathf.RoundToInt(2500f * (12f - GenDate.TimeZoneFloatAt(longitude)));
			}
		}

		// Token: 0x06007C5D RID: 31837 RVA: 0x002C2030 File Offset: 0x002C0230
		public static float CurCelestialSunGlow(Map map)
		{
			return GenCelestial.CelestialSunGlow(map, Find.TickManager.TicksAbs);
		}

		// Token: 0x06007C5E RID: 31838 RVA: 0x002C2042 File Offset: 0x002C0242
		public static float CelestialSunGlow(Map map, int ticksAbs)
		{
			return GenCelestial.CelestialSunGlow(map.Tile, ticksAbs);
		}

		// Token: 0x06007C5F RID: 31839 RVA: 0x002C2050 File Offset: 0x002C0250
		public static float CelestialSunGlow(int tile, int ticksAbs)
		{
			Vector2 vector = Find.WorldGrid.LongLatOf(tile);
			return GenCelestial.CelestialSunGlowPercent(vector.y, GenDate.DayOfYear((long)ticksAbs, vector.x), GenDate.DayPercent((long)ticksAbs, vector.x));
		}

		// Token: 0x06007C60 RID: 31840 RVA: 0x002C208E File Offset: 0x002C028E
		public static float CurShadowStrength(Map map)
		{
			return Mathf.Clamp01(Mathf.Abs(GenCelestial.CurCelestialSunGlow(map) - 0.6f) / 0.15f);
		}

		// Token: 0x06007C61 RID: 31841 RVA: 0x002C20AC File Offset: 0x002C02AC
		public static GenCelestial.LightInfo GetLightSourceInfo(Map map, GenCelestial.LightType type)
		{
			float num = GenLocalDate.DayPercent(map);
			bool flag;
			float intensity;
			if (type == GenCelestial.LightType.Shadow)
			{
				flag = GenCelestial.IsDaytime(GenCelestial.CurCelestialSunGlow(map));
				intensity = GenCelestial.CurShadowStrength(map);
			}
			else if (type == GenCelestial.LightType.LightingSun)
			{
				flag = true;
				intensity = Mathf.Clamp01((GenCelestial.CurCelestialSunGlow(map) - 0.6f + 0.2f) / 0.15f);
			}
			else if (type == GenCelestial.LightType.LightingMoon)
			{
				flag = false;
				intensity = Mathf.Clamp01(-(GenCelestial.CurCelestialSunGlow(map) - 0.6f - 0.2f) / 0.15f);
			}
			else
			{
				Log.ErrorOnce("Invalid light type requested", 64275614);
				flag = true;
				intensity = 0f;
			}
			float t;
			float num2;
			float num3;
			if (flag)
			{
				t = num;
				num2 = -1.5f;
				num3 = 15f;
			}
			else
			{
				if (num > 0.5f)
				{
					t = Mathf.InverseLerp(0.5f, 1f, num) * 0.5f;
				}
				else
				{
					t = 0.5f + Mathf.InverseLerp(0f, 0.5f, num) * 0.5f;
				}
				num2 = -0.9f;
				num3 = 15f;
			}
			float num4 = Mathf.LerpUnclamped(-num3, num3, t);
			float y = num2 - 2.5f * (num4 * num4 / 100f);
			return new GenCelestial.LightInfo
			{
				vector = new Vector2(num4, y),
				intensity = intensity
			};
		}

		// Token: 0x06007C62 RID: 31842 RVA: 0x002C21E4 File Offset: 0x002C03E4
		public static Vector3 CurSunPositionInWorldSpace()
		{
			int ticksAbsForSunPosInWorldSpace = GenCelestial.TicksAbsForSunPosInWorldSpace;
			return GenCelestial.SunPositionUnmodified((float)GenDate.DayOfYear((long)ticksAbsForSunPosInWorldSpace, 0f), GenDate.DayPercent((long)ticksAbsForSunPosInWorldSpace, 0f), new Vector3(0f, 0f, -1f), 0f);
		}

		// Token: 0x06007C63 RID: 31843 RVA: 0x002C222E File Offset: 0x002C042E
		public static bool IsDaytime(float glow)
		{
			return glow > 0.6f;
		}

		// Token: 0x06007C64 RID: 31844 RVA: 0x002C2238 File Offset: 0x002C0438
		private static Vector3 SunPosition(float latitude, int dayOfYear, float dayPercent)
		{
			Vector3 target = GenCelestial.SurfaceNormal(latitude);
			Vector3 current = GenCelestial.SunPositionUnmodified((float)dayOfYear, dayPercent, new Vector3(1f, 0f, 0f), latitude);
			float num = GenCelestial.SunPeekAroundDegreesFactorCurve.Evaluate(latitude);
			current = Vector3.RotateTowards(current, target, 0.33161256f * num, 9999999f);
			float num2 = Mathf.InverseLerp(60f, 0f, Mathf.Abs(latitude));
			if (num2 > 0f)
			{
				current = Vector3.RotateTowards(current, target, 6.2831855f * (17f * num2 / 360f), 9999999f);
			}
			return current.normalized;
		}

		// Token: 0x06007C65 RID: 31845 RVA: 0x002C22D0 File Offset: 0x002C04D0
		private static Vector3 SunPositionUnmodified(float dayOfYear, float dayPercent, Vector3 initialSunPos, float latitude = 0f)
		{
			Vector3 point = initialSunPos * 100f;
			float num = -Mathf.Cos(dayOfYear / 60f * 3.1415927f * 2f);
			point.y += num * 100f * GenCelestial.SunOffsetFractionFromLatitudeCurve.Evaluate(latitude);
			point = Quaternion.AngleAxis((dayPercent - 0.5f) * 360f, Vector3.up) * point;
			return point.normalized;
		}

		// Token: 0x06007C66 RID: 31846 RVA: 0x002C2348 File Offset: 0x002C0548
		private static float CelestialSunGlowPercent(float latitude, int dayOfYear, float dayPercent)
		{
			Vector3 vector = GenCelestial.SurfaceNormal(latitude);
			Vector3 rhs = GenCelestial.SunPosition(latitude, dayOfYear, dayPercent);
			float value = Vector3.Dot(vector.normalized, rhs);
			return Mathf.Clamp01(Mathf.InverseLerp(0f, 0.7f, value));
		}

		// Token: 0x06007C67 RID: 31847 RVA: 0x002C2388 File Offset: 0x002C0588
		public static float AverageGlow(float latitude, int dayOfYear)
		{
			float num = 0f;
			for (int i = 0; i < 24; i++)
			{
				num += GenCelestial.CelestialSunGlowPercent(latitude, dayOfYear, (float)i / 24f);
			}
			return num / 24f;
		}

		// Token: 0x06007C68 RID: 31848 RVA: 0x002C23C4 File Offset: 0x002C05C4
		private static Vector3 SurfaceNormal(float latitude)
		{
			Vector3 vector = new Vector3(1f, 0f, 0f);
			vector = Quaternion.AngleAxis(latitude, new Vector3(0f, 0f, 1f)) * vector;
			return vector;
		}

		// Token: 0x06007C69 RID: 31849 RVA: 0x002C240C File Offset: 0x002C060C
		public static void LogSunGlowForYear()
		{
			for (int i = -90; i <= 90; i += 10)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Sun visibility percents for latitude " + i + ", for each hour of each day of the year");
				stringBuilder.AppendLine("---------------------------------------");
				stringBuilder.Append("Day/hr".PadRight(6));
				for (int j = 0; j < 24; j += 2)
				{
					stringBuilder.Append((j.ToString() + "h").PadRight(6));
				}
				stringBuilder.AppendLine();
				for (int k = 0; k < 60; k += 5)
				{
					stringBuilder.Append(k.ToString().PadRight(6));
					for (int l = 0; l < 24; l += 2)
					{
						stringBuilder.Append(GenCelestial.CelestialSunGlowPercent((float)i, k, (float)l / 24f).ToString("F2").PadRight(6));
					}
					stringBuilder.AppendLine();
				}
				Log.Message(stringBuilder.ToString());
			}
		}

		// Token: 0x06007C6A RID: 31850 RVA: 0x002C2510 File Offset: 0x002C0710
		public static void LogSunAngleForYear()
		{
			for (int i = -90; i <= 90; i += 10)
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Sun angles for latitude " + i + ", for each hour of each day of the year");
				stringBuilder.AppendLine("---------------------------------------");
				stringBuilder.Append("Day/hr".PadRight(6));
				for (int j = 0; j < 24; j += 2)
				{
					stringBuilder.Append((j.ToString() + "h").PadRight(6));
				}
				stringBuilder.AppendLine();
				for (int k = 0; k < 60; k += 5)
				{
					stringBuilder.Append(k.ToString().PadRight(6));
					for (int l = 0; l < 24; l += 2)
					{
						float num = Vector3.Angle(GenCelestial.SurfaceNormal((float)i), GenCelestial.SunPositionUnmodified((float)k, (float)l / 24f, new Vector3(1f, 0f, 0f), 0f));
						stringBuilder.Append((90f - num).ToString("F0").PadRight(6));
					}
					stringBuilder.AppendLine();
				}
				Log.Message(stringBuilder.ToString());
			}
		}

		// Token: 0x040044D8 RID: 17624
		public const float ShadowMaxLengthDay = 15f;

		// Token: 0x040044D9 RID: 17625
		public const float ShadowMaxLengthNight = 15f;

		// Token: 0x040044DA RID: 17626
		private const float ShadowGlowLerpSpan = 0.15f;

		// Token: 0x040044DB RID: 17627
		private const float ShadowDayNightThreshold = 0.6f;

		// Token: 0x040044DC RID: 17628
		private static SimpleCurve SunPeekAroundDegreesFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(70f, 1f),
				true
			},
			{
				new CurvePoint(75f, 0.05f),
				true
			}
		};

		// Token: 0x040044DD RID: 17629
		private static SimpleCurve SunOffsetFractionFromLatitudeCurve = new SimpleCurve
		{
			{
				new CurvePoint(70f, 0.2f),
				true
			},
			{
				new CurvePoint(75f, 1.5f),
				true
			}
		};

		// Token: 0x020027FB RID: 10235
		public struct LightInfo
		{
			// Token: 0x04009721 RID: 38689
			public Vector2 vector;

			// Token: 0x04009722 RID: 38690
			public float intensity;
		}

		// Token: 0x020027FC RID: 10236
		public enum LightType
		{
			// Token: 0x04009724 RID: 38692
			Shadow,
			// Token: 0x04009725 RID: 38693
			LightingSun,
			// Token: 0x04009726 RID: 38694
			LightingMoon
		}
	}
}
