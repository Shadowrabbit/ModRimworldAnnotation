using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200005F RID: 95
	[StaticConstructorOnStartup]
	public static class TexGame
	{
		// Token: 0x0600040F RID: 1039 RVA: 0x00015B30 File Offset: 0x00013D30
		static TexGame()
		{
			Shader.SetGlobalTexture("_NoiseTex", TexGame.NoiseTex);
			Shader.SetGlobalTexture("_RippleTex", TexGame.RippleTex);
		}

		// Token: 0x04000137 RID: 311
		public static readonly Texture2D AlphaAddTex = ContentFinder<Texture2D>.Get("Other/RoughAlphaAdd", true);

		// Token: 0x04000138 RID: 312
		public static readonly Texture2D RippleTex = ContentFinder<Texture2D>.Get("Other/Ripples", true);

		// Token: 0x04000139 RID: 313
		public static readonly Texture2D NoiseTex = ContentFinder<Texture2D>.Get("Other/Noise", true);

		// Token: 0x0400013A RID: 314
		public static readonly Texture2D InvisDistortion = ContentFinder<Texture2D>.Get("Other/InvisDistortion", true);
	}
}
