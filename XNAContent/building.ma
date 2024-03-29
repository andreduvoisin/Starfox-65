//Maya ASCII 2014 scene
//Name: building.ma
//Last modified: Thu, Apr 10, 2014 05:56:26 PM
//Codeset: 1252
requires maya "2014";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2014";
fileInfo "version" "2014 x64";
fileInfo "cutIdentifier" "201303010241-864206";
fileInfo "osv" "Microsoft Windows 7 Business Edition, 64-bit Windows 7 Service Pack 1 (Build 7601)\n";
fileInfo "license" "education";
createNode transform -n "pCube1";
	setAttr ".t" -type "double3" -0.49616425974684475 5.3779136716560494 -0.49616425974684475 ;
createNode mesh -n "pCubeShape1" -p "pCube1";
	setAttr -k off ".v";
	setAttr ".vir" yes;
	setAttr ".vif" yes;
	setAttr ".pv" -type "double2" 0.019103378057479858 0.020973729923015494 ;
	setAttr ".uvst[0].uvsn" -type "string" "map1";
	setAttr ".cuvs" -type "string" "map1";
	setAttr ".dcc" -type "string" "Ambient+Diffuse";
	setAttr ".covm[0]"  0 1 1;
	setAttr ".cdvm[0]"  0 1 1;
createNode polyTweakUV -n "polyTweakUV8";
	setAttr ".uopa" yes;
	setAttr -s 20 ".uvtk";
	setAttr ".uvtk[0]" -type "float2" 0.61485136 0.0034095275 ;
	setAttr ".uvtk[1]" -type "float2" 0.3790206 -0.00056577474 ;
	setAttr ".uvtk[2]" -type "float2" -0.40005115 -0.27617168 ;
	setAttr ".uvtk[3]" -type "float2" -0.631899 0.73532128 ;
	setAttr ".uvtk[4]" -type "float2" 0.013992578 0.9736774 ;
	setAttr ".uvtk[5]" -type "float2" 0.26759264 -0.37716472 ;
	setAttr ".uvtk[7]" -type "float2" -0.0044377143 -0.9744646 ;
	setAttr ".uvtk[10]" -type "float2" 0.13751759 -0.0022730166 ;
	setAttr ".uvtk[11]" -type "float2" 0.13410802 0.73532128 ;
	setAttr ".uvtk[12]" -type "float2" -0.12956202 0.0022730175 ;
	setAttr ".uvtk[13]" -type "float2" -0.13297154 0.73873085 ;
	setAttr ".uvtk[15]" -type "float2" 0.015092969 -0.97759289 ;
	setAttr ".uvtk[16]" -type "float2" -0.009931094 0.96695817 ;
	setAttr ".uvtk[17]" -type "float2" -0.35306537 -0.3707298 ;
	setAttr ".uvtk[18]" -type "float2" 0.40118763 0.76600701 ;
	setAttr ".uvtk[19]" -type "float2" 0.36368287 -0.24548146 ;
	setAttr ".uvtk[20]" -type "float2" -0.3920956 0.76714355 ;
	setAttr ".uvtk[21]" -type "float2" 0.61485136 0.73645782 ;
	setAttr ".uvtk[22]" -type "float2" -0.61257833 -0.0011365092 ;
	setAttr ".uvtk[23]" -type "float2" -0.3756142 2.2556633e-006 ;
createNode polyMapCut -n "polyMapCut8";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 4 "e[0]" "e[4:5]" "e[8]" "e[10:11]";
createNode polyTweakUV -n "polyTweakUV7";
	setAttr ".uopa" yes;
	setAttr -s 2 ".uvtk";
	setAttr ".uvtk[3]" -type "float2" 0.0056829695 -0.014207488 ;
	setAttr ".uvtk[19]" -type "float2" 0.0056829695 -0.014207488 ;
createNode polyMapCut -n "polyMapCut7";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 3 "e[1]" "e[5]" "e[7]";
createNode polyTweakUV -n "polyTweakUV6";
	setAttr ".uopa" yes;
	setAttr -s 2 ".uvtk";
	setAttr ".uvtk[3]" -type "float2" 0.0094716735 0.017048994 ;
	setAttr ".uvtk[19]" -type "float2" 0.0094716735 0.017048994 ;
createNode polyMapCut -n "polyMapCut6";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 2 "e[1]" "e[7]";
createNode polyTweakUV -n "polyTweakUV5";
	setAttr ".uopa" yes;
	setAttr ".uvtk[2]" -type "float2" 0.020837644 0.029362144;
createNode polyMapCut -n "polyMapCut5";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 3 "e[1]" "e[4]" "e[6]";
createNode polyMapCut -n "polyMapCut4";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 3 "e[1]" "e[4]" "e[6]";
createNode polyTweakUV -n "polyTweakUV4";
	setAttr ".uopa" yes;
	setAttr -s 2 ".uvtk";
	setAttr ".uvtk[18]" -type "float2" -0.02462629 -0.030309305 ;
	setAttr ".uvtk[20]" -type "float2" 0.027467819 -0.030309308 ;
createNode polyMapCut -n "polyMapCut3";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 2 "e[1]" "e[4:7]";
createNode polyTweakUV -n "polyTweakUV3";
	setAttr ".uopa" yes;
	setAttr -s 10 ".uvtk";
	setAttr ".uvtk[4]" -type "float2" -0.37303746 -0.48078427 ;
	setAttr ".uvtk[5]" -type "float2" 0.054213032 0 ;
	setAttr ".uvtk[6]" -type "float2" -0.18619168 -0.72093415 ;
	setAttr ".uvtk[7]" -type "float2" 0.4583025 0.2573542 ;
	setAttr ".uvtk[8]" -type "float2" -0.18619168 0.0134058 ;
	setAttr ".uvtk[9]" -type "float2" 0.56304359 0.014895324 ;
	setAttr ".uvtk[14]" -type "float2" 0.56900167 -0.72093415 ;
	setAttr ".uvtk[15]" -type "float2" -0.29658255 0.25735417 ;
	setAttr ".uvtk[16]" -type "float2" 0.37850684 -0.48227382 ;
	setAttr ".uvtk[17]" -type "float2" 0.038941737 0 ;
createNode polyMapCut -n "polyMapCut2";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 2 "e[2]" "e[6:9]";
createNode polyTweakUV -n "polyTweakUV2";
	setAttr ".uopa" yes;
	setAttr -s 6 ".uvtk";
	setAttr ".uvtk[6]" -type "float2" -0.18722689 0 ;
	setAttr ".uvtk[7]" -type "float2" -0.080327973 0 ;
	setAttr ".uvtk[8]" -type "float2" -0.18722689 0 ;
	setAttr ".uvtk[9]" -type "float2" -0.18722689 0 ;
	setAttr ".uvtk[14]" -type "float2" -0.18722689 0 ;
	setAttr ".uvtk[15]" -type "float2" -0.07880085 0 ;
createNode polyMapCut -n "polyMapCut1";
	setAttr ".uopa" yes;
	setAttr ".ics" -type "componentList" 2 "e[3]" "e[8:11]";
createNode deleteComponent -n "deleteComponent1";
	setAttr ".dc" -type "componentList" 1 "map[6:9]";
createNode polyTweakUV -n "polyTweakUV1";
	setAttr ".uopa" yes;
	setAttr -s 14 ".uvtk[0:13]" -type "float2" 0.004393816 0.017575055 -0.0043936968
		 0.017575055 0.004393816 0.0087875128 -0.0043936968 0.0087875128 0.004393816 0 -0.0043936968
		 0 0.004393816 -0.0087875128 -0.0043936968 -0.0087875128 0.004393816 -0.017575026
		 -0.0043936968 -0.017575026 -0.01318121 0.017575055 -0.01318121 0.0087875128 0.013181329
		 0.017575055 0.013181329 0.0087875128;
createNode polyCube -n "polyCube1";
	setAttr ".w" 3.0119186099879016;
	setAttr ".h" 10.755827343312099;
	setAttr ".d" 3.0119186099879087;
	setAttr ".cuv" 4;
createNode materialInfo -n "materialInfo1";
createNode shadingEngine -n "lambert2SG";
	setAttr ".ihi" 0;
	setAttr ".ro" yes;
createNode lambert -n "lambert2";
createNode file -n "file1";
	setAttr ".ftn" -type "string" "C:/Users/ITP/Documents/jean/XNAContent/building_texture1.jpg";
createNode place2dTexture -n "place2dTexture1";
createNode lightLinker -s -n "lightLinker1";
	setAttr -s 3 ".lnk";
	setAttr -s 3 ".slnk";
select -ne :time1;
	setAttr ".o" 1;
	setAttr ".unw" 1;
select -ne :renderPartition;
	setAttr -s 3 ".st";
select -ne :initialShadingGroup;
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultShaderList1;
	setAttr -s 3 ".s";
select -ne :defaultTextureList1;
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderUtilityList1;
select -ne :defaultRenderingList1;
select -ne :renderGlobalsList1;
select -ne :defaultResolution;
	setAttr ".pa" 1;
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
select -ne :hardwareRenderingGlobals;
	setAttr ".otfna" -type "stringArray" 18 "NURBS Curves" "NURBS Surfaces" "Polygons" "Subdiv Surfaces" "Particles" "Fluids" "Image Planes" "UI:" "Lights" "Cameras" "Locators" "Joints" "IK Handles" "Deformers" "Motion Trails" "Components" "Misc. UI" "Ornaments"  ;
	setAttr ".otfva" -type "Int32Array" 18 0 1 1 1 1 1
		 1 0 0 0 0 0 0 0 0 0 0 0 ;
select -ne :defaultHardwareRenderGlobals;
	setAttr ".fn" -type "string" "im";
	setAttr ".res" -type "string" "ntsc_4d 646 485 1.333";
connectAttr "polyTweakUV8.out" "pCubeShape1.i";
connectAttr "polyTweakUV8.uvtk[0]" "pCubeShape1.uvst[0].uvtw";
connectAttr "polyMapCut8.out" "polyTweakUV8.ip";
connectAttr "polyTweakUV7.out" "polyMapCut8.ip";
connectAttr "polyMapCut7.out" "polyTweakUV7.ip";
connectAttr "polyTweakUV6.out" "polyMapCut7.ip";
connectAttr "polyMapCut6.out" "polyTweakUV6.ip";
connectAttr "polyTweakUV5.out" "polyMapCut6.ip";
connectAttr "polyMapCut5.out" "polyTweakUV5.ip";
connectAttr "polyMapCut4.out" "polyMapCut5.ip";
connectAttr "polyTweakUV4.out" "polyMapCut4.ip";
connectAttr "polyMapCut3.out" "polyTweakUV4.ip";
connectAttr "polyTweakUV3.out" "polyMapCut3.ip";
connectAttr "polyMapCut2.out" "polyTweakUV3.ip";
connectAttr "polyTweakUV2.out" "polyMapCut2.ip";
connectAttr "polyMapCut1.out" "polyTweakUV2.ip";
connectAttr "deleteComponent1.og" "polyMapCut1.ip";
connectAttr "polyTweakUV1.out" "deleteComponent1.ig";
connectAttr "polyCube1.out" "polyTweakUV1.ip";
connectAttr "lambert2SG.msg" "materialInfo1.sg";
connectAttr "lambert2.msg" "materialInfo1.m";
connectAttr "file1.msg" "materialInfo1.t" -na;
connectAttr "lambert2.oc" "lambert2SG.ss";
connectAttr "pCubeShape1.iog" "lambert2SG.dsm" -na;
connectAttr "file1.oc" "lambert2.c";
connectAttr "place2dTexture1.c" "file1.c";
connectAttr "place2dTexture1.tf" "file1.tf";
connectAttr "place2dTexture1.rf" "file1.rf";
connectAttr "place2dTexture1.mu" "file1.mu";
connectAttr "place2dTexture1.mv" "file1.mv";
connectAttr "place2dTexture1.s" "file1.s";
connectAttr "place2dTexture1.wu" "file1.wu";
connectAttr "place2dTexture1.wv" "file1.wv";
connectAttr "place2dTexture1.re" "file1.re";
connectAttr "place2dTexture1.of" "file1.of";
connectAttr "place2dTexture1.r" "file1.ro";
connectAttr "place2dTexture1.n" "file1.n";
connectAttr "place2dTexture1.vt1" "file1.vt1";
connectAttr "place2dTexture1.vt2" "file1.vt2";
connectAttr "place2dTexture1.vt3" "file1.vt3";
connectAttr "place2dTexture1.vc1" "file1.vc1";
connectAttr "place2dTexture1.o" "file1.uv";
connectAttr "place2dTexture1.ofs" "file1.fs";
relationship "link" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "link" ":lightLinker1" "lambert2SG.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialShadingGroup.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" ":initialParticleSE.message" ":defaultLightSet.message";
relationship "shadowLink" ":lightLinker1" "lambert2SG.message" ":defaultLightSet.message";
connectAttr "lambert2SG.pa" ":renderPartition.st" -na;
connectAttr "lambert2.msg" ":defaultShaderList1.s" -na;
connectAttr "file1.msg" ":defaultTextureList1.tx" -na;
connectAttr "place2dTexture1.msg" ":defaultRenderUtilityList1.u" -na;
// End of building.ma
