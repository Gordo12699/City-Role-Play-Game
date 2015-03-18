// Safe
// Info: The user have a safe where he or she can store (items) and money.

// Brick data
datablock fxDTSBrickData(SafeBrickData)
{
	brickFile = "Add-Ons/RP_Money/bricks/Safe.blb";
	iconName = "Add-ons/RP_Money/bricks/Icon_Safe";
	
	category = "CRPG";
	subCategory = "Miscallenous";
	
	uiName = "Safe";
	
	specialBrickType = "";
	
	cost = 5000;
	
	isInfoBrick = 1;
	adminOnly = 0;
};

// Brick data
datablock fxDTSBrickData(WallSafeBrickData)
{
	brickFile = "Add-Ons/RP_Money/bricks/WallSafe.blb";
	iconName = "Add-ons/RP_Money/bricks/Icon_Safe";
	
	category = "CRPG";
	subCategory = "Miscallenous";
	
	uiName = "Wall Safe";
	
	specialBrickType = "";
	
	cost = 5000;
	
	isInfoBrick = 1;
	adminOnly = 0;
};

// Brick functions

// Activate function
function SafeBrickData::Activate(%datablock, %client, %brick)
{
	%ID = %client.getSaveKey();
	
	%brickGroup = getBrickGroupFromObject(%brick);
	%target = %brickGroup.client;
	%OwnerID = (!isObject(%target)) ? ($Server::serverType $= "Internet" ? %brickGroup.bl_id : %brickGroup.name) : %target.getSaveKey();
	
	%OwnerName = RPDB.get(%OwnerID, "name");
	%client.safeType = 0; // What the user is doing
	
	%test = RPModsExists("Jobs Crime");
	%jobID = %client.RPData.value["jobID"];
	
	// Own the safe
	if (%ID == %OwnerID)
	{
		messageClient(%client, '', "\c3Your safe");
		messageClient(%client, '', '\c3Contains: \c3%1', prepareDisplayMoney(getMetricPrefix(%client.RPData.value["safe"])));
		messageClient(%client, '', '\c31. \c6Deposit %1', $RP::pref::moneyName);
		messageClient(%client, '', '\c32. \c6Withdraw %1', $RP::pref::moneyName);
		messageClient(%client, '', '\c33. \c6Deposit all %1', $RP::pref::moneyName);
		%client.safeType = 1;
	}
	// Burglary
	else if (%test && $RP::job::breakSafe[%jobID])
	{
		messageClient(%client, '', '\c6Safe owned by: \c3%1', %OwnerName);
		messageClient(%client, '', "\c31. \c6Break the safe");
		%client.safeType = 2;
	}
	// Not allowed
	else
	{
		messageClient(%client, '', "\c6This is not your safe.");
		return false;
	}
	
	%client.targetID = %OwnerID;
	%client.activateDatablock = %datablock;
	%client.activateBrick = %brick;
	%client.activateState = 0;
	return true;
}

// Handle data
function SafeBrickData::parseData(%datablock, %client, %text)
{
	%client.ThrowLastActive();
	%ID = %client.getSaveKey();
	%brick = %client.activateBrick;
	// Have not activated
	if (!isObject(%brick))
		return false;
	
	%number = atoi(%text);
	%OwnerID = %client.targetID;
	%client.targetID = "";
	
	switch (%client.safeType)
	{
		// The owner
		case 1:
			switch (%client.activateState)
			{
				case 0:
					switch (%number)
					{
						// Insert money
						case 1:
							messageClient(%client, '', '\c6Insert the amount of %1 to be deposited to your safe.', $RP::pref::moneyName);
							%client.activateState = 1.1;
						// Remove money
						case 2:
							messageClient(%client, '', '\c6Insert the amount of %1 to be withdrawn from your safe.', $RP::pref::moneyName);
							%client.activateState = 2.1;
						// Insert all money
						case 3:
							%amount = %client.RPData.value["money"];
							%client.RPData.value["safe"] += %amount;
							%client.RPData.value["money"] = 0;
							messageClient(%client, '', '\c6You have deposited \c3%1\c6 into your safe.', prepareDisplayMoney(getMetricPrefix(%amount)));
							%client.stopActivate();
							%client.safeType = 0;
						default:
							%client.stopActivate();
							%client.safeType = 0;
							return false;
					}
				// Insert money
				case 1.1:
					if (%number > 0)
					{
						%number = (%number > %client.RPData.value["money"]) ? %client.RPData.value["money"] : %number;
						%client.RPData.value["safe"] += %number;
						%client.RPData.value["money"] -= %number;
						messageClient(%client, '', '\c6You have deposited \c3%1\c6 into your safe.', prepareDisplayMoney(getMetricPrefix(%number)));
					}
					%client.stopActivate();
					%client.safeType = 0;
				// Remove money
				case 2.1:
					if (%number > 0)
					{
						%number = (%number > %client.RPData.value["safe"]) ? %client.RPData.value["safe"] : %number;
						%client.RPData.value["money"] += %number;
						%client.RPData.value["safe"] -= %number;
						messageClient(%client, '', '\c6You have withdrawn \c3%1\c6 from your safe.', prepareDisplayMoney(getMetricPrefix(%number)));
					}
					%client.stopActivate();
					%client.safeType = 0;
				default:
					%client.stopActivate();
					%client.safeType = 0;
					return false;
			}
		// Burglary
		case 2:
			switch (%client.activateState)
			{
				case 0:
					switch (%number)
					{
						// Break the safe
						case 1:
							%exp = %client.RPData.value["CrimeExp"];
							%failed = 99 - (%exp / 40);
							%failed = mAbs(%failed);
							%amount = getRandom(0, %exp);
							%safe = RPDB.get(%OwnerID, "safe");
							%amount = (%amount > %safe) ? %safe : %amount;
							// Can break safe?
							if (getRandom(0, 100) < %failed)
							{
								// Enough money in safe or got any money
								if (%amount > 0)
								{
									RPDB.set(%OwnerID, "safe", RPDB.get(%OwnerID, "safe") - %amount);
									
									%client.RPData.value["money"] += %amount; // Add money to player
									%client.RPData.value["stolenMoney"] += %amount; // Add stolen money to player
									%client.RPData.value["exp"] += mFloor(%amount / 10); // Give the player experience
									%client.RPData.value["CrimeEXP"] += mFloor(%amount / 100);
									messageClient(%client, '', '\c6You stole \c3%1\c6 from the safe.', prepareDisplayMoney(getMetricPrefix(%amount)));
									CommitCrime(%client, "Stealing", mFloor(%amount / 2));
								}
								// Did not find anything
								else
									messageClient(%client, '', "\c6You did not find anything.");
							}
							%client.stopActivate();
							%client.safeType = 0;
						default:
							%client.stopActivate();
							%client.safeType = 0;
							return false;
					}
				default:
					%client.stopActivate();
					%client.safeType = 0;
					return false;
			}
		default:
			%client.stopActivate();
			%client.safeType = 0;
			return false;
	}
	return true;
}

//Wall Safe
// Activate function
function WallSafeBrickData::Activate(%datablock, %client, %brick)
{
	%ID = %client.getSaveKey();
	
	%brickGroup = getBrickGroupFromObject(%brick);
	%target = %brickGroup.client;
	%OwnerID = (!isObject(%target)) ? ($Server::serverType $= "Internet" ? %brickGroup.bl_id : %brickGroup.name) : %target.getSaveKey();
	
	%OwnerName = RPDB.get(%OwnerID, "name");
	%client.safeType = 0; // What the user is doing
	
	%test = RPModsExists("Jobs Crime");
	%jobID = %client.RPData.value["jobID"];
	
	// Own the safe
	if (%ID == %OwnerID)
	{
		messageClient(%client, '', "\c3Your safe");
		messageClient(%client, '', '\c3Contains: \c3%1', prepareDisplayMoney(getMetricPrefix(%client.RPData.value["safe"])));
		messageClient(%client, '', '\c31. \c6Deposit %1', $RP::pref::moneyName);
		messageClient(%client, '', '\c32. \c6Withdraw %1', $RP::pref::moneyName);
		messageClient(%client, '', '\c33. \c6Deposit all %1', $RP::pref::moneyName);
		%client.safeType = 1;
	}
	// Burglary
	else if (%test && $RP::job::breakSafe[%jobID])
	{
		messageClient(%client, '', '\c6Safe owned by: \c3%1', %OwnerName);
		messageClient(%client, '', "\c31. \c6Break the safe");
		%client.safeType = 2;
	}
	// Not allowed
	else
	{
		messageClient(%client, '', "\c6This is not your safe.");
		return false;
	}
	
	%client.targetID = %OwnerID;
	%client.activateDatablock = %datablock;
	%client.activateBrick = %brick;
	%client.activateState = 0;
	return true;
}

// Handle data
function WallSafeBrickData::parseData(%datablock, %client, %text)
{
	%client.ThrowLastActive();
	%ID = %client.getSaveKey();
	%brick = %client.activateBrick;
	// Have not activated
	if (!isObject(%brick))
		return false;
	
	%number = atoi(%text);
	%OwnerID = %client.targetID;
	%client.targetID = "";
	
	switch (%client.safeType)
	{
		// The owner
		case 1:
			switch (%client.activateState)
			{
				case 0:
					switch (%number)
					{
						// Insert money
						case 1:
							messageClient(%client, '', '\c6Insert the amount of %1 to be deposited to your safe.', $RP::pref::moneyName);
							%client.activateState = 1.1;
						// Remove money
						case 2:
							messageClient(%client, '', '\c6Insert the amount of %1 to be withdrawn from your safe.', $RP::pref::moneyName);
							%client.activateState = 2.1;
						// Insert all money
						case 3:
							%amount = %client.RPData.value["money"];
							%client.RPData.value["safe"] += %amount;
							%client.RPData.value["money"] = 0;
							messageClient(%client, '', '\c6You have deposited \c3%1\c6 into your safe.', prepareDisplayMoney(getMetricPrefix(%amount)));
							%client.stopActivate();
							%client.safeType = 0;
						default:
							%client.stopActivate();
							%client.safeType = 0;
							return false;
					}
				// Insert money
				case 1.1:
					if (%number > 0)
					{
						%number = (%number > %client.RPData.value["money"]) ? %client.RPData.value["money"] : %number;
						%client.RPData.value["safe"] += %number;
						%client.RPData.value["money"] -= %number;
						messageClient(%client, '', '\c6You have deposited \c3%1\c6 into your safe.', prepareDisplayMoney(getMetricPrefix(%number)));
					}
					%client.stopActivate();
					%client.safeType = 0;
				// Remove money
				case 2.1:
					if (%number > 0)
					{
						%number = (%number > %client.RPData.value["safe"]) ? %client.RPData.value["safe"] : %number;
						%client.RPData.value["money"] += %number;
						%client.RPData.value["safe"] -= %number;
						messageClient(%client, '', '\c6You have withdrawn \c3%1\c6 from your safe.', prepareDisplayMoney(getMetricPrefix(%number)));
					}
					%client.stopActivate();
					%client.safeType = 0;
				default:
					%client.stopActivate();
					%client.safeType = 0;
					return false;
			}
		// Burglary
		case 2:
			switch (%client.activateState)
			{
				case 0:
					switch (%number)
					{
						// Break the safe
						case 1:
							%exp = %client.RPData.value["CrimeExp"];
							%failed = 99 - (%exp / 40);
							%failed = mAbs(%failed);
							%amount = getRandom(0, %exp);
							%safe = RPDB.get(%OwnerID, "safe");
							%amount = (%amount > %safe) ? %safe : %amount;
							// Can break safe?
							if (getRandom(0, 100) < %failed)
							{
								// Enough money in safe or got any money
								if (%amount > 0)
								{
									RPDB.set(%OwnerID, "safe", RPDB.get(%OwnerID, "safe") - %amount);
									
									%client.RPData.value["money"] += %amount; // Add money to player
									%client.RPData.value["stolenMoney"] += %amount; // Add stolen money to player
									%client.RPData.value["exp"] += mFloor(%amount / 10); // Give the player experience
									%client.RPData.value["CrimeEXP"] += mFloor(%amount / 100);
									messageClient(%client, '', '\c6You stole \c3%1\c6 from the safe.', prepareDisplayMoney(getMetricPrefix(%amount)));
									CommitCrime(%client, "Stealing", mFloor(%amount / 2));
								}
								// Did not find anything
								else
									messageClient(%client, '', "\c6You did not find anything.");
							}
							%client.stopActivate();
							%client.safeType = 0;
							return;
						default:
							%client.stopActivate();
							%client.safeType = 0;
							return false;
					}
				default:
					%client.stopActivate();
					%client.safeType = 0;
					return false;
			}
		default:
			%client.stopActivate();
			%client.safeType = 0;
			return false;
	}
	return true;
}
