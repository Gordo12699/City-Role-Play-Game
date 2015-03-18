// --------------------------
// RP Resources :: Resources
// --------------------------

// Variable "each" is used to set the amount of resources gathered every hit. 
// So if "each" is 3, you will gather 3 resources every hit.

function LoadOre()
{
	%newFile = "Add-Ons/RP_Resources/data/ore.dat";
	%file = "config/server/RP_Core/prefs/ore.dat";
	
	// Transfer data to a new file
	if (!isFile(%file))
	{
		GameTransferFile(%newFile, %file);
	}
	// Check version
	else
	{
		%v1 = CheckFileVersion(%newFile);
		%v2 = CheckFileVersion(%file);
		// Do update
		if (%v1 > %v2)
		{
			echo("RP_Resources: Updates old orefile.");
			GameTransferFile(%newFile, %file);
		}
	}
	
	// Load ores
	if (GamePreloadFile(%file))
	{
		for (%i = 1; %i <= $Pre::amount; %i++)
		{
			// Prepare variables
			%name = $Pre::name[%i];
			%resources = $Pre::resources[%i];
			%hardCore = $Pre::hardCore[%i];
			%colorHasRes = $Pre::colorHasRes[%i];
			%colorHasNoRes = $Pre::colorHasNoRes[%i];
			%oreIsBack = $Pre::reSpawn[%i];
			%adminOnly = $Pre::plantAdminOnly[%i];
			%gemChance = $Pre::gemChance[%i];
			%gems = $Pre::gems[%i];

			// Create the datablock
			datablock fxDTSBrickData(OreBrickData)
			{
				brickFile = "Add-Ons/RP_Resources/Ore.blb";
				iconName = "Add-ons/RP_Resources/Ore";
				
				category = "CRPG";
				subCategory = "Resources";
				
				uiName = %name;
				
				specialBrickType = "";
				
				isOre = true;
				hasOre = true;
				resources = %resources;
				hardCore = %hardCore;
				colorHasRes = %colorHasRes;
				colorHasNoRes = %colorHasNoRes;
				oreIsBack = %oreIsBack * 1000;
				gemChance = %gemChance;
				gems = %gems;
				
				adminOnly = %adminOnly;
			};
			// Rename it
			OreBrickData.setName(strreplace(%name, " ", "") @ "BrickData");
		}
		// Delete
		deleteVariables("$Pre::*");
	}
}

function LoadGem()
{
	%newFile = "Add-Ons/RP_Resources/data/gem.dat";
	%file = "config/server/RP_Core/prefs/gem.dat";
	
	// Transfer data to a new file
	if (!isFile(%file))
	{
		GameTransferFile(%newFile, %file);
	}
	// Check version
	else
	{
		%v1 = CheckFileVersion(%newFile);
		%v2 = CheckFileVersion(%file);
		// Do update
		if (%v1 > %v2)
		{
			echo("RP_Resources: Updates old gemfile.");
			GameTransferFile(%newFile, %file);
		}
	}
	
	// Load gems
	if (GamePreloadFile(%file))
	{
		$RP::resources::gem::amount = 0;
		for (%i = 1; %i <= $Pre::amount; %i++)
		{
			// Prepare variables
			%name = $Pre::name[%i];
			%resources = $Pre::resources[%i];
			%chance = $Pre::chance[%i];
			
			// Get currently amount and increase it
			%n = $RP::resources::gem::amount;
			$RP::resources::gem::amount++;
			
			// Add gem
			$RP::resources::gem::name[%n] = %name;
			$RP::resources::gem::resources[%n] = %resources;
			$RP::resources::gem::chance[%n] = %chance;
			$RP::resources::gem::indexName[%name] = %n;
		}
		// Delete
		deleteVariables("$Pre::*");
	}
}

function LoadTree()
{
	%newFile = "Add-Ons/RP_Resources/data/tree.dat";
	%file = "config/server/RP_Core/prefs/tree.dat";
	
	// Transfer data to a new file
	if (!isFile(%file))
	{
		GameTransferFile(%newFile, %file);
	}
	// Check version
	else
	{
		%v1 = CheckFileVersion(%newFile);
		%v2 = CheckFileVersion(%file);
		// Do update
		if (%v1 > %v2)
		{
			echo("RP_Resources: Updates old treefile.");
			GameTransferFile(%newFile, %file);
		}
	}
	
	// Load trees
	if (GamePreloadFile(%file))
	{
		for (%i = 1; %i <= $Pre::amount; %i++)
		{
			// Prepare variables
			%name = $Pre::name[%i];
			%resources = $Pre::resources[%i];
			%each = $Pre::each[%i];
			%hardCore = $Pre::hardCore[%i];
			%colorHasRes = $Pre::colorHasRes[%i];
			%colorHasNoRes = $Pre::colorHasNoRes[%i];
			%reGrow = $Pre::reGrow[%i];
			%adminOnly = $Pre::plantAdminOnly[%i];
			%cost = $Pre::cost[%i];
			
			// Create datablock
			datablock fxDTSBrickData(TreeBrickData : brickPineTreeData)
			{
				category = "CRPG";
				subCategory = "Resources";
				
				uiName = %name;
				
				specialBrickType = "";
				cost = %cost;
				
				isTree = true;
				resources = %resources;
				each = %each;
				hardCore = %hardCore;
				colorHasRes = %colorHasRes;
				colorHasNoRes = %colorHasNoRes;
				reGrow = %reGrow * 1000;
				
				adminOnly = %adminOnly;
			};
			// Rename it
			TreeBrickData.setName(strreplace(%name, " ", "") @ "BrickData");
		}
		// Delete
		deleteVariables("$Pre::*");
	}
}

function LoadFish()
{
	%newFile = "Add-Ons/RP_Resources/data/fish.dat";
	%file = "config/server/RP_Core/prefs/fish.dat";
	
	// Transfer data to a new file
	if (!isFile(%file))
	{
		GameTransferFile(%newFile, %file);
	}
	// Check version
	else
	{
		%v1 = CheckFileVersion(%newFile);
		%v2 = CheckFileVersion(%file);
		// Do update
		if (%v1 > %v2)
		{
			echo("RP_Resources: Updates old fishfile.");
			GameTransferFile(%newFile, %file);
		}
	}
	
	// Load fishes
	if (GamePreloadFile(%file))
	{
		// Prepare default datablocks
		datablock fxDTSBrickData(CommonFishBrickData : brick8x8fData)
		{
			category = "CRPG";
			subCategory = "Resources";
			
			uiName = "Common fish";
			
			specialBrickType = "";
			
			isFishingSpot = true;
			fishGroup = "common";
			amountFish = 10;
			fishIsBack = 30000;
			
			adminOnly = 0;
		};
		datablock fxDTSBrickData(UncommonFishBrickData : brick8x8fData)
		{
			category = "CRPG";
			subCategory = "Resources";
			
			uiName = "Uncommon fish";
			
			specialBrickType = "";
			
			isFishingSpot = true;
			fishGroup = "uncommon";
			amountFish = 5;
			fishIsBack = 80000;
			
			adminOnly = 1;
		};
		datablock fxDTSBrickData(RareFishBrickData : brick8x8fData)
		{
			category = "CRPG";
			subCategory = "Resources";
			
			uiName = "Rare fish";
			
			specialBrickType = "";
			
			isFishingSpot = true;
			fishGroup = "rare";
			amountFish = 2;
			fishIsBack = 120000;
			
			adminOnly = 1;
		};
		
		// Predefine variables
		$RP::resources::fish::amountCommon = 0;
		$RP::resources::fish::amountUncommon = 0;
		$RP::resources::fish::amountRare = 0;
		
		for (%i = 1; %i <= $Pre::amount; %i++)
		{
			// Prepare variables
			%name = $Pre::name[%i];
			%fishGroup = $Pre::fishGroup[%i];
			%each = $Pre::each[%i];
			%hardCore = $Pre::hardCore[%i];
			
			// Get currently amount and increase it
			%n = $RP::resources::fish::amount[%fishGroup];
			$RP::resources::fish::amount[%fishGroup]++;
			
			// Add fish to group
			$RP::resources::fish::name[%fishGroup, %n] = %name;
			$RP::resources::fish::each[%fishGroup, %n] = %each;
			$RP::resources::fish::hardCore[%fishGroup, %n] = %hardCore;
		}
		// Delete
		deleteVariables("$Pre::*");
	}
}
