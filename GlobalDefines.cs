//-----------------------------------------------------------------------------
// These defines don't affect the balance of the game, but change things like
// the graphics parameters and camera speed.
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

namespace itp380
{
	public static class GlobalDefines
	{
		public static int iMouseCursorSize = 32;
		public static float fMouseDefaultSpeed = 1.2f;
		public static float fCameraZoom = 20.0f;

		public static bool bVSync = true;
		public static bool bFullScreen = false;
		// Windowed resolution -- in full screen mode, it automatically
		// selects the desktop resolution.
		public static int WindowedWidth = 1024;
		public static int WindowHeight = 768;
	}
}
