using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020016BA RID: 5818
	[StaticConstructorOnStartup]
	public class Building_FermentingBarrel : Building
	{
		// Token: 0x170013C3 RID: 5059
		// (get) Token: 0x06007F82 RID: 32642 RVA: 0x00055A10 File Offset: 0x00053C10
		// (set) Token: 0x06007F83 RID: 32643 RVA: 0x00055A18 File Offset: 0x00053C18
		public float Progress
		{
			get
			{
				return this.progressInt;
			}
			set
			{
				if (value == this.progressInt)
				{
					return;
				}
				this.progressInt = value;
				this.barFilledCachedMat = null;
			}
		}

		// Token: 0x170013C4 RID: 5060
		// (get) Token: 0x06007F84 RID: 32644 RVA: 0x00055A32 File Offset: 0x00053C32
		private Material BarFilledMat
		{
			get
			{
				if (this.barFilledCachedMat == null)
				{
					this.barFilledCachedMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.Lerp(Building_FermentingBarrel.BarZeroProgressColor, Building_FermentingBarrel.BarFermentedColor, this.Progress), false);
				}
				return this.barFilledCachedMat;
			}
		}

		// Token: 0x170013C5 RID: 5061
		// (get) Token: 0x06007F85 RID: 32645 RVA: 0x00055A69 File Offset: 0x00053C69
		public int SpaceLeftForWort
		{
			get
			{
				if (this.Fermented)
				{
					return 0;
				}
				return 25 - this.wortCount;
			}
		}

		// Token: 0x170013C6 RID: 5062
		// (get) Token: 0x06007F86 RID: 32646 RVA: 0x00055A7E File Offset: 0x00053C7E
		private bool Empty
		{
			get
			{
				return this.wortCount <= 0;
			}
		}

		// Token: 0x170013C7 RID: 5063
		// (get) Token: 0x06007F87 RID: 32647 RVA: 0x00055A8C File Offset: 0x00053C8C
		public bool Fermented
		{
			get
			{
				return !this.Empty && this.Progress >= 1f;
			}
		}

		// Token: 0x170013C8 RID: 5064
		// (get) Token: 0x06007F88 RID: 32648 RVA: 0x0025D690 File Offset: 0x0025B890
		private float CurrentTempProgressSpeedFactor
		{
			get
			{
				CompProperties_TemperatureRuinable compProperties = this.def.GetCompProperties<CompProperties_TemperatureRuinable>();
				float ambientTemperature = base.AmbientTemperature;
				if (ambientTemperature < compProperties.minSafeTemperature)
				{
					return 0.1f;
				}
				if (ambientTemperature < 7f)
				{
					return GenMath.LerpDouble(compProperties.minSafeTemperature, 7f, 0.1f, 1f, ambientTemperature);
				}
				return 1f;
			}
		}

		// Token: 0x170013C9 RID: 5065
		// (get) Token: 0x06007F89 RID: 32649 RVA: 0x00055AA8 File Offset: 0x00053CA8
		private float ProgressPerTickAtCurrentTemp
		{
			get
			{
				return 2.7777778E-06f * this.CurrentTempProgressSpeedFactor;
			}
		}

		// Token: 0x170013CA RID: 5066
		// (get) Token: 0x06007F8A RID: 32650 RVA: 0x00055AB6 File Offset: 0x00053CB6
		private int EstimatedTicksLeft
		{
			get
			{
				return Mathf.Max(Mathf.RoundToInt((1f - this.Progress) / this.ProgressPerTickAtCurrentTemp), 0);
			}
		}

		// Token: 0x06007F8B RID: 32651 RVA: 0x00055AD6 File Offset: 0x00053CD6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.wortCount, "wortCount", 0, false);
			Scribe_Values.Look<float>(ref this.progressInt, "progress", 0f, false);
		}

		// Token: 0x06007F8C RID: 32652 RVA: 0x00055B06 File Offset: 0x00053D06
		public override void TickRare()
		{
			base.TickRare();
			if (!this.Empty)
			{
				this.Progress = Mathf.Min(this.Progress + 250f * this.ProgressPerTickAtCurrentTemp, 1f);
			}
		}

		// Token: 0x06007F8D RID: 32653 RVA: 0x0025D6E8 File Offset: 0x0025B8E8
		public void AddWort(int count)
		{
			base.GetComp<CompTemperatureRuinable>().Reset();
			if (this.Fermented)
			{
				Log.Warning("Tried to add wort to a barrel full of beer. Colonists should take the beer first.", false);
				return;
			}
			int num = Mathf.Min(count, 25 - this.wortCount);
			if (num <= 0)
			{
				return;
			}
			this.Progress = GenMath.WeightedAverage(0f, (float)num, this.Progress, (float)this.wortCount);
			this.wortCount += num;
		}

		// Token: 0x06007F8E RID: 32654 RVA: 0x00055B39 File Offset: 0x00053D39
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "RuinedByTemperature")
			{
				this.Reset();
			}
		}

		// Token: 0x06007F8F RID: 32655 RVA: 0x00055B4E File Offset: 0x00053D4E
		private void Reset()
		{
			this.wortCount = 0;
			this.Progress = 0f;
		}

		// Token: 0x06007F90 RID: 32656 RVA: 0x0025D758 File Offset: 0x0025B958
		public void AddWort(Thing wort)
		{
			int num = Mathf.Min(wort.stackCount, 25 - this.wortCount);
			if (num > 0)
			{
				this.AddWort(num);
				wort.SplitOff(num).Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06007F91 RID: 32657 RVA: 0x0025D794 File Offset: 0x0025B994
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			CompTemperatureRuinable comp = base.GetComp<CompTemperatureRuinable>();
			if (!this.Empty && !comp.Ruined)
			{
				if (this.Fermented)
				{
					stringBuilder.AppendLine("ContainsBeer".Translate(this.wortCount, 25));
				}
				else
				{
					stringBuilder.AppendLine("ContainsWort".Translate(this.wortCount, 25));
				}
			}
			if (!this.Empty)
			{
				if (this.Fermented)
				{
					stringBuilder.AppendLine("Fermented".Translate());
				}
				else
				{
					stringBuilder.AppendLine("FermentationProgress".Translate(this.Progress.ToStringPercent(), this.EstimatedTicksLeft.ToStringTicksToPeriod(true, false, true, true)));
					if (this.CurrentTempProgressSpeedFactor != 1f)
					{
						stringBuilder.AppendLine("FermentationBarrelOutOfIdealTemperature".Translate(this.CurrentTempProgressSpeedFactor.ToStringPercent()));
					}
				}
			}
			stringBuilder.AppendLine("Temperature".Translate() + ": " + base.AmbientTemperature.ToStringTemperature("F0"));
			stringBuilder.AppendLine("IdealFermentingTemperature".Translate() + ": " + 7f.ToStringTemperature("F0") + " ~ " + comp.Props.maxSafeTemperature.ToStringTemperature("F0"));
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06007F92 RID: 32658 RVA: 0x00055B62 File Offset: 0x00053D62
		public Thing TakeOutBeer()
		{
			if (!this.Fermented)
			{
				Log.Warning("Tried to get beer but it's not yet fermented.", false);
				return null;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Beer, null);
			thing.stackCount = this.wortCount;
			this.Reset();
			return thing;
		}

		// Token: 0x06007F93 RID: 32659 RVA: 0x0025D964 File Offset: 0x0025BB64
		public override void Draw()
		{
			base.Draw();
			if (!this.Empty)
			{
				Vector3 drawPos = this.DrawPos;
				drawPos.y += 0.042857144f;
				drawPos.z += 0.25f;
				GenDraw.DrawFillableBar(new GenDraw.FillableBarRequest
				{
					center = drawPos,
					size = Building_FermentingBarrel.BarSize,
					fillPercent = (float)this.wortCount / 25f,
					filledMat = this.BarFilledMat,
					unfilledMat = Building_FermentingBarrel.BarUnfilledMat,
					margin = 0.1f,
					rotation = Rot4.North
				});
			}
		}

		// Token: 0x06007F94 RID: 32660 RVA: 0x00055B96 File Offset: 0x00053D96
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (Prefs.DevMode && !this.Empty)
			{
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set progress to 1",
					action = delegate()
					{
						this.Progress = 1f;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x040052CA RID: 21194
		private int wortCount;

		// Token: 0x040052CB RID: 21195
		private float progressInt;

		// Token: 0x040052CC RID: 21196
		private Material barFilledCachedMat;

		// Token: 0x040052CD RID: 21197
		public const int MaxCapacity = 25;

		// Token: 0x040052CE RID: 21198
		private const int BaseFermentationDuration = 360000;

		// Token: 0x040052CF RID: 21199
		public const float MinIdealTemperature = 7f;

		// Token: 0x040052D0 RID: 21200
		private static readonly Vector2 BarSize = new Vector2(0.55f, 0.1f);

		// Token: 0x040052D1 RID: 21201
		private static readonly Color BarZeroProgressColor = new Color(0.4f, 0.27f, 0.22f);

		// Token: 0x040052D2 RID: 21202
		private static readonly Color BarFermentedColor = new Color(0.9f, 0.85f, 0.2f);

		// Token: 0x040052D3 RID: 21203
		private static readonly Material BarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
