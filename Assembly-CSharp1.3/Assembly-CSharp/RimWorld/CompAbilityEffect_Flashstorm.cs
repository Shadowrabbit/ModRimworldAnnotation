using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D36 RID: 3382
	public class CompAbilityEffect_Flashstorm : CompAbilityEffect
	{
		// Token: 0x06004F2F RID: 20271 RVA: 0x001A8CC4 File Offset: 0x001A6EC4
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Map map = this.parent.pawn.Map;
			Thing conditionCauser = GenSpawn.Spawn(ThingDefOf.Flashstorm, target.Cell, this.parent.pawn.Map, WipeMode.Vanish);
			GameCondition_Flashstorm gameCondition_Flashstorm = (GameCondition_Flashstorm)GameConditionMaker.MakeCondition(GameConditionDefOf.Flashstorm, -1);
			gameCondition_Flashstorm.centerLocation = target.Cell.ToIntVec2;
			gameCondition_Flashstorm.areaRadiusOverride = new IntRange(Mathf.RoundToInt(this.parent.def.EffectRadius), Mathf.RoundToInt(this.parent.def.EffectRadius));
			gameCondition_Flashstorm.Duration = Mathf.RoundToInt((float)this.parent.def.EffectDuration(this.parent.pawn).SecondsToTicks());
			gameCondition_Flashstorm.suppressEndMessage = true;
			gameCondition_Flashstorm.initialStrikeDelay = new IntRange(60, 180);
			gameCondition_Flashstorm.conditionCauser = conditionCauser;
			gameCondition_Flashstorm.ambientSound = true;
			map.gameConditionManager.RegisterCondition(gameCondition_Flashstorm);
			this.ApplyGoodwillImpact(target, gameCondition_Flashstorm.AreaRadius);
		}

		// Token: 0x06004F30 RID: 20272 RVA: 0x001A8DD4 File Offset: 0x001A6FD4
		private void ApplyGoodwillImpact(LocalTargetInfo target, int radius)
		{
			if (this.parent.pawn.Faction != Faction.OfPlayer)
			{
				return;
			}
			this.affectedFactionCache.Clear();
			foreach (Thing thing in GenRadial.RadialDistinctThingsAround(target.Cell, this.parent.pawn.Map, (float)radius, true))
			{
				Pawn p;
				if ((p = (thing as Pawn)) != null && thing.Faction != null && thing.Faction != this.parent.pawn.Faction && !thing.Faction.HostileTo(this.parent.pawn.Faction) && !this.affectedFactionCache.Contains(thing.Faction) && (base.Props.applyGoodwillImpactToLodgers || !p.IsQuestLodger()))
				{
					this.affectedFactionCache.Add(thing.Faction);
					Faction.OfPlayer.TryAffectGoodwillWith(thing.Faction, base.Props.goodwillImpact, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
			this.affectedFactionCache.Clear();
		}

		// Token: 0x06004F31 RID: 20273 RVA: 0x001A8F20 File Offset: 0x001A7120
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.Cell.Roofed(this.parent.pawn.Map))
			{
				if (throwMessages)
				{
					Messages.Message("AbilityRoofed".Translate(this.parent.def.LabelCap), target.ToTargetInfo(this.parent.pawn.Map), MessageTypeDefOf.RejectInput, false);
				}
				return false;
			}
			return true;
		}

		// Token: 0x04002F8C RID: 12172
		private HashSet<Faction> affectedFactionCache = new HashSet<Faction>();
	}
}
