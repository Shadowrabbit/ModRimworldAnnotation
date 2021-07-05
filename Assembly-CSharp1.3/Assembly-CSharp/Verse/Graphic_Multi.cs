using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200035A RID: 858
	public class Graphic_Multi : Graphic
	{
		// Token: 0x170004E5 RID: 1253
		// (get) Token: 0x0600184C RID: 6220 RVA: 0x0009060C File Offset: 0x0008E80C
		public string GraphicPath
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x170004E6 RID: 1254
		// (get) Token: 0x0600184D RID: 6221 RVA: 0x00090614 File Offset: 0x0008E814
		public override Material MatSingle
		{
			get
			{
				return this.MatSouth;
			}
		}

		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x0600184E RID: 6222 RVA: 0x0009061C File Offset: 0x0008E81C
		public override Material MatWest
		{
			get
			{
				return this.mats[3];
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x0600184F RID: 6223 RVA: 0x00090626 File Offset: 0x0008E826
		public override Material MatSouth
		{
			get
			{
				return this.mats[2];
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06001850 RID: 6224 RVA: 0x00090630 File Offset: 0x0008E830
		public override Material MatEast
		{
			get
			{
				return this.mats[1];
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06001851 RID: 6225 RVA: 0x0009063A File Offset: 0x0008E83A
		public override Material MatNorth
		{
			get
			{
				return this.mats[0];
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06001852 RID: 6226 RVA: 0x00090644 File Offset: 0x0008E844
		public override bool WestFlipped
		{
			get
			{
				return this.westFlipped;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x06001853 RID: 6227 RVA: 0x0009064C File Offset: 0x0008E84C
		public override bool EastFlipped
		{
			get
			{
				return this.eastFlipped;
			}
		}

		// Token: 0x170004ED RID: 1261
		// (get) Token: 0x06001854 RID: 6228 RVA: 0x00090654 File Offset: 0x0008E854
		public override bool ShouldDrawRotated
		{
			get
			{
				return (this.data == null || this.data.drawRotated) && (this.MatEast == this.MatNorth || this.MatWest == this.MatNorth);
			}
		}

		// Token: 0x170004EE RID: 1262
		// (get) Token: 0x06001855 RID: 6229 RVA: 0x00090693 File Offset: 0x0008E893
		public override float DrawRotatedExtraAngleOffset
		{
			get
			{
				return this.drawRotatedExtraAngleOffset;
			}
		}

		// Token: 0x06001856 RID: 6230 RVA: 0x0009069C File Offset: 0x0008E89C
		public override void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
			foreach (Material material in this.mats)
			{
				Texture2D mask = null;
				if (material.HasProperty(ShaderPropertyIDs.MaskTex))
				{
					mask = (Texture2D)material.GetTexture(ShaderPropertyIDs.MaskTex);
				}
				GlobalTextureAtlasManager.TryInsertStatic(groupKey, (Texture2D)material.mainTexture, mask);
			}
		}

		// Token: 0x06001857 RID: 6231 RVA: 0x000906F8 File Offset: 0x0008E8F8
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			Texture2D[] array = new Texture2D[this.mats.Length];
			array[0] = ContentFinder<Texture2D>.Get(req.path + "_north", false);
			array[1] = ContentFinder<Texture2D>.Get(req.path + "_east", false);
			array[2] = ContentFinder<Texture2D>.Get(req.path + "_south", false);
			array[3] = ContentFinder<Texture2D>.Get(req.path + "_west", false);
			if (array[0] == null)
			{
				if (array[2] != null)
				{
					array[0] = array[2];
					this.drawRotatedExtraAngleOffset = 180f;
				}
				else if (array[1] != null)
				{
					array[0] = array[1];
					this.drawRotatedExtraAngleOffset = -90f;
				}
				else if (array[3] != null)
				{
					array[0] = array[3];
					this.drawRotatedExtraAngleOffset = 90f;
				}
				else
				{
					array[0] = ContentFinder<Texture2D>.Get(req.path, false);
				}
			}
			if (array[0] == null)
			{
				Log.Error("Failed to find any textures at " + req.path + " while constructing " + this.ToStringSafe<Graphic_Multi>());
				return;
			}
			if (array[2] == null)
			{
				array[2] = array[0];
			}
			if (array[1] == null)
			{
				if (array[3] != null)
				{
					array[1] = array[3];
					this.eastFlipped = base.DataAllowsFlip;
				}
				else
				{
					array[1] = array[0];
				}
			}
			if (array[3] == null)
			{
				if (array[1] != null)
				{
					array[3] = array[1];
					this.westFlipped = base.DataAllowsFlip;
				}
				else
				{
					array[3] = array[0];
				}
			}
			Texture2D[] array2 = new Texture2D[this.mats.Length];
			if (req.shader.SupportsMaskTex())
			{
				string str = this.maskPath.NullOrEmpty() ? this.path : this.maskPath;
				string str2 = this.maskPath.NullOrEmpty() ? "m" : string.Empty;
				array2[0] = ContentFinder<Texture2D>.Get(str + "_north" + str2, false);
				array2[1] = ContentFinder<Texture2D>.Get(str + "_east" + str2, false);
				array2[2] = ContentFinder<Texture2D>.Get(str + "_south" + str2, false);
				array2[3] = ContentFinder<Texture2D>.Get(str + "_west" + str2, false);
				if (array2[0] == null)
				{
					if (array2[2] != null)
					{
						array2[0] = array2[2];
					}
					else if (array2[1] != null)
					{
						array2[0] = array2[1];
					}
					else if (array2[3] != null)
					{
						array2[0] = array2[3];
					}
				}
				if (array2[2] == null)
				{
					array2[2] = array2[0];
				}
				if (array2[1] == null)
				{
					if (array2[3] != null)
					{
						array2[1] = array2[3];
					}
					else
					{
						array2[1] = array2[0];
					}
				}
				if (array2[3] == null)
				{
					if (array2[1] != null)
					{
						array2[3] = array2[1];
					}
					else
					{
						array2[3] = array2[0];
					}
				}
			}
			for (int i = 0; i < this.mats.Length; i++)
			{
				MaterialRequest req2 = default(MaterialRequest);
				req2.mainTex = array[i];
				req2.shader = req.shader;
				req2.color = this.color;
				req2.colorTwo = this.colorTwo;
				req2.maskTex = array2[i];
				req2.shaderParameters = req.shaderParameters;
				this.mats[i] = MaterialPool.MatFrom(req2);
			}
		}

		// Token: 0x06001858 RID: 6232 RVA: 0x00090A8C File Offset: 0x0008EC8C
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Multi>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x06001859 RID: 6233 RVA: 0x00090AAC File Offset: 0x0008ECAC
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Multi(initPath=",
				this.path,
				", color=",
				this.color,
				", colorTwo=",
				this.colorTwo,
				")"
			});
		}

		// Token: 0x0600185A RID: 6234 RVA: 0x00090B09 File Offset: 0x0008ED09
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<string>(0, this.path), this.color), this.colorTwo);
		}

		// Token: 0x04001099 RID: 4249
		private Material[] mats = new Material[4];

		// Token: 0x0400109A RID: 4250
		private bool westFlipped;

		// Token: 0x0400109B RID: 4251
		private bool eastFlipped;

		// Token: 0x0400109C RID: 4252
		private float drawRotatedExtraAngleOffset;
	}
}
