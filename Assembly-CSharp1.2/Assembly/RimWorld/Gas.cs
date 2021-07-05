using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001703 RID: 5891
	public class Gas : Thing
	{
		// Token: 0x0600819A RID: 33178 RVA: 0x002676DC File Offset: 0x002658DC
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			for (;;)
			{
				Thing gas = base.Position.GetGas(map);
				if (gas == null)
				{
					break;
				}
				gas.Destroy(DestroyMode.Vanish);
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.destroyTick = Find.TickManager.TicksGame + this.def.gas.expireSeconds.RandomInRange.SecondsToTicks();
			}
			this.graphicRotationSpeed = Rand.Range(-this.def.gas.rotationSpeed, this.def.gas.rotationSpeed) / 60f;
		}

		// Token: 0x0600819B RID: 33179 RVA: 0x000570BF File Offset: 0x000552BF
		public override void Tick()
		{
			if (this.destroyTick <= Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.graphicRotation += this.graphicRotationSpeed;
		}

		// Token: 0x0600819C RID: 33180 RVA: 0x000570ED File Offset: 0x000552ED
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}

		// Token: 0x0400541C RID: 21532
		public int destroyTick;

		// Token: 0x0400541D RID: 21533
		public float graphicRotation;

		// Token: 0x0400541E RID: 21534
		public float graphicRotationSpeed;
	}
}
