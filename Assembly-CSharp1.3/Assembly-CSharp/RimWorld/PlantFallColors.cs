using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020010BF RID: 4287
	public static class PlantFallColors
	{
		// Token: 0x06006699 RID: 26265 RVA: 0x0022A578 File Offset: 0x00228778
		public static float GetFallColorFactor(float latitude, int dayOfYear)
		{
			float a = GenCelestial.AverageGlow(latitude, dayOfYear);
			float b = GenCelestial.AverageGlow(latitude, dayOfYear + 1);
			float x = Mathf.LerpUnclamped(a, b, PlantFallColors.FallSlopeComponent);
			return GenMath.LerpDoubleClamped(PlantFallColors.FallColorBegin, PlantFallColors.FallColorEnd, 0f, 1f, x);
		}

		// Token: 0x0600669A RID: 26266 RVA: 0x0022A5BC File Offset: 0x002287BC
		public static void SetFallShaderGlobals(Map map)
		{
			if (PlantFallColors.FallIntensityOverride)
			{
				Shader.SetGlobalFloat(ShaderPropertyIDs.FallIntensity, PlantFallColors.FallIntensity);
			}
			else
			{
				Vector2 vector = Find.WorldGrid.LongLatOf(map.Tile);
				Shader.SetGlobalFloat(ShaderPropertyIDs.FallIntensity, PlantFallColors.GetFallColorFactor(vector.y, GenLocalDate.DayOfYear(map)));
			}
			Shader.SetGlobalInt("_FallGlobalControls", PlantFallColors.FallGlobalControls ? 1 : 0);
			if (PlantFallColors.FallGlobalControls)
			{
				Shader.SetGlobalVector("_FallSrc", new Vector3(PlantFallColors.FallSrcR, PlantFallColors.FallSrcG, PlantFallColors.FallSrcB));
				Shader.SetGlobalVector("_FallDst", new Vector3(PlantFallColors.FallDstR, PlantFallColors.FallDstG, PlantFallColors.FallDstB));
				Shader.SetGlobalVector("_FallRange", new Vector3(PlantFallColors.FallRangeBegin, PlantFallColors.FallRangeEnd));
			}
		}

		// Token: 0x040039E9 RID: 14825
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorBegin = 0.55f;

		// Token: 0x040039EA RID: 14826
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorEnd = 0.45f;

		// Token: 0x040039EB RID: 14827
		[TweakValue("Graphics", 0f, 30f)]
		private static float FallSlopeComponent = 15f;

		// Token: 0x040039EC RID: 14828
		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallIntensityOverride = false;

		// Token: 0x040039ED RID: 14829
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallIntensity = 0f;

		// Token: 0x040039EE RID: 14830
		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallGlobalControls = false;

		// Token: 0x040039EF RID: 14831
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcR = 0.3803f;

		// Token: 0x040039F0 RID: 14832
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcG = 0.4352f;

		// Token: 0x040039F1 RID: 14833
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcB = 0.1451f;

		// Token: 0x040039F2 RID: 14834
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstR = 0.4392f;

		// Token: 0x040039F3 RID: 14835
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstG = 0.3254f;

		// Token: 0x040039F4 RID: 14836
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstB = 0.1765f;

		// Token: 0x040039F5 RID: 14837
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeBegin = 0.02f;

		// Token: 0x040039F6 RID: 14838
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeEnd = 0.1f;
	}
}
