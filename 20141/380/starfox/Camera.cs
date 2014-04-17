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
        const float fHDist = 3.0f;
        const float fVDist = 3.0f;
        const float fSpringConstant = 256;
        const float fHorizontalTrack = 0.3f;
        public const float trackpoint = 12f;
        float fDampConstant = 1f * (float)Math.Sqrt(fSpringConstant);
        
        Game m_Game;

        public float Trackpoint
        {
            get { return trackpoint; }
        }

		Vector3 m_vCameraPosition = new Vector3(0, 0, 10);
		public Vector3 Position
        {
            get { return m_vCameraPosition; }
        }

        Vector3 TargetPosition
        {
            get { return m_ShipTarget.Position + m_ShipTarget.Forward * trackpoint; }
        }

        public Vector3 Forward
        {
            get { return Vector3.Normalize(TargetPosition - Position); }
        }

        Ship m_ShipTarget;
		
		Matrix m_Camera;
		public Matrix CameraMatrix
		{
			get { return m_Camera; }
		}

        Vector3 vCameraVelocity = Vector3.Zero;

		public Camera(Game game, Ship ship)
		{
			m_Game = game;
            m_ShipTarget = ship;
		}

		public void Update(float fDeltaTime)
		{
			// TODO: If we want a moving camera, we need to make changes here
            ComputeMatrix(fDeltaTime);
		}

		public void ComputeMatrix(float fDeltaTime)
        {
            Vector3 vShipForward, vIdealPosition;
            Vector3 vDisplacement, vSpringAccel;

            vShipForward = m_ShipTarget.Forward;
            vShipForward.Y = 0;
            vShipForward.Normalize();
            vIdealPosition = TargetPosition - (vShipForward * fHDist) + (Vector3.UnitY * fVDist);

            vDisplacement = m_vCameraPosition - vIdealPosition;
            vDisplacement.Z *= fHorizontalTrack;
            vDisplacement.X *= fHorizontalTrack;
            vSpringAccel = (-fSpringConstant * vDisplacement) - (fDampConstant * vCameraVelocity);
            vCameraVelocity += vSpringAccel * fDeltaTime;
            m_vCameraPosition += vCameraVelocity * fDeltaTime;

            m_Camera = Matrix.CreateLookAt(m_vCameraPosition, TargetPosition, Vector3.UnitY);
		}
	}
}
