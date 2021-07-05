using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F0C RID: 3852
	public class StageEndTrigger_RolesArrived : StageEndTrigger
	{
		// Token: 0x06005BD8 RID: 23512 RVA: 0x001FBDAE File Offset: 0x001F9FAE
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_TickCondition(delegate()
			{
				foreach (string b in this.roleIds)
				{
					bool flag = false;
					foreach (KeyValuePair<Pawn, PawnTags> keyValuePair in ritual.perPawnTags)
					{
						RitualRole ritualRole = ritual.RoleFor(keyValuePair.Key, true);
						if (ritualRole != null && ritualRole.id == b && keyValuePair.Value.Contains("Arrived"))
						{
							flag = true;
							break;
						}
					}
					if (!flag)
					{
						return false;
					}
				}
				if (this.clearTag)
				{
					foreach (string b2 in this.roleIds)
					{
						foreach (KeyValuePair<Pawn, PawnTags> keyValuePair2 in ritual.perPawnTags)
						{
							RitualRole ritualRole2 = ritual.RoleFor(keyValuePair2.Key, true);
							if (ritualRole2 != null && ritualRole2.id == b2 && keyValuePair2.Value.Contains("Arrived"))
							{
								keyValuePair2.Value.tags.Remove("Arrived");
							}
						}
					}
				}
				return true;
			}, 1);
		}

		// Token: 0x06005BD9 RID: 23513 RVA: 0x001FBDD4 File Offset: 0x001F9FD4
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Undefined, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.clearTag, "clearTag", false, false);
		}

		// Token: 0x0400357F RID: 13695
		public List<string> roleIds;

		// Token: 0x04003580 RID: 13696
		public bool clearTag;

		// Token: 0x04003581 RID: 13697
		public const string ArrivalTag = "Arrived";
	}
}
