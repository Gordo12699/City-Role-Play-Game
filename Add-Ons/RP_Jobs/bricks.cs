// --------------------------
// RP Jobs :: Bricks
// --------------------------

if (isPackage(RP_Jobs_Bricks_Core))
	deactivatePackage(RP_Jobs_Bricks_Core);

package RP_Jobs_Bricks_Core
{
	// Planting a brick
	function fxDTSBrick::onPlant(%brick)
	{
		
		if (!isObject(%brick))
			return false;
		
		%datablock = %brick.getDatablock();
		
		if (%datablock.isSpawnBrick)
			%brick.AddSpawn();
			
		Parent::onPlant(%brick);
	}
	
	function fxDTSBrick::onLoadPlant(%brick)
	{
		if(!isObject(%brick))
			return false;
			
		%datablock = %brick.getDatablock();
		
		if (%datablock.isSpawnBrick)
			%brick.AddSpawn();
			
		Parent::onLoadPlant(%brick);
		
		if(isObject(%brick))
		{
			//talk("isObject, brick!");
			%brickGroup = getBrickGroupFromObject(%brick);
			%client = %brickGroup.client;
			if(isObject(%client))
			{
				//talk("is Object, client! " @ %client.name);
				switch(%brick.getDatablock().CRPGBrickType)
				{
					case 420:
						if(%brick.getDataBlock().CRPGBrickType == 420 || %brick.getDataBlock().CRPGBrickType == 420 && %client.isAdmin)
						{
							//talk("bricktype420");
							messageAll(%client,'',"\c6GOOO");
							if(%client.RPData.value["Money"] >= mFloor(%brick.getDatablock().price))
							{
								if(%client.RPData.value["drugamount"] <= $CRPG::drug::maxdrugplants)
								{
									//talk("drug amount less than max");
									//%client.RPData.value["Money"] -= mFloor(%brick.getDatablock().price);
									%client.RPData.value["drugamount"]++;
									%drug = %brick.getID();
									%drug.canchange = true;
									%drug.isGrowing = false;
									%drug.grew = false;
									%drug.watered = false;
									%drug.isDrug = true;
									%drug.currentColor = 45;
									%drug.setColor(45);
									%drug.owner = %client.bl_id;
									%drug.hasemitter = true;
									%drug.growtime = 0;
									%drug.health = 0;
									%drug.orighealth = %drug.health;
									if(%brick.getDataBlock().drugType $= "marijuana")
									{
										//talk("drug type marijuana");
										%drug.random = getRandom($CRPG::drugs::marijuana::harvestMin,$CRPG::drugs::marijuana::harvestMax);
										%drug.uiName = "Marijuana";
										//messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick.");
									}
									else if(%brick.getDataBlock().drugType $= "opium")
									{
										//talk("drug type opium");
										%drug.random = getRandom($CRPG::drugs::opium::harvestMin,$CRPG::drugs::opium::harvestMax);
										%drug.uiName = "opium";
										//messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									}
									%drug.canbecolored = false;
									%drug.setEmitter("None");
									%drug.cansetemitter = false;
									%drug.emitterr = "GrassEmitter";
									%drug.canchange = false;
								}
								else
								{
									//talk("met limit of drugs");
									commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									%brick.schedule(0, "delete");
								}
							}
							else
							{
								//talk("Need more money");
								commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								%brick.schedule(0, "delete");
							}
						}
					default:
				}
			}
			else
			{
				switch(%brick.getDatablock().CRPGBrickType)
				{
					case 420:
						if(%brick.getDataBlock().CRPGBrickType == 420)
						{
							%brickGroup = getBrickGroupFromObject(%brick);
							%target = %brickGroup.client;
							%OwnerID = (!isObject(%target)) ? ($Server::serverType $= "Internet" ? %brickGroup.bl_id : %brickGroup.name) : %target.getSaveKey();
							//talk("bricktype420");
							messageAll(%client,'',"\c6GOOO");
								if(RPDB.get(%OwnerID, "drugamount") <= $CRPG::drug::maxdrugplants)
								{
									//talk("drug amount less than max");
									RPDB.set(%OwnerID, "drugamount", RPDB.get(%OwnerID, "drugamount") + 1);
									%drug = %brick.getID();
									%drug.canchange = true;
									%drug.isGrowing = false;
									%drug.grew = false;
									%drug.watered = false;
									%drug.isDrug = true;
									%drug.currentColor = 45;
									%drug.setColor(45);
									%drug.owner = %OwnerID;
									%drug.hasemitter = true;
									%drug.growtime = 0;
									%drug.health = 0;
									%drug.orighealth = %drug.health;
									if(%brick.getDataBlock().drugType $= "marijuana")
									{
										//talk("drug type marijuana");
										%drug.random = getRandom($CRPG::drugs::marijuana::harvestMin,$CRPG::drugs::marijuana::harvestMax);
										%drug.uiName = "Marijuana";
										//messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick.");
									}
									else if(%brick.getDataBlock().drugType $= "opium")
									{
										//talk("drug type opium");
										%drug.random = getRandom($CRPG::drugs::opium::harvestMin,$CRPG::drugs::opium::harvestMax);
										%drug.uiName = "opium";
										//messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									}
									%drug.canbecolored = false;
									%drug.setEmitter("None");
									%drug.cansetemitter = false;
									%drug.emitterr = "GrassEmitter";
									%drug.canchange = false;
								}
								else
								{
									//talk("met limit of drugs");
									%brick.schedule(0, "delete");
								}
						}
					default:
				}
			}
		}
	}
	
	// Killing a brick
	function fxDTSBrick::onDeath(%brick)
	{
		if (%brick.getDatablock().isSpawnBrick)
			RemoveSpawn(%brick);
		Parent::onDeath(%brick);
	}
};
activatePackage(RP_Jobs_Bricks_Core);

if (isPackage(RP_Jobs_Bricks))
	deactivatePackage(RP_Jobs_Bricks);

package RP_Jobs_Bricks
{
	// When planting a brick
	function fxDTSBrick::PlantBrick(%brick)
	{
		Parent::PlantBrick(%brick);
		
		%brickGroup = getBrickGroupFromObject(%brick);
		%client = %brickGroup.client;
		
		// Check job build rights
		if (!(%client.isAdmin || $RP::job::build[%client.RPData.value["jobID"]]))
		{
			commandToClient(%client, 'centerPrint', "\c3Your job is not allowed to build.", 3);
			%brick.schedule(0, "delete");
			return false;
		}
	}
};
RPRegPackage(RP_Jobs_Bricks);

// Add spawn to list
function fxDTSBrick::AddSpawn(%brick)
{
	$RP::spawnPoints = addToList($RP::spawnPoints, %brick);
}

// Remove spawn from list
function RemoveSpawn(%brick)
{
	$RP::spawnPoints = removeFromList($RP::spawnPoints, %brick);
}

datablock fxDTSBrickData(CRPGPoliceVehicleData : brickVehicleSpawnData)
{
	category = "CRPG";
	subCategory = "Spawns";
	uiName = "Police Vehicle Spawn";
	adminOnly = true;
};

datablock fxDTSBrickData(CRPGCrimeVehicleData : brickVehicleSpawnData)
{
	category = "CRPG";
	subCategory = "Spawns";
	uiName = "Crime Vehicle Spawn";
	adminOnly = true;
};

datablock fxDTSBrickData(CRPGParaVehicleData : brickVehicleSpawnData)
{
	category = "CRPG";
	subCategory = "Spawns";
	uiName = "Paramedic Vehicle Spawn";
	adminOnly = true;
};
