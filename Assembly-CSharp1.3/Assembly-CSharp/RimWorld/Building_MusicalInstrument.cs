using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x0200107E RID: 4222
	public class Building_MusicalInstrument : Building
	{
		// Token: 0x0600646F RID: 25711 RVA: 0x0021DF49 File Offset: 0x0021C149
		public static bool IsAffectedByInstrument(ThingDef instrumentDef, IntVec3 instrumentPos, IntVec3 pawnPos, Map map)
		{
			return instrumentPos.DistanceTo(pawnPos) < instrumentDef.building.instrumentRange && instrumentPos.GetRoom(map) == pawnPos.GetRoom(map);
		}

		// Token: 0x17001130 RID: 4400
		// (get) Token: 0x06006470 RID: 25712 RVA: 0x0021DF71 File Offset: 0x0021C171
		public bool IsBeingPlayed
		{
			get
			{
				return this.currentPlayer != null;
			}
		}

		// Token: 0x17001131 RID: 4401
		// (get) Token: 0x06006471 RID: 25713 RVA: 0x0021DF7C File Offset: 0x0021C17C
		public FloatRange SoundRange
		{
			get
			{
				if (this.soundPlaying == null)
				{
					return FloatRange.Zero;
				}
				if (this.soundPlaying.def.subSounds.NullOrEmpty<SubSoundDef>())
				{
					return FloatRange.Zero;
				}
				return this.soundPlaying.def.subSounds.First<SubSoundDef>().distRange;
			}
		}

		// Token: 0x06006472 RID: 25714 RVA: 0x0021DFCE File Offset: 0x0021C1CE
		public void StartPlaying(Pawn player)
		{
			if (!ModLister.CheckRoyalty("Musical instrument"))
			{
				return;
			}
			this.currentPlayer = player;
		}

		// Token: 0x06006473 RID: 25715 RVA: 0x0021DFE4 File Offset: 0x0021C1E4
		public override void Tick()
		{
			base.Tick();
			if (this.currentPlayer != null)
			{
				if (this.def.soundPlayInstrument != null && this.soundPlaying == null)
				{
					this.soundPlaying = this.def.soundPlayInstrument.TrySpawnSustainer(SoundInfo.InMap(new TargetInfo(base.Position, base.Map, false), MaintenanceType.PerTick));
				}
			}
			else
			{
				this.soundPlaying = null;
			}
			if (this.soundPlaying != null)
			{
				this.soundPlaying.Maintain();
			}
		}

		// Token: 0x06006474 RID: 25716 RVA: 0x0021E05E File Offset: 0x0021C25E
		public void StopPlaying()
		{
			this.currentPlayer = null;
		}

		// Token: 0x06006475 RID: 25717 RVA: 0x0021E067 File Offset: 0x0021C267
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.currentPlayer, "currentPlayer", false);
		}

		// Token: 0x06006476 RID: 25718 RVA: 0x0021E080 File Offset: 0x0021C280
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!ModLister.CheckRoyalty("Musical instrument"))
			{
				yield break;
			}
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (Prefs.DevMode)
			{
				yield return new Command_Action
				{
					defaultLabel = "Debug: Toggle is playing",
					action = delegate()
					{
						this.currentPlayer = ((this.currentPlayer == null) ? PawnsFinder.AllMaps_FreeColonists.FirstOrDefault<Pawn>() : null);
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x040038A0 RID: 14496
		private Pawn currentPlayer;

		// Token: 0x040038A1 RID: 14497
		private Sustainer soundPlaying;
	}
}
