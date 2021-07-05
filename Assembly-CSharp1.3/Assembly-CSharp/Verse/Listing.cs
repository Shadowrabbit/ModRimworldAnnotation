using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200040F RID: 1039
	public abstract class Listing
	{
		// Token: 0x170005FE RID: 1534
		// (get) Token: 0x06001F23 RID: 7971 RVA: 0x000C252E File Offset: 0x000C072E
		public float CurHeight
		{
			get
			{
				return this.curY;
			}
		}

		// Token: 0x170005FF RID: 1535
		// (get) Token: 0x06001F24 RID: 7972 RVA: 0x000C2536 File Offset: 0x000C0736
		public float MaxColumnHeightSeen
		{
			get
			{
				return Math.Max(this.CurHeight, this.maxHeightColumnSeen);
			}
		}

		// Token: 0x17000600 RID: 1536
		// (get) Token: 0x06001F26 RID: 7974 RVA: 0x000C2559 File Offset: 0x000C0759
		// (set) Token: 0x06001F25 RID: 7973 RVA: 0x000C2549 File Offset: 0x000C0749
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

		// Token: 0x06001F27 RID: 7975 RVA: 0x000C2561 File Offset: 0x000C0761
		public void NewColumn()
		{
			this.maxHeightColumnSeen = Math.Max(this.curY, this.maxHeightColumnSeen);
			this.curY = 0f;
			this.curX += this.ColumnWidth + 17f;
		}

		// Token: 0x06001F28 RID: 7976 RVA: 0x000C259E File Offset: 0x000C079E
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

		// Token: 0x06001F29 RID: 7977 RVA: 0x000C25C4 File Offset: 0x000C07C4
		public Rect GetRect(float height)
		{
			this.NewColumnIfNeeded(height);
			Rect result = new Rect(this.curX, this.curY, this.ColumnWidth, height);
			this.curY += height;
			return result;
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x000C25F3 File Offset: 0x000C07F3
		public void Gap(float gapHeight = 12f)
		{
			this.curY += gapHeight;
		}

		// Token: 0x06001F2B RID: 7979 RVA: 0x000C2604 File Offset: 0x000C0804
		public void GapLine(float gapHeight = 12f)
		{
			float y = this.curY + gapHeight / 2f;
			Color color = GUI.color;
			GUI.color = color * new Color(1f, 1f, 1f, 0.4f);
			Widgets.DrawLineHorizontal(this.curX, y, this.ColumnWidth);
			GUI.color = color;
			this.curY += gapHeight;
		}

		// Token: 0x06001F2C RID: 7980 RVA: 0x000C266E File Offset: 0x000C086E
		public void Indent(float gapWidth = 12f)
		{
			this.curX += gapWidth;
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x000C267E File Offset: 0x000C087E
		public void Outdent(float gapWidth = 12f)
		{
			this.curX -= gapWidth;
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x000C2690 File Offset: 0x000C0890
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
					}));
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

		// Token: 0x06001F2F RID: 7983 RVA: 0x000C2753 File Offset: 0x000C0953
		public virtual void End()
		{
			GUI.EndGroup();
		}

		// Token: 0x040012F5 RID: 4853
		public float verticalSpacing = 2f;

		// Token: 0x040012F6 RID: 4854
		protected Rect listingRect;

		// Token: 0x040012F7 RID: 4855
		protected float curY;

		// Token: 0x040012F8 RID: 4856
		protected float curX;

		// Token: 0x040012F9 RID: 4857
		private float columnWidthInt;

		// Token: 0x040012FA RID: 4858
		private bool hasCustomColumnWidth;

		// Token: 0x040012FB RID: 4859
		private float maxHeightColumnSeen;

		// Token: 0x040012FC RID: 4860
		public bool maxOneColumn;

		// Token: 0x040012FD RID: 4861
		public const float ColumnSpacing = 17f;

		// Token: 0x040012FE RID: 4862
		public const float DefaultGap = 12f;

		// Token: 0x040012FF RID: 4863
		public const float DefaultIndent = 12f;
	}
}
