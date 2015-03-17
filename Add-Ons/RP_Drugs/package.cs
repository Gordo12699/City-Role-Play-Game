// ============================================================
// Project				:	CRPG
// Author				:	Iban & Trader & Jookia
// Description			:	Code that overwrites main functions of Blockland.
// ============================================================
// Table of Contents
// 1. Brick Packages
// 2. Client Packages
// 3. Player Packages
// 4. Misc. Packages
// 5. Chat Packages/Functions
// 6. Banned Events
// 8. Test Packages
// ============================================================

package CRPG_MainDrugPackage
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
	}
	
	function fxDTSBrick::onDeath(%brick)
	{
		if(%brick.getDataBlock().isDrug)
			{
				%brick.owner.RPData.value["drugamount"]--;
			
				if(isObject(getBrickGroupFromObject(%brick).client))
				{
					getBrickGroupFromObject(%brick).client.SetInfo();
				}
			}
			
		parent::onDeath(%brick);
	}
	
	function fxDTSBrick::onPlant(%brick)
	{		
		Parent::onPlant(%brick);
		
		if(isObject(%brick))
		{
			if(isObject(%client = %brick.getGroup().client))
			{
				switch(%brick.getDatablock().CRPGBrickType)
				{
					case 420:
						if(%brick.getDataBlock().CRPGBrickType == 420 || %brick.getDataBlock().CRPGBrickType == 420 && %client.isAdmin)
						{
							messageAll(%client,'',"\c6GOOO");
							if(%client.RPData.value["Money"] >= mFloor(%brick.getDatablock().price))
							{
								if(%client.RPData.value["drugamount"] <= $CRPG::drug::maxdrugplants)
								{
									%client.RPData.value["money"] -= mFloor(%brick.getDatablock().price);
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
										%drug.random = getRandom($CRPG::drugs::marijuana::harvestMin,$CRPG::drugs::marijuana::harvestMax);
										%drug.uiName = "Marijuana";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant a \c3Marijuana\c6 brick.");
									}
									else if(%brick.getDataBlock().drugType $= "opium")
									{
										%drug.random = getRandom($CRPG::drugs::opium::harvestMin,$CRPG::drugs::opium::harvestMax);
										%drug.uiName = "opium";
										messageClient(%client, '', "\c6You have paid \c3$" @ mFloor(%brick.getDatablock().price) @ "\c6 to plant an \c3Opium\c6 brick.");
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
					default:
				}
			}
		}
		else
			%brick.schedule(0, "delete");
	}
};
activatePackage(CRPG_MainDrugPackage);
