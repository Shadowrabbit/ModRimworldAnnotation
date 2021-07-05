using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02001742 RID: 5954
	public class WorldFeatures : IExposable
	{
		// Token: 0x06008982 RID: 35202 RVA: 0x003157AF File Offset: 0x003139AF
		private static void TextWrapThreshold_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x06008983 RID: 35203 RVA: 0x003157AF File Offset: 0x003139AF
		protected static void ForceLegacyText_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x06008984 RID: 35204 RVA: 0x00315FB4 File Offset: 0x003141B4
		public void ExposeData()
		{
			Scribe_Collections.Look<WorldFeature>(ref this.features, "features", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				WorldGrid grid = Find.WorldGrid;
				if (grid.tileFeature != null && grid.tileFeature.Length != 0)
				{
					DataSerializeUtility.LoadUshort(grid.tileFeature, grid.TilesCount, delegate(int i, ushort data)
					{
						grid[i].feature = ((data == ushort.MaxValue) ? null : this.GetFeatureWithID((int)data));
					});
				}
				this.textsCreated = false;
			}
		}

		// Token: 0x06008985 RID: 35205 RVA: 0x00316040 File Offset: 0x00314240
		public void UpdateFeatures()
		{
			if (!this.textsCreated)
			{
				this.textsCreated = true;
				this.CreateTextsAndSetPosition();
			}
			bool showWorldFeatures = Find.PlaySettings.showWorldFeatures;
			for (int i = 0; i < this.features.Count; i++)
			{
				Vector3 position = WorldFeatures.texts[i].Position;
				bool flag = showWorldFeatures && !WorldRendererUtility.HiddenBehindTerrainNow(position);
				if (flag != WorldFeatures.texts[i].Active)
				{
					WorldFeatures.texts[i].SetActive(flag);
					WorldFeatures.texts[i].WrapAroundPlanetSurface();
				}
				if (flag)
				{
					this.UpdateAlpha(WorldFeatures.texts[i], this.features[i]);
				}
			}
		}

		// Token: 0x06008986 RID: 35206 RVA: 0x003160FC File Offset: 0x003142FC
		public WorldFeature GetFeatureWithID(int uniqueID)
		{
			for (int i = 0; i < this.features.Count; i++)
			{
				if (this.features[i].uniqueID == uniqueID)
				{
					return this.features[i];
				}
			}
			return null;
		}

		// Token: 0x06008987 RID: 35207 RVA: 0x00316144 File Offset: 0x00314344
		private void UpdateAlpha(WorldFeatureTextMesh text, WorldFeature feature)
		{
			float num = 0.3f * feature.alpha;
			if (text.Color.a != num)
			{
				text.Color = new Color(1f, 1f, 1f, num);
				text.WrapAroundPlanetSurface();
			}
			float num2 = Time.deltaTime * 5f;
			if (this.GoodCameraAltitudeFor(feature))
			{
				feature.alpha += num2;
			}
			else
			{
				feature.alpha -= num2;
			}
			feature.alpha = Mathf.Clamp01(feature.alpha);
		}

		// Token: 0x06008988 RID: 35208 RVA: 0x003161D4 File Offset: 0x003143D4
		private bool GoodCameraAltitudeFor(WorldFeature feature)
		{
			float num = feature.EffectiveDrawSize;
			float altitude = Find.WorldCameraDriver.altitude;
			float num2 = 1f / (altitude / WorldFeatures.AlphaScale * (altitude / WorldFeatures.AlphaScale));
			num *= num2;
			if (Find.WorldCameraDriver.CurrentZoom <= WorldCameraZoomRange.VeryClose && num >= 0.56f)
			{
				return false;
			}
			if (num < WorldFeatures.VisibleMinimumSize)
			{
				return Find.WorldCameraDriver.AltitudePercent <= 0.07f;
			}
			return num <= WorldFeatures.VisibleMaximumSize || Find.WorldCameraDriver.AltitudePercent >= 0.35f;
		}

		// Token: 0x06008989 RID: 35209 RVA: 0x00316260 File Offset: 0x00314460
		private void CreateTextsAndSetPosition()
		{
			this.CreateOrDestroyTexts();
			float averageTileSize = Find.WorldGrid.averageTileSize;
			for (int i = 0; i < this.features.Count; i++)
			{
				WorldFeatures.texts[i].Text = this.features[i].name.WordWrapAt(WorldFeatures.TextWrapThreshold);
				WorldFeatures.texts[i].Size = this.features[i].EffectiveDrawSize * averageTileSize;
				Vector3 normalized = this.features[i].drawCenter.normalized;
				Quaternion quaternion = Quaternion.LookRotation(Vector3.Cross(normalized, Vector3.up), normalized);
				quaternion *= Quaternion.Euler(Vector3.right * 90f);
				quaternion *= Quaternion.Euler(Vector3.forward * (90f - this.features[i].drawAngle));
				WorldFeatures.texts[i].Rotation = quaternion;
				WorldFeatures.texts[i].LocalPosition = this.features[i].drawCenter;
				WorldFeatures.texts[i].WrapAroundPlanetSurface();
				WorldFeatures.texts[i].SetActive(false);
			}
		}

		// Token: 0x0600898A RID: 35210 RVA: 0x003163AC File Offset: 0x003145AC
		private void CreateOrDestroyTexts()
		{
			for (int i = 0; i < WorldFeatures.texts.Count; i++)
			{
				WorldFeatures.texts[i].Destroy();
			}
			WorldFeatures.texts.Clear();
			bool flag = LanguageDatabase.activeLanguage == LanguageDatabase.defaultLanguage;
			for (int j = 0; j < this.features.Count; j++)
			{
				WorldFeatureTextMesh worldFeatureTextMesh;
				if (WorldFeatures.ForceLegacyText || (!flag && this.HasCharactersUnsupportedByTextMeshPro(this.features[j].name)))
				{
					worldFeatureTextMesh = new WorldFeatureTextMesh_Legacy();
				}
				else
				{
					worldFeatureTextMesh = new WorldFeatureTextMesh_TextMeshPro();
				}
				worldFeatureTextMesh.Init();
				WorldFeatures.texts.Add(worldFeatureTextMesh);
			}
		}

		// Token: 0x0600898B RID: 35211 RVA: 0x00316450 File Offset: 0x00314650
		private bool HasCharactersUnsupportedByTextMeshPro(string str)
		{
			TMP_FontAsset font = WorldFeatureTextMesh_TextMeshPro.WorldTextPrefab.GetComponent<TextMeshPro>().font;
			for (int i = 0; i < str.Length; i++)
			{
				if (!this.HasCharacter(font, str[i]))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600898C RID: 35212 RVA: 0x00316494 File Offset: 0x00314694
		private bool HasCharacter(TMP_FontAsset font, char character)
		{
			if (TMP_FontAsset.GetCharacters(font).IndexOf(character) >= 0)
			{
				return true;
			}
			List<TMP_FontAsset> fallbackFontAssetTable = font.fallbackFontAssetTable;
			for (int i = 0; i < fallbackFontAssetTable.Count; i++)
			{
				if (TMP_FontAsset.GetCharacters(fallbackFontAssetTable[i]).IndexOf(character) >= 0)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04005729 RID: 22313
		public List<WorldFeature> features = new List<WorldFeature>();

		// Token: 0x0400572A RID: 22314
		public bool textsCreated;

		// Token: 0x0400572B RID: 22315
		private static List<WorldFeatureTextMesh> texts = new List<WorldFeatureTextMesh>();

		// Token: 0x0400572C RID: 22316
		private const float BaseAlpha = 0.3f;

		// Token: 0x0400572D RID: 22317
		private const float AlphaChangeSpeed = 5f;

		// Token: 0x0400572E RID: 22318
		[TweakValue("Interface", 0f, 300f)]
		private static float TextWrapThreshold = 150f;

		// Token: 0x0400572F RID: 22319
		[TweakValue("Interface.World", 0f, 100f)]
		protected static bool ForceLegacyText = false;

		// Token: 0x04005730 RID: 22320
		[TweakValue("Interface.World", 1f, 150f)]
		protected static float AlphaScale = 30f;

		// Token: 0x04005731 RID: 22321
		[TweakValue("Interface.World", 0f, 1f)]
		protected static float VisibleMinimumSize = 0.04f;

		// Token: 0x04005732 RID: 22322
		[TweakValue("Interface.World", 0f, 5f)]
		protected static float VisibleMaximumSize = 1f;
	}
}
