using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000402 RID: 1026
	public class GridLayout
	{
		// Token: 0x06001EB9 RID: 7865 RVA: 0x000C0220 File Offset: 0x000BE420
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

		// Token: 0x06001EBA RID: 7866 RVA: 0x000C02BC File Offset: 0x000BE4BC
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

		// Token: 0x06001EBB RID: 7867 RVA: 0x000C0354 File Offset: 0x000BE554
		public Rect GetCellRectByIndex(int index, int colspan = 1, int rowspan = 1)
		{
			int col = index % this.cols;
			int row = index / this.cols;
			return this.GetCellRect(col, row, colspan, rowspan);
		}

		// Token: 0x06001EBC RID: 7868 RVA: 0x000C0380 File Offset: 0x000BE580
		public Rect GetCellRect(int col, int row, int colspan = 1, int rowspan = 1)
		{
			return new Rect(Mathf.Floor(this.container.x + this.outerPadding + (float)col * this.colStride), Mathf.Floor(this.container.y + this.outerPadding + (float)row * this.rowStride), Mathf.Ceil(this.colWidth) * (float)colspan + this.innerPadding * (float)(colspan - 1), Mathf.Ceil(this.rowHeight) * (float)rowspan + this.innerPadding * (float)(rowspan - 1));
		}

		// Token: 0x040012C9 RID: 4809
		public Rect container;

		// Token: 0x040012CA RID: 4810
		private int cols;

		// Token: 0x040012CB RID: 4811
		private float outerPadding;

		// Token: 0x040012CC RID: 4812
		private float innerPadding;

		// Token: 0x040012CD RID: 4813
		private float colStride;

		// Token: 0x040012CE RID: 4814
		private float rowStride;

		// Token: 0x040012CF RID: 4815
		private float colWidth;

		// Token: 0x040012D0 RID: 4816
		private float rowHeight;
	}
}
