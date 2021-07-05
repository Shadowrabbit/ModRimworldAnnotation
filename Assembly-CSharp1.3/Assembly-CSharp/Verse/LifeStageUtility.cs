using System;
using Verse.Sound;

namespace Verse
{
	// Token: 0x020004CC RID: 1228
	public static class LifeStageUtility
	{
		// Token: 0x0600255C RID: 9564 RVA: 0x000E9270 File Offset: 0x000E7470
		public static void PlayNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> getter, float volumeFactor = 1f)
		{
			SoundDef soundDef;
			float pitchFactor;
			float num;
			LifeStageUtility.GetNearestLifestageSound(pawn, getter, out soundDef, out pitchFactor, out num);
			if (soundDef == null)
			{
				return;
			}
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			SoundInfo info = SoundInfo.InMap(new TargetInfo(pawn.PositionHeld, pawn.MapHeld, false), MaintenanceType.None);
			info.pitchFactor = pitchFactor;
			info.volumeFactor = num * volumeFactor;
			soundDef.PlayOneShot(info);
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000E92CC File Offset: 0x000E74CC
		private static void GetNearestLifestageSound(Pawn pawn, Func<LifeStageAge, SoundDef> getter, out SoundDef def, out float pitch, out float volume)
		{
			int num = pawn.ageTracker.CurLifeStageIndex;
			LifeStageAge lifeStageAge;
			for (;;)
			{
				lifeStageAge = pawn.RaceProps.lifeStageAges[num];
				def = getter(lifeStageAge);
				if (def != null)
				{
					break;
				}
				num++;
				if (num < 0 || num >= pawn.RaceProps.lifeStageAges.Count)
				{
					goto IL_84;
				}
			}
			pitch = pawn.ageTracker.CurLifeStage.voxPitch / lifeStageAge.def.voxPitch;
			volume = pawn.ageTracker.CurLifeStage.voxVolume / lifeStageAge.def.voxVolume;
			return;
			IL_84:
			def = null;
			pitch = (volume = 1f);
		}
	}
}
