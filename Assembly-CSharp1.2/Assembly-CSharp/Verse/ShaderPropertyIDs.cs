using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200087F RID: 2175
	[StaticConstructorOnStartup]
	public static class ShaderPropertyIDs
	{
		// Token: 0x0400258E RID: 9614
		private static readonly string PlanetSunLightDirectionName = "_PlanetSunLightDirection";

		// Token: 0x0400258F RID: 9615
		private static readonly string PlanetSunLightEnabledName = "_PlanetSunLightEnabled";

		// Token: 0x04002590 RID: 9616
		private static readonly string PlanetRadiusName = "_PlanetRadius";

		// Token: 0x04002591 RID: 9617
		private static readonly string MapSunLightDirectionName = "_CastVect";

		// Token: 0x04002592 RID: 9618
		private static readonly string GlowRadiusName = "_GlowRadius";

		// Token: 0x04002593 RID: 9619
		private static readonly string GameSecondsName = "_GameSeconds";

		// Token: 0x04002594 RID: 9620
		private static readonly string ColorName = "_Color";

		// Token: 0x04002595 RID: 9621
		private static readonly string ColorTwoName = "_ColorTwo";

		// Token: 0x04002596 RID: 9622
		private static readonly string MaskTexName = "_MaskTex";

		// Token: 0x04002597 RID: 9623
		private static readonly string SwayHeadName = "_SwayHead";

		// Token: 0x04002598 RID: 9624
		private static readonly string ShockwaveSpanName = "_ShockwaveSpan";

		// Token: 0x04002599 RID: 9625
		private static readonly string AgeSecsName = "_AgeSecs";

		// Token: 0x0400259A RID: 9626
		public static int PlanetSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightDirectionName);

		// Token: 0x0400259B RID: 9627
		public static int PlanetSunLightEnabled = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightEnabledName);

		// Token: 0x0400259C RID: 9628
		public static int PlanetRadius = Shader.PropertyToID(ShaderPropertyIDs.PlanetRadiusName);

		// Token: 0x0400259D RID: 9629
		public static int MapSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.MapSunLightDirectionName);

		// Token: 0x0400259E RID: 9630
		public static int GlowRadius = Shader.PropertyToID(ShaderPropertyIDs.GlowRadiusName);

		// Token: 0x0400259F RID: 9631
		public static int GameSeconds = Shader.PropertyToID(ShaderPropertyIDs.GameSecondsName);

		// Token: 0x040025A0 RID: 9632
		public static int AgeSecs = Shader.PropertyToID(ShaderPropertyIDs.AgeSecsName);

		// Token: 0x040025A1 RID: 9633
		public static int Color = Shader.PropertyToID(ShaderPropertyIDs.ColorName);

		// Token: 0x040025A2 RID: 9634
		public static int ColorTwo = Shader.PropertyToID(ShaderPropertyIDs.ColorTwoName);

		// Token: 0x040025A3 RID: 9635
		public static int MaskTex = Shader.PropertyToID(ShaderPropertyIDs.MaskTexName);

		// Token: 0x040025A4 RID: 9636
		public static int SwayHead = Shader.PropertyToID(ShaderPropertyIDs.SwayHeadName);

		// Token: 0x040025A5 RID: 9637
		public static int ShockwaveColor = Shader.PropertyToID("_ShockwaveColor");

		// Token: 0x040025A6 RID: 9638
		public static int ShockwaveSpan = Shader.PropertyToID(ShaderPropertyIDs.ShockwaveSpanName);

		// Token: 0x040025A7 RID: 9639
		public static int WaterCastVectSun = Shader.PropertyToID("_WaterCastVectSun");

		// Token: 0x040025A8 RID: 9640
		public static int WaterCastVectMoon = Shader.PropertyToID("_WaterCastVectMoon");

		// Token: 0x040025A9 RID: 9641
		public static int WaterOutputTex = Shader.PropertyToID("_WaterOutputTex");

		// Token: 0x040025AA RID: 9642
		public static int WaterOffsetTex = Shader.PropertyToID("_WaterOffsetTex");

		// Token: 0x040025AB RID: 9643
		public static int ShadowCompositeTex = Shader.PropertyToID("_ShadowCompositeTex");

		// Token: 0x040025AC RID: 9644
		public static int FallIntensity = Shader.PropertyToID("_FallIntensity");
	}
}
