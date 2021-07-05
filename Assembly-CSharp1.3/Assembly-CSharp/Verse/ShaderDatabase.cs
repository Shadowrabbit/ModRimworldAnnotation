using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000047 RID: 71
	[StaticConstructorOnStartup]
	public static class ShaderDatabase
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060003B9 RID: 953 RVA: 0x00014737 File Offset: 0x00012937
		public static Shader DefaultShader
		{
			get
			{
				return ShaderDatabase.Cutout;
			}
		}

		// Token: 0x060003BA RID: 954 RVA: 0x00014740 File Offset: 0x00012940
		public static Shader LoadShader(string shaderPath)
		{
			if (ShaderDatabase.lookup == null)
			{
				ShaderDatabase.lookup = new Dictionary<string, Shader>();
			}
			if (!ShaderDatabase.lookup.ContainsKey(shaderPath))
			{
				ShaderDatabase.lookup[shaderPath] = (Shader)Resources.Load("Materials/" + shaderPath, typeof(Shader));
			}
			Shader shader = ShaderDatabase.lookup[shaderPath];
			if (shader == null)
			{
				Log.Warning("Could not load shader " + shaderPath);
				return ShaderDatabase.DefaultShader;
			}
			return shader;
		}

		// Token: 0x040000EF RID: 239
		public static readonly Shader Cutout = ShaderDatabase.LoadShader("Map/Cutout");

		// Token: 0x040000F0 RID: 240
		public static readonly Shader CutoutPlant = ShaderDatabase.LoadShader("Map/CutoutPlant");

		// Token: 0x040000F1 RID: 241
		public static readonly Shader CutoutComplex = ShaderDatabase.LoadShader("Map/CutoutComplex");

		// Token: 0x040000F2 RID: 242
		public static readonly Shader CutoutSkinOverlay = ShaderDatabase.LoadShader("Map/CutoutSkinOverlay");

		// Token: 0x040000F3 RID: 243
		public static readonly Shader CutoutSkin = ShaderDatabase.LoadShader("Map/CutoutSkin");

		// Token: 0x040000F4 RID: 244
		public static readonly Shader Wound = ShaderDatabase.LoadShader("Map/Wound");

		// Token: 0x040000F5 RID: 245
		public static readonly Shader WoundSkin = ShaderDatabase.LoadShader("Map/WoundSkin");

		// Token: 0x040000F6 RID: 246
		public static readonly Shader CutoutSkinColorOverride = ShaderDatabase.LoadShader("Map/CutoutSkinOverride");

		// Token: 0x040000F7 RID: 247
		public static readonly Shader CutoutFlying = ShaderDatabase.LoadShader("Map/CutoutFlying");

		// Token: 0x040000F8 RID: 248
		public static readonly Shader Transparent = ShaderDatabase.LoadShader("Map/Transparent");

		// Token: 0x040000F9 RID: 249
		public static readonly Shader TransparentPostLight = ShaderDatabase.LoadShader("Map/TransparentPostLight");

		// Token: 0x040000FA RID: 250
		public static readonly Shader TransparentPlant = ShaderDatabase.LoadShader("Map/TransparentPlant");

		// Token: 0x040000FB RID: 251
		public static readonly Shader Mote = ShaderDatabase.LoadShader("Map/Mote");

		// Token: 0x040000FC RID: 252
		public static readonly Shader MoteGlow = ShaderDatabase.LoadShader("Map/MoteGlow");

		// Token: 0x040000FD RID: 253
		public static readonly Shader MoteGlowPulse = ShaderDatabase.LoadShader("Map/MoteGlowPulse");

		// Token: 0x040000FE RID: 254
		public static readonly Shader MoteWater = ShaderDatabase.LoadShader("Map/MoteWater");

		// Token: 0x040000FF RID: 255
		public static readonly Shader MoteGlowDistorted = ShaderDatabase.LoadShader("Map/MoteGlowDistorted");

		// Token: 0x04000100 RID: 256
		public static readonly Shader MoteGlowDistortBG = ShaderDatabase.LoadShader("Map/MoteGlowDistortBackground");

		// Token: 0x04000101 RID: 257
		public static readonly Shader MoteProximityScannerRadius = ShaderDatabase.LoadShader("Map/MoteProximityScannerRadius");

		// Token: 0x04000102 RID: 258
		public static readonly Shader TerrainHard = ShaderDatabase.LoadShader("Map/TerrainHard");

		// Token: 0x04000103 RID: 259
		public static readonly Shader TerrainFade = ShaderDatabase.LoadShader("Map/TerrainFade");

		// Token: 0x04000104 RID: 260
		public static readonly Shader TerrainFadeRough = ShaderDatabase.LoadShader("Map/TerrainFadeRough");

		// Token: 0x04000105 RID: 261
		public static readonly Shader TerrainWater = ShaderDatabase.LoadShader("Map/TerrainWater");

		// Token: 0x04000106 RID: 262
		public static readonly Shader WorldTerrain = ShaderDatabase.LoadShader("World/WorldTerrain");

		// Token: 0x04000107 RID: 263
		public static readonly Shader WorldOcean = ShaderDatabase.LoadShader("World/WorldOcean");

		// Token: 0x04000108 RID: 264
		public static readonly Shader WorldOverlayCutout = ShaderDatabase.LoadShader("World/WorldOverlayCutout");

		// Token: 0x04000109 RID: 265
		public static readonly Shader WorldOverlayTransparent = ShaderDatabase.LoadShader("World/WorldOverlayTransparent");

		// Token: 0x0400010A RID: 266
		public static readonly Shader WorldOverlayTransparentLit = ShaderDatabase.LoadShader("World/WorldOverlayTransparentLit");

		// Token: 0x0400010B RID: 267
		public static readonly Shader WorldOverlayAdditive = ShaderDatabase.LoadShader("World/WorldOverlayAdditive");

		// Token: 0x0400010C RID: 268
		public static readonly Shader MetaOverlay = ShaderDatabase.LoadShader("Map/MetaOverlay");

		// Token: 0x0400010D RID: 269
		public static readonly Shader MetaOverlayDesaturated = ShaderDatabase.LoadShader("Map/MetaOverlayDesaturated");

		// Token: 0x0400010E RID: 270
		public static readonly Shader SolidColor = ShaderDatabase.LoadShader("Map/SolidColor");

		// Token: 0x0400010F RID: 271
		public static readonly Shader VertexColor = ShaderDatabase.LoadShader("Map/VertexColor");

		// Token: 0x04000110 RID: 272
		public static readonly Shader RitualStencil = ShaderDatabase.LoadShader("Map/RitualStencil");

		// Token: 0x04000111 RID: 273
		public static readonly Shader Invisible = ShaderDatabase.LoadShader("Misc/Invisible");

		// Token: 0x04000112 RID: 274
		private static Dictionary<string, Shader> lookup;
	}
}
