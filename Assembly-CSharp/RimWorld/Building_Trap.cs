using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200167B RID: 5755
	public abstract class Building_Trap : Building
	{
		// Token: 0x17001354 RID: 4948
		// (get) Token: 0x06007D88 RID: 32136 RVA: 0x000545FF File Offset: 0x000527FF
		private bool CanSetAutoRearm
		{
			get
			{
				return base.Faction == Faction.OfPlayer && this.def.blueprintDef != null && this.def.IsResearchFinished;
			}
		}

		// Token: 0x06007D89 RID: 32137 RVA: 0x00054628 File Offset: 0x00052828
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.autoRearm, "autoRearm", false, false);
			Scribe_Collections.Look<Pawn>(ref this.touchingPawns, "testees", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x06007D8A RID: 32138 RVA: 0x00054658 File Offset: 0x00052858
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.autoRearm = (this.CanSetAutoRearm && map.areaManager.Home[base.Position]);
			}
		}

		// Token: 0x06007D8B RID: 32139 RVA: 0x002575BC File Offset: 0x002557BC
		public override void Tick()
		{
			if (base.Spawned)
			{
				List<Thing> thingList = base.Position.GetThingList(base.Map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Pawn pawn = thingList[i] as Pawn;
					if (pawn != null && !this.touchingPawns.Contains(pawn))
					{
						this.touchingPawns.Add(pawn);
						this.CheckSpring(pawn);
					}
				}
				for (int j = 0; j < this.touchingPawns.Count; j++)
				{
					Pawn pawn2 = this.touchingPawns[j];
					if (!pawn2.Spawned || pawn2.Position != base.Position)
					{
						this.touchingPawns.Remove(pawn2);
					}
				}
			}
			base.Tick();
		}

		// Token: 0x06007D8C RID: 32140 RVA: 0x00257680 File Offset: 0x00255880
		private void CheckSpring(Pawn p)
		{
			if (Rand.Chance(this.SpringChance(p)))
			{
				Map map = base.Map;
				this.Spring(p);
				if (p.Faction == Faction.OfPlayer || p.HostFaction == Faction.OfPlayer)
				{
					Find.LetterStack.ReceiveLetter("LetterFriendlyTrapSprungLabel".Translate(p.LabelShort, p).CapitalizeFirst(), "LetterFriendlyTrapSprung".Translate(p.LabelShort, p).CapitalizeFirst(), LetterDefOf.NegativeEvent, new TargetInfo(base.Position, map, false), null, null, null, null);
				}
			}
		}

		// Token: 0x06007D8D RID: 32141 RVA: 0x00257734 File Offset: 0x00255934
		protected virtual float SpringChance(Pawn p)
		{
			float num = 1f;
			if (this.KnowsOfTrap(p))
			{
				if (p.Faction == null)
				{
					if (p.RaceProps.Animal)
					{
						num = 0.2f;
						num *= this.def.building.trapPeacefulWildAnimalsSpringChanceFactor;
					}
					else
					{
						num = 0.3f;
					}
				}
				else if (p.Faction == base.Faction)
				{
					num = 0.005f;
				}
				else
				{
					num = 0f;
				}
			}
			num *= this.GetStatValue(StatDefOf.TrapSpringChance, true) * p.GetStatValue(StatDefOf.PawnTrapSpringChance, true);
			return Mathf.Clamp01(num);
		}

		// Token: 0x06007D8E RID: 32142 RVA: 0x002577C8 File Offset: 0x002559C8
		public bool KnowsOfTrap(Pawn p)
		{
			return (p.Faction != null && !p.Faction.HostileTo(base.Faction)) || (p.Faction == null && p.RaceProps.Animal && !p.InAggroMentalState) || (p.guest != null && p.guest.Released) || (!p.IsPrisoner && base.Faction != null && p.HostFaction == base.Faction) || (p.RaceProps.Humanlike && p.IsFormingCaravan()) || (p.IsPrisoner && p.guest.ShouldWaitInsteadOfEscaping && base.Faction == p.HostFaction) || (p.Faction == null && p.RaceProps.Humanlike);
		}

		// Token: 0x06007D8F RID: 32143 RVA: 0x0005468C File Offset: 0x0005288C
		public override ushort PathFindCostFor(Pawn p)
		{
			if (!this.KnowsOfTrap(p))
			{
				return 0;
			}
			return 800;
		}

		// Token: 0x06007D90 RID: 32144 RVA: 0x0005469E File Offset: 0x0005289E
		public override ushort PathWalkCostFor(Pawn p)
		{
			if (!this.KnowsOfTrap(p))
			{
				return 0;
			}
			return 40;
		}

		// Token: 0x06007D91 RID: 32145 RVA: 0x000546AD File Offset: 0x000528AD
		public override bool IsDangerousFor(Pawn p)
		{
			return this.KnowsOfTrap(p);
		}

		// Token: 0x06007D92 RID: 32146 RVA: 0x0025789C File Offset: 0x00255A9C
		public void Spring(Pawn p)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			this.SpringSub(p);
			if (this.def.building.trapDestroyOnSpring)
			{
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
				if (spawned)
				{
					this.CheckAutoRebuild(map);
				}
			}
		}

		// Token: 0x06007D93 RID: 32147 RVA: 0x002578EC File Offset: 0x00255AEC
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Kill(dinfo, exactCulprit);
			if (spawned)
			{
				this.CheckAutoRebuild(map);
			}
		}

		// Token: 0x06007D94 RID: 32148
		protected abstract void SpringSub(Pawn p);

		// Token: 0x06007D95 RID: 32149 RVA: 0x00257918 File Offset: 0x00255B18
		private void CheckAutoRebuild(Map map)
		{
			if (this.autoRearm && this.CanSetAutoRearm && map != null && GenConstruct.CanPlaceBlueprintAt(this.def, base.Position, base.Rotation, map, false, null, null, base.Stuff).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(this.def, base.Position, map, base.Rotation, Faction.OfPlayer, base.Stuff);
			}
		}

		// Token: 0x06007D96 RID: 32150 RVA: 0x000546B6 File Offset: 0x000528B6
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.CanSetAutoRearm)
			{
				yield return new Command_Toggle
				{
					defaultLabel = "CommandAutoRearm".Translate(),
					defaultDesc = "CommandAutoRearmDesc".Translate(),
					hotKey = KeyBindingDefOf.Misc3,
					icon = TexCommand.RearmTrap,
					isActive = (() => this.autoRearm),
					toggleAction = delegate()
					{
						this.autoRearm = !this.autoRearm;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x040051E2 RID: 20962
		private bool autoRearm;

		// Token: 0x040051E3 RID: 20963
		private List<Pawn> touchingPawns = new List<Pawn>();

		// Token: 0x040051E4 RID: 20964
		private const float KnowerSpringChanceFactorSameFaction = 0.005f;

		// Token: 0x040051E5 RID: 20965
		private const float KnowerSpringChanceFactorWildAnimal = 0.2f;

		// Token: 0x040051E6 RID: 20966
		private const float KnowerSpringChanceFactorFactionlessHuman = 0.3f;

		// Token: 0x040051E7 RID: 20967
		private const float KnowerSpringChanceFactorOther = 0f;

		// Token: 0x040051E8 RID: 20968
		private const ushort KnowerPathFindCost = 800;

		// Token: 0x040051E9 RID: 20969
		private const ushort KnowerPathWalkCost = 40;
	}
}
