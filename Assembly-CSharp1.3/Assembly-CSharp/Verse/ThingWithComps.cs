using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200037C RID: 892
	public class ThingWithComps : Thing
	{
		// Token: 0x17000559 RID: 1369
		// (get) Token: 0x060019F3 RID: 6643 RVA: 0x00097EA6 File Offset: 0x000960A6
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

		// Token: 0x1700055A RID: 1370
		// (get) Token: 0x060019F4 RID: 6644 RVA: 0x00097EBC File Offset: 0x000960BC
		// (set) Token: 0x060019F5 RID: 6645 RVA: 0x00097EE8 File Offset: 0x000960E8
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

		// Token: 0x1700055B RID: 1371
		// (get) Token: 0x060019F6 RID: 6646 RVA: 0x00097EF4 File Offset: 0x000960F4
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

		// Token: 0x1700055C RID: 1372
		// (get) Token: 0x060019F7 RID: 6647 RVA: 0x00097F3C File Offset: 0x0009613C
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

		// Token: 0x060019F8 RID: 6648 RVA: 0x00097FBC File Offset: 0x000961BC
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

		// Token: 0x060019F9 RID: 6649 RVA: 0x00098004 File Offset: 0x00096204
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

		// Token: 0x060019FA RID: 6650 RVA: 0x0009805B File Offset: 0x0009625B
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

		// Token: 0x060019FB RID: 6651 RVA: 0x0009806C File Offset: 0x0009626C
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

		// Token: 0x060019FC RID: 6652 RVA: 0x000980BC File Offset: 0x000962BC
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
						Log.Error("Could not instantiate or initialize a ThingComp: " + arg);
						this.comps.Remove(thingComp);
					}
				}
			}
		}

		// Token: 0x060019FD RID: 6653 RVA: 0x00098188 File Offset: 0x00096388
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

		// Token: 0x060019FE RID: 6654 RVA: 0x000981D4 File Offset: 0x000963D4
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

		// Token: 0x060019FF RID: 6655 RVA: 0x00098224 File Offset: 0x00096424
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

		// Token: 0x06001A00 RID: 6656 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x06001A01 RID: 6657 RVA: 0x0009826C File Offset: 0x0009646C
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

		// Token: 0x06001A02 RID: 6658 RVA: 0x000982B4 File Offset: 0x000964B4
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

		// Token: 0x06001A03 RID: 6659 RVA: 0x00098300 File Offset: 0x00096500
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

		// Token: 0x06001A04 RID: 6660 RVA: 0x0009834C File Offset: 0x0009654C
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

		// Token: 0x06001A05 RID: 6661 RVA: 0x0009838C File Offset: 0x0009658C
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

		// Token: 0x06001A06 RID: 6662 RVA: 0x000983CC File Offset: 0x000965CC
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

		// Token: 0x06001A07 RID: 6663 RVA: 0x0009840C File Offset: 0x0009660C
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

		// Token: 0x06001A08 RID: 6664 RVA: 0x00098464 File Offset: 0x00096664
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

		// Token: 0x06001A09 RID: 6665 RVA: 0x000984AA File Offset: 0x000966AA
		public override void Draw()
		{
			if (this.def.drawerType == DrawerType.RealtimeOnly)
			{
				base.Draw();
			}
			this.Comps_PostDraw();
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x000984C8 File Offset: 0x000966C8
		protected void Comps_PostDraw()
		{
			if (this.comps != null)
			{
				int count = this.comps.Count;
				for (int i = 0; i < count; i++)
				{
					this.comps[i].PostDraw();
				}
			}
		}

		// Token: 0x06001A0B RID: 6667 RVA: 0x00098508 File Offset: 0x00096708
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

		// Token: 0x06001A0C RID: 6668 RVA: 0x0009854C File Offset: 0x0009674C
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

		// Token: 0x06001A0D RID: 6669 RVA: 0x00098590 File Offset: 0x00096790
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

		// Token: 0x06001A0E RID: 6670 RVA: 0x000985CD File Offset: 0x000967CD
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (Gizmo gizmo2 in this.comps[i].CompGetGizmosExtra())
					{
						yield return gizmo2;
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06001A0F RID: 6671 RVA: 0x000985E0 File Offset: 0x000967E0
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

		// Token: 0x06001A10 RID: 6672 RVA: 0x0009863C File Offset: 0x0009683C
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

		// Token: 0x06001A11 RID: 6673 RVA: 0x00098688 File Offset: 0x00096888
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

		// Token: 0x06001A12 RID: 6674 RVA: 0x000986D8 File Offset: 0x000968D8
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

		// Token: 0x06001A13 RID: 6675 RVA: 0x00098728 File Offset: 0x00096928
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
						Log.ErrorOnce(this.comps[i].GetType() + " CompInspectStringExtra ended with whitespace: " + text, 25612);
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

		// Token: 0x06001A14 RID: 6676 RVA: 0x000987E0 File Offset: 0x000969E0
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

		// Token: 0x06001A15 RID: 6677 RVA: 0x00098822 File Offset: 0x00096A22
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

		// Token: 0x06001A16 RID: 6678 RVA: 0x00098839 File Offset: 0x00096A39
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

		// Token: 0x06001A17 RID: 6679 RVA: 0x00098850 File Offset: 0x00096A50
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

		// Token: 0x06001A18 RID: 6680 RVA: 0x00098898 File Offset: 0x00096A98
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

		// Token: 0x06001A19 RID: 6681 RVA: 0x000988E0 File Offset: 0x00096AE0
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

		// Token: 0x06001A1A RID: 6682 RVA: 0x00098924 File Offset: 0x00096B24
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

		// Token: 0x06001A1B RID: 6683 RVA: 0x00098968 File Offset: 0x00096B68
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

		// Token: 0x06001A1C RID: 6684 RVA: 0x000989AC File Offset: 0x00096BAC
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

		// Token: 0x06001A1D RID: 6685 RVA: 0x000989F0 File Offset: 0x00096BF0
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

		// Token: 0x06001A1E RID: 6686 RVA: 0x00098A34 File Offset: 0x00096C34
		public override void Notify_Unequipped(Pawn pawn)
		{
			base.Notify_Unequipped(pawn);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_Unequipped(pawn);
				}
			}
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x00098A78 File Offset: 0x00096C78
		public override void Notify_UsedWeapon(Pawn pawn)
		{
			base.Notify_UsedWeapon(pawn);
			if (ModsConfig.IdeologyActive && pawn.Ideo != null)
			{
				IdeoWeaponDisposition dispositionForWeapon = pawn.Ideo.GetDispositionForWeapon(this.def);
				if (dispositionForWeapon == IdeoWeaponDisposition.Despised)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.UsedDespisedWeapon, pawn.Named(HistoryEventArgsNames.Doer)), true);
				}
				else if (dispositionForWeapon == IdeoWeaponDisposition.Noble)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.UsedNobleWeapon, pawn.Named(HistoryEventArgsNames.Doer)), true);
				}
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_UsedWeapon(pawn);
				}
			}
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x00098B28 File Offset: 0x00096D28
		public void Notify_KilledPawn(Pawn pawn)
		{
			if (ModsConfig.IdeologyActive && pawn.Ideo != null && this.def.IsWeapon)
			{
				IdeoWeaponDisposition dispositionForWeapon = pawn.Ideo.GetDispositionForWeapon(this.def);
				if (dispositionForWeapon == IdeoWeaponDisposition.Despised)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KillWithDespisedWeapon, pawn.Named(HistoryEventArgsNames.Doer)), true);
				}
				else if (dispositionForWeapon == IdeoWeaponDisposition.Noble)
				{
					Find.HistoryEventsManager.RecordEvent(new HistoryEvent(HistoryEventDefOf.KillWithNobleWeapon, pawn.Named(HistoryEventArgsNames.Doer)), true);
				}
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_KilledPawn(pawn);
				}
			}
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x00098BDD File Offset: 0x00096DDD
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

		// Token: 0x06001A22 RID: 6690 RVA: 0x00098BF0 File Offset: 0x00096DF0
		public override void Notify_Explosion(Explosion explosion)
		{
			base.Notify_Explosion(explosion);
			CompWakeUpDormant comp = this.GetComp<CompWakeUpDormant>();
			if (comp != null && (explosion.Position - base.Position).LengthHorizontal <= explosion.radius)
			{
				comp.Activate(true, false);
			}
		}

		// Token: 0x0400111C RID: 4380
		private List<ThingComp> comps;

		// Token: 0x0400111D RID: 4381
		private static readonly List<ThingComp> EmptyCompsList = new List<ThingComp>();
	}
}
