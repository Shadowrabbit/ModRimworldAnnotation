using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020014DB RID: 5339
	public class Need_Authority : Need
	{
		// Token: 0x17001198 RID: 4504
		// (get) Token: 0x0600731C RID: 29468 RVA: 0x0004D6CE File Offset: 0x0004B8CE
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

		// Token: 0x17001199 RID: 4505
		// (get) Token: 0x0600731D RID: 29469 RVA: 0x00232914 File Offset: 0x00230B14
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

		// Token: 0x1700119A RID: 4506
		// (get) Token: 0x0600731E RID: 29470 RVA: 0x00232968 File Offset: 0x00230B68
		public bool IsActive
		{
			get
			{
				return this.pawn.royalty != null && this.pawn.Spawned && this.pawn.Map != null && this.pawn.Map.IsPlayerHome && this.pawn.royalty.CanRequireThroneroom();
			}
		}

		// Token: 0x1700119B RID: 4507
		// (get) Token: 0x0600731F RID: 29471 RVA: 0x0004D6ED File Offset: 0x0004B8ED
		protected override bool IsFrozen
		{
			get
			{
				return this.pawn.Map == null || !this.pawn.Map.IsPlayerHome || this.FallPerDay <= 0f;
			}
		}

		// Token: 0x1700119C RID: 4508
		// (get) Token: 0x06007320 RID: 29472 RVA: 0x002329C8 File Offset: 0x00230BC8
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

		// Token: 0x1700119D RID: 4509
		// (get) Token: 0x06007321 RID: 29473 RVA: 0x0004D720 File Offset: 0x0004B920
		public override bool ShowOnNeedList
		{
			get
			{
				return this.IsActive;
			}
		}

		// Token: 0x1700119E RID: 4510
		// (get) Token: 0x06007322 RID: 29474 RVA: 0x0004D728 File Offset: 0x0004B928
		public bool IsCurrentlyReigning
		{
			get
			{
				return this.pawn.CurJobDef == JobDefOf.Reign;
			}
		}

		// Token: 0x1700119F RID: 4511
		// (get) Token: 0x06007323 RID: 29475 RVA: 0x0004D73C File Offset: 0x0004B93C
		public bool IsCurrentlyGivingSpeech
		{
			get
			{
				return this.pawn.CurJobDef == JobDefOf.GiveSpeech;
			}
		}

		// Token: 0x06007324 RID: 29476 RVA: 0x00232AA0 File Offset: 0x00230CA0
		public Need_Authority(Pawn pawn) : base(pawn)
		{
		}

		// Token: 0x06007325 RID: 29477 RVA: 0x00232B04 File Offset: 0x00230D04
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

		// Token: 0x04004BCA RID: 19402
		public const float LevelGainPerDayOfReigning = 2f;

		// Token: 0x04004BCB RID: 19403
		public const float LevelGainPerDayOfGivingSpeech = 3f;

		// Token: 0x04004BCC RID: 19404
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
