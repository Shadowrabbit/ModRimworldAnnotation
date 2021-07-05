using System;
using System.IO;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002FB RID: 763
	public static class TextureAtlasHelper
	{
		// Token: 0x06001626 RID: 5670 RVA: 0x00081010 File Offset: 0x0007F210
		public static Mesh CreateMeshForUV(Rect uv, float scale = 1f)
		{
			return new Mesh
			{
				vertices = new Vector3[]
				{
					new Vector3(-1f * scale, 0f, -1f * scale),
					new Vector3(-1f * scale, 0f, 1f * scale),
					new Vector3(1f * scale, 0f, 1f * scale),
					new Vector3(1f * scale, 0f, -1f * scale)
				},
				normals = new Vector3[]
				{
					Vector3.up,
					Vector3.up,
					Vector3.up,
					Vector3.up
				},
				uv = new Vector2[]
				{
					uv.min,
					new Vector2(uv.xMin, uv.yMax),
					uv.max,
					new Vector2(uv.xMax, uv.yMin)
				},
				triangles = new int[]
				{
					0,
					1,
					2,
					2,
					3,
					0
				}
			};
		}

		// Token: 0x06001627 RID: 5671 RVA: 0x0008115C File Offset: 0x0007F35C
		public static void WriteDebugPNG(RenderTexture atlas, string path)
		{
			Texture2D texture2D = new Texture2D(atlas.width, atlas.height, TextureFormat.ARGB32, false);
			RenderTexture.active = atlas;
			texture2D.ReadPixels(new Rect(0f, 0f, (float)atlas.width, (float)atlas.height), 0, 0);
			RenderTexture.active = null;
			File.WriteAllBytes(path, texture2D.EncodeToPNG());
		}

		// Token: 0x06001628 RID: 5672 RVA: 0x000811BA File Offset: 0x0007F3BA
		public static void WriteDebugPNG(Texture2D atlas, string path)
		{
			File.WriteAllBytes(path, atlas.EncodeToPNG());
		}

		// Token: 0x06001629 RID: 5673 RVA: 0x000811C8 File Offset: 0x0007F3C8
		public static TextureAtlasGroup ToAtlasGroup(this ThingCategory category)
		{
			switch (category)
			{
			case ThingCategory.Item:
				return TextureAtlasGroup.Item;
			case ThingCategory.Building:
				return TextureAtlasGroup.Building;
			case ThingCategory.Plant:
				return TextureAtlasGroup.Plant;
			case ThingCategory.Filth:
				return TextureAtlasGroup.Filth;
			}
			return TextureAtlasGroup.Misc;
		}
	}
}
