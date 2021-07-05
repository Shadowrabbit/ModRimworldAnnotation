using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020010AB RID: 4267
	public class Gas : Thing
	{
		// Token: 0x060065D8 RID: 26072 RVA: 0x00226AC0 File Offset: 0x00224CC0
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

		// Token: 0x060065D9 RID: 26073 RVA: 0x00226B4E File Offset: 0x00224D4E
		public override void Tick()
		{
			if (this.destroyTick <= Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.graphicRotation += this.graphicRotationSpeed;
		}

		// Token: 0x060065DA RID: 26074 RVA: 0x00226B7C File Offset: 0x00224D7C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}

		// Token: 0x04003983 RID: 14723
		public int destroyTick;

		// Token: 0x04003984 RID: 14724
		public float graphicRotation;

		// Token: 0x04003985 RID: 14725
		public float graphicRotationSpeed;
	}
}
