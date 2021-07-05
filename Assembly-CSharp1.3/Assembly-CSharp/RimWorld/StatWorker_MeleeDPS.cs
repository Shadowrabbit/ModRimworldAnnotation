using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F8 RID: 5368
	public class StatWorker_MeleeDPS : StatWorker
	{
		// Token: 0x06007FF2 RID: 32754 RVA: 0x002D44EB File Offset: 0x002D26EB
		public override bool IsDisabledFor(Thing thing)
		{
			return base.IsDisabledFor(thing) || StatDefOf.MeleeHitChance.Worker.IsDisabledFor(thing);
		}

		// Token: 0x06007FF3 RID: 32755 RVA: 0x002D4E13 File Offset: 0x002D3013
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (req.Thing == null)
			{
				Log.Error("Getting MeleeDPS stat for " + req.Def + " without concrete pawn. This always returns 0.");
			}
			return this.GetMeleeDamage(req, applyPostProcess) * this.GetMeleeHitChance(req, applyPostProcess) / this.GetMeleeCooldown(req, applyPostProcess);
		}

		// Token: 0x06007FF4 RID: 32756 RVA: 0x002D4E54 File Offset: 0x002D3054
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

		// Token: 0x06007FF5 RID: 32757 RVA: 0x002D4FCC File Offset: 0x002D31CC
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

		// Token: 0x06007FF6 RID: 32758 RVA: 0x002D5044 File Offset: 0x002D3244
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

		// Token: 0x06007FF7 RID: 32759 RVA: 0x002D513F File Offset: 0x002D333F
		private float GetMeleeHitChance(StatRequest req, bool applyPostProcess = true)
		{
			if (req.HasThing)
			{
				return req.Thing.GetStatValue(StatDefOf.MeleeHitChance, applyPostProcess);
			}
			return req.BuildableDef.GetStatValueAbstract(StatDefOf.MeleeHitChance, null);
		}

		// Token: 0x06007FF8 RID: 32760 RVA: 0x002D5170 File Offset: 0x002D3370
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

		// Token: 0x06007FF9 RID: 32761 RVA: 0x002D4536 File Offset: 0x002D2736
		public override bool ShouldShowFor(StatRequest req)
		{
			return base.ShouldShowFor(req) && req.Thing is Pawn;
		}

		// Token: 0x06007FFA RID: 32762 RVA: 0x002D5272 File Offset: 0x002D3472
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
