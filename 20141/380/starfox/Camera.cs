//-----------------------------------------------------------------------------
// Camera Singleton that for now, doesn't do much.
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

namespace itp380
{
	public class Camera
	{
		Game m_Game;

		Vector3 m_vEye = new Vector3(0, 0, 10);
        public Vector3 CameraEye
        {
            get { return m_vEye; }
            set { m_vEye = value; }
        }

		Vector3 m_vTarget = Vector3.Zero;
        public Vector3 CameraTarget
        {
            get { return m_vTarget; }
            set { m_vTarget = value; }
        }
		
		Matrix m_Camera;
		public Matrix CameraMatrix
		{
			get { return m_Camera; }
		}

		public Camera(Game game)
		{
			m_Game = game;
			ComputeMatrix();
		}

		public void Update(float fDeltaTime)
		{
			// TODO: If we want a moving camera, we need to make changes here
		}

		void ComputeMatrix()
		{
			Vector3 vEye = m_vEye;
			Vector3 vTarget = m_vTarget;
			Vector3 vUp = Vector3.Cross(Vector3.Zero - vEye, Vector3.Left);
			m_Camera = Matrix.CreateLookAt(vEye, vTarget, vUp);
		}
	}
}
