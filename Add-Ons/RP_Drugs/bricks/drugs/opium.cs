// ============================================================
// Project				:	CityRPG
// Author				:	Moppy
// Description			:	Opium Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Events
// 3. Package Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CityRPGOpiumData)
{
	brickFile = "Add-Ons/RP_Drugs/shapes/Bricks/brick_6x6.blb";
	iconName = "Add-Ons/RP_Drugs/shapes/BrickIcons/opium";
	
	category = "CRPG";
	subCategory = "Drugs";
	
	uiName = "Opium";
	drugType = "Opium";
	
	owner = 0;
	canchange = false;
	cansetemitter = false;
	emitter = "GrassEmitter";
	isDrug = true;
	hasDrug = false;
	isOpium = true;
	isGrowing = false;
	growtime = 0;
	canbecolored = false;
	health = 100;
	orighealth = 100;
	
	watered = 0;
	grew = 0;
	
	price = $CRPG::drugs::Opium::placePrice;
	
	harvestAmt = $CRPG::drugs::Opium::harvestAmt;
	growthTime = $CRPG::drugs::Opium::growthTime;
	
	CRPGBrickType = 420;
	CRPGBrickAdmin = false;
};
