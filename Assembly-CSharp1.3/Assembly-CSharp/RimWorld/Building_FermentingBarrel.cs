using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001079 RID: 4217
	[StaticConstructorOnStartup]
	public class Building_FermentingBarrel : Building
	{
		// Token: 0x17001123 RID: 4387
		// (get) Token: 0x06006439 RID: 25657 RVA: 0x0021CEC6 File Offset: 0x0021B0C6
		// (set) Token: 0x0600643A RID: 25658 RVA: 0x0021CECE File Offset: 0x0021B0CE
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

		// Token: 0x17001124 RID: 4388
		// (get) Token: 0x0600643B RID: 25659 RVA: 0x0021CEE8 File Offset: 0x0021B0E8
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

		// Token: 0x17001125 RID: 4389
		// (get) Token: 0x0600643C RID: 25660 RVA: 0x0021CF1F File Offset: 0x0021B11F
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

		// Token: 0x17001126 RID: 4390
		// (get) Token: 0x0600643D RID: 25661 RVA: 0x0021CF34 File Offset: 0x0021B134
		private bool Empty
		{
			get
			{
				return this.wortCount <= 0;
			}
		}

		// Token: 0x17001127 RID: 4391
		// (get) Token: 0x0600643E RID: 25662 RVA: 0x0021CF42 File Offset: 0x0021B142
		public bool Fermented
		{
			get
			{
				return !this.Empty && this.Progress >= 1f;
			}
		}

		// Token: 0x17001128 RID: 4392
		// (get) Token: 0x0600643F RID: 25663 RVA: 0x0021CF60 File Offset: 0x0021B160
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

		// Token: 0x17001129 RID: 4393
		// (get) Token: 0x06006440 RID: 25664 RVA: 0x0021CFB8 File Offset: 0x0021B1B8
		private float ProgressPerTickAtCurrentTemp
		{
			get
			{
				return 2.7777778E-06f * this.CurrentTempProgressSpeedFactor;
			}
		}

		// Token: 0x1700112A RID: 4394
		// (get) Token: 0x06006441 RID: 25665 RVA: 0x0021CFC6 File Offset: 0x0021B1C6
		private int EstimatedTicksLeft
		{
			get
			{
				return Mathf.Max(Mathf.RoundToInt((1f - this.Progress) / this.ProgressPerTickAtCurrentTemp), 0);
			}
		}

		// Token: 0x06006442 RID: 25666 RVA: 0x0021CFE6 File Offset: 0x0021B1E6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.wortCount, "wortCount", 0, false);
			Scribe_Values.Look<float>(ref this.progressInt, "progress", 0f, false);
		}

		// Token: 0x06006443 RID: 25667 RVA: 0x0021D016 File Offset: 0x0021B216
		public override void TickRare()
		{
			base.TickRare();
			if (!this.Empty)
			{
				this.Progress = Mathf.Min(this.Progress + 250f * this.ProgressPerTickAtCurrentTemp, 1f);
			}
		}

		// Token: 0x06006444 RID: 25668 RVA: 0x0021D04C File Offset: 0x0021B24C
		public void AddWort(int count)
		{
			base.GetComp<CompTemperatureRuinable>().Reset();
			if (this.Fermented)
			{
				Log.Warning("Tried to add wort to a barrel full of beer. Colonists should take the beer first.");
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

		// Token: 0x06006445 RID: 25669 RVA: 0x0021D0B9 File Offset: 0x0021B2B9
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "RuinedByTemperature")
			{
				this.Reset();
			}
		}

		// Token: 0x06006446 RID: 25670 RVA: 0x0021D0CE File Offset: 0x0021B2CE
		private void Reset()
		{
			this.wortCount = 0;
			this.Progress = 0f;
		}

		// Token: 0x06006447 RID: 25671 RVA: 0x0021D0E4 File Offset: 0x0021B2E4
		public void AddWort(Thing wort)
		{
			int num = Mathf.Min(wort.stackCount, 25 - this.wortCount);
			if (num > 0)
			{
				this.AddWort(num);
				wort.SplitOff(num).Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06006448 RID: 25672 RVA: 0x0021D120 File Offset: 0x0021B320
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

		// Token: 0x06006449 RID: 25673 RVA: 0x0021D2F0 File Offset: 0x0021B4F0
		public Thing TakeOutBeer()
		{
			if (!this.Fermented)
			{
				Log.Warning("Tried to get beer but it's not yet fermented.");
				return null;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Beer, null);
			thing.stackCount = this.wortCount;
			this.Reset();
			return thing;
		}

		// Token: 0x0600644A RID: 25674 RVA: 0x0021D324 File Offset: 0x0021B524
		public override void Draw()
		{
			base.Draw();
			if (!this.Empty)
			{
				Vector3 drawPos = this.DrawPos;
				drawPos.y += 0.04054054f;
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

		// Token: 0x0600644B RID: 25675 RVA: 0x0021D3D0 File Offset: 0x0021B5D0
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

		// Token: 0x04003892 RID: 14482
		private int wortCount;

		// Token: 0x04003893 RID: 14483
		private float progressInt;

		// Token: 0x04003894 RID: 14484
		private Material barFilledCachedMat;

		// Token: 0x04003895 RID: 14485
		public const int MaxCapacity = 25;

		// Token: 0x04003896 RID: 14486
		private const int BaseFermentationDuration = 360000;

		// Token: 0x04003897 RID: 14487
		public const float MinIdealTemperature = 7f;

		// Token: 0x04003898 RID: 14488
		private static readonly Vector2 BarSize = new Vector2(0.55f, 0.1f);

		// Token: 0x04003899 RID: 14489
		private static readonly Color BarZeroProgressColor = new Color(0.4f, 0.27f, 0.22f);

		// Token: 0x0400389A RID: 14490
		private static readonly Color BarFermentedColor = new Color(0.9f, 0.85f, 0.2f);

		// Token: 0x0400389B RID: 14491
		private static readonly Material BarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
