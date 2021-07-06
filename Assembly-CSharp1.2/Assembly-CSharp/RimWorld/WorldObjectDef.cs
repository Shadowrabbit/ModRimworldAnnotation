using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001013 RID: 4115
	public class WorldObjectDef : Def
	{
		// Token: 0x17000DE5 RID: 3557
		// (get) Token: 0x060059C2 RID: 22978 RVA: 0x0003E500 File Offset: 0x0003C700
		public Material Material
		{
			get
			{
				if (this.texture.NullOrEmpty())
				{
					return null;
				}
				if (this.material == null)
				{
					this.material = MaterialPool.MatFrom(this.texture, ShaderDatabase.WorldOverlayTransparentLit, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.material;
			}
		}

		// Token: 0x17000DE6 RID: 3558
		// (get) Token: 0x060059C3 RID: 22979 RVA: 0x0003E540 File Offset: 0x0003C740
		public Texture2D ExpandingIconTexture
		{
			get
			{
				if (this.expandingIconTextureInt == null)
				{
					if (this.expandingIconTexture.NullOrEmpty())
					{
						return null;
					}
					this.expandingIconTextureInt = ContentFinder<Texture2D>.Get(this.expandingIconTexture, true);
				}
				return this.expandingIconTextureInt;
			}
		}

		// Token: 0x060059C4 RID: 22980 RVA: 0x001D3288 File Offset: 0x001D1488
		public override void PostLoad()
		{
			base.PostLoad();
			if (this.inspectorTabs != null)
			{
				for (int i = 0; i < this.inspectorTabs.Count; i++)
				{
					if (this.inspectorTabsResolved == null)
					{
						this.inspectorTabsResolved = new List<InspectTabBase>();
					}
					try
					{
						this.inspectorTabsResolved.Add(InspectTabManager.GetSharedInstance(this.inspectorTabs[i]));
					}
					catch (Exception ex)
					{
						Log.Error(string.Concat(new object[]
						{
							"Could not instantiate inspector tab of type ",
							this.inspectorTabs[i],
							": ",
							ex
						}), false);
					}
				}
			}
		}

		// Token: 0x060059C5 RID: 22981 RVA: 0x001D3334 File Offset: 0x001D1534
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			for (int i = 0; i < this.comps.Count; i++)
			{
				this.comps[i].ResolveReferences(this);
			}
		}

		// Token: 0x060059C6 RID: 22982 RVA: 0x0003E577 File Offset: 0x0003C777
		public override IEnumerable<string> ConfigErrors()
		{
			foreach (string text in base.ConfigErrors())
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			int num;
			for (int i = 0; i < this.comps.Count; i = num + 1)
			{
				foreach (string text2 in this.comps[i].ConfigErrors(this))
				{
					yield return text2;
				}
				enumerator = null;
				num = i;
			}
			if (this.expandMore && !this.expandingIcon)
			{
				yield return "has expandMore but doesn't have any expanding icon";
			}
			yield break;
			yield break;
		}

		// Token: 0x04003C67 RID: 15463
		public Type worldObjectClass = typeof(WorldObject);

		// Token: 0x04003C68 RID: 15464
		public bool canHaveFaction = true;

		// Token: 0x04003C69 RID: 15465
		public bool saved = true;

		// Token: 0x04003C6A RID: 15466
		public bool canBePlayerHome;

		// Token: 0x04003C6B RID: 15467
		public List<WorldObjectCompProperties> comps = new List<WorldObjectCompProperties>();

		// Token: 0x04003C6C RID: 15468
		public bool allowCaravanIncidentsWhichGenerateMap;

		// Token: 0x04003C6D RID: 15469
		public bool isTempIncidentMapOwner;

		// Token: 0x04003C6E RID: 15470
		public List<IncidentTargetTagDef> IncidentTargetTags;

		// Token: 0x04003C6F RID: 15471
		public bool selectable = true;

		// Token: 0x04003C70 RID: 15472
		public bool neverMultiSelect;

		// Token: 0x04003C71 RID: 15473
		public MapGeneratorDef mapGenerator;

		// Token: 0x04003C72 RID: 15474
		public List<Type> inspectorTabs;

		// Token: 0x04003C73 RID: 15475
		[Unsaved(false)]
		public List<InspectTabBase> inspectorTabsResolved;

		// Token: 0x04003C74 RID: 15476
		public bool useDynamicDrawer;

		// Token: 0x04003C75 RID: 15477
		public bool expandingIcon;

		// Token: 0x04003C76 RID: 15478
		[NoTranslate]
		public string expandingIconTexture;

		// Token: 0x04003C77 RID: 15479
		public float expandingIconPriority;

		// Token: 0x04003C78 RID: 15480
		[NoTranslate]
		public string texture;

		// Token: 0x04003C79 RID: 15481
		[Unsaved(false)]
		private Material material;

		// Token: 0x04003C7A RID: 15482
		[Unsaved(false)]
		private Texture2D expandingIconTextureInt;

		// Token: 0x04003C7B RID: 15483
		public bool expandMore;

		// Token: 0x04003C7C RID: 15484
		public bool rotateGraphicWhenTraveling;

		// Token: 0x04003C7D RID: 15485
		public Color? expandingIconColor;

		// Token: 0x04003C7E RID: 15486
		public float expandingIconDrawSize = 1f;

		// Token: 0x04003C7F RID: 15487
		public bool blockExitGridUntilBattleIsWon;
	}
}
