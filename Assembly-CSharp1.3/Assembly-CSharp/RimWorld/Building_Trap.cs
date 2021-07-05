using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001050 RID: 4176
	public abstract class Building_Trap : Building
	{
		// Token: 0x170010CA RID: 4298
		// (get) Token: 0x060062C1 RID: 25281 RVA: 0x002177C6 File Offset: 0x002159C6
		private bool CanSetAutoRearm
		{
			get
			{
				return base.Faction == Faction.OfPlayer && this.def.blueprintDef != null && this.def.IsResearchFinished;
			}
		}

		// Token: 0x060062C2 RID: 25282 RVA: 0x002177EF File Offset: 0x002159EF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.autoRearm, "autoRearm", false, false);
			Scribe_Collections.Look<Pawn>(ref this.touchingPawns, "testees", LookMode.Reference, Array.Empty<object>());
		}

		// Token: 0x060062C3 RID: 25283 RVA: 0x0021781F File Offset: 0x00215A1F
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.autoRearm = (this.CanSetAutoRearm && map.areaManager.Home[base.Position]);
			}
		}

		// Token: 0x060062C4 RID: 25284 RVA: 0x00217854 File Offset: 0x00215A54
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

		// Token: 0x060062C5 RID: 25285 RVA: 0x00217918 File Offset: 0x00215B18
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

		// Token: 0x060062C6 RID: 25286 RVA: 0x002179CC File Offset: 0x00215BCC
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

		// Token: 0x060062C7 RID: 25287 RVA: 0x00217A60 File Offset: 0x00215C60
		public bool KnowsOfTrap(Pawn p)
		{
			return (p.Faction != null && !p.Faction.HostileTo(base.Faction)) || (p.Faction == null && p.RaceProps.Animal && !p.InAggroMentalState) || (p.guest != null && p.guest.Released) || (!p.IsPrisoner && base.Faction != null && p.HostFaction == base.Faction) || (p.RaceProps.Humanlike && p.IsFormingCaravan()) || (p.IsPrisoner && p.guest.ShouldWaitInsteadOfEscaping && base.Faction == p.HostFaction) || (p.Faction == null && p.RaceProps.Humanlike);
		}

		// Token: 0x060062C8 RID: 25288 RVA: 0x00217B34 File Offset: 0x00215D34
		public override ushort PathFindCostFor(Pawn p)
		{
			if (!this.KnowsOfTrap(p))
			{
				return 0;
			}
			return 800;
		}

		// Token: 0x060062C9 RID: 25289 RVA: 0x00217B46 File Offset: 0x00215D46
		public override ushort PathWalkCostFor(Pawn p)
		{
			if (!this.KnowsOfTrap(p))
			{
				return 0;
			}
			return 40;
		}

		// Token: 0x060062CA RID: 25290 RVA: 0x00217B55 File Offset: 0x00215D55
		public override bool IsDangerousFor(Pawn p)
		{
			return this.KnowsOfTrap(p);
		}

		// Token: 0x060062CB RID: 25291 RVA: 0x00217B60 File Offset: 0x00215D60
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

		// Token: 0x060062CC RID: 25292 RVA: 0x00217BB0 File Offset: 0x00215DB0
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

		// Token: 0x060062CD RID: 25293
		protected abstract void SpringSub(Pawn p);

		// Token: 0x060062CE RID: 25294 RVA: 0x00217BDC File Offset: 0x00215DDC
		private void CheckAutoRebuild(Map map)
		{
			if (this.autoRearm && this.CanSetAutoRearm && map != null && GenConstruct.CanPlaceBlueprintAt(this.def, base.Position, base.Rotation, map, false, null, null, base.Stuff).Accepted)
			{
				GenConstruct.PlaceBlueprintForBuild(this.def, base.Position, map, base.Rotation, Faction.OfPlayer, base.Stuff);
			}
		}

		// Token: 0x060062CF RID: 25295 RVA: 0x00217C4B File Offset: 0x00215E4B
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

		// Token: 0x0400381A RID: 14362
		private bool autoRearm;

		// Token: 0x0400381B RID: 14363
		private List<Pawn> touchingPawns = new List<Pawn>();

		// Token: 0x0400381C RID: 14364
		private const float KnowerSpringChanceFactorSameFaction = 0.005f;

		// Token: 0x0400381D RID: 14365
		private const float KnowerSpringChanceFactorWildAnimal = 0.2f;

		// Token: 0x0400381E RID: 14366
		private const float KnowerSpringChanceFactorFactionlessHuman = 0.3f;

		// Token: 0x0400381F RID: 14367
		private const float KnowerSpringChanceFactorOther = 0f;

		// Token: 0x04003820 RID: 14368
		private const ushort KnowerPathFindCost = 800;

		// Token: 0x04003821 RID: 14369
		private const ushort KnowerPathWalkCost = 40;
	}
}
