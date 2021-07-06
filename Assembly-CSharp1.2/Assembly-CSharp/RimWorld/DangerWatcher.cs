using System;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001261 RID: 4705
	public class DangerWatcher
	{
		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x06006696 RID: 26262 RVA: 0x00046191 File Offset: 0x00044391
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

		// Token: 0x06006697 RID: 26263 RVA: 0x001F993C File Offset: 0x001F7B3C
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

		// Token: 0x06006698 RID: 26264 RVA: 0x000461CA File Offset: 0x000443CA
		public DangerWatcher(Map map)
		{
			this.map = map;
		}

		// Token: 0x06006699 RID: 26265 RVA: 0x000461EF File Offset: 0x000443EF
		public void Notify_ColonistHarmedExternally()
		{
			this.lastColonistHarmedTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600669A RID: 26266 RVA: 0x001F9A88 File Offset: 0x001F7C88
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

		// Token: 0x04004449 RID: 17481
		private Map map;

		// Token: 0x0400444A RID: 17482
		private StoryDanger dangerRatingInt;

		// Token: 0x0400444B RID: 17483
		private int lastUpdateTick = -10000;

		// Token: 0x0400444C RID: 17484
		private int lastColonistHarmedTick = -10000;

		// Token: 0x0400444D RID: 17485
		private const int UpdateInterval = 101;
	}
}
