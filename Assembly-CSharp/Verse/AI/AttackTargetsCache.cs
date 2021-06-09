using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000AC2 RID: 2754
	public class AttackTargetsCache
	{
		// Token: 0x17000A0E RID: 2574
		// (get) Token: 0x060040F4 RID: 16628 RVA: 0x00030893 File Offset: 0x0002EA93
		public HashSet<IAttackTarget> TargetsHostileToColony
		{
			get
			{
				return this.TargetsHostileToFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x000308A0 File Offset: 0x0002EAA0
		public AttackTargetsCache(Map map)
		{
			this.map = map;
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x000308DB File Offset: 0x0002EADB
		public static void AttackTargetsCacheStaticUpdate()
		{
			AttackTargetsCache.targets.Clear();
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x00185AC0 File Offset: 0x00183CC0
		public void UpdateTarget(IAttackTarget t)
		{
			if (!this.allTargets.Contains(t))
			{
				return;
			}
			this.DeregisterTarget(t);
			Thing thing = t.Thing;
			if (thing.Spawned && thing.Map == this.map)
			{
				this.RegisterTarget(t);
			}
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x00185B08 File Offset: 0x00183D08
		public List<IAttackTarget> GetPotentialTargetsFor(IAttackTargetSearcher th)
		{
			Thing thing = th.Thing;
			AttackTargetsCache.targets.Clear();
			Faction faction = thing.Faction;
			if (faction != null)
			{
				foreach (IAttackTarget attackTarget in this.TargetsHostileToFaction(faction))
				{
					if (thing.HostileTo(attackTarget.Thing))
					{
						AttackTargetsCache.targets.Add(attackTarget);
					}
				}
			}
			foreach (Pawn pawn in this.pawnsInAggroMentalState)
			{
				if (thing.HostileTo(pawn))
				{
					AttackTargetsCache.targets.Add(pawn);
				}
			}
			foreach (Pawn pawn2 in this.factionlessHumanlikes)
			{
				if (thing.HostileTo(pawn2))
				{
					AttackTargetsCache.targets.Add(pawn2);
				}
			}
			Pawn pawn3 = th as Pawn;
			if (pawn3 != null && PrisonBreakUtility.IsPrisonBreaking(pawn3))
			{
				Faction hostFaction = pawn3.guest.HostFaction;
				List<Pawn> list = this.map.mapPawns.SpawnedPawnsInFaction(hostFaction);
				for (int i = 0; i < list.Count; i++)
				{
					if (thing.HostileTo(list[i]))
					{
						AttackTargetsCache.targets.Add(list[i]);
					}
				}
			}
			return AttackTargetsCache.targets;
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x000308E7 File Offset: 0x0002EAE7
		public HashSet<IAttackTarget> TargetsHostileToFaction(Faction f)
		{
			if (f == null)
			{
				Log.Warning("Called TargetsHostileToFaction with null faction.", false);
				return AttackTargetsCache.emptySet;
			}
			if (this.targetsHostileToFaction.ContainsKey(f))
			{
				return this.targetsHostileToFaction[f];
			}
			return AttackTargetsCache.emptySet;
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x00185CA4 File Offset: 0x00183EA4
		public void Notify_ThingSpawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.RegisterTarget(attackTarget);
			}
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x00185CC4 File Offset: 0x00183EC4
		public void Notify_ThingDespawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.DeregisterTarget(attackTarget);
			}
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x00185CE4 File Offset: 0x00183EE4
		public void Notify_FactionHostilityChanged(Faction f1, Faction f2)
		{
			AttackTargetsCache.tmpTargets.Clear();
			foreach (IAttackTarget attackTarget in this.allTargets)
			{
				Thing thing = attackTarget.Thing;
				Pawn pawn = thing as Pawn;
				if (thing.Faction == f1 || thing.Faction == f2 || (pawn != null && pawn.HostFaction == f1) || (pawn != null && pawn.HostFaction == f2))
				{
					AttackTargetsCache.tmpTargets.Add(attackTarget);
				}
			}
			for (int i = 0; i < AttackTargetsCache.tmpTargets.Count; i++)
			{
				this.UpdateTarget(AttackTargetsCache.tmpTargets[i]);
			}
			AttackTargetsCache.tmpTargets.Clear();
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x00185DB4 File Offset: 0x00183FB4
		private void RegisterTarget(IAttackTarget target)
		{
			if (this.allTargets.Contains(target))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to register the same target twice ",
					target.ToStringSafe<IAttackTarget>(),
					" in ",
					base.GetType()
				}), false);
				return;
			}
			Thing thing = target.Thing;
			if (!thing.Spawned)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to register unspawned thing ",
					thing.ToStringSafe<Thing>(),
					" in ",
					base.GetType()
				}), false);
				return;
			}
			if (thing.Map != this.map)
			{
				Log.Warning("Tried to register attack target " + thing.ToStringSafe<Thing>() + " but its Map is not this one.", false);
				return;
			}
			this.allTargets.Add(target);
			List<Faction> allFactionsListForReading = Find.FactionManager.AllFactionsListForReading;
			for (int i = 0; i < allFactionsListForReading.Count; i++)
			{
				if (thing.HostileTo(allFactionsListForReading[i]))
				{
					if (!this.targetsHostileToFaction.ContainsKey(allFactionsListForReading[i]))
					{
						this.targetsHostileToFaction.Add(allFactionsListForReading[i], new HashSet<IAttackTarget>());
					}
					this.targetsHostileToFaction[allFactionsListForReading[i]].Add(target);
				}
			}
			Pawn pawn = target as Pawn;
			if (pawn != null)
			{
				if (pawn.InAggroMentalState)
				{
					this.pawnsInAggroMentalState.Add(pawn);
				}
				if (pawn.Faction == null && pawn.RaceProps.Humanlike)
				{
					this.factionlessHumanlikes.Add(pawn);
				}
			}
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x00185F30 File Offset: 0x00184130
		private void DeregisterTarget(IAttackTarget target)
		{
			if (!this.allTargets.Contains(target))
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to deregister ",
					target,
					" but it's not in ",
					base.GetType()
				}), false);
				return;
			}
			this.allTargets.Remove(target);
			foreach (KeyValuePair<Faction, HashSet<IAttackTarget>> keyValuePair in this.targetsHostileToFaction)
			{
				keyValuePair.Value.Remove(target);
			}
			Pawn pawn = target as Pawn;
			if (pawn != null)
			{
				this.pawnsInAggroMentalState.Remove(pawn);
				this.factionlessHumanlikes.Remove(pawn);
			}
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x00185FF8 File Offset: 0x001841F8
		private void Debug_AssertHostile(Faction f, HashSet<IAttackTarget> targets)
		{
			AttackTargetsCache.tmpToUpdate.Clear();
			foreach (IAttackTarget attackTarget in targets)
			{
				if (!attackTarget.Thing.HostileTo(f))
				{
					AttackTargetsCache.tmpToUpdate.Add(attackTarget);
					Log.Error(string.Concat(new string[]
					{
						"Target ",
						attackTarget.ToStringSafe<IAttackTarget>(),
						" is not hostile to ",
						f.ToStringSafe<Faction>(),
						" (in ",
						base.GetType().Name,
						") but it's in the list (forgot to update the target somewhere?). Trying to update the target..."
					}), false);
				}
			}
			for (int i = 0; i < AttackTargetsCache.tmpToUpdate.Count; i++)
			{
				this.UpdateTarget(AttackTargetsCache.tmpToUpdate[i]);
			}
			AttackTargetsCache.tmpToUpdate.Clear();
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x0003091D File Offset: 0x0002EB1D
		public bool Debug_CheckIfInAllTargets(IAttackTarget t)
		{
			return t != null && this.allTargets.Contains(t);
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x00030930 File Offset: 0x0002EB30
		public bool Debug_CheckIfHostileToFaction(Faction f, IAttackTarget t)
		{
			return f != null && t != null && this.targetsHostileToFaction[f].Contains(t);
		}

		// Token: 0x04002CDD RID: 11485
		private Map map;

		// Token: 0x04002CDE RID: 11486
		private HashSet<IAttackTarget> allTargets = new HashSet<IAttackTarget>();

		// Token: 0x04002CDF RID: 11487
		private Dictionary<Faction, HashSet<IAttackTarget>> targetsHostileToFaction = new Dictionary<Faction, HashSet<IAttackTarget>>();

		// Token: 0x04002CE0 RID: 11488
		private HashSet<Pawn> pawnsInAggroMentalState = new HashSet<Pawn>();

		// Token: 0x04002CE1 RID: 11489
		private HashSet<Pawn> factionlessHumanlikes = new HashSet<Pawn>();

		// Token: 0x04002CE2 RID: 11490
		private static List<IAttackTarget> targets = new List<IAttackTarget>();

		// Token: 0x04002CE3 RID: 11491
		private static HashSet<IAttackTarget> emptySet = new HashSet<IAttackTarget>();

		// Token: 0x04002CE4 RID: 11492
		private static List<IAttackTarget> tmpTargets = new List<IAttackTarget>();

		// Token: 0x04002CE5 RID: 11493
		private static List<IAttackTarget> tmpToUpdate = new List<IAttackTarget>();
	}
}
