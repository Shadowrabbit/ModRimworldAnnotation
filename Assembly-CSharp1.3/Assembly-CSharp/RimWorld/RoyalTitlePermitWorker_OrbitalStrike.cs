using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000DEC RID: 3564
	public class RoyalTitlePermitWorker_OrbitalStrike : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x06005292 RID: 21138 RVA: 0x001BDBC4 File Offset: 0x001BBDC4
		public override bool ValidateTarget(LocalTargetInfo target, bool showMessages = true)
		{
			if (!base.CanHitTarget(target))
			{
				if (target.IsValid && showMessages)
				{
					Messages.Message(this.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06005293 RID: 21139 RVA: 0x001BDC1C File Offset: 0x001BBE1C
		public override void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, this.def.royalAid.targetingRange, Color.white, null);
			GenDraw.DrawRadiusRing(target.Cell, this.def.royalAid.radius + this.def.royalAid.explosionRadiusRange.max, Color.white, null);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x06005294 RID: 21140 RVA: 0x001BDC96 File Offset: 0x001BBE96
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallBombardment(target.Cell);
		}

		// Token: 0x06005295 RID: 21141 RVA: 0x001BDCA5 File Offset: 0x001BBEA5
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + "CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION")), null, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
				yield break;
			}
			string label = this.def.LabelCap + ": ";
			Action action = null;
			bool free;
			if (base.FillAidOption(pawn, faction, ref label, out free))
			{
				action = delegate()
				{
					this.BeginCallBombardment(pawn, faction, map, free);
				};
			}
			yield return new FloatMenuOption(label, action, faction.def.FactionIcon, faction.Color, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0);
			yield break;
		}

		// Token: 0x06005296 RID: 21142 RVA: 0x001BDCCC File Offset: 0x001BBECC
		private void BeginCallBombardment(Pawn caller, Faction faction, Map map, bool free)
		{
			this.targetingParameters = new TargetingParameters();
			this.targetingParameters.canTargetLocations = true;
			this.targetingParameters.canTargetSelf = true;
			this.targetingParameters.canTargetFires = true;
			this.targetingParameters.canTargetItems = true;
			this.caller = caller;
			this.map = map;
			this.faction = faction;
			this.free = free;
			this.targetingParameters.validator = ((TargetInfo target) => (this.def.royalAid.targetingRange <= 0f || target.Cell.DistanceTo(caller.Position) <= this.def.royalAid.targetingRange) && !target.Cell.Fogged(map));
			Find.Targeter.BeginTargeting(this, null);
		}

		// Token: 0x06005297 RID: 21143 RVA: 0x001BDD7C File Offset: 0x001BBF7C
		private void CallBombardment(IntVec3 targetCell)
		{
			Bombardment bombardment = (Bombardment)GenSpawn.Spawn(ThingDefOf.Bombardment, targetCell, this.map, WipeMode.Vanish);
			bombardment.impactAreaRadius = this.def.royalAid.radius;
			bombardment.explosionRadiusRange = this.def.royalAid.explosionRadiusRange;
			bombardment.bombIntervalTicks = this.def.royalAid.intervalTicks;
			bombardment.randomFireRadius = 1;
			bombardment.explosionCount = this.def.royalAid.explosionCount;
			bombardment.warmupTicks = this.def.royalAid.warmupTicks;
			bombardment.instigator = this.caller;
			SoundDefOf.OrbitalStrike_Ordered.PlayOneShotOnCamera(null);
			this.caller.royalty.GetPermit(this.def, this.faction).Notify_Used();
			if (!this.free)
			{
				this.caller.royalty.TryRemoveFavor(this.faction, this.def.royalAid.favorCost);
			}
		}

		// Token: 0x040030B2 RID: 12466
		private Faction faction;
	}
}
