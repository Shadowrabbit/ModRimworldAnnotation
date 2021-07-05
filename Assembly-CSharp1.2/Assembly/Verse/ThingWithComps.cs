using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000519 RID: 1305
	public class ThingWithComps : Thing
	{
		// Token: 0x17000643 RID: 1603
		// (get) Token: 0x0600212E RID: 8494 RVA: 0x0001CEE2 File Offset: 0x0001B0E2
		public List<ThingComp> AllComps
		{
			get
			{
				if (this.comps == null)
				{
					return ThingWithComps.EmptyCompsList;
				}
				return this.comps;
			}
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x0600212F RID: 8495 RVA: 0x00105CE8 File Offset: 0x00103EE8
		// (set) Token: 0x06002130 RID: 8496 RVA: 0x0001CEF8 File Offset: 0x0001B0F8
		public override Color DrawColor
		{
			get
			{
				CompColorable comp = this.GetComp<CompColorable>();
				if (comp != null && comp.Active)
				{
					return comp.Color;
				}
				return base.DrawColor;
			}
			set
			{
				this.SetColor(value, true);
			}
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x06002131 RID: 8497 RVA: 0x00105D14 File Offset: 0x00103F14
		public override string LabelNoCount
		{
			get
			{
				string text = base.LabelNoCount;
				if (this.comps != null)
				{
					int i = 0;
					int count = this.comps.Count;
					while (i < count)
					{
						text = this.comps[i].TransformLabel(text);
						i++;
					}
				}
				return text;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x06002132 RID: 8498 RVA: 0x00105D5C File Offset: 0x00103F5C
		public override string DescriptionFlavor
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.DescriptionFlavor);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						string descriptionPart = this.comps[i].GetDescriptionPart();
						if (!descriptionPart.NullOrEmpty())
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.AppendLine();
								stringBuilder.AppendLine();
							}
							stringBuilder.Append(descriptionPart);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06002133 RID: 8499 RVA: 0x00105DDC File Offset: 0x00103FDC
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostMake();
				}
			}
		}

		// Token: 0x06002134 RID: 8500 RVA: 0x00105E24 File Offset: 0x00104024
		public T GetComp<T>() where T : ThingComp
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						return t;
					}
					i++;
				}
			}
			return default(T);
		}

		// Token: 0x06002135 RID: 8501 RVA: 0x0001CF02 File Offset: 0x0001B102
		public IEnumerable<T> GetComps<T>() where T : ThingComp
		{
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						yield return t;
					}
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x06002136 RID: 8502 RVA: 0x00105E7C File Offset: 0x0010407C
		public ThingComp GetCompByDef(CompProperties def)
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					if (this.comps[i].props == def)
					{
						return this.comps[i];
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x06002137 RID: 8503 RVA: 0x00105ECC File Offset: 0x001040CC
		public void InitializeComps()
		{
			if (this.def.comps.Any<CompProperties>())
			{
				this.comps = new List<ThingComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					ThingComp thingComp = null;
					try
					{
						thingComp = (ThingComp)Activator.CreateInstance(this.def.comps[i].compClass);
						thingComp.parent = this;
						this.comps.Add(thingComp);
						thingComp.Initialize(this.def.comps[i]);
					}
					catch (Exception arg)
					{
						Log.Error("Could not instantiate or initialize a ThingComp: " + arg, false);
						this.comps.Remove(thingComp);
					}
				}
			}
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x00105F98 File Offset: 0x00104198
		public override string GetCustomLabelNoCount(bool includeHp = true)
		{
			string text = base.GetCustomLabelNoCount(includeHp);
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					text = this.comps[i].TransformLabel(text);
					i++;
				}
			}
			return text;
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x00105FE4 File Offset: 0x001041E4
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostExposeData();
				}
			}
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x00106034 File Offset: 0x00104234
		public void BroadcastCompSignal(string signal)
		{
			this.ReceiveCompSignal(signal);
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].ReceiveCompSignal(signal);
					i++;
				}
			}
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x00006A05 File Offset: 0x00004C05
		protected virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x0010607C File Offset: 0x0010427C
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostSpawnSetup(respawningAfterLoad);
				}
			}
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x001060C4 File Offset: 0x001042C4
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDeSpawn(map);
				}
			}
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x00106110 File Offset: 0x00104310
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDestroy(mode, map);
				}
			}
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x0010615C File Offset: 0x0010435C
		public override void Tick()
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTick();
					i++;
				}
			}
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x0010619C File Offset: 0x0010439C
		public override void TickRare()
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTickRare();
					i++;
				}
			}
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x001061DC File Offset: 0x001043DC
		public override void TickLong()
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTickLong();
					i++;
				}
			}
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x0010621C File Offset: 0x0010441C
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPreApplyDamage(dinfo, out absorbed);
					if (absorbed)
					{
						return;
					}
				}
			}
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x00106274 File Offset: 0x00104474
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostApplyDamage(dinfo, totalDamageDealt);
				}
			}
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x0001CF12 File Offset: 0x0001B112
		public override void Draw()
		{
			base.Draw();
			this.Comps_PostDraw();
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x001062BC File Offset: 0x001044BC
		protected void Comps_PostDraw()
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDraw();
				}
			}
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x001062F8 File Offset: 0x001044F8
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDrawExtraSelectionOverlays();
				}
			}
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x0010633C File Offset: 0x0010453C
		public override void Print(SectionLayer layer)
		{
			base.Print(layer);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPrintOnto(layer);
				}
			}
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x00106380 File Offset: 0x00104580
		public virtual void PrintForPowerGrid(SectionLayer layer)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPrintForPowerGrid(layer);
				}
			}
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x0001CF20 File Offset: 0x0001B120
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (Gizmo gizmo in this.comps[i].CompGetGizmosExtra())
					{
						yield return gizmo;
					}
					IEnumerator<Gizmo> enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600214A RID: 8522 RVA: 0x001063C0 File Offset: 0x001045C0
		public override bool TryAbsorbStack(Thing other, bool respectStackLimit)
		{
			if (!this.CanStackWith(other))
			{
				return false;
			}
			int count = ThingUtility.TryAbsorbStackNumToTake(this, other, respectStackLimit);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PreAbsorbStack(other, count);
				}
			}
			return base.TryAbsorbStack(other, respectStackLimit);
		}

		// Token: 0x0600214B RID: 8523 RVA: 0x0010641C File Offset: 0x0010461C
		public override Thing SplitOff(int count)
		{
			Thing thing = base.SplitOff(count);
			if (thing != null && this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostSplitOff(thing);
				}
			}
			return thing;
		}

		// Token: 0x0600214C RID: 8524 RVA: 0x00106468 File Offset: 0x00104668
		public override bool CanStackWith(Thing other)
		{
			if (!base.CanStackWith(other))
			{
				return false;
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (!this.comps[i].AllowStackWith(other))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x001064B8 File Offset: 0x001046B8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			string text = this.InspectStringPartsFromComps();
			if (!text.NullOrEmpty())
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x00106508 File Offset: 0x00104708
		protected string InspectStringPartsFromComps()
		{
			if (this.comps == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.comps.Count; i++)
			{
				string text = this.comps[i].CompInspectStringExtra();
				if (!text.NullOrEmpty())
				{
					if (Prefs.DevMode && char.IsWhiteSpace(text[text.Length - 1]))
					{
						Log.ErrorOnce(this.comps[i].GetType() + " CompInspectStringExtra ended with whitespace: " + text, 25612, false);
						text = text.TrimEndNewlines();
					}
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x001065C0 File Offset: 0x001047C0
		public override void DrawGUIOverlay()
		{
			base.DrawGUIOverlay();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].DrawGUIOverlay();
				}
			}
		}

		// Token: 0x06002150 RID: 8528 RVA: 0x0001CF30 File Offset: 0x0001B130
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetFloatMenuOptions(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (FloatMenuOption floatMenuOption2 in this.comps[i].CompFloatMenuOptions(selPawn))
					{
						yield return floatMenuOption2;
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x0001CF47 File Offset: 0x0001B147
		public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(List<Pawn> selPawns)
		{
			foreach (FloatMenuOption floatMenuOption in base.GetMultiSelectFloatMenuOptions(selPawns))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (FloatMenuOption floatMenuOption2 in this.comps[i].CompMultiSelectFloatMenuOptions(selPawns))
					{
						yield return floatMenuOption2;
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x00106604 File Offset: 0x00104804
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PrePreTraded(action, playerNegotiator, trader);
				}
			}
			base.PreTraded(action, playerNegotiator, trader);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x0010664C File Offset: 0x0010484C
		public override void PostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			base.PostGeneratedForTrader(trader, forTile, forFaction);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostGeneratedForTrader(trader, forTile, forFaction);
				}
			}
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x00106694 File Offset: 0x00104894
		protected override void PrePostIngested(Pawn ingester)
		{
			base.PrePostIngested(ingester);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PrePostIngested(ingester);
				}
			}
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x001066D8 File Offset: 0x001048D8
		protected override void PostIngested(Pawn ingester)
		{
			base.PostIngested(ingester);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostIngested(ingester);
				}
			}
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x0010671C File Offset: 0x0010491C
		public override void Notify_SignalReceived(Signal signal)
		{
			base.Notify_SignalReceived(signal);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_SignalReceived(signal);
				}
			}
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x00106760 File Offset: 0x00104960
		public override void Notify_LordDestroyed()
		{
			base.Notify_LordDestroyed();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_LordDestroyed();
				}
			}
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x001067A4 File Offset: 0x001049A4
		public override void Notify_Equipped(Pawn pawn)
		{
			base.Notify_Equipped(pawn);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_Equipped(pawn);
				}
			}
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x001067E8 File Offset: 0x001049E8
		public override void Notify_UsedWeapon(Pawn pawn)
		{
			base.Notify_UsedWeapon(pawn);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_UsedWeapon(pawn);
				}
			}
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x0010682C File Offset: 0x00104A2C
		public void Notify_KilledPawn(Pawn pawn)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_KilledPawn(pawn);
				}
			}
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x0001CF5E File Offset: 0x0001B15E
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in base.SpecialDisplayStats())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					IEnumerable<StatDrawEntry> enumerable = this.comps[i].SpecialDisplayStats();
					if (enumerable != null)
					{
						foreach (StatDrawEntry statDrawEntry2 in enumerable)
						{
							yield return statDrawEntry2;
						}
						enumerator = null;
					}
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x0010686C File Offset: 0x00104A6C
		public override void Notify_Explosion(Explosion explosion)
		{
			base.Notify_Explosion(explosion);
			CompWakeUpDormant comp = this.GetComp<CompWakeUpDormant>();
			if (comp != null && (explosion.Position - base.Position).LengthHorizontal <= explosion.radius)
			{
				comp.Activate(true, false);
			}
		}

		// Token: 0x040016C3 RID: 5827
		private List<ThingComp> comps;

		// Token: 0x040016C4 RID: 5828
		private static readonly List<ThingComp> EmptyCompsList = new List<ThingComp>();
	}
}
