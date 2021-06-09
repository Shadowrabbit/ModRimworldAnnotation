using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D4B RID: 7499
	public class StatPart_Stuff : StatPart
	{
		// Token: 0x0600A2E5 RID: 41701 RVA: 0x002F6594 File Offset: 0x002F4794
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

		// Token: 0x0600A2E6 RID: 41702 RVA: 0x002F6694 File Offset: 0x002F4894
		public override void TransformValue(StatRequest req, ref float value)
		{
			float num = (req.StuffDef != null) ? req.StuffDef.GetStatValueAbstract(this.stuffPowerStat, null) : 0f;
			value += this.GetMultiplier(req) * num;
		}

		// Token: 0x0600A2E7 RID: 41703 RVA: 0x0006C2EB File Offset: 0x0006A4EB
		private float GetMultiplier(StatRequest req)
		{
			if (req.HasThing)
			{
				return req.Thing.GetStatValue(this.multiplierStat, true);
			}
			return req.BuildableDef.GetStatValueAbstract(this.multiplierStat, null);
		}

		// Token: 0x0600A2E8 RID: 41704 RVA: 0x0006C31D File Offset: 0x0006A51D
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest req)
		{
			if (req.StuffDef != null)
			{
				yield return new Dialog_InfoCard.Hyperlink(req.StuffDef, -1);
			}
			yield break;
		}

		// Token: 0x04006E9F RID: 28319
		public StatDef stuffPowerStat;

		// Token: 0x04006EA0 RID: 28320
		public StatDef multiplierStat;
	}
}
