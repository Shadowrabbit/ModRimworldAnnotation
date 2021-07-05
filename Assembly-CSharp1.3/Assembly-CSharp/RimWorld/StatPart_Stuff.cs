using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020014E6 RID: 5350
	public class StatPart_Stuff : StatPart
	{
		// Token: 0x06007F75 RID: 32629 RVA: 0x002D0AF0 File Offset: 0x002CECF0
		public override string ExplanationPart(StatRequest req)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (req.BuildableDef.MadeFromStuff)
			{
				string t = (req.StuffDef != null) ? req.StuffDef.label : "None".TranslateSimple();
				string t2 = (req.StuffDef != null) ? req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null).ToStringByStyle(this.parentStat.ToStringStyleUnfinalized, ToStringNumberSense.Absolute) : "0";
				stringBuilder.AppendLine("StatsReport_Material".Translate() + " (" + t + "): " + t2);
				stringBuilder.AppendLine();
				stringBuilder.AppendLine("StatsReport_StuffEffectMultiplier".Translate() + ": " + this.GetMultiplier(req).ToStringPercent("F0"));
				stringBuilder.AppendLine();
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06007F76 RID: 32630 RVA: 0x002D0BF0 File Offset: 0x002CEDF0
		public override void TransformValue(StatRequest req, ref float value)
		{
			float num = (req.StuffDef != null) ? req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null) : 0f;
			value += this.GetMultiplier(req) * num;
		}

		// Token: 0x06007F77 RID: 32631 RVA: 0x002D0C2F File Offset: 0x002CEE2F
		private float GetMultiplier(StatRequest req)
		{
			if (req.HasThing)
			{
				return req.Thing.GetStatValue(this.multiplierStat, true);
			}
			return req.BuildableDef.GetStatValueAbstract(this.multiplierStat, null);
		}

		// Token: 0x06007F78 RID: 32632 RVA: 0x002D0C61 File Offset: 0x002CEE61
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			if (req.StuffDef != null)
			{
				yield return new Dialog_InfoCard.Hyperlink(req.StuffDef, -1);
			}
			yield break;
		}

		// Token: 0x04004F98 RID: 20376
		public StatDef stuffPowerStat;

		// Token: 0x04004F99 RID: 20377
		public StatDef multiplierStat;
	}
}
