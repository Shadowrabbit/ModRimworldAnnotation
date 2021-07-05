using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F0F RID: 3855
	public class StageEndTrigger_AnyPawnDead : StageEndTrigger
	{
		// Token: 0x06005BE1 RID: 23521 RVA: 0x001FBE9C File Offset: 0x001FA09C
		protected virtual bool Trigger(LordJob_Ritual ritual)
		{
			foreach (string roleId in this.roleIds)
			{
				using (IEnumerator<Pawn> enumerator2 = ritual.assignments.AssignedPawns(roleId).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						if (enumerator2.Current.Dead)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06005BE2 RID: 23522 RVA: 0x001FBF30 File Offset: 0x001FA130
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_Custom((TriggerSignal signal) => this.Trigger(ritual));
		}

		// Token: 0x06005BE3 RID: 23523 RVA: 0x001FBF55 File Offset: 0x001FA155
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x04003584 RID: 13700
		[NoTranslate]
		public List<string> roleIds;
	}
}
