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
using itp380.Objects;

namespace itp380
{
	public class Camera
	{
		Game m_Game;

		Vector3 m_vCameraPosition = new Vector3(0, 0, 10);
		Vector3 m_vTarget = Vector3.Zero;

        Ship m_ShipTarget;
        public Ship CameraShipTarget
        {
            get { return m_ShipTarget; }
            set { m_ShipTarget = value; }
        }
		
		Matrix m_Camera;
		public Matrix CameraMatrix
		{
			get { return m_Camera; }
		}

        float fHDist = 10.0f;
        float fVDist = 3.0f;
        float fSpringConstant;
        float fDampConstant;
        Vector3 vCameraVelocity = Vector3.Zero;

		public Camera(Game game)
		{
			m_Game = game;

            fSpringConstant = 512.0f;
            fDampConstant = 1.4f * (float)Math.Sqrt(fSpringConstant);
		}

		public void Update(float fDeltaTime)
		{
			// TODO: If we want a moving camera, we need to make changes here
            ComputeMatrix(fDeltaTime);
		}

		public void ComputeMatrix(float fDeltaTime)
        {
            Vector3 vShipForward = Vector3.Normalize(m_ShipTarget.Forward);
            Vector3 vShipUp = Vector3.Normalize(m_ShipTarget.Up);
            Vector3 vIdealPosition = m_ShipTarget.Position - (vShipForward * fHDist) + (vShipUp * fVDist);
            
            Vector3 vDisplacement = m_vCameraPosition - vIdealPosition;
            Vector3 vSpringAccel = (-fSpringConstant * vDisplacement) - (fDampConstant * vCameraVelocity);
            vCameraVelocity += vSpringAccel * fDeltaTime;
            m_vCameraPosition += vCameraVelocity * fDeltaTime;

            Vector3 vCameraForward = Vector3.Normalize(m_ShipTarget.Position - m_vCameraPosition);
            Vector3 vCameraLeft = Vector3.Normalize(Vector3.Cross(vShipUp, vCameraForward));
            Vector3 vCameraUp = Vector3.Normalize(Vector3.Cross(vCameraForward, vCameraLeft));

            m_Camera = Matrix.CreateLookAt(m_vCameraPosition, m_ShipTarget.Position, vCameraUp);
		}
	}
}
