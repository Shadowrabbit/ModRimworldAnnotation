using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000721 RID: 1825
	public class GridLayout
	{
		// Token: 0x06002E08 RID: 11784 RVA: 0x00136578 File Offset: 0x00134778
		public GridLayout(Rect container, int cols = 1, int rows = 1, float outerPadding = 4f, float innerPadding = 4f)
		{
			this.container = new Rect(container);
			this.cols = cols;
			this.innerPadding = innerPadding;
			this.outerPadding = outerPadding;
			float num = container.width - outerPadding * 2f - (float)(cols - 1) * innerPadding;
			float num2 = container.height - outerPadding * 2f - (float)(rows - 1) * innerPadding;
			this.colWidth = num / (float)cols;
			this.rowHeight = num2 / (float)rows;
			this.colStride = this.colWidth + innerPadding;
			this.rowStride = this.rowHeight + innerPadding;
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x00136614 File Offset: 0x00134814
		public GridLayout(float colWidth, float rowHeight, int cols, int rows, float outerPadding = 4f, float innerPadding = 4f)
		{
			this.colWidth = colWidth;
			this.rowHeight = rowHeight;
			this.cols = cols;
			this.innerPadding = innerPadding;
			this.outerPadding = outerPadding;
			this.colStride = colWidth + innerPadding;
			this.rowStride = rowHeight + innerPadding;
			this.container = new Rect(0f, 0f, outerPadding * 2f + colWidth * (float)cols + innerPadding * (float)cols - 1f, outerPadding * 2f + rowHeight * (float)rows + innerPadding * (float)rows - 1f);
		}

		// Token: 0x06002E0A RID: 11786 RVA: 0x001366AC File Offset: 0x001348AC
		public Rect GetCellRectByIndex(int index, int colspan = 1, int rowspan = 1)
		{
			int col = index % this.cols;
			int row = index / this.cols;
			return this.GetCellRect(col, row, colspan, rowspan);
		}

		// Token: 0x06002E0B RID: 11787 RVA: 0x001366D8 File Offset: 0x001348D8
		public Rect GetCellRect(int col, int row, int colspan = 1, int rowspan = 1)
		{
			return new Rect(Mathf.Floor(this.container.x + this.outerPadding + (float)col * this.colStride), Mathf.Floor(this.container.y + this.outerPadding + (float)row * this.rowStride), Mathf.Ceil(this.colWidth) * (float)colspan + this.innerPadding * (float)(colspan - 1), Mathf.Ceil(this.rowHeight) * (float)rowspan + this.innerPadding * (float)(rowspan - 1));
		}

		// Token: 0x04001F6E RID: 8046
		public Rect container;

		// Token: 0x04001F6F RID: 8047
		private int cols;

		// Token: 0x04001F70 RID: 8048
		private float outerPadding;

		// Token: 0x04001F71 RID: 8049
		private float innerPadding;

		// Token: 0x04001F72 RID: 8050
		private float colStride;

		// Token: 0x04001F73 RID: 8051
		private float rowStride;

		// Token: 0x04001F74 RID: 8052
		private float colWidth;

		// Token: 0x04001F75 RID: 8053
		private float rowHeight;
	}
}
