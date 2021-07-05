using System;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F2 RID: 5362
	public class StatWorker_SuppressionFallRate : StatWorker
	{
		// Token: 0x06007FD5 RID: 32725 RVA: 0x002D3ED0 File Offset: 0x002D20D0
		public override bool ShouldShowFor(StatRequest req)
		{
			Pawn pawn;
			return base.ShouldShowFor(req) && (pawn = (req.Thing as Pawn)) != null && pawn.IsSlave;
		}

		// Token: 0x06007FD6 RID: 32726 RVA: 0x002D3F00 File Offset: 0x002D2100
		private static float CurrentFallRateBasedOnSuppression(float suppression)
		{
			if (suppression > 0.3f)
			{
				return 0.2f;
			}
			if (suppression > 0.15f)
			{
				return 0.1f;
			}
			return 0.05f;
		}

		// Token: 0x06007FD7 RID: 32727 RVA: 0x002D3F23 File Offset: 0x002D2123
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			return StatWorker_SuppressionFallRate.CurrentFallRateBasedOnSuppression(((Pawn)req.Thing).needs.TryGetNeed<Need_Suppression>().CurLevelPercentage);
		}

		// Token: 0x06007FD8 RID: 32728 RVA: 0x002D3F48 File Offset: 0x002D2148
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			Need_Suppression need_Suppression = ((Pawn)req.Thing).needs.TryGetNeed<Need_Suppression>();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetExplanationUnfinalized(req, numberSense));
			stringBuilder.Append(string.Format("{0} ({1}): {2}", "CurrentSuppression".Translate(), need_Suppression.CurLevelPercentage.ToStringPercent(), StatWorker_SuppressionFallRate.CurrentFallRateBasedOnSuppression(need_Suppression.CurLevelPercentage).ToStringPercent()));
			return stringBuilder.ToString();
		}

		// Token: 0x06007FD9 RID: 32729 RVA: 0x002D3FC0 File Offset: 0x002D21C0
		public string GetExplanationForTooltip(StatRequest req)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("SuppressionFallRate".Translate() + ": " + base.GetValue(req.Thing, true).ToStringPercent());
			Need_Suppression need_Suppression = ((Pawn)req.Thing).needs.TryGetNeed<Need_Suppression>();
			stringBuilder.AppendLine(string.Format("   {0} ({1}): {2}", "CurrentSuppression".Translate(), need_Suppression.CurLevelPercentage.ToStringPercent(), StatWorker_SuppressionFallRate.CurrentFallRateBasedOnSuppression(need_Suppression.CurLevelPercentage).ToStringPercent()));
			if (this.stat.parts != null)
			{
				for (int i = 0; i < this.stat.parts.Count; i++)
				{
					string text = this.stat.parts[i].ExplanationPart(req);
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine("   " + text);
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x04004FA7 RID: 20391
		public const float FastFallRate = 0.2f;

		// Token: 0x04004FA8 RID: 20392
		public const float MediumFallRate = 0.1f;

		// Token: 0x04004FA9 RID: 20393
		public const float SlowFallRate = 0.05f;

		// Token: 0x04004FAA RID: 20394
		public const float FastFallRateThreshold = 0.3f;

		// Token: 0x04004FAB RID: 20395
		public const float MediumFallRateThreshold = 0.15f;
	}
}
