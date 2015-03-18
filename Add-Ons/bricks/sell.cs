// Resource Brick

// Brick Data
datablock fxDtsBrickData(ResourceBrickData)
{
	brickfile = "./TvOrange.blb";
	iconName = "Add-Ons/RP_Core/LcdTV";	
	
	category = "CRPG";
	subcategory = "Admin event bricks";
	
	uiName = "Resource brick";
	
	specialBrickType = "";
	
	isInfoBrick = 1;
	adminOnly = 1;
};

// Brick functions

// Activate data
function ResourceBrickData::Activate(%datablock, %client)
{
	if(deadBrick(%datablock))
		return;
	
	%ore = %client.RPData.value["ore"];
	%wood = %client.RPData.value["wood"];
	%fish = %client.RPData.value["fish"];
	// No resources
	if (%ore <= 0 && %wood <= 0 && %fish <= 0)
	{
		%client.activateDatablock = 0;
		messageClient(%client, '', "\c3You have no resources.");
		return false;
	}
	messageClient(%client, '', "\c3Resources");
	// You have ores
	if (%ore > 0)
		messageClient(%client, '', "\c31. \c6Sell ores");
	// You have wood
	if (%wood > 0)
		messageClient(%client, '', "\c32. \c6Sell logs");
	// You have fish
	if (%fish > 0)
		messageClient(%client, '', "\c33. \c6Sell fish");
	if (%ore > 0 || %wood > 0 || %fish > 0)
		messageClient(%client, '', "\c34. \c6Sell all");
	%client.activateDatablock = %datablock;
	%client.activateState = 0;
	return true;
}

// Handle data
function ResourceBrickData::parseData(%datablock, %client, %text)
{	
	%client.ThrowLastActive();
	%ID = %client.bl_id;
	
	%number = mFloor(%text);
	switch (%client.activateState)
	{
		// Start
		case 0:
			switch (%number)
			{
				// Sell ores
				case 1:
					%ore = %client.RPData.value["ore"] * $RP::pref::gain::money::ore;
					if (%ore == 0)
						return false;
					
					$CRPG::citydata::minerals += %client.RPData.value["ore"];
					%client.RPData.value["ore"] = 0;
					if($RP::job::group[%client.RPData.value["jobID"]] $= "999999")
					{
						%client.RPData.value["LaborExp"] += 1;
						%client.RPData.value["money"] += %ore;
						messageClient(%client, '', '\c6You sold \c3ores\c6 for \c3%1\c6.', prepareDisplayMoney(%ore));
					}
					else
					{
						messageClient(%client, '', '\c6You did not receive a payment for your services because you are not in the laboring career.');
					}
					%client.displayInfo();
					%client.stopActivate();
				// Sell logs
				case 2:
					%wood = %client.RPData.value["wood"] * $RP::pref::gain::money::wood;
					if (%wood == 0)
						return false;
					
					$CRPG::citydata::lumber += %client.RPData.value["wood"];
					%client.RPData.value["wood"] = 0;
					if($RP::job::group[%client.RPData.value["jobID"]] $= "999999")
					{
						%client.RPData.value["LaborExp"] += 1;
						%client.RPData.value["money"] += %wood;
						messageClient(%client, '', '\c6You sold \c3logs\c6 for \c3%1\c6.', prepareDisplayMoney(%wood));
					}
					else
					{
						messageClient(%client, '', '\c6You did not receive a payment for your services because you are not in the laboring career.');
					}
					%client.displayInfo();
					%client.stopActivate();
				// Sell fish
				case 3:
					%fish = %client.RPData.value["fish"] * $RP::pref::gain::money::fish;
					if (%fish == 0)
						return false;
					
					%client.RPData.value["fish"] = 0;
					if($RP::job::group[%client.RPData.value["jobID"]] $= "999999")
					{
						%client.RPData.value["LaborExp"] += 1;
						%client.RPData.value["money"] += %fish;
						messageClient(%client, '', '\c6You sold \c3fish\c6 for \c3%1\c6.', prepareDisplayMoney(%fish));
					}
					else
					{
						messageClient(%client, '', '\c6You did not receive a payment for your services because you are not in the laboring career.');
					}
					%client.displayInfo();
					%client.stopActivate();
				// Sell all
				case 4:
					// Ore
					%ore = %client.RPData.value["ore"] * $RP::pref::gain::money::ore;
					if (%ore > 0)
					{
						$CRPG::citydata::minerals += %client.RPData.value["ore"];
						%client.RPData.value["ore"] = 0;
						if($RP::job::group[%client.RPData.value["jobID"]] $= "999999")
						{
							%client.RPData.value["LaborExp"] += 1;
							%client.RPData.value["money"] += %ore;
							messageClient(%client, '', '\c6You sold \c3ores\c6 for \c3%1\c6.', prepareDisplayMoney(%ore));
						}
						else
						{
							messageClient(%client, '', '\c6You did not receive a payment for your services because you are not in the laboring career.');
						}
					}
					
					// Wood
					%wood = %client.RPData.value["wood"] * $RP::pref::gain::money::wood;
					if (%wood > 0)
					{
						$CRPG::citydata::lumber += %client.RPData.value["wood"];
						%client.RPData.value["wood"] = 0;
						if($RP::job::group[%client.RPData.value["jobID"]] $= "999999")
						{
							%client.RPData.value["LaborExp"] += 1;
							%client.RPData.value["money"] += %wood;
							messageClient(%client, '', '\c6You sold \c3logs\c6 for \c3%1\c6.', prepareDisplayMoney(%wood));
						}
						else
						{
							messageClient(%client, '', '\c6You did not receive a payment for your services because you are not in the laboring career.');
						}
					}
					
					// Fish
					%fish = %client.RPData.value["fish"] * $RP::pref::gain::money::fish;
					if (%fish > 0)
					{
						%client.RPData.value["fish"] = 0;
						if($RP::job::group[%client.RPData.value["jobID"]] $= "999999")
						{
							%client.RPData.value["LaborExp"] += 1;
							%client.RPData.value["money"] += %fish;
							messageClient(%client, '', '\c6You sold \c3fish\c6 for \c3%1\c6.', prepareDisplayMoney(%fish));
						}
						else
						{
							messageClient(%client, '', '\c6You did not receive a payment for your services because you are not in the laboring career.');
						}
					}
					%client.displayInfo();
					%client.stopActivate();
				// Invalid
				default:
					%client.stopActivate();
					return false;
			}
		// Invalid
		default:
			%client.stopActivate();
			return false;
	}
	return true;
}
