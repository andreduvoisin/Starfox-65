//-----------------------------------------------------------------------------
// Base GameObject class that every other class in the Objects namespace
// inherits from. It's both drawable and updatable.
//
// __Defense Sample for Game Programming Algorithms and Techniques
// Copyright (C) Sanjay Madhav. All rights reserved.
//
// Released under the Microsoft Permissive License.
// See LICENSE.txt for full details.
//-----------------------------------------------------------------------------
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace itp380
{
	public class GameObject
	{
		protected Game m_Game;
		public Model m_Model;
		protected Matrix[] m_ModelBones;
		protected string m_ModelName;

		protected Matrix m_WorldTransform = Matrix.Identity;
		protected bool m_bTransformDirty = false;

		protected eDrawOrder m_DrawOrder = eDrawOrder.Default;
		public eDrawOrder DrawOrder
		{
			get { return m_DrawOrder; }
		}

		// Anything that's timer logic is assumed to be affected by time factor
		protected Utils.Timer m_Timer = new Utils.Timer();

        Quaternion m_Rotation = Quaternion.Identity;
        public Quaternion Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; m_bTransformDirty = true; }
        }

		Vector3 m_vPos = Vector3.Zero;
		public Vector3 Position
		{
			get { return m_vPos; }
			set { m_vPos = value; m_bTransformDirty = true; }
		}

		float m_fScale = 1.0f;
		public float Scale
		{
			get { return m_fScale; }
			set { m_fScale = value; m_bTransformDirty = true; }
		}

        public Vector3 Forward
        {
            get
            {
                return Vector3.Transform(Vector3.UnitX,
                    Matrix.CreateFromQuaternion(m_Rotation));
            }
        }

        public Vector3 Up
        {
            get
            {
                return Vector3.Transform(Vector3.UnitY,
                    Matrix.CreateFromQuaternion(m_Rotation));
            }
        }

		public void RebuildWorldTransform()
		{
			m_bTransformDirty = false;
			// Scale, rotation, translation
			m_WorldTransform = Matrix.CreateScale(m_fScale) *
				Matrix.CreateFromQuaternion(m_Rotation) * Matrix.CreateTranslation(m_vPos);
		}

		public bool m_bEnabled = true;
		public bool Enabled
		{
			get { return m_bEnabled; }
			set { m_bEnabled = value; }
		}

		public GameObject(Game game)
		{
			m_Game = game;
		}

		public virtual void Load()
		{
			if (m_ModelName != "")
			{
				m_Model = m_Game.Content.Load<Model>(m_ModelName);
				m_ModelBones = new Matrix[m_Model.Bones.Count];
				m_Model.CopyAbsoluteBoneTransformsTo(m_ModelBones);
			}

			RebuildWorldTransform();
		}

		public virtual void Unload()
		{

		}

		public virtual void Update(float fDeltaTime)
		{
			if (m_bTransformDirty)
			{
				RebuildWorldTransform();
			}

			m_Timer.Update(fDeltaTime);
		}

		public virtual void Draw(float fDeltaTime)
		{
			if (m_bTransformDirty)
			{
				RebuildWorldTransform();
			}

			foreach (ModelMesh mesh in m_Model.Meshes)
			{
				foreach (BasicEffect effect in mesh.Effects)
				{
					effect.World = m_ModelBones[mesh.ParentBone.Index] * m_WorldTransform;
					effect.View = GameState.Get().CameraMatrix;
					effect.Projection = GraphicsManager.Get().Projection;
					effect.EnableDefaultLighting();
					effect.AmbientLightColor = new Vector3(1.0f, 1.0f, 1.0f);
					effect.DirectionalLight0.Enabled = true;
					effect.DirectionalLight1.Enabled = true;
					effect.DirectionalLight2.Enabled = true;
					effect.PreferPerPixelLighting = true;
				}
				mesh.Draw();
			}
		}
	}
}
