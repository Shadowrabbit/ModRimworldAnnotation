using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001548 RID: 5448
	public static class InteractionUtility
	{
		// Token: 0x060075FE RID: 30206 RVA: 0x0023ECAC File Offset: 0x0023CEAC
		public static bool CanInitiateInteraction(Pawn pawn, InteractionDef interactionDef = null)
		{
			return pawn.interactions != null && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.Awake() && !pawn.IsBurning() && !pawn.IsInteractionBlocked(interactionDef, true, false);
		}

		// Token: 0x060075FF RID: 30207 RVA: 0x0004F98E File Offset: 0x0004DB8E
		public static bool CanReceiveInteraction(Pawn pawn, InteractionDef interactionDef = null)
		{
			return pawn.Awake() && !pawn.IsBurning() && !pawn.IsInteractionBlocked(interactionDef, false, false);
		}

		// Token: 0x06007600 RID: 30208 RVA: 0x0023ED00 File Offset: 0x0023CF00
		public static bool CanInitiateRandomInteraction(Pawn p)
		{
			return InteractionUtility.CanInitiateInteraction(p, null) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState && !p.IsInteractionBlocked(null, true, true) && p.Faction != null;
		}

		// Token: 0x06007601 RID: 30209 RVA: 0x0004F9B2 File Offset: 0x0004DBB2
		public static bool CanReceiveRandomInteraction(Pawn p)
		{
			return InteractionUtility.CanReceiveInteraction(p, null) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState;
		}

		// Token: 0x06007602 RID: 30210 RVA: 0x0004F9DF File Offset: 0x0004DBDF
		public static bool IsGoodPositionForInteraction(Pawn p, Pawn recipient)
		{
			return InteractionUtility.IsGoodPositionForInteraction(p.Position, recipient.Position, p.Map);
		}

		// Token: 0x06007603 RID: 30211 RVA: 0x0004F9F8 File Offset: 0x0004DBF8
		public static bool IsGoodPositionForInteraction(IntVec3 cell, IntVec3 recipientCell, Map map)
		{
			return cell.InHorDistOf(recipientCell, 6f) && GenSight.LineOfSight(cell, recipientCell, map, true, null, 0, 0);
		}

		// Token: 0x06007604 RID: 30212 RVA: 0x0023ED50 File Offset: 0x0023CF50
		public static bool HasAnyVerbForSocialFight(Pawn p)
		{
			if (p.Dead)
			{
				return false;
			}
			List<Verb> allVerbs = p.verbTracker.AllVerbs;
			for (int i = 0; i < allVerbs.Count; i++)
			{
				if (allVerbs[i].IsMeleeAttack && allVerbs[i].IsStillUsableBy(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007605 RID: 30213 RVA: 0x0023EDA4 File Offset: 0x0023CFA4
		public static bool TryGetRandomVerbForSocialFight(Pawn p, out Verb verb)
		{
			if (p.Dead)
			{
				verb = null;
				return false;
			}
			return (from x in p.verbTracker.AllVerbs
			where x.IsMeleeAttack && x.IsStillUsableBy(p)
			select x).TryRandomElementByWeight((Verb x) => x.verbProps.AdjustedMeleeDamageAmount(x, p), out verb);
		}

		// Token: 0x04004DF6 RID: 19958
		public const float MaxInteractRange = 6f;
	}
}
