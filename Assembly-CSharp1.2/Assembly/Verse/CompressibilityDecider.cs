using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	// Token: 0x0200023F RID: 575
	public class CompressibilityDecider
	{
		// Token: 0x06000EBF RID: 3775 RVA: 0x00011173 File Offset: 0x0000F373
		public CompressibilityDecider(Map map)
		{
			this.map = map;
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x000B3D4C File Offset: 0x000B1F4C
		public void DetermineReferences()
		{
			this.referencedThings.Clear();
			foreach (Thing item in from des in this.map.designationManager.allDesignations
			select des.target.Thing)
			{
				this.referencedThings.Add(item);
			}
			foreach (Thing item2 in this.map.reservationManager.AllReservedThings())
			{
				this.referencedThings.Add(item2);
			}
			List<Pawn> allPawnsSpawned = this.map.mapPawns.AllPawnsSpawned;
			for (int i = 0; i < allPawnsSpawned.Count; i++)
			{
				Job curJob = allPawnsSpawned[i].jobs.curJob;
				if (curJob != null)
				{
					if (curJob.targetA.HasThing)
					{
						this.referencedThings.Add(curJob.targetA.Thing);
					}
					if (curJob.targetB.HasThing)
					{
						this.referencedThings.Add(curJob.targetB.Thing);
					}
					if (curJob.targetC.HasThing)
					{
						this.referencedThings.Add(curJob.targetC.Thing);
					}
				}
			}
			List<Thing> list = this.map.listerThings.ThingsInGroup(ThingRequestGroup.Projectile);
			for (int j = 0; j < list.Count; j++)
			{
				Projectile projectile = (Projectile)list[j];
				if (projectile.usedTarget.HasThing)
				{
					this.referencedThings.Add(projectile.usedTarget.Thing);
				}
				if (projectile.intendedTarget.HasThing)
				{
					this.referencedThings.Add(projectile.intendedTarget.Thing);
				}
			}
			List<Lord> lords = this.map.lordManager.lords;
			for (int k = 0; k < lords.Count; k++)
			{
				LordJob_FormAndSendCaravan lordJob_FormAndSendCaravan = lords[k].LordJob as LordJob_FormAndSendCaravan;
				if (lordJob_FormAndSendCaravan != null)
				{
					for (int l = 0; l < lordJob_FormAndSendCaravan.transferables.Count; l++)
					{
						TransferableOneWay transferableOneWay = lordJob_FormAndSendCaravan.transferables[l];
						for (int m = 0; m < transferableOneWay.things.Count; m++)
						{
							this.referencedThings.Add(transferableOneWay.things[m]);
						}
					}
				}
			}
			List<Thing> list2 = this.map.listerThings.ThingsInGroup(ThingRequestGroup.Transporter);
			for (int n = 0; n < list2.Count; n++)
			{
				CompTransporter compTransporter = list2[n].TryGetComp<CompTransporter>();
				if (compTransporter.leftToLoad != null)
				{
					for (int num = 0; num < compTransporter.leftToLoad.Count; num++)
					{
						TransferableOneWay transferableOneWay2 = compTransporter.leftToLoad[num];
						for (int num2 = 0; num2 < transferableOneWay2.things.Count; num2++)
						{
							this.referencedThings.Add(transferableOneWay2.things[num2]);
						}
					}
				}
			}
		}

		// Token: 0x06000EC1 RID: 3777 RVA: 0x0001118D File Offset: 0x0000F38D
		public bool IsReferenced(Thing th)
		{
			return this.referencedThings.Contains(th);
		}

		// Token: 0x04000C24 RID: 3108
		private Map map;

		// Token: 0x04000C25 RID: 3109
		private HashSet<Thing> referencedThings = new HashSet<Thing>();
	}
}
