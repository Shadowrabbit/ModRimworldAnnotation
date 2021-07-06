using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000741 RID: 1857
	public abstract class Listing
	{
		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x06002EAF RID: 11951 RVA: 0x0002492C File Offset: 0x00022B2C
		public float CurHeight
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x06002EB0 RID: 11952 RVA: 0x00024934 File Offset: 0x00022B34
		public float MaxColumnHeightSeen
		{
			get
			{
				return Math.Max(this.CurHeight, this.maxHeightColumnSeen);
			}
		}

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x06002EB2 RID: 11954 RVA: 0x00024957 File Offset: 0x00022B57
		// (set) Token: 0x06002EB1 RID: 11953 RVA: 0x00024947 File Offset: 0x00022B47
		public float ColumnWidth
		{
			get
			{
				return this.columnWidthInt;
			}
			set
			{
				this.columnWidthInt = value;
				this.hasCustomColumnWidth = true;
			}
		}

		// Token: 0x06002EB3 RID: 11955 RVA: 0x0002495F File Offset: 0x00022B5F
		public void NewColumn()
		{
			this.maxHeightColumnSeen = Math.Max(this.curY, this.maxHeightColumnSeen);
			this.curY = 0f;
			this.curX += this.ColumnWidth + 17f;
		}

		// Token: 0x06002EB4 RID: 11956 RVA: 0x0002499C File Offset: 0x00022B9C
		protected void NewColumnIfNeeded(float neededHeight)
		{
			if (this.maxOneColumn)
			{
				return;
			}
			if (this.curY + neededHeight > this.listingRect.height)
			{
				this.NewColumn();
			}
		}

		// Token: 0x06002EB5 RID: 11957 RVA: 0x000249C2 File Offset: 0x00022BC2
		public Rect GetRect(float height)
		{
			this.NewColumnIfNeeded(height);
			Rect result = new Rect(this.curX, this.curY, this.ColumnWidth, height);
			this.curY += height;
			return result;
		}

		// Token: 0x06002EB6 RID: 11958 RVA: 0x000249F1 File Offset: 0x00022BF1
		public void Gap(float gapHeight = 12f)
		{
			this.curY += gapHeight;
		}

		// Token: 0x06002EB7 RID: 11959 RVA: 0x00138ACC File Offset: 0x00136CCC
		public void GapLine(float gapHeight = 12f)
		{
			float y = this.curY + gapHeight / 2f;
			Color color = GUI.color;
			GUI.color = color * new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(this.curX, y, this.ColumnWidth);
			GUI.color = color;
			this.curY += gapHeight;
		}

		// Token: 0x06002EB8 RID: 11960 RVA: 0x00024A01 File Offset: 0x00022C01
		public void Indent(float gapWidth = 12f)
		{
			this.curX += gapWidth;
		}

		// Token: 0x06002EB9 RID: 11961 RVA: 0x00024A11 File Offset: 0x00022C11
		public void Outdent(float gapWidth = 12f)
		{
			this.curX -= gapWidth;
		}

		// Token: 0x06002EBA RID: 11962 RVA: 0x00138B38 File Offset: 0x00136D38
		public virtual void Begin(Rect rect)
		{
			this.listingRect = rect;
			if (this.hasCustomColumnWidth)
			{
				if (this.columnWidthInt > this.listingRect.width)
				{
					Log.Error(string.Concat(new object[]
					{
						"Listing set ColumnWith to ",
						this.columnWidthInt,
						" which is more than the whole listing rect width of ",
						this.listingRect.width,
						". Clamping."
					}), false);
					this.columnWidthInt = this.listingRect.width;
				}
			}
			else
			{
				this.columnWidthInt = this.listingRect.width;
			}
			this.curX = 0f;
			this.curY = 0f;
			this.maxHeightColumnSeen = 0f;
			GUI.BeginGroup(rect);
		}

		// Token: 0x06002EBB RID: 11963 RVA: 0x00024A21 File Offset: 0x00022C21
		public virtual void End()
		{
			GUI.EndGroup();
		}

		// Token: 0x04001FC6 RID: 8134
		public float verticalSpacing = 2f;

		// Token: 0x04001FC7 RID: 8135
		protected Rect listingRect;

		// Token: 0x04001FC8 RID: 8136
		protected float curY;

		// Token: 0x04001FC9 RID: 8137
		protected float curX;

		// Token: 0x04001FCA RID: 8138
		private float columnWidthInt;

		// Token: 0x04001FCB RID: 8139
		private bool hasCustomColumnWidth;

		// Token: 0x04001FCC RID: 8140
		private float maxHeightColumnSeen;

		// Token: 0x04001FCD RID: 8141
		public bool maxOneColumn;

		// Token: 0x04001FCE RID: 8142
		public const float ColumnSpacing = 17f;

		// Token: 0x04001FCF RID: 8143
		public const float DefaultGap = 12f;

		// Token: 0x04001FD0 RID: 8144
		public const float DefaultIndent = 12f;
	}
}
