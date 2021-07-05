using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D64 RID: 7524
	public abstract class StatWorker_MeleeDamageAmount : StatWorker
	{
		// Token: 0x0600A394 RID: 41876 RVA: 0x002FA228 File Offset: 0x002F8428
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			float num = base.GetValueUnfinalized(req, true);
			ThingDef def = (ThingDef)req.Def;
			if (req.StuffDef != null)
			{
				StatDef statDef = null;
				if (this.CategoryOfDamage(def) != null)
				{
					statDef = this.CategoryOfDamage(def).multStat;
				}
				if (statDef != null)
				{
					num *= req.StuffDef.GetStatValueAbstract(statDef, null);
				}
			}
			return num;
		}

		// Token: 0x0600A395 RID: 41877 RVA: 0x002FA284 File Offset: 0x002F8484
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetExplanationUnfinalized(req, numberSense));
			ThingDef def = (ThingDef)req.Def;
			if (req.StuffDef != null)
			{
				StatDef statDef = null;
				if (this.CategoryOfDamage(def) != null)
				{
					statDef = this.CategoryOfDamage(def).multStat;
				}
				if (statDef != null)
				{
					stringBuilder.AppendLine(req.StuffDef.LabelCap + ": x" + req.StuffDef.GetStatValueAbstract(statDef, null).ToStringPercent());
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600A396 RID: 41878
		protected abstract DamageArmorCategoryDef CategoryOfDamage(ThingDef def);
	}
}
