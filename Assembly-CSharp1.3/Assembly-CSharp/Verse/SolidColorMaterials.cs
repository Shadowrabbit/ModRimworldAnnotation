using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004D5 RID: 1237
	public static class SolidColorMaterials
	{
		// Token: 0x17000729 RID: 1833
		// (get) Token: 0x0600256C RID: 9580 RVA: 0x000E984D File Offset: 0x000E7A4D
		public static int SimpleColorMatCount
		{
			get
			{
				return SolidColorMaterials.simpleColorMats.Count + SolidColorMaterials.simpleColorAndVertexColorMats.Count;
			}
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x000E9864 File Offset: 0x000E7A64
		public static Material SimpleSolidColorMaterial(Color col, bool careAboutVertexColors = false)
		{
			col = col;
			Material material;
			if (careAboutVertexColors)
			{
				if (!SolidColorMaterials.simpleColorAndVertexColorMats.TryGetValue(col, out material))
				{
					material = SolidColorMaterials.NewSolidColorMaterial(col, ShaderDatabase.VertexColor);
					SolidColorMaterials.simpleColorAndVertexColorMats.Add(col, material);
				}
			}
			else if (!SolidColorMaterials.simpleColorMats.TryGetValue(col, out material))
			{
				material = SolidColorMaterials.NewSolidColorMaterial(col, ShaderDatabase.SolidColor);
				SolidColorMaterials.simpleColorMats.Add(col, material);
			}
			return material;
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x000E98D4 File Offset: 0x000E7AD4
		public static Material NewSolidColorMaterial(Color col, Shader shader)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a material from a different thread.");
				return null;
			}
			Material material = MaterialAllocator.Create(shader);
			material.color = col;
			material.name = string.Concat(new object[]
			{
				"SolidColorMat-",
				shader.name,
				"-",
				col
			});
			return material;
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x000E9936 File Offset: 0x000E7B36
		public static Texture2D NewSolidColorTexture(float r, float g, float b, float a)
		{
			return SolidColorMaterials.NewSolidColorTexture(new Color(r, g, b, a));
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x000E9948 File Offset: 0x000E7B48
		public static Texture2D NewSolidColorTexture(Color color)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a texture from a different thread.");
				return null;
			}
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.name = "SolidColorTex-" + color;
			texture2D.SetPixel(0, 0, color);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x04001771 RID: 6001
		private static Dictionary<Color, Material> simpleColorMats = new Dictionary<Color, Material>();

		// Token: 0x04001772 RID: 6002
		private static Dictionary<Color, Material> simpleColorAndVertexColorMats = new Dictionary<Color, Material>();
	}
}
