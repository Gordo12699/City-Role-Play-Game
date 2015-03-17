// --------------------------
// RP Crime :: Crime
// --------------------------

// Open doors
function fxDTSBrick::onBashDoor(%brick, %client)
{
	if (!isObject(%brick.shape))
		return;
	
	if (!%brick.BrutalOpenDoor(%client))
		commandToClient(%client, 'centerPrint', "\c6The door is batonproof.", 2);
}

// Brutally opens a door
function fxDTSBrick::BrutalOpenDoor(%brick, %client)
{
	// Is door
	if (!isObject(%brick.shape))
		return 1;
	
	// Cannot open admin doors
	if (getBrickGroupFromObject(%brick).client.isAdmin)
		return 0;
	
	// JVS hack
	if (%brick.isContentBlocked(%client, 1, "CW") <= 0)
	{
		%brick.contentUse(%client, 0, "START", "CW");
		%brick.schedule(3000, "contentUse", %client, 0, "STOP", "CW");
	}
	else if (%brick.isContentBlocked(%client, 1, "CCW") <= 0)
	{
		%brick.contentUse(%client, 0, "START", "CCW");
		%brick.schedule(3000, "contentUse", %client, 1, "STOP", "CCW");
	}
	else
	{
		return 0;
	}
	
	return 1;
}

// Jail criminals
function Player::onUseBaton(%player, %client)
{
	if(%client.RPData.value["demerits"] > 0)
	{
		messageClient(%client, '', "You can not jail people while you have demerits yourself!");
		return;
	}
	
	%target = %player.client;
	if (!isObject(%target))
		return;
	
	%targetID = %target.getSaveKey();
	%demerits = %target.RPData.value["demerits"];
	%jail = %target.RPData.value["jail"];
	
	// Is not wanted
	if (%demerits < $RP::pref::demerits::minimum)
	{
		if (%client.TakeDirtyMoney(%target, true))
			return;
		commandToClient(%client, 'centerPrint', "\c3Stop hitting innocent people.", 2);
		return;
	}
	
	if(%target.RPData.value["totaldrugs"] > 0)
	{
		CommitCrime(%target, "Possession of Narcotics", $RP::pref::crime::havingDrugs * %target.RPData.value["totaldrugs"]);
	}
	
	if (RPModExist("Hunger"))
	{
		if(%target.RPData.value["hunger"] < 3)
			%target.RPData.value["hunger"] = 3;
	}
	
	// Calculate time
	%dems = %demerits;
	%time = mCeil(%dems / $RP::pref::demerits::amountDemeritsTick);
	%time = (%time > $RP::pref::jail::maxJailTime) ? $RP::pref::jail::maxJailTime : %time;
	%exp = mFloor(%dems / $RP::pref::demerits::jailExp);
	
	%client.RPData.value["exp"] += %exp;	// Give experience
	%client.RPData.value["LawExp"] += %exp / 2;
	%target.RPData.value["jail"] += %time;	// Add jail time
	%target.RPData.value["demerits"] = 0;	// Remove dems
	%target.RPData.value["Marijuana"] = 0; //Remove Drugs
	%target.RPData.value["Opium"] = 0;
	%target.RPData.value["totaldrugs"] = 0;
	
	if (RPModExist("Money"))
	{
		// Profit
		%client.RPData.value["money"] += %dems;	// Give money
		%money = "\c3" @ prepareDisplayMoney(%dems) @ "\c6 and ";
		
		// Dirty money
		%client.TakeDirtyMoney(%target, false);
	}
	
	messageAll('', '\c3%1\c6 has jailed \c3%2\c6 for \c3%3\c6 hours.', %client.name, %target.name, %time);
	commandToClient(%client, 'centerPrint', "\c6You got " @ %money @ "\c3" @ %exp @ "\c6 experience, and " @ %exp / 2 @ " career experience.", 3);
	messageClient(%target, '', "\c6The police took any drugs you may have been carrying.");
	
	// Change job
	if (RPModExist("Jobs"))
		%target.RPData.value["jobID"] = $RP::job::listID[$RP::job::indexName[$RP::pref::jail::job]];
	
	%target.displayInfo();
	// Remove ghostbrick
	if (isObject(%player.tempBrick))
		%player.tempBrick.delete();
	
	// Respawn player
	if (isObject(%target.player))
		%target.player.delete();
	%target.schedule(1, spawnPlayer);
	
	// Jail player
	RemoveFromCriminalList(%targetID);
	AddToJailList(%targetID);
}

// Open safe with baton
function fxDTSBrick::onOpenSafe(%brick, %client)
{
	// Requires money
	if (!RPModExist("Money"))
		return;
	
	if (!%brick.isSafe)
		return;
	
	%ID = %client.getSaveKey();
	
	%brickGroup = getBrickGroupFromObject(%brick);
	%target = %brickGroup.client;
	%OwnerID = (!isObject(%target)) ? ($Server::serverType $= "Internet" ? %brickGroup.bl_id : %brickGroup.name) : %target.getSaveKey();
	
	// How did we happened to get here?
	if (%ID == %OwnerID)
	{
		messageClient(%client, '', "\c6Somehow, it is your own safe.");
		return;
	}
	
	%stolen = RPDB.get(%OwnerID, "stolenMoney");
	// No stolen money
	if (%stolen == 0)
	{
		messageClient(%client, '', "\c6This safe is clean.");
		%client.RPData.value["LawExp"] += 1;
		return;
	}
	
	// Transfer money
	RPDB.set(%OwnerID, "stolenMoney", 0);
	%client.RPData.value["stolenMoney"] += %stolen;
	%client.RPData.value["money"] += %stolen;
	
	messageClient(%client, '', '\c6You found \c3%1\c6 worth of stolen money.', prepareDisplayMoney(%stolen));
	%client.RPData.value["LawExp"] += 1;
	messageClient(%client, '', '\c6You have \c3%1\c6 hours to return them to the nearest Police Department.', $RP::pref::user::timeReturnMoney);
}

// Picklock
function fxDTSBrick::onPicklock(%brick, %client)
{
	if (!isObject(%brick.shape))
		return;
	
	// Prepare variables
	%experience = %client.RPData.value["exp"];
	%breaking = (99 - (%experience / 20));
	%breaking = (%breaking < 0) ? 0 : %breaking;
	%failed = (50 - (%experience / 20));
	%failed = (%failed < 0) ? 0 : %failed;
	if (getRandom(0, 100) >= %breaking)
	{
		// Succeed
		if (%brick.BrutalOpenDoor(%client))
			CommitCrime(%client, "Breaking and Entering", $RP::pref::crime::BreakingAndEntering);
		// Failed
		else
			CommitCrime(%client, "Activated Alarm", $RP::pref::crime::activateAlarm);
	}
	else
	{
		// Failed
		if (getRandom(0, 100) >= %failed)
			commandToClient(%client, 'centerPrint', "\c6Picklocking failed.", 1);
		// ... miserably
		else
			CommitCrime(%client, "Activated Alarm", $RP::pref::crime::activateAlarm);
	}
}

// Jail Baton
function Player::onJailBaton(%player, %client)
{
	%target = %player.client;
	if (!isObject(%target))
		return;
	
	%targetID = %target.bl_id;
	
	// Is in jail
	if (%target.RPData.value["jail"] <= 0)
	{
		commandToClient(%obj.client, 'centerPrint', "\c3Stop hitting innocent people.", 2);
		return;
	}
	
	// Have done something wrong
	if (%target.RPData.value["demerits"] > 0)
	{
		// Calculate time
		%dems = %target.RPData.value["demerits"];
		%time = mCeil(%dems / $RP::pref::demerits::amountDemeritsTick);
		%time = (%time > $RP::pref::jail::maxJailTime) ? $RP::pref::jail::maxJailTime : %time;
		%exp = mFloor(%dems / $RP::pref::demerits::jailExp);
		
		%client.RPData.value["exp"] += %exp;	// Give experience
		%target.RPData.value["jail"] += %time;	// Add jail time
		%target.RPData.value["demerits"] = 0;	// Remove dems
		
		// Remove from criminal list
		RemoveFromCriminalList(%targetID);
		
		if (RPModExist("Money"))
		{
			%client.RPData.value["money"] += %dems;	// Give money
			%money = "\c3" @ prepareDisplayMoney(%dems) @ "\c6 and ";
			
			%client.TakeDirtyMoney(%target, false);
		}
		
		messageAll('', '\c3%1\c6 has put \c3%2\c6 back in jail for \c3%3\c6 hours.', %client.name, %target.name, %target.RPData.value["jail"] * 8);
		commandToClient(%client, 'centerPrint', "\c6You got " @ %money @ "\c3" @ %exp @ "\c6 experience.", 3);
		
		%target.displayInfo();
	}
	
	// Remove ghostbrick
	if (isObject(%player.tempBrick))
		%player.tempBrick.delete();
	
	// Respawn player
	if (isObject(%target.player))
		%target.player.delete();
	%target.schedule(1, spawnPlayer);
}
