using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FC6 RID: 4038
	public class CompRitualEffect_Drum : CompRitualEffect_IntervalSpawn
	{
		// Token: 0x06005F17 RID: 24343 RVA: 0x000FE248 File Offset: 0x000FC448
		protected override Vector3 SpawnPos(LordJob_Ritual ritual)
		{
			return Vector3.zero;
		}

		// Token: 0x17001056 RID: 4182
		// (get) Token: 0x06005F18 RID: 24344 RVA: 0x002087E4 File Offset: 0x002069E4
		protected new CompProperties_RitualEffectDrum Props
		{
			get
			{
				return (CompProperties_RitualEffectDrum)this.props;
			}
		}

		// Token: 0x06005F19 RID: 24345 RVA: 0x002087F4 File Offset: 0x002069F4
		public override void SpawnFleck(LordJob_Ritual ritual, Vector3? forcedPos = null, float? exactRotation = null)
		{
			foreach (Thing thing in ritual.Map.listerBuldingOfDefInProximity.GetForCell(ritual.selectedTarget.Cell, (float)this.Props.maxDistance, ThingDefOf.Drum, null))
			{
				Building_MusicalInstrument building_MusicalInstrument = thing as Building_MusicalInstrument;
				if (building_MusicalInstrument != null && thing.GetRoom(RegionType.Set_All) == ritual.selectedTarget.Cell.GetRoom(ritual.Map) && building_MusicalInstrument.IsBeingPlayed)
				{
					for (int i = 0; i < this.Props.spawnCount; i++)
					{
						float num = (float)Rand.Sign;
						float num2 = (float)Rand.Sign;
						Vector3 b = new Vector3(num * Rand.Value * this.Props.maxOffset, 0f, num2 * Rand.Value * this.Props.maxOffset);
						base.SpawnFleck(ritual, new Vector3?(thing.Position.ToVector3Shifted() + b), null);
					}
				}
			}
			this.burstsDone++;
			this.lastSpawnTick = GenTicks.TicksGame;
		}
	}
}
