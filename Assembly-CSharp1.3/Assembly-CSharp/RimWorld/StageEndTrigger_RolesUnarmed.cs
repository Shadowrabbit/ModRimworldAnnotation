using System;
using System.Collections.Generic;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000F0E RID: 3854
	public class StageEndTrigger_RolesUnarmed : StageEndTrigger
	{
		// Token: 0x06005BDE RID: 23518 RVA: 0x001FBE5C File Offset: 0x001FA05C
		public override Trigger MakeTrigger(LordJob_Ritual ritual, TargetInfo spot, IEnumerable<TargetInfo> foci, RitualStage stage)
		{
			return new Trigger_Custom(delegate(TriggerSignal signal)
			{
				foreach (string roleId in this.roleIds)
				{
					using (List<ThingWithComps>.Enumerator enumerator2 = ritual.PawnWithRole(roleId).equipment.AllEquipmentListForReading.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.def.IsWeapon)
							{
								return false;
							}
						}
					}
				}
				return true;
			});
		}

		// Token: 0x06005BDF RID: 23519 RVA: 0x001FBE81 File Offset: 0x001FA081
		public override void ExposeData()
		{
			Scribe_Collections.Look<string>(ref this.roleIds, "roleIds", LookMode.Value, Array.Empty<object>());
		}

		// Token: 0x04003583 RID: 13699
		[NoTranslate]
		public List<string> roleIds;
	}
}
