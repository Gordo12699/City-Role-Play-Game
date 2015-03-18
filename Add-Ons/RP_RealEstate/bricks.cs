$CRPG::loadedDatablocks = true;

if(!$CRPG::loadedDatablocks)
{
	return;
}

datablock triggerData(CRPGLotTriggerData)
{
	tickPeriodMS = 500;
	parent = 0;
};

datablock triggerData(CRPGInputTriggerData)
{
	tickPeriodMS = 500;
	parent = 0;
};

// ============================================================
// Section 2 : Lots
// ============================================================
datablock fxDTSBrickData(CRPGSmallLotBrickData : brick16x16FData)
{
	iconName = "Add-Ons/RP_RealEstate/shapes/BrickIcons/16x16LotIcon";
	
	category = "CRPG";
	subCategory = "CRPG Lots";
	
	uiName = "Small Lot";
	
	CRPGBrickType = 1;
	adminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 16 48";
	trigger = 0;
	
	cost = 500;
	taxAmount = 10;
};

datablock fxDTSBrickData(CRPGHalfSmallLotBrickData : brick16x32FData)
{
	iconName = "Add-Ons/RP_RealEstate/shapes/BrickIcons/16x32LotIcon";
	
	category = "CRPG";
	subCategory = "CRPG Lots";
	
	uiName = "Half-Small Lot";
	
	CRPGBrickType = 1;
	adminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 32 48";
	trigger = 0;
	
	cost = 1000;
	taxAmount = 20;
};

datablock fxDTSBrickData(CRPGMediumLotBrickData : brick32x32FData)
{
	iconName = "Add-Ons/RP_RealEstate/shapes/BrickIcons/32x32LotIcon";
	
	category = "CRPG";
	subCategory = "CRPG Lots";
	
	uiName = "Medium Lot";
	
	CRPGBrickType = 1;
	adminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "32 32 64";
	trigger = 0;
	
	cost = 2000;
	taxAmount = 40;
};

datablock fxDTSBrickData(CRPGLargeLotBrickData : brick64x64FData)
{
	iconName = "Add-Ons/RP_RealEstate/shapes/BrickIcons/64x64LotIcon";
	
	category = "CRPG";
	subCategory = "CRPG Lots";
	
	uiName = "Large Lot";
	
	CRPGBrickType = 1;
	adminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "64 64 128";
	trigger = 0;
	
	cost = 8000;
	taxAmount = 80;
};

datablock fxDTSBrickData(CRPGSmalltLotBrickData : brick16x16FData)
{
	iconName = "Add-Ons/RP_RealEstate/shapes/BrickIcons/16x16LotIcon";
	
	category = "CRPG";
	subCategory = "Taxless Lots";
	
	uiName = "Small Taxless Lot";
	
	CRPGBrickType = 1;
	adminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 16 96";
	trigger = 0;
	
	cost = 750;
	taxAmount = 0;
};

datablock fxDTSBrickData(CRPGHalfSmalltLotBrickData : brick16x32FData)
{
	iconName = "Add-Ons/RP_RealEstate/shapes/BrickIcons/16x32LotIcon";
	
	category = "CRPG";
	subCategory = "Taxless Lots";
	
	uiName = "Half-Small Taxless Lot";
	
	CRPGBrickType = 1;
	adminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 32 96";
	trigger = 0;
	
	cost = 1500;
	taxAmount = 0;
};

datablock fxDTSBrickData(CRPGMediumtLotBrickData : brick32x32FData)
{
	iconName = "Add-Ons/RP_RealEstate/shapes/BrickIcons/32x32LotIcon";
	
	category = "CRPG";
	subCategory = "Taxless Lots";
	
	uiName = "Medium Taxless Lot";
	
	CRPGBrickType = 1;
	adminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "32 32 128";
	trigger = 0;
	
	cost = 3000;
	taxAmount = 0;
};

datablock fxDTSBrickData(CRPGLargetLotBrickData : brick64x64FData)
{
	iconName = "Add-Ons/RP_RealEstate/shapes/BrickIcons/64x64LotIcon";
	
	category = "CRPG";
	subCategory = "Taxless Lots";
	
	uiName = "Large Taxless Lot";
	
	CRPGBrickType = 1;
	adminOnly = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "64 64 256";
	trigger = 0;
	
	cost = 12000;
	taxAmount = 0;
};

// Sale Lots
datablock fxDTSBrickData(CRPGSmallZoneBrickData : brick16x16FData)
{
	iconName = "Add-Ons/RP_RealEstate/shapes/BrickIcons/16x16ZoneIcon";
	
	category = "CRPG";
	subCategory = "CRPG Zones";
	
	uiName = "Small Zone";

	adminOnly = true;
	CRPGMatchingLot = CRPGSmallLotBrickData;
};

datablock fxDTSBrickData(CRPGHalfSmallZoneBrickData : brick16x32FData)
{
	iconName = "Add-Ons/RP_RealEstate/shapes/BrickIcons/16x32ZoneIcon";
	
	category = "CRPG";
	subCategory = "CRPG Zones";
	
	uiName = "Half-Small Zone";
	
	adminOnly = true;
	CRPGMatchingLot = CRPGHalfSmallLotBrickData;
};

datablock fxDTSBrickData(CRPGMediumZoneBrickData : brick32x32FData)
{
	iconName = "Add-Ons/RP_RealEstate/shapes/BrickIcons/32x32ZoneIcon";
	
	category = "CRPG";
	subCategory = "CRPG Zones";
	
	uiName = "Medium Zone";
	
	adminOnly = true;
	CRPGMatchingLot = CRPGMediumLotBrickData;
};

datablock fxDTSBrickData(CRPGLargeZoneBrickData : brick64x64FData)
{
	iconName = "Add-Ons/RP_RealEstate/shapes/BrickIcons/64x64ZoneIcon";
	
	category = "CRPG";
	subCategory = "CRPG Zones";
	
	uiName = "Large Zone";
	
	adminOnly = true;
	CRPGMatchingLot = CRPGLargeLotBrickData;
};

// ============================================================
// Section 3 : Data Bricks
// ============================================================
exec("./bricks/info/realestate.cs");

function fxDTSBrick::handleCRPGBrickCreation(%brick, %data)
{
	if(!isObject(%brick.trigger))
	{
		%datablock = %brick.getDatablock();
		
		%trigX = getWord(%datablock.triggerSize, 0);
		%trigY = getWord(%datablock.triggerSize, 1);
		%trigZ = getWord(%datablock.triggerSize, 2);
		
		if(mFloor(getWord(%brick.rotation, 3)) == 90)
			%scale = (%trigY / 2) SPC (%trigX / 2) SPC (%trigZ / 2);
		else
			%scale = (%trigX / 2) SPC (%trigY / 2) SPC (%trigZ / 2);
			
		
		%brick.trigger = new trigger()
		{
			datablock = %datablock.triggerDatablock;
			position = getWords(%brick.getWorldBoxCenter(), 0, 1) SPC getWord(%brick.getWorldBoxCenter(), 2) + ((getWord(%datablock.triggerSize, 2) / 4) + (%datablock.brickSizeZ * 0.1));
			rotation = "1 0 0 0";
			scale = %scale;
			polyhedron = "-0.5 -0.5 -0.5 1 0 0 0 1 0 0 0 1";
			parent = %brick;
		};
		
		%boxSize = getWord(%scale, 0) / 2.5 SPC getWord(%scale, 1) / 2.5 SPC getWord(%scale, 2) / 2.5;
		
		initContainerBoxSearch(%brick.trigger.getWorldBoxCenter(), %boxSize, $typeMasks::playerObjectType);
		
		while(isObject(%player = containerSearchNext()))
			%brick.trigger.getDatablock().onEnterTrigger(%brick.trigger, %player);
		
		if(%brick.getDatablock().CRPGBrickType == 1)
		{
			getBrickGroupFromObject(%brick).taxes += %brick.getDatablock().taxAmount;
			getBrickGroupFromObject(%brick).lotsOwned++;
			
			if(isObject(getBrickGroupFromObject(%brick).client))
				getBrickGroupFromObject(%brick).client.displayInfo();
		}
	}
}

function fxDTSBrick::handleCRPGBrickDelete(%brick, %data)
{
	if(isObject(%brick.trigger))
	{
		for(%a = 0; %a < clientGroup.getCount(); %a++)
		{
			%subClient = ClientGroup.getObject(%a);
			if(isObject(%subClient.player) && %subClient.CRPGTrigger == %brick.trigger)
				%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, clientGroup.getObject(%a).player, true);
		}
		
		%boxSize = getWord(%brick.trigger.scale, 0) / 2.5 SPC getWord(%brick.trigger.scale, 1) / 2.5 SPC getWord(%brick.trigger.scale, 2) / 2.5;
		
		initContainerBoxSearch(%brick.trigger.getWorldBoxCenter(), %boxSize, $typeMasks::playerObjectType);
		
		while(isObject(%player = containerSearchNext()))
			%brick.trigger.getDatablock().onLeaveTrigger(%brick.trigger, %player);
		
		%brick.trigger.delete();
		
		if(%brick.getDatablock().CRPGBrickType == 1)
		{
			getBrickGroupFromObject(%brick).taxes -= %brick.getDatablock().taxAmount;
			getBrickGroupFromObject(%brick).lotsOwned--;
			
			if(isObject(getBrickGroupFromObject(%brick).client))
				getBrickGroupFromObject(%brick).client.displayInfo();
			
			if(%brick.getName() !$= "")
			{
				%brickGroup = getBrickGroupFromObject(%brick);
				%target = %brickGroup.client;
				%OwnerID = (!isObject(%target)) ? ($Server::serverType $= "Internet" ? %brickGroup.bl_id : %brickGroup.name) : %target.getSaveKey();
				
				%data = RPDB.get(%OwnerID, "lotData");
				
				%newData = getWord(%data, 0) - 1;
				
				for(%a = 1; %a < getWordCount(%data); %a++)
				{
					if(%cancelNext)
					{
						%cancelNext = false;
						continue;
					}
					
					%newData = %newData SPC getWord(%data, %a);
					
					if(%a % 2 == 0)
						continue;
					
					%newBrick = "_" @ getWord(%data, %a);
					if(isObject(%newBrick) && %newBrick $= %brick.getName())
					{
						%found = true;
						
						%newData = getWords(%newData, 0, getWordCount(%newData) - 2);
						
						%cancelNext = true;
					}
				}
				
				if(%found)
				{
					RPDB.set(%OwnerID, "lotData", %newData);
				}
			}
		}
	}	
}

function fxDTSBrick::transferLot(%brick, %targetBG)
{
	%ownerBG = getBrickGroupFromObject(%brick);
	
	if(%brick.tran || !isObject(%brick))
		return;
	
	if(isObject(%ownerBG))
		%ownerBG.remove(%brick);
	
	%targetBG.add(%brick);
	%brick.tran = 1;
	%brick.transferLot(%targetBG);
	
	for(%i = 0; %i < %brick.getNumUpBricks(); %i++)
	{
		%target = %brick.getUpBrick(%i);
		if(isObject(%target))
			%target.transferLot(%targetBG);
	}
}

package CRPG_MainRealEstatePackage
{
	// ============================================================
	// Section 1 : Brick Packages
	// ============================================================
	function fxDTSBrick::onActivate(%brick, %obj, %client, %pos, %dir)
	{
		parent::onActivate(%brick, %obj, %client, %pos, %dir);

		if(isObject(%brick.getDatablock().CRPGMatchingLot))
		{
			if(!isObject(%client.player.serviceOrigin))
			{
				%client.player.serviceType = "zone";
				%client.player.serviceOrigin = %brick;
				%client.player.serviceFee = %brick.getDatablock().CRPGMatchingLot.cost;
				messageClient(%client, '', '\c6It costs \c3%1\c6 to build in this zone. Type \c3/yes\c6 to accept and \c3/no\c6 to decline', %client.player.serviceFee);
			}
			else if(isObject(%client.player.serviceOrigin) && %client.player.serviceOrigin != %brick)
				messageClient(%client, '', "\c6You already have an active transfer. Type \c3/no\c6 to decline it.");
		}	
	}
	
	function fxDTSBrick::onDeath(%brick)
	{
		switch(%brick.getDatablock().CRPGBrickType)
		{
			case 1:
				%brick.handleCRPGBrickDelete();
			case 2:
				%brick.handleCRPGBrickDelete();
			case 3:
				if(getWord($CRPG::temp::spawnPoints, 0) == %brick)
					$CRPG::temp::spawnPoints = strReplace($CRPG::temp::spawnPoints, %brick @ " ", "");
				else
					$CRPG::temp::spawnPoints = strReplace($CRPG::temp::spawnPoints, " " @ %brick, "");
		}
		
		parent::onDeath(%brick);
	}
	
	function fxDTSBrick::onPlant(%brick)
	{		
		Parent::onPlant(%brick);
		
		if(isObject(%brick))
		{
			if(%brick == $LastLoadedBrick)
			{
				switch(%brick.getDatablock().CRPGBrickType)
				{
					case 1:
						%brick.schedule(1, "handleCRPGBrickCreation");
					case 2:
						%brick.schedule(1, "handleCRPGBrickCreation");
					case 3:
						$CRPG::temp::spawnPoints = ($CRPG::temp::spawnPoints $= "") ? %brick : $CRPG::temp::spawnPoints SPC %brick;
					case 4:
						%brick.resources = %brick.getDatablock().resources;
					case 6:
						%brick.schedule(1, "handleCRPGBrickCreation");
					case 10:
						%brick.schedule(1, "handleCRPGBrickCreation");
				}
			}
			else
			{
				if(isObject(%client = %brick.getGroup().client))
				{
					if(mFloor(getWord(%brick.rotation, 3)) == 90)
					{
						%boxSize = getWord(%brick.getDatablock().brickSizeY, 1) / 2.5 SPC getWord(%brick.getDatablock().brickSizeX, 0) / 2.5 SPC getWord(%brick.getDatablock().brickSizeZ, 2) / 2.5;
					}
					else
					{
						%boxSize = getWord(%brick.getDatablock().brickSizeX, 1) / 2.5 SPC getWord(%brick.getDatablock().brickSizeY, 0) / 2.5 SPC getWord(%brick.getDatablock().brickSizeZ, 2) / 2.5;
					}
					
					initContainerBoxSearch(%brick.getWorldBoxCenter(), %boxSize, $typeMasks::triggerObjectType);
					
					while(isObject(%trigger = containerSearchNext()))
					{
						if(%trigger.getDatablock() == CRPGLotTriggerData.getID())
						{
							%lotTrigger = %trigger;
						}
					}
					
					if(%lotTrigger && %brick.getDatablock().CRPGBrickType != 1)
					{
						%lotTriggerMinX = getWord(%lotTrigger.getWorldBox(), 0);
						%lotTriggerMinY = getWord(%lotTrigger.getWorldBox(), 1);
						%lotTriggerMinZ = getWord(%lotTrigger.getWorldBox(), 2);
						
						%lotTriggerMaxX = getWord(%lotTrigger.getWorldBox(), 3);
						%lotTriggerMaxY = getWord(%lotTrigger.getWorldBox(), 4);
						%lotTriggerMaxZ = getWord(%lotTrigger.getWorldBox(), 5);
						
						%brickMinX = getWord(%brick.getWorldBox(), 0) + 0.0016;
						%brickMinY = getWord(%brick.getWorldBox(), 1) + 0.0013;
						%brickMinZ = getWord(%brick.getWorldBox(), 2) + 0.00126;
						
						%brickMaxX = getWord(%brick.getWorldBox(), 3) - 0.0016;
						%brickMaxY = getWord(%brick.getWorldBox(), 4) - 0.0013;
						%brickMaxZ = getWord(%brick.getWorldBox(), 5) - 0.00126;
						
						
						if(%brickMinX >= %lotTriggerMinX && %brickMinY >= %lotTriggerMinY && %brickMinZ >= %lotTriggerMinZ)
						{
							if(%brickMaxX <= %lotTriggerMaxX && %brickMaxY <= %lotTriggerMaxY && %brickMaxZ <= %lotTriggerMaxZ)
							{
								if(%brick.getDatablock().adminOnly &&!%client.isAdmin)
								{
									commandToClient(%client, 'centerPrint', "You must be an admin to plant this brick.", 3);
									%brick.schedule(0, "delete");
								}
								else
								{
									switch(%brick.getDatablock().CRPGBrickType)
									{
										case 2:
											%brick.schedule(0, "handleCRPGBrickCreation");
										default:
											if(!%brick.brickGroup.client.isAdmin && %brick.getDatablock().getID() == brickVehicleSpawnData.getID())
											{
												if(%client.RPData.value["money"] >= mFloor($CRPG::prices::vehicleSpawn))
												{
													commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
													%client.RPData.value["money"] -= mFloor($CRPG::prices::vehicleSpawn);
													%client.displayInfo();
												}
												else
												{
													commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor($CRPG::prices::vehicleSpawn) SPC "\c6in order to plant this vehicle spawn!", 3);
													%brick.schedule(0, "delete");
												}
											}	
									}
								}
							}
							else
								%brick.schedule(0, "delete");
						}
						else
							%brick.schedule(0, "delete");
					}
					else if(%lotTrigger && %brick.getDatablock().CRPGBrickType == 1)
					{
						commandToClient(%client, 'centerPrint', "Only Chuck Norris can put a lot on top of another lot.", 3);
						%brick.schedule(0, "delete");
					}
					else if(!%lotTrigger && %brick.getDatablock().CRPGBrickType == 1)
					{
						if(%client.RPData.value["money"] >= mFloor(%brick.getDatablock().cost))
						{
							if(%client.brickGroup.lotsOwned < $CRPG::pref::realestate::maxLots)
							{
								%brick.schedule(0, "handleCRPGBrickCreation");
							}
							else
							{
								commandToClient(%client, 'centerPrint', "You already own the maximum number of lots.", 3);
								%brick.schedule(0, "delete");
							}
						}
						else
						{
							commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().cost) SPC "\c6in order to plant this lot!", 3);
							%brick.schedule(0, "delete");
						}
					}
					else if(!%lotTrigger)
					{
						if(!%client.isAdmin)
						{
							commandToClient(%client, 'centerPrint', "You cannot plant a brick outside of a lot.\n\c6Use a lot brick to start your build!", 3);
							%brick.schedule(0, "delete");
						}
						else
						{
							if(%brick.getDatablock().adminOnly &&!%client.isAdmin)
							{
								commandToClient(%client, 'centerPrint', "You must be an admin to plant this brick.", 3);
								%brick.schedule(0, "delete");
							}
							else
							{
								switch(%brick.getDatablock().CRPGBrickType)
								{
									case 2:
										%brick.schedule(0, "handleCRPGBrickCreation");
									case 3:
										$CRPG::temp::spawnPoints = ($CRPG::temp::spawnPoints $= "") ? %brick : $CRPG::temp::spawnPoints SPC %brick;
									default:
										if(%brick.getDatablock().getID() == brickVehicleSpawnData.getID())
										{
											if(%client.RPData.value["money"] >= mFloor($CRPG::prices::vehicleSpawn))
											{
												commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
												%client.RPData.value["money"] -= mFloor($CRPG::prices::vehicleSpawn);
											}
											else
											{
												commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor($CRPG::prices::vehicleSpawn) SPC "\c6in order to plant this vehicle spawn!", 3);
												%brick.schedule(0, "delete");
											}
										}	
								}
							}
						}
					}
					else
						messageAll('', "fxDTSBrick::onPlant() - Brick fell through tests!");
				}
				else
				{
					switch(%brick.getDatablock().CRPGBrickType)
					{
						case 1:
							%brick.schedule(0, "delete");
						case 2:
							%brick.schedule(0, "handleCRPGBrickCreation");
					}
				}
			}
		}
	}

	function fxDTSBrick::onLoadPlant(%brick)
	{		
		Parent::onLoadPlant(%brick);
		
		if(isObject(%brick))
		{
			if(%brick == $LastLoadedBrick)
			{
				switch(%brick.getDatablock().CRPGBrickType)
				{
					case 1:
						%brick.schedule(1, "handleCRPGBrickCreation");
					case 2:
						%brick.schedule(1, "handleCRPGBrickCreation");
					case 6:
						%brick.schedule(1, "handleCRPGBrickCreation");
					case 10:
						%brick.schedule(1, "handleCRPGBrickCreation");
				}
			}
			else
			{
				if(isObject(%client = %brick.getGroup().client))
				{
					if(mFloor(getWord(%brick.rotation, 3)) == 90)
					{
						%boxSize = getWord(%brick.getDatablock().brickSizeY, 1) / 2.5 SPC getWord(%brick.getDatablock().brickSizeX, 0) / 2.5 SPC getWord(%brick.getDatablock().brickSizeZ, 2) / 2.5;
					}
					else
					{
						%boxSize = getWord(%brick.getDatablock().brickSizeX, 1) / 2.5 SPC getWord(%brick.getDatablock().brickSizeY, 0) / 2.5 SPC getWord(%brick.getDatablock().brickSizeZ, 2) / 2.5;
					}
					
					initContainerBoxSearch(%brick.getWorldBoxCenter(), %boxSize, $typeMasks::triggerObjectType);
					
					while(isObject(%trigger = containerSearchNext()))
					{
						if(%trigger.getDatablock() == CRPGLotTriggerData.getID())
						{
							%lotTrigger = %trigger;
						}
					}
					
					if(%lotTrigger && %brick.getDatablock().CRPGBrickType != 1)
					{
						%lotTriggerMinX = getWord(%lotTrigger.getWorldBox(), 0);
						%lotTriggerMinY = getWord(%lotTrigger.getWorldBox(), 1);
						%lotTriggerMinZ = getWord(%lotTrigger.getWorldBox(), 2);
						
						%lotTriggerMaxX = getWord(%lotTrigger.getWorldBox(), 3);
						%lotTriggerMaxY = getWord(%lotTrigger.getWorldBox(), 4);
						%lotTriggerMaxZ = getWord(%lotTrigger.getWorldBox(), 5);
						
						%brickMinX = getWord(%brick.getWorldBox(), 0) + 0.0016;
						%brickMinY = getWord(%brick.getWorldBox(), 1) + 0.0013;
						%brickMinZ = getWord(%brick.getWorldBox(), 2) + 0.00126;
						
						%brickMaxX = getWord(%brick.getWorldBox(), 3) - 0.0016;
						%brickMaxY = getWord(%brick.getWorldBox(), 4) - 0.0013;
						%brickMaxZ = getWord(%brick.getWorldBox(), 5) - 0.00126;
						
						
						if(%brickMinX >= %lotTriggerMinX && %brickMinY >= %lotTriggerMinY && %brickMinZ >= %lotTriggerMinZ)
						{
							if(%brickMaxX <= %lotTriggerMaxX && %brickMaxY <= %lotTriggerMaxY && %brickMaxZ <= %lotTriggerMaxZ)
							{
								if(%brick.getDatablock().adminOnly &&!%client.isAdmin)
								{
									commandToClient(%client, 'centerPrint', "You must be an admin to plant this brick.", 3);
									%brick.schedule(0, "delete");
								}
								else
								{
									switch(%brick.getDatablock().CRPGBrickType)
									{
										case 2:
											%brick.schedule(0, "handleCRPGBrickCreation");
										default:
											if(!%brick.brickGroup.client.isAdmin && %brick.getDatablock().getID() == brickVehicleSpawnData.getID())
											{
												if(%client.RPData.value["money"] >= mFloor($CRPG::prices::vehicleSpawn))
												{
													commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
													%client.RPData.value["money"] -= mFloor($CRPG::prices::vehicleSpawn);
													%client.displayInfo();
												}
												else
												{
													commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor($CRPG::prices::vehicleSpawn) SPC "\c6in order to plant this vehicle spawn!", 3);
													%brick.schedule(0, "delete");
												}
											}	
									}
								}
							}
							else
								%brick.schedule(0, "delete");
						}
						else
							%brick.schedule(0, "delete");
					}
					else if(%lotTrigger && %brick.getDatablock().CRPGBrickType == 1)
					{
						commandToClient(%client, 'centerPrint', "Only Chuck Norris can put a lot on top of another lot.", 3);
						%brick.schedule(0, "delete");
					}
					else if(!%lotTrigger && %brick.getDatablock().CRPGBrickType == 1)
					{
						if(%client.RPData.value["money"] >= mFloor(%brick.getDatablock().cost))
						{
							if(%client.brickGroup.lotsOwned < $CRPG::pref::realestate::maxLots)
							{
								%brick.schedule(0, "handleCRPGBrickCreation");
							}
							else
							{
								commandToClient(%client, 'centerPrint', "You already own the maximum number of lots.", 3);
								%brick.schedule(0, "delete");
							}
						}
						else
						{
							commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().cost) SPC "\c6in order to plant this lot!", 3);
							%brick.schedule(0, "delete");
						}
					}
					else if(!%lotTrigger)
					{
						if(!%client.isAdmin)
						{
							commandToClient(%client, 'centerPrint', "You cannot plant a brick outside of a lot.\n\c6Use a lot brick to start your build!", 3);
							%brick.schedule(0, "delete");
						}
						else
						{
							if(%brick.getDatablock().adminOnly &&!%client.isAdmin)
							{
								commandToClient(%client, 'centerPrint', "You must be an admin to plant this brick.", 3);
								%brick.schedule(0, "delete");
							}
							else
							{
								switch(%brick.getDatablock().CRPGBrickType)
								{
									case 2:
										%brick.schedule(0, "handleCRPGBrickCreation");
									default:
										if(%brick.getDatablock().getID() == brickVehicleSpawnData.getID())
										{
											if(%client.RPData.value["money"] >= mFloor($CRPG::prices::vehicleSpawn))
											{
												commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
												%client.RPData.value["money"] -= mFloor($CRPG::prices::vehicleSpawn);
											}
											else
											{
												commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor($CRPG::prices::vehicleSpawn) SPC "\c6in order to plant this vehicle spawn!", 3);
												%brick.schedule(0, "delete");
											}
										}	
								}
							}
						}
					}
					else
						messageAll('', "fxDTSBrick::onLoadPlant() - Brick fell through tests!");
				}
				else
				{
					switch(%brick.getDatablock().CRPGBrickType)
					{
						case 1:
							%brick.schedule(0, "delete");
						case 2:
							%brick.schedule(0, "handleCRPGBrickCreation");
					}
				}
			}
		}
	}
	
	function fxDTSBrick::onRemove(%brick)
	{
		switch(%brick.getDatablock().CRPGBrickType)
		{
			case 1:
				%brick.handleCRPGBrickDelete();
			case 2:
				%brick.handleCRPGBrickDelete();
		}
		
		parent::onRemove(%brick);
	}
	
	// ============================================================
	// Section 2 : Client Packages
	// ============================================================	
	function gameConnection::onDeath(%client, %killerPlayer, %killer, %damageType, %unknownA)
	{	
		if(isObject(%client.CRPGTrigger))
			%client.CRPGTrigger.getDatablock().onLeaveTrigger(%client.CRPGTrigger, %client.player);

		parent::onDeath(%client, %player, %killer, %damageType, %unknownA);
	}

	// ============================================================
	// Section 5 : Chat Functions/Packages
	// ============================================================	
	function serverCmdmessageSent(%client, %text)
	{
		if(isObject(%client.player) && isObject(%client.CRPGTrigger) && isObject(%client.CRPGTrigger.parent) && %client.CRPGTrigger.parent.getDatablock().CRPGBrickType == 2)
			%client.CRPGTrigger.parent.getDatablock().parseData(%client.CRPGTrigger.parent, %client, "", %text);
	}
};
activatePackage(CRPG_MainRealEstatePackage);
