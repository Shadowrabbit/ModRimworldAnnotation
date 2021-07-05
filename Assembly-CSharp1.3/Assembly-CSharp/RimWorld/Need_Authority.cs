using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3E RID: 3646
	public class Need_Authority : Need
	{
		// Token: 0x17000E66 RID: 3686
		// (get) Token: 0x0600547C RID: 21628 RVA: 0x001CA2E6 File Offset: 0x001C84E6
		public override int GUIChangeArrow
		{
			get
			{
				if (this.IsFrozen)
				{
					return 0;
				}
				if (this.IsCurrentlyReigning || this.IsCurrentlyGivingSpeech)
				{
					return 1;
				}
				return -1;
			}
		}

		// Token: 0x17000E67 RID: 3687
		// (get) Token: 0x0600547D RID: 21629 RVA: 0x001CA308 File Offset: 0x001C8508
		public AuthorityCategory CurCategory
		{
			get
			{
				float curLevel = this.CurLevel;
				if (curLevel < 0.01f)
				{
					return AuthorityCategory.Gone;
				}
				if (curLevel < 0.15f)
				{
					return AuthorityCategory.Weak;
				}
				if (curLevel < 0.3f)
				{
					return AuthorityCategory.Uncertain;
				}
				if (curLevel > 0.7f && curLevel < 0.85f)
				{
					return AuthorityCategory.Strong;
				}
				if (curLevel >= 0.85f)
				{
					return AuthorityCategory.Total;
				}
				return AuthorityCategory.Normal;
			}
		}

		// Token: 0x17000E68 RID: 3688
		// (get) Token: 0x0600547E RID: 21630 RVA: 0x001CA35C File Offset: 0x001C855C
		public bool IsActive
		{
			get
			{
				return this.pawn.royalty != null && this.pawn.Spawned && this.pawn.Map != null && this.pawn.Map.IsPlayerHome && this.pawn.royalty.CanRequireThroneroom();
			}
		}

		// Token: 0x17000E69 RID: 3689
		// (get) Token: 0x0600547F RID: 21631 RVA: 0x001CA3BB File Offset: 0x001C85BB
		protected override bool IsFrozen
		{
			get
			{
				return this.pawn.Map == null || !this.pawn.Map.IsPlayerHome || this.FallPerDay <= 0f;
			}
		}

		// Token: 0x17000E6A RID: 3690
		// (get) Token: 0x06005480 RID: 21632 RVA: 0x001CA3F0 File Offset: 0x001C85F0
		public float FallPerDay
		{
			get
			{
				if (this.pawn.royalty == null || !this.pawn.Spawned)
				{
					return 0f;
				}
				if (this.pawn.Map == null || !this.pawn.Map.IsPlayerHome)
				{
					return 0f;
				}
				float num = 0f;
				foreach (RoyalTitle royalTitle in this.pawn.royalty.AllTitlesInEffectForReading)
				{
				}
				int num2 = this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction).Count<Pawn>();
				return num * this.FallFactorCurve.Evaluate((float)num2);
			}
		}

		// Token: 0x17000E6B RID: 3691
		// (get) Token: 0x06005481 RID: 21633 RVA: 0x001CA4C8 File Offset: 0x001C86C8
		public override bool ShowOnNeedList
		{
			get
			{
				return this.IsActive;
			}
		}

		// Token: 0x17000E6C RID: 3692
		// (get) Token: 0x06005482 RID: 21634 RVA: 0x001CA4D0 File Offset: 0x001C86D0
		public bool IsCurrentlyReigning
		{
			get
			{
				return this.pawn.CurJobDef == JobDefOf.Reign;
			}
		}

		// Token: 0x17000E6D RID: 3693
		// (get) Token: 0x06005483 RID: 21635 RVA: 0x001CA4E4 File Offset: 0x001C86E4
		public bool IsCurrentlyGivingSpeech
		{
			get
			{
				return this.pawn.CurJobDef == JobDefOf.GiveSpeech;
			}
		}

		// Token: 0x06005484 RID: 21636 RVA: 0x001CA4F8 File Offset: 0x001C86F8
		public Need_Authority(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x06005485 RID: 21637 RVA: 0x001CA55C File Offset: 0x001C875C
		public override void NeedInterval()
		{
			float num = 400f;
			float num2 = this.FallPerDay / num;
			if (this.IsFrozen)
			{
				this.CurLevel = 1f;
				return;
			}
			if (this.pawn.Map.mapPawns.SpawnedPawnsInFaction(this.pawn.Faction).Count <= 1)
			{
				this.SetInitialLevel();
				return;
			}
			if (this.IsCurrentlyReigning)
			{
				this.CurLevel += 2f / num;
				return;
			}
			if (this.IsCurrentlyGivingSpeech)
			{
				this.CurLevel += 3f / num;
				return;
			}
			this.CurLevel -= num2;
		}

		// Token: 0x040031C8 RID: 12744
		public const float LevelGainPerDayOfReigning = 2f;

		// Token: 0x040031C9 RID: 12745
		public const float LevelGainPerDayOfGivingSpeech = 3f;

		// Token: 0x040031CA RID: 12746
		private readonly SimpleCurve FallFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(1f, 0f),
				true
			},
			{
				new CurvePoint(3f, 0.5f),
				true
			},
			{
				new CurvePoint(5f, 1f),
				true
			}
		};
	}
}
