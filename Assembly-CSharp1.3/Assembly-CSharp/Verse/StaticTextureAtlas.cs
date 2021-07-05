using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000301 RID: 769
	public class StaticTextureAtlas
	{
		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x06001635 RID: 5685 RVA: 0x000815B4 File Offset: 0x0007F7B4
		public Texture2D ColorTexture
		{
			get
			{
				return this.colorTexture;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x06001636 RID: 5686 RVA: 0x000815BC File Offset: 0x0007F7BC
		public Texture2D MaskTexture
		{
			get
			{
				return this.maskTexture;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x06001637 RID: 5687 RVA: 0x000815C4 File Offset: 0x0007F7C4
		public static int MaxPixelsPerAtlas
		{
			get
			{
				return StaticTextureAtlas.MaxAtlasSize / 2 * (StaticTextureAtlas.MaxAtlasSize / 2);
			}
		}

		// Token: 0x170004AB RID: 1195
		// (get) Token: 0x06001638 RID: 5688 RVA: 0x000815D5 File Offset: 0x0007F7D5
		public static int MaxAtlasSize
		{
			get
			{
				return SystemInfo.maxTextureSize;
			}
		}

		// Token: 0x06001639 RID: 5689 RVA: 0x000815DC File Offset: 0x0007F7DC
		public StaticTextureAtlas(TextureAtlasGroup group)
		{
			this.group = group;
			this.colorTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
			this.maskTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
		}

		// Token: 0x0600163A RID: 5690 RVA: 0x00081635 File Offset: 0x0007F835
		public void Insert(Texture2D texture, Texture2D mask = null)
		{
			this.textures.Add(texture);
			if (mask != null)
			{
				this.masks.Add(texture, mask);
			}
		}

		// Token: 0x0600163B RID: 5691 RVA: 0x0008165C File Offset: 0x0007F85C
		public void Bake(bool rebake = false)
		{
			if (rebake)
			{
				foreach (KeyValuePair<Texture, StaticTextureAtlasTile> keyValuePair in this.tiles)
				{
					UnityEngine.Object.Destroy(keyValuePair.Value.mesh);
				}
				this.tiles.Clear();
			}
			List<Texture2D> destroyTextures = new List<Texture2D>();
			try
			{
				Texture2D[] array = this.textures.Select(delegate(Texture2D t)
				{
					if (!t.isReadable)
					{
						Texture2D texture2D3 = StaticTextureAtlas.<Bake>g__MakeReadableTextureInstance|18_0(t);
						destroyTextures.Add(texture2D3);
						return texture2D3;
					}
					return t;
				}).ToArray<Texture2D>();
				Rect[] array2 = this.colorTexture.PackTextures(array, 8, StaticTextureAtlas.MaxAtlasSize, false);
				this.colorTexture.name = string.Concat(new object[]
				{
					"TextureAtlas_",
					this.group.ToString(),
					"_",
					this.colorTexture.GetInstanceID()
				});
				this.maskTexture.Resize(this.colorTexture.width, this.colorTexture.height);
				for (int i = 0; i < array2.Length; i++)
				{
					Texture2D key = this.textures[i];
					Texture2D texture2D;
					if (this.masks.TryGetValue(key, out texture2D))
					{
						Rect rect = array2[i];
						int x = (int)(rect.xMin * (float)this.colorTexture.width);
						int y = (int)(rect.yMin * (float)this.colorTexture.height);
						if (!texture2D.isReadable)
						{
							Texture2D texture2D2 = StaticTextureAtlas.<Bake>g__MakeReadableTextureInstance|18_0(texture2D);
							destroyTextures.Add(texture2D2);
							texture2D = texture2D2;
						}
						this.maskTexture.SetPixels(x, y, this.textures[i].width, this.textures[i].height, texture2D.GetPixels(0), 0);
					}
				}
				this.maskTexture.name = "Mask_" + this.colorTexture.name;
				this.maskTexture.Apply(true, false);
				if (array2.Length != array.Length)
				{
					Log.Error("Texture packing failed! Clearing out atlas...");
					this.textures.Clear();
				}
				else
				{
					for (int j = 0; j < array.Length; j++)
					{
						Mesh mesh = TextureAtlasHelper.CreateMeshForUV(array2[j], 0.5f);
						mesh.name = string.Concat(new object[]
						{
							"TextureAtlasMesh_",
							this.group.ToString(),
							"_",
							mesh.GetInstanceID()
						});
						this.tiles.Add(this.textures[j], new StaticTextureAtlasTile
						{
							atlas = this,
							mesh = mesh,
							uvRect = array2[j]
						});
					}
				}
			}
			finally
			{
				foreach (Texture2D obj in destroyTextures)
				{
					UnityEngine.Object.Destroy(obj);
				}
			}
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x000819C0 File Offset: 0x0007FBC0
		public bool TryGetTile(Texture texture, out StaticTextureAtlasTile tile)
		{
			return this.tiles.TryGetValue(texture, out tile);
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x000819D0 File Offset: 0x0007FBD0
		public void Destroy()
		{
			UnityEngine.Object.Destroy(this.colorTexture);
			UnityEngine.Object.Destroy(this.maskTexture);
			foreach (KeyValuePair<Texture, StaticTextureAtlasTile> keyValuePair in this.tiles)
			{
				UnityEngine.Object.Destroy(keyValuePair.Value.mesh);
			}
			this.textures.Clear();
			this.tiles.Clear();
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x00081A5C File Offset: 0x0007FC5C
		[CompilerGenerated]
		internal static Texture2D <Bake>g__MakeReadableTextureInstance|18_0(Texture2D source)
		{
			RenderTexture temporary = RenderTexture.GetTemporary(source.width, source.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
			Graphics.Blit(source, temporary);
			RenderTexture active = RenderTexture.active;
			RenderTexture.active = temporary;
			Texture2D texture2D = new Texture2D(source.width, source.height);
			texture2D.ReadPixels(new Rect(0f, 0f, (float)temporary.width, (float)temporary.height), 0, 0);
			texture2D.Apply();
			RenderTexture.active = active;
			RenderTexture.ReleaseTemporary(temporary);
			return texture2D;
		}

		// Token: 0x04000F74 RID: 3956
		public readonly TextureAtlasGroup group;

		// Token: 0x04000F75 RID: 3957
		private List<Texture2D> textures = new List<Texture2D>();

		// Token: 0x04000F76 RID: 3958
		private Dictionary<Texture2D, Texture2D> masks = new Dictionary<Texture2D, Texture2D>();

		// Token: 0x04000F77 RID: 3959
		private Dictionary<Texture, StaticTextureAtlasTile> tiles = new Dictionary<Texture, StaticTextureAtlasTile>();

		// Token: 0x04000F78 RID: 3960
		private Texture2D colorTexture;

		// Token: 0x04000F79 RID: 3961
		private Texture2D maskTexture;

		// Token: 0x04000F7A RID: 3962
		public const int MaxTextureSizeForTiles = 512;

		// Token: 0x04000F7B RID: 3963
		public const int TexturePadding = 8;
	}
}
