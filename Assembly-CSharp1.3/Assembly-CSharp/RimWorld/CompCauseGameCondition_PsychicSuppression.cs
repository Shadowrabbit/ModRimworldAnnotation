using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F3 RID: 4339
	public class CompCauseGameCondition_PsychicSuppression : CompCauseGameCondition
	{
		// Token: 0x060067E0 RID: 26592 RVA: 0x002324CD File Offset: 0x002306CD
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.gender = Gender.Male;
		}

		// Token: 0x060067E1 RID: 26593 RVA: 0x002324DD File Offset: 0x002306DD
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		// Token: 0x060067E2 RID: 26594 RVA: 0x002324F7 File Offset: 0x002306F7
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			yield return new Command_Action
			{
				defaultLabel = this.gender.GetLabel(false),
				action = delegate()
				{
					if (this.gender == Gender.Female)
					{
						this.gender = Gender.Male;
					}
					else
					{
						this.gender = Gender.Female;
					}
					base.ReSetupAllConditions();
				},
				hotKey = KeyBindingDefOf.Misc1
			};
			yield break;
		}

		// Token: 0x060067E3 RID: 26595 RVA: 0x00232508 File Offset: 0x00230708
		public override void CompTick()
		{
			base.CompTick();
			if (!base.Active || base.MyTile == -1)
			{
				return;
			}
			foreach (Caravan caravan in Find.World.worldObjects.Caravans)
			{
				if (Find.WorldGrid.ApproxDistanceInTiles(caravan.Tile, base.MyTile) < (float)base.Props.worldRange)
				{
					foreach (Pawn pawn in caravan.pawns)
					{
						GameCondition_PsychicSuppression.CheckPawn(pawn, this.gender);
					}
				}
			}
		}

		// Token: 0x060067E4 RID: 26596 RVA: 0x002325E0 File Offset: 0x002307E0
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_PsychicSuppression)condition).gender = this.gender;
		}

		// Token: 0x060067E5 RID: 26597 RVA: 0x002325FC File Offset: 0x002307FC
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("AffectedGender".Translate() + ": " + this.gender.GetLabel(false).CapitalizeFirst());
		}

		// Token: 0x060067E6 RID: 26598 RVA: 0x0023265B File Offset: 0x0023085B
		public override void RandomizeSettings(Site site)
		{
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
		}

		// Token: 0x04003A7B RID: 14971
		public Gender gender;
	}
}
