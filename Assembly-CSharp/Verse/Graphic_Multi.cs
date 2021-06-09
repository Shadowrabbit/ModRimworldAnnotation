using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E1 RID: 1249
	public class Graphic_Multi : Graphic
	{
		// Token: 0x170005C3 RID: 1475
		// (get) Token: 0x06001F22 RID: 7970 RVA: 0x0001B703 File Offset: 0x00019903
		public string GraphicPath
		{
			get
			{
				return this.path;
			}
		}

		// Token: 0x170005C4 RID: 1476
		// (get) Token: 0x06001F23 RID: 7971 RVA: 0x0001B70B File Offset: 0x0001990B
		public override Material MatSingle
		{
			get
			{
				return this.MatSouth;
			}
		}

		// Token: 0x170005C5 RID: 1477
		// (get) Token: 0x06001F24 RID: 7972 RVA: 0x0001B713 File Offset: 0x00019913
		public override Material MatWest
		{
			get
			{
				return this.mats[3];
			}
		}

		// Token: 0x170005C6 RID: 1478
		// (get) Token: 0x06001F25 RID: 7973 RVA: 0x0001B71D File Offset: 0x0001991D
		public override Material MatSouth
		{
			get
			{
				return this.mats[2];
			}
		}

		// Token: 0x170005C7 RID: 1479
		// (get) Token: 0x06001F26 RID: 7974 RVA: 0x0001B727 File Offset: 0x00019927
		public override Material MatEast
		{
			get
			{
				return this.mats[1];
			}
		}

		// Token: 0x170005C8 RID: 1480
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x0001B731 File Offset: 0x00019931
		public override Material MatNorth
		{
			get
			{
				return this.mats[0];
			}
		}

		// Token: 0x170005C9 RID: 1481
		// (get) Token: 0x06001F28 RID: 7976 RVA: 0x0001B73B File Offset: 0x0001993B
		public override bool WestFlipped
		{
			get
			{
				return this.westFlipped;
			}
		}

		// Token: 0x170005CA RID: 1482
		// (get) Token: 0x06001F29 RID: 7977 RVA: 0x0001B743 File Offset: 0x00019943
		public override bool EastFlipped
		{
			get
			{
				return this.eastFlipped;
			}
		}

		// Token: 0x170005CB RID: 1483
		// (get) Token: 0x06001F2A RID: 7978 RVA: 0x0001B74B File Offset: 0x0001994B
		public override bool ShouldDrawRotated
		{
			get
			{
				return (this.data == null || this.data.drawRotated) && (this.MatEast == this.MatNorth || this.MatWest == this.MatNorth);
			}
		}

		// Token: 0x170005CC RID: 1484
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x0001B78A File Offset: 0x0001998A
		public override float DrawRotatedExtraAngleOffset
		{
			get
			{
				return this.drawRotatedExtraAngleOffset;
			}
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x000FF178 File Offset: 0x000FD378
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
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
				Log.Error("Failed to find any textures at " + req.path + " while constructing " + this.ToStringSafe<Graphic_Multi>(), false);
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
				array2[0] = ContentFinder<Texture2D>.Get(req.path + "_northm", false);
				array2[1] = ContentFinder<Texture2D>.Get(req.path + "_eastm", false);
				array2[2] = ContentFinder<Texture2D>.Get(req.path + "_southm", false);
				array2[3] = ContentFinder<Texture2D>.Get(req.path + "_westm", false);
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

		// Token: 0x06001F2D RID: 7981 RVA: 0x0001B792 File Offset: 0x00019992
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Multi>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x000FF4D4 File Offset: 0x000FD6D4
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

		// Token: 0x06001F2F RID: 7983 RVA: 0x0001B7AE File Offset: 0x000199AE
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<Color>(Gen.HashCombineStruct<Color>(Gen.HashCombine<string>(0, this.path), this.color), this.colorTwo);
		}

		// Token: 0x040015F5 RID: 5621
		private Material[] mats = new Material[4];

		// Token: 0x040015F6 RID: 5622
		private bool westFlipped;

		// Token: 0x040015F7 RID: 5623
		private bool eastFlipped;

		// Token: 0x040015F8 RID: 5624
		private float drawRotatedExtraAngleOffset;
	}
}
