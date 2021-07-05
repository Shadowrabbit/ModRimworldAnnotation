using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E89 RID: 3721
	public static class InteractionUtility
	{
		// Token: 0x0600572C RID: 22316 RVA: 0x001D9A5C File Offset: 0x001D7C5C
		public static bool CanInitiateInteraction(Pawn pawn, InteractionDef interactionDef = null)
		{
			return pawn.interactions != null && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Talking) && pawn.Awake() && !pawn.IsBurning() && !pawn.IsInteractionBlocked(interactionDef, true, false);
		}

		// Token: 0x0600572D RID: 22317 RVA: 0x001D9AAE File Offset: 0x001D7CAE
		public static bool CanReceiveInteraction(Pawn pawn, InteractionDef interactionDef = null)
		{
			return pawn.Awake() && !pawn.IsBurning() && !pawn.IsInteractionBlocked(interactionDef, false, false);
		}

		// Token: 0x0600572E RID: 22318 RVA: 0x001D9AD4 File Offset: 0x001D7CD4
		public static bool CanInitiateRandomInteraction(Pawn p)
		{
			return InteractionUtility.CanInitiateInteraction(p, null) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState && !p.IsInteractionBlocked(null, true, true) && p.Faction != null;
		}

		// Token: 0x0600572F RID: 22319 RVA: 0x001D9B21 File Offset: 0x001D7D21
		public static bool CanReceiveRandomInteraction(Pawn p)
		{
			return InteractionUtility.CanReceiveInteraction(p, null) && p.RaceProps.Humanlike && !p.Downed && !p.InAggroMentalState;
		}

		// Token: 0x06005730 RID: 22320 RVA: 0x001D9B4E File Offset: 0x001D7D4E
		public static bool IsGoodPositionForInteraction(Pawn p, Pawn recipient)
		{
			return InteractionUtility.IsGoodPositionForInteraction(p.Position, recipient.Position, p.Map);
		}

		// Token: 0x06005731 RID: 22321 RVA: 0x001D9B67 File Offset: 0x001D7D67
		public static bool IsGoodPositionForInteraction(IntVec3 cell, IntVec3 recipientCell, Map map)
		{
			return cell.InHorDistOf(recipientCell, 6f) && GenSight.LineOfSight(cell, recipientCell, map, true, null, 0, 0);
		}

		// Token: 0x06005732 RID: 22322 RVA: 0x001D9B88 File Offset: 0x001D7D88
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

		// Token: 0x06005733 RID: 22323 RVA: 0x001D9BDC File Offset: 0x001D7DDC
		public static bool TryGetRandomVerbForSocialFight(Pawn p, out Verb verb)
		{
			if (p.Dead)
			{
				verb = null;
				return false;
			}
			return (from x in p.verbTracker.AllVerbs
			where x.IsMeleeAttack && x.IsStillUsableBy(p)
			select x).TryRandomElementByWeight((Verb x) => x.verbProps.AdjustedMeleeDamageAmount(x, p) * ((x.tool != null) ? x.tool.chanceFactor : 1f), out verb);
		}

		// Token: 0x06005734 RID: 22324 RVA: 0x001D9C3C File Offset: 0x001D7E3C
		public static void ImitateSocialInteractionWithManyPawns(Pawn initiator, List<Pawn> targets, InteractionDef intDef)
		{
			List<Pawn> list = targets.Except(initiator).ToList<Pawn>();
			if (targets.NullOrEmpty<Pawn>())
			{
				Log.Error(string.Concat(new object[]
				{
					initiator,
					" tried to do interaction ",
					intDef,
					" with no targets. "
				}));
				return;
			}
			if (intDef.initiatorXpGainSkill != null)
			{
				initiator.skills.Learn(intDef.initiatorXpGainSkill, (float)intDef.initiatorXpGainAmount, false);
			}
			foreach (Pawn pawn in list)
			{
				if (initiator != pawn && initiator.interactions.CanInteractNowWith(pawn, intDef))
				{
					if (intDef.recipientThought != null && pawn.needs.mood != null)
					{
						Pawn_InteractionsTracker.AddInteractionThought(pawn, initiator, intDef.recipientThought);
					}
					if (intDef.recipientXpGainSkill != null && pawn.RaceProps.Humanlike)
					{
						pawn.skills.Learn(intDef.recipientXpGainSkill, (float)intDef.recipientXpGainAmount, false);
					}
				}
			}
			MoteMaker.MakeInteractionBubble(initiator, list.RandomElement<Pawn>(), intDef.interactionMote, intDef.GetSymbol(initiator.Faction, initiator.Ideo), intDef.GetSymbolColor(initiator.Faction));
			PlayLogEntry_InteractionWithMany entry = new PlayLogEntry_InteractionWithMany(intDef, initiator, list, null);
			Find.PlayLog.Add(entry);
		}

		// Token: 0x04003399 RID: 13209
		public const float MaxInteractRange = 6f;
	}
}
