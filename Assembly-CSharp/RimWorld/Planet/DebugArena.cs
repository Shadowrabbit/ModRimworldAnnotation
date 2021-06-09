using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002190 RID: 8592
	public class DebugArena : WorldObjectComp
	{
		// Token: 0x0600B770 RID: 46960 RVA: 0x00076F2E File Offset: 0x0007512E
		public DebugArena()
		{
			this.tickCreated = Find.TickManager.TicksGame;
		}

		// Token: 0x0600B771 RID: 46961 RVA: 0x0034E964 File Offset: 0x0034CB64
		public override void CompTick()
		{
			if (this.lhs == null || this.rhs == null)
			{
				Log.ErrorOnce("DebugArena improperly set up", 73785616, false);
				return;
			}
			if ((this.tickFightStarted == 0 && Find.TickManager.TicksGame - this.tickCreated > 10000) || (this.tickFightStarted != 0 && Find.TickManager.TicksGame - this.tickFightStarted > 60000))
			{
				Log.Message("Fight timed out", false);
				ArenaUtility.ArenaResult obj = default(ArenaUtility.ArenaResult);
				obj.tickDuration = Find.TickManager.TicksGame - this.tickCreated;
				obj.winner = ArenaUtility.ArenaResult.Winner.Other;
				this.callback(obj);
				this.parent.Destroy();
				return;
			}
			if (this.tickFightStarted == 0)
			{
				foreach (Pawn pawn3 in this.lhs.Concat(this.rhs))
				{
					if (pawn3.records.GetValue(RecordDefOf.ShotsFired) > 0f || (pawn3.CurJob != null && pawn3.CurJob.def == JobDefOf.AttackMelee && pawn3.Position.DistanceTo(pawn3.CurJob.targetA.Thing.Position) <= 2f))
					{
						this.tickFightStarted = Find.TickManager.TicksGame;
						break;
					}
				}
			}
			if (this.tickFightStarted != 0)
			{
				bool flag = !this.lhs.Any((Pawn pawn) => !pawn.Dead && !pawn.Downed && pawn.Spawned);
				bool flag2 = !this.rhs.Any((Pawn pawn) => !pawn.Dead && !pawn.Downed && pawn.Spawned);
				if (flag || flag2)
				{
					ArenaUtility.ArenaResult obj2 = default(ArenaUtility.ArenaResult);
					obj2.tickDuration = Find.TickManager.TicksGame - this.tickFightStarted;
					if (flag && !flag2)
					{
						obj2.winner = ArenaUtility.ArenaResult.Winner.Rhs;
					}
					else if (!flag && flag2)
					{
						obj2.winner = ArenaUtility.ArenaResult.Winner.Lhs;
					}
					else
					{
						obj2.winner = ArenaUtility.ArenaResult.Winner.Other;
					}
					this.callback(obj2);
					foreach (Pawn pawn2 in this.lhs.Concat(this.rhs))
					{
						if (!pawn2.Destroyed)
						{
							pawn2.Destroy(DestroyMode.Vanish);
						}
					}
					this.parent.Destroy();
				}
			}
		}

		// Token: 0x04007D87 RID: 32135
		public List<Pawn> lhs;

		// Token: 0x04007D88 RID: 32136
		public List<Pawn> rhs;

		// Token: 0x04007D89 RID: 32137
		public Action<ArenaUtility.ArenaResult> callback;

		// Token: 0x04007D8A RID: 32138
		private int tickCreated;

		// Token: 0x04007D8B RID: 32139
		private int tickFightStarted;
	}
}
