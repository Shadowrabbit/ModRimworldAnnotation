using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020000AE RID: 174
	[StaticConstructorOnStartup]
	public class LifeStageAge
	{
		// Token: 0x06000561 RID: 1377 RVA: 0x0001BD28 File Offset: 0x00019F28
		public Texture2D GetIcon(Pawn forPawn)
		{
			if (this.def.iconTex != null)
			{
				return this.def.iconTex;
			}
			int count = forPawn.RaceProps.lifeStageAges.Count;
			int num = forPawn.RaceProps.lifeStageAges.IndexOf(this);
			if (num == count - 1)
			{
				return LifeStageAge.AdultIcon;
			}
			if (num == 0)
			{
				return LifeStageAge.VeryYoungIcon;
			}
			return LifeStageAge.YoungIcon;
		}

		// Token: 0x04000302 RID: 770
		public LifeStageDef def;

		// Token: 0x04000303 RID: 771
		public float minAge;

		// Token: 0x04000304 RID: 772
		public SoundDef soundCall;

		// Token: 0x04000305 RID: 773
		public SoundDef soundAngry;

		// Token: 0x04000306 RID: 774
		public SoundDef soundWounded;

		// Token: 0x04000307 RID: 775
		public SoundDef soundDeath;

		// Token: 0x04000308 RID: 776
		private static readonly Texture2D VeryYoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/VeryYoung", true);

		// Token: 0x04000309 RID: 777
		private static readonly Texture2D YoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Young", true);

		// Token: 0x0400030A RID: 778
		private static readonly Texture2D AdultIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Adult", true);
	}
}
