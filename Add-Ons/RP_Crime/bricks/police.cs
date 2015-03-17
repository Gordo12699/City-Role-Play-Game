// Police Brick

// Brick Data
datablock fxDtsBrickData(PoliceBrickData)
{
	brickfile = "./TvNavy.blb";
	iconName = "Add-Ons/RP_Core/LcdTV";	
	
	category = "CRPG";
	subcategory = "Admin event bricks";
	
	uiName = "Police Brick";
	
	specialBrickType = "";
	
	isInfoBrick = 1;
	adminOnly = 1;
};

// Brick functions

function PoliceBrickData::Activate(%datablock, %client)
{
	%dems = %client.RPData.value["demerits"];
	%job = %client.RPData.value["jobID"];
	%money = %client.RPData.value["money"];
	%stolen = %client.RPData.value["stolenMoney"];
	
	messageClient(%client, '', "\c3Police Department");
	
	// User got dems
	if (%dems > 0)
		messageClient(%client, '', '\c6You have \c3%1\c6 demerits.', %dems);
		
	messageClient(%client, '', "\c31. \c6Wanted criminals");

	if ($RP::job::pardon[%job] && getWordCount($RP::jailList) > 0)
		messageClient(%client, '', "\c32. \c6Pardon");
		
	// Clear record
	if ($RP::job::clearRecord[%job])
		messageClient(%client, '', "\c33. \c6Clear Record");
		
	// Give in money
	if (%stolen > 0)
		messageClient(%client, '', '\c34. \c6Give in stolen money [\c3%1\c6]', prepareDisplayMoney(%stolen));
		
	%client.activateDatablock = %datablock;
	%client.activateState = 0;
	return true;
}

function PoliceBrickData::parseData(%datablock, %client, %text)
{	
	%client.ThrowLastActive();
	%number = mFloor(%text);
	switch (%client.activateState)
	{
		case 0:
			switch (%number)
			{
				// Check wanted list
				case 1:
					%client.stopActivate();
					if (getWordCount($RP::criminalList) == 0)
					{
						messageClient(%client, '', "\c3There are no wanted criminals.");
						return true;
					}
					messageClient(%client, '', "\c3Wanted criminals");
					for (%i = 0; %i < getWordCount($RP::criminalList); %i++)
					{
						%key = getWord($RP::criminalList, %i);
						%demerits = RPDB.get(%key, "demerits");
						%name = RPDB.get(%key, "name");
						%target = findClientByName(%name);
						if (%demerits >= $RP::pref::demerits::minimum)
						{
							if(isObject(%target))
							{
								messageClient(%client, '', '\c3   %1 - %2 - %3', %name, %demerits, (isObject(%target) ? "\c2Online" : "\c0Offline"));
							}
						}
					}
				// Pardon jailed
				case 2:
					// Can pardon someone jailed
					if (!$RP::job::pardon[%client.RPData.value["jobID"]] && getWordCount($RP::jailList) == 0)
					{
						%client.stopActivate();
						return false;
					}
					messageClient(%client, '', "\c6Type the name of the person you want to pardon.");
					for (%i = 0; %i < getWordCount($RP::jailList); %i++)
					{
						%key = getWord($RP::jailList, %i);
						%target = findClientByName(RPDB.get(%key, "name"));
						if (isObject(%target))
							messageClient(%client, '', "\c3" @ %target.name);
					}
					
					%client.activateState = 3.1;
				// Clear record
				case 3:
					if (!$RP::job::clearRecord[%client.RPData.value["jobID"]])
					{
						%client.stopActivate();
						return false;
					}
					messageClient(%client, '', "\c6Type the name of the person who's criminal record you want to clear.");
					%client.activateState = 4.1;
				// Give in stolen money
				case 4:
					%client.stopActivate();
					%money = %client.RPData.value["money"];
					%stolen = %client.RPData.value["stolenMoney"];
					if (%stolen == 0)
						return false;
					
					%give = (%money < %stolen) ? %money : %stolen;
					
					%client.RPData.value["money"] -= %give;
					%stolen -= %give;
					%client.RPData.value["stolenMoney"] = %stolen;
					
					if (%stolen > 0)
						messageClient(%client, '', '\c6You have returned \c3%1\c6, but still have \c3%2\c6 left.', %give, %stolen);
					else
					{
						messageClient(%client, '', '\c6You have returned \c3all\c6 of the stolen money.');
						%client.returnMoney = 0;
					}
					%client.RPData.value["LawExp"] += 1;
				// Invalid
				default:
					%client.stopActivate();
					return false;
			}
		// Pardon jailed
		case 3.1:
			%client.stopActivate();
			%target = findClientByName(%text);
			// Player does not exist
			if (!isObject(%target))
			{
				messageClient(%client, '', "\c6User is not online.");
				return true;
			}
			%key = %target.getSaveKey();
			if (%target.RPData.value["jail"] <= 0)
			{
				messageClient(%client, '', '\c6Invalid target (\c3%1\c6).', %text);
				RemoveFromJailList(%key);
				return true;
			}
			// Pay the pardon
			if (RPModExist("Money"))
			{
				%money = $RP::pref::demerits::pardonMultiplier * %target.RPData.value["jail"];
				if (%client.RPData.value["money"] < %money)
				{
					messageClient(%client, '', '\c6You need \c3%1\c6 to pardon \c3%2\c6.', prepareDisplayMoney(getMetricPrefix(%money)), %target.name);
					return true;
				}
				%payed = " for \c3" @ prepareDisplayMoney(getMetricPrefix(%money));
				// Client
				%client.RPData.value["money"] -= %money;
			}
			// Target
			%target.RPData.value["jail"] = 0;
			// Change job
			if (RPModExist("Jobs"))
				RPDB.set(%key, "jobID", 1);
			
			RemoveFromJailList(%key);
			commandToClient(%client, 'centerPrint', "\c6You have pardoned \c3" @ %target.name @ "\c6" @ %payed @ "\c6.", 2);
			commandToClient(%target, 'centerPrint', "\c6You have been pardoned by \c3" @ %client.name @ "\c6.", 2);
			
			// Respawn player
			if (isObject(%target.player))
				%target.player.delete();
			%target.spawnPlayer();
			return true;
		// Clear Record
		case 4.1:
			%client.stopActivate();
			%target = findClientByName(%text);
			// Player does not exist
			if (!isObject(%target))
			{
				messageClient(%client, '', "\c6User is not online.");
				return true;
			}
			%key = %target.getSaveKey();
			// Nothing to clear
			if (%target.RPData.value["jail"] < 0 && %target.RPData.value["demerits"] <= 0)
			{
				messageClient(%client, '', '\c6Invalid target (\c3%1\c6).', %text);
				return true;
			}
			// Cannot release
			if (%target.RPData.value["jail"] > 10)
			{
				messageClient(%client, '', "\c6You cannot pardon a person who has done that mcuh damage to the community.");
				return true;
			}
			
			if (RPModExist("Money"))
			{
				// Not enough money
				if (%client.RPData.value["money"] < $RP::pref::demerits::clearRecords)
				{
					messageClient(%client, '', '\c6You need \c3%1\c6 to clear \c3%2\c6\'s record.', prepareDisplayMoney(getMetricPrefix($RP::pref::demerits::clearRecords)), %target.name);
					return true;
				}
				%payed = " for \c3" @ prepareDisplayMoney(getMetricPrefix($RP::pref::demerits::clearRecords));
				// Client
				%client.RPData.value["money"] -= $RP::pref::demerits::clearRecords;
			}
			// Relase if in jail
			if (%target.RPData.value["jail"] > 0)
				%jailed = true;
			if (%target.RPData.value["demerits"] > 0)
				RemoveFromCriminalList(%key);
			
			// Target
			%target.RPData.value["jail"] = -1;
			%target.RPData.value["demerits"] = 0;
			
			commandToClient(%client, 'centerPrint', "\c6You have cleared \c3" @ %target.name @ "\c6's record" @ %payed @ "\c6.", 2);
			commandToClient(%target, 'centerPrint', "\c6Your record has been cleared by \c3" @ %client.name @ "\c6.", 2);
			
			if (%jailed)
			{
				// Change job
				if (RPModExist("Jobs"))
					RPDB.set(%key, "jobID", 1);
				RemoveFromJailList(%key);
				// Respawn player
				if (isObject(%target.player))
					%target.player.delete();
				%target.spawnPlayer();
			}
		// Invalid
		default:
			%client.stopActivate();
			return false;
	}
	return true;
}
