using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020004E5 RID: 1253
	public class Graphic_Single : Graphic
	{
		// Token: 0x170005CF RID: 1487
		// (get) Token: 0x06001F44 RID: 8004 RVA: 0x0001B978 File Offset: 0x00019B78
		public override Material MatSingle
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170005D0 RID: 1488
		// (get) Token: 0x06001F45 RID: 8005 RVA: 0x0001B978 File Offset: 0x00019B78
		public override Material MatWest
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170005D1 RID: 1489
		// (get) Token: 0x06001F46 RID: 8006 RVA: 0x0001B978 File Offset: 0x00019B78
		public override Material MatSouth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170005D2 RID: 1490
		// (get) Token: 0x06001F47 RID: 8007 RVA: 0x0001B978 File Offset: 0x00019B78
		public override Material MatEast
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170005D3 RID: 1491
		// (get) Token: 0x06001F48 RID: 8008 RVA: 0x0001B978 File Offset: 0x00019B78
		public override Material MatNorth
		{
			get
			{
				return this.mat;
			}
		}

		// Token: 0x170005D4 RID: 1492
		// (get) Token: 0x06001F49 RID: 8009 RVA: 0x0001B980 File Offset: 0x00019B80
		public override bool ShouldDrawRotated
		{
			get
			{
				return this.data == null || this.data.drawRotated;
			}
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x000FF6D4 File Offset: 0x000FD8D4
		public override void Init(GraphicRequest req)
		{
			this.data = req.graphicData;
			this.path = req.path;
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
				req2.maskTex = ContentFinder<Texture2D>.Get(req.path + Graphic_Single.MaskSuffix, false);
			}
			this.mat = MaterialPool.MatFrom(req2);
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x0001B99A File Offset: 0x00019B9A
		public override Graphic GetColoredVersion(Shader newShader, Color newColor, Color newColorTwo)
		{
			return GraphicDatabase.Get<Graphic_Single>(this.path, newShader, this.drawSize, newColor, newColorTwo, this.data);
		}

		// Token: 0x06001F4C RID: 8012 RVA: 0x0001B978 File Offset: 0x00019B78
		public override Material MatAt(Rot4 rot, Thing thing = null)
		{
			return this.mat;
		}

		// Token: 0x06001F4D RID: 8013 RVA: 0x000FF7B0 File Offset: 0x000FD9B0
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

		// Token: 0x040015FF RID: 5631
		protected Material mat;

		// Token: 0x04001600 RID: 5632
		public static readonly string MaskSuffix = "_m";
	}
}
