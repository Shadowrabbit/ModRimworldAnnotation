using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001782 RID: 6018
	public class CompCauseGameCondition_PsychicSuppression : CompCauseGameCondition
	{
		// Token: 0x060084B1 RID: 33969 RVA: 0x00058E64 File Offset: 0x00057064
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.gender = Gender.Male;
		}

		// Token: 0x060084B2 RID: 33970 RVA: 0x00058E74 File Offset: 0x00057074
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.None, false);
		}

		// Token: 0x060084B3 RID: 33971 RVA: 0x00058E8E File Offset: 0x0005708E
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

		// Token: 0x060084B4 RID: 33972 RVA: 0x002744A4 File Offset: 0x002726A4
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

		// Token: 0x060084B5 RID: 33973 RVA: 0x00058E9E File Offset: 0x0005709E
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_PsychicSuppression)condition).gender = this.gender;
		}

		// Token: 0x060084B6 RID: 33974 RVA: 0x0027457C File Offset: 0x0027277C
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("AffectedGender".Translate() + ": " + this.gender.GetLabel(false).CapitalizeFirst());
		}

		// Token: 0x060084B7 RID: 33975 RVA: 0x00058EB9 File Offset: 0x000570B9
		public override void RandomizeSettings_NewTemp_NewTemp(Site site)
		{
			this.gender = (Rand.Bool ? Gender.Male : Gender.Female);
		}

		// Token: 0x040055EF RID: 21999
		public Gender gender;
	}
}
