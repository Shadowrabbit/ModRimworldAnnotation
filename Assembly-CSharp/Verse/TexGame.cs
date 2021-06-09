using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000A7 RID: 167
	[StaticConstructorOnStartup]
	public static class TexGame
	{
		// Token: 0x06000568 RID: 1384 RVA: 0x0008BAD4 File Offset: 0x00089CD4
		static TexGame()
		{
			Shader.SetGlobalTexture("_NoiseTex", TexGame.NoiseTex);
			Shader.SetGlobalTexture("_RippleTex", TexGame.RippleTex);
		}

		// Token: 0x040002A4 RID: 676
		public static readonly Texture2D AlphaAddTex = ContentFinder<Texture2D>.Get("Other/RoughAlphaAdd", true);

		// Token: 0x040002A5 RID: 677
		public static readonly Texture2D RippleTex = ContentFinder<Texture2D>.Get("Other/Ripples", true);

		// Token: 0x040002A6 RID: 678
		public static readonly Texture2D NoiseTex = ContentFinder<Texture2D>.Get("Other/Noise", true);

		// Token: 0x040002A7 RID: 679
		public static readonly Texture2D InvisDistortion = ContentFinder<Texture2D>.Get("Other/InvisDistortion", true);
	}
}
