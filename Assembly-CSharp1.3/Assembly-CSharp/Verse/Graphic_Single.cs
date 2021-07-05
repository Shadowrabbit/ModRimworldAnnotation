using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200035E RID: 862
	public class Graphic_Single : Graphic
	{
		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06001872 RID: 6258 RVA: 0x00090F30 File Offset: 0x0008F130
		public override Material MatSingle
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06001873 RID: 6259 RVA: 0x00090F30 File Offset: 0x0008F130
		public override Material MatWest
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06001874 RID: 6260 RVA: 0x00090F30 File Offset: 0x0008F130
		public override Material MatSouth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06001875 RID: 6261 RVA: 0x00090F30 File Offset: 0x0008F130
		public override Material MatEast
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06001876 RID: 6262 RVA: 0x00090F30 File Offset: 0x0008F130
		public override Material MatNorth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170004F6 RID: 1270
		// (get) Token: 0x06001877 RID: 6263 RVA: 0x00090F38 File Offset: 0x0008F138
		public override bool ShouldDrawRotated
		{
			get
			{
				return this.data == null || this.data.drawRotated;
			}
		}

		// Token: 0x06001878 RID: 6264 RVA: 0x00090F54 File Offset: 0x0008F154
		public override void TryInsertIntoAtlas(TextureAtlasGroup groupKey)
		{
			Texture2D mask = null;
			if (this.mat.HasProperty(ShaderPropertyIDs.MaskTex))
			{
				mask = (Texture2D)this.mat.GetTexture(ShaderPropertyIDs.MaskTex);
			}
			GlobalTextureAtlasManager.TryInsertStatic(groupKey, (Texture2D)this.mat.mainTexture, mask);
		}

		// Token: 0x06001879 RID: 6265 RVA: 0x00090FA4 File Offset: 0x0008F1A4
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
			this.maskPath = req.maskPath;
			this.color = req.color;
			this.colorTwo = req.colorTwo;
			this.drawSize = req.drawSize;
			MaterialRequest req2 = default(MaterialRequest);
			req2.mainTex = ContentFinder<Texture2D>.Get(req.path, true);
			req2.shader = req.shader;
			req2.color = this.color;
			req2.colorTwo = this.colorTwo;
			req2.renderQueue = req.renderQueue;
			req2.shaderParameters = req.shaderParameters;
			if (req.shader.SupportsMaskTex())
			{
				req2.maskTex = ContentFinder<Texture2D>.Get(this.maskPath.NullOrEmpty() ? (this.path + Graphic_Single.MaskSuffix) : this.maskPath, false);
			}
			this.mat = MaterialPool.MatFrom(req2);
		}

		// Token: 0x0600187A RID: 6266 RVA: 0x000910A0 File Offset: 0x0008F2A0
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data, null);
		}

		// Token: 0x0600187B RID: 6267 RVA: 0x00090F30 File Offset: 0x0008F130
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.mat;
		}

		// Token: 0x0600187C RID: 6268 RVA: 0x000910C0 File Offset: 0x0008F2C0
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"Single(path=",
				this.path,
				", color=",
				this.color,
				", colorTwo=",
				this.colorTwo,
				")"
			});
		}

		// Token: 0x040010A3 RID: 4259
		protected Material mat;

		// Token: 0x040010A4 RID: 4260
		public static readonly string MaskSuffix = "_m";
	}
}
