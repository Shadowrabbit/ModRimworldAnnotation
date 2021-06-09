using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001708 RID: 5896
	public class Hive : ThingWithComps, IAttackTarget, ILoadReferenceable
	{
		// Token: 0x17001422 RID: 5154
		// (get) Token: 0x060081C5 RID: 33221 RVA: 0x00057237 File Offset: 0x00055437
		public CompCanBeDormant CompDormant
		{
			get
			{
				return base.GetComp<CompCanBeDormant>();
			}
		}

		// Token: 0x17001423 RID: 5155
		// (get) Token: 0x060081C6 RID: 33222 RVA: 0x000187F7 File Offset: 0x000169F7
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		// Token: 0x17001424 RID: 5156
		// (get) Token: 0x060081C7 RID: 33223 RVA: 0x0005723F File Offset: 0x0005543F
		public float TargetPriorityFactor
		{
			get
			{
				return 0.4f;
			}
		}

		// Token: 0x17001425 RID: 5157
		// (get) Token: 0x060081C8 RID: 33224 RVA: 0x00057246 File Offset: 0x00055446
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				return LocalTargetInfo.Invalid;
			}
		}

		// Token: 0x17001426 RID: 5158
		// (get) Token: 0x060081C9 RID: 33225 RVA: 0x0005724D File Offset: 0x0005544D
		public CompSpawnerPawn PawnSpawner
		{
			get
			{
				return base.GetComp<CompSpawnerPawn>();
			}
		}

		// Token: 0x060081CA RID: 33226 RVA: 0x00267B98 File Offset: 0x00265D98
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!base.Spawned)
			{
				return true;
			}
			CompCanBeDormant comp = base.GetComp<CompCanBeDormant>();
			return comp != null && !comp.Awake;
		}

		// Token: 0x060081CB RID: 33227 RVA: 0x00057255 File Offset: 0x00055455
		public static void ResetStaticData()
		{
			Hive.spawnablePawnKinds.Clear();
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megascarab);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Spelopede);
			Hive.spawnablePawnKinds.Add(PawnKindDefOf.Megaspider);
		}

		// Token: 0x060081CC RID: 33228 RVA: 0x0005728E File Offset: 0x0005548E
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (base.Faction == null)
			{
				this.SetFaction(Faction.OfInsects, null);
			}
		}

		// Token: 0x060081CD RID: 33229 RVA: 0x000572AC File Offset: 0x000554AC
		public override void Tick()
		{
			base.Tick();
			if (base.Spawned && !this.CompDormant.Awake && !base.Position.Fogged(base.Map))
			{
				this.CompDormant.WakeUp();
			}
		}

		// Token: 0x060081CE RID: 33230 RVA: 0x00267BC4 File Offset: 0x00265DC4
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

		// Token: 0x060081CF RID: 33231 RVA: 0x00267C14 File Offset: 0x00265E14
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

		// Token: 0x060081D0 RID: 33232 RVA: 0x00267CBC File Offset: 0x00265EBC
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

		// Token: 0x060081D1 RID: 33233 RVA: 0x00267D50 File Offset: 0x00265F50
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

		// Token: 0x060081D2 RID: 33234 RVA: 0x00267DC0 File Offset: 0x00265FC0
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

		// Token: 0x060081D3 RID: 33235 RVA: 0x000572E7 File Offset: 0x000554E7
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

		// Token: 0x060081D4 RID: 33236 RVA: 0x00267E24 File Offset: 0x00266024
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

		// Token: 0x0400542F RID: 21551
		public const int PawnSpawnRadius = 2;

		// Token: 0x04005430 RID: 21552
		public const float MaxSpawnedPawnsPoints = 500f;

		// Token: 0x04005431 RID: 21553
		public const float InitialPawnsPoints = 200f;

		// Token: 0x04005432 RID: 21554
		public static List<PawnKindDef> spawnablePawnKinds = new List<PawnKindDef>();

		// Token: 0x04005433 RID: 21555
		public static readonly string MemoAttackedByEnemy = "HiveAttacked";

		// Token: 0x04005434 RID: 21556
		public static readonly string MemoDeSpawned = "HiveDeSpawned";

		// Token: 0x04005435 RID: 21557
		public static readonly string MemoBurnedBadly = "HiveBurnedBadly";

		// Token: 0x04005436 RID: 21558
		public static readonly string MemoDestroyedNonRoofCollapse = "HiveDestroyedNonRoofCollapse";
	}
}
