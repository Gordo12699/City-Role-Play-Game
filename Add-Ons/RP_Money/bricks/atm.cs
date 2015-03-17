// Bank Brick

// Brick Data
datablock fxDtsBrickData(ATMBrickData)
{
	brickFile = "Add-Ons/RP_Money/bricks/ATM.blb";
	iconName = "Add-ons/RP_Money/bricks/ATM";
	hasPrint=1;
	printAspectRatio="2x2r";
	
	category = "CRPG";
	subcategory = "Event bricks";
	
	uiName = "ATM Brick";
	
	specialBrickType = "";
	
	cost = 2500;
	
	isInfoBrick = 1;
	adminOnly = 0;
};

datablock fxDtsBrickData(ATM2BrickData)
{
	brickFile = "Add-Ons/RP_Money/bricks/ATM2.blb";
	iconName = "Add-ons/RP_Money/bricks/ATM";
	hasPrint=1;
	printAspectRatio="2x2r";
	
	category = "CRPG";
	subcategory = "Event bricks";
	
	uiName = "ATM Brick - Alternate";
	
	specialBrickType = "";
	
	cost = 2500;
	
	isInfoBrick = 1;
	adminOnly = 0;
};

// Brick functions

// Activate data
function ATMBrickData::Activate(%datablock, %client)
{
	messageClient(%client, '', "\c3ATM");
	messageClient(%client, '', '\c6You have \c3%1\c6 in bank account.', prepareDisplayMoney(getMetricPrefix(%client.RPData.value["bank"])));
	messageClient(%client, '', '\c31. \c6Withdraw %1', $RP::pref::moneyName);
	messageClient(%client, '', '\c32. \c6Hack ATM');
	%client.activateDatablock = %datablock;
	%client.activateState = 0;
	return true;
}

function ATM2BrickData::Activate(%datablock, %client)
{
	messageClient(%client, '', "\c3ATM");
	messageClient(%client, '', '\c6You have \c3%1\c6 in bank account.', prepareDisplayMoney(getMetricPrefix(%client.RPData.value["bank"])));
	messageClient(%client, '', '\c31. \c6Withdraw %1', $RP::pref::moneyName);
	messageClient(%client, '', '\c32. \c6Hack ATM');
	%client.activateDatablock = %datablock;
	%client.activateState = 0;
	return true;
}

// Handle data
function ATMBrickData::parseData(%datablock, %client, %text)
{
	%client.ThrowLastActive();
	
	%number = atoi(%text);
	switch (%client.activateState)
	{
		// Start
		case 0:
			switch (%number)
			{
				// Withdraw
				case 1:
					messageClient(%client, '', '\c6Insert the amount of \c3%1\c6 to be withdrawn.', $RP::pref::moneyName);
					%client.activateState = 1.1;
				case 2:
					if(!(ClientGroup.getCount() > 3))
					{
						messageClient(%client,'',"\c6The server must have at least 4 players to be able to do this action.");
						return;
					}
					if(%client.AccessableATM == 1) 
					{
						messageClient(%client,'',"\c6You need to wait a while before hacking again. 7 seconds from the last hack.");
						return;
					}
					else
					{
						if(%client.RPData.value["education"] >= 3)
						{
							%stealchance = getRandom(1,2);
							%caughtchance = getRandom(1,4);
							%lockoutchance = getRandom(1,3);
							%beencaught = 1;
							if(%stealchance == 1)
							{
								if(%lockoutchance != 1)
								{
									%stolen = getRandom($ATM::Min,$ATM::Max);
									messageClient(%client,'',"\c6You managed to steal \c3$" @ %stolen @ "\c6 from the ATM.");
									%client.RPData.value["money"] += %stolen;
									%client.AccessableATM = 1;
									//%client.RPData.value["demerits"] += $ATM::Demerits;
									schedule(7000,0,"resetAccessableATM",%client);
									if(%caughtchance != 1)
									{
										messageAll('',"\c3" @ %client.name @ "\c6 has been caught hacking an ATM!");
										//CityRPGData.getData(%client.bl_id).valueDemerits += $ATM::Demerits;
									}
								}
								else
								{
									%this.hackable = 0;
									messageClient(%client,'',"\c6Your attempt failed, and you have been locked out of the machine.");
                            
								}
							}
							else 
							{
								messageClient(%client,'',"\c6Failed to hack.");
							}
						}
						else
						{
							messageClient(%client,'',"\c6Your education level must be \c3" @ $CRPG::pref::hack::education @ "\c6.");
						}
						return;
					}
				// Invalid
				default:
					%client.stopActivate();
					return false;
			}
		// Withdraw money
		case 1.1:
			%client.stopActivate();
			if (%number <= 0)
				return false;
			
			%number = (%number > %client.RPData.value["bank"]) ? %client.RPData.value["bank"] : %number;
			messageClient(%client, '', '\c6You have withdrawn \c3%1\c6.', prepareDisplayMoney(getMetricPrefix(%number)));
			%client.RPData.value["bank"] -= %number;
			%client.RPData.value["money"] += %number;
		// Invalid
		default:
			%client.stopActivate();
			return false;
	}
	return true;
}

function ATM2BrickData::parseData(%datablock, %client, %text)
{
	%client.ThrowLastActive();
	
	%number = atoi(%text);
	switch (%client.activateState)
	{
		// Start
		case 0:
			switch (%number)
			{
				// Withdraw
				case 1:
					messageClient(%client, '', '\c6Insert the amount of \c3%1\c6 to be withdrawn.', $RP::pref::moneyName);
					%client.activateState = 1.1;
				// Invalid
				default:
					%client.stopActivate();
					return false;
			}
		// Withdraw money
		case 1.1:
			%client.stopActivate();
			if (%number <= 0)
				return false;
			
			%number = (%number > %client.RPData.value["bank"]) ? %client.RPData.value["bank"] : %number;
			messageClient(%client, '', '\c6You have withdrawn \c3%1\c6.', prepareDisplayMoney(getMetricPrefix(%number)));
			%client.RPData.value["bank"] -= %number;
			%client.RPData.value["money"] += %number;
		// Invalid
		default:
			%client.stopActivate();
			return false;
	}
	return true;
}
