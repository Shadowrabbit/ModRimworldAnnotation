using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse.Noise
{
	// Token: 0x02000514 RID: 1300
	public static class GradientPresets
	{
		// Token: 0x06002747 RID: 10055 RVA: 0x000F1E7C File Offset: 0x000F007C
		static GradientPresets()
		{
			List<GradientColorKey> list = new List<GradientColorKey>();
			list.Add(new GradientColorKey(Color.black, 0f));
			list.Add(new GradientColorKey(Color.white, 1f));
			List<GradientColorKey> list2 = new List<GradientColorKey>();
			list2.Add(new GradientColorKey(Color.red, 0f));
			list2.Add(new GradientColorKey(Color.green, 0.5f));
			list2.Add(new GradientColorKey(Color.blue, 1f));
			List<GradientColorKey> list3 = new List<GradientColorKey>();
			list3.Add(new GradientColorKey(Color.red, 0f));
			list3.Add(new GradientColorKey(Color.green, 0.33333334f));
			list3.Add(new GradientColorKey(Color.blue, 0.6666667f));
			list3.Add(new GradientColorKey(Color.black, 1f));
			List<GradientAlphaKey> list4 = new List<GradientAlphaKey>();
			list4.Add(new GradientAlphaKey(0f, 0.6666667f));
			list4.Add(new GradientAlphaKey(1f, 1f));
			List<GradientColorKey> list5 = new List<GradientColorKey>();
			list5.Add(new GradientColorKey(new Color(0f, 0f, 0.5f), 0f));
			list5.Add(new GradientColorKey(new Color(0.125f, 0.25f, 0.5f), 0.4f));
			list5.Add(new GradientColorKey(new Color(0.25f, 0.375f, 0.75f), 0.48f));
			list5.Add(new GradientColorKey(new Color(0f, 0.75f, 0f), 0.5f));
			list5.Add(new GradientColorKey(new Color(0.75f, 0.75f, 0f), 0.625f));
			list5.Add(new GradientColorKey(new Color(0.625f, 0.375f, 0.25f), 0.75f));
			list5.Add(new GradientColorKey(new Color(0.5f, 1f, 1f), 0.875f));
			list5.Add(new GradientColorKey(Color.white, 1f));
			List<GradientAlphaKey> list6 = new List<GradientAlphaKey>();
			list6.Add(new GradientAlphaKey(1f, 0f));
			list6.Add(new GradientAlphaKey(1f, 1f));
			GradientPresets._empty = new Gradient();
			GradientPresets._rgb = new Gradient();
			GradientPresets._rgb.SetKeys(list2.ToArray(), list6.ToArray());
			GradientPresets._rgba = new Gradient();
			GradientPresets._rgba.SetKeys(list3.ToArray(), list4.ToArray());
			GradientPresets._grayscale = new Gradient();
			GradientPresets._grayscale.SetKeys(list.ToArray(), list6.ToArray());
			GradientPresets._terrain = new Gradient();
			GradientPresets._terrain.SetKeys(list5.ToArray(), list6.ToArray());
		}

		// Token: 0x170007AF RID: 1967
		// (get) Token: 0x06002748 RID: 10056 RVA: 0x000F2169 File Offset: 0x000F0369
		public static Gradient Empty
		{
			get
			{
				return GradientPresets._empty;
			}
		}

		// Token: 0x170007B0 RID: 1968
		// (get) Token: 0x06002749 RID: 10057 RVA: 0x000F2170 File Offset: 0x000F0370
		public static Gradient Grayscale
		{
			get
			{
				return GradientPresets._grayscale;
			}
		}

		// Token: 0x170007B1 RID: 1969
		// (get) Token: 0x0600274A RID: 10058 RVA: 0x000F2177 File Offset: 0x000F0377
		public static Gradient RGB
		{
			get
			{
				return GradientPresets._rgb;
			}
		}

		// Token: 0x170007B2 RID: 1970
		// (get) Token: 0x0600274B RID: 10059 RVA: 0x000F217E File Offset: 0x000F037E
		public static Gradient RGBA
		{
			get
			{
				return GradientPresets._rgba;
			}
		}

		// Token: 0x170007B3 RID: 1971
		// (get) Token: 0x0600274C RID: 10060 RVA: 0x000F2185 File Offset: 0x000F0385
		public static Gradient Terrain
		{
			get
			{
				return GradientPresets._terrain;
			}
		}

		// Token: 0x04001865 RID: 6245
		private static Gradient _empty;

		// Token: 0x04001866 RID: 6246
		private static Gradient _grayscale;

		// Token: 0x04001867 RID: 6247
		private static Gradient _rgb;

		// Token: 0x04001868 RID: 6248
		private static Gradient _rgba;

		// Token: 0x04001869 RID: 6249
		private static Gradient _terrain;
	}
}
