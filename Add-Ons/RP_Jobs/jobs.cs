// --------------------------
// RP Jobs :: Jobs
// --------------------------

// Load jobs
function LoadJobs()
{
	$RP::jobs = 0;
	deleteVariables("$RP::job::*");
	
	%newFile = "Add-Ons/RP_Jobs/data/jobs.dat";
	%file = "config/server/RP_Core/prefs/jobs.dat";
	
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
			echo("RP_Jobs: Updates old jobfile.");
			GameTransferFile(%newFile, %file);
		}
	}
	
	%stream = new fileObject();
	%stream.openForRead(%file);
	
	// Load jobs
	while (!%stream.isEOF())
	{
		%line = trim(%stream.readLine());
		// Invalid
		if (%line $= "" || getWordCount(%line) < 2)
			continue;
		
		// Version line
		if (getSubStr(%line, 0, 1) $= "v" && atoi(getSubStr(%line, 1, strlen(%line) - 1) > 0))
			continue;
		// Comment line
		if (getSubStr(%line, 0, 2) $= "//")
			continue;
		
		%first = firstWord(%line);
		%rest = restWords(%line);
		
		// New job
		if (%first $= "ID")
		{
			%ID = getWord(%line, 1);
			// Multiple ID's
			if ($RP::job::indexID[%ID])
				continue;
			$RP::jobs++;
			$RP::job::listID[$RP::jobs] = %ID;
			$RP::job::indexID[%ID] = $RP::jobs;
		}
		// Boolean values
		// Usage: Add the name of the value, and it will become true, else it's allways false
		else if (%first $= "boolean")
		{
			%temp = %rest;
			%count = getWordCount(%temp);
			for (%i = 0; %i < %count; %i++)
				eval("$RP::job::" @ getWord(%temp, %i) @ "[" @ $RP::job::listID[$RP::jobs] @ "] = 1;");
		}
		// Preference
		else
		{
			eval("$RP::job::" @ %first @ "[" @ $RP::job::listID[$RP::jobs] @ "] = \"" @ %rest @ "\";");
			// Index name
			if (%first $= "name")
				$RP::job::indexName[%rest] = $RP::jobs;
		}
	}
	
	%stream.close();
	%stream.delete();
}

// Create special spawns
function createSpawns()
{
	if (isObject(GamePersonalSpawnBrick))
		return;
	// Personal spawn
	datablock fxDTSBrickData(GamePersonalSpawnBrick : brickSpawnPointdata)
	{
		category = "CRPG";
		subCategory = "Spawns";
		uiName = "Personal Spawn";
		specialBrickType = "";
		isSpawnBrick = 1;
		adminOnly = 0;
		spawnData = "personalSpawn";
	};
	
	// Jail spawn
	if (RPModExist("Crime"))
	{
		datablock fxDTSBrickData(GameCrimeSpawnBrick : brickSpawnPointdata)
		{
			category = "CRPG";
			subCategory = "Spawns";
			uiName = "Jail Spawn";
			specialBrickType = "";
			isSpawnBrick = 1;
			adminOnly = 1;
			spawnData = "jailSpawn";
		};
	}
	
	// Add spawn points
	for (%i = 1; %i <= $RP::jobs; %i++)
	{
		%ID = $RP::job::listID[%i];
		//if (!RPModsExists($RP::job::requireMod[%ID]))
		//if (RPModsExists($RP::job::requireMod[%ID]))
			//continue;
		
		// Job already exist
		if (isObject("GameJob" @ %ID @ "SpawnBrick"))
			continue;
		
		datablock fxDTSBrickData(GameSpawnBrick : brickSpawnPointData)
		{
			category = "CRPG";
			subCategory = "Spawns";
			uiName = $RP::job::name[%ID] @ " Spawn";
			specialBrickType = "";
			isSpawnBrick = 1;
			adminOnly = 1;
			spawnData = "jobSpawn " @ %ID;
		};
		// Rename to make unique
		GameSpawnBrick.setName("GameJob" @ %ID @ "SpawnBrick");
	}
}

// Find job ID by name
function FindJobIDByName(%name)
{
	%i = $RP::job::indexName[%name];
	%ID = $RP::job::listID[%i];
	// Does not exist
	if (%ID $= "")
		return 0;
	
	// Requires a mod to use
	if (!RPModsExists($RP::job::requireMod[%ID]))
		return 0;
	return %ID;
}

// List job for events
function EventJobList()
{
	%str = "list";
	for (%i = 1; %i <= $RP::jobs; %i++)
	{
		%ID = $RP::job::listID[%i];
		if (RPModsExists($RP::job::requireMod[%ID]))
			%str = %str SPC strReplace($RP::job::name[%ID], " ", "") SPC %ID;
	}
	return %str;
}

// Promotion
function Promotion(%client)
{
	if (!isObject(%client))
		return;
	// Already promoting
	if (isEventPending(%client.promotion))
		return;
	
	%promotions = $RP::job::promotion[%client.RPData.value["jobID"]];
	// Job have a promotion
	if (%promotions $= "")
		return;
	
	%count = getWordCount(%promotions);
	%amount = 0;
	
	// Get jobs
	for (%i = 0; %i < %count; %i++)
	{
		%job = getWord(%promotions, %i);
		%exp = mFloor($RP::job::experience[%job]);
		%careerExp = mFloor($RP::job::careerExperience[%job]);
		// Enough exp
		if (%client.RPData.value["exp"] < %exp)
			continue;
		// Enough exp
		%group = $RP::job::group[%client.RPData.value["jobID"]];
		if(%group $= "FFFFFF")
		{
			%careerExperience = %client.RPData.value["exp"];
		}
		else if(%group $= "999999")
		{
			%careerExperience = %client.RPData.value["LaborExp"];
		}
		else if(%group $= "333333")
		{
			%careerExperience = %client.RPData.value["CrimeExp"];
		}
		else if(%group $= "CCCC00")
		{
			%careerExperience = %client.RPData.value["LawExp"];
		}
		else if(%group $= "0000CC")
		{
			%careerExperience = %client.RPData.value["BusinessExp"];
		}
		else if(%group $= "CC00CC")
		{
			%careerExperience = %client.RPData.value["MedicalExp"];
		}
		else
		{
			%careerExperience = %client.RPData.value["exp"];
		}
			
		if (%careerExperience < %careerExp)
			continue;
		%exp = mSqrt((%client.RPData.value["exp"] - %exp));
		// Limitations
		%exp = mClamp(%exp, 0, 50);
		// And then some random stuff
		if (getRandom(0, 100) < 95 - %exp)
		{
			%list[%amount] = %job;
			%amount++;
		}
	}
	
	// Better luck next time
	if (%amount == 0)
		return;
	
	%ID = %list[getRandom(0, %amount - 1)];
	
	messageClient(%client, '', '\c6You have been offered a promotion to \c3%1\c6.', $RP::job::name[%ID]);
	messageClient(%client, '', "\c6Type \c3yes \c6to accept.");
	%client.promotionID = %ID;
	// Auto decline with time limit
	%client.promotion = schedule($RP::pref::server::autoDeclineTime * 1000, 0, "Promote", %client, "no");
	return;
	
	// Job have a promotion
	if (!(%promotion > 0))
		return;
	// Enough exp
	if (%client.RPData.value["exp"] < mFloor($RP::job::experience[%promotion]))
		return;
	%exp = mSqrt((%client.RPData.value["exp"] - $RP::job::experience[%promotion]));
	// Limitations
	%exp = (%exp > 50) ? 50 : %exp;
	// And then some random stuff
	if (getRandom(0, 100) < 95 - %exp)
		return;
	
	messageClient(%client, '', "\c6You have been requested to promote to \c3" @ $RP::job::name[%promotion] @ "\c6.");
	messageClient(%client, '', "\c6Type \c3yes \c6to accept.");
	%client.promotionID = %ID;
	// Auto decline with time limit
	%client.promotion = schedule($RP::pref::server::autoDeclineTime * 1000, 0, "Promote", %client, "no");
}

// Promote
function Promote(%client, %answer)
{
	if (!isObject(%client))
		return;
	if (isEventPending(%client.promotion))
		Cancel(%client.promotion);
	
	%ID = %client.promotionID;
	%client.promotionID = "";
	if (%ID $= "")
		return;
	
	// Declined
	if (strLwr(%answer) !$= "yes")
	{
		messageClient(%client, '', "\c6You declined the offer.");
		return;
	}
	
	%client.RPData.value["jobID"] = %ID;
	
	%player = %client.player;
	if (isObject(%player))
	{
		// Choose datablock
		%datablock = $RP::job::datablock[%ID];
		if (isObject(%datablock))
			%client.RP_PlayerDatablock = %datablock;
		%player.changeDataBlock(%datablock, %client);
		// Recolor for group
		%group = $RP::job::group[%ID];
		if (%group !$= "")
			%player.setShapeNameColor(HexToRGBA(%group));
		// Reset tools
		%client.RPData.value["tools"] = "";
		%player.RP_giveDefaultEquipment();
	}
	messageClient(%client, '', '\c6You accepted the offer, you have been promoted to \c3%1\c6.', $RP::job::name[%ID]);
	%client.RPData.value["exp"] += $RP::pref::user::promoteExp;
}
