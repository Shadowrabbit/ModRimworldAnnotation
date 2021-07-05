using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000D65 RID: 3429
	public class CompAbilityEffect_StartAnimaLinking : CompAbilityEffect_StartRitual
	{
		// Token: 0x17000DC8 RID: 3528
		// (get) Token: 0x06004FAD RID: 20397 RVA: 0x001AAB53 File Offset: 0x001A8D53
		public override bool ShouldHideGizmo
		{
			get
			{
				return this.parent.pawn.GetPsylinkLevel() >= this.parent.pawn.GetMaxPsylinkLevel() || !MeditationFocusDefOf.Natural.CanPawnUse(this.parent.pawn);
			}
		}

		// Token: 0x06004FAE RID: 20398 RVA: 0x001AAB94 File Offset: 0x001A8D94
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = this.parent.pawn;
			Thing thing = target.Thing;
			CompPsylinkable compPsylinkable = (thing != null) ? thing.TryGetComp<CompPsylinkable>() : null;
			if (compPsylinkable == null)
			{
				return false;
			}
			int requiredPlantCount = compPsylinkable.GetRequiredPlantCount(pawn);
			if (requiredPlantCount == -1)
			{
				return false;
			}
			if (!pawn.CanReserve(target.Thing, 1, -1, null, false))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(target.Thing, pawn);
				if (throwMessages)
				{
					Messages.Message((pawn2 == null) ? "Reserved".Translate() : "ReservedBy".Translate(pawn.LabelShort, pawn2), target.Thing, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			CompSpawnSubplant compSpawnSubplant = target.Thing.TryGetComp<CompSpawnSubplant>();
			if (compSpawnSubplant.SubplantsForReading.Count < requiredPlantCount)
			{
				if (throwMessages)
				{
					Messages.Message("BeginLinkingRitualNeedSubplants".Translate(requiredPlantCount.ToString(), compSpawnSubplant.Props.subplant.label, compSpawnSubplant.SubplantsForReading.Count.ToString()), target.Thing, MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			LocalTargetInfo localTargetInfo;
			if (!compPsylinkable.TryFindLinkSpot(pawn, out localTargetInfo) && throwMessages)
			{
				Messages.Message("BeginLinkingRitualNeedLinkSpot".Translate(), target.Thing, MessageTypeDefOf.RejectInput, false);
			}
			return base.Valid(target, throwMessages);
		}
	}
}
