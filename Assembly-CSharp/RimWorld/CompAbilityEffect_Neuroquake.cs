using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001384 RID: 4996
	public class CompAbilityEffect_Neuroquake : CompAbilityEffect
	{
		// Token: 0x170010C3 RID: 4291
		// (get) Token: 0x06006C88 RID: 27784 RVA: 0x00049CE9 File Offset: 0x00047EE9
		public new CompProperties_AbilityNeuroquake Props
		{
			get
			{
				return (CompProperties_AbilityNeuroquake)this.props;
			}
		}

		// Token: 0x06006C89 RID: 27785 RVA: 0x0021559C File Offset: 0x0021379C
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			if (this.affectedFactions == null)
			{
				this.affectedFactions = new Dictionary<Faction, Pair<bool, Pawn>>();
			}
			else
			{
				this.affectedFactions.Clear();
			}
			this.giveMentalStateTo.Clear();
			foreach (Pawn pawn in this.parent.pawn.Map.mapPawns.AllPawnsSpawned)
			{
				if (this.CanApplyEffects(pawn) && !pawn.Fogged())
				{
					bool flag = !pawn.Spawned || pawn.Position.InHorDistOf(this.parent.pawn.Position, this.parent.def.EffectRadius);
					this.AffectGoodwill(pawn.FactionOrExtraMiniOrHomeFaction, !flag, pawn);
					if (!flag)
					{
						this.giveMentalStateTo.Add(pawn);
					}
					else
					{
						this.GiveNeuroquakeThought(pawn);
					}
				}
			}
			foreach (Map map in Find.Maps)
			{
				if (map != this.parent.pawn.Map && Find.WorldGrid.TraversalDistanceBetween(map.Tile, this.parent.pawn.Map.Tile, true, this.Props.worldRangeTiles + 1) <= this.Props.worldRangeTiles)
				{
					foreach (Pawn p in map.mapPawns.AllPawns)
					{
						if (this.CanApplyEffects(p))
						{
							this.GiveNeuroquakeThought(p);
						}
					}
				}
			}
			foreach (Caravan caravan in Find.WorldObjects.Caravans)
			{
				if (Find.WorldGrid.TraversalDistanceBetween(caravan.Tile, this.parent.pawn.Map.Tile, true, this.Props.worldRangeTiles + 1) <= this.Props.worldRangeTiles)
				{
					foreach (Pawn p2 in caravan.pawns)
					{
						if (this.CanApplyEffects(p2))
						{
							this.GiveNeuroquakeThought(p2);
						}
					}
				}
			}
			foreach (Pawn pawn2 in this.giveMentalStateTo)
			{
				MentalStateDef def;
				if (!pawn2.RaceProps.IsMechanoid)
				{
					def = MentalStateDefOf.Berserk;
				}
				else
				{
					def = MentalStateDefOf.BerserkMechanoid;
				}
				CompAbilityEffect_GiveMentalState.TryGiveMentalStateWithDuration(def, pawn2, this.parent.def, StatDefOf.PsychicSensitivity);
				RestUtility.WakeUp(pawn2);
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.IsPlayer && !faction.defeated && !faction.HostileTo(Faction.OfPlayer))
				{
					this.AffectGoodwill(faction, false, null);
				}
			}
			foreach (KeyValuePair<Faction, Pair<bool, Pawn>> keyValuePair in this.affectedFactions)
			{
				Faction key = keyValuePair.Key;
				bool first = keyValuePair.Value.First;
				Pawn second = keyValuePair.Value.Second;
				key.TryAffectGoodwillWith(this.parent.pawn.Faction, first ? this.Props.goodwillImpactForBerserk : this.Props.goodwillImpactForNeuroquake, true, true, (first ? "GoodwillChangedReason_CausedBerserk" : "GoodwillChangedReason_CausedNeuroquakeEcho").Translate(second.Named("PAWN")), new GlobalTargetInfo?(second));
			}
			base.Apply(target, dest);
			this.affectedFactions.Clear();
			this.giveMentalStateTo.Clear();
		}

		// Token: 0x06006C8A RID: 27786 RVA: 0x00215A3C File Offset: 0x00213C3C
		private void AffectGoodwill(Faction faction, bool gaveMentalBreak, Pawn p = null)
		{
			Pair<bool, Pawn> pair;
			if (faction != null && !faction.IsPlayer && !faction.HostileTo(Faction.OfPlayer) && (!this.affectedFactions.TryGetValue(faction, out pair) || (!pair.First && gaveMentalBreak)))
			{
				this.affectedFactions[faction] = new Pair<bool, Pawn>(gaveMentalBreak, p);
			}
		}

		// Token: 0x06006C8B RID: 27787 RVA: 0x00049CF6 File Offset: 0x00047EF6
		private void GiveNeuroquakeThought(Pawn p)
		{
			Pawn_NeedsTracker needs = p.needs;
			if (needs == null)
			{
				return;
			}
			Need_Mood mood = needs.mood;
			if (mood == null)
			{
				return;
			}
			mood.thoughts.memories.TryGainMemory(ThoughtDefOf.NeuroquakeEcho, null);
		}

		// Token: 0x06006C8C RID: 27788 RVA: 0x00049D22 File Offset: 0x00047F22
		private bool CanApplyEffects(Pawn p)
		{
			return !p.Dead && !p.Downed && !p.Suspended && p.GetStatValue(StatDefOf.PsychicSensitivity, true) > float.Epsilon;
		}

		// Token: 0x040047FD RID: 18429
		private Dictionary<Faction, Pair<bool, Pawn>> affectedFactions;

		// Token: 0x040047FE RID: 18430
		private List<Pawn> giveMentalStateTo = new List<Pawn>();
	}
}
