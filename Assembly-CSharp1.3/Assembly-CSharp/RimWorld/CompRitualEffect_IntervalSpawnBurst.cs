using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FBA RID: 4026
	public abstract class CompRitualEffect_IntervalSpawnBurst : CompRitualEffect_IntervalSpawn
	{
		// Token: 0x06005EFB RID: 24315 RVA: 0x002080E4 File Offset: 0x002062E4
		public override Mote SpawnMote(LordJob_Ritual ritual, Vector3? forcedPos = null)
		{
			for (int i = 0; i < base.Props.spawnCount; i++)
			{
				base.SpawnMote(ritual, null);
			}
			this.lastSpawnTick = GenTicks.TicksGame;
			this.burstsDone++;
			return null;
		}

		// Token: 0x06005EFC RID: 24316 RVA: 0x00208134 File Offset: 0x00206334
		public override void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			for (int i = 0; i < base.Props.spawnCount; i++)
			{
				base.SpawnFleck(ritual, null, null);
			}
			this.lastSpawnTick = GenTicks.TicksGame;
			this.burstsDone++;
		}
	}
}
