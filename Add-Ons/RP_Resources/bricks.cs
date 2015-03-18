// --------------------------
// RP Resources :: Bricks
// --------------------------

if (isPackage(RP_Resources_Bricks_Core))
	deactivatePackage(RP_Resources_Bricks_Core);

package RP_Resources_Bricks_Core
{
	// Color
	function fxDTSBrick::setColor(%brick, %color)
	{
		%datablock = %brick.getDatablock();
		if (%datablock.isOre)
			%color = (%brick.hasOre) ? %datablock.colorHasRes : %datablock.colorHasNoRes;
		if (%datablock.isTree)
			%color = (%brick.resources > 0) ? %datablock.colorHasRes : %datablock.colorHasNoRes;
		
		Parent::setColor(%brick, %color);
	}
	
	// Color FX
	function fxDTSBrick::setColorFX(%brick, %FX)
	{
		%datablock = %brick.getDatablock();
		if (%datablock.isOre)
			%FX = 0;
		if (%datablock.isTree)
			%FX = 0;
		Parent::setColorFX(%brick, %FX);
	}
	
	// Shape FX
	function fxDTSBrick::setShapeFX(%brick, %FX)
	{
		%datablock = %brick.getDatablock();
		if (%datablock.isOre)
			%FX = 0;
		if (%datablock.isTree)
			%FX = 0;
		Parent::setShapeFX(%brick, %FX);
	}
	
	// Emitter
	function fxDTSBrick::setEmitter(%brick, %emitter)
	{
		%datablock = %brick.getDatablock();
		// Create table if not created
		if($UiNameTableCreated == false)
			createUiNameTable();
		
		if (%datablock.isOre)
			%emitter = 0;
		if (%datablock.isTree)
			%emitter = 0;
		if (%datablock.isFishingSpot)
		{
			%emitter = (%brick.amountFish > 0) ? $uiNameTable_Emitters["Player Foam Droplets"] : 0;
			%brick.setColliding(0);
			%brick.setRendering(0);
			%brick.setRayCasting(1);
		}
		Parent::setEmitter(%brick, %emitter);
	}
	
	// RP Plant Brick
	function fxDTSBrick::PlantBrick(%brick)
	{
		Parent::PlantBrick(%brick);
		
		if (!isObject(%brick))
			return false;
		
		%brick.ResetResource();
	}
	
	// RP Loading Brick
	function fxDTSBrick::LoadingBrick(%brick)
	{
		Parent::LoadingBrick(%brick);
		
		if (!isObject(%brick))
			return false;
		
		%brick.ResetResource();
	}
	
	// RP Remove Brick
	function RemoveBrick(%brick)
	{
		Parent::RemoveBrick(%brick);
		if (isEventPending(%brick.scheduleColor))
			Cancel(%brick.scheduleColor);
	}
	
	// Planting a brick making resource do as it should do
	function fxDTSBrick::onPlant(%brick)
	{
		Parent::onPlant(%brick);
		
		if ($RP::activated)
			return;
		
		%brick.ResetResource();
	}
};
activatePackage(RP_Resources_Bricks_Core);

// Reset resource
function fxDTSBrick::ResetResource(%brick)
{
	%datablock = %brick.getDatablock();
	switch (true)
	{
		// Ore
		case %datablock.isOre:
			%brick.hasOre = true;
			%brick.setColor(0);
		// Tree
		case %datablock.isTree:
			%brick.resources = %datablock.resources;
			%brick.setColor(0);
		// Fish
		case %datablock.isFishingSpot:
			%brick.amountFish = %datablock.amountFish;
			%brick.setEmitter(0);
	}
}

// Mining
function fxDTSBrick::onMine(%brick, %client)
{
	// Does not exist
	if (!isObject(%brick))
		return;
	
	%datablock = %brick.getDatablock();
	
	// No mining object
	if (!%datablock.isOre)
		return;
	
	if (!%client.CheckLastAction(%brick))
		return;
	
	%client.lastAction = $sim::time;
	
	%exp = %client.RPData.value["exp"];
	
	// Has no ore
	if (!%brick.hasOre)
	{
		// Check time
		if (%brick.nextTimeRes < $sim::time)
			%brick.ResetResource();
		else
		{
			messageClient(%client, 'centerPrint', "\c6This ore has ran out.");
			return;
		}
	}
	
	// Gems
	if (getRandom(0, 100) > (100 - ((%datablock.gemChance + %client.RPData.value["education"]) / 2)))
	{
		%got = -1;
		for (%i = 0; %i < getWordCount(%datablock.gems); %i++)
		{
			%gem = getWord(%datablock.gems, %i);
			%id = $RP::resources::gem::indexName[%gem];
			%crop = mClamp($RP::resources::gem::chance[%id] - (%exp / $RP::pref::server::hardcoreExp), 50, 100);
			if (getRandom(0, 100) > %crop)
			{
				%got = %id;
				break;
			}
		}
		// Get gem
		if (%got != -1)
		{
			%name = $RP::resources::gem::name[%got];
			%amount = $RP::resources::gem::resources[%got];
			%client.RPData.value["exp"] += %amount;
			%client.RPData.value["ore"] += %amount;
			commandToClient(%client, 'centerPrint', "\c6You found " @ findVowel(%name) @ " \c3" @ %name @ "\c6 worth \c3" @ %amount @ "\c6 ores in this ore!", 2);
			%client.displayInfo();
			return;
		}
	}
	
	switch(%client.RPData.value["hunger"])
	{
		case 1:
			%addval = 8;
			// break;
		case 2:
			%addval = 5;
			// break;
		case 3:
			%addval = 3.5;
			// break;
		case 4:
			%addval = 2;
			// break;
		case 5:
			%addval = 1;
			// break;
		case 6:
			%addval = 0;
			// break;
		case 7:
			%addval = 0;
			// break;
		case 8:
			%addval = 0;
			// break;
		case 9:
			%addval = 0;
			// break;
		case 10:
			%addval = 0;
			// break;
		default:
			%addval = 5;
	}
	
	%crop = (90 - mFloor((%client.RPData.value["education"] / 6) * 4)) + %addval;
	//%crop = mClamp(%datablock.hardCore - (%exp / $RP::pref::server::hardcoreExp), 50, 100);
	// Mining
	if (getRandom(0, 100) < %crop)
		return;
	
	%res = %datablock.resources;
	%client.RPData.value["exp"] += $RP::pref::gain::exp::mineOre * %res;
	if($RP::job::group[%client.RPData.value["jobID"]] $= "999999")
	{
		%client.RPData.value["LaborExp"] += 1;
	}
	%client.RPData.value["ore"] += %res;
	%brick.CheckOreColor();
	commandToClient(%client, 'centerPrint', "\c6You harvested \c3" @ %res SPC %datablock.uiName @ "\c6 ore" @ (%res > 1 ? "s" : "") @ ".", 1);
	%client.displayInfo();
}

// Control ore color
function fxDTSBrick::CheckOreColor(%brick)
{
	if (!isObject(%brick))
		return;
	
	%datablock = %brick.getDatablock();
	
	// Is no ore
	if (!%datablock.isOre)
		return;
	
	%brick.nextTimeRes = $sim::time + (%datablock.oreIsBack / 1000);
	%brick.hasOre = false;
	%brick.setColor(0);
	
	// Not required, but a good thing to do
	%brick.scheduleColor = %brick.schedule(%datablock.oreIsBack, ResetResource);
}

// Cutting a tree
function fxDTSBrick::onCut(%brick, %client)
{
	// Does not exist
	if (!(isObject(%brick) && isObject(%client)))
		return;
	
	%datablock = %brick.getDatablock();
	
	// No tree object
	if (!%datablock.isTree)
		return;
	
	if (!%client.CheckLastAction(%brick))
		return;
	
	%client.lastAction = $sim::time;
	
	%exp = %client.RPData.value["exp"];
	
	// Has no wood
	if (%brick.resources <= 0)
	{
		// Check time
		if (%brick.nextTimeRes < $sim::time)
			%brick.ResetResource();
		else
		{
			messageClient(%client, 'centerPrint', "\c6This tree has already chopped down.");
			return;
		}
	}
	
	switch(%client.RPData.value["hunger"])
	{
		case 1:
			%addval = 4;
			// break;
		case 2:
			%addval = 3;
			// break;
		case 3:
			%addval = 2;
			// break;
		case 4:
			%addval = 1.5;
			// break;
		case 5:
			%addval = 1;
			// break;
		case 6:
			%addval = 0;
			// break;
		case 7:
			%addval = 0;
			// break;
		case 8:
			%addval = 0;
			// break;
		case 9:
			%addval = 0;
			// break;
		case 10:
			%addval = 0;
			// break;
		default:
			%addval = 5;
	}
	
	%crop = (95 - mFloor((%client.RPData.value["education"] / 6) * 4)) + %addval;
	//%crop = mClamp(%datablock.hardCore - (%exp / $RP::pref::server::hardcoreExp), 50, 100);
	// Mining
	if (getRandom(0, 100) < %crop)
		return;
	
	%res = %datablock.each;
	%client.RPData.value["exp"] += $RP::pref::gain::exp::cutTree * %res;
	%client.RPData.value["LaborExp"] += 1;
	%client.RPData.value["wood"] += %res;
	%brick.resources -= %res;
	%brick.CheckTreeResources(0);
	commandToClient(%client, 'centerPrint', "\c6You harvested \c3" @ %res SPC %datablock.uiName @ "\c6 log" @ (%res > 1 ? "s" : "") @ ".", 1);
	%client.displayInfo();
	
	
}

// Control tree resources
function fxDTSBrick::CheckTreeResources(%brick)
{
	if (!isObject(%brick))
		return;
	
	%datablock = %brick.getDatablock();
	
	// Is no tree
	if (!%datablock.isTree)
		return;
	
	// Do not continue
	if (%brick.resources > 0)
		return;
	
	%brick.nextTimeRes = $sim::time + (%datablock.reGrow / 1000);
	%brick.setColor(0);
	
	%brick.scheduleColor = %brick.schedule(%datablock.reGrow, ResetResource);
}

// Fishing
function fxDTSBrick::onFish(%brick, %client)
{
	// Does not exist
	if (!isObject(%brick))
		return;
	
	%datablock = %brick.getDatablock();
	
	// No fishing object
	if (!%datablock.isFishingSpot)
		return;
	
	if (!%client.CheckLastAction(%brick))
		return;
	
	%client.lastAction = $sim::time;
	
	%exp = %client.RPData.value["exp"];
	
	// Has no fish
	if (%brick.amountFish <= 0)
	{
		// Check time
		if (%brick.nextTimeRes < $sim::time)
			%brick.ResetResource();
		else
		{
			messageClient(%client, 'centerPrint', "\c6There are currently no fish here.");
			return;
		}
	}
	
	%group = %datablock.fishGroup;
	%amount = 0;
	
	// What type of fish
	for (%i = 0; %i < $RP::resources::fish::amount[%group]; %i++)
	{
		%crop = mClamp($RP::resources::fish::hardCore[%group, %i] - (%exp / $RP::pref::server::hardcoreExp), 50, 100);
		if (getRandom(0, 100) > %crop)
		{
			%list[%amount] = %i;
			%amount++;
		}
	}
	
	// Nothing
	if (%amount == 0)
		return;
	
	%type = %list[getRandom(0, %amount - 1)];
	
	%res = $RP::resources::fish::each[%group, %type];
	%brick.amountFish -= %res;
	%client.RPData.value["exp"] += $RP::pref::gain::exp::fishing * %res;
	%client.RPData.value["LaborExp"] += 1;
	%client.RPData.value["fish"] += %res;
	%brick.CheckFishResource();
	%name = $RP::resources::fish::name[%group, %type];
	commandToClient(%client, 'centerPrint', "\c6You fished " @ findVowel(%name) @ " \c3" @ %name @ "\c6 worth \c3" @ %res @ "\c6 fish" @ (%res > 1 ? "es" : "") @ ".", 1);
	%client.displayInfo();
}

// Control fish emitter
function fxDTSBrick::CheckFishResource(%brick)
{
	if (!isObject(%brick))
		return;
	
	%datablock = %brick.getDatablock();
	
	// Is no fish
	if (!%datablock.isFishingSpot)
		return;
	
	if (%brick.amountFish > 0)
	{
		%brick.setEmitter(0);
		%brick.setRayCasting(1);
		return;
	}
	
	%brick.nextTimeRes = $sim::time + (%datablock.fishIsBack / 1000);
	%brick.setEmitter(0);
	
	// Disable raycasting to avoid being hit
	%brick.setRayCasting(0);
	
	%brick.scheduleColor = %brick.schedule(%datablock.fishIsBack, ResetResource);
}
