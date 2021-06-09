using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000880 RID: 2176
	public static class SolidColorMaterials
	{
		// Token: 0x1700084B RID: 2123
		// (get) Token: 0x060035FE RID: 13822 RVA: 0x00029DA8 File Offset: 0x00027FA8
		public static int SimpleColorMatCount
		{
			get
			{
				return SolidColorMaterials.simpleColorMats.Count + SolidColorMaterials.simpleColorAndVertexColorMats.Count;
			}
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x0015B40C File Offset: 0x0015960C
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

		// Token: 0x06003600 RID: 13824 RVA: 0x0015B47C File Offset: 0x0015967C
		public static Material NewSolidColorMaterial(Color col, Shader shader)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a material from a different thread.", false);
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

		// Token: 0x06003601 RID: 13825 RVA: 0x00029DBF File Offset: 0x00027FBF
		public static Texture2D NewSolidColorTexture(float r, float g, float b, float a)
		{
			return SolidColorMaterials.NewSolidColorTexture(new Color(r, g, b, a));
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x0015B4E0 File Offset: 0x001596E0
		public static Texture2D NewSolidColorTexture(Color color)
		{
			if (!UnityData.IsInMainThread)
			{
				Log.Error("Tried to create a texture from a different thread.", false);
				return null;
			}
			Texture2D texture2D = new Texture2D(1, 1);
			texture2D.name = "SolidColorTex-" + color;
			texture2D.SetPixel(0, 0, color);
			texture2D.Apply();
			return texture2D;
		}

		// Token: 0x040025AD RID: 9645
		private static Dictionary<Color, Material> simpleColorMats = new Dictionary<Color, Material>();

		// Token: 0x040025AE RID: 9646
		private static Dictionary<Color, Material> simpleColorAndVertexColorMats = new Dictionary<Color, Material>();
	}
}
