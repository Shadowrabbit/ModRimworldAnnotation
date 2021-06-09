using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200171E RID: 5918
	public static class PlantFallColors
	{
		// Token: 0x06008292 RID: 33426 RVA: 0x0026B708 File Offset: 0x00269908
		public static float GetFallColorFactor(float latitude, int dayOfYear)
		{
			float a = GenCelestial.AverageGlow(latitude, dayOfYear);
			float b = GenCelestial.AverageGlow(latitude, dayOfYear + 1);
			float x = Mathf.LerpUnclamped(a, b, PlantFallColors.FallSlopeComponent);
			return GenMath.LerpDoubleClamped(PlantFallColors.FallColorBegin, PlantFallColors.FallColorEnd, 0f, 1f, x);
		}

		// Token: 0x06008293 RID: 33427 RVA: 0x0026B74C File Offset: 0x0026994C
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

		// Token: 0x040054A1 RID: 21665
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorBegin = 0.55f;

		// Token: 0x040054A2 RID: 21666
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallColorEnd = 0.45f;

		// Token: 0x040054A3 RID: 21667
		[TweakValue("Graphics", 0f, 30f)]
		private static float FallSlopeComponent = 15f;

		// Token: 0x040054A4 RID: 21668
		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallIntensityOverride = false;

		// Token: 0x040054A5 RID: 21669
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallIntensity = 0f;

		// Token: 0x040054A6 RID: 21670
		[TweakValue("Graphics", 0f, 100f)]
		private static bool FallGlobalControls = false;

		// Token: 0x040054A7 RID: 21671
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcR = 0.3803f;

		// Token: 0x040054A8 RID: 21672
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcG = 0.4352f;

		// Token: 0x040054A9 RID: 21673
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallSrcB = 0.1451f;

		// Token: 0x040054AA RID: 21674
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstR = 0.4392f;

		// Token: 0x040054AB RID: 21675
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstG = 0.3254f;

		// Token: 0x040054AC RID: 21676
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallDstB = 0.1765f;

		// Token: 0x040054AD RID: 21677
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeBegin = 0.02f;

		// Token: 0x040054AE RID: 21678
		[TweakValue("Graphics", 0f, 1f)]
		private static float FallRangeEnd = 0.1f;
	}
}
