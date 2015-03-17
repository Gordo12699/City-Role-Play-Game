// ============================================================
// Project				:	CityRPG
// Author				:	Moppy
// Description			:	Marijuana Brick Code File
// ============================================================
// Table of Contents
// 1. Brick Data
// 2. Events
// 3. Package Data
// ============================================================

// ============================================================
// Section 1 : Brick Data
// ============================================================
datablock fxDTSBrickData(CRPGMariData)
{
	brickFile = "Add-Ons/RP_Drugs/shapes/Bricks/brick_6x6.blb";
	iconName = "Add-Ons/RP_Drugs/shapes/BrickIcons/cannabis";
	
	category = "CRPG";
	subCategory = "Drugs";
	
	uiName = "Marijuana";
	drugType = "Marijuana";
	
	owner = 0;
	canchange = false;
	cansetemitter = false;
	emitter = "GrassEmitter";
	isDrug = true;
	hasDrug = false;
	isMarijuana = true;
	isGrowing = false;
	growtime = 0;
	canbecolored = false;
	health = 100;
	orighealth = 100;
	
	watered = 0;
	grew = 0;
	
	price = $CRPG::drugs::marijuana::placePrice;
	
	harvestAmt = $CRPG::drugs::marijuana::harvestAmt;
	growthTime = $CRPG::drugs::marijuana::growthTime;
	
	CRPGBrickType = 420;
	CRPGBrickAdmin = false;
};
