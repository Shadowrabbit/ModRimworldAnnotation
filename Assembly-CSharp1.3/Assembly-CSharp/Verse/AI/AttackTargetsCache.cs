using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x0200065E RID: 1630
	public class AttackTargetsCache
	{
		// Token: 0x1700089F RID: 2207
		// (get) Token: 0x06002E11 RID: 11793 RVA: 0x00113FF4 File Offset: 0x001121F4
		public HashSet<IAttackTarget> TargetsHostileToColony
		{
			get
			{
				return this.TargetsHostileToFaction(Faction.OfPlayer);
			}
		}

		// Token: 0x06002E12 RID: 11794 RVA: 0x00114001 File Offset: 0x00112201
		public AttackTargetsCache(Map map)
		{
			this.map = map;
		}

		// Token: 0x06002E13 RID: 11795 RVA: 0x0011403C File Offset: 0x0011223C
		public static void AttackTargetsCacheStaticUpdate()
		{
			AttackTargetsCache.targets.Clear();
		}

		// Token: 0x06002E14 RID: 11796 RVA: 0x00114048 File Offset: 0x00112248
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

		// Token: 0x06002E15 RID: 11797 RVA: 0x00114090 File Offset: 0x00112290
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
			if (pawn3 != null && ModsConfig.IdeologyActive && SlaveRebellionUtility.IsRebelling(pawn3))
			{
				Faction faction2 = pawn3.Faction;
				List<Pawn> list2 = this.map.mapPawns.SpawnedPawnsInFaction(faction2);
				for (int j = 0; j < list2.Count; j++)
				{
					if (thing.HostileTo(list2[j]))
					{
						AttackTargetsCache.targets.Add(list2[j]);
					}
				}
			}
			return AttackTargetsCache.targets;
		}

		// Token: 0x06002E16 RID: 11798 RVA: 0x00114294 File Offset: 0x00112494
		public HashSet<IAttackTarget> TargetsHostileToFaction(Faction f)
		{
			if (f == null)
			{
				Log.Warning("Called TargetsHostileToFaction with null faction.");
				return AttackTargetsCache.emptySet;
			}
			if (this.targetsHostileToFaction.ContainsKey(f))
			{
				return this.targetsHostileToFaction[f];
			}
			return AttackTargetsCache.emptySet;
		}

		// Token: 0x06002E17 RID: 11799 RVA: 0x001142CC File Offset: 0x001124CC
		public void Notify_ThingSpawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.RegisterTarget(attackTarget);
			}
		}

		// Token: 0x06002E18 RID: 11800 RVA: 0x001142EC File Offset: 0x001124EC
		public void Notify_ThingDespawned(Thing th)
		{
			IAttackTarget attackTarget = th as IAttackTarget;
			if (attackTarget != null)
			{
				this.DeregisterTarget(attackTarget);
			}
		}

		// Token: 0x06002E19 RID: 11801 RVA: 0x0011430C File Offset: 0x0011250C
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

		// Token: 0x06002E1A RID: 11802 RVA: 0x001143DC File Offset: 0x001125DC
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
				}));
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
				}));
				return;
			}
			if (thing.Map != this.map)
			{
				Log.Warning("Tried to register attack target " + thing.ToStringSafe<Thing>() + " but its Map is not this one.");
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

		// Token: 0x06002E1B RID: 11803 RVA: 0x00114554 File Offset: 0x00112754
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
				}));
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

		// Token: 0x06002E1C RID: 11804 RVA: 0x0011461C File Offset: 0x0011281C
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
					}));
				}
			}
			for (int i = 0; i < AttackTargetsCache.tmpToUpdate.Count; i++)
			{
				this.UpdateTarget(AttackTargetsCache.tmpToUpdate[i]);
			}
			AttackTargetsCache.tmpToUpdate.Clear();
		}

		// Token: 0x06002E1D RID: 11805 RVA: 0x00114708 File Offset: 0x00112908
		public bool Debug_CheckIfInAllTargets(IAttackTarget t)
		{
			return t != null && this.allTargets.Contains(t);
		}

		// Token: 0x06002E1E RID: 11806 RVA: 0x0011471B File Offset: 0x0011291B
		public bool Debug_CheckIfHostileToFaction(Faction f, IAttackTarget t)
		{
			return f != null && t != null && this.targetsHostileToFaction[f].Contains(t);
		}

		// Token: 0x04001C55 RID: 7253
		private Map map;

		// Token: 0x04001C56 RID: 7254
		private HashSet<IAttackTarget> allTargets = new HashSet<IAttackTarget>();

		// Token: 0x04001C57 RID: 7255
		private Dictionary<Faction, HashSet<IAttackTarget>> targetsHostileToFaction = new Dictionary<Faction, HashSet<IAttackTarget>>();

		// Token: 0x04001C58 RID: 7256
		private HashSet<Pawn> pawnsInAggroMentalState = new HashSet<Pawn>();

		// Token: 0x04001C59 RID: 7257
		private HashSet<Pawn> factionlessHumanlikes = new HashSet<Pawn>();

		// Token: 0x04001C5A RID: 7258
		private static List<IAttackTarget> targets = new List<IAttackTarget>();

		// Token: 0x04001C5B RID: 7259
		private static HashSet<IAttackTarget> emptySet = new HashSet<IAttackTarget>();

		// Token: 0x04001C5C RID: 7260
		private static List<IAttackTarget> tmpTargets = new List<IAttackTarget>();

		// Token: 0x04001C5D RID: 7261
		private static List<IAttackTarget> tmpToUpdate = new List<IAttackTarget>();
	}
}
