using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020016C1 RID: 5825
	public class Building_MusicalInstrument : Building
	{
		// Token: 0x06007FCA RID: 32714 RVA: 0x00055D61 File Offset: 0x00053F61
		public static bool IsAffectedByInstrument(ThingDef instrumentDef, IntVec3 instrumentPos, IntVec3 pawnPos, Map map)
		{
			return instrumentPos.DistanceTo(pawnPos) < instrumentDef.building.instrumentRange && instrumentPos.GetRoom(map, RegionType.Set_Passable) == pawnPos.GetRoom(map, RegionType.Set_Passable);
		}

		// Token: 0x170013D5 RID: 5077
		// (get) Token: 0x06007FCB RID: 32715 RVA: 0x00055D8B File Offset: 0x00053F8B
		public bool IsBeingPlayed
		{
			get
			{
				return this.currentPlayer != null;
			}
		}

		// Token: 0x170013D6 RID: 5078
		// (get) Token: 0x06007FCC RID: 32716 RVA: 0x0025E4FC File Offset: 0x0025C6FC
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

		// Token: 0x06007FCD RID: 32717 RVA: 0x00055D96 File Offset: 0x00053F96
		public void StartPlaying(Pawn player)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Musical instruments are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 19285, false);
				return;
			}
			this.currentPlayer = player;
		}

		// Token: 0x06007FCE RID: 32718 RVA: 0x0025E550 File Offset: 0x0025C750
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

		// Token: 0x06007FCF RID: 32719 RVA: 0x00055DB7 File Offset: 0x00053FB7
		public void StopPlaying()
		{
			this.currentPlayer = null;
		}

		// Token: 0x06007FD0 RID: 32720 RVA: 0x00055DC0 File Offset: 0x00053FC0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.currentPlayer, "currentPlayer", false);
		}

		// Token: 0x06007FD1 RID: 32721 RVA: 0x00055DD9 File Offset: 0x00053FD9
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Musical instruments are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 19285, false);
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

		// Token: 0x040052E3 RID: 21219
		private Pawn currentPlayer;

		// Token: 0x040052E4 RID: 21220
		private Sustainer soundPlaying;
	}
}
