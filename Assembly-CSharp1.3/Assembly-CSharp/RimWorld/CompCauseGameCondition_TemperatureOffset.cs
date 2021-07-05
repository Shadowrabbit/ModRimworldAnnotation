using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010F4 RID: 4340
	public class CompCauseGameCondition_TemperatureOffset : CompCauseGameCondition
	{
		// Token: 0x170011BF RID: 4543
		// (get) Token: 0x060067E9 RID: 26601 RVA: 0x0023268F File Offset: 0x0023088F
		public new CompProperties_CausesGameCondition_ClimateAdjuster Props
		{
			get
			{
				return (CompProperties_CausesGameCondition_ClimateAdjuster)this.props;
			}
		}

		// Token: 0x060067EA RID: 26602 RVA: 0x0023269C File Offset: 0x0023089C
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.temperatureOffset = this.Props.temperatureOffsetRange.min;
		}

		// Token: 0x060067EB RID: 26603 RVA: 0x002326BB File Offset: 0x002308BB
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.temperatureOffset, "temperatureOffset", 0f, false);
		}

		// Token: 0x060067EC RID: 26604 RVA: 0x002326D9 File Offset: 0x002308D9
		private string GetFloatStringWithSign(float val)
		{
			if (val < 0f)
			{
				return val.ToString("0");
			}
			return "+" + val.ToString("0");
		}

		// Token: 0x060067ED RID: 26605 RVA: 0x00232706 File Offset: 0x00230906
		public void SetTemperatureOffset(float offset)
		{
			this.temperatureOffset = this.Props.temperatureOffsetRange.ClampToRange(offset);
			base.ReSetupAllConditions();
		}

		// Token: 0x060067EE RID: 26606 RVA: 0x00232725 File Offset: 0x00230925
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "-10";
			Command_Action command_Action2 = command_Action;
			command_Action2.action = (Action)Delegate.Combine(command_Action2.action, new Action(delegate()
			{
				this.SetTemperatureOffset(this.temperatureOffset - 10f);
			}));
			command_Action.hotKey = KeyBindingDefOf.Misc1;
			yield return command_Action;
			Command_Action command_Action3 = new Command_Action();
			command_Action3.defaultLabel = "-1";
			Command_Action command_Action4 = command_Action3;
			command_Action4.action = (Action)Delegate.Combine(command_Action4.action, new Action(delegate()
			{
				this.SetTemperatureOffset(this.temperatureOffset - 1f);
			}));
			command_Action3.hotKey = KeyBindingDefOf.Misc2;
			yield return command_Action3;
			Command_Action command_Action5 = new Command_Action();
			command_Action5.defaultLabel = "+1";
			Command_Action command_Action6 = command_Action5;
			command_Action6.action = (Action)Delegate.Combine(command_Action6.action, new Action(delegate()
			{
				this.SetTemperatureOffset(this.temperatureOffset + 1f);
			}));
			command_Action5.hotKey = KeyBindingDefOf.Misc3;
			yield return command_Action5;
			Command_Action command_Action7 = new Command_Action();
			command_Action7.defaultLabel = "+10";
			Command_Action command_Action8 = command_Action7;
			command_Action8.action = (Action)Delegate.Combine(command_Action8.action, new Action(delegate()
			{
				this.SetTemperatureOffset(this.temperatureOffset + 10f);
			}));
			command_Action7.hotKey = KeyBindingDefOf.Misc4;
			yield return command_Action7;
			yield break;
		}

		// Token: 0x060067EF RID: 26607 RVA: 0x00232738 File Offset: 0x00230938
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("Temperature".Translate() + ": " + this.GetFloatStringWithSign(this.temperatureOffset));
		}

		// Token: 0x060067F0 RID: 26608 RVA: 0x00232792 File Offset: 0x00230992
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_TemperatureOffset)condition).tempOffset = this.temperatureOffset;
		}

		// Token: 0x060067F1 RID: 26609 RVA: 0x002327B0 File Offset: 0x002309B0
		public override void RandomizeSettings(Site site)
		{
			bool flag = false;
			bool flag2 = false;
			foreach (WorldObject worldObject in Find.WorldObjects.AllWorldObjects)
			{
				Settlement settlement;
				if ((settlement = (worldObject as Settlement)) != null && settlement.Faction == Faction.OfPlayer)
				{
					if (settlement.Map != null)
					{
						bool flag3 = false;
						foreach (GameCondition gameCondition in settlement.Map.GameConditionManager.ActiveConditions)
						{
							if (gameCondition is GameCondition_TemperatureOffset)
							{
								float num = gameCondition.TemperatureOffset();
								if (num > 0f)
								{
									flag3 = true;
									flag = true;
									flag2 = false;
								}
								else if (num < 0f)
								{
									flag3 = true;
									flag2 = true;
									flag = false;
								}
								if (flag3)
								{
									break;
								}
							}
						}
						if (flag3)
						{
							break;
						}
					}
					int tile = worldObject.Tile;
					if ((float)Find.WorldGrid.TraversalDistanceBetween(site.Tile, tile, true, this.Props.worldRange + 1) <= (float)this.Props.worldRange)
					{
						float num2 = GenTemperature.MinTemperatureAtTile(tile);
						float num3 = GenTemperature.MaxTemperatureAtTile(tile);
						if (num2 < -5f)
						{
							flag2 = true;
						}
						if (num3 > 20f)
						{
							flag = true;
						}
					}
				}
			}
			if (flag2 == flag)
			{
				this.temperatureOffset = (Rand.Bool ? this.Props.temperatureOffsetRange.min : this.Props.temperatureOffsetRange.max);
				return;
			}
			if (flag2)
			{
				this.temperatureOffset = this.Props.temperatureOffsetRange.min;
				return;
			}
			if (flag)
			{
				this.temperatureOffset = this.Props.temperatureOffsetRange.max;
			}
		}

		// Token: 0x04003A7C RID: 14972
		public float temperatureOffset;

		// Token: 0x04003A7D RID: 14973
		private const float MaxTempForMinOffset = -5f;

		// Token: 0x04003A7E RID: 14974
		private const float MinTempForMaxOffset = 20f;
	}
}
