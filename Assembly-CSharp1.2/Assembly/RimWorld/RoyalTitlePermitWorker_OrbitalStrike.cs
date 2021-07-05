using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001474 RID: 5236
	public class RoyalTitlePermitWorker_OrbitalStrike : RoyalTitlePermitWorker_Targeted
	{
		// Token: 0x06007101 RID: 28929 RVA: 0x00228CFC File Offset: 0x00226EFC
		public override bool ValidateTarget(LocalTargetInfo target)
		{
			if (!base.CanHitTarget(target))
			{
				if (target.IsValid)
				{
					Messages.Message(this.def.LabelCap + ": " + "AbilityCannotHitTarget".Translate(), MessageTypeDefOf.RejectInput, true);
				}
				return false;
			}
			return true;
		}

		// Token: 0x06007102 RID: 28930 RVA: 0x00228D54 File Offset: 0x00226F54
		public override void DrawHighlight(LocalTargetInfo target)
		{
			GenDraw.DrawRadiusRing(this.caller.Position, this.def.royalAid.targetingRange, Color.white, null);
			GenDraw.DrawRadiusRing(target.Cell, this.def.royalAid.radius + this.def.royalAid.explosionRadiusRange.max, Color.white, null);
			if (target.IsValid)
			{
				GenDraw.DrawTargetHighlight(target);
			}
		}

		// Token: 0x06007103 RID: 28931 RVA: 0x0004C20D File Offset: 0x0004A40D
		public override void OrderForceTarget(LocalTargetInfo target)
		{
			this.CallBombardment(target.Cell);
		}

		// Token: 0x06007104 RID: 28932 RVA: 0x0004C21C File Offset: 0x0004A41C
		public override IEnumerable<FloatMenuOption> GetRoyalAidOptions(Map map, Pawn pawn, Faction faction)
		{
			if (faction.HostileTo(Faction.OfPlayer))
			{
				yield return new FloatMenuOption(this.def.LabelCap + ": " + "CommandCallRoyalAidFactionHostile".Translate(faction.Named("FACTION")), null, MenuOptionPriority.Default, null, null, 0f, null, null);
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
			yield return new FloatMenuOption(label, action, faction.def.FactionIcon, faction.Color, MenuOptionPriority.Default, null, null, 0f, null, null);
			yield break;
		}

		// Token: 0x06007105 RID: 28933 RVA: 0x00228DD0 File Offset: 0x00226FD0
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

		// Token: 0x06007106 RID: 28934 RVA: 0x00228E80 File Offset: 0x00227080
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

		// Token: 0x04004A96 RID: 19094
		private Faction faction;
	}
}
