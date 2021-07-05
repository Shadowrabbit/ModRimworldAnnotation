using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C77 RID: 3191
	public class DangerWatcher
	{
		// Token: 0x17000CE3 RID: 3299
		// (get) Token: 0x06004A67 RID: 19047 RVA: 0x00189C95 File Offset: 0x00187E95
		public StoryDanger DangerRating
		{
			get
			{
				if (Find.TickManager.TicksGame > this.lastUpdateTick + 101)
				{
					this.dangerRatingInt = this.CalculateDangerRating();
					this.lastUpdateTick = Find.TickManager.TicksGame;
				}
				return this.dangerRatingInt;
			}
		}

		// Token: 0x06004A68 RID: 19048 RVA: 0x00189CD0 File Offset: 0x00187ED0
		private StoryDanger CalculateDangerRating()
		{
			float num = (from x in this.map.attackTargetsCache.TargetsHostileToColony
			where this.AffectsStoryDanger(x)
			select x).Sum(delegate(IAttackTarget t)
			{
				Pawn pawn;
				if ((pawn = (t as Pawn)) != null)
				{
					return pawn.kindDef.combatPower;
				}
				Building_TurretGun building_TurretGun;
				if ((building_TurretGun = (t as Building_TurretGun)) != null && building_TurretGun.def.building.IsMortar && !building_TurretGun.IsMannable)
				{
					return building_TurretGun.def.building.combatPower;
				}
				return 0f;
			});
			if (num == 0f)
			{
				return StoryDanger.None;
			}
			int num2 = (from p in this.map.mapPawns.FreeColonistsSpawned
			where !p.Downed
			select p).Count<Pawn>();
			if (num < 150f && num <= (float)num2 * 18f)
			{
				return StoryDanger.Low;
			}
			if (num > 400f)
			{
				return StoryDanger.High;
			}
			if (this.lastColonistHarmedTick > Find.TickManager.TicksGame - 900)
			{
				return StoryDanger.High;
			}
			foreach (Lord lord in this.map.lordManager.lords)
			{
				if (lord.faction.HostileTo(Faction.OfPlayer) && lord.CurLordToil.ForceHighStoryDanger && lord.AnyActivePawn)
				{
					return StoryDanger.High;
				}
			}
			return StoryDanger.Low;
		}

		// Token: 0x06004A69 RID: 19049 RVA: 0x00189E1C File Offset: 0x0018801C
		public DangerWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x06004A6A RID: 19050 RVA: 0x00189E41 File Offset: 0x00188041
		public void Notify_ColonistHarmedExternally()
		{
			this.lastColonistHarmedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06004A6B RID: 19051 RVA: 0x00189E54 File Offset: 0x00188054
		private bool AffectsStoryDanger(IAttackTarget t)
		{
			Pawn pawn = t.Thing as Pawn;
			if (pawn != null)
			{
				Lord lord = pawn.GetLord();
				if (lord != null && (lord.LordJob is LordJob_DefendPoint || lord.LordJob is LordJob_MechanoidDefendBase) && pawn.CurJobDef != JobDefOf.AttackMelee && pawn.CurJobDef != JobDefOf.AttackStatic)
				{
					return false;
				}
				CompCanBeDormant comp = pawn.GetComp<CompCanBeDormant>();
				if (comp != null && !comp.Awake)
				{
					return false;
				}
			}
			return GenHostility.IsActiveThreatToPlayer(t);
		}

		// Token: 0x04002D33 RID: 11571
		private Map map;

		// Token: 0x04002D34 RID: 11572
		private StoryDanger dangerRatingInt;

		// Token: 0x04002D35 RID: 11573
		private int lastUpdateTick = -10000;

		// Token: 0x04002D36 RID: 11574
		private int lastColonistHarmedTick = -10000;

		// Token: 0x04002D37 RID: 11575
		private const int UpdateInterval = 101;
	}
}
