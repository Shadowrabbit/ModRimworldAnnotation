using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001D66 RID: 7526
	public class StatWorker_MeleeDPS : StatWorker
	{
		// Token: 0x0600A39B RID: 41883 RVA: 0x0006C774 File Offset: 0x0006A974
		public override bool IsDisabledFor(Thing thing)
		{
			return base.IsDisabledFor(thing) || StatDefOf.MeleeHitChance.Worker.IsDisabledFor(thing);
		}

		// Token: 0x0600A39C RID: 41884 RVA: 0x002FA34C File Offset: 0x002F854C
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (req.Thing == null)
			{
				Log.Error("Getting MeleeDPS stat for " + req.Def + " without concrete pawn. This always returns 0.", false);
			}
			return this.GetMeleeDamage(req, applyPostProcess) * this.GetMeleeHitChance(req, applyPostProcess) / this.GetMeleeCooldown(req, applyPostProcess);
		}

		// Token: 0x0600A39D RID: 41885 RVA: 0x002FA398 File Offset: 0x002F8598
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("StatsReport_MeleeDPSExplanation".Translate());
			stringBuilder.AppendLine("StatsReport_MeleeDamage".Translate() + " (" + "AverageOfAllAttacks".Translate() + ")");
			stringBuilder.AppendLine("  " + this.GetMeleeDamage(req, true).ToString("0.##"));
			stringBuilder.AppendLine("StatsReport_Cooldown".Translate() + " (" + "AverageOfAllAttacks".Translate() + ")");
			stringBuilder.AppendLine("  " + "StatsReport_CooldownFormat".Translate(this.GetMeleeCooldown(req, true).ToString("0.##")));
			stringBuilder.AppendLine("StatsReport_MeleeHitChance".Translate());
			stringBuilder.AppendLine(StatDefOf.MeleeHitChance.Worker.GetExplanationUnfinalized(req, StatDefOf.MeleeHitChance.toStringNumberSense).TrimEndNewlines().Indented("    "));
			stringBuilder.Append(StatDefOf.MeleeHitChance.Worker.GetExplanationFinalizePart(req, StatDefOf.MeleeHitChance.toStringNumberSense, this.GetMeleeHitChance(req, true)).Indented("    "));
			return stringBuilder.ToString();
		}

		// Token: 0x0600A39E RID: 41886 RVA: 0x002FA510 File Offset: 0x002F8710
		public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
		{
			return string.Format("{0} ( {1} x {2} / {3} )", new object[]
			{
				value.ToStringByStyle(stat.toStringStyle, numberSense),
				this.GetMeleeDamage(optionalReq, true).ToString("0.##"),
				StatDefOf.MeleeHitChance.ValueToString(this.GetMeleeHitChance(optionalReq, true), ToStringNumberSense.Absolute, true),
				this.GetMeleeCooldown(optionalReq, true).ToString("0.##")
			});
		}

		// Token: 0x0600A39F RID: 41887 RVA: 0x002FA588 File Offset: 0x002F8788
		private float GetMeleeDamage(StatRequest req, bool applyPostProcess = true)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return 0f;
			}
			List<VerbEntry> updatedAvailableVerbsList = pawn.meleeVerbs.GetUpdatedAvailableVerbsList(false);
			if (updatedAvailableVerbsList.Count == 0)
			{
				return 0f;
			}
			float num = 0f;
			for (int i = 0; i < updatedAvailableVerbsList.Count; i++)
			{
				if (updatedAvailableVerbsList[i].IsMeleeAttack)
				{
					num += updatedAvailableVerbsList[i].GetSelectionWeight(null);
				}
			}
			if (num == 0f)
			{
				return 0f;
			}
			float num2 = 0f;
			for (int j = 0; j < updatedAvailableVerbsList.Count; j++)
			{
				if (updatedAvailableVerbsList[j].IsMeleeAttack)
				{
					num2 += updatedAvailableVerbsList[j].GetSelectionWeight(null) / num * updatedAvailableVerbsList[j].verb.verbProps.AdjustedMeleeDamageAmount(updatedAvailableVerbsList[j].verb, pawn);
				}
			}
			return num2;
		}

		// Token: 0x0600A3A0 RID: 41888 RVA: 0x0006C9A8 File Offset: 0x0006ABA8
		private float GetMeleeHitChance(StatRequest req, bool applyPostProcess = true)
		{
			if (req.HasThing)
			{
				return req.Thing.GetStatValue(StatDefOf.MeleeHitChance, applyPostProcess);
			}
			return req.BuildableDef.GetStatValueAbstract(StatDefOf.MeleeHitChance, null);
		}

		// Token: 0x0600A3A1 RID: 41889 RVA: 0x002FA684 File Offset: 0x002F8884
		private float GetMeleeCooldown(StatRequest req, bool applyPostProcess = true)
		{
			Pawn pawn = req.Thing as Pawn;
			if (pawn == null)
			{
				return 1f;
			}
			List<VerbEntry> updatedAvailableVerbsList = pawn.meleeVerbs.GetUpdatedAvailableVerbsList(false);
			if (updatedAvailableVerbsList.Count == 0)
			{
				return 1f;
			}
			float num = 0f;
			for (int i = 0; i < updatedAvailableVerbsList.Count; i++)
			{
				if (updatedAvailableVerbsList[i].IsMeleeAttack)
				{
					num += updatedAvailableVerbsList[i].GetSelectionWeight(null);
				}
			}
			if (num == 0f)
			{
				return 1f;
			}
			float num2 = 0f;
			for (int j = 0; j < updatedAvailableVerbsList.Count; j++)
			{
				if (updatedAvailableVerbsList[j].IsMeleeAttack)
				{
					num2 += updatedAvailableVerbsList[j].GetSelectionWeight(null) / num * (float)updatedAvailableVerbsList[j].verb.verbProps.AdjustedCooldownTicks(updatedAvailableVerbsList[j].verb, pawn);
				}
			}
			return num2 / 60f;
		}

		// Token: 0x0600A3A2 RID: 41890 RVA: 0x0006C7C0 File Offset: 0x0006A9C0
		public override bool ShouldShowFor(StatRequest req)
		{
			return base.ShouldShowFor(req) && req.Thing is Pawn;
		}

		// Token: 0x0600A3A3 RID: 41891 RVA: 0x0006C9D8 File Offset: 0x0006ABD8
		public override IEnumerable<Dialog_InfoCard.Hyperlink> GetInfoCardHyperlinks(StatRequest statRequest)
		{
			Pawn pawn = statRequest.Thing as Pawn;
			if (pawn != null && pawn.equipment != null && pawn.equipment.Primary != null)
			{
				yield return new Dialog_InfoCard.Hyperlink(pawn.equipment.Primary, -1);
			}
			yield break;
		}
	}
}
