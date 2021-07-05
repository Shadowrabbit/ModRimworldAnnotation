using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F9 RID: 5369
	public abstract class StatWorker_MeleeDamageAmount : StatWorker
	{
		// Token: 0x06007FFC RID: 32764 RVA: 0x002D5284 File Offset: 0x002D3484
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

		// Token: 0x06007FFD RID: 32765 RVA: 0x002D52E0 File Offset: 0x002D34E0
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

		// Token: 0x06007FFE RID: 32766
		protected abstract DamageArmorCategoryDef CategoryOfDamage(ThingDef def);
	}
}
