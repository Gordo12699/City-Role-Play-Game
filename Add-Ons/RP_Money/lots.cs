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
	iconName = "Add-Ons/RP_Money/shapes/BrickIcons/16x16LotIcon";
	
	category = "CRPG";
	subCategory = "CRPG Lots";
	
	uiName = "Small Lot";
	
	CRPGBrickType = 1;
	CRPGBrickAdmin = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 16 4800";
	trigger = 0;
	
	initialPrice = 500;
	taxAmount = 10;
};

datablock fxDTSBrickData(CRPGHalfSmallLotBrickData : brick16x32FData)
{
	iconName = "Add-Ons/RP_Money/shapes/BrickIcons/16x32LotIcon";
	
	category = "CRPG";
	subCategory = "CRPG Lots";
	
	uiName = "Half-Small Lot";
	
	CRPGBrickType = 1;
	CRPGBrickAdmin = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "16 32 4800";
	trigger = 0;
	
	initialPrice = 750;
	taxAmount = 15;
};

datablock fxDTSBrickData(CRPGMediumLotBrickData : brick32x32FData)
{
	iconName = "Add-Ons/RP_Money/shapes/BrickIcons/32x32LotIcon";
	
	category = "CRPG";
	subCategory = "CRPG Lots";
	
	uiName = "Medium Lot";
	
	CRPGBrickType = 1;
	CRPGBrickAdmin = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "32 32 6400";
	trigger = 0;
	
	initialPrice = 1500;
	taxAmount = 25;
};

datablock fxDTSBrickData(CRPGLargeLotBrickData : brick64x64FData)
{
	iconName = "Add-Ons/RP_Money/shapes/BrickIcons/64x64LotIcon";
	
	category = "CRPG";
	subCategory = "CRPG Lots";
	
	uiName = "Large Lot";
	
	CityRPGBrickType = 1;
	CityRPGBrickAdmin = false;
	
	triggerDatablock = CRPGLotTriggerData;
	triggerSize = "64 64 12800";
	trigger = 0;
	
	initialPrice = 4500;
	taxAmount = 60;
};

// Sale Lots
//datablock fxDTSBrickData(CityRPGSmallZoneBrickData : brick16x16FData)
//{
//	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x16ZoneIcon";
	
//	category = "CityRPG";
//	subCategory = "CityRPG Zones";
	
//	uiName = "Small Zone";

//	CityRPGBrickAdmin = true;
//	CityRPGMatchingLot = CityRPGSmallLotBrickData;
//};

//datablock fxDTSBrickData(CityRPGHalfSmallZoneBrickData : brick16x32FData)
//{
//	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/16x32ZoneIcon";
	
//	category = "CityRPG";
//	subCategory = "CityRPG Zones";
	
//	uiName = "Half-Small Zone";
	
//	CityRPGBrickAdmin = true;
//	CityRPGMatchingLot = CityRPGHalfSmallLotBrickData;
//};

//datablock fxDTSBrickData(CityRPGMediumZoneBrickData : brick32x32FData)
//{
//	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/32x32ZoneIcon";
	
//	category = "CityRPG";
//	subCategory = "CityRPG Zones";
	
//	uiName = "Medium Zone";
	
//	CityRPGBrickAdmin = true;
//	CityRPGMatchingLot = CityRPGMediumLotBrickData;
//};

//datablock fxDTSBrickData(CityRPGLargeZoneBrickData : brick64x64FData)
//{
//	iconName = "Add-Ons/Gamemode_TysCityRPG/shapes/BrickIcons/64x64ZoneIcon";
	
//	category = "CityRPG";
//	subCategory = "CityRPG Zones";
	
//	uiName = "Large Zone";
	
//	CityRPGBrickAdmin = true;
//	CityRPGMatchingLot = CityRPGLargeLotBrickData;
//};

function CityRPG_AssembleEvents()
 {
	registerInputEvent("fxDTSBrick", "RPonEnterLot", "Self fxDTSBrick" TAB "Player player" TAB "Client gameConnection");
	registerInputEvent("fxDTSBrick", "RPonLeaveLot", "Self fxDTSBrick" TAB "Player player" TAB "Client gameConnection");
 }
 
 function fxDTSBrick::RPonEnterLot(%brick, %obj)
{
	$inputTarget_self = %brick;
 	 $inputTarget_client = %obj.client;
	$inputTarget_player = %obj.client.player;
 	 $inputTarget_miniGame = (isObject(getMiniGameFromObject(%obj.client))) ? getMiniGameFromObject(%obj.client) : 0;
 	 %brick.processInputEvent("RPonEnterLot", %obj.client);
}

function fxDTSBrick::RPonLeaveLot(%brick, %obj)
		{
		$inputTarget_self = %brick;
 	 	$inputTarget_client = %obj.client;
		$inputTarget_player = %obj.client.player;
 	 	$inputTarget_miniGame = (isObject(getMiniGameFromObject(%obj.client))) ? getMiniGameFromObject(%obj.client) : 0;
 	 	%brick.processInputEvent("RPonLeaveLot", %obj.client);
		}
		
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
				getBrickGroupFromObject(%brick).client.updateInfo();
		}
		
		if(%brick.getDatablock().CRPGBrickType == 10)
 		{
		
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
		
		if(%brick.getDatablock().CRPGBrickType == 420)
 		{
			getBrickGroupFromObject(%brick).RPData.value["drugAmount"]--;
			
			if(isObject(getBrickGroupFromObject(%brick).client))
				getBrickGroupFromObject(%brick).client.updateInfo();
 		}
		
		if(%brick.getDatablock().CRPGBrickType == 1)
 		{
			getBrickGroupFromObject(%brick).taxes -= %brick.getDatablock().taxAmount;
			getBrickGroupFromObject(%brick).lotsOwned--;
			
			if(isObject(getBrickGroupFromObject(%brick).client))
				getBrickGroupFromObject(%brick).client.SetInfo();
				
			if(%brick.getName() !$= "")
			{
				%data = getBrickGroupFromObject(%brick).RPData.value["lotData"];//CityRPGData.getData(getBrickGroupFromObject(%brick).bl_id).valueLotData;
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
					getBrickGroupFromObject(%brick).RPData.value["lotData"] = %newData;
				}
			}
 		}
 		else if(%brick.getDatablock().CRPGBrickType == 420)
 		{
			%client.RPData.value["drugAmount"]--;
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

function gameConnection::sellZone(%client, %sellerID, %brick, %price)
	{
		if(isObject(%brick) && %brick.getClassName() $= "fxDTSBrick" && %client.RPData.value["money"] >= %price)
		{
			if(%client.brickGroup.lotsOwned < $CRPG::pref::realestate::maxLots)
			{
				%brick.setDatablock(%brick.getDatablock().CRPGMatchingLot);
				%client.brickGroup.add(%brick);
				%brick.handleCRPGBrickCreation();
				
				messageClient(%client, '', '\c6You have purchased a \c3%1\c6 for $%2', %brick.getDatablock().uiName, %price);
				%client.RPData.value["money"] -= %price;
				if(%price)
				{
					if(isObject(%seller = FindClientByBL_ID(%sellerID)))
					{
						messageClient(%seller, '', '\c6You just gained \c3$%1\c6 for selling a zone to \c3%2\c6.', %price, %client.name); 
						%seller.RPData.value["bank"] += %price;
					}
				}
				
				%client.updateInfo();
			}
			else 
				messageClient(%client, '', "\c6You already own enough lots.");
		}
	}
	
	function CRPGLotTriggerData::onEnterTrigger(%this, %trigger, %obj)
	{
		parent::onEnterTrigger(%this, %trigger, %obj);
		
		if(!isObject(%obj.client))
		{
			if(isObject(%obj.getControllingClient()))
				%client = %obj.getControllingClient(); 
			else 
				return;
		}
		else
		
		%client = %obj.client;
		%trigger.parent.onEnterLot(%obj);
		%client.CRPGTrigger = %trigger;
		%client.CRPGLotBrick = %trigger.parent;
		%client.updateInfo();
	} 
	
	function CRPGLotTriggerData::onLeaveTrigger(%this, %trigger, %obj)
	{
		if(!isObject(%obj.client))
		{
			if(isObject(%obj.getControllingClient()))
				%client = %obj.getControllingClient();
			else
				return; 
		} 
		else 
		
		%client = %obj.client; 
		%trigger.parent.onLeaveLot(%obj);
		%client.CRPGTrigger = "";
		%client.CRPGLotBrick = "";
		%client.updateInfo();
	}
	
	function CRPGInputTriggerData::onEnterTrigger(%this, %trigger, %obj)
	{
		if(!isObject(%obj.client))
		{
			return;
		} 
		
		%obj.client.CRPGTrigger = %trigger;
		%trigger.parent.getDatablock().parseData(%trigger.parent, %obj.client, true, "");
	}
	
	function CRPGInputTriggerData::onLeaveTrigger(%this, %trigger, %obj, %a)
	{ 	
		if(!isObject(%obj.client))
		{
			return;
		} 	
		
		if(%obj.client.CRPGTrigger == %trigger)
		{ 	
			%trigger.parent.getDatablock().parseData(%trigger.parent, %obj.client, false, ""); 
			%obj.client.CRPGTrigger = "";
		} 
	}

function serverCmdhelp(%client, %section, %term)
	{	
		if(!isObject(%client.player)) 
			return; 
			
		if(!isObject(CRPGHelp)) 
		{ 	
			echo("CRPG :: Creating new Hellen on Request."); 
			new scriptObject(CRPGHelp) 
			{ 		
				class = Hellen;
			};
		} 
		
		if(%section $= "") 	
		{ 	
			CRPGHelp.displayHelp(%client, "main"); 
			
			if(%client.isAdmin)
				CRPGHelp.displayHelp(%client, "main_adminline"); 
		} 
		else 
		{ 		
			if(%term) 
				messageClient(%client, '', "\c6You must specify a term. Usage: \c3/help [section] [term]\c6."); 
			else 
			{ 		
				%query = %section @ "_" @ %term; 
				
				if(%section $= "list") 		
				{ 	
					if(%term $= "jobs") 
					{ 				
						messageClient(%client, '', "\c6Type \c3/job\c6 then one of the jobs below to apply for that job."); 	
						for(%a = 1; %a <= JobSO.getJobCount(); %a++) 		
						{ 		
							messageClient(%client, '', "\c3" @ JobSO.job[%a].name SPC "\c6- Inital Investment: \c3" @ JobSO.job[%a].invest SPC "- \c6Pay: \c3" @ JobSO.job[%a].pay SPC "- \c6Required Education: \c3" @ JobSO.job[%a].education); 	
							messageClient(%client, '', JobSO.job[%a].helpline); 	
						} 		
					} 		
					else if(%term $= "lots") 
					{ 			
						messageClient(%client, '', "\c6This is a list of lots you can plant."); 	
						for(%a = 0; %a < datablockGroup.getCount(); %a++) 	
						{ 					
							%datablock = datablockGroup.getObject(%a);
							if(%datablock.CRPGBrickType == 1) 		
							{ 					
								messageClient(%client, '', "\c3" @ %datablock.uiName SPC "\c6- Size: \c3" @ %datablock.brickSizeX @ "x" @ %datablock.brickSizeY SPC "\c6Cost: \c3" @ %datablock.initialPrice SPC "\c6Tax: \c3" SPC %datablock.taxAmount); 	
							} 			
						} 		
					} 				
					else if(%term $= "items") 
					{ 				
						messageClient(%client, '', "\c6This is a list of item prices."); 	
						for(%a = 1; %a <= $ListAmt; %a++) 	
						{ 					
							messageClient(%client, '', "\c3" @ $CRPG::prices::weapon::name[%a].uiName SPC "\c6- \c3$" @ $CRPG::prices::weapon::price[%a]); 
						} 			
					} 			
					else if(%term $= "food") 	
					{ 				
						messageClient(%client, '', "\c6This is a list of all food stuff."); 
						for(%a = 12; %a > 0; %a--) 	
						{ 						
							messageClient(%client, '', '\c3%1\c6 - \c3$%2', $CRPG::food::name[%a], mFloor((%a) * 4));
						} 			
					} 			
					else 			
						messageClient(%client, '', "\c6You did not request a real list."); 	
				} 		
				else 	
				{ 				
					if(!CRPGHelp.displayHelp(%client, %query)) 
					{ 				
						messageClient(%client, '', "\c6Help doc '\c3" @ %query @ "\c6' does not exist\c6!"); 
					} 	
				} 	
			} 	
		}
	}

package CRPG_MainPackage
{
	// ============================================================
	// Section 1 : Brick Packages
	// ============================================================
	function fxDTSBrick::onActivate(%brick, %obj, %client, %pos, %dir)
	{
		parent::onActivate(%brick, %obj, %client, %pos, %dir);
	  	if(%brick.getDataBlock().hasDrug == false)
	  	{
			if(%brick.getDataBlock().isDrug == true)
			{
		  		%drug = %brick.getID();
	      		if(%drug.watered == 0)
	  	  		{
				 	%drug.watered = 1;
					
					if(%client.bl_id == %brick.client.bl_id || %client.name == %brick.client.getPlayerName())
					{
						messageClient(%client,'',"\c6You watered your \c3" @ %brick.getDataBlock().uiName @ " \c6plant.");
					}
					else
					{
						messageClient(%client,'',"\c6You watered someones \c3" @ %brick.getDataBlock().uiName @ " \c6plant.");
					}
					
				 	%drug.startGrowing(%drug,%brick);
	  	  		}
	  	  		else if(%drug.watered == 1)
				{
					if(%drug.hasDrug)
						commandToClient(%client,'centerPrint',"\c6This \c3" @ %brick.getDataBlock().uiName @ " \c6plant is ready to be harvested!",1);
					else
		      			commandToClient(%client,'centerPrint',"\c6This plant is already watered.",1);
				}
			}
		}
		if(isObject(%brick.getDatablock().CRPGMatchingLot))
		{
			if(!isObject(%client.player.serviceOrigin))
			{
				%client.player.serviceType = "zone";
				%client.player.serviceOrigin = %brick;
				%client.player.serviceFee = %brick.getDatablock().CRPGMatchingLot.initialPrice;
				messageClient(%client, '', '\c6It costs \c3%1\c6 to build in this zone. Type \c3/yes\c6 to accept and \c3/no\c6 to decline', %client.player.serviceFee);
			}
			else if(isObject(%client.player.serviceOrigin) && %client.player.serviceOrigin != %brick)
				messageClient(%client, '', "\c6You already have an active transfer. Type \c3/no\c6 to decline it.");
		}	
	}
	
	function fxDTSBrick::onDeath(%brick)
	{
		if(%brick.getDataBlock().isDrug)
		{
			%brick.client.RPData.value["drugAmount"]--;
			
			if(isObject(getBrickGroupFromObject(%brick).client))
			{
				getBrickGroupFromObject(%brick).client.updateInfo();
			}
		}
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
								    if(%brick.getDatablock().CRPGBrickAdmin && !%client.isAdmin)
							    {
                                    if(%brick.getDatablock().CRPGBrickPlayerPrivliage)
                                    {
                                        if(%client.RPData.value["money"] >= %brick.getDatablock().CRPGBrickCost)
                                        {
                                            messageClient(%client,'',"\c6You have bought the brick for \c3$" @ %brick.getDatablock().CRPGBrickCost @ "\c6.");
                                            %client.RPData.value["money"] -= %brick.getDatablock().CRPGBrickCost;
                                            switch(%brick.getDatablock().CRPGBrickType)
								            {
									            case 2:
										            %brick.schedule(0, "handleCRPGBrickCreation");
									            case 3:
										            $CRPG::temp::spawnPoints = ($CRPG::temp::spawnPoints $= "") ? %brick : $CRPG::temp::spawnPoints SPC %brick;
									            case 4:
										            if(%brick.getDatablock().resources)
										            {
											            %brick.resources = %brick.getDatablock().resources;
										            }
									            default:
										            if(%brick.getDatablock().getID() == brickVehicleSpawnData.getID())
										            {
											            if(CRPGData.getData(%client.bl_id).valueMoney >= mFloor($CRPG::prices::vehicleSpawn))
											            {
												            commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
												            %client.RPData.value["money"] -= $CRPG::prices::vehicleSpawn;
															schedule(3000, 0, removeMoney, %brick, %client, mFloor($CRPG::prices::vehicleSpawn));
											            }
											            else
											            {
												            commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor($CRPG::prices::vehicleSpawn) SPC "\c6in order to plant this vehicle spawn!", 3);
												            %brick.schedule(0, "delete");
											            }
										            }	
								            }
                                        } else {
								            %brick.schedule(0, "delete");
                                            commandToClient(%client, 'centerPrint', "\c6You don't have enough money to buy this brick! \c3($" @ %brick.getDatablock().CRPGBrickCost @ ")", 3);
                                        }
                                    } else {
								        commandToClient(%client, 'centerPrint', "You must be an admin to plant this brick. You aren't able to buy this brick either.", 3);
								        %brick.schedule(0, "delete");
                                    }
							    }else
								    {
									    switch(%brick.getDatablock().CRPGBrickType)
									    {
										    case 2:
											    %brick.schedule(0, "handleCRPGBrickCreation");
										    case 3:
											    $CRPG::temp::spawnPoints = ($CRPG::temp::spawnPoints $= "") ? %brick : $CRPG::temp::spawnPoints SPC %brick;
										    case 4:
											    if(%brick.getDatablock().resources)
												    %brick.resources = %brick.getDatablock().resources;
										    case 420:
					    if(%lotTrigger && %brick.getDataBlock().CRPGBrickType == 420 || !%lotTrigger && %brick.getDataBlock().CRPGBrickType == 420 && %client.isAdmin)
					    {
						    if(!%lotTrigger && !%client.isAdmin)
						    {
							    commandToClient(%client, 'centerPrint', "You cannot plant a brick outside of a lot.\n\c6Use a lot brick to start your build!", 3);
							    messageAll(%client,'',"\c6ERRORRRR");
							    %brick.schedule(0, "delete");
							    return;
						    }
						    else if(%lotTrigger && %client.isAdmin || %lotTrigger && !%client.isAdmin || !%lotTrigger && %client.isAdmin)
						    {
							    messageAll(%client,'',"\c6GOOO");
							    if(%client.RPData.value["money"] >= mFloor(%brick.getDatablock().price))
							    {
								    if(%client.RPData.value["drugAmount"] <= $CRPG::drug::maxdrugplants)
								    {
										schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
										%client.RPData.value["money"] -= %brick.getDatablock().price;
									    %client.RPData.value["drugAmount"]++;
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
										    %drug.random = getRandom($CRPG::drugs::marijuana::harvestMin,$CRPG::drugs::marijuana::harvestMax);
										    %drug.uiName = "Marijuana";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
									    }
									    else if(%brick.getDataBlock().drugType $= "opium")
									    {
										    %drug.random = getRandom($CRPG::drugs::opium::harvestMin,$CRPG::drugs::opium::harvestMax);
										    %drug.uiName = "opium";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									    }
									    else if(%brick.getDataBlock().drugType $= "speed")
								        {
									        %drug.random = getRandom($CRPG::drugs::speed::harvestMin,$CRPG::drugs::speed::harvestMax);
									        %drug.uiName = "speed";
									        messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
								        }
								        else if(%brick.getDataBlock().drugType $= "steroid")
								        {
									        %drug.random = getRandom($CRPG::drugs::steroid::harvestMin,$CRPG::drugs::steroid::harvestMax);
									        %drug.uiName = "steroid";
									        messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
								        }
                                        %drug.canbecolored = false;
									    %drug.setEmitter("None");
									    %drug.cansetemitter = false;
									    %drug.emitterr = "GrassEmitter";
									    %drug.canchange = false;
								    }
								    else
								    {
									    commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									    %brick.schedule(0, "delete");
								    }
							    }
							    else
							    {
								    commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								    %brick.schedule(0, "delete");
							    }
						    }
						    else
						    {
							    if(%client.RPData.value["money"] >= mFloor(%brick.getDatablock().price))
							    {
								    if(%client.RPData.value["drugAmount"] < $CRPG::drug::maxdrugplants)
								    {
										schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
										%client.RPData.value["money"] -= %brick.getDatablock().price;
									    %client.RPData.value["drugAmount"]++;//CityRPGData.getData(%client.bl_id).valuedrugamount++;
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
										    %drug.random = getRandom($CRPG::drugs::marijuana::harvestMin,$CRPG::drugs::marijuana::harvestMax);
										    %drug.uiName = "Marijuana";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
									    }
									    else if(%brick.getDataBlock().drugType $= "opium")
									    {
										    %drug.random = getRandom($CRPG::drugs::opium::harvestMin,$CRPG::drugs::opium::harvestMax);
										    %drug.uiName = "opium";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									    }
                                        else if(%brick.getDataBlock().drugType $= "speed")
									    {
										    %drug.random = getRandom($CRPG::drugs::speed::harvestMin,$CRPG::drugs::speed::harvestMax);
										    %drug.uiName = "speed";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
									    }
									    else if(%brick.getDataBlock().drugType $= "steroid")
									    {
										    %drug.random = getRandom($CRPG::drugs::steroid::harvestMin,$CRPG::drugs::steroid::harvestMax);
										    %drug.uiName = "steroid";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
									    }
									    %drug.canbecolored = false;
									    %drug.setEmitter("None");
									    %drug.cansetemitter = false;
									    %drug.emitterr = "GrassEmitter";
									    %drug.canchange = false;
								    }
								    else
								    {
									    commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									    %brick.schedule(0, "delete");
								    }
							    }
							    else
							    {
								    commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								    %brick.schedule(0, "delete");
							    }
						    }
					    }
										    default:
											    if(!%brick.brickGroup.client.isAdmin && %brick.getDatablock().getID() == brickVehicleSpawnData.getID())
											    {
												    if(%client.RPData.value["money"] >= mFloor($CRPG::prices::vehicleSpawn))
												    {
													    commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
													    %client.RPData.value["money"] -= $CRPG::prices::vehicleSpawn;
														schedule(3000, 0, removeMoney, %brick, %client, mFloor($CRPG::prices::vehicleSpawn));
													    %client.updateInfo();
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
					    else if(%lotTrigger && %brick.getDataBlock().CRPGBrickType == 420 || !%lotTrigger && %brick.getDataBlock().CRPGBrickType == 420 && %client.isAdmin)
					    {
						    if(!%lotTrigger && !%client.isAdmin)
						    {
							    commandToClient(%client, 'centerPrint', "You cannot plant a brick outside of a lot.\n\c6Use a lot brick to start your build!", 3);
							    %brick.schedule(0, "delete");
							    return;
						    }
						    else if(%lotTrigger && %client.isAdmin || %lotTrigger && !%client.isAdmin || !%lotTrigger && %client.isAdmin)
						    {
							    if(%client.RPData.value["Money"] >= mFloor(%brick.getDatablock().price))
							    {
								    if(%client.RPData.value["drugAmount"] <= $CRPG::drug::maxdrugplants)
								    {
										schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
										%client.RPData.value["money"] -= %brick.getDatablock().price;
									    %client.RPData.value["drugAmount"]++;//CRPGData.getData(%client.bl_id).valuedrugamount++;
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
										    %drug.random = getRandom($CRPG::drugs::marijuana::harvestMin,$CRPG::drugs::marijuana::harvestMax);
										    %drug.uiName = "Marijuana";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
									    }
									    else if(%brick.getDataBlock().drugType $= "opium")
									    {
										    %drug.random = getRandom($CRPG::drugs::opium::harvestMin,$CityRPG::drugs::opium::harvestMax);
										    %drug.uiName = "opium";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									    }
                                        else if(%brick.getDataBlock().drugType $= "speed")
									    {
										    %drug.random = getRandom($CRPG::drugs::speed::harvestMin,$CityRPG::drugs::speed::harvestMax);
										    %drug.uiName = "speed";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
									    }
									    else if(%brick.getDataBlock().drugType $= "steroid")
									    {
										    %drug.random = getRandom($CRPG::drugs::steroid::harvestMin,$CityRPG::drugs::steroid::harvestMax);
										    %drug.uiName = "steroid";
										    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
									    }
									    %drug.canbecolored = false;
									    %drug.setEmitter("None");
									    %drug.cansetemitter = false;
									    %drug.emitterr = "GrassEmitter";
									    %drug.canchange = false;
								    }
								    else
								    {
									    commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									    %brick.schedule(0, "delete");
								    }
							    }
							    else
							    {
								    commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								    %brick.schedule(0, "delete");
							    }
						    }
					    }
					    else if(%lotTrigger && %brick.getDatablock().CRPGBrickType == 1)
					    {
						    commandToClient(%client, 'centerPrint', "Only Chuck Norris can put a lot on top of another lot.", 3);
						    %brick.schedule(0, "delete");
					    }
					    else if(!%lotTrigger && %brick.getDatablock().CRPGBrickType == 1)
					    {
						    if(%client.RPData.value["money"] >= mFloor(%brick.getDatablock().initialPrice))
						    {
							    if(%client.brickGroup.lotsOwned < $CRPG::pref::realestate::maxLots)
							    {
								    messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().initialPrice) @ "\c6 to plant this lot.");
                                    %client.RPData.value["money"] -= %brick.getDatablock().initialPrice;
									schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().initialPrice));
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
							    commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().initialPrice) SPC "\c6in order to plant this lot!", 3);
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
							    if(%brick.getDatablock().CRPGBrickAdmin && !%client.isAdmin)
							    {
                                    if(%brick.getDatablock().CRPGBrickPlayerPrivliage)
                                    {
                                        if(%client.RPData.value["money"] >= %brick.getDatablock().CityRPGBrickCost)
                                        {
                                            messageClient(%client,'',"\c6You have bought the brick for \c3$" @ %brick.getDatablock().CRPGBrickCost @ "\c6.");
                                            %client.RPData.value["money"] -= %brick.getDatablock().CRPGBrickCost;
                                            switch(%brick.getDatablock().CRPGBrickType)
								            {
									            case 2:
										            %brick.schedule(0, "handleCRPGBrickCreation");
									            case 3:
										            $CRPG::temp::spawnPoints = ($CRPG::temp::spawnPoints $= "") ? %brick : $CRPG::temp::spawnPoints SPC %brick;
									            case 4:
										            if(%brick.getDatablock().resources)
										            {
											            %brick.resources = %brick.getDatablock().resources;
										            }
									            default:
										            if(%brick.getDatablock().getID() == brickVehicleSpawnData.getID())
										            {
											            if(%client.RPData.value["money"] >= mFloor($CRPG::prices::vehicleSpawn))
											            {
												            commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
												            %client.RPData.value["money"] -= $CRPG::prices::vehicleSpawn;
															schedule(3000, 0, removeMoney, %brick, %client, mFloor($CRPG::prices::vehicleSpawn));
											            }
											            else
											            {
												            commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor($CRPG::prices::vehicleSpawn) SPC "\c6in order to plant this vehicle spawn!", 3);
												            %brick.schedule(0, "delete");
											            }
										            }	
								            }
                                        } else {
								            %brick.schedule(0, "delete");
                                            commandToClient(%client, 'centerPrint', "\c6You don't have enough money to buy this brick! \c3($" @ %brick.getDatablock().CRPGBrickCost @ ")", 3);
                                        }
                                    } else {
								        commandToClient(%client, 'centerPrint', "You must be an admin to plant this brick. You arn't able to buy this brick either.", 3);
								        %brick.schedule(0, "delete");
                                    }
							    }
							    else
							    {
								    switch(%brick.getDatablock().CRPGBrickType)
								    {
									    case 2:
										    %brick.schedule(0, "handleCRPGBrickCreation");
									    case 3:
										    $CityRPG::temp::spawnPoints = ($CRPG::temp::spawnPoints $= "") ? %brick : $CRPG::temp::spawnPoints SPC %brick;
									    case 4:
										    if(%brick.getDatablock().resources)
										    {
											    %brick.resources = %brick.getDatablock().resources;
										    }
									    default:
										    if(%brick.getDatablock().getID() == brickVehicleSpawnData.getID())
										    {
											    if(%client.RPData.value["money"] >= mFloor($CRPG::prices::vehicleSpawn))
											    {
												    commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
												    %client.RPData.value["money"] -= $CRPG::prices::vehicleSpawn;
													schedule(3000, 0, removeMoney, %brick, %client, mFloor($CRPG::prices::vehicleSpawn));
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
						    case 3:
							    $CityRPG::temp::spawnPoints = ($CRPG::temp::spawnPoints $= "") ? %brick : $CRPG::temp::spawnPoints SPC %brick;
						    case 4:
							    %brick.resources = %brick.getDatablock().resources;
					    }
				    }
			    }
		    }
	}
	
	function fxDTSBrick::onRemove(%brick,%client)
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
			case 420:
				%brick.handleCRPGBrickDelete();
				
		}
		
		parent::onRemove(%brick);
	}
	
	function fxDTSBrick::setVehicle(%brick, %vehicle)
	{
		if(%brick.getDatablock().getName() !$= "CRPGPoliceVehicleData")
		{
			if(!isObject(%brick.getGroup().client) || !%brick.getGroup().client.isAdmin)
			{
				if(isObject(%vehicle))
				{
					for(%a = 0; $CRPG::vehicles::banned[%a] !$= "" && !%hasBeenBanned; %a++)
					{
						if(%vehicle.getName() $= $CRPG::vehicles::banned[%a])
						{
							if(isObject(%brick.getGroup().client))
							{
								messageClient(%brick.getGroup().client, '', "\c6Standard users may not spawn a\c3" SPC %vehicle.uiName @ "\c6.");
							}
							%vehicle = 0;
							%hasBeenBanned = true;
						}
					}
				}
			}
		}
		
		parent::setVehicle(%brick, %vehicle);
	}
	
	function removeMoney(%col, %client, %arg1)
    {
        if(!%col.isPlanted())
        {
            %client.RPData.value["money"] += %arg1;
            messageClient(%client, '', "Your money has been returned to you because you were unable to plant the lot!");
        }
    }
	
	 function fxDTSBrick::onLoadPlant(%this, %brick) 
    {
        parent::onLoadPlant(%this, %brick);
		
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
								if(%brick.getDatablock().CRPGBrickAdmin &&!%client.isAdmin)
								{
									commandToClient(%client, 'centerPrint', "You must be an admin to plant this brick.3", 3);
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
										case 4:
											if(%brick.getDatablock().resources)
												%brick.resources = %brick.getDatablock().resources;
										case 420:
					if(%lotTrigger && %brick.getDataBlock().CRPGBrickType == 420 || !%lotTrigger && %brick.getDataBlock().CRPGBrickType == 420 && %client.isAdmin)
					{
						if(!%lotTrigger && !%client.isAdmin)
						{
							commandToClient(%client, 'centerPrint', "You cannot plant a brick outside of a lot.\n\c6Use a lot brick to start your build!", 3);
							messageAll(%client,'',"\c6ERRORRRR");
							%brick.schedule(0, "delete");
							return;
						}
						else if(%lotTrigger && %client.isAdmin || %lotTrigger && !%client.isAdmin || !%lotTrigger && %client.isAdmin)
						{
							messageAll(%client,'',"\c6GOOO");
							if(%client.RPData.value["money"] >= mFloor(%brick.getDatablock().price))
							{
								if(%client.RPData.value["drugAmount"] <= $CRPG::drug::maxdrugplants)
								{
									schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
									%client.RPData.value["money"] -= %brick.getDatablock().price;
									%client.RPData.value["drugAmount"]++;//CityRPGData.getData(%client.bl_id).valuedrugamount++;
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
										%drug.random = getRandom($CRPG::drugs::marijuana::harvestMin,$CRPG::drugs::marijuana::harvestMax);
										%drug.uiName = "Marijuana";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
									}
									else if(%brick.getDataBlock().drugType $= "opium")
									{
										%drug.random = getRandom($CRPG::drugs::opium::harvestMin,$CRPG::drugs::opium::harvestMax);
										%drug.uiName = "opium";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									}
									else if(%brick.getDataBlock().drugType $= "speed")
									{
										%drug.random = getRandom($CRPG::drugs::speed::harvestMin,$CRPG::drugs::speed::harvestMax);
										%drug.uiName = "speed";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
									}
									else if(%brick.getDataBlock().drugType $= "steroid")
									{
										%drug.random = getRandom($CRPG::drugs::steroid::harvestMin,$CRPG::drugs::steroid::harvestMax);
										%drug.uiName = "steroid";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
									}
									%drug.canbecolored = false;
									%drug.setEmitter("None");
									%drug.cansetemitter = false;
									%drug.emitterr = "GrassEmitter";
									%drug.canchange = false;
								}
								else
								{
									commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									%brick.schedule(0, "delete");
								}
							}
							else
							{
								commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								%brick.schedule(0, "delete");
							}
						}
						else
						{
							if(%client.RPData.value["money"] >= mFloor(%brick.getDatablock().price))
							{
								if(%client.RPData.value["drugAmount"] < $CRPG::drug::maxdrugplants)
								{
									schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
									%client.RPData.value["money"] -= %brick.getDatablock().price;
									%client.RPData.value["drugAmount"]++;//CityRPGData.getData(%client.bl_id).valuedrugamount++;
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
										%drug.random = getRandom($CRPG::drugs::marijuana::harvestMin,$CRPG::drugs::marijuana::harvestMax);
										%drug.uiName = "Marijuana";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
									}
									else if(%brick.getDataBlock().drugType $= "opium")
									{
										%drug.random = getRandom($CRPG::drugs::opium::harvestMin,$CRPG::drugs::opium::harvestMax);
										%drug.uiName = "opium";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									}
                                    else if(%brick.getDataBlock().drugType $= "speed")
									{
										%drug.random = getRandom($CRPG::drugs::speed::harvestMin,$CRPG::drugs::speed::harvestMax);
										%drug.uiName = "speed";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
									}
									else if(%brick.getDataBlock().drugType $= "steroid")
									{
										%drug.random = getRandom($CRPG::drugs::steroid::harvestMin,$CRPG::drugs::steroid::harvestMax);
										%drug.uiName = "steroid";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
									}
									%drug.canbecolored = false;
									%drug.setEmitter("None");
									%drug.cansetemitter = false;
									%drug.emitterr = "GrassEmitter";
									%drug.canchange = false;
								}
								else
								{
									commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									%brick.schedule(0, "delete");
								}
							}
							else
							{
								commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								%brick.schedule(0, "delete");
							}
						}
					}
										default:
											if(!%brick.brickGroup.client.isAdmin && %brick.getDatablock().getID() == brickVehicleSpawnData.getID())
											{
												if(%client.RPData.value["money"] >= mFloor($CRPG::prices::vehicleSpawn))
												{
													commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
													%client.RPData.value["money"] -= $CRPG::prices::vehicleSpawn;
													schedule(3000, 0, removeMoney, %brick, %client, mFloor($CRPG::prices::vehicleSpawn));
													%client.setInfo();
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
					else if(%lotTrigger && %brick.getDataBlock().CRPGBrickType == 420 || !%lotTrigger && %brick.getDataBlock().CRPGBrickType == 420 && %client.isAdmin)
					{
						if(!%lotTrigger && !%client.isAdmin)
						{
							commandToClient(%client, 'centerPrint', "You cannot plant a brick outside of a lot.\n\c6Use a lot brick to start your build!", 3);
							%brick.schedule(0, "delete");
							return;
						}
						else if(%lotTrigger && %client.isAdmin || %lotTrigger && !%client.isAdmin || !%lotTrigger && %client.isAdmin)
						{
							if(%client.RPData.value["money"] >= mFloor(%brick.getDatablock().price))
							{
								if(%client.RPData.value["drugAmount"] <= $CRPG::drug::maxdrugplants)
								{
									schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().price));
									%client.RPData.value["money"] -= %brick.getDatablock().price;
									%client.RPData.value["drugAmount"]++;//CityRPGData.getData(%client.bl_id).valuedrugamount++;
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
										%drug.random = getRandom($CRPG::drugs::marijuana::harvestMin,$CRPG::drugs::marijuana::harvestMax);
										%drug.uiName = "Marijuana";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick. Use by: /usemarijuana");
									}
									else if(%brick.getDataBlock().drugType $= "opium")
									{
										%drug.random = getRandom($CRPG::drugs::opium::harvestMin,$CRPG::drugs::opium::harvestMax);
										%drug.uiName = "opium";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
									}
									else if(%brick.getDataBlock().drugType $= "speed")
									{
										%drug.random = getRandom($CRPG::drugs::speed::harvestMin,$CRPG::drugs::speed::harvestMax);
										%drug.uiName = "speed";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Speed\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usespeed");
									}
									else if(%brick.getDataBlock().drugType $= "steroid")
									{
										%drug.random = getRandom($CRPG::drugs::steroid::harvestMin,$CRPG::drugs::steroid::harvestMax);
										%drug.uiName = "steroid";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Steroid\c6 brick. This drug can't be sold, only dropped to other players. Use by: /usesteroid");
									}
                                    %drug.canbecolored = false;
									%drug.setEmitter("None");
									%drug.cansetemitter = false;
									%drug.emitterr = "GrassEmitter";
									%drug.canchange = false;
								}
								else
								{
									commandToClient(%client, 'centerPrint', "\c6You have met the limit of drugs you can plant.", 1);
									%brick.schedule(0, "delete");
								}
							}
							else
							{
								commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().price) SPC "\c6in order to plant this.", 1);
								%brick.schedule(0, "delete");
							}
						}
					}
					else if(%lotTrigger && %brick.getDatablock().CRPGBrickType == 1)
					{
						commandToClient(%client, 'centerPrint', "Only Chuck Norris can put a lot on top of another lot.", 3);
						%brick.schedule(0, "delete");
					}
					else if(!%lotTrigger && %brick.getDatablock().CRPGBrickType == 1)
					{
						if(%client.RPData.value["money"] >= mFloor(%brick.getDatablock().initialPrice))
						{
							if(%client.brickGroup.lotsOwned < $CRPG::pref::realestate::maxLots)
							{
								messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().initialPrice) @ "\c6 to plant this lot.");
								%client.RPData.value["money"] -= %brick.getDatablock().initialPrice;
								schedule(3000, 0, removeMoney, %brick, %client, mFloor(%brick.getDatablock().initialPrice));
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
							commandToClient(%client, 'centerPrint', "\c6You need at least \c3$" @ mFloor(%brick.getDatablock().initialPrice) SPC "\c6in order to plant this lot!", 3);
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
							if(%brick.getDatablock().CRPGBrickAdmin &&!%client.isAdmin)
							{
								commandToClient(%client, 'centerPrint', "You must be an admin to plant this brick.4", 3);
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
									case 4:
										if(%brick.getDatablock().resources)
										{
											%brick.resources = %brick.getDatablock().resources;
										}
									default:
										if(%brick.getDatablock().getID() == brickVehicleSpawnData.getID())
										{
											if(%client.RPData.value["money"] >= mFloor($CRPG::prices::vehicleSpawn))
											{
												commandToClient(%client, 'centerPrint', "\c6You have paid \c3$" @ mFloor($CRPG::prices::vehicleSpawn) @ "\c6 to plant this vehicle spawn.", 3);
												%client.RPData.value["money"] -= $CRPG::prices::vehicleSpawn;
												schedule(3000, 0, removeMoney, %brick, %client, mFloor($CRPG::prices::vehicleSpawn));
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
						case 3:
							$CRPG::temp::spawnPoints = ($CRPG::temp::spawnPoints $= "") ? %brick : $CRPG::temp::spawnPoints SPC %brick;
						case 4:
							%brick.resources = %brick.getDatablock().resources;
					}
				}
			}
		}
    }
	
	function GameConnection::autoadmincheck(%client)
	{
		//multi-client check
		for(%a = 0; %a < ClientGroup.getCount(); %a++)
		{
			%subClient = ClientGroup.getObject(%a);
			
			if(%client.bl_id == %subClient.bl_id)
			{
				if(%client.getID() > %subClient.getID())
				{
					%subClient.delete();
				}
			}
		}
		parent::autoadmincheck(%client);
	}
	
	function gameConnection::setScore(%client, %score)
	{
		if($Score::Type $= "Money")
			%score = CityRPGData.getData(%client.bl_id).valueMoney + CityRPGData.getData(%client.bl_id).valueBank;
		else if($Score::Type $= "Edu")
			%score = CityRPGData.getData(%client.bl_id).valueEducation;
		else
			%score = %client.RPData.value["money"] + %client.RPData.value["bank"];
		parent::setScore(%client, %score);
	}
	
	};
deactivatePackage(CRPG_MainPackage);
activatepackage(CRPG_MainPackage);
