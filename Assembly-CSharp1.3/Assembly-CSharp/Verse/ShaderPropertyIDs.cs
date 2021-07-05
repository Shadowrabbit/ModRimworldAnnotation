using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D4 RID: 1236
	[StaticConstructorOnStartup]
	public static class ShaderPropertyIDs
	{
		// Token: 0x0400174C RID: 5964
		private static readonly string PlanetSunLightDirectionName = "_PlanetSunLightDirection";

		// Token: 0x0400174D RID: 5965
		private static readonly string PlanetSunLightEnabledName = "_PlanetSunLightEnabled";

		// Token: 0x0400174E RID: 5966
		private static readonly string PlanetRadiusName = "_PlanetRadius";

		// Token: 0x0400174F RID: 5967
		private static readonly string MapSunLightDirectionName = "_CastVect";

		// Token: 0x04001750 RID: 5968
		private static readonly string GlowRadiusName = "_GlowRadius";

		// Token: 0x04001751 RID: 5969
		private static readonly string GameSecondsName = "_GameSeconds";

		// Token: 0x04001752 RID: 5970
		private static readonly string ColorName = "_Color";

		// Token: 0x04001753 RID: 5971
		private static readonly string ColorTwoName = "_ColorTwo";

		// Token: 0x04001754 RID: 5972
		private static readonly string MaskTexName = "_MaskTex";

		// Token: 0x04001755 RID: 5973
		private static readonly string SwayHeadName = "_SwayHead";

		// Token: 0x04001756 RID: 5974
		private static readonly string ShockwaveSpanName = "_ShockwaveSpan";

		// Token: 0x04001757 RID: 5975
		private static readonly string AgeSecsName = "_AgeSecs";

		// Token: 0x04001758 RID: 5976
		private static readonly string AgeSecsPausableName = "_AgeSecsPausable";

		// Token: 0x04001759 RID: 5977
		private static readonly string MaskTextureOffsetName = "_Mask_TexOffset";

		// Token: 0x0400175A RID: 5978
		private static readonly string MaskTextureScaleName = "_Mask_TexScale";

		// Token: 0x0400175B RID: 5979
		public static int PlanetSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightDirectionName);

		// Token: 0x0400175C RID: 5980
		public static int PlanetSunLightEnabled = Shader.PropertyToID(ShaderPropertyIDs.PlanetSunLightEnabledName);

		// Token: 0x0400175D RID: 5981
		public static int PlanetRadius = Shader.PropertyToID(ShaderPropertyIDs.PlanetRadiusName);

		// Token: 0x0400175E RID: 5982
		public static int MapSunLightDirection = Shader.PropertyToID(ShaderPropertyIDs.MapSunLightDirectionName);

		// Token: 0x0400175F RID: 5983
		public static int GlowRadius = Shader.PropertyToID(ShaderPropertyIDs.GlowRadiusName);

		// Token: 0x04001760 RID: 5984
		public static int GameSeconds = Shader.PropertyToID(ShaderPropertyIDs.GameSecondsName);

		// Token: 0x04001761 RID: 5985
		public static int AgeSecs = Shader.PropertyToID(ShaderPropertyIDs.AgeSecsName);

		// Token: 0x04001762 RID: 5986
		public static int AgeSecsPausable = Shader.PropertyToID(ShaderPropertyIDs.AgeSecsPausableName);

		// Token: 0x04001763 RID: 5987
		public static int Color = Shader.PropertyToID(ShaderPropertyIDs.ColorName);

		// Token: 0x04001764 RID: 5988
		public static int ColorTwo = Shader.PropertyToID(ShaderPropertyIDs.ColorTwoName);

		// Token: 0x04001765 RID: 5989
		public static int MaskTex = Shader.PropertyToID(ShaderPropertyIDs.MaskTexName);

		// Token: 0x04001766 RID: 5990
		public static int SwayHead = Shader.PropertyToID(ShaderPropertyIDs.SwayHeadName);

		// Token: 0x04001767 RID: 5991
		public static int ShockwaveColor = Shader.PropertyToID("_ShockwaveColor");

		// Token: 0x04001768 RID: 5992
		public static int ShockwaveSpan = Shader.PropertyToID(ShaderPropertyIDs.ShockwaveSpanName);

		// Token: 0x04001769 RID: 5993
		public static int WaterCastVectSun = Shader.PropertyToID("_WaterCastVectSun");

		// Token: 0x0400176A RID: 5994
		public static int WaterCastVectMoon = Shader.PropertyToID("_WaterCastVectMoon");

		// Token: 0x0400176B RID: 5995
		public static int WaterOutputTex = Shader.PropertyToID("_WaterOutputTex");

		// Token: 0x0400176C RID: 5996
		public static int WaterOffsetTex = Shader.PropertyToID("_WaterOffsetTex");

		// Token: 0x0400176D RID: 5997
		public static int ShadowCompositeTex = Shader.PropertyToID("_ShadowCompositeTex");

		// Token: 0x0400176E RID: 5998
		public static int FallIntensity = Shader.PropertyToID("_FallIntensity");

		// Token: 0x0400176F RID: 5999
		public static int MaskTextureOffset = Shader.PropertyToID(ShaderPropertyIDs.MaskTextureOffsetName);

		// Token: 0x04001770 RID: 6000
		public static int MaskTextureScale = Shader.PropertyToID(ShaderPropertyIDs.MaskTextureScaleName);
	}
}
