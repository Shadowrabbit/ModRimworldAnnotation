using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D46 RID: 7494
	public class StatPart_ReloadMarketValue : StatPart
	{
		// Token: 0x0600A2D1 RID: 41681 RVA: 0x0006C205 File Offset: 0x0006A405
		public override void TransformValue(StatRequest req, ref float val)
		{
			StatPart_ReloadMarketValue.TransformAndExplain(req, ref val, null);
		}

		// Token: 0x0600A2D2 RID: 41682 RVA: 0x002F6130 File Offset: 0x002F4330
		public override string ExplanationPart(StatRequest req)
		{
			float num = 1f;
			StringBuilder stringBuilder = new StringBuilder();
			StatPart_ReloadMarketValue.TransformAndExplain(req, ref num, stringBuilder);
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x0600A2D3 RID: 41683 RVA: 0x002F6160 File Offset: 0x002F4360
		private static void TransformAndExplain(StatRequest req, ref float val, StringBuilder explanation)
		{
			Thing thing = req.Thing;
			CompReloadable compReloadable = (thing != null) ? thing.TryGetComp<CompReloadable>() : null;
			if (compReloadable == null)
			{
				return;
			}
			if (compReloadable.RemainingCharges == compReloadable.MaxCharges)
			{
				return;
			}
			if (compReloadable.AmmoDef != null)
			{
				int num = compReloadable.MaxAmmoNeeded(true);
				float num2 = -compReloadable.AmmoDef.BaseMarketValue * (float)num;
				val += num2;
				if (explanation != null)
				{
					explanation.AppendLine("StatsReport_ReloadMarketValue".Translate(compReloadable.AmmoDef.Named("AMMO"), num.Named("COUNT")) + ": " + num2.ToStringMoneyOffset(null));
				}
			}
			else if (compReloadable.Props.destroyOnEmpty)
			{
				float num3 = (float)compReloadable.RemainingCharges / (float)compReloadable.MaxCharges;
				if (explanation != null)
				{
					explanation.AppendLine("StatsReport_ReloadRemainingChargesMultipler".Translate(compReloadable.Props.ChargeNounArgument, compReloadable.LabelRemaining) + ": x" + num3.ToStringPercent());
				}
				val *= num3;
			}
			if (val < 0f)
			{
				val = 0f;
			}
		}
	}
}
