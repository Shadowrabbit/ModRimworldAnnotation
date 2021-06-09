using System;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000114 RID: 276
	[StaticConstructorOnStartup]
	public class LifeStageAge
	{
		// Token: 0x06000782 RID: 1922 RVA: 0x000924FC File Offset: 0x000906FC
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

		// Token: 0x040004E4 RID: 1252
		public LifeStageDef def;

		// Token: 0x040004E5 RID: 1253
		public float minAge;

		// Token: 0x040004E6 RID: 1254
		public SoundDef soundCall;

		// Token: 0x040004E7 RID: 1255
		public SoundDef soundAngry;

		// Token: 0x040004E8 RID: 1256
		public SoundDef soundWounded;

		// Token: 0x040004E9 RID: 1257
		public SoundDef soundDeath;

		// Token: 0x040004EA RID: 1258
		private static readonly Texture2D VeryYoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/VeryYoung", true);

		// Token: 0x040004EB RID: 1259
		private static readonly Texture2D YoungIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Young", true);

		// Token: 0x040004EC RID: 1260
		private static readonly Texture2D AdultIcon = ContentFinder<Texture2D>.Get("UI/Icons/LifeStage/Adult", true);
	}
}
