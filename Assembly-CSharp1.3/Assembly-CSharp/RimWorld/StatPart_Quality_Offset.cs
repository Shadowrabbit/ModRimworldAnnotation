using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DF RID: 5343
	public class StatPart_Quality_Offset : StatPart
	{
		// Token: 0x06007F59 RID: 32601 RVA: 0x002D040D File Offset: 0x002CE60D
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (this.ApplyTo(req))
			{
				val += this.QualityOffset(req.QualityCategory);
			}
		}

		// Token: 0x06007F5A RID: 32602 RVA: 0x002D042A File Offset: 0x002CE62A
		public override string ExplanationPart(StatRequest req)
		{
			if (!this.ApplyTo(req))
			{
				return null;
			}
			return "StatsReport_QualityOffset".Translate() + ": " + this.QualityOffset(req.QualityCategory);
		}

		// Token: 0x06007F5B RID: 32603 RVA: 0x002D0468 File Offset: 0x002CE668
		private bool ApplyTo(StatRequest req)
		{
			return this.thingDefs == null || (req.Def != null && this.thingDefs.Contains(req.Def)) || (req.Thing != null && this.thingDefs.Contains(req.Thing.def));
		}

		// Token: 0x06007F5C RID: 32604 RVA: 0x002D04C0 File Offset: 0x002CE6C0
		private float QualityOffset(QualityCategory qc)
		{
			switch (qc)
			{
			case QualityCategory.Awful:
				return this.offsetAwful;
			case QualityCategory.Poor:
				return this.offsetPoor;
			case QualityCategory.Normal:
				return this.offsetNormal;
			case QualityCategory.Good:
				return this.offsetGood;
			case QualityCategory.Excellent:
				return this.offsetExcellent;
			case QualityCategory.Masterwork:
				return this.offsetMasterwork;
			case QualityCategory.Legendary:
				return this.offsetLegendary;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x04004F87 RID: 20359
		public float offsetAwful;

		// Token: 0x04004F88 RID: 20360
		public float offsetPoor;

		// Token: 0x04004F89 RID: 20361
		public float offsetNormal;

		// Token: 0x04004F8A RID: 20362
		public float offsetGood;

		// Token: 0x04004F8B RID: 20363
		public float offsetExcellent;

		// Token: 0x04004F8C RID: 20364
		public float offsetMasterwork;

		// Token: 0x04004F8D RID: 20365
		public float offsetLegendary;

		// Token: 0x04004F8E RID: 20366
		public List<ThingDef> thingDefs;
	}
}
