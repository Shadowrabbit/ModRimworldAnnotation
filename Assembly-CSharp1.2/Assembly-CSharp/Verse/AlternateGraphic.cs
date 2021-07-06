using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000163 RID: 355
	public class AlternateGraphic
	{
		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x060008EF RID: 2287 RVA: 0x0000D0AF File Offset: 0x0000B2AF
		public float Weight
		{
			get
			{
				return this.weight;
			}
		}

		// Token: 0x060008F0 RID: 2288 RVA: 0x00097B7C File Offset: 0x00095D7C
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

		// Token: 0x04000793 RID: 1939
		private float weight = 0.5f;

		// Token: 0x04000794 RID: 1940
		private string texPath;

		// Token: 0x04000795 RID: 1941
		private Color? color;

		// Token: 0x04000796 RID: 1942
		private Color? colorTwo;

		// Token: 0x04000797 RID: 1943
		private GraphicData graphicData;
	}
}
