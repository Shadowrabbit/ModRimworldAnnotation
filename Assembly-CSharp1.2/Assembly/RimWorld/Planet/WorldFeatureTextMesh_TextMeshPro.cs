using System;
using TMPro;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200202F RID: 8239
	[StaticConstructorOnStartup]
	public class WorldFeatureTextMesh_TextMeshPro : WorldFeatureTextMesh
	{
		// Token: 0x0600AEA6 RID: 44710 RVA: 0x0007184E File Offset: 0x0006FA4E
		private static void TextScale_Changed()
		{
			Find.WorldFeatures.textsCreated = false;
		}

		// Token: 0x170019B3 RID: 6579
		// (get) Token: 0x0600AEA7 RID: 44711 RVA: 0x000719C8 File Offset: 0x0006FBC8
		public override bool Active
		{
			get
			{
				return this.textMesh.gameObject.activeInHierarchy;
			}
		}

		// Token: 0x170019B4 RID: 6580
		// (get) Token: 0x0600AEA8 RID: 44712 RVA: 0x000719DA File Offset: 0x0006FBDA
		public override Vector3 Position
		{
			get
			{
				return this.textMesh.transform.position;
			}
		}

		// Token: 0x170019B5 RID: 6581
		// (get) Token: 0x0600AEA9 RID: 44713 RVA: 0x000719EC File Offset: 0x0006FBEC
		// (set) Token: 0x0600AEAA RID: 44714 RVA: 0x000719F9 File Offset: 0x0006FBF9
		public override Color Color
		{
			get
			{
				return this.textMesh.color;
			}
			set
			{
				this.textMesh.color = value;
			}
		}

		// Token: 0x170019B6 RID: 6582
		// (get) Token: 0x0600AEAB RID: 44715 RVA: 0x00071A07 File Offset: 0x0006FC07
		// (set) Token: 0x0600AEAC RID: 44716 RVA: 0x00071A14 File Offset: 0x0006FC14
		public override string Text
		{
			get
			{
				return this.textMesh.text;
			}
			set
			{
				this.textMesh.text = value;
			}
		}

		// Token: 0x170019B7 RID: 6583
		// (set) Token: 0x0600AEAD RID: 44717 RVA: 0x00071A22 File Offset: 0x0006FC22
		public override float Size
		{
			set
			{
				this.textMesh.fontSize = value * WorldFeatureTextMesh_TextMeshPro.TextScale;
			}
		}

		// Token: 0x170019B8 RID: 6584
		// (get) Token: 0x0600AEAE RID: 44718 RVA: 0x00071A36 File Offset: 0x0006FC36
		// (set) Token: 0x0600AEAF RID: 44719 RVA: 0x00071A48 File Offset: 0x0006FC48
		public override Quaternion Rotation
		{
			get
			{
				return this.textMesh.transform.rotation;
			}
			set
			{
				this.textMesh.transform.rotation = value;
			}
		}

		// Token: 0x170019B9 RID: 6585
		// (get) Token: 0x0600AEB0 RID: 44720 RVA: 0x00071A5B File Offset: 0x0006FC5B
		// (set) Token: 0x0600AEB1 RID: 44721 RVA: 0x00071A6D File Offset: 0x0006FC6D
		public override Vector3 LocalPosition
		{
			get
			{
				return this.textMesh.transform.localPosition;
			}
			set
			{
				this.textMesh.transform.localPosition = value;
			}
		}

		// Token: 0x0600AEB2 RID: 44722 RVA: 0x00071A80 File Offset: 0x0006FC80
		public override void SetActive(bool active)
		{
			this.textMesh.gameObject.SetActive(active);
		}

		// Token: 0x0600AEB3 RID: 44723 RVA: 0x00071A93 File Offset: 0x0006FC93
		public override void Destroy()
		{
			UnityEngine.Object.Destroy(this.textMesh.gameObject);
		}

		// Token: 0x0600AEB4 RID: 44724 RVA: 0x0032C2D8 File Offset: 0x0032A4D8
		public override void Init()
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(WorldFeatureTextMesh_TextMeshPro.WorldTextPrefab);
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			this.textMesh = gameObject.GetComponent<TextMeshPro>();
			this.Color = new Color(1f, 1f, 1f, 0f);
			Material[] sharedMaterials = this.textMesh.GetComponent<MeshRenderer>().sharedMaterials;
			for (int i = 0; i < sharedMaterials.Length; i++)
			{
				sharedMaterials[i].renderQueue = WorldMaterials.FeatureNameRenderQueue;
			}
		}

		// Token: 0x0600AEB5 RID: 44725 RVA: 0x0032C350 File Offset: 0x0032A550
		public override void WrapAroundPlanetSurface()
		{
			this.textMesh.ForceMeshUpdate();
			TMP_TextInfo textInfo = this.textMesh.textInfo;
			int characterCount = textInfo.characterCount;
			if (characterCount == 0)
			{
				return;
			}
			float num = this.textMesh.bounds.extents.x * 2f;
			float num2 = Find.WorldGrid.DistOnSurfaceToAngle(num);
			Matrix4x4 localToWorldMatrix = this.textMesh.transform.localToWorldMatrix;
			Matrix4x4 worldToLocalMatrix = this.textMesh.transform.worldToLocalMatrix;
			for (int i = 0; i < characterCount; i++)
			{
				TMP_CharacterInfo tmp_CharacterInfo = textInfo.characterInfo[i];
				if (tmp_CharacterInfo.isVisible)
				{
					int materialReferenceIndex = this.textMesh.textInfo.characterInfo[i].materialReferenceIndex;
					int vertexIndex = tmp_CharacterInfo.vertexIndex;
					Vector3 vector = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] + this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3];
					vector /= 4f;
					float num3 = vector.x / (num / 2f);
					bool flag = num3 >= 0f;
					num3 = Mathf.Abs(num3);
					float num4 = num2 / 2f * num3;
					float num5 = (180f - num4) / 2f;
					float num6 = 200f * Mathf.Tan(num4 / 2f * 0.017453292f);
					Vector3 vector2 = new Vector3(Mathf.Sin(num5 * 0.017453292f) * num6 * (flag ? 1f : -1f), vector.y, Mathf.Cos(num5 * 0.017453292f) * num6);
					Vector3 b = vector2 - vector;
					Vector3 vector3 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] + b;
					Vector3 vector4 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] + b;
					Vector3 vector5 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] + b;
					Vector3 vector6 = this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3] + b;
					Quaternion rotation = Quaternion.Euler(0f, num4 * (flag ? -1f : 1f), 0f);
					vector3 = rotation * (vector3 - vector2) + vector2;
					vector4 = rotation * (vector4 - vector2) + vector2;
					vector5 = rotation * (vector5 - vector2) + vector2;
					vector6 = rotation * (vector6 - vector2) + vector2;
					vector3 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector3).normalized * (100f + WorldAltitudeOffsets.WorldText));
					vector4 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector4).normalized * (100f + WorldAltitudeOffsets.WorldText));
					vector5 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector5).normalized * (100f + WorldAltitudeOffsets.WorldText));
					vector6 = worldToLocalMatrix.MultiplyPoint(localToWorldMatrix.MultiplyPoint(vector6).normalized * (100f + WorldAltitudeOffsets.WorldText));
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex] = vector3;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 1] = vector4;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 2] = vector5;
					this.textMesh.textInfo.meshInfo[materialReferenceIndex].vertices[vertexIndex + 3] = vector6;
				}
			}
			this.textMesh.UpdateVertexData(TMP_VertexDataUpdateFlags.All);
		}

		// Token: 0x040077DD RID: 30685
		private TextMeshPro textMesh;

		// Token: 0x040077DE RID: 30686
		public static readonly GameObject WorldTextPrefab = Resources.Load<GameObject>("Prefabs/WorldText");

		// Token: 0x040077DF RID: 30687
		[TweakValue("Interface.World", 0f, 5f)]
		private static float TextScale = 1f;
	}
}
