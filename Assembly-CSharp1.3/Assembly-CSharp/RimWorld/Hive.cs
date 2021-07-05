using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020010AD RID: 4269
	public class Hive : ThingWithComps, IAttackTarget, ILoadReferenceable
	{
		// Token: 0x17001171 RID: 4465
		// (get) Token: 0x060065F3 RID: 26099 RVA: 0x00226E8A File Offset: 0x0022508A
		public CompCanBeDormant CompDormant
		{
			get
			{
				return base.GetComp<CompCanBeDormant>();
			}
		}

		// Token: 0x17001172 RID: 4466
		// (get) Token: 0x060065F4 RID: 26100 RVA: 0x00072AAA File Offset: 0x00070CAA
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001173 RID: 4467
		// (get) Token: 0x060065F5 RID: 26101 RVA: 0x00226E92 File Offset: 0x00225092
		public float TargetPriorityFactor
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x17001174 RID: 4468
		// (get) Token: 0x060065F6 RID: 26102 RVA: 0x00226E99 File Offset: 0x00225099
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x17001175 RID: 4469
		// (get) Token: 0x060065F7 RID: 26103 RVA: 0x00226EA0 File Offset: 0x002250A0
		public CompSpawnerPawn PawnSpawner
		{
			get
			{
				return base.GetComp<CompSpawnerPawn>();
			}
		}

		// Token: 0x060065F8 RID: 26104 RVA: 0x00226EA8 File Offset: 0x002250A8
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!base.Spawned)
			{
				return true;
			}
			CompCanBeDormant comp = base.GetComp<CompCanBeDormant>();
			return comp != null && !comp.Awake;
		}

		// Token: 0x060065F9 RID: 26105 RVA: 0x00226ED4 File Offset: 0x002250D4
		public static void ResetStaticData()
		{
			Hive.spawnablePawnKinds.Clear();
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megascarab);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Spelopede);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megaspider);
		}

		// Token: 0x060065FA RID: 26106 RVA: 0x00226F0D File Offset: 0x0022510D
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction == null)
			{
				this.SetFaction(Faction.OfInsects, null);
			}
		}

		// Token: 0x060065FB RID: 26107 RVA: 0x00226F2B File Offset: 0x0022512B
		public override void Tick()
		{
			base.Tick();
			if (base.Spawned && !this.CompDormant.Awake && !base.Position.Fogged(base.Map))
			{
				this.CompDormant.WakeUp();
			}
		}

		// Token: 0x060065FC RID: 26108 RVA: 0x00226F68 File Offset: 0x00225168
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			List<Lord> lords = map.lordManager.lords;
			for (int i = 0; i < lords.Count; i++)
			{
				lords[i].ReceiveMemo(Hive.MemoDeSpawned);
			}
			HiveUtility.Notify_HiveDespawned(this, map);
		}

		// Token: 0x060065FD RID: 26109 RVA: 0x00226FB8 File Offset: 0x002251B8
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (!this.questTags.NullOrEmpty<string>())
			{
				bool flag = false;
				List<Thing> list = base.Map.listerThings.ThingsOfDef(this.def);
				for (int i = 0; i < list.Count; i++)
				{
					Hive hive;
					if ((hive = (list[i] as Hive)) != null && hive != this && hive.CompDormant.Awake && !hive.questTags.NullOrEmpty<string>() && QuestUtility.AnyMatchingTags(hive.questTags, this.questTags))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					QuestUtility.SendQuestTargetSignals(this.questTags, "AllHivesDestroyed");
				}
			}
			base.Destroy(mode);
		}

		// Token: 0x060065FE RID: 26110 RVA: 0x00227060 File Offset: 0x00225260
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (dinfo.Def.ExternalViolenceFor(this) && dinfo.Instigator != null && dinfo.Instigator.Faction != null)
			{
				Lord lord = base.GetComp<CompSpawnerPawn>().Lord;
				if (lord != null)
				{
					lord.ReceiveMemo(Hive.MemoAttackedByEnemy);
				}
			}
			if (dinfo.Def == DamageDefOf.Flame && (float)this.HitPoints < (float)base.MaxHitPoints * 0.3f)
			{
				Lord lord2 = base.GetComp<CompSpawnerPawn>().Lord;
				if (lord2 != null)
				{
					lord2.ReceiveMemo(Hive.MemoBurnedBadly);
				}
			}
			base.PostApplyDamage(dinfo, totalDamageDealt);
		}

		// Token: 0x060065FF RID: 26111 RVA: 0x002270F4 File Offset: 0x002252F4
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (base.Spawned && (dinfo == null || dinfo.Value.Category != DamageInfo.SourceCategory.Collapse))
			{
				List<Lord> lords = base.Map.lordManager.lords;
				for (int i = 0; i < lords.Count; i++)
				{
					lords[i].ReceiveMemo(Hive.MemoDestroyedNonRoofCollapse);
				}
			}
			base.Kill(dinfo, exactCulprit);
		}

		// Token: 0x06006600 RID: 26112 RVA: 0x00227164 File Offset: 0x00225364
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (this.PawnSpawner.spawnedPawns.Count > 0)
			{
				if (this.PawnSpawner.spawnedPawns.Any((Pawn p) => !p.Downed))
				{
					reason = this.def.label;
					return true;
				}
			}
			reason = null;
			return false;
		}

		// Token: 0x06006601 RID: 26113 RVA: 0x002271C8 File Offset: 0x002253C8
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			foreach (Gizmo gizmo2 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006602 RID: 26114 RVA: 0x002271D8 File Offset: 0x002253D8
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode != LoadSaveMode.Saving)
			{
				bool flag = false;
				Scribe_Values.Look<bool>(ref flag, "active", false, false);
				if (flag)
				{
					this.CompDormant.WakeUp();
				}
			}
		}

		// Token: 0x04003987 RID: 14727
		public const int PawnSpawnRadius = 2;

		// Token: 0x04003988 RID: 14728
		public const float MaxSpawnedPawnsPoints = 500f;

		// Token: 0x04003989 RID: 14729
		public const float InitialPawnsPoints = 200f;

		// Token: 0x0400398A RID: 14730
		public static List<PawnKindDef> spawnablePawnKinds = new List<PawnKindDef>();

		// Token: 0x0400398B RID: 14731
		public static readonly string MemoAttackedByEnemy = "HiveAttacked";

		// Token: 0x0400398C RID: 14732
		public static readonly string MemoDeSpawned = "HiveDeSpawned";

		// Token: 0x0400398D RID: 14733
		public static readonly string MemoBurnedBadly = "HiveBurnedBadly";

		// Token: 0x0400398E RID: 14734
		public static readonly string MemoDestroyedNonRoofCollapse = "HiveDestroyedNonRoofCollapse";
	}
}
