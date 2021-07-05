using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D4B RID: 3403
	public class CompAbilityEffect_Neuroquake : CompAbilityEffect
	{
		// Token: 0x17000DBA RID: 3514
		// (get) Token: 0x06004F68 RID: 20328 RVA: 0x001A9B10 File Offset: 0x001A7D10
		public new CompProperties_AbilityNeuroquake Props
		{
			get
			{
				return (CompProperties_AbilityNeuroquake)this.props;
			}
		}

		// Token: 0x06004F69 RID: 20329 RVA: 0x001A9B20 File Offset: 0x001A7D20
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
					bool flag = !pawn.Spawned || pawn.Position.InHorDistOf(this.parent.pawn.Position, this.parent.def.EffectRadius) || !pawn.Position.InHorDistOf(this.parent.pawn.Position, this.Props.mentalStateRadius);
					this.AffectGoodwill(pawn.HomeFaction, !flag, pawn);
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
				CompAbilityEffect_GiveMentalState.TryGiveMentalStateWithDuration(def, pawn2, this.parent.def, StatDefOf.PsychicSensitivity, this.parent.pawn);
				RestUtility.WakeUp(pawn2);
			}
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				if (!faction.IsPlayer && !faction.defeated && !faction.HostileTo(Faction.OfPlayer))
				{
					this.AffectGoodwill(faction, false, null);
				}
			}
			if (this.parent.pawn.Faction == Faction.OfPlayer)
			{
				foreach (KeyValuePair<Faction, Pair<bool, Pawn>> keyValuePair in this.affectedFactions)
				{
					Faction key = keyValuePair.Key;
					bool first = keyValuePair.Value.First;
					Pawn second = keyValuePair.Value.Second;
					int goodwillChange = first ? this.Props.goodwillImpactForBerserk : this.Props.goodwillImpactForNeuroquake;
					Faction.OfPlayer.TryAffectGoodwillWith(key, goodwillChange, true, true, HistoryEventDefOf.UsedHarmfulAbility, null);
				}
			}
			base.Apply(target, dest);
			this.affectedFactions.Clear();
			this.giveMentalStateTo.Clear();
		}

		// Token: 0x06004F6A RID: 20330 RVA: 0x001A9FEC File Offset: 0x001A81EC
		private void AffectGoodwill(Faction faction, bool gaveMentalBreak, Pawn p = null)
		{
			Pair<bool, Pawn> pair;
			if (faction != null && !faction.IsPlayer && !faction.HostileTo(Faction.OfPlayer) && (!this.affectedFactions.TryGetValue(faction, out pair) || (!pair.First && gaveMentalBreak)))
			{
				this.affectedFactions[faction] = new Pair<bool, Pawn>(gaveMentalBreak, p);
			}
		}

		// Token: 0x06004F6B RID: 20331 RVA: 0x001AA042 File Offset: 0x001A8242
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
			mood.thoughts.memories.TryGainMemory(ThoughtDefOf.NeuroquakeEcho, null, null);
		}

		// Token: 0x06004F6C RID: 20332 RVA: 0x001AA06F File Offset: 0x001A826F
		private bool CanApplyEffects(Pawn p)
		{
			return !p.Dead && !p.Suspended && p.GetStatValue(StatDefOf.PsychicSensitivity, true) > float.Epsilon;
		}

		// Token: 0x06004F6D RID: 20333 RVA: 0x001AA098 File Offset: 0x001A8298
		public override void OnGizmoUpdate()
		{
			if (CompAbilityEffect_Neuroquake.cachedRadiusCellsTarget == null || CompAbilityEffect_Neuroquake.cachedRadiusCellsTarget.Value == this.parent.pawn.Position || CompAbilityEffect_Neuroquake.cachedRadiusCellsMap != this.parent.pawn.Map)
			{
				CompAbilityEffect_Neuroquake.cachedRadiusCells.Clear();
				foreach (IntVec3 item in this.parent.pawn.Map.AllCells)
				{
					if (item.InHorDistOf(this.parent.pawn.Position, this.Props.mentalStateRadius))
					{
						CompAbilityEffect_Neuroquake.cachedRadiusCells.Add(item);
					}
				}
				CompAbilityEffect_Neuroquake.cachedRadiusCellsTarget = new IntVec3?(this.parent.pawn.Position);
			}
			GenDraw.DrawFieldEdges(CompAbilityEffect_Neuroquake.cachedRadiusCells);
		}

		// Token: 0x04002FAB RID: 12203
		private Dictionary<Faction, Pair<bool, Pawn>> affectedFactions;

		// Token: 0x04002FAC RID: 12204
		private List<Pawn> giveMentalStateTo = new List<Pawn>();

		// Token: 0x04002FAD RID: 12205
		private static List<IntVec3> cachedRadiusCells = new List<IntVec3>();

		// Token: 0x04002FAE RID: 12206
		private static IntVec3? cachedRadiusCellsTarget = null;

		// Token: 0x04002FAF RID: 12207
		private static Map cachedRadiusCellsMap = null;
	}
}
