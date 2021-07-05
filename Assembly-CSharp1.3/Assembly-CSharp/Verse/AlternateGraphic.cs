using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000ED RID: 237
	public class AlternateGraphic
	{
		// Token: 0x17000123 RID: 291
		// (get) Token: 0x0600066C RID: 1644 RVA: 0x0001F792 File Offset: 0x0001D992
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x0600066D RID: 1645 RVA: 0x0001F79C File Offset: 0x0001D99C
		public Graphic GetGraphic(Graphic other)
		{
			if (this.graphicData == null)
			{
				this.graphicData = new GraphicData();
			}
			this.graphicData.CopyFrom(other.data);
			if (!this.texPath.NullOrEmpty())
			{
				this.graphicData.texPath = this.texPath;
			}
			this.graphicData.color = (this.color ?? other.color);
			this.graphicData.colorTwo = (this.colorTwo ?? other.colorTwo);
			return this.graphicData.Graphic;
		}

		// Token: 0x0400059E RID: 1438
		private float weight = 0.5f;

		// Token: 0x0400059F RID: 1439
		private string texPath;

		// Token: 0x040005A0 RID: 1440
		private Color? color;

		// Token: 0x040005A1 RID: 1441
		private Color? colorTwo;

		// Token: 0x040005A2 RID: 1442
		private GraphicData graphicData;
	}
}
