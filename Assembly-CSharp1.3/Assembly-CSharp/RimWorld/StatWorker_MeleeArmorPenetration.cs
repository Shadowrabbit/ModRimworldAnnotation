using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014F5 RID: 5365
	public class StatWorker_MeleeArmorPenetration : StatWorker
	{
		// Token: 0x06007FE2 RID: 32738 RVA: 0x002D44EB File Offset: 0x002D26EB
		public override bool IsDisabledFor(Thing thing)
		{
			return base.IsDisabledFor(thing) || StatDefOf.MeleeHitChance.Worker.IsDisabledFor(thing);
		}

		// Token: 0x06007FE3 RID: 32739 RVA: 0x002D4508 File Offset: 0x002D2708
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			if (req.Thing == null)
			{
				Log.Error("Getting MeleeArmorPenetration stat for " + req.Def + " without concrete pawn. This always returns 0.");
			}
			return this.GetArmorPenetration(req, applyPostProcess);
		}

		// Token: 0x06007FE4 RID: 32740 RVA: 0x002D4536 File Offset: 0x002D2736
		public override bool ShouldShowFor(StatRequest req)
		{
			return base.ShouldShowFor(req) && req.Thing is Pawn;
		}

		// Token: 0x06007FE5 RID: 32741 RVA: 0x002D4554 File Offset: 0x002D2754
		private float GetArmorPenetration(StatRequest req, bool applyPostProcess = true)
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
					num2 += updatedAvailableVerbsList[j].GetSelectionWeight(null) / num * updatedAvailableVerbsList[j].verb.verbProps.AdjustedArmorPenetration(updatedAvailableVerbsList[j].verb, pawn);
				}
			}
			return num2;
		}
	}
}
