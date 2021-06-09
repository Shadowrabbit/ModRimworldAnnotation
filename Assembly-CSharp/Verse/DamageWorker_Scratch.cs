﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Verse
{
	// Token: 0x02000399 RID: 921
	public class DamageWorker_Scratch : DamageWorker_AddInjury
	{
		// Token: 0x060016FD RID: 5885 RVA: 0x000162AA File Offset: 0x000144AA
		protected override BodyPartRecord ChooseHitPart(DamageInfo dinfo, Pawn pawn)
		{
			return pawn.health.hediffSet.GetRandomNotMissingPart(dinfo.Def, dinfo.Height, BodyPartDepth.Outside, null);
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x000DA3C4 File Offset: 0x000D85C4
		protected override void ApplySpecialEffectsToPart(Pawn pawn, float totalDamage, DamageInfo dinfo, DamageWorker.DamageResult result)
		{
			if (dinfo.HitPart.depth == BodyPartDepth.Inside)
			{
				List<BodyPartRecord> list = new List<BodyPartRecord>();
				for (BodyPartRecord bodyPartRecord = dinfo.HitPart; bodyPartRecord != null; bodyPartRecord = bodyPartRecord.parent)
				{
					list.Add(bodyPartRecord);
					if (bodyPartRecord.depth == BodyPartDepth.Outside)
					{
						break;
					}
				}
				float num = (float)list.Count;
				for (int i = 0; i < list.Count; i++)
				{
					DamageInfo dinfo2 = dinfo;
					dinfo2.SetHitPart(list[i]);
					base.FinalizeAndAddInjury(pawn, totalDamage / num, dinfo2, result);
				}
				return;
			}
			IEnumerable<BodyPartRecord> enumerable = dinfo.HitPart.GetDirectChildParts();
			if (dinfo.HitPart.parent != null)
			{
				enumerable = enumerable.Concat(dinfo.HitPart.parent);
				if (dinfo.HitPart.parent.parent != null)
				{
					enumerable = enumerable.Concat(dinfo.HitPart.parent.GetDirectChildParts());
				}
			}
			enumerable = from target in enumerable
			where target != dinfo.HitPart && !target.def.conceptual && target.depth == BodyPartDepth.Outside && !pawn.health.hediffSet.PartIsMissing(target)
			select target;
			BodyPartRecord bodyPartRecord2 = enumerable.RandomElementWithFallback(null);
			if (bodyPartRecord2 == null)
			{
				base.FinalizeAndAddInjury(pawn, base.ReduceDamageToPreserveOutsideParts(totalDamage, dinfo, pawn), dinfo, result);
				return;
			}
			base.FinalizeAndAddInjury(pawn, base.ReduceDamageToPreserveOutsideParts(totalDamage * this.def.scratchSplitPercentage, dinfo, pawn), dinfo, result);
			DamageInfo dinfo3 = dinfo;
			dinfo3.SetHitPart(bodyPartRecord2);
			base.FinalizeAndAddInjury(pawn, base.ReduceDamageToPreserveOutsideParts(totalDamage * this.def.scratchSplitPercentage, dinfo3, pawn), dinfo3, result);
		}
	}
}
