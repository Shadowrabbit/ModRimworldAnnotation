using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001660 RID: 5728
	public class MinifiedThing : ThingWithComps, IThingHolder
	{
		// Token: 0x1700132F RID: 4911
		// (get) Token: 0x06007CBA RID: 31930 RVA: 0x00053CF5 File Offset: 0x00051EF5
		// (set) Token: 0x06007CBB RID: 31931 RVA: 0x00254BD0 File Offset: 0x00252DD0
		public Thing InnerThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				return this.innerContainer[0];
			}
			set
			{
				if (value == this.InnerThing)
				{
					return;
				}
				if (value == null)
				{
					this.innerContainer.Clear();
					return;
				}
				if (this.innerContainer.Count != 0)
				{
					Log.Warning(string.Concat(new string[]
					{
						"Assigned 2 things to the same MinifiedThing ",
						this.ToStringSafe<MinifiedThing>(),
						" (first=",
						this.innerContainer[0].ToStringSafe<Thing>(),
						" second=",
						value.ToStringSafe<Thing>(),
						")"
					}), false);
					this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x17001330 RID: 4912
		// (get) Token: 0x06007CBC RID: 31932 RVA: 0x00254C74 File Offset: 0x00252E74
		public override Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.cachedGraphic = this.InnerThing.Graphic.ExtractInnerGraphicFor(this.InnerThing);
					if ((float)this.InnerThing.def.size.x > 1.1f || (float)this.InnerThing.def.size.z > 1.1f)
					{
						Vector2 minifiedDrawSize = this.GetMinifiedDrawSize(this.InnerThing.def.size.ToVector2(), 1.1f);
						Vector2 newDrawSize = new Vector2(minifiedDrawSize.x / (float)this.InnerThing.def.size.x * this.cachedGraphic.drawSize.x, minifiedDrawSize.y / (float)this.InnerThing.def.size.z * this.cachedGraphic.drawSize.y);
						this.cachedGraphic = this.cachedGraphic.GetCopy(newDrawSize);
					}
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x17001331 RID: 4913
		// (get) Token: 0x06007CBD RID: 31933 RVA: 0x00053D12 File Offset: 0x00051F12
		public override string LabelNoCount
		{
			get
			{
				return this.InnerThing.LabelNoCount;
			}
		}

		// Token: 0x17001332 RID: 4914
		// (get) Token: 0x06007CBE RID: 31934 RVA: 0x00053D1F File Offset: 0x00051F1F
		public override string DescriptionDetailed
		{
			get
			{
				return this.InnerThing.DescriptionDetailed;
			}
		}

		// Token: 0x17001333 RID: 4915
		// (get) Token: 0x06007CBF RID: 31935 RVA: 0x00053D2C File Offset: 0x00051F2C
		public override string DescriptionFlavor
		{
			get
			{
				return this.InnerThing.DescriptionFlavor;
			}
		}

		// Token: 0x06007CC0 RID: 31936 RVA: 0x00053D39 File Offset: 0x00051F39
		public MinifiedThing()
		{
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep);
		}

		// Token: 0x06007CC1 RID: 31937 RVA: 0x00053D4F File Offset: 0x00051F4F
		public override void Tick()
		{
			if (this.InnerThing == null)
			{
				Log.Error("MinifiedThing with null InnerThing. Destroying.", false);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Tick();
			if (this.InnerThing is Building_Battery)
			{
				this.innerContainer.ThingOwnerTick(true);
			}
		}

		// Token: 0x06007CC2 RID: 31938 RVA: 0x00053D8B File Offset: 0x00051F8B
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06007CC3 RID: 31939 RVA: 0x00053D93 File Offset: 0x00051F93
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06007CC4 RID: 31940 RVA: 0x00254D84 File Offset: 0x00252F84
		public override Thing SplitOff(int count)
		{
			MinifiedThing minifiedThing = (MinifiedThing)base.SplitOff(count);
			if (minifiedThing != this)
			{
				minifiedThing.InnerThing = ThingMaker.MakeThing(this.InnerThing.def, this.InnerThing.Stuff);
				ThingWithComps thingWithComps = this.InnerThing as ThingWithComps;
				if (thingWithComps != null)
				{
					for (int i = 0; i < thingWithComps.AllComps.Count; i++)
					{
						thingWithComps.AllComps[i].PostSplitOff(minifiedThing.InnerThing);
					}
				}
			}
			return minifiedThing;
		}

		// Token: 0x06007CC5 RID: 31941 RVA: 0x00254E00 File Offset: 0x00253000
		public override bool CanStackWith(Thing other)
		{
			MinifiedThing minifiedThing = other as MinifiedThing;
			return minifiedThing != null && base.CanStackWith(other) && this.InnerThing.CanStackWith(minifiedThing.InnerThing);
		}

		// Token: 0x06007CC6 RID: 31942 RVA: 0x00053DA1 File Offset: 0x00051FA1
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x06007CC7 RID: 31943 RVA: 0x000FBCDC File Offset: 0x000F9EDC
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
			if (blueprint_Install != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
			}
		}

		// Token: 0x06007CC8 RID: 31944 RVA: 0x00254E38 File Offset: 0x00253038
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.crateFrontGraphic == null)
			{
				this.crateFrontGraphic = GraphicDatabase.Get<Graphic_Single>("Things/Item/Minified/CrateFront", ShaderDatabase.Cutout, this.GetMinifiedDrawSize(this.InnerThing.def.size.ToVector2(), 1.1f) * 1.16f, Color.white);
			}
			this.crateFrontGraphic.DrawFromDef(drawLoc + Altitudes.AltIncVect * 0.1f, Rot4.North, null, 0f);
			if (this.Graphic is Graphic_Single)
			{
				this.Graphic.Draw(drawLoc, Rot4.North, this, 0f);
				return;
			}
			this.Graphic.Draw(drawLoc, Rot4.South, this, 0f);
		}

		// Token: 0x06007CC9 RID: 31945 RVA: 0x00254EF8 File Offset: 0x002530F8
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Destroy(mode);
			if (this.InnerThing != null)
			{
				InstallBlueprintUtility.CancelBlueprintsFor(this);
				if (spawned)
				{
					if (mode == DestroyMode.Deconstruct)
					{
						SoundDefOf.Building_Deconstructed.PlayOneShot(new TargetInfo(base.Position, map, false));
						GenLeaving.DoLeavingsFor(this.InnerThing, map, mode, this.OccupiedRect(), null, null);
					}
					else if (mode == DestroyMode.KillFinalize)
					{
						GenLeaving.DoLeavingsFor(this.InnerThing, map, mode, this.OccupiedRect(), null, null);
					}
				}
				if (this.InnerThing is MonumentMarker)
				{
					this.InnerThing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06007CCA RID: 31946 RVA: 0x00053DC3 File Offset: 0x00051FC3
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PreTraded(action, playerNegotiator, trader);
			InstallBlueprintUtility.CancelBlueprintsFor(this);
		}

		// Token: 0x06007CCB RID: 31947 RVA: 0x00053DD4 File Offset: 0x00051FD4
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in base.GetGizmos())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return InstallationDesignatorDatabase.DesignatorFor(this.def);
			yield break;
			yield break;
		}

		// Token: 0x06007CCC RID: 31948 RVA: 0x00254F94 File Offset: 0x00253194
		public override string GetInspectString()
		{
			string text = "NotInstalled".Translate();
			string inspectString = this.InnerThing.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				text += "\n";
				text += inspectString;
			}
			return text;
		}

		// Token: 0x06007CCD RID: 31949 RVA: 0x00254FDC File Offset: 0x002531DC
		private Vector2 GetMinifiedDrawSize(Vector2 drawSize, float maxSideLength)
		{
			float num = maxSideLength / Mathf.Max(drawSize.x, drawSize.y);
			if (num >= 1f)
			{
				return drawSize;
			}
			return drawSize * num;
		}

		// Token: 0x0400518A RID: 20874
		private const float MaxMinifiedGraphicSize = 1.1f;

		// Token: 0x0400518B RID: 20875
		private const float CrateToGraphicScale = 1.16f;

		// Token: 0x0400518C RID: 20876
		private ThingOwner innerContainer;

		// Token: 0x0400518D RID: 20877
		private Graphic cachedGraphic;

		// Token: 0x0400518E RID: 20878
		private Graphic crateFrontGraphic;
	}
}
