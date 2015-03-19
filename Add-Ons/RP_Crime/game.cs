// --------------------------
// RP Crime :: Game
// --------------------------

if (isPackage(RP_Crime_Game_Boot))
	deactivatePackage(RP_Crime_Game_Boot);

package RP_Crime_Game_Boot
{
	// Booting game
	function BootRP()
	{
		if ($RP::activated)
			return;
		Parent::BootRP();
		
		RPDB.addValue("demerits", 0);
		RPDB.addValue("jail", -1); // Negative means a clean record
		RPDB.addValue("bounty", 0);
		RPDB.addValue("stolenMoney", 0);
		RPDB.saveDefs();
		
		RP_AddStat("Demerits", "demerits", 0);
		RP_AddStat("Jail", "jail", 0);
		RP_AddStat("Bounty", "bounty", 0);
		
		// Find criminals, jailed and bountied
		for (%i = 1; %i <= RPDB.countKeys; %i++)
		{
			%key = RPDB.listKey[%i];
			%demerits = RPDB.get(%key, "demerits");
			// Criminal
			if (%demerits > 0)
				AddToCriminalList(%key);
			// Jailed
			if (RPDB.get(%key, "jail") > 0)
				AddToJailList(%key);
			// Check for dirty reputation
			else if (%demerits >= $RP::pref::demerits::minimum)
				RPDB.set(%key, "jail", 0);
			// Bountied
			if (RPDB.get(%key, "bounty") > 0)
				AddToBountyList(%key);
		}
	}
};
activatePackage(RP_Crime_Game_Boot);

if (isPackage(RP_Crime_Game))
	deactivatePackage(RP_Crime_Game);

package RP_Crime_Game
{	
	// RP Tick
	function RPTick()
	{
		// Criminal activities
		%TCO = $RP::pref::timeCriminalOffset; // Time Criminal Offset
		%TCA = $RP::pref::timeCriminalAmount; // Time Criminal Amount
		if (%TCA <= 0)
			%TCA = 1;
		%TL = mCeil(24 / %TCA); // Time limit
		%time = (%TCO / %TL) + ($RP::pref::timeHour / %TL);
		
		if (%time == mFloor(%time))
		{
			// Criminals
			for (%i = 0; %i < getWordCount($RP::criminalList); %i++)
				if (CheckCriminal(getWord($RP::criminalList, %i)))
					%i--;
			
			//Prisoners
			// for (%i = 0; %i < getWordCount($RP::jailList); %i++)
				// if (CheckPrisioner(getWord($RP::jailList, %i)))
					// %i--;
		}
		
		//Prison Time - Works while inmate is Offline
		for (%i = 0; %i < getWordCount($RP::jailList); %i++)
		{
			%jail = RPDB.get(getWord($RP::jailList, %i), "jail");
			%inmate = findClientByName(RPDB.get(getWord($RP::jailList, %i), "name"));
			
			if (%jail >= 1)
			{
				%jail--;
				RPDB.set(getWord($RP::jailList, %i), "jail", %jail);
				
				// Is online
				if (isObject(%inmate))
				{
					if(%jail > 1)
					%hourSuffix = "s";
						
					messageClient(%inmate, '', '\c6 - You have \c3%1\c6 hour%2 left in prison.', %inmate.RPData.value["jail"], %hourSuffix);
					%inmate.displayInfo();
				}
			}
			else
			{
				RPDB.set(getWord($RP::jailList, %i), "tools", "");
				RemoveFromJailList(getWord($RP::jailList, %i));
				// Change job
				if (RPModExist("Jobs"))
					RPDB.set(getWord($RP::jailList, %i), "jobID", 2); //Set Inmate's job to Civilian
	
				// Is online
				if (isObject(%inmate))
				{
					// Angus ghost
					if (isObject(%inmate.player))
						%inmate.player.delete();
					%inmate.spawnPlayer();
					messageClient(%inmate, '', "\c6 - You have been let out of prison.");
				}
				return true;
			}
		}
			
		// if(getWord(%so.valueJailData, 1))
		// {
			// if(%ticks = getWord(%so.valueJailData, 1) > 1)
			// {
				// %daysLeft = (getWord(%so.valueJailData, 1) - 1);
			
				// if(%daysLeft > 1)
					// %daySuffix = "s";
				
				// messageClient(%client, '', '\c6 - You have \c3%1\c6 day%2 left in Prison.', %daysLeft, %daySuffix);
			// }
		// }
		return	Parent::RPTick();
	}
	
	// RP Tick ID
	function CheckTickID(%ID)
	{
		%name = RPDB.get(%ID, "name");
		%client = findClientByName(%name);
		if (!isObject(%client))
			return false;
		
		%stolen = %client.RPData.value["stolenMoney"];
		// Note the user
		if (%stolen > 0)
		{
			if(%client.RPData.value["jail"] < 1)
			{
				%client.returnMoney++;
				if (%client.returnMoney >= $RP::pref::user::timeReturnMoney)
				{
					if ($RP::job::group[%client.RPData.value["jobID"]] $= "333333")
					{
						messageClient(%client, '', "\c6 - You held on to the money long enough to lose your trace. The money is no longer dirty.");
						CommitCrime(%client, "Theft", %stolen / 10);
						%client.RPData.value["exp"] += %stolen /10;
						%client.RPData.value["stolenMoney"] = 0;
						%client.returnMoney = 0;
					}
					else
					{
						messageClient(%client, '', "\c6 - You did not return the money to PD, and therefore you are a criminal.");
						%client.RPData.value["stolenMoney"] = 0;
						CommitCrime(%client, "Theft", %stolen / 10);
					}
				}
				else
				{
					messageClient(%client, '', '\c6 - You currently have \c3%1\c6 hours left to return money to the PD.', 24 - %client.returnMoney);
				}
			}
		}
		
		parent::CheckTickID(%ID);
	}
	
	// Create special spawns
	function CreateSpawns()
	{
		if (isObject(GameJailSpawnBrick))
			return Parent::CreateSpawns();
		// Jail spawn
		datablock fxDTSBrickData(GameJailSpawnBrick : brickSpawnPointdata)
		{
			category = "RP Admin";
			subCategory = "Spawns";
			uiName = "Jail Spawn";
			specialBrickType = "";
			isSpawnBrick = 1;
			adminOnly = 1;
			spawnData = "jailSpawn";
		};
		
		return Parent::CreateSpawns();
	}
};
RPRegPackage(RP_Crime_Game);

// You have commit a crime
function CommitCrime(%client, %text, %dems)
{
	%disp = $RP::pref::user::displayCommitCrime;
	
	if (%disp)
		commandToClient(%client, 'centerPrint', "\c6You have committed a crime [\c3" @ %text @ "\c6]", 3);
	
	%oldDems = %client.RPData.value["demerits"];
	// Criminal list
	if (%oldDems <= 0)
		AddToCriminalList(%client.getSaveKey());
	%client.RPData.value["demerits"] += mFloor(%dems);
	%client.RPData.value["CrimeExp"] += 1;
	
	// Display that player got a dirty record
	if (%client.RPData.value["demerits"] >= $RP::pref::demerits::minimum && %oldDems < $RP::pref::demerits::minimum && %client.RPData.value["jail"] < 0)
	{
		%client.RPData.value["jail"] = 0;
		if (RPModExist("Jobs") && $RP::job::clean[%client.RPData.value["jobID"]])
		{
			%client.RPData.value["jobID"] = 1;
			// Give default equipments
			%client.RPData.value["tools"] = "";
			// Reset user
			%client.FinishRPSpawning(true);
			%and = " and therefore being demoted to \c3" @ $RP::job::name[1] @ "\c6";
		}
		if (%disp)
			messageClient(%client, '', '\c6You have made your clean record dirty%1.', %and);
	}
	
	// Display that player cannot use public services
	if (%client.RPData.value["demerits"] > $RP::pref::demerits::maximum && %oldDems < $RP::pref::demerits::maximum && %disp)
		messageClient(%client, '', "\c6Now you cannot be served at public services.");
}

// Check criminals
function CheckCriminal(%key)
{
	if ($RP::pref::demerits::StatueOfLimitations <= 0)
		return false;
	
	%demerits = RPDB.get(%key, "demerits");
	%lower = (%demerits >= $RP::pref::demerits::StatueOfLimitations) ? $RP::pref::demerits::StatueOfLimitations : %demerits;
	%demerits -= %lower;
	RPDB.set(%key, "demerits", %demerits);
	
	%client = findClientByName(RPDB.get(%key, "name"));
	// Is online
	if (isObject(%client))
	{
		messageClient(%client, '', '\c6 - Due to the Statute of Limitations, your demerits have been reduced to \c3%1\c6.', %demerits);
		%client.displayInfo();
	}
	
	if (%demerits > 0)
		return false;
	
	RemoveFromCriminalList(%key);
	return true;
}

// Check prisioners
function CheckPrisioner(%key)
{
	%jail = RPDB.get(%key, "jail");
	%jail--;
	RPDB.set(%key, "jail", %jail);
	
	%client = findClientByName(RPDB.get(%key, "name"));
	
	if (%jail > 0)
	{
		// Is online
		if (isObject(%client))
		{
			messageClient(%client, '', '\c6 - You have \c3%1\c6 hours left.', (%client.RPData.value["jail"]));
			%client.displayInfo();
		}
		return false;
	}
	
	RPDB.set(%key, "tools", "");
	RemoveFromJailList(%key);
	// Change job
	if (RPModExist("Jobs"))
		RPDB.set(%key, "jobID", 1);
	
	// Is online
	if (isObject(%client))
	{
		// Angus ghost
		if (isObject(%client.player))
			%client.player.delete();
		%client.spawnPlayer();
		messageClient(%client, '', "\c6 - You have been let out of prison.");
	}
	return true;
}

// Add to criminal list
function AddToCriminalList(%ID)
{
	$RP::criminalList = addToList($RP::criminalList, %ID);
}

// Add to jail list
function AddToJailList(%ID)
{
	$RP::jailList = addToList($RP::jailList, %ID);
}

// Add to bounty list
function AddToBountyList(%ID)
{
	$RP::bountyList = addToList($RP::bountyList, %ID);
}

// Remove from criminal list
function RemoveFromCriminalList(%ID)
{
	$RP::criminalList = removeFromList($RP::criminalList, %ID);
}

// Remove from criminal list
function RemoveFromJailList(%ID)
{
	$RP::jailList = removeFromList($RP::jailList, %ID);
}

// Remove from bounty list
function RemoveFromBountyList(%ID)
{
	$RP::bountyList = removeFromList($RP::bountyList, %ID);
}
