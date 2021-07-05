using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020013D5 RID: 5077
	public struct EventPack
	{
		// Token: 0x1700159D RID: 5533
		// (get) Token: 0x06007B71 RID: 31601 RVA: 0x002B860E File Offset: 0x002B680E
		public string Tag
		{
			get
			{
				return this.tagInt;
			}
		}

		// Token: 0x1700159E RID: 5534
		// (get) Token: 0x06007B72 RID: 31602 RVA: 0x002B8616 File Offset: 0x002B6816
		public IntVec3 Cell
		{
			get
			{
				return this.cellInt;
			}
		}

		// Token: 0x1700159F RID: 5535
		// (get) Token: 0x06007B73 RID: 31603 RVA: 0x002B861E File Offset: 0x002B681E
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				return this.cellsInt;
			}
		}

		// Token: 0x06007B74 RID: 31604 RVA: 0x002B8626 File Offset: 0x002B6826
		public EventPack(string tag)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = null;
		}

		// Token: 0x06007B75 RID: 31605 RVA: 0x002B8641 File Offset: 0x002B6841
		public EventPack(string tag, IntVec3 cell)
		{
			this.tagInt = tag;
			this.cellInt = cell;
			this.cellsInt = null;
		}

		// Token: 0x06007B76 RID: 31606 RVA: 0x002B8658 File Offset: 0x002B6858
		public EventPack(string tag, IEnumerable<IntVec3> cells)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = cells;
		}

		// Token: 0x06007B77 RID: 31607 RVA: 0x002B8673 File Offset: 0x002B6873
		public static implicit operator EventPack(string s)
		{
			return new EventPack(s);
		}

		// Token: 0x06007B78 RID: 31608 RVA: 0x002B867C File Offset: 0x002B687C
		public override string ToString()
		{
			if (this.Cell.IsValid)
			{
				return this.Tag + "-" + this.Cell;
			}
			return this.Tag;
		}

		// Token: 0x04004448 RID: 17480
		private string tagInt;

		// Token: 0x04004449 RID: 17481
		private IntVec3 cellInt;

		// Token: 0x0400444A RID: 17482
		private IEnumerable<IntVec3> cellsInt;
	}
}
