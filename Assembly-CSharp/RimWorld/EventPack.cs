using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001BDE RID: 7134
	public struct EventPack
	{
		// Token: 0x170018A7 RID: 6311
		// (get) Token: 0x06009D0B RID: 40203 RVA: 0x000687E5 File Offset: 0x000669E5
		public string Tag
		{
			get
			{
				return this.tagInt;
			}
		}

		// Token: 0x170018A8 RID: 6312
		// (get) Token: 0x06009D0C RID: 40204 RVA: 0x000687ED File Offset: 0x000669ED
		public IntVec3 Cell
		{
			get
			{
				return this.cellInt;
			}
		}

		// Token: 0x170018A9 RID: 6313
		// (get) Token: 0x06009D0D RID: 40205 RVA: 0x000687F5 File Offset: 0x000669F5
		public IEnumerable<IntVec3> Cells
		{
			get
			{
				return this.cellsInt;
			}
		}

		// Token: 0x06009D0E RID: 40206 RVA: 0x000687FD File Offset: 0x000669FD
		public EventPack(string tag)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = null;
		}

		// Token: 0x06009D0F RID: 40207 RVA: 0x00068818 File Offset: 0x00066A18
		public EventPack(string tag, IntVec3 cell)
		{
			this.tagInt = tag;
			this.cellInt = cell;
			this.cellsInt = null;
		}

		// Token: 0x06009D10 RID: 40208 RVA: 0x0006882F File Offset: 0x00066A2F
		public EventPack(string tag, IEnumerable<IntVec3> cells)
		{
			this.tagInt = tag;
			this.cellInt = IntVec3.Invalid;
			this.cellsInt = cells;
		}

		// Token: 0x06009D11 RID: 40209 RVA: 0x0006884A File Offset: 0x00066A4A
		public static implicit operator EventPack(string s)
		{
			return new EventPack(s);
		}

		// Token: 0x06009D12 RID: 40210 RVA: 0x002DF228 File Offset: 0x002DD428
		public override string ToString()
		{
			if (this.Cell.IsValid)
			{
				return this.Tag + "-" + this.Cell;
			}
			return this.Tag;
		}

		// Token: 0x040063EB RID: 25579
		private string tagInt;

		// Token: 0x040063EC RID: 25580
		private IntVec3 cellInt;

		// Token: 0x040063ED RID: 25581
		private IEnumerable<IntVec3> cellsInt;
	}
}
