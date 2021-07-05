using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017EE RID: 6126
	public class DebugArena : WorldObjectComp
	{
		// Token: 0x06008EDD RID: 36573 RVA: 0x00334002 File Offset: 0x00332202
		public DebugArena()
		{
			this.tickCreated = Find.TickManager.TicksGame;
		}

		// Token: 0x06008EDE RID: 36574 RVA: 0x0033401C File Offset: 0x0033221C
		public override void CompTick()
		{
			if (this.lhs == null || this.rhs == null)
			{
				Log.ErrorOnce("DebugArena improperly set up", 73785616);
				return;
			}
			if ((this.tickFightStarted == 0 && Find.TickManager.TicksGame - this.tickCreated > 10000) || (this.tickFightStarted != 0 && Find.TickManager.TicksGame - this.tickFightStarted > 60000))
			{
				Log.Message("Fight timed out");
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

		// Token: 0x040059FD RID: 23037
		public List<Pawn> lhs;

		// Token: 0x040059FE RID: 23038
		public List<Pawn> rhs;

		// Token: 0x040059FF RID: 23039
		public Action<ArenaUtility.ArenaResult> callback;

		// Token: 0x04005A00 RID: 23040
		private int tickCreated;

		// Token: 0x04005A01 RID: 23041
		private int tickFightStarted;
	}
}
